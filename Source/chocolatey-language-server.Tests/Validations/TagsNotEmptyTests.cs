using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;
using System.Collections.Generic;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class TagsNotEmptyTests : ValidationBaseTests<TagsNotEmpty>
    {
        public override string ExpectedId { get; } = "choco0013";

        [Test]
        public void Should_FailValidationWhenAllTagsAreEmpty()
        {
            var package = new Package
            {
                Tags = new List<MetaValue<string>>
                {
                    null,
                    "",
                    "                "
                }
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_FailValidationWhenThereAreNoTags()
        {
            var package = new Package();

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenNoTagsAreEmpty()
        {
            var package = new Package
            {
                Tags = new List<MetaValue<string>>
                {
                    "chocolatey-language-server",
                    "build"
                }
            };

            ValidateDiagnosticResult(package, 0);
        }
    }
}