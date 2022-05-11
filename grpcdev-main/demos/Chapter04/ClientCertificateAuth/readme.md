# Client Certificate Auth

In this demo, we show how to use Client Certificates to authenticate with a gRPC server.

First, we need a client certificate that we can use to authenticate with the server. We can make one with the following command, as described on https://docs.microsoft.com/en-us/powershell/module/pki/new-selfsignedcertificate?view=windowsserver2019-ps

```powershell
New-SelfSignedCertificate -Type Custom -Subject "CN=Patti Fuller,OU=UserAccounts,DC=corp,DC=contoso,DC=com" -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.2","2.5.29.17={text}upn=pattifuller@contoso.com") -KeyUsage DigitalSignature -KeyAlgorithm RSA -KeyLength 2048 -CertStoreLocation "Cert:\CurrentUser\My"
```

This example creates a self-signed client authentication certificate in the user MY store. The certificate uses the default provider, which is the Microsoft Software Key Storage Provider. The certificate uses an RSA asymmetric key with a key size of 2048 bits. The certificate has a subject alternative name of pattifuller@contoso.com.

The certificate expires in one year.

- Enhanced Key Usage. 2.5.29.37  
- Client Authentication. 1.3.6.1.5.5.7.3.2
- Subject Alternative Name. 2.5.29.17  



Now you can install this certificate in your Trusted Root.

- Open mmc.exe
- Select "Add Remove Snap Ins"
- Select "Certificates"
- Select "My User Account"
- Select "Trusted Root Certification Authorities"
- Select "Certificates"
- Right click "Certificates"
- Select "All Tasks -> Import"
- Select the certificate you created in the previous step
- Select "Trusted Root Certification Authorities" as a place to install the certificates
- Click Next and the Finish.

Now that your certificate is trusted, you can use it to authenticate with the gRPC server.

In the server, follow the instructions described in //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-6.0#configure-your-server-to-require-certificates

```cs
builder.Services.Configure<KestrelServerOptions>(options => {
    options.ConfigureHttpsDefaults(options =>
        options.ClientCertificateMode = ClientCertificateMode.RequireCertificate);
});
```

And then the instructions described in //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-6.0

```cs
builder.Services
    .AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(o => o.RevocationMode = X509RevocationMode.NoCheck);
```

On the client, follow the instructions described on https://docs.microsoft.com/en-us/aspnet/core/grpc/authn-and-authz?view=aspnetcore-6.0#client-certificate-authentication


```cs
//open the certificate store
X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
store.Open(OpenFlags.OpenExistingOnly);
//find the client certificate
X509Certificate2 certificate = store.Certificates.Find(X509FindType.FindBySubjectName, "patti", true)[0];

//add the certificate to the handler
var handler = new HttpClientHandler();
handler.ClientCertificates.Add(certificate);

// Create the gRPC channel with the handler
var channel = GrpcChannel.ForAddress(baseAddress, new GrpcChannelOptions {
    HttpHandler = handler
});
```