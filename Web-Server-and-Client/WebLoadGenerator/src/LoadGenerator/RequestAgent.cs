using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace msantana.amaral.LoadGenerator
{
    public class RequestAgent
    {
        public RequestAgent(HttpClient httpClient, string serverUrl, string authKey, string userName, Program programReference, System.Collections.Concurrent.ConcurrentQueue<RequestResult> resultsOutput)
        {
            HttpClientInstance = httpClient;
            ServerUrl = new Uri(serverUrl);
            AuthKey = authKey;
            UserName = userName;
            ProgramReference = programReference;
            ResultsOutput = resultsOutput;
        }

        #region Private Properties

        private HttpClient HttpClientInstance { get; }
        private Uri ServerUrl { get; }
        private string AuthKey { get; }
        private string UserName { get; }
        private Program ProgramReference { get; }
        private ConcurrentQueue<RequestResult> ResultsOutput { get; }

        #endregion Private Properties

        public async Task Start(CancellationToken cancelToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            while (cancelToken.IsCancellationRequested == false)
            {
                stopwatch.Reset();
                stopwatch.Start();
                int nextRequestNumber = Interlocked.Increment(ref ProgramReference.RequestNumber);
                var requestResult = await DoRequestAsync(nextRequestNumber);
                ResultsOutput.Enqueue(requestResult);
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                if (elapsedMilliseconds < 1_000)
                {
                    await Task.Delay(Convert.ToInt32(1_000 - elapsedMilliseconds));
                }
            }
        }

        private async Task<RequestResult> DoRequestAsync(int requestNumber)
        {
            try
            {
                Payload payload = new Payload(UserName, requestNumber);
                string payloadStr = Newtonsoft.Json.JsonConvert.SerializeObject(payload, Newtonsoft.Json.Formatting.None);

                var request = new HttpRequestMessage(HttpMethod.Post, ServerUrl);
                request.Headers.Clear();
                request.Content = new StringContent(payloadStr, Encoding.UTF8, "application/json");
                request.Headers.Add("X-Api-Key", AuthKey);
                request.Headers.Add("UserName", UserName);

                var responseMessage = await HttpClientInstance.SendAsync(request);
                return await ComputeResultAsync(responseMessage);
            }
            catch (Exception exOnDoRequestAsync)
            {
                RequestResult requestResult = new RequestResult()
                {
                    Successful = false,
                    ErrorMessage = exOnDoRequestAsync.Message,
                };
                return requestResult;
            }
        }

        public async Task<RequestResult> ComputeResultAsync(HttpResponseMessage responseMessage)
        {
            RequestResult requestResult = new RequestResult();
            string responseContent = null;
            requestResult.StatusCode = responseMessage.StatusCode;
            if (responseMessage.IsSuccessStatusCode && responseMessage.Content != null)
            {
                try
                {
                    responseContent = await responseMessage.Content.ReadAsStringAsync();
                    var response = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(responseContent);
                    requestResult.Successful = response.successful;
                }
                catch (Exception)
                {
                    requestResult.ErrorMessage = responseContent;
                    requestResult.Successful = false;
                }
            }
            else
            {
                if (responseMessage.Content != null) responseContent = await responseMessage.Content.ReadAsStringAsync();
                requestResult.Successful = false;
                requestResult.ErrorMessage = responseContent ?? responseMessage.ReasonPhrase;
            }
            return requestResult;
        }
    }
}
