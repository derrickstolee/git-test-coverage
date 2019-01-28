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
        }

        public static void Render(IRenderer renderer, CombinedReport reports)
        {
            renderer.WriteHeader();

            foreach (ComparisonReport report in reports.Comparisons)
            {
                renderer.WriteDiffSectionHeader(report.BaseBranch, report.TargetBranch, report.BaseCommitId, report.TargetCommitId);
                
                foreach (string file in report.Files.Keys)
                {
                    renderer.WriteFileSectionHeader(file);

                    foreach (FileReportLine line in report.Files[file])
                    {
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
    }
}
