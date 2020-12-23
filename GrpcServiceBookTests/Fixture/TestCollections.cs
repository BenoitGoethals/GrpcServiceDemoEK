using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcServiceBook;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GrpcServiceBookTests.Fixture
{
    public static class TestCollections
    {
        public const string ApiIntegration = "ApiIntegration";
    }
    [CollectionDefinition(TestCollections.ApiIntegration)]
    public class TestServerCollection : ICollectionFixture<TestServerFixture>
    {
    }

    public sealed class TestServerFixture : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public TestServerFixture()
        {
            _factory = new WebApplicationFactory<Startup>();
            var client = _factory.CreateDefaultClient(new ResponseVersionHandler());
            GrpcChannel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
        }

        public GrpcChannel GrpcChannel { get; }

        public void Dispose()
        {
            _factory.Dispose();
        }

        private class ResponseVersionHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                var response = await base.SendAsync(request, cancellationToken);
                response.Version = request.Version;
                return response;
            }
        }
    }
}
