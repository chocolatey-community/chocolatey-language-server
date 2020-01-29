using System;
using System.Net;

namespace Chocolatey.Language.Server.Extensions
{
    /// <summary>
    ///   Extensions for Uri
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        ///   Tries to validate an URL
        /// </summary>
        /// <param name="url">Uri object</param>
        /// <param name="useGetMethod">Bool Switch to GET request.</param>
        public static bool IsValid(this Uri url, bool useGetMethod = false)
        {
            if (url == null)
            {
                return true;
            }

            if (url.Scheme == "mailto")
            {
                // mailto links are not expected/allowed, therefore immediately fail with no further processing
                return false;
            }

            if (!url.Scheme.StartsWith("http"))
            {
                // Currently we can only validate http/https URL's, therefore simply return true for any others.
                return true;
            }

            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                var cookieContainer = new CookieContainer();

                request.CookieContainer = cookieContainer;
                //This would allow 301 and 302 to be valid as well
                request.AllowAutoRedirect = true;
                request.Timeout = 15000;
                if (!useGetMethod) request.Method = WebRequestMethods.Http.Head;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";

                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.SecureChannelFailure)
                {
                    //Since this is likely due to missing Ciphers on the machine running the language server, this URL will be marked as valid for the time being.
                    return true;
                }

                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Message == "The remote server returned an error: (503) Server Unavailable.")
                {
                    //This could be due to Cloudflare DDoS protection acting in front of the site, or another valid reason, as such, this URL will be marked as valid for the time being.
                    return true;
                }

                return !useGetMethod && url.IsValid(true);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///   Tries to validate if an URL is SSL capable.
        ///   HTTP: Will return true if the URL validates with SSL, otherwise false.
        ///   HTTPS: it returns false
        /// </summary>
        /// <param name="url">Uri object</param>
        public static bool SslCapable(this Uri url)
        {
            if (url.Scheme.Equals(Uri.UriSchemeHttps))
            {
                return false;
            }

            var uri = new UriBuilder(url);
            // Handle http: override the scheme and use the default https port
            uri.Scheme = Uri.UriSchemeHttps;
            uri.Port = -1;
            return uri.Uri.IsValid();
        }
    }
}
