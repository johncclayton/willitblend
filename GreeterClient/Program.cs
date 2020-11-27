using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace GreeterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // reference when certs come knocking on your validated door
            // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-5.0#trust-the-aspnet-core-https-development-certificate-on-windows-and-macos

            // OR; just ignore the darn cert validation error
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // on macOS, we expect utter failure when using HTTP/2 w/TLS.
            // Unable to bind to https://localhost:5001 on the IPv4 loopback interface: 'HTTP/2 over TLS is not supported on macOS due to missing ALPN support.'.
            // look here: https://docs.microsoft.com/en-us/aspnet/core/grpc/troubleshoot?view=aspnetcore-5.0#call-a-grpc-service-with-an-untrustedinvalid-certificate

            using var channel = GrpcChannel.ForAddress("http://localhost:5000", 
                new GrpcChannelOptions { HttpHandler = httpHandler } );
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.BlendItForMeAsync(new HelloRequest { Name = "Stupid" });
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any stupid key to exit...");
            Console.ReadKey();
        }
    }
}
