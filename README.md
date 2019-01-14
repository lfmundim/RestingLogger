# RestingLogger
## What does it do?
*RestingLogger* is a simple package to help you Log your REST requests and responses with ease. What it does is really simple (you could implement it yourself easily), but it is even simpler to use a package to do so 😋

## How does it do it?
First of all, it uses two solid packages:
1. RestEase, to create the Client from the `Interface` provided
1. Serilog, to (almost?) fully log every single request and response in your prefered *Serilog sink*

It has a `HttpClient` builder that overrides the client's default `SendAsync` method to insert logs before a request is sent and after the response is received.

The logs by default have the following format:
```csharp
_logger.Information("Id: {@correlationId}\tRequest:{@request}, Content:{@content}", correlationId, request, content);
```

Where:
* `correlationId`: a GUID that is used to pair a request and a response, so you can be certain of what response matches what request;
* `request`: the request URI, HTTP Method, Headers and so on;
* `content`: the request's body (if any)

## How do I use it?
In your `startup.cs` file (or wherever you want to instantiate a `HttpClient`), create an instance of `RestingLogger.Builders.LoggedRestClientBuilder`
```csharp
(...)
var clientBuilder = new RestingLogger.Builders.LoggedRestClientBuilder();
(...)
```

and then, whenever you wish to instantiante the client:
```csharp
(...)
var httpClient = clientBuilder.BuildLoggedClient<IRestEaseClient>(baseApiUri, serilogLogger);
(...)
```

It will return to you a fully functional RestEase HttpClient that you can use normally, with the bonus of logging every request and response 😊