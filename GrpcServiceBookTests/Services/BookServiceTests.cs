using System;
using System.Threading.Tasks;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceBook;
using Xunit;
using XUnitTestProjectServiceUsers.Fixture;

namespace GrpcServiceBookTests.Services
{
    public class BookServiceTests : FunctionalTestBase
    {

        public BookServiceTests(GrpcServerFactory<Startup> factory) : base(factory)
        {

        }

        [Fact()]
        public void GetBooksTest()
        {
            var client=new LibBook.LibBookClient(_channel);
            var rt = client.Books(new Empty()).Should().NotBeNull();
        }


        [Fact()]
        public async Task InsertBookTest()
        {
            var client = new LibBook.LibBookClient(_channel);
            var rt = await client.insertBookAsync(new Book() {Genre = Genre.Novel, Isbn = "dfdsfdsfds",Author = "bbenoit",Language = "NL",Title = "sd",Pages = 1,Published = new Timestamp(),Id=0});
            rt.Id.Should().NotBe(null);
        }


        [Fact()]
        public void GetBookTest()
        {
            var client = new LibBook.LibBookClient(_channel);
            var rt = client.GetBook(new RequestIsbn() { Isbn = "16190610 7261" }).Id.Should().Be(2);
        }

        [Fact()]
        public void GetBookBadTest()
        {
            var client = new LibBook.LibBookClient(_channel);
            client.Invoking(y => y.GetBook(new RequestIsbn()))
                .Should().Throw<RpcException>();

        }
    }
}