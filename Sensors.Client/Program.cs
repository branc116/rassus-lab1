using static Sensors.Grpc.Helper;
var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
System.Net.ServicePointManager.ServerCertificateValidationCallback = (_,_,_,_) => true;


// -- <listen_ip> <listen_port> <server_ip> <server_port>



var (listen_ip, listen_port, server_ip, server_port) = (args[0], args[1], args[2], args[3]);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddGrpc();
builder.Services.AddRouting();
builder.Services.AddHostedService<Sensors.Client.RelayService>();
builder.Services.AddSingleton<Sensors.Grpc.Client>(i => {
    var httpHandler = new HttpClientHandler();
    httpHandler.ServerCertificateCustomValidationCallback = 
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

    var channel = Grpc.Net.Client.GrpcChannel.ForAddress($"https://{server_ip}:{server_port}",
    new Grpc.Net.Client.GrpcChannelOptions { HttpHandler = httpHandler });
    var a = new Sensors.Grpc.Client(channel);
    return a;
} );

System.Console.WriteLine(listen_ip.iptoint().inttoip());
System.Console.WriteLine(server_ip.iptoint().inttoip());

builder.Services.AddSingleton<IDataAccaquerer>(i => new FakeDataAccaquerer(DateTime.Now, "FakeData.csv"));
builder.Services.AddSingleton(i => {
    var client = i.GetRequiredService<Sensors.Grpc.Client>();
    var random = new Random();
    var a = client.RegisterSensor(new Register {
                IpAddress= listen_ip.iptoint(),
                Port=int.Parse(listen_port),
                Latitude= (float)(random.NextDouble() * 180 - 90),
                Longitude= (float)(random.NextDouble() * 360 - 180)
            });
    var nn = client.NearestNeighbro(new NN {SensorId = a.SensorId});
    var httpHandler = new HttpClientHandler();
    httpHandler.ServerCertificateCustomValidationCallback = 
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

    var channel = Grpc.Net.Client.GrpcChannel.ForAddress($"https://{nn.IpAddress.inttoip()}:{nn.Port}",
        new Grpc.Net.Client.GrpcChannelOptions { HttpHandler = httpHandler });
    return new Sensors.Grpc.SensorC(channel)
    {
        SensorId = a.SensorId
    };
});

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(i => {
    i.MapGrpcService<Sensors.Grpc.SensorRelay>();
});

app.UseStaticFiles();

app.MapRazorPages();

app.Run();
