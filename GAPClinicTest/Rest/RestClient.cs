using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GAPClinicTest.Rest
{
    public class RestClient
    {

        string Toke;
        JsonSerializerSettings _deserializationSettings;
        public RestClient(string toke)
        {
            this.Toke = toke;

            _deserializationSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,

                //ContractResolver = new ReadOnlyJsonContractResolver(),
                //Converters = new List<JsonConverter>
                //    {
                //        new Iso8601TimeSpanConverter()
                //    }
            };
        }

        private async Task<HttpResponseMessage> Call(string url, HttpMethod method, CancellationToken cancellationToken, object data = null)
        {
            using (var httpClient = new HttpClient())
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (method == HttpMethod.Get || method == HttpMethod.Delete)
                {
                    if (data != null)
                    {
                        var query = data.ToString();
                        url += "?" + query;
                    }
                }

                using (var request = new HttpRequestMessage { RequestUri = new Uri(url), Method = method })
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (!string.IsNullOrEmpty(Toke))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", this.Toke);
                    }




                    cancellationToken.ThrowIfCancellationRequested();
                    if (method != HttpMethod.Get)
                    {
                        var jsonContent = JsonConvert.SerializeObject(data);
                        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        request.Content.Headers.ContentType =
                            new System.Net.Http.Headers.MediaTypeHeaderValue(
                            "application/json");                     
                    }

                    request.Headers.TryAddWithoutValidation(
                          "x-custom-header", "value");

                    HttpResponseMessage response = new HttpResponseMessage();
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    return response;
                }

            }
        }

        public async Task<TResult> MakeApiCall<TResult>(string url, HttpMethod method, object data = null, CancellationToken cancellationToken = default(CancellationToken)) where TResult : class
        {
            try
            {
                HttpResponseMessage response = await Call(url, method, cancellationToken, data);
                cancellationToken.ThrowIfCancellationRequested();
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {


                    try
                    {
                        var result = JsonConvert.DeserializeObject<TResult>(responseContent, _deserializationSettings);
                        return result;
                    }
                    catch (JsonException ex)
                    {
                        if (response != null)
                        {
                            response.Dispose();
                        }
                        throw new SerializationException("Unable to deserialize the response: "  + ex.Message , ex);
                    }


                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new AuthenticationException();                       
                    }


                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var excep = new ValidationException(responseContent);
                        throw excep;
                    }
                    else
                    {
                        throw new Exception(responseContent);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TResult> MakeApiCallRaw<TResult>(string url, HttpMethod method, object data = null, CancellationToken cancellationToken = default(CancellationToken)) where TResult : struct
        {
            try
            {
                HttpResponseMessage response = await Call(url, method, cancellationToken, data);
                cancellationToken.ThrowIfCancellationRequested();
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {


                    try
                    {
                        var result = JsonConvert.DeserializeObject<TResult>(responseContent, _deserializationSettings);
                        return result;
                    }
                    catch (JsonException ex)
                    {
                        if (response != null)
                        {
                            response.Dispose();
                        }
                        throw new SerializationException("Unable to deserialize the response.", ex);
                    }


                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(TResult);
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new AuthenticationException();
                    }


                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var excep = new ValidationException(responseContent);
                        throw excep;
                    }
                    else
                    {
                        throw new Exception(responseContent);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> MakeApiCall(string url, HttpMethod method, object data = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                HttpResponseMessage response = await Call(url, method, cancellationToken, data);
                cancellationToken.ThrowIfCancellationRequested();
                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent || response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return true;
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new AuthenticationException();
                    }

                    var stringSerialized = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var excep = new ValidationException(stringSerialized);
                        throw excep;
                    }
                    else
                    {
                        throw new Exception(stringSerialized);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}