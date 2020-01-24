using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class DescriptionRequiredValidationTests : ValidationBaseTests<DescriptionRequiredValidation>
    {
        public override string ExpectedId { get; } = "choco0002";

        [TestCase(null)]
        [TestCase("")]
        [TestCase("                   ")]
        public void Should_FailValidationWhenDescriptionIsNullOrWhitespace(string value)
        {
            var package = new Package
            {
                Description = value
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenDescriptionHaveText()
        {
            var package = new Package
            {
                Description = "My new awesome package description"
            };

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }
    }
}