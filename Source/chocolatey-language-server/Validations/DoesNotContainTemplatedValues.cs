using System;
using System.Collections.Generic;
using System.Linq;
using Chocolatey.Language.Server.Models;
using Microsoft.Language.Xml;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using DiagnosticSeverity = OmniSharp.Extensions.LanguageServer.Protocol.Models.DiagnosticSeverity;

namespace Chocolatey.Language.Server.Validations
{
    /// <summary>
    ///   Handler to validate that no templated values remain in the nuspec.
    /// </summary>
    /// <seealso href="https://github.com/chocolatey/package-validator/blob/master/src/chocolatey.package.validator/infrastructure.app/rules/NuspecDoesNotContainTemplatedValuesRequirement.cs">Package validator requirement for templated values.</seealso>
    public class DoesNotContainTemplatedValues : NuspecRuleBase
    {
        /// <summary>
        /// Gets the string Id for the rule, similar to choco0001
        /// </summary>
        public override string Id
        {
            get
            {
                return "choco0001";
            }
        }

        /// <summary>
        /// Gets the type of of validation
        /// </summary>
        public override ValidationType ValidationType
        {
            get
            {
                return ValidationType.Requirement;
            }
        }

        private static readonly IReadOnlyCollection<string> TemplatedValues = new []
        {
            "__replace",
            "space_separated",
            "tag1"
        };

        public override IEnumerable<Diagnostic> Validate(Package package)
        {
            foreach (var element in package.AllElements
                .Where(e => TemplatedValues.Any(t =>
                    string.Equals(e.Value, t, StringComparison.OrdinalIgnoreCase))))
            {

                yield return CreateDiagnostic(
                    element.Value,
                    "Templated value which should be removed.");
            }
        }
    }
}
