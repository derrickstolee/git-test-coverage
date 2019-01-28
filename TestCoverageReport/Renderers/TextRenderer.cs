using System;
using System.Collections.Generic;
using System.IO;

namespace TestCoverageReport
{
    public class TextRenderer : IRenderer
    {
        private readonly StreamWriter writer;

        public TextRenderer(StreamWriter writer)
        {
            this.writer = writer;
        }

        public void WriteCommitLine(CommitInfo info)
        {
            writer.Write($"{info.Author}\t{info.CommitId} {info.Message}");
        }

        public void WriteCommitSectionFooter()
        {
            writer.WriteLine();
        }

        public void WriteCommitSectionHeader()
        {
            writer.WriteLine("Commits introducting uncovered code:");
        }

        public void WriteDiffSectionHeader(string baseBranch, string targetBranch, string baseCommitId, string targetCommitId)
        {
            writer.WriteLine($@"
Uncovered code in '{targetBranch}' not in '{baseBranch}'
--------------------------------------------------------
");
        }

        public void WriteFileLine(FileReportLine line)
        {
            writer.WriteLine($"{line.CommitId} {line.FileName} {line.LineNumber}) {line.LineContents.TrimStart('\t')}");
        }

        public void WriteFileSectionFooter()
        {
            writer.WriteLine();
        }

        public void WriteFileSectionHeader(string file)
        {
            writer.WriteLine(file);
        }

        public void WriteFooter()
        {
            writer.WriteLine();
        }

        public void WriteHeader()
        {
        }

        public void WriteBranches(List<Tuple<string, string>> branches)
        {
            foreach (Tuple<string, string> branch in branches)
            {
                this.writer.WriteLine($"{branch.Item1}\t{branch.Item2}");
            }
            this.writer.WriteLine();
        }
    }
}
