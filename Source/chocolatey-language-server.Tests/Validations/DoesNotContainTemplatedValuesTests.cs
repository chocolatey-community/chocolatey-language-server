using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class DoesNotContainTemplatedValuesTests : ValidationBaseTests<DoesNotContainTemplatedValues>
    {
        public static IEnumerable XmlElements
        {
            get => new[]
            {
                "id",
                "version",
                "owners",
                "title",
                "authors",
                "projectUrl",
                "iconUrl",
                "copyright",
                "licenseUrl",
                "requireLicenseAcceptance",
                "projectSourceUrl",
                "docsUrl",
                "mailingListUrl",
                "bugTrackerUrl",
                "tags",
                "summary",
                "description",
                "releaseNotes",
            };
        }

        public override string ExpectedId { get; } = "choco0001";

        [Test]
        public void Should_FailValidationWhenTemplatedValuesArePresent(
            [ValueSource(nameof(XmlElements))] string element,
            [Values("__replace", "space_separated", "tag1")] string value)
        {
            var package = new Package
            {
                AllElements = new Dictionary<string, MetaValue<string>>
                {
                    { element, value }
                }
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenTemplatedValuesAreNotUsed(
            [ValueSource(nameof(XmlElements))] string element)
        {
            var package = new Package
            {
                AllElements = new Dictionary<string, MetaValue<string>>
                {
                    { element, "Some value" }
                }
            };

            var result = Rule.Validate(package).ToList();

            Assert.That(result, Is.Empty);
        }
    }
}