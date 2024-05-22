using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AFS_L33tSp34k3r_App.Models;
using AFS_L33tSp34k3r_App.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace AFS_L33tSp34k3r_App.Tests
{
    /// <summary>
    /// Test class for LeetTranslatoService
    /// </summary>
    public class LeetTranslatorServiceTests
    {
        /// <summary>
        /// Testing the (main) TranslateAsync method.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TranslateAsync_API_success()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            mockHttpMessageHandler.SetupResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"contents\":{\"translated\":\"1337 7r4n514710n\",\"text\":\"normal text\",\"translation\":\"leetspeak\"}}")
            });

            var httpClient = new HttpClient(mockHttpMessageHandler);
            var options = new LeetTranslatorServiceOptions { LogFilePath = "test.log" };

            var service = new LeetTranslatorService(httpClient, Options.Create(options));

            // Act
            var result = await service.TranslateAsync("normal text");

            // Assert
            Assert.Equal("1337 7r4n514710n", result);
        }


        /// <summary>
        /// Tests the (main) TranslateAsync method on api fail (testing the fallback method for translation basically)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TranslateAsyncFallback()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            mockHttpMessageHandler.SetupResponse(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("API error message")
            });

            var httpClient = new HttpClient(mockHttpMessageHandler);
            var options = new LeetTranslatorServiceOptions { LogFilePath = "test.log" };

            var service = new LeetTranslatorService(httpClient, Options.Create(options));

            // Act
            var result = await service.TranslateAsync("normal text");

            // Assert
            Assert.Equal("n0rm41 73x7", result);
        }

        /// <summary>
        /// Tests the <see cref="LeetTranslatorService.TranslateToLeet"/> method for correct translation to Leet Speak.
        /// </summary>
        [Fact]
        public void TranslationValidation()
        {
            // Arrange
            var service = new LeetTranslatorService(null, null);
            var input = "Hello World";
            var expected = "h3110 w0r1d";

            // Act
            var result = service.TranslateToLeetNative(input);

            // Assert
            Assert.Equal(expected, result);
        }


        /// <summary>
        /// Tests the <see cref="LeetTranslatorService.TranslateToLeet"/> method with an empty input string. A bit redundant tho since empty strings are acceptable.
        /// </summary>
        [Fact]
        public void TranslateToNullEmpty()
        {
            // Arrange
            var service = new LeetTranslatorService(null, null);
            var input = "";
            var expected = "";

            // Act
            var result = service.TranslateToLeetNative(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }





    // Helper class for mocking HttpMessageHandler
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _response;

        public void SetupResponse(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}