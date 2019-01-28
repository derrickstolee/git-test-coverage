using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestCoverageReport
{
    public static class Collect
    {
        public static void Execute(string[] args)
        {
            string ignoredDirectory = null;
            ComparisonReport report = new ComparisonReport();

            report.TargetBranch = args[1];
            report.TargetCommitId = args[2];
            report.BaseBranch = args[3];
            report.BaseCommitId = args[4];

            if (args.Length > 5)
            {
                ignoredDirectory = args[5];
            }

            report.Files = new Dictionary<string, List<FileReportLine>>();
            report.Commits = new List<CommitInfo>();

            HashSet<string> commits = new HashSet<string>();

            foreach (string file in ChangedFiles(report.BaseCommitId, report.TargetCommitId))
            {
                List<FileReportLine> lines = GetUncoveredLines(file, report.BaseCommitId, report.TargetCommitId, ignoredDirectory);

                if (lines.Count == 0)
                {
                    continue;
                }

                report.Files[file] = lines;

                foreach (FileReportLine line in lines)
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
            string output = RunGitProcess($"diff --name-only {from} {to} -- *.c");

            return new List<string>(output.Split('\n', StringSplitOptions.RemoveEmptyEntries));
        }

        public static List<FileReportLine> GetUncoveredLines(string file, string from, string to, string ignoredDirectory)
        {
            string hashFile = file.Replace('/', '#') + ".gcov";
            List<FileReportLine> reportLines = new List<FileReportLine>(); ;

            if (!File.Exists(hashFile))
            {
                return reportLines;
            }

            string diff = RunGitProcess($"diff {from} {to} -- {file}");

            List<int> newLines = GetNewLines(diff);

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

            reportLines = GetBlameLines(file,
                                        newLines.Where(num => uncoveredLines.Contains(num)).ToHashSet(),
                                        blameOutput);

            HashSet<string> ignoredLines = GetIgnoredLines(ignoredDirectory, file);
            if (ignoredLines.Count == 0)
            {
                return reportLines;
            }

            foreach (FileReportLine line in reportLines)
            {
                string ignoreLine = $"{line.LineNumber}:{line.LineContents.Trim()}";

                if (ignoredLines.Contains(ignoreLine))
                {
                    line.Ignored = true;
                }
            }

            return reportLines;
        }

        public static List<int> GetNewLines(string diff)
        {
            List<int> newLines = new List<int>();
            int curLine = 0;
            foreach (string line in diff.Split('\n'))
            {
                if (line.StartsWith("@@"))
                {
                    int skip = line.IndexOf('-') + 1;
                    curLine = int.Parse(line.Substring(skip, line.IndexOf(',', skip) - skip));
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

            return newLines;
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
            string info = RunGitProcess($"log --pretty=format:\"%an|%s\" -1 {commitId}");

            string[] split = info.Split('|', 2);

            return new CommitInfo
            {
                Author = split[0],
                CommitId = commitId,
                Message = split[1]
            };
        }

        public static HashSet<string> GetIgnoredLines(string ignoredDirectory, string file)
        {
            if (string.IsNullOrEmpty(ignoredDirectory) ||
                !File.Exists(Path.Combine(ignoredDirectory, file)))
            {
                return new HashSet<string>();
            }

            return new HashSet<string>(File.ReadLines(Path.Combine(ignoredDirectory, file))
                                        .Select(line => line.Trim()));

        }

        private static string RunGitProcess(string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = arguments;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.FileName = "git";

            Process process = new Process();
            process.StartInfo = startInfo;

            Console.Error.Write($"Running 'git {arguments}'...");

            process.Start();

            string result = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            Console.Error.WriteLine("...done");

            return result;
        }
    }
}
