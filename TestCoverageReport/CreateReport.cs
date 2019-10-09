using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestCoverageReport
{
    public static class CreateReport
    {
        public static void Execute(string[] args)
        {
            CombinedReport reports = new CombinedReport();
            reports.ReportDate = DateTime.Now;
            reports.Comparisons = new List<ComparisonReport>();

            for (int i = 1; i < args.Length; i++)
            {
                try
                {
                    ComparisonReport report = JsonConvert.DeserializeObject<ComparisonReport>(File.ReadAllText(args[i]));

                    reports.Comparisons.Add(report);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"Error while collecting reports: {e}");
                }
            }

            using (Stream textStream = File.OpenWrite("report.txt"))
            using (StreamWriter textWriter = new StreamWriter(textStream))
            {
                TextRenderer textRenderer = new TextRenderer(textWriter);
                Render(textRenderer, reports);
            }

            using (Stream htmlStream = File.OpenWrite("report.htm"))
            using (StreamWriter htmlWriter = new StreamWriter(htmlStream))
            {
                HtmlRenderer htmlRenderer = new HtmlRenderer(htmlWriter);
                Render(htmlRenderer, reports);
            }

            using (Stream textStream = File.OpenWrite("commit-report.txt"))
            using (StreamWriter textWriter = new StreamWriter(textStream))
            {
                TextRenderer textRenderer = new TextRenderer(textWriter);
                RenderCommitView(textRenderer, reports);
            }

            using (Stream htmlStream = File.OpenWrite("commit-report.htm"))
            using (StreamWriter htmlWriter = new StreamWriter(htmlStream))
            {
                HtmlRenderer htmlRenderer = new HtmlRenderer(htmlWriter);
                RenderCommitView(htmlRenderer, reports);
            }
        }

        public static void Render(IRenderer renderer, CombinedReport reports)
        {
            renderer.WriteHeader();

            Dictionary<string, string> branchDict = new Dictionary<string, string>();
            foreach (ComparisonReport report in reports.Comparisons)
            {
                branchDict[report.TargetBranch] = report.TargetCommitId;
                branchDict[report.BaseBranch] = report.BaseCommitId;
            }

            renderer.WriteBranches(branchDict);

            foreach (ComparisonReport report in reports.Comparisons)
            {
                renderer.WriteDiffSectionHeader(report.BaseBranch, report.TargetBranch, report.BaseCommitId, report.TargetCommitId);

                foreach (string file in report.Files.Keys)
                {
                    renderer.WriteFileSectionHeader(file);

                    foreach (FileReportLine line in report.Files[file])
                    {
                        line.TargetCommitId = report.TargetCommitId;
                        renderer.WriteFileLine(line);
                    }

                    renderer.WriteFileSectionFooter();
                }

                renderer.WriteCommitSectionHeader();
                foreach (CommitInfo info in report.Commits)
                {
                    renderer.WriteCommitLine(info);
                }
                renderer.WriteCommitSectionFooter();
            }

            renderer.WriteFooter();
        }

        public static void RenderCommitView(IRenderer renderer, CombinedReport reports)
        {
            renderer.WriteHeader();

            Dictionary<string, string> branchDict = new Dictionary<string, string>();
            foreach (ComparisonReport report in reports.Comparisons)
            {
                branchDict[report.TargetBranch] = report.TargetCommitId;
                branchDict[report.BaseBranch] = report.BaseCommitId;
            }

            renderer.WriteBranches(branchDict);

            foreach (ComparisonReport report in reports.Comparisons)
            {
                renderer.WriteDiffSectionHeader(report.BaseBranch, report.TargetBranch, report.BaseCommitId, report.TargetCommitId);

                renderer.WriteCommitSectionHeader();

                var commitToLines = new Dictionary<string, Dictionary<string, List<FileReportLine>>>();

                foreach (string file in report.Files.Keys)
                {
                    List<FileReportLine> lineList = report.Files[file];

                    foreach (FileReportLine line in lineList)
                    {
                        Dictionary<string, List<FileReportLine>> commitDict;
                        List<FileReportLine> commitList;

                        if (commitToLines.TryGetValue(line.CommitId, out commitDict))
                        {
                            if (commitDict.TryGetValue(file, out commitList))
                            {
                                commitList.Add(line);
                            }
                            else
                            {
                                commitDict[file] = new List<FileReportLine>() { line };
                            }
                        }
                        else
                        {
                            commitToLines[line.CommitId] = new Dictionary<string, List<FileReportLine>>();
                            commitToLines[line.CommitId][file] = new List<FileReportLine>() { line };
                        }
                    }
                }

                foreach (CommitInfo info in report.Commits)
                {
                    if (commitToLines.TryGetValue(info.CommitId, out Dictionary<string, List<FileReportLine>> files))
                    {
                        renderer.WriteCommitLine(info);

                        foreach (string file in files.Keys)
                        {
                            List<FileReportLine> lines = files[file];

                            renderer.WriteFileSectionHeader(file);
                            foreach (FileReportLine line in lines)
                            {
                                renderer.WriteFileLine(line);
                            }
                            renderer.WriteFileSectionFooter();
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine($"Did not find commit id '{info.CommitId}'");
                    }
                }

                renderer.WriteCommitSectionFooter();
            }

            renderer.WriteFooter();
        }
    }
}
