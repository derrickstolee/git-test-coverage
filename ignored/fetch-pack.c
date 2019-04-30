1390:die("expected '<hash> <uri>', got: %s\n", reader->line);
1395:die("expected DELIM");
1522:die("fetch-pack: unable to spawn http-fetch");
1526:die("fetch-pack: expected keep then TAB at start of http-fetch output");
1531:die("fetch-pack: expected hash then LF at end of http-fetch output");
1538:die("fetch-pack: unable to finish http-fetch");
1542:die("fetch-pack: pack downloaded from %s does not match expected hash %.*s",
1543:uri, (int) the_hash_algo->hexsz,
1544:packfile_uris.items[i].string);