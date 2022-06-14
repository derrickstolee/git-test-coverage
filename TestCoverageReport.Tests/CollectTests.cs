using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCoverageReport.Tests
{
    [TestClass]
    public class CollectTests
    {
        [TestMethod]
        public void ParseGCovOutput()
        {
            string[] output = new string[]
            {
                "        -:  148:",
                "      905:  149:                chunk_lookup += GRAPH_CHUNKLOOKUP_WIDTH;",
                "        -:  150:",
                "      905:  151:                if (chunk_offset > graph_size - the_hash_algo->rawsz) {",
                "branch  0 taken 0% (fallthrough)",
                "branch  1 taken 100%",
                "    #####:  152:                        error(_(\"improper chunk offset %08x%08x\"), (uint32_t)(chunk_offset >> 32),",
                "    $$$$$:  152-block  0",
                "call    0 never executed",
                "call    1 never executed",
                "call    2 never executed",
                "        -:  153:                              (uint32_t)chunk_offset);",
                "    #####:  154:                        goto cleanup_fail;",
                "        -:  155:                }",
                "        -:  156:",
                "      905:  157:                switch (chunk_id) {",
                "      905:  157-block  0",
                "branch  0 taken 28%",
                "branch  1 taken 28%",
                "branch  2 taken 28%",
                "branch  3 taken 16%",
                "branch  4 taken 1%",
                "      253:  158:                case GRAPH_CHUNKID_OIDFANOUT:",
                "      253:  159:                        if (graph->chunk_oid_fanout)",
                "      253:  159-block  0",
                "branch  0 taken 0% (fallthrough)",
                "branch  1 taken 100%",
                "    #####:  160:                                chunk_repeated = 1;",
                "    $$$$$:  160-block  0",
                "        -:  161:                        else",
                "      253:  162:                                graph->chunk_oid_fanout = (uint32_t*)(data + chunk_offset);",
                "      253:  162-block  0",
                "      253:  163:                        break;",
            };

            HashSet<int> lines = Collect.GetUncoveredLines(output);

            Assert.AreEqual(3, lines.Count);
            Assert.IsTrue(lines.Contains(152));
            Assert.IsTrue(lines.Contains(154));
            Assert.IsTrue(lines.Contains(160));
        }

        [TestMethod]
        public void ParseBlameOutput()
        {
            string blameOutput = @"08fd81c9b6495a395a527985d18aa51c4ae66cdc    1) #include ""cache.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc    2) #include ""config.h""
33286dcd6d79eeb81b74f36a324f260275582639    3) #include ""dir.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc    4) #include ""git-compat-util.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc    5) #include ""lockfile.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc    6) #include ""pack.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc    7) #include ""packfile.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc    8) #include ""commit.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc    9) #include ""object.h""
59fb87701ff68eb114e54ce6834e91c4ae8f60a7   10) #include ""refs.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc   11) #include ""revision.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc   12) #include ""sha1-lookup.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc   13) #include ""commit-graph.h""
b10edb2df55241b2e042b3d5473537904d09d193   14) #include ""object-store.h""
96af91d410c70ab750a9a1ecdf858c9ec46be767   15) #include ""alloc.h""
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   16) #include ""hashmap.h""
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   17) #include ""replace-object.h""
7b0f2292224cec841de78acc911e4c378ea79faa   18) #include ""progress.h""
08fd81c9b6495a395a527985d18aa51c4ae66cdc   19)
08fd81c9b6495a395a527985d18aa51c4ae66cdc   20) #define GRAPH_SIGNATURE 0x43475048 /* ""CGPH"" */
08fd81c9b6495a395a527985d18aa51c4ae66cdc   21) #define GRAPH_CHUNKID_OIDFANOUT 0x4f494446 /* ""OIDF"" */
08fd81c9b6495a395a527985d18aa51c4ae66cdc   22) #define GRAPH_CHUNKID_OIDLOOKUP 0x4f49444c /* ""OIDL"" */
08fd81c9b6495a395a527985d18aa51c4ae66cdc   23) #define GRAPH_CHUNKID_DATA 0x43444154 /* ""CDAT"" */
08fd81c9b6495a395a527985d18aa51c4ae66cdc   24) #define GRAPH_CHUNKID_LARGEEDGES 0x45444745 /* ""EDGE"" */
08fd81c9b6495a395a527985d18aa51c4ae66cdc   25)
c166599862d10a273f61b834559eaa567c3dbfd9   26) #define GRAPH_DATA_WIDTH (the_hash_algo->rawsz + 16)
08fd81c9b6495a395a527985d18aa51c4ae66cdc   27)
08fd81c9b6495a395a527985d18aa51c4ae66cdc   28) #define GRAPH_VERSION_1 0x1
08fd81c9b6495a395a527985d18aa51c4ae66cdc   29) #define GRAPH_VERSION GRAPH_VERSION_1
08fd81c9b6495a395a527985d18aa51c4ae66cdc   30)
08fd81c9b6495a395a527985d18aa51c4ae66cdc   31) #define GRAPH_OCTOPUS_EDGES_NEEDED 0x80000000
08fd81c9b6495a395a527985d18aa51c4ae66cdc   32) #define GRAPH_EDGE_LAST_MASK 0x7fffffff
08fd81c9b6495a395a527985d18aa51c4ae66cdc   33) #define GRAPH_PARENT_NONE 0x70000000
08fd81c9b6495a395a527985d18aa51c4ae66cdc   34)
08fd81c9b6495a395a527985d18aa51c4ae66cdc   35) #define GRAPH_LAST_EDGE 0x80000000
08fd81c9b6495a395a527985d18aa51c4ae66cdc   36)
0e3b97cccbec2bd01eae4b3267bf00a9bfb277d8   37) #define GRAPH_HEADER_SIZE 8
08fd81c9b6495a395a527985d18aa51c4ae66cdc   38) #define GRAPH_FANOUT_SIZE (4 * 256)
08fd81c9b6495a395a527985d18aa51c4ae66cdc   39) #define GRAPH_CHUNKLOOKUP_WIDTH 12
0e3b97cccbec2bd01eae4b3267bf00a9bfb277d8   40) #define GRAPH_MIN_SIZE (GRAPH_HEADER_SIZE + 4 * GRAPH_CHUNKLOOKUP_WIDTH \
c166599862d10a273f61b834559eaa567c3dbfd9   41)                  + GRAPH_FANOUT_SIZE + the_hash_algo->rawsz)
08fd81c9b6495a395a527985d18aa51c4ae66cdc   42)
2a2e32bdc5a80221981939e77643cec3462b4793   43) char *get_commit_graph_filename(const char *obj_dir)
08fd81c9b6495a395a527985d18aa51c4ae66cdc   44) {
08fd81c9b6495a395a527985d18aa51c4ae66cdc   45)  return xstrfmt(""%s/info/commit-graph"", obj_dir);
08fd81c9b6495a395a527985d18aa51c4ae66cdc   46) }
08fd81c9b6495a395a527985d18aa51c4ae66cdc   47)
c166599862d10a273f61b834559eaa567c3dbfd9   48) static uint8_t oid_version(void)
c166599862d10a273f61b834559eaa567c3dbfd9   49) {
c166599862d10a273f61b834559eaa567c3dbfd9   50)  return 1;
c166599862d10a273f61b834559eaa567c3dbfd9   51) }
c166599862d10a273f61b834559eaa567c3dbfd9   52)
2a2e32bdc5a80221981939e77643cec3462b4793   53) static struct commit_graph *alloc_commit_graph(void)
2a2e32bdc5a80221981939e77643cec3462b4793   54) {
2a2e32bdc5a80221981939e77643cec3462b4793   55)  struct commit_graph *g = xcalloc(1, sizeof(*g));
2a2e32bdc5a80221981939e77643cec3462b4793   56)  g->graph_fd = -1;
2a2e32bdc5a80221981939e77643cec3462b4793   57)
2a2e32bdc5a80221981939e77643cec3462b4793   58)  return g;
2a2e32bdc5a80221981939e77643cec3462b4793   59) }
2a2e32bdc5a80221981939e77643cec3462b4793   60)
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   61) extern int read_replace_refs;
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   62)
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   63) static int commit_graph_compatible(struct repository *r)
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   64) {
5cef295f283e84351a104c66f949a53a56297aa7   65)  if (!r->gitdir)
5cef295f283e84351a104c66f949a53a56297aa7   66)          return 0;
5cef295f283e84351a104c66f949a53a56297aa7   67)
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   68)  if (read_replace_refs) {
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   69)          prepare_replace_object(r);
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   70)          if (hashmap_get_size(&r->objects->replace_map->map))
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   71)                  return 0;
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   72)  }
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   73)
20fd6d57996e33c30b6bb030329523d0116f15ec   74)  prepare_commit_graft(r);
20fd6d57996e33c30b6bb030329523d0116f15ec   75)  if (r->parsed_objects && r->parsed_objects->grafts_nr)
20fd6d57996e33c30b6bb030329523d0116f15ec   76)          return 0;
20fd6d57996e33c30b6bb030329523d0116f15ec   77)  if (is_repository_shallow(r))
20fd6d57996e33c30b6bb030329523d0116f15ec   78)          return 0;
20fd6d57996e33c30b6bb030329523d0116f15ec   79)
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   80)  return 1;
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   81) }
d6538246d3d4edbfbc9b0af6a2aa38552d35f7f1   82)
2a2e32bdc5a80221981939e77643cec3462b4793   83) struct commit_graph *load_commit_graph_one(const char *graph_file)
2a2e32bdc5a80221981939e77643cec3462b4793   84) {
2a2e32bdc5a80221981939e77643cec3462b4793   85)  void *graph_map;
2a2e32bdc5a80221981939e77643cec3462b4793   86)  const unsigned char *data, *chunk_lookup;
2a2e32bdc5a80221981939e77643cec3462b4793   87)  size_t graph_size;
2a2e32bdc5a80221981939e77643cec3462b4793   88)  struct stat st;
2a2e32bdc5a80221981939e77643cec3462b4793   89)  uint32_t i;
2a2e32bdc5a80221981939e77643cec3462b4793   90)  struct commit_graph *graph;
2a2e32bdc5a80221981939e77643cec3462b4793   91)  int fd = git_open(graph_file);
2a2e32bdc5a80221981939e77643cec3462b4793   92)  uint64_t last_chunk_offset;
2a2e32bdc5a80221981939e77643cec3462b4793   93)  uint32_t last_chunk_id;
2a2e32bdc5a80221981939e77643cec3462b4793   94)  uint32_t graph_signature;
2a2e32bdc5a80221981939e77643cec3462b4793   95)  unsigned char graph_version, hash_version;
2a2e32bdc5a80221981939e77643cec3462b4793   96)
2a2e32bdc5a80221981939e77643cec3462b4793   97)  if (fd < 0)
2a2e32bdc5a80221981939e77643cec3462b4793   98)          return NULL;
2a2e32bdc5a80221981939e77643cec3462b4793   99)  if (fstat(fd, &st)) {
2a2e32bdc5a80221981939e77643cec3462b4793  100)          close(fd);";

            HashSet<int> uncoveredLines = new HashSet<int>() { 50, 65, 66, 98 };
            List<FileReportLine> lines = Collect.GetBlameLines("commit-graph.c", uncoveredLines, blameOutput);

            Assert.AreEqual(uncoveredLines.Count, lines.Count);

            Assert.AreEqual(50, lines[0].LineNumber);
            Assert.AreEqual(" return 1;", lines[0].LineContents);
            Assert.AreEqual("c166599862d10a273f61b834559eaa567c3dbfd9", lines[0].CommitId);
            Assert.AreEqual("commit-graph.c", lines[0].FileName);
        }

        [TestMethod]
        public void GetNewLines()
        {
            string diffOutput = @"@@ -43,13 +38,18 @@
 #define GRAPH_FANOUT_SIZE (4 * 256)
 #define GRAPH_CHUNKLOOKUP_WIDTH 12
 #define GRAPH_MIN_SIZE (GRAPH_HEADER_SIZE + 4 * GRAPH_CHUNKLOOKUP_WIDTH \
-                       + GRAPH_FANOUT_SIZE + GRAPH_OID_LEN)
+                       + GRAPH_FANOUT_SIZE + the_hash_algo->rawsz)

 char *get_commit_graph_filename(const char *obj_dir)
 {
        return xstrfmt(""%s/info/commit-graph"", obj_dir);
 }

