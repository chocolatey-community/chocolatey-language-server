using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class PackageIdDoesNotEndWithConfigTests : ValidationBaseTests<PackageIdDoesNotEndWithConfig>
    {
        public override string ExpectedId { get; } = "choco0009";

        [Test]
        public void Should_FailValidationWhenPackageIdEndsWithConfig()
        {
            var package = new Package
            {
                Id = "my-my.config",
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenPackageIdDoesNotEndWithConfig()
        {
            var package = new Package
            {
                Id = "my-my",
            };

            ValidateDiagnosticResult(package, 0);
        }
    }
}