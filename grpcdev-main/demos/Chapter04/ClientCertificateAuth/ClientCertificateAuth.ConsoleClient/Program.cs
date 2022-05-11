using ClientCertificateAuth.ServerSide;
using Grpc.Net.Client;
using System.Security.Cryptography.X509Certificates;


string baseAddress = "https://localhost:7142";

Greeter.GreeterClient clientWithoutACert = CreateClientWithoutCert(baseAddress);
await SendRequest(clientWithoutACert);

X509Certificate2 certificate = FindCertificate()[0];
Greeter.GreeterClient clientWithCert = CreateClientWithCert(baseAddress, certificate);
await SendRequest(clientWithCert);

Console.ReadLine();

async Task SendRequest(Greeter.GreeterClient client) {
    try {
        var response = await client.SayHelloAsync(new HelloRequest() { Name = "Simona" });
        Console.WriteLine("Response: " + response.Message);
    } catch (Exception ex) {
        Console.WriteLine("Error while sending a request: " + ex.GetType().Name + " " + ex.Message);
    }
}

Greeter.GreeterClient CreateClientWithoutCert(string baseAddress) {
    // Create the gRPC channel without a handler to send the certificate
    var channel = GrpcChannel.ForAddress(baseAddress);

    return new Greeter.GreeterClient(channel);
}


//https://docs.microsoft.com/en-us/aspnet/core/grpc/authn-and-authz?view=aspnetcore-6.0#client-certificate-authentication
Greeter.GreeterClient CreateClientWithCert(
    string baseAddress,
    X509Certificate2 certificate) {
    // Add client cert to the handler
    var handler = new HttpClientHandler();
    handler.ClientCertificates.Add(certificate);

    // Create the gRPC channel with the handler
    var channel = GrpcChannel.ForAddress(baseAddress, new GrpcChannelOptions {
        HttpHandler = handler
    });

    return new Greeter.GreeterClient(channel);
}

List<X509Certificate2> FindCertificate() {
    List<X509Certificate2> certificates = new List<X509Certificate2>();
    X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
    store.Open(OpenFlags.OpenExistingOnly);
    foreach (X509Certificate2 cert in store.Certificates.Find(X509FindType.FindBySubjectName, "simona", true)) {
        certificates.Add(cert);
    }
    return certificates;
}

void PrintCerts() {
    List<X509Certificate2> certs = FindCertificate();
    foreach (var item in certs) {
        Console.WriteLine("**** CERTIFICATE ****");
        Console.WriteLine($"Subject: \n\t{item.Subject}\n");
        Console.WriteLine($"Issuer: \n\t{item.Issuer}\n");
        Console.WriteLine($"FriendlyName: \n\t{item.FriendlyName}\n");
        Console.WriteLine($"NotBefore: \n\t{item.NotBefore}\n");
        Console.WriteLine($"NotAfter: \n\t{item.NotAfter}\n");
    }
}