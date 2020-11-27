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
            // DO THIS FIRST: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-5.0#trust-the-aspnet-core-https-development-certificate-on-windows-and-macos

            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = "Stupid" });
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any stupid key to exit...");
            Console.ReadKey();
        }
    }
}
