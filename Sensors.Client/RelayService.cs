using Sensors.Grpc;

namespace Sensors.Client;

public class RelayService : IHostedService
{
    private readonly SensorC _relay;
    private readonly IDataAccaquerer _dataAccaquerer;
    Thread? thread;
    
    public RelayService(Sensors.Grpc.SensorC relay, IDataAccaquerer dataAccaquerer)
    {
        _relay = relay;
        _dataAccaquerer = dataAccaquerer;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        thread = new Thread(async () => {
            var entry = await _dataAccaquerer.GetEntryAsync();
            await _relay.SendReadingAsync(new Reading {
                Temperature = entry.Temperature,
                Pressure = entry.Pressure,
                Humidity = entry.Humidity,
                CO = entry.CO,
                NO2 = entry.NO2,
                SO2 = entry.SO2,
                SensorId = _relay.SensorId
            });
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
