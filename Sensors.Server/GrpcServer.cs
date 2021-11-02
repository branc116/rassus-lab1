
using Grpc.Core;
using static Sensors.Grpc.Helper;

namespace SensorServer;
public class ServerConfiguration {
    public string? Ip { get; set; }
    public int Port { get; set; }
}

public class GrpcServer : global::Server.ServerBase {
    private readonly ServerConfiguration _configuration;

    public GrpcServer(ServerConfiguration configuration) : base()
    {
        _configuration = configuration;
    }
    public async override Task<RegisterReplay> RegisterSensor(Register request, ServerCallContext context)
    {
        // return base.RegisterSensor(request, context);
        return new RegisterReplay {
            SensorId = 69
        };
    }
    public async override Task<NNReplay> NearestNeighbro(NN request, ServerCallContext context)
    {
        return new NNReplay {
            IpAddress = _configuration.Ip?.iptoint() ?? 0,
            Port = _configuration.Port
        };
    }
    public async override Task<AliveReplay> ImAlive(Alive request, ServerCallContext context)
    {
        return new AliveReplay();
    }
}