using System;

namespace TestCoverageReport
{
    class Program
    {
        public static readonly string Usage = @"
usage:
    dotnet TestCoverageReport collect <targetBranch> <targetCommitId> <baseBranch> <baseCommitId>
    dotnet TestCoverageReport combine <report-files>";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine(Usage);
            }

            switch (args[0])
            {
                case "collect":
                    if (args.Length < 5)
                    {
                        Console.Error.WriteLine(Usage);
                        return;
                    }

                    Collect.Execute(args);
                    break;

                case "combine":
                    CreateReport.Execute(args);
                    break;

                default:
                    Console.Error.WriteLine(Usage);
                    break;
            }
        }
    }
}
