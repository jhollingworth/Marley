using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;
using NUnit.Framework;

namespace Marley.Tests.Pipeline
{
    [TestFixture]
    public class OrderedContributorListTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _metadata = new RegistrationMetadata();
            _order = new ContributorOrder();
            _contributors = new List<IPipelineContributor>();
            _factory = new ContributorFactory(_order, _contributors);
        }

        #endregion

        private RegistrationMetadata _metadata;
        private ContributorOrder _order;
        private List<IPipelineContributor> _contributors;
        private ContributorFactory _factory;

        public class ContributorRegistration<T>
        {
            public ContributorRegistration(int correctOrder, RegistrationMetadata registrationMetadata, ContributorFactory contributorFactory)
            {
                var contributor = contributorFactory.CreateContributor<T>(correctOrder);
                which_is_executed = new ContributorOrderRegistration(this, registrationMetadata, contributorFactory, contributor);
            }

            public ContributorOrderRegistration which_is_executed { get; set; }

            #region Nested type: ContributorOrderRegistration

            public class ContributorOrderRegistration
            {
                private readonly ContributorOrder _order;
                private readonly List<IPipelineContributor> _contributors;
                private readonly IPipelineContributor _contributor;
                private readonly RegistrationMetadata _registrationMetadata;
                private readonly ContributorFactory _contributorFactory;
                private ContributorRegistration<T> _contributorRegistration;

                public ContributorOrderRegistration(ContributorRegistration<T> contributorRegistration, RegistrationMetadata registrationMetadata, ContributorFactory contributorFactory, IPipelineContributor contributor)
                {
                    _contributorRegistration = contributorRegistration;
                    _registrationMetadata = registrationMetadata;
                    _contributorFactory = contributorFactory;
                    _contributor = contributor;
                }

                public void After<J>(int correctOrder)
                {
                    AddContributor<J>(correctOrder, RegistrationMetadata.RegistrationType.After);
                }

                public void Before<J>(int correctOrder)
                {
                    AddContributor<J>(correctOrder, RegistrationMetadata.RegistrationType.Before);
                }

                private void AddContributor<J>(int correctOrder, RegistrationMetadata.RegistrationType orderType)
                {
                    var contributor = _contributorFactory.CreateContributor<J>(correctOrder);

                    _registrationMetadata.Add(new RegistrationMetadata.Registration
                                                  {
                                                      Contributor = _contributor,
                                                      OtherContributorType = typeof(DummyContributor<J>),
                                                      Type = orderType
                                                  });

                    Debug.WriteLine(string.Format("{0} is executed {1} {2}", _contributor, orderType, contributor));
                }
            }

            #endregion
        }

        public class ContributorFactory
        {
            private readonly ContributorOrder _order;
            private readonly List<IPipelineContributor> _contributors;

            public ContributorFactory(ContributorOrder order, List<IPipelineContributor> contributors)
            {
                _order = order;
                _contributors = contributors;
            }

            public IPipelineContributor CreateContributor<T>(int correctOrder)
            {
                var contributor = _contributors.SingleOrDefault(c => c.GetType().GetGenericArguments()[0] == typeof (T));

                if(contributor != null)
                {
                    return contributor;
                }

                contributor = new DummyContributor<T>(_order, correctOrder);

                _contributors.Add(contributor);

                return contributor;
            }

        }

        private void then_the_contributors_should_be_ordered_correctly()
        {
            var contributors = new OrderedContributorList(_contributors, _metadata);

            Debug.WriteLine(contributors);

            foreach (IPipelineContributor contributor in contributors)
            {
                Assert.AreEqual(PipelineContinuation.Continue, contributor.Execute(null));
            }
        }

        public ContributorRegistration<T> given_the_contributor<T>(int correctOrder)
        {
            return new ContributorRegistration<T>(correctOrder, _metadata, _factory
            );
        }

        public class ContributorOrder
        {
            private int _order;

            public ContributorOrder()
            {
                _order = 0;
            }

            public bool IsExecutedInCorrectOrder(IPipelineContributor contributor, int order)
            {
                _order++;

                return _order == order;
            }
        }

        public class DummyContributor<T> : IPipelineContributor
        {
            private readonly int _correctOrder;
            private readonly ContributorOrder _order;

            public DummyContributor(ContributorOrder order, int correctOrder)
            {
                _order = order;
                _correctOrder = correctOrder;
            }

            #region IPipelineContributor Members

            public void Register(IPipelineConfiguration context)
            {
            }

            public PipelineContinuation Execute(IApiContext context)
            {
                return _order.IsExecutedInCorrectOrder(this, _correctOrder) ? PipelineContinuation.Continue : PipelineContinuation.Abort;
            }

            #endregion

            public override string ToString()
            {
                return typeof (T).Name;
            }
        }

        [Test]
        public void when_one_contributor_registers_to_be_before_another_ordering_is_correct()
        {
            given_the_contributor<int>(2).
                which_is_executed.After<string>(1);

            then_the_contributors_should_be_ordered_correctly();
        }

        [Test]
        public void when_one_contributorr_registers_to_be_after_another_ordering_is_correct()
        {
            given_the_contributor<string>(1)
                .which_is_executed.Before<int>(2);

            then_the_contributors_should_be_ordered_correctly();
        }

        [Test]
        [ExpectedException(typeof(ConflictingMetadataException))]
        public void when_two_contributors_have_conflicting_metadata_throw_ConflictingMetaDataException()
        {
            given_the_contributor<string>(1)   
                .which_is_executed.Before<int>(2);

            given_the_contributor<int>(2)
                .which_is_executed.Before<string>(1);

            then_should_throw_exception();
        }

        [Test]
        public void when_two_contributors_are_registered_one_after_another_then_they_are_ordered_correctly()
        {
            given_the_contributor<string>(3)
                .which_is_executed.After<bool>(2);

            given_the_contributor<int>(1)
                .which_is_executed.Before<bool>(2);

            then_the_contributors_should_be_ordered_correctly();
        }

        [Test]
        public void when_there_are_lots_of_contributors_where_are_ordered_in_odd_ways_it_all_works()
        {
            given_the_contributor<string>(4)
                .which_is_executed.After<int>(1);

            given_the_contributor<float>(3)
                .which_is_executed.After<int>(1);

            given_the_contributor<bool>(2)
                .which_is_executed.After<int>(1);

            then_the_contributors_should_be_ordered_correctly();
        }

        private void then_should_throw_exception()
        {
            new OrderedContributorList(_contributors, _metadata);
        }
    }
}