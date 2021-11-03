
using Grpc.Core;
using static Sensors.Grpc.Helper;

namespace SensorServer;
public class ServerConfiguration {
    public string? Ip { get; set; }
    public int Port { get; set; }
}

public class GrpcServer : global::Server.ServerBase {
    private readonly ServerConfiguration _configuration;
    private int? lasatIp;
    private int? lastPort;
    static readonly List<Register> reg = new();
    public GrpcServer(ServerConfiguration configuration) : base()
    {
        _configuration = configuration;
    }
    public async override Task<RegisterReplay> RegisterSensor(Register request, ServerCallContext context)
    {
        // return base.RegisterSensor(request, context);
        reg.Add(request);
        return new RegisterReplay {
            SensorId = reg.Count - 1
        };
    }
    public async override Task<NNReplay> NearestNeighbro(NN request, ServerCallContext context)
    {
        System.Console.WriteLine($"Asking nn for: {request.SensorId}");
        if (request.SensorId == 0)
            return new NNReplay {
                IpAddress = _configuration.Ip?.iptoint() ?? 0,
                Port = _configuration.Port
            };
        return new NNReplay {
            IpAddress = reg[(int)(request.SensorId - 1)].IpAddress,
            Port = reg[(int)(request.SensorId - 1)].Port
        };
    }
    public async override Task<AliveReplay> ImAlive(Alive request, ServerCallContext context)
    {
        return new AliveReplay();
    }
}
public class GrpcSensorServer : global::Sensor.SensorBase {
    private readonly ServerConfiguration _configuration;

    public static readonly List<(
        long Temperature,
        long Pressure,
        long Humidity,
        long CO,
        long NO2,
        long SO2,
        long SensorId
    )> L = new();
    public GrpcSensorServer(ServerConfiguration configuration) : base()
    {
        _configuration = configuration;
    }
    public override Task<ReadingReplay> SendReading(Reading request, ServerCallContext context)
    {
        L.Add((
            request.Temperature,
            request.Pressure,
            request.Humidity,
            request.CO,
            request.NO2,
            request.SO2,
            request.SensorId
        ));
        return Task.FromResult(new ReadingReplay {Message = "OK"});
    }
}