using System;

namespace TestCoverageReport
{
    public class CommitInfo : IComparable
    {
        public string Author { get; set; }
        public string CommitId { get; set; }
        public string Message { get; set; }
        public bool AllIgnored { get; set; }

        public string GetShortCommitId()
        {
            return this.CommitId.Substring(0, 8);
        }

        public int CompareTo(object obj)
        {
            CommitInfo info = (CommitInfo)obj;

            return this.Author.CompareTo(info.Author);
        }
    }
}
