using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class DescriptionMinimumWordCountTests : ValidationBaseTests<DescriptionMinimumWordCount>
    {
        public override string ExpectedId { get; } = "choco1001";

        [TestCase(6)]
        [TestCase(10)]
        [TestCase(30, TestName = "Should_FailValidationWhenDescriptionIsEqualTo30Characters")]
        public void Should_FailValidationWhenDescriptionIsLessThan30Characters(int length)
        {
            var package = new Package
            {
                Description = "Hello".PadLeft(length, 'x')
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenDescriptionIsMoreThan30Characters()
        {
            var package = new Package
            {
                Description = "Yo".PadLeft(31, 'x')
            };

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }
    }
}