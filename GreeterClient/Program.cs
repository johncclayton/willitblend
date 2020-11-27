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

            using var channel = GrpcChannel.ForAddress("https://localhost:5001", 
                new GrpcChannelOptions { HttpHandler = httpHandler } );
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.BlendItForMeAsync(new HelloRequest { Name = "Stupid" });
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any stupid key to exit...");
            Console.ReadKey();
        }
    }
}
