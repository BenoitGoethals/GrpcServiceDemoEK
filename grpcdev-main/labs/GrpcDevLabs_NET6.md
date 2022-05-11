# GRPCDEV Labs
## Lab1

### Prerequisites

This lab requires the installation of `grpcurl` and `grpcui`.

### grpcurl
- Download and install the latest `grpcurl` release from https://github.com/fullstorydev/grpcurl/releases

### grpcui
- Download the latest release of the `go` language from https://go.dev/doc/install
- To install `grpcui`, follow the instuctions on https://github.com/fullstorydev/grpcui#installation

### The Lab

In this Lab we will make our first (simple) gRPC client and server. Follow the following steps:
- Start Visual Studio and create a new project of type "ASP.NET Core gRPC Service".
- Take a look at the generated *Program.cs*, *GreeterService.cs* and *greet.proto*
- Add a NuGet-package `Grpc.AspNetCore.Server.Reflection`
- Open file *Program.cs*
	- after the call `builder.Services.AddGrpc()` add a call to `builder.Services.AddGrpcReflection();`
	- before the call to `app.MapGrpcService<GreeterService>();` add a call to `app.MapGrpcReflectionService();`

- Build and run the program. Look at the output in the console window and take not on which ports the server is listening to, both the http and https ones.

The Server is running. Now we can test the server:

- Open a Command-Window and enter the following command: 
```
grpcurl localhost:5001 describe
``` 
Make sure to use the same https port as the one noted previously. Look at the output.  
- enter the following command: 

```
grpcui localhost:5000
``` 

Make sure to use the same http port as the one noted previously. Look at the web page that is opened and fill in the form to test your service.

Now we will create the client:
- Add a new project to the solution of type "Console App (.Net Core)".
- Add a NuGet-package `Grpc.Net.Client`,`Grpc.Tools` and `Google.Protobuf` to the project.
- Add a folder `Protos` to the Client project.
- Add `greet.proto` from the Server project to the Client project
	- Make sure that the file properties have "Protobuf compiler" as the *Build Action*
	- Change the value of "gRPC Stub Classes" to *Client only*.
- Build the project to generate the Stub Classes.
- Open *Program.cs* and add `namespace` `Grpc.Net.Client`.  Also add the `namespace` of your server project.
- Create a variable of type `GrpcChannel` (use `using`) and create a channel using the *static* member function `ForAddress`. Pass the right url to connect to the server.
- Create a variable of type `HelloRequest` and instantiate an object of this type.
- Set the *Name* property of this object to a string with your own name.
- Create a variable of type `Greeter.GreeterClient` and instantiate an object of this type. Pass the channel as the parameter to the constructor.
- Invoke the method `SayHelloAsync()` on this object and pass your instance of `HelloRequest` as a parameter. This is an `async` method, so use `await` to call it. Declare a new variable of type `HelloReply` and assign the return value of `SayHelloAsync()` to this variable.
- Call method `ShutdownAsync()` on the channel. This is an `async` method, so use `await` to call it.
- Print member `Message` of the returned `HelloReply` to the console.
- Build the client and make sure that the server still runs. Run the client and see the result.

## Lab2

- Create a new Visual Studio Solution with a gRPC project. We are going to build a service that registers subscriptions to an on demand tv channel.
- Remove the content of the generated *.proto* file and rename the file to "registration.proto"
- in the *.proto*-file define a package called "registration"
- define a service called `Registration` with a method, called `Register` that accepts a `RegisterRequest` and returns a `RegisterReply`.
- define an `enum` called `MemberType` with three constant values: `Basic`, `Gold`, `Platinum`.
- define a *message* called `Address` containing fields for:
	- `street`, type `string`
	- `number`, type `uint32`
	- `zip`, type `string`
	- `city`, type `string`
- define a `message` called `RegisterRequest` containing fields for:
	- `name`, type `string`
	- one of:
		- a field `address` of type `Address`
		- a field `email` of type `string`
	- `birth_date`, type `Timestamp`
	- `picture`, type `bytes`
	- `subscription_type`, type `MemberType`
	- `repeated` field `family_members`, type `string`
- define a `message` called `RegisterReply` containing fields for:
	- `welcome`, type `string`
	- `price`, type `uint32`
	- `confirmed_age`, type `Duration`
	- `repeated` field `subscription_keys`, type `string`

- Now rename file "GreeterService.cs" to "RegistrationService.cs", open it and remove all references to the GreeterService and replace them with your own "RegistrationService"
- Check whether the applicant is 18 years or older. If not throw an exception with status code `GRPC_STATUS_INTERNAL` and a message.
- Check the number of family members. It must not exceed 5 (including the applicant). If there are more family members, throw an exception with the same status code but a different message.
- the `picture` that is sent, must be written to a file (current directory) with the name of the applicant (spaces replaced by '_') and the extension ".png".
- Create subscription keys for all family members (including the applicant) by appending a random number to their names.
- the price depends on the type of Membership and the number of family members. The prices are:
	- `Basic`: �10 + �1 for each family member
	- `Gold`: �15 + �2 for each family member
	- `Platinum`: �20 + 3 for each family member
