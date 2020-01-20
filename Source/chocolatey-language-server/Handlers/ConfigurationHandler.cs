using Chocolatey.Language.Server.CustomProtocol;
using Chocolatey.Language.Server.Engine;
using Chocolatey.Language.Server.Extensions;
using Chocolatey.Language.Server.Handlers;
using OmniSharp.Extensions.Embedded.MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chocolatey.Language.Server.Handlers
{
    /// <summary>
    ///     Language Server message handler that tracks configuration.
    /// </summary>
    public sealed class ConfigurationHandler
        : IDidChangeConfigurationSettingsHandler, IConfigurationProvider
    {
        /// <summary>
        ///     The JSON serialiser used to read settings from LSP notifications.
        /// </summary>
        /// <returns></returns>
        readonly JsonSerializer _settingsSerializer = new JsonSerializer();

        /// <summary>
        ///     Create a new <see cref="ConfigurationHandler"/>.
        /// </summary>
        /// <param name="configuration">
        ///     The language server configuration.
        /// </param>
        public ConfigurationHandler(Configuration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            Configuration = configuration;
        }

        /// <summary>
        ///     Raised when configuration has changed.
        /// </summary>
        public event EventHandler<EventArgs> ConfigurationChanged;

        /// <summary>
        ///     The language server configuration.
        /// </summary>
        public Configuration Configuration { get; }

        /// <summary>
        ///     Has the client supplied configuration capabilities?
        /// </summary>
        bool HaveConfigurationCapabilities => ConfigurationCapabilities != null;

        /// <summary>
        ///     The client's configuration capabilities.
        /// </summary>
        DidChangeConfigurationCapability ConfigurationCapabilities { get; set; }

        /// <summary>
        ///     Called when configuration has changed.
        /// </summary>
        /// <param name="parameters">
        ///     The notification parameters.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/> representing the operation.
        /// </returns>
        Task<Unit> OnDidChangeConfiguration(DidChangeConfigurationObjectParams parameters)
        {
            Configuration.UpdateFrom(parameters);

            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(this, EventArgs.Empty);
            }

            return Unit.Task;
        }

        /// <summary>
        ///     Called to inform the handler of the language server's configuration capabilities.
        /// </summary>
        /// <param name="capabilities">
        ///     A <see cref="SynchronizationCapability"/> data structure representing the capabilities.
        /// </param>
        void ICapability<DidChangeConfigurationCapability>.SetCapability(DidChangeConfigurationCapability capabilities)
        {
            ConfigurationCapabilities = capabilities;
        }

        /// <summary>
        ///     Unused.
        /// </summary>
        /// <returns>
        ///     <c>null</c>
        /// </returns>
        object IRegistration<object>.GetRegistrationOptions()
        {
            return null;
        }

        /// <summary>
        ///     Add log context for an operation.
        /// </summary>
        /// <param name="operationName">
        ///     The operation name.
        /// </param>
        /// <returns>
        ///     An <see cref="IDisposable"/> representing the log-context scope.
        /// </returns>
        IDisposable BeginOperation(string operationName)
        {
            if (String.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentException("Argument cannot be null, empty, or entirely composed of whitespace: 'operationName'.", nameof(operationName));
            }

            return Serilog.Context.LogContext.PushProperty("Operation", operationName);
        }

        /// <summary>
        ///     Handle a change in configuration.
        /// </summary>
        /// <param name="request">The notification request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task"/> representing the operation.
        /// </returns>
        Task<Unit> IRequestHandler<DidChangeConfigurationObjectParams, Unit>.Handle(DidChangeConfigurationObjectParams request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            using (BeginOperation("OnDidChangeConfiguration"))
            {
                try
                {
                    return OnDidChangeConfiguration(request);
                }
                catch (Exception unexpectedError)
                {
                    Log.Error(unexpectedError, "Unhandled exception in {Method:l}.", "OnDidChangeConfiguration");
                    return Unit.Task;
                }
            }
        }
    }
}
