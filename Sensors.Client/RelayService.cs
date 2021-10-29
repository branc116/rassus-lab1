using Sensors.Grpc;

namespace Sensors.Client;

public class RelayService : IHostedService
{
    private readonly Grpc.Client _nextClient;
    private readonly SensorRelay _relay;
    private readonly IDataAccaquerer _dataAccaquerer;
    Thread? thread;
    
    public RelayService(Sensors.Grpc.SensorRelay relay, IDataAccaquerer dataAccaquerer)
    {
        _relay = relay;
        _dataAccaquerer = dataAccaquerer;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        thread = new Thread(() => {
            
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        thread.Abort();

    }
    
}
