#!/bin/bash

# ./add-reports.sh <dir> <prefix> <name>

# update to latest commit
git pull origin master

dir="$1"
prefix="$2"
name="$3"

# put the reports in the correct place
cp "$dir/report.txt" "reports/$prefix.txt"
cp "$dir/report.htm" "reports/$prefix.htm"
cp "$dir/commit-report.txt" "reports/$prefix-commits.txt"
git add reports

# update index.html
insertion="<li>\n\t<a href=\"reports/$prefix.htm\">$name</a> (<a href=\"reports\\/$prefix-commits.txt\">Commit view<\\/a>)\n<\\/li>"

sed "12i$insertion" index.html >index.html-new
mv index.html-new index.html

git add -p

git commit -s -m "Report for $name"

git push origin master

