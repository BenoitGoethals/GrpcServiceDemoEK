using FirstDemo.ServerSide.Services;
using Grpc.Core;
using Grpc.Net.Client;

GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7013");

ProductsService.ProductsServiceClient productsServiceClient = new(channel);

await UnaryExample(productsServiceClient);

await ServerStreamingExample(productsServiceClient);

await ClientStreamingExample(productsServiceClient);

DuplexStreaming(productsServiceClient);

await MetadataExample(productsServiceClient);

await DeadlineExample(productsServiceClient);

await CancelExample(productsServiceClient);

Console.ReadLine();

static async Task UnaryExample(ProductsService.ProductsServiceClient productsServiceClient) {
    Console.WriteLine("********** GetProductByIdAsync - Id == 1 ************");

    //AsyncUnaryCall<GetProductByIdResponse>? call = productsServiceClient.GetProductByIdAsync(new GetProductByIdRequest() { Id = 5 });
    //GetProductByIdResponse? resp = await call.ResponseAsync;
    //Metadata? headers = await call.ResponseHeadersAsync;

    GetProductByIdResponse? response = await productsServiceClient.GetProductByIdAsync(new GetProductByIdRequest() { Id = 5 });

    Console.WriteLine($"{response.Id} {response.Name} {response.Brand} {response.Price}");
}

static async Task ServerStreamingExample(ProductsService.ProductsServiceClient productsServiceClient) {
    Console.WriteLine("*********** GetProducts ***********");

    AsyncServerStreamingCall<GetProductsResponse> stream = productsServiceClient.GetProducts(new GetProductsRequest() { });

    await foreach (GetProductsResponse? res in stream.ResponseStream.ReadAllAsync()) {
        Console.WriteLine($"{res.Id} {res.Name} {res.Brand} {res.Price}");
    }
}

static async Task ClientStreamingExample(ProductsService.ProductsServiceClient productsServiceClient) {
    Console.WriteLine("*********** BatchUpdateProducts ***********");

    AsyncClientStreamingCall<BatchUpdateProductRequest, BatchUpdateProductResponse>? asyncClientStreamingCall = productsServiceClient.BatchUpdateProducts();
    for (uint i = 0; i < 10; i++) {
        await asyncClientStreamingCall.RequestStream.WriteAsync(new BatchUpdateProductRequest() { Id = i, Name = $"Product{i}", Brand = $"Brand{i}", Price = 12.34 * i });
    }
    await asyncClientStreamingCall.RequestStream.CompleteAsync();
    BatchUpdateProductResponse? resp = await asyncClientStreamingCall;
    Console.WriteLine($"Updated Products: {resp.UpdatedProducts}");
}

static void DuplexStreaming(ProductsService.ProductsServiceClient productsServiceClient) {
    Console.WriteLine("********** BatchInsert ************");
    AsyncDuplexStreamingCall<BatchInsertProductRequest, BatchInsertProductResponse>? asyncDuplexStreamingCall = productsServiceClient.BatchInsertProducts();

    Parallel.Invoke(async () => {
        for (int i = 0; i < 10; i++) {
            Console.WriteLine($"Sending Product {i}");
            await asyncDuplexStreamingCall.RequestStream.WriteAsync(new BatchInsertProductRequest() { Id = 0, Name = $"Product{i}", Brand = $"Brand{i}", Price = 12.34 * i });
        }
    },async () => {
        await foreach (var res in asyncDuplexStreamingCall.ResponseStream.ReadAllAsync()) {
            Console.WriteLine($"Received product: {res.Id} {res.Name} {res.Brand} {res.Price}");
        }
    });
}

static async Task MetadataExample(ProductsService.ProductsServiceClient productsServiceClient) {
    Metadata requestMetadata = new Metadata();
    requestMetadata.Add("one-metadata-key", "One Metadata Value");
    requestMetadata.Add("another-metadata-key", "Another Metadata Value");
    var unaryCall = productsServiceClient.UpdateProductAsync(new UpdateProductRequest() { Id = 1 }, requestMetadata);
    var res = await unaryCall;
    Metadata responseMetadata = unaryCall.GetTrailers();
    Console.WriteLine("Trailers from UpdateProductAsync: ");
    foreach (var item in responseMetadata) {
        Console.WriteLine($"{item.Key} - {item.Value}");
    }
}

static async Task DeadlineExample(ProductsService.ProductsServiceClient productsServiceClient) { 
    InsertProductRequest request = new InsertProductRequest() { 
        Id = 1,
        Brand = "The Brand",
        Name = "The Name",
        Price = 123.4
    };
    try {
        /*
         * https://docs.microsoft.com/en-us/aspnet/core/grpc/deadlines-cancellation?view=aspnetcore-6.0
           If a deadline is exceeded
           The client immediately aborts the underlying HTTP request 
           and throws a DeadlineExceeded error. 
           The client app can choose to catch the error 
           and display a timeout message to the user.
        */
        InsertProductResponse? response = await productsServiceClient.InsertProductAsync(request,
            deadline: DateTime.UtcNow.AddSeconds(5));
    } catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded) {
        Console.WriteLine($"Insert Product Timeout! {ex.Message}");
    }
}

static async Task CancelExample(ProductsService.ProductsServiceClient productsServiceClient) {
    Console.WriteLine("********** Cancel The Call ************");

    AsyncServerStreamingCall<DeleteProductResponse>? call = productsServiceClient.DeleteProduct(new DeleteProductRequest() { Id = 5 });

    Parallel.Invoke(async () => {
        try {
            await foreach (var item in call.ResponseStream.ReadAllAsync()) {
                Console.WriteLine(item.Name);
            }
        } catch (ObjectDisposedException ex) {
            Console.WriteLine("Call Canceled " + ex.Message);
        } catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled) {
            Console.WriteLine("Call Canceled " + ex.Message);
        }
    },
    () => {
        Console.WriteLine("PRESS ENTER TO CANCEL THE CALL");
        Console.ReadLine();
        call.Dispose();
    });


    //Console.WriteLine($"{response.Id} {response.Name} {response.Brand} {response.Price}");
}