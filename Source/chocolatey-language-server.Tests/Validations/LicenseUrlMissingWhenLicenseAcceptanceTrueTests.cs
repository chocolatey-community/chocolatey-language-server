using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class LicenseUrlMissingWhenLicenseAcceptanceTrueTests : ValidationBaseTests<LicenseUrlMissingWhenLicenseAcceptanceTrue>
    {
        public override string ExpectedId { get; } = "choco0008";

        [Test]
        public void Should_FailValidationWhenUrlIsEmpytAndLicenseAcceptanceIsTrue(
            [Values(null, "", "     ")]string license)
        {
            var package = new Package
            {
                LicenseUrl = license,
                RequireLicenseAcceptance = true,
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenLicenseIsNotEmpty()
        {
            var package = new Package
            {
                LicenseUrl = "https://my-license.org/LICENSE.txt",
                RequireLicenseAcceptance = true,
            };

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Should_NotFailValidationWhenLicenseIsNotRequired()
        {
            var package = new Package
            {
                RequireLicenseAcceptance = false,
            };

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }
    }
}