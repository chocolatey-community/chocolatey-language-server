using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class UrlSslCapableTests : ValidationBaseTests<UrlSslCapable>
    {
        public static IEnumerable SupportedElements
        {
            get => new[]
            {
                "bugTrackerUrl",
                "docsUrl",
                "iconUrl",
                "licenseUrl",
                "mailingListUrl",
                "packageSourceUrl",
                "projectSourceUrl",
                "projectUrl",
                "wikiUrl"
            };
        }

        public override string ExpectedId { get; } = "choco1002";

        [Test]
        public void Should_FailValidationWhenSslCanBeUsedOnUrl([ValueSource(nameof(SupportedElements))] string xmlElement)
        {
            var package = new Package
            {
                AllElements = new Dictionary<string, MetaValue<string>>
                {
                    { xmlElement, "http://chocolatey.org" }
                }
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenSslIsUsedOnUrl([ValueSource(nameof(SupportedElements))] string xmlElement)
        {
            var package = new Package
            {
                AllElements = new Dictionary<string, MetaValue<string>>
                {
                    { xmlElement, "https://chocolatey.org" }
                }
            };

            ValidateDiagnosticResult(package, 0);
        }

        // TODO: Validation for when a url is not ssl capable should also be tested.
    }
}