using System.Collections.Generic;

namespace TestCoverageReport
{
    public class ComparisonReport
    {
        public string BaseBranch { get; set; }
        public string TargetBranch { get; set; }
        public string BaseCommitId { get; set; }
        public string TargetCommitId { get; set; }
        public Dictionary<string, List<FileReportLine>> Files { get; set; }
        public List<CommitInfo> Commits { get; set; }
    }
}
