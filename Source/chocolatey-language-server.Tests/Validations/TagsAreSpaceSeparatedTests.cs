using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;
using System.Collections.Generic;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class TagsAreSpaceSeparatedTests : ValidationBaseTests<TagsAreSpaceSeparated>
    {
        public override string ExpectedId { get; } = "choco0012";

        [Test]
        public void Should_FailValidationWhenTagsAreCommaSeparated()
        {
            var package = new Package
            {
                Tags = new List<MetaValue<string>>
                {
                    "my",
                    "tag-e",
                    "is",
                    "comma,delimited"
                }
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenTagsAreEmpty()
        {
            var package = new Package();

            ValidateDiagnosticResult(package, 0);
        }

        [Test]
        public void Should_NotFailValidationWhenTagsAreNotCommaSeparated()
        {
            var package = new Package
            {
                Tags = new List<MetaValue<string>>
                {
                    "my",
                    "tag-e",
                    "is",
                    "comma",
                    "delimited"
                }
            };

            ValidateDiagnosticResult(package, 0);
        }
    }
}