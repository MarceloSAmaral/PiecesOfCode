using msantana.amaral.LoadGenerator;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class RequestAgentTests
    {
        [Fact(DisplayName = "RequestAgent.ComputeResultAsync: Recognizes a successful HttpResponseMessage.")]
        public async Task RequestAgent_ComputeResultAsync_RecognizesASuccessfulHttpResponseMessage()
        {
            RequestAgent requestAgent = CreateRequestAgent();
            var requestResponse = CreateSuccessHttpResponseMessage();

            var requestResult = await requestAgent.ComputeResultAsync(requestResponse);

            Assert.True(requestResult.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(requestResult.Successful);
            Assert.Null(requestResult.ErrorMessage);
        }

        [Fact(DisplayName = "RequestAgent.ComputeResultAsync: Recognizes a failure based on StatusCode.")]
        public async Task RequestAgent_ComputeResultAsync_RecognizesAFailurebasedOnStatusCode()
        {
            RequestAgent requestAgent = CreateRequestAgent();
            var requestResponse = CreateFailureHttpResponseMessage();

            var requestResult = await requestAgent.ComputeResultAsync(requestResponse);

            Assert.True(requestResult.StatusCode == System.Net.HttpStatusCode.NotFound);
            Assert.True(requestResult.Successful == false);
        }

        [Fact(DisplayName = "RequestAgent.ComputeResultAsync: StatusCode has a higher priorty than message content.")]
        public async Task RequestAgent_ComputeResultAsync_StatusCodeHasHigherPriorityThanMessageContent()
        {
            RequestAgent requestAgent = CreateRequestAgent();
            var requestResponse = CreateSuccessHttpResponseMessage();
            requestResponse.StatusCode = System.Net.HttpStatusCode.NotFound;

            var requestResult = await requestAgent.ComputeResultAsync(requestResponse);

            Assert.True(requestResult.StatusCode == System.Net.HttpStatusCode.NotFound);
            Assert.True(requestResult.Successful == false);
        }

        [Fact(DisplayName = "RequestAgent.ComputeResultAsync: Recognizes a failure based on message content.")]
        public async Task RequestAgent_ComputeResultAsync_RecognizesFailureFromMessageContent()
        {
            RequestAgent requestAgent = CreateRequestAgent();
            var requestResponse = CreateFailureHttpResponseMessage();
            requestResponse.StatusCode = System.Net.HttpStatusCode.OK;

            var requestResult = await requestAgent.ComputeResultAsync(requestResponse);

            Assert.True(requestResult.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(requestResult.Successful == false);
            Assert.Null(requestResult.ErrorMessage);
        }

        [Fact(DisplayName = "RequestAgent.ComputeResultAsync: Empty message means error.")]
        public async Task RequestAgent_ComputeResultAsync_EmptyMessageMeansError()
        {
            RequestAgent requestAgent = CreateRequestAgent();
            var requestResponse = CreateSuccessHttpResponseMessage();
            requestResponse.Content = new StringContent(String.Empty, System.Text.Encoding.UTF8, "application/json");

            var requestResult = await requestAgent.ComputeResultAsync(requestResponse);

            Assert.True(requestResult.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(requestResult.Successful == false);
            Assert.True(String.IsNullOrEmpty(requestResult.ErrorMessage));
        }

        [Fact(DisplayName = "RequestAgent.ComputeResultAsync: Null content means error.")]
        public async Task RequestAgent_ComputeResultAsync_NullContentMeansError()
        {
            RequestAgent requestAgent = CreateRequestAgent();
            var requestResponse = CreateSuccessHttpResponseMessage();
            requestResponse.Content = null;

            var requestResult = await requestAgent.ComputeResultAsync(requestResponse);

            Assert.True(requestResult.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(requestResult.Successful == false);
        }

        [Fact(DisplayName = "RequestAgent.ComputeResultAsync: When message is not in the expected format then computes error.")]
        public async Task RequestAgent_ComputeResultAsync_WhenMessageIsNotInExpectedFormatThenComputesError()
        {
            RequestAgent requestAgent = CreateRequestAgent();
            var requestResponse = CreateSuccessHttpResponseMessage();
            requestResponse.Content = new StringContent("{ \"ThisIsNotOK\": true}", System.Text.Encoding.UTF8, "application/json");

            var requestResult = await requestAgent.ComputeResultAsync(requestResponse);

            Assert.True(requestResult.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(requestResult.Successful == false);
            Assert.Null(requestResult.ErrorMessage);
        }

        private static RequestAgent CreateRequestAgent()
        {
            return new RequestAgent(null, "http://127.0.0.1", null, null, null, null);
        }

        private static HttpResponseMessage CreateSuccessHttpResponseMessage()
        {
            Response successMessage = new Response() { successful = true };
            HttpResponseMessage successfulRequestResponse = new HttpResponseMessage();
            successfulRequestResponse.StatusCode = System.Net.HttpStatusCode.OK;
            successfulRequestResponse.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(successMessage), System.Text.Encoding.UTF8, "application/json");
            return successfulRequestResponse;
        }

        private static HttpResponseMessage CreateFailureHttpResponseMessage()
        {
            Response successMessage = new Response() { successful = false };
            HttpResponseMessage successfulRequestResponse = new HttpResponseMessage();
            successfulRequestResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            successfulRequestResponse.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(successMessage), System.Text.Encoding.UTF8, "application/json");
            return successfulRequestResponse;
        }
    }
}
