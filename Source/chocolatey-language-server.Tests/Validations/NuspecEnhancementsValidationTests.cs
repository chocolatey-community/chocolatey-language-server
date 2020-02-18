using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;
using System.Collections.Generic;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class NuspecEnhancementsValidationTests : ValidationBaseTests<NuspecEnhancementsValidation>
    {
        public override string ExpectedId { get; } = "choco2001";

        [Test]
        public void Should_FailValidationWhenEnhancementUrlsIsNotUsed()
        {
            var package = new Package
            {
                AllElements = new Dictionary<string, MetaValue<string>>()
            };

            ValidateDiagnosticResult(package, 4);
        }

        [Test]
        public void Should_NotIncludeSpecifiedUrlElement(
            [Values("bugTrackerUrl", "docsUrl", "mailingListUrl", "projectSourceUrl")] string element)
        {
            var package = new Package
            {
                AllElements = new Dictionary<string, MetaValue<string>>
                {
                    { element, "https://some-kind-of-url.io" },
                },
            };

            var result = ValidateDiagnosticResult(package, 3);

            Assert.That(result, Is.All.Property("Message").Not.Contains(element));
        }

        [Test]
        public void Should_NotFailValidationWhenAllEnhancementsAreSpecified()
        {
            const string dummyUrl = "https://some-kind-of-url.io";
            var package = new Package
            {
                AllElements = new Dictionary<string, MetaValue<string>>
                {
                    { "bugTrackerUrl", dummyUrl },
                    { "docsUrl", dummyUrl },
                    { "mailingListUrl", dummyUrl },
                    { "projectSourceUrl", dummyUrl }
                }
            };

            ValidateDiagnosticResult(package, 0);
        }
    }
}