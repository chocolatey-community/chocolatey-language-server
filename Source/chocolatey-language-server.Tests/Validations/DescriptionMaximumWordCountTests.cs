using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class DescriptionMaximumWordCountTests : ValidationBaseTests<DescriptionMaximumWordCount>
    {
        public override string ExpectedId { get; } = "choco0003";

        [Test]
        public void Should_FailValidationWhenDescriptionIsLongerThan4000Characters()
        {
            var package = new Package
            {
                Description = "Hello".PadLeft(4001, 'x'),
            };

            ValidateDiagnosticResult(package, 1);
        }

        [TestCase(100)]
        [TestCase(3990)]
        [TestCase(4000, TestName = "Should_NotFailValidationWhenDescriptionIsEqualTo4000Characters")]
        public void Should_NotFailValidationWhenDescriptionIsLessThan4000Characters(int length)
        {
            var package = new Package
            {
                Description = "Hello".PadLeft(length, 'x')
            };

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }
    }
}