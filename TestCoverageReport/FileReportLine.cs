namespace TestCoverageReport
{
    public class FileReportLine
    {
        public string FileName { get; set; }
        public string CommitId { get; set; }
        public int LineNumber { get; set; }
        public string LineContents { get; set; }
        public bool Ignored { get; set; }
        public string TargetCommitId { get; set; }

        public string GetShortCommitId()
        {
            return this.CommitId.Substring(0, 8);
        }
    }
}
