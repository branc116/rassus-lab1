var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddGrpc();
int iptoint(string ip) {
    var a = ip.Split(".").Select(int.Parse).ToArray();
    int ret = 0;
    for(int i = 0; i < 4;i++) {
        ret |= a[i] << (8*i);
    }
    return ret;
}
int inttoip(string ip) {
    
}
builder.Services.AddSingleton(i => {
    var client = i.GetRequiredService<Sensors.Grpc.Client>();
    var random = new Random();
    client.RegisterSensorAsync(new Register {IpAddress=  args[1], Port Latitude = (float)(random.NextDouble() * 180 - 90), Longitude = (float)(random.NextDouble() * 360 - 180)});
    client.NearestNeighbro(new NN {SensorId = })
})

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseEndpoints(i => {
    i.MapGrpcService<Sensors.Grpc.SensorRelay>();
});

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
