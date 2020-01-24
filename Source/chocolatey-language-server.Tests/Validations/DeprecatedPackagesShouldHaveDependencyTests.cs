using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class DeprecatedPackagesShouldHaveDependencyTests : ValidationBaseTests<DeprecatedPackagesShouldHaveDependency>
    {
        public override string ExpectedId { get; } = "choco0007";

        [Test]
        public void Should_FailValidationWhenNoDependencyIsSpecified()
        {
            var package = new Package
            {
                Title = "[deprecated] Some kind of title"
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenADependencyIsSpecified()
        {
            var package = new Package
            {
                Title = "[deprecated] Some kind of title"
            };
            package.AddDependency(new Dependency { Id = "some-package-id" });

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }
    }
}