# Examples of different rpcs and messages

# GetProductById
In this example we show a simple unary call.  
The request is a message of type `GetProductByIdRequest` and the response is a message of type `GetProductByIdResponse`.  
The request is sent to the server and the response is received.  
The `GetProductByIdRequest` contains the Id of the product.
The `GetProductByIdResponse` contains the Id, Name,  Brand and Price of the product.

# GetProducts
In this example we show a simple server-streaming call.
The request is a message of type `GetProductsRequest` and the response is a message of type `GetProductsResponse`.
The request is sent to the server and the response is received as a stream of products.
The `GetProductsRequest` is empty.
The `GetProductsResponse` contains the Id, Name,  Brand and Price of the product.

# BatchUpdateProducts
In this example we show a simple client-streaming call.
The request is a message of type `BatchUpdateProductsRequest` and the response is a message of type `BatchUpdateProductsResponse`.
The request is sent as a stream of products, then one answer is received.
The `BatchUpdateProductsRequest` contains the Id, Name,  Brand and Price of the product.
The `BatchUpdateProductsResponse` contains the number of updated products.

# BatchInsertProducts
In this example we show a simple bidirectional streaming call.
The request is a message of type `BatchInsertProductsRequest` and the response is a message of type `BatchInsertProductsResponse`.
The request is sent as a stream of products, while in the meantime a stream of responses is read in a parallel thread.
The `BatchInsertProductsRequest` contains the Id, Name,  Brand and Price of the product.
The `BatchInsertProductsResponse` contains the Id, Name,  Brand and Price of the inserted product.

# UpdateProduct
In this example we show a simple unary call, but we use the metadata to send and receive additional information.  
The client adds two keys and values to the request metadata.
The server finds the metadata in the context.RequestHeaders.  
The server adds five keys and values in the context.ResponseTrailers.
The client reads the Trailers by invoking the GetTrailers method of the unary call.

# InsertProduct
In this example we show how to set a deadline.  
The client sets a deadline of 5 second and is ready to eventually catch an RpcException with a StatusCode of DeadlineExceeded.  
The server can check the deadline in the Deadline property of the context.  
The server waits indefinitely before sending the response. This causes the exception on the client side.

# DeleteProduct
In this example we show how to cancel a call.
The client cancels the call by invoking the call.Dispose() method.
This causes an exception client side, which is promptly caught.
The server gets an exception of type InvalidOperationException, which is promptly caught.

