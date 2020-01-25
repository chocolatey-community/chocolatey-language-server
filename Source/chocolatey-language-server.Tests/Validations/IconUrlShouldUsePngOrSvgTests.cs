using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class IconUrlShouldUsePngOrSvgTests : ValidationBaseTests<IconUrlShouldUsePngOrSvg>
    {
        public override string ExpectedId { get; } = "choco2002";

        [Test]
        public void Should_FailValidationWhenExtensionIsNotPngOrSvg(
            [Values("", ".jpg", ".bmp", ".tiff")] string extension)
        {
            var package = new Package
            {
                IconUrl = "https://some_url.org/package" + extension
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenCorrectExtensionIsUsed(
            [Values(".png", ".svg")] string extension)
        {
            var package = new Package
            {
                IconUrl = "https://some_url.org/package" + extension,
            };

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Should_NotFailValidationWhenIconUrlIsEmpty(
            [Values(null, "", "        ")] string url)
        {
            var package = new Package
            {
                IconUrl = url,
            };

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }
    }
}