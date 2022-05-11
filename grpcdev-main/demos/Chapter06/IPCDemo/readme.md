# Inter Process Communication

In this demo, we show how to configure Kestrel to use a LinuxUnixSocket instead of Http, then how to configure a client to use the same SocketPath to exchange messages with the gRpc server.

You can find more on https://github.com/grpc/grpc-dotnet/tree/master/examples/Transporter

Server side, we configure Kestrel like this:

```cs
builder.WebHost.UseKestrel(options =>
{
    if (File.Exists(SocketPath)) {
        File.Delete(SocketPath);
    }
    options.ListenUnixSocket(SocketPath, listenOptions =>
    {
        //listenOptions.Protocols = HttpProtocols.Http2;
        //listenOptions.UseHttps();
    });
});
```

where the socket path is a file path to a unix socket, such as:

```cs
string SocketPath = Path.Combine(Path.GetTempPath(), "socket.tmp");
```

Client Side, we need a SocketsHttpHandler to use the UnixSocket.  
This handler is configured to use the same socket path as the server.

```cs
string SocketPath = Path.Combine(Path.GetTempPath(), "socket.tmp");
var udsEndPoint = new UnixDomainSocketEndPoint(SocketPath);
var connectionFactory = new UnixDomainSocketConnectionFactory(udsEndPoint);
var socketsHttpHandler = new SocketsHttpHandler {
    ConnectCallback = connectionFactory.ConnectAsync
};
using GrpcChannel channel = GrpcChannel.ForAddress(@"http://localhost", new GrpcChannelOptions {
    HttpHandler = socketsHttpHandler,
    ServiceConfig = new ServiceConfig() { MethodConfigs = { defaultMethodConfig } }
});

```

The UnixDomainSocketConnectionFactory is a class of ours, which uses a Socket in its ConnectAsync.

```cs
using System.Net;
using System.Net.Sockets;

namespace IPCDemo.ClientSide;
public class UnixDomainSocketConnectionFactory {
    private readonly EndPoint _endPoint;

    public UnixDomainSocketConnectionFactory(EndPoint endPoint) {
        _endPoint = endPoint;
    }

    public async ValueTask<Stream> ConnectAsync(SocketsHttpConnectionContext _,
        CancellationToken cancellationToken = default) {
        var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

        try {
            await socket.ConnectAsync(_endPoint, cancellationToken).ConfigureAwait(false);
            return new NetworkStream(socket, true);
        } catch {
            socket.Dispose();
            throw;
        }
    }
}
```