using System;
using System.Collections.Generic;

namespace TestCoverageReport
{
    public class CombinedReport
    {
        public DateTime ReportDate { get; set; }

        public List<ComparisonReport> Comparisons { get; set; }
    }
}
