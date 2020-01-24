using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class CopyrightWordCountTests : ValidationBaseTests<CopyrightWordCount>
    {
        public override string ExpectedId { get; } = "choco0006";

        [Test]
        public void Should_FailValidationWhenCharacterCountIsEqualTo4()
        {
            var package = new Package
            {
                Copyright = "2020"
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_FailValidationWhenCharacterCountIsLessThan4()
        {
            var package = new Package
            {
                Copyright = "© m"
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenCharacterCountIsMoreThan4()
        {
            var package = new Package
            {
                Copyright = "© 2020"
            };

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }
    }
}