using System;
using System.Collections.Generic;

namespace TestCoverageReport
{
    public interface IRenderer
    {
        void WriteHeader();
        void WriteBranches(Dictionary<string, string> branches);
        void WriteDiffSectionHeader(
            string baseBranch,
            string targetBranch,
            string baseCommitId,
            string targetCommitId);
        void WriteFileSectionHeader(string file);
        void WriteFileLine(FileReportLine line);
        void WriteFileSectionFooter();
        void WriteCommitSectionHeader();
        void WriteCommitLine(CommitInfo info);
        void WriteCommitSectionFooter();
        void WriteFooter();
    }
}
