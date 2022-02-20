using System;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.DataProcessors;
using CryptoExchange.Net.Objects;

namespace CryptoExchange.Net
{
    /// <summary>
    /// Base API for all API clients
    /// </summary>
    public abstract class BaseApiClient: IDisposable
    {
        private ApiCredentials? _apiCredentials;
        private AuthenticationProvider? _authenticationProvider;
        private bool _created;

        /// <summary>
        /// The authentication provider for this API client. (null if no credentials are set)
        /// </summary>
        public AuthenticationProvider? AuthenticationProvider
        {
            get 
            {
                if (!_created && _apiCredentials != null)
                {
                    _authenticationProvider = CreateAuthenticationProvider(_apiCredentials);
                    _created = true;
                }

                return _authenticationProvider;
            }
        }

        /// <summary>
        /// The base address for this API client
        /// </summary>
        internal protected string BaseAddress { get; }

        /// <summary>
        /// Api client options
        /// </summary>
        internal ApiClientOptions Options { get; }

        /// <summary>
        /// Processor for received data
        /// </summary>
        public IDataProcessor DataProcessor { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options">Client options</param>
        /// <param name="apiOptions">Api client options</param>
        /// <param name="dataProcessor">Processor for received data</param>
        protected BaseApiClient(BaseClientOptions options, ApiClientOptions apiOptions, IDataProcessor dataProcessor)
        {
            Options = apiOptions;
            _apiCredentials = apiOptions.ApiCredentials?.Copy() ?? options.ApiCredentials?.Copy();
            DataProcessor = dataProcessor;
            BaseAddress = apiOptions.BaseAddress;
        }

        /// <summary>
        /// Create an AuthenticationProvider implementation instance based on the provided credentials
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        protected abstract AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials);

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            _apiCredentials = credentials;
            _created = false;
            _authenticationProvider = null;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            AuthenticationProvider?.Credentials?.Dispose();
        }
    }
}
