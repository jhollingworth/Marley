using System;
using System.Collections.Generic;
using Marley.Pipeline;

namespace Marley.Tests
{
    public class RegistrationMetadata : List<RegistrationMetadata.Registration>
    {
        #region RegistrationType enum

        public enum RegistrationType
        {
            Before,
            After
        }

        #endregion

        #region Nested type: Registration

        public class Registration
        {
            public IPipelineContributor Contributor { get; set; }
            public IPipelineContributor OtherContributor { get; set; }
            public Type OtherContributorType { get; set; }
            public RegistrationType Type { get; set; }

            public override string ToString()
            {
                var other = OtherContributor == null ? OtherContributorType.Name : OtherContributor.GetType().Name;

                return Contributor + " " + Type + " " + other;
            }
        }

        #endregion
    }
}