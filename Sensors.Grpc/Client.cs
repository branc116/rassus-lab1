using Grpc.Core;

namespace Sensors.Grpc;

public class Client : Server.ServerClient { }
public class SensorC : Sensor.SensorClient { }
public class SensorRelay : Sensor.SensorBase {
    private readonly SensorC? _nextC;

    public SensorRelay(SensorC? nextC)
    {
        _nextC = nextC;
    }
    public async override Task<ReadingReplay> SendReading(Reading request, ServerCallContext context)
    {
        if (_nextC is null)
            return new ReadingReplay() { Message = "No next one" };
        var res = await _nextC.SendReadingAsync(request);
        return res;
    }
}