- if the registration contains a physical address the price is raised with �5 because the confirmation has to be sent to this address.
- Create a `RegisterReply` object and set the fields to the right values (do not forget the `confirmed_age`). Return the `RegisterReply` object.

- Now we will create a client for this RegistrationService.
- Add a new Console project (.NET Core) to the solution.
- Add the necessary NuGet packages.
- Copy the `registration.proto` from the server project and set it to generate the Client stub classes.
- Add the necessary `using` declarations and create the request object.
- Fill the request object with the required content. You can use one of the provided pictures, if you want.
	- make sure the `DateTime` you use for the `BirthDate` is UTC.
- Create a channel and a client stub and call the `Register()` method. Shutdown the channel. Do not forget `exception` handling
- Write the different fields in the reply object to the Console and check whether they are correct.
- Test also whether `exception` handling works correctly.


Images by <a href="https://pixabay.com/users/openclipart-vectors-30363/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=image&amp;utm_content=2023311">OpenClipart-Vectors</a> from <a href="https://pixabay.com/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=image&amp;utm_content=2023311">Pixabay</a>


## Lab3

In this Lab we will use streaming patterns.

### Exercise1

- Create a new .NET Core project for a gRPC Service.
- Rename the files, packages, namespaces, services etc. to "calculate..."
- Create the `.proto` file for a service that accepts a stream of messages that each contain just a number (`int32`). The service returns a message that contains a single number (`int64`)
- The service receives messages from a client and after all messages are received, the server calculates the sum of the received numbers and returns the result.
- Add a .NET Core Console project for the client.
- Create the client and test your application.

### Exercise2

- Change the implementation of Exercise1: the server does not return the resulting sum itself, but a sequence of messages of which each contain a prime factor of the sum of the numbers that the client sent.
	- example: suppose the client sends all numbers from 0 to 99. The resulting sum is 4950, but the server returns the numbers: 2, 3, 3, 5, 5, 11 (each number in a separate message).

## Lab4

In this lab we will authenticate the client using a client certificate.

- Create a new Visual Studio Solution with a gRPC project.
- Add NuGet package: `Microsoft.AspNetCore.Authentication.Certificate`
- Open `Program.cs` 
- Add a `using` declaration for 
  - `Microsoft.AspNetCore.Authentication.Certificate` 
  - `Microsoft.AspNetCore.Server.Kestrel.Core`
  - `Microsoft.AspNetCore.Server.Kestrel.Https`
  - `System.Security.Cryptography.X509Certificates`
- Call `COnfigure<KestrelServerOptions>()` on the `builder.Services` object.
	- pass a lambda that gets an `options` parameter and call `ConfigureHttpsDefaults()` on it.
		- pass a lambda that gets an `options` parameter and set:
			- `ClientCertificateMode` to `RequireCertificate`
- Call the method `AddAuthentication()` on the `builder.Services` property.
	- pass `CertificateAuthenticationDefaults.AuthenticationScheme` as parameter
- Call `AddCertificate()` after `AddAuthentication()` and pass a lambda that gets an `options` parameter as parameter.
	- on the `options` parameter set `RevocationMode` to `NoCheck`.
- Call `UseAuthentication()` on the `app` object.


- Open *GreeterService.cs* and inside `SayHello()` get the `User.Identity.Name` from the `HttpContext` (that you get from the `ServerCallContext`) and write it to the log.

- Add a .NET Core Console project to your solution.
- Create a gRPC Client as we did in the previous exercises.
- Test your client and remark that it doesn't run, because the server refuses the connection.

Now we will add code to use a client certificate.
- add a `using` declaration for `System.Security.Cryptography.X509Certificates`
- Create a variable of type `X509Store` and create an object, passing the right `StoreName.My` as `StoreName` and `StoreLocation.CurrentUser` as `StoreLocation`
- Open the store as `ReadOnly`
- Create a variable of type `X509Certificate2Collection` and initialize it by calling `Find()` on the Certificate-collection of your store.
- Create a variable of type `HttpClientHandler` and create a new object of this type.
	- in this object add your certificate to the property `ClientCertificates`
- Create a variable of type `GrpcChannelOptions` and an object of this type. Assign your instance of `HttpClientHandler` to the property `HttpHandler`.
- Now pass your instance of `GrpcChannelOptions` as second parameter to the `ForAddress()` method of `GrpcChannel`.
- Now test your client again and it should work.
- Check whether the server application logged your username.
