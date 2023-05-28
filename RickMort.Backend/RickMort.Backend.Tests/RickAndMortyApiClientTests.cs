using FluentAssertions;
using Moq;
using Moq.Protected;
using RickMort.Backend.Api.Models;
using RickMort.Backend.Api.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RickMort.Backend.Tests
{
    public class RickAndMortyApiClientTests
    {
        [Fact]
        public async Task GetEpisodesAsync_ReturnsEpisodeResult()
        {
            // Arrange
            var episodeResult = new EpisodeResult
            {
                Info = new Info
                {
                    Count = 1,
                    Pages = 1,
                    Next = null,
                    Prev = null
                },
                Results = new List<Episode>
                {
                    new Episode
                    {
                        Id = 1,
                        Name = "Pilot",
                        AirDate = "December 2, 2013",
                        EpisodeCode = "S01E01",
                        Characters = new List<string>(),
                        Url = "https://rickandmortyapi.com/api/episode/1",
                        Created = DateTime.Parse("2017-11-10T12:56:33.798Z")
                    }
                }
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(episodeResult))
            };

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            messageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(messageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://rickandmortyapi.com/")
            };

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var rickAndMortyApiClient = new RickAndMortyApiClient(httpClientFactoryMock.Object.CreateClient());

            // Act
            var result = await rickAndMortyApiClient.GetEpisodesAsync(2);

            // Assert
            result.Should().NotBeNull();
            result.Info.Should().NotBeNull();
            result.Info.Count.Should().Be(1);
            result.Info.Pages.Should().Be(1);
            result.Info.Next.Should().BeNull();
            result.Info.Prev.Should().BeNull();
            result.Results.Should().HaveCount(1);
            result.Results[0].Name.Should().Be("Pilot");
            result.Results[0].EpisodeCode.Should().Be("S01E01");
        }

        [Fact]
        public async Task GetEpisodesAsync_ThrowsException_OnErrorStatusCode()
        {
            // Arrange
            var errorResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ReasonPhrase = "Internal Server Error"
            };

            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            messageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(errorResponse);

            var httpClient = new HttpClient(messageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://rickandmortyapi.com/")
            };

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var rickAndMortyApiClient = new RickAndMortyApiClient(httpClientFactoryMock.Object.CreateClient());

            // Act
            Func<Task> act = async () => await rickAndMortyApiClient.GetEpisodesAsync();

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Failed to fetch episodes:*API returned 500 - InternalServerError");
        }

        [Fact]
        public async Task GetEpisodesAsync_ThrowsException_OnException()
        {
            // Arrange
            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            messageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Failed to connect to the server"));

            var httpClient = new HttpClient(messageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://rickandmortyapi.com/")
            };

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var rickAndMortyApiClient = new RickAndMortyApiClient(httpClientFactoryMock.Object.CreateClient());

            // Act
            Func<Task> act = async () => await rickAndMortyApiClient.GetEpisodesAsync();

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Failed to fetch episodes: Failed to connect to the server");
        }
    }
}