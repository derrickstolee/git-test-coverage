using System;
using System.Collections.Generic;
using System.IO;

namespace TestCoverageReport
{
    public class HtmlRenderer : IRenderer
    {
        private readonly StreamWriter writer;

        public HtmlRenderer(StreamWriter writer)
        {
            this.writer = writer;
        }

        public void WriteHeader()
        {
            this.writer.WriteLine($@"<!DOCTYPE html>
<html>
<head>
	<title>Git Test Coverage Report ({DateTime.Now.ToShortDateString()})</title>
	<link rel=""stylesheet"" href=""style.css""/>
    <script src = ""code.js""></script>
</head>
<body>
<p>
<button onClick=""toggleIgnored()"">Show/Hide Ignored Lines</button>
</p>");
        }

        public void WriteBranches(Dictionary<string, string> branches)
        {
            this.writer.WriteLine("<table>");
            foreach (KeyValuePair<string,string> branch in branches)
            {
                this.writer.WriteLine($@"
<tr>
    <td>{branch.Key}</td>
    <td>{branch.Value}</td>
</tr>");
            }
            this.writer.WriteLine("</table>");
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
                {targetCommitId}
            </a>
		</td>
	</tr>
	<tr>
		<td class=""branch"">
		    {baseBranch}
		</td>
		<td class=""full-commit-id"">
			<a href=""https://github.com/git/git/tree/{targetCommitId}"">
                {baseCommitId}
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

        public void WriteFileLine(FileReportLine line)
        {
            string tr_class = line.Ignored ? "ignored-hidden" : $"commit-{line.CommitId}";
            this.writer.WriteLine($@"
    <tr class=""{tr_class}"">
		<td class=""commit-id"">
            <a href=""https://github.com/git/git/tree/{line.CommitId}/{line.FileName}#L{line.LineNumber}"">
                {line.GetShortCommitId()}
            </a>
        </td>
        <td class=""line-number"">
			{line.LineNumber}
		</td>
		<td class=""code"">
			{Sanitize(line.LineContents)}
		</td>
	</tr>");
        }

        public string Sanitize(string code)
        {
            return code.Replace(">", "&gt;").Replace("<", "&lt;");
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

        public void WriteCommitLine(CommitInfo info)
        {
            string tr_class = info.AllIgnored ? "ignored-hidden" : $"commit-{info.CommitId}";
            this.writer.WriteLine($@"
	<tr class=""{tr_class}"" onclick=""toggleCommit('commit-{info.CommitId}')"">
		<td class=""author"">
            {info.Author}
        </td>
        <td class=""commit-id"">
			<a href=""https://github.com/git/git/commit/{info.CommitId}"">{info.GetShortCommitId()}</a>
		</td>
		<td class=""message"">
			{info.Message}
        </td>
	</tr>");
        }

        public void WriteCommitSectionFooter()
        {
            this.writer.WriteLine("</table>");
        }

        public void WriteFooter()
        {
            this.writer.WriteLine(@"
<p>
<button onClick=""toggleIgnored()"">Show/Hide Ignored Lines</button>
</p>
</body>
</html>");
        }
    }
}
