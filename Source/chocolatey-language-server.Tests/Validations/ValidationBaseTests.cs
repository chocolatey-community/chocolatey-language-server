using Chocolatey.Language.Server.Engine;
using Chocolatey.Language.Server.Models;
using Chocolatey.Language.Server.Validations;
using NUnit.Framework;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chocolatey.Language.Server.Tests.Validations
{
    public abstract class ValidationBaseTests<TValidationRule>
        where TValidationRule : INuspecRule, new()
    {
        public string ExpectedDocumentationUrl => $"https://gep13.github.io/chocolatey-vscode/docs/rules/{ExpectedId}";
        public abstract string ExpectedId { get; }

        public ValidationType ExpectedValidationType
        {
            get
            {
                if (ExpectedId.StartsWith("choco0"))
                {
                    return ValidationType.Requirement;
                }
                else if (ExpectedId.StartsWith("choco1"))
                {
                    return ValidationType.Guideline;
                }
                else if (ExpectedId.StartsWith("choco2"))
                {
                    return ValidationType.Suggestion;
                }
                else if (ExpectedId.StartsWith("choco3"))
                {
                    return ValidationType.Note;
                }
                else
                {
                    throw new AssertionException("The chocolatey id do not match any supported validation type");
                }
            }
        }

        public DiagnosticSeverity ExpectedSeverity
        {
            get
            {
                return ExpectedValidationType switch
                {
                    ValidationType.Requirement => DiagnosticSeverity.Error,
                    ValidationType.Guideline => DiagnosticSeverity.Warning,
                    ValidationType.Suggestion => DiagnosticSeverity.Information,
                    ValidationType.Note => DiagnosticSeverity.Hint,
                    _ => throw new NotSupportedException(),
                };
            }
        }

        protected INuspecRule Rule { get; private set; }

        [SetUp]
        public void SetUp()
        {
            var textPositions = new TextPositions("");
            var rule = new TValidationRule();

            typeof(NuspecRuleBase).GetProperty("TextPositions", BindingFlags.Static | BindingFlags.NonPublic).SetValue(rule, textPositions);

            Rule = rule;
        }

        [Test]
        public void Should_SetCorrectRuleId()
        {
            Assert.That(Rule.Id, Is.EqualTo(ExpectedId));
        }

        [Test]
        public void Should_SetSupportedIdFormat()
        {
            Assert.That(Rule.Id, Is.Not.Empty.And.Matches(@"^choco[0-3]\d{3}$"));
        }

        [Test]
        public void Should_SetCorrectValidationType()
        {
            Assert.That(Rule.ValidationType, Is.EqualTo(ExpectedValidationType));
        }

        [Test]
        public void Should_UseCorrectDocumentationUrl()
        {
            Assert.That(Rule.DocumentationUrl, Is.EqualTo(ExpectedDocumentationUrl));
        }

        protected IEnumerable<Diagnostic> ValidateDiagnosticResult(Package package, int expectedResults)
        {
            return ValidateDiagnosticResult(package, expectedResults, ExpectedSeverity);
        }

        protected IEnumerable<Diagnostic> ValidateDiagnosticResult(Package package, int expectedResults, DiagnosticSeverity expectedSeverity)
        {
            var result = Rule.Validate(package).ToList();

            Assert.That(result, Has.Count.EqualTo(expectedResults));
            Assert.That(result, Has.All.Property(nameof(Diagnostic.Severity)).EqualTo(expectedSeverity));
            Assert.That(result, Has.All.Property(nameof(Diagnostic.Code)).EqualTo((DiagnosticCode)ExpectedId));
            Assert.That(result, Has.All.Property(nameof(Diagnostic.Source)).EqualTo("chocolatey"));

            return result;
        }
    }
}