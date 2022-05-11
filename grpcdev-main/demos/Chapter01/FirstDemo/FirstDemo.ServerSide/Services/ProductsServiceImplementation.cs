using Grpc.Core;

namespace FirstDemo.ServerSide.Services {
    public class ProductsServiceImplementation : ProductsService.ProductsServiceBase {
        private readonly ILogger<ProductsServiceImplementation> logger;

        public ProductsServiceImplementation(ILogger<ProductsServiceImplementation> logger) {
            this.logger = logger;
        }
        public override Task<GetProductByIdResponse> GetProductById(GetProductByIdRequest request, ServerCallContext context) {
            logger.LogInformation($"GetProductById got {request.Id}");
            GetProductByIdResponse response = new GetProductByIdResponse() { Id = request.Id, Name = $"Product{request.Id}", Brand = $"Brand{request.Id}", Price = 12.34 * request.Id};
            return Task.FromResult(response);
        }

        public override async Task GetProducts(GetProductsRequest request, IServerStreamWriter<GetProductsResponse> responseStream, ServerCallContext context) {
            logger.LogInformation($"GetProducts");
            for (uint i = 0; i < 10; i++) {
                GetProductsResponse response = new GetProductsResponse() { Id = i, Name = $"Product{i}", Brand = $"Brand{i}", Price = 12.34 * i };
                logger.LogInformation($"GetProducts sending {response.Id}");
                await responseStream.WriteAsync(response);
            }
        }

        public override async Task<BatchUpdateProductResponse> BatchUpdateProducts(IAsyncStreamReader<BatchUpdateProductRequest> requestStream, ServerCallContext context) {
            logger.LogInformation($"BatchUpdateProducts");
            uint counter = 0;
            await foreach (BatchUpdateProductRequest? req in requestStream.ReadAllAsync()) {
                logger.LogInformation($"BatchUpdateProducts got {req.Id}");
                counter++;
            }
            logger.LogInformation($"BatchUpdateProducts sending {counter}");
            return new BatchUpdateProductResponse() { UpdatedProducts = counter };
        }

        public override async Task BatchInsertProducts(IAsyncStreamReader<BatchInsertProductRequest> requestStream, IServerStreamWriter<BatchInsertProductResponse> responseStream, ServerCallContext context) {
            logger.LogInformation($"BatchInsertProducts");
            uint id = 0;
            await foreach (var req in requestStream.ReadAllAsync()) {
                logger.LogInformation($"BatchInsertProducts got {req.Id}");
                logger.LogInformation($"BatchInsertProducts sending {id++}");
                await responseStream.WriteAsync(new BatchInsertProductResponse() { Id = id, Brand = req.Brand, Name = req.Name, Price = req.Price });
            }
        }

        public override Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context) {
            Metadata metadataSentByClient = context.RequestHeaders;
            foreach (var item in metadataSentByClient) {
                logger.LogInformation($"key: {item.Key}, value: {item.Value}");
            }
            
            for (int i = 0; i < 5; i++) {
                context.ResponseTrailers.Add($"additional-data-{i}", $"Some value {i}");
            }

            return Task.FromResult(new UpdateProductResponse() { });
        }

        public override async Task<InsertProductResponse> InsertProduct(InsertProductRequest request, ServerCallContext context) {
            //https://docs.microsoft.com/en-us/aspnet/core/grpc/deadlines-cancellation?view=aspnetcore-6.0
            logger.LogInformation($"InsertProduct has this deadline: {context.Deadline}");
            /*
             * If a deadline is exceeded
            On the server, the executing HTTP request is aborted 
            and ServerCallContext.CancellationToken is raised. 
            Although the HTTP request is aborted, 
            the gRPC call continues to run on the server until the method completes. 
            It's important that the cancellation token is passed to async methods 
            so they are cancelled along with the call. 
            For example, passing a cancellation token to async database queries 
            and HTTP requests. Passing a cancellation token allows 
            the canceled call to complete quickly on the server 
            and free up resources for other calls.
            */
            
            await Task.Delay(-1, context.CancellationToken);
            return new InsertProductResponse() { Id = request.Id, Brand = request.Brand, Name = request.Name, Price = request.Price };
        }

        public override async Task DeleteProduct(DeleteProductRequest request, IServerStreamWriter<DeleteProductResponse> responseStream, ServerCallContext context) {
            logger.LogInformation($"The client could cancel the call at any time... {request.Id}");
            uint i = 0;
            try {
                while (true) {
                    await responseStream.WriteAsync(new DeleteProductResponse() { Id = i++, Name = $"Product{i}" });
                    await Task.Delay(1000);
                }
            } catch (InvalidOperationException ex) {
                logger?.LogError(ex, "Problem server side!");
            }
        }
    }
}
