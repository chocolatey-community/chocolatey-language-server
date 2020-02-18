using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;
using System.Collections;
using System.Linq;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class AuthorDoesNotMatchMaintainerTests : ValidationBaseTests<AuthorDoesNotMatchMaintainer>
    {
        public override string ExpectedId { get; } = "choco3002";

        public static IEnumerable SameDadaValues
        {
            get
            {
                yield return new TestCaseData("AdmiringWorm", "AdmiringWorm");
                yield return new TestCaseData("admiringworm", "AdmiringWorm");
                yield return new TestCaseData("AdmiringWorm", "admiringworm");
                yield return new TestCaseData("ADMIRINGWORM", "AdmiringWorm");
                yield return new TestCaseData("AdmiringWorm", "ADMIRINGWORM");
            }
        }

        [TestCaseSource(nameof(SameDadaValues))]
        public void Should_FailValidationWhenAuthorAndOwnerIsSame(string author, string owner)
        {
            var package = new Package
            {
                Authors = new MetaValue<string>[] { author },
                Maintainers = new MetaValue<string>[] { owner }
            };

            ValidateDiagnosticResult(package, 2);
        }

        [TestCase("AdmiringWorm", "gep13")]
        [TestCase("mkevenaar", "steviecoaster")]
        public void Should_NotFailValidationWhenAuthonAndOwnerIsNotSame(string author, string owner)
        {
            var package = new Package
            {
                Authors = new MetaValue<string>[] { author },
                Maintainers = new MetaValue<string>[] { owner }
            };

            var result = Rule.Validate(package).ToList();

            Assert.That(result, Is.Empty);
        }
    }
}