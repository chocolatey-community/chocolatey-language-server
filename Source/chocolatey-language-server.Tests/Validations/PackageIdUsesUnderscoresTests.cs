using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class PackageIdUsesUnderscoresTests : ValidationBaseTests<PackageIdUsesUnderscores>
    {
        public override string ExpectedId { get; } = "choco3001";

        [Test]
        public void Should_FailValidationWhenPackageIdContainsUnderscore()
        {
            var package = new Package
            {
                Id = "my_awesome_package_id",
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenPackageIdDoesNotContainUnderscore()
        {
            var package = new Package
            {
                Id = "again-my-awesome-package-id",
            };

            ValidateDiagnosticResult(package, 0);
        }
    }
}