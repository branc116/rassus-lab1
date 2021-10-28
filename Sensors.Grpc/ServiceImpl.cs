using Grpc.Core;

namespace Sensor.Grps;

public class ServiceImpl : Greeter.GreeterBase {
    public async override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        await Task.Delay(1000);
        return new HelloReply { Message = "Fuck you" };
    }
}