+static uint8_t oid_version(void)
+{
+       return 1;
+}
+
 static struct commit_graph *alloc_commit_graph(void)
 {
        struct commit_graph *g = xcalloc(1, sizeof(*g));
@@ -124,15 +124,15 @@ struct commit_graph *load_commit_graph_one(const char *graph_file)
        }

        hash_version = *(unsigned char*)(data + 5);
-       if (hash_version != GRAPH_OID_VERSION) {
+       if (hash_version != oid_version()) {
                error(_(""hash version %X does not match version %X""),
-                     hash_version, GRAPH_OID_VERSION);
+                     hash_version, oid_version());
                goto cleanup_fail;
        }

        graph = alloc_commit_graph();

-       graph->hash_len = GRAPH_OID_LEN;
+       graph->hash_len = the_hash_algo->rawsz;
        graph->num_chunks = *(unsigned char*)(data + 6);
        graph->graph_fd = fd;
        graph->data = graph_map;
@@ -148,7 +148,7 @@ struct commit_graph *load_commit_graph_one(const char *graph_file)

                chunk_lookup += GRAPH_CHUNKLOOKUP_WIDTH;

-               if (chunk_offset > graph_size - GIT_MAX_RAWSZ) {
+               if (chunk_offset > graph_size - the_hash_algo->rawsz) {
                        error(_(""improper chunk offset %08x%08x""), (uint32_t)(chunk_offset >> 32),
                              (uint32_t)chunk_offset);
                        goto cleanup_fail;
@@ -288,7 +288,8 @@ static int bsearch_graph(struct commit_graph *g, struct object_id *oid, uint32_t
                            g->chunk_oid_lookup, g->hash_len, pos);
 }

-static struct commit_list **insert_parent_or_die(struct commit_graph *g,
+static struct commit_list **insert_parent_or_die(struct repository *r,
+                                                struct commit_graph *g,
                                                 uint64_t pos,
                                                 struct commit_list **pptr)
 {
";

            List<int> newLines = Collect.GetNewLines(diffOutput);

            Assert.AreEqual(12, newLines.Count);
            Assert.AreEqual(41, newLines[0]);
            Assert.AreEqual(48, newLines[1]);
            Assert.AreEqual(49, newLines[2]);
            Assert.AreEqual(50, newLines[3]);
            Assert.AreEqual(51, newLines[4]);
            Assert.AreEqual(52, newLines[5]);
            Assert.AreEqual(127, newLines[6]);
            Assert.AreEqual(129, newLines[7]);
            Assert.AreEqual(135, newLines[8]);
            Assert.AreEqual(151, newLines[9]);
            Assert.AreEqual(291, newLines[10]);
            Assert.AreEqual(292, newLines[11]);
        }
    }
}
