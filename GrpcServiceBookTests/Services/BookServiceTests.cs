using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
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
        public void GetBookTest()
        {
            var client = new LibBook.LibBookClient(_channel);
            var rt = client.GetBook(new RequestIsbn() {Isbn = "16190610 7261"}).Id.Should().Be(2);
        }
    }
}