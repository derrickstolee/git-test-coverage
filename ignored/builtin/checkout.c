256:die(_("make_cache_entry failed for path '%s'"), path);
302:return;
355:return error(_("index file corrupt"));
415:die(_("'%s' cannot be used with updating paths"),
425:die(_("'%s' cannot be used with %s"),
433:die(_("neither '%s' or '%s' is specified"),
437:die(_("'%s' must be used when '%s' is not specified"),
442:die(_("'%s' or '%s' cannot be used with %s"),
447:die(_("'%s' or '%s' cannot be used with %s"),
456:patch_mode = "--patch=reset";
460:BUG("either flag must have been set, worktree=%d, index=%d",
536:die(_("unable to write new index file"));
656:BUG("'switch --orphan' should never accept a commit as starting point");
1039:BUG("'switch --orphan' should never accept a commit as starting point");
1176:die(_("only one reference expected"));
1300:const char *ref = to_free;
1302:if (skip_prefix(ref, "refs/tags/", &ref))
1303:die(_("a branch is expected, got tag '%s'"), ref);
1304:if (skip_prefix(ref, "refs/remotes/", &ref))
1305:die(_("a branch is expected, got remote branch '%s'"), ref);
1306:die(_("a branch is expected, got '%s'"), ref);
1314:die(_("a branch is expected, got '%s'"), branch_info->name);
1329:die(_("cannot switch branch in the middle of an am session\n"
1333:die(_("cannot switch branch while rebasing\n"
1337:die(_("cannot switch branch while cherry-picking\n"
1341:die(_("cannot switch branch while reverting\n"
1345:die(_("cannot switch branch while bisecting\n"
1372:die(_("'%s' cannot be used with '%s'"), "--discard-changes", "--merge");
1510:BUG("make up your mind, you need to take _something_");
1542:opts->checkout_index = 0;
1552:BUG("these flags should be non-negative by now");
1613:die(_("could not resolve %s"), opts->from_treeish);