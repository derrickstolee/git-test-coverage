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
            writer.WriteLine($"{info.Author}\t{info.GetShortCommitId()} {info.Message}");
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
            if (!line.Ignored)
            {
                writer.WriteLine($"{line.GetShortCommitId()} {line.LineNumber}) {line.LineContents.TrimStart('\t')}");
            }
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

        public void WriteBranches(Dictionary<string, string> branches)
        {
            foreach (KeyValuePair<string, string> branch in branches)
            {
                this.writer.WriteLine($"{branch.Key}\t{branch.Value}");
            }
            this.writer.WriteLine();
        }
    }
}
