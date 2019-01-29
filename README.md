Git Test Coverage Reports
=========================

This repo contains tools to generate test coverage reports for
[the Git project](https://github.com/git/git). It also contains the reports
themselves after they are generated for display on
[https://derrickstolee.github.io/git-test-coverage](https://derrickstolee.github.io/git-test-coverage).

Contributing
------------

Pull requests are welcome! Do you see a way to make the reports easier to read?
Do you want a feature that helps you read the HTML report?

Ignored Lines
-------------

Test coverage is a tricky business. We should not pursue 100% coverage, for
in that way lies madness. There are a lot of blocks that simply catch error
conditions that should never happen.

We can ignore these trivial lines by contributing to this repo!

If you see a line such as this in the coverage report:

> date.c
> acdd37769d  113) die("Timestamp too large for this system: %"PRItime, time);

then you can add a file `ignored/date.c` with the following line:

> 113:die("Timestamp too large for this system: %"PRItime, time);

The line format is very simple: `<line-number>:<code>` where the `code` is
trimmed of whitespace.

