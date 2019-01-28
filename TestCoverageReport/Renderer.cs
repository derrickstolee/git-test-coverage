using System;
using System.IO;

namespace TestCoverageReport
{
    public class Renderer
    {
        private readonly StreamWriter writer;

        public Renderer(StreamWriter writer)
        {
            this.writer = writer;
        }

        public void WriteHeader()
        {
            this.writer.WriteLine($@"
<!DOCTYPE html>
<html>
<head>
	<title>Git Test Coverage Report ({DateTime.Now.ToShortDateString()})</title>
	<link rel=""stylesheet"" href=""style.css""/>
    <script src = ""code.js""/>
</head>
<body>");
        }

        public void WriteDiffSectionHeader(
            string baseBranch,
            string targetBranch,
            string baseCommitId,
            string targetCommitId)
        {
            this.writer.WriteLine($@"
<h2>Uncovered code in '{targetBranch}' not in '{baseBranch}'</h2>

<table class=""branches"">
	<tr>
		<td class=""branch"">
            {targetBranch}
        </td>
        <td class=""full-commit-id"">
			<a href=""https://github.com/git/git/tree/{baseCommitId}"">
                {baseCommitId}
            </a>
		</td>
	</tr>
	<tr>
		<td class=""branch"">
		    {targetBranch}
		</td>
		<td class=""full-commit-id"">
			<a href=""https://github.com/git/git/tree/{targetCommitId}"">
                {targetCommitId}
            </a>
		</td>
	</tr>
</table>");
        }

        public void WriteFileSectionHeader(string file)
        {
            this.writer.WriteLine($@"
<h3>{file}</h3>

<table  class=""uncovered-lines"">");
        }

        public void WriteFileLine(string file, string commitId, string lineNumber, string code)
        {
            this.writer.WriteLine($@"
    <tr class=""commit-{commitId}"">
		<td class=""commit-id"">
            <a href=""https://github.com/git/git/tree/{commitId}/{file}#L{lineNumber}"">
                {GetShortHash(commitId)}
            </a>
        </td>
        <td class=""line-number"">
			{lineNumber}
		</td>
		<td class=""code"">
			{code}
		</td>
	</tr>");
        }

        public void WriteFileSectionFooter()
        {
            this.writer.WriteLine("</table>");
        }

        public void WriteCommitSectionHeader()
        {
            this.writer.WriteLine($@"
<h3>Commits introducing uncovered code:</h3>

<table class=""commits"">");
        }

        public void WriteCommitLine(string commitId, string author, string message)
        {
            this.writer.WriteLine($@"
	<tr class=""commit-{commitId}"" onclick=""toggleCommit('commit-{commitId}')"">
		<td class=""author"">
            {author}
        </td>
        <td class=""commit-id"">
			<a href=""https://github.com/git/git/commit/{commitId}"">{GetShortHash(commitId)}</a>
		</td>
		<td class=""message"">
			{message}
        </td>
	</tr>");
        }

        public void WriteCommitsSectionFooter()
        {
            this.writer.WriteLine("</table>");
        }

        public void WriteFooter()
        {
            this.writer.WriteLine("</body></html>");
        }

        public static string GetShortHash(string commitId) => commitId.Substring(0, 10);
    }
}
