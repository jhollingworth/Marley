using System.Collections.Generic;

namespace Marley.Pipeline
{
    public class PipelineContributorSorter
    {
        public IEnumerable<IPipelineContributor> SortContributors(IEnumerable<IPipelineContributor> contributors)
        {
            return contributors;
        }
    }
}