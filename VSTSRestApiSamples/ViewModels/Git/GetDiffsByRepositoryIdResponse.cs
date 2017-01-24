using System.Collections.Generic;

namespace VstsRestApiSamples.ViewModels.Git
{
    public class GetDiffsByRepositoryIdResponse
    {
        public class Diffs : BaseViewModel
        {
            public bool allChangesIncluded { get; set; }
            public ChangeCounts changeCounts { get; set; }
            public List<Change> changes { get; set; }
            public string commonCommit { get; set; }
            public int aheadCount { get; set; }
            public int behindCount { get; set; }
        }

        public class ChangeCounts
        {
            public int Add { get; set; }
            public int Edit { get; set; }
        }

        public class Item
        {
            public string gitObjectType { get; set; }
            public string commitId { get; set; }
            public string path { get; set; }
            public bool isFolder { get; set; }
            public string url { get; set; }
        }

        public class Change
        {
            public Item item { get; set; }
            public string changeType { get; set; }
        }

        
    }
}
