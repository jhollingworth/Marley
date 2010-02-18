using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marley.Pipeline.Configuration;
using Marley.Tests;

namespace Marley.Pipeline
{
    public class OrderedContributorList : IEnumerable<IPipelineContributor>
    {
        private readonly List<IPipelineContributor> _contributors;

        public OrderedContributorList(IEnumerable<IPipelineContributor> contributors, RegistrationMetadata data)
        {
            _contributors = contributors.ToList();
            
            try
            {
                _contributors.Sort(new PipelineContributorComparer(data));
            }
            catch (InvalidOperationException ex)
            {
                throw ex.InnerException;
            }
        }

        #region IEnumerable<IPipelineContributor> Members

        IEnumerator<IPipelineContributor> IEnumerable<IPipelineContributor>.GetEnumerator()
        {
            return _contributors.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _contributors.GetEnumerator();
        }

        #endregion

        #region Nested type: PipelineContributorComparer

        private class PipelineContributorComparer : IComparer<IPipelineContributor>
        {
            private readonly RegistrationMetadata _metadata;

            public PipelineContributorComparer(RegistrationMetadata metadata)
            {
                _metadata = metadata;
            }

            #region IComparer<IPipelineContributor> Members

            public int Compare(IPipelineContributor x, IPipelineContributor y)
            {
                if (x.Equals(y))
                {
                    return 0;
                }

                var metadata = _metadata.Where(m => RegistrationReferencesBothContributors(m, x, y));

                if (metadata.Any())
                {
                    var result = 0;

                    foreach (var m in metadata)
                    {
                        var tempResult = Compare(m, x);

                        if(tempResult == 0)
                        {
                            tempResult = -Compare(m, y);
                        }

                        if (result != 0 && tempResult != result)
                        {
                            throw new ConflictingMetadataException("Conflicting metadata");
                        }

                        result = tempResult;
                    }

                    return result;
                }

                return 0;
            }

            private static int Compare(RegistrationMetadata.Registration registration, IPipelineContributor contributor)
            {
                if(registration.Contributor == contributor)
                    return registration.Type == RegistrationMetadata.RegistrationType.Before ? -1 : 1;

                if (registration.Contributor == contributor)
                    return registration.Type == RegistrationMetadata.RegistrationType.After ? -1 : 1;

                return 0;
            }

            #endregion

            private static bool RegistrationReferencesBothContributors(RegistrationMetadata.Registration registration, IPipelineContributor contributorX, IPipelineContributor contributorY)
            {
                var contains = new Predicate<IPipelineContributor>(c =>  registration.Contributor == c || registration.OtherContributorType == c.GetType());

                return contains(contributorX) && contains(contributorY);
            }
        }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Ordered Contributors:");

            var order = 1;
            foreach (var contributor in _contributors)
                sb.AppendLine("\t " + order++ + ": " + contributor);

            return sb.ToString();
        }
    }
}