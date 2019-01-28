using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace TestCoverageReport
{
    public static class Collect
    {
        public static void Execute(string[] args)
        {
            ComparisonReport report = new ComparisonReport();

            report.TargetBranch = args[1];
            report.TargetCommitId = args[2];
            report.BaseBranch = args[3];
            report.BaseCommitId = args[4];

            report.Files = new Dictionary<string, List<FileReportLine>>();
            report.Commits = new List<CommitInfo>();

            HashSet<string> commits = new HashSet<string>();

            foreach (string file in ChangedFiles(report.BaseCommitId, report.TargetCommitId))
            {
                report.Files[file] = GetUncoveredLines(file, report.BaseCommitId, report.TargetCommitId);

                foreach (FileReportLine line in report.Files[file])
                {
                    commits.Add(line.CommitId);
                }
            }

            foreach (string commitId in commits)
            {
                report.Commits.Add(GetCommitInfo(commitId));
            }

            report.Commits.Sort();

            File.WriteAllText("report.json", JsonConvert.SerializeObject(report));
        }

        public static List<string> ChangedFiles(string from, string to)
        {
            string output = RunGitProcess($"diff --name-only {from} {to} -- \\*.c");

            return new List<string>(output.Split('\n', StringSplitOptions.RemoveEmptyEntries));
        }

        public static List<FileReportLine> GetUncoveredLines(string file, string from, string to)
        {
            string hashFile = file.Replace('/', '#');
            List<FileReportLine> reportLines = new List<FileReportLine>(); ;

            if (!File.Exists(hashFile))
            {
                return reportLines;
            }

            string diff = RunGitProcess($"diff {from} {to} -- {file}");

            int curLine = 0;
            List<int> newLines = new List<int>();
            foreach (string line in diff.Split('\n'))
            {
                if (line.StartsWith("@@"))
                {
                    int skip = "@@ -".Length;
                    curLine = int.Parse(line.Substring(skip, line.IndexOf(',') - skip));
                    continue;
                }

                if (line.StartsWith("-"))
                {
                    continue;
                }

                if (line.StartsWith("+"))
                {
                    newLines.Add(curLine);
                }

                curLine++;
            }

            if (newLines.Count == 0)
            {
                return reportLines;
            }

            string[] gcovOutput = File.ReadAllLines(hashFile);

            HashSet<int> uncoveredLines = GetUncoveredLines(gcovOutput);

            if (uncoveredLines.Count == 0)
            {
                return reportLines;
            }

            string blameOutput = RunGitProcess($"-c core.abbrev=40 blame -s {to} -- {file}");

            int lineCount = 0;
            Regex regex = new Regex(@"[0-9a-f]+ [0-9]+) .*", RegexOptions.IgnoreCase);
            foreach (string blameLine in blameOutput.Split('\n'))
            {
                lineCount++;

                if (uncoveredLines.Contains(lineCount))
                {
                    Match match = regex.Match(blameLine);

                    if (!match.Success)
                    {
                        Console.Error.WriteLine($"Line doesn't match regex: {blameLine}");
                        break;
                    }

                    FileReportLine line = new FileReportLine
                    {
                        FileName = file,
                        CommitId = match.Groups[0].Value,
                        LineNumber = lineCount,
                        LineContents = match.Groups[2].Value
                    };
                }
            }

            return reportLines;
        }

        public static HashSet<int> GetUncoveredLines(string[] gcovOutput)
        {
            HashSet<int> uncoveredLines = new HashSet<int>();

            foreach (string gcovLine in gcovOutput)
            {
                if (gcovLine.Contains("####:"))
                {
                    int start = gcovLine.IndexOf("####:") + 5;
                    int lineNumber = int.Parse(gcovLine.Substring(start, gcovLine.IndexOf(':', start) - start));
                    uncoveredLines.Add(lineNumber);
                }
            }

            return uncoveredLines;
        }

        public static List<FileReportLine> GetBlameLines(string file, HashSet<int> uncoveredLines, string blameOutput)
        {
            List<FileReportLine> lines = new List<FileReportLine>();

            int lineCount = 0;
            foreach (string blameLine in blameOutput.Split('\n'))
            {
                lineCount++;

                if (uncoveredLines.Contains(lineCount))
                {
                    string lineNumberStr = $"{lineCount}) ";
                    lines.Add(new FileReportLine
                    {
                        FileName = file,
                        CommitId = blameLine.Substring(0, 40),
                        LineNumber = lineCount,
                        LineContents = blameLine.Substring(blameLine.IndexOf(lineNumberStr) + lineNumberStr.Length).TrimEnd()
                    });
                }
            }

            return lines;
        }

        public static CommitInfo GetCommitInfo(string commitId)
        {
            string info = RunGitProcess("log --pretty=format:'%an|%m'");

            string[] split = info.Split('|', 2);

            return new CommitInfo
            {
                Author = split[0],
                CommitId = commitId,
                Message = split[1]
            };
        }

        private static string RunGitProcess(string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = arguments;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "git";

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            string result = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return result;
        }
    }
}
