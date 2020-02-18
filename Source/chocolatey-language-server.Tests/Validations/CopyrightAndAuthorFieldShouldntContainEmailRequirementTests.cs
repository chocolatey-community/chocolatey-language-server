using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public class CopyrightAndAuthorFieldShouldntContainEmailRequirementTests : ValidationBaseTests<CopyrightAndAuthorFieldShouldntContainEmailRequirement>
    {
        public static IEnumerable FailingAuthors
        {
            get
            {
                yield return new TestCaseData("bob@bob.co.uk", null);
                yield return new TestCaseData("Chocolatey", "bob@bob.co.uk");
                yield return new TestCaseData("bob@bob.co.uk", "kim@kim.no");
            }
        }

        public static IEnumerable FailingCopyrights
        {
            get
            {
                yield return new TestCaseData("bob@bob.co.uk");
            }
        }

        public override string ExpectedId { get; } = "choco0005";

        [TestCaseSource(nameof(FailingAuthors))]
        public void Should_FailValidationWhenEmailIsUsedInAuthorField(string author1, string author2)
        {
            var authors = new List<MetaValue<string>> { author1 };
            if (author2 != null) { authors.Add(author2); }

            var package = new Package
            {
                Authors = authors,
                Copyright = string.Empty
            };

            ValidateDiagnosticResult(package, authors.Count(a => a.Value.Contains('@')));
        }

        [TestCaseSource(nameof(FailingCopyrights))]
        public void Should_FailValidationWhenEmailIsUsedInCopyrightField(string copyright)
        {
            var package = new Package
            {
                Copyright = copyright,
            };

            ValidateDiagnosticResult(package, 1);
        }

        [Test]
        public void Should_NotFailValidationWhenEmailIsNotUsedInAuthors()
        {
            var package = new Package
            {
                Authors = new MetaValue<string>[] { "Chocolatey", "NuGet" }
            };

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Should_NotFailValidationWhenEmailIsNotUsedInCopyright()
        {
            var package = new Package
            {
                Copyright = "Copyright 2016"
            };

            var result = Rule.Validate(package);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Should_SetDiagnosticsForAllFoundEmailsInAuthors()
        {
            var package = new Package
            {
                Authors = new MetaValue<string>[] { "bob@bob.co.uk kim@kim.no" }
            };

            ValidateDiagnosticResult(package, 2);
        }

        [Test]
        public void Should_SetDiagnosticsForAllFoundEmailsInCopyright()
        {
            var package = new Package
            {
                Copyright = "bob@bob.co.uk something kim@kim.no"
            };

            ValidateDiagnosticResult(package, 2);
        }
    }
}