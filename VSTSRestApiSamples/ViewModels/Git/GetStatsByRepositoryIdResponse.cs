using System.Collections.Generic;

namespace VstsRestApiSamples.ViewModels.Git
{
    public class GetStatsByRepositoryIdResponse
    {
        public class Stats : BaseViewModel
        {
            public int count { get; set; }
            public List<Value> value { get; set; }
        }

        public class Author
        {
            public string name { get; set; }
            public string email { get; set; }
            public string date { get; set; }
        }

        public class Committer
        {
            public string name { get; set; }
            public string email { get; set; }
            public string date { get; set; }
        }

        public class Commit
        {
            public string commitId { get; set; }
            public Author author { get; set; }
            public Committer committer { get; set; }
            public string comment { get; set; }
            public string url { get; set; }
            public List<string> parents { get; set; }
            public string treeId { get; set; }
        }

        public class Value
        {
            public Commit commit { get; set; }
            public string name { get; set; }
            public int aheadCount { get; set; }
            public int behindCount { get; set; }
            public bool isBaseVersion { get; set; }
        }

    }
}
