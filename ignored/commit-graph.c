570:BUG("missing parent %s for commit %s",
988:error(_("error adding pack %s"), packname.buf);
989:res = 1;
990:goto cleanup;
993:error(_("error opening index for %s"), packname.buf);
994:res = 1;
995:goto cleanup;
1068:error(_("the commit graph format cannot write %d commits"), count_distinct);
1069:res = 1;
1070:goto cleanup;
1104:error(_("too many commits to write graph"));
1105:res = 1;
1106:goto cleanup;
1114:error(_("unable to create leading directories of %s"),
1116:res = errno;
1117:goto cleanup;