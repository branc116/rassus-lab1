using Sensors.Grpc;

namespace Sensors.Client;

public class RelayService : IHostedService
{
    private readonly SensorC _relay;
    private readonly IDataAccaquerer _dataAccaquerer;
    private readonly Grpc.Client _server;
    Thread? thread;
    
    public RelayService(Sensors.Grpc.SensorC relay,
        Sensors.Grpc.Client server,
        IDataAccaquerer dataAccaquerer)
    {
        _relay = relay;
        _dataAccaquerer = dataAccaquerer;
        _server = server;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        thread = new Thread(async () => {
            while(true) {
                var entry = await _dataAccaquerer.GetEntryAsync();
                var res = await _relay.SendReadingAsync(new Reading {
                    Temperature = entry.Temperature,
                    Pressure = entry.Pressure,
                    Humidity = entry.Humidity,
                    CO = entry.CO,
                    NO2 = entry.NO2,
                    SO2 = entry.SO2,
                    SensorId = _relay.SensorId
                });
                System.Console.WriteLine(res.Message);
                var ress = await _server.ImAliveAsync(new Alive {SensorId = _relay.SensorId});
                await Task.Delay(1000);
            }
        });
        thread.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        thread?.Join();
        return Task.CompletedTask;
    }
    
}
