using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class PrereleaseVersionAsPartOfPackageIdTests : ValidationBaseTests<PrereleaseVersionAsPartOfPackageId>
    {
        public override string ExpectedId { get; } = "choco0010";

        [Test]
        public void Should_FailValidationWhenPackageIdContainsPreReleaseVersion(
            [Values("{0}-my-id", "my-{0}id", "my-id-{0}")] string id,
            [Values("alpha", "beta", "prerelease")] string tag)
        {
            var package = new Package
            {
                Id = string.Format(id, tag),
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenPackageIdDoesNotContainPreReleaseVersion()
        {
            var package = new Package
            {
                Id = "chocolatey-language-server",
            };

            ValidateDiagnosticResult(package, 0);
        }
    }
}