// -- <listen_ip> <listen_port>
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
var (listen_ip, listen_port) = (args[0], args[1]);



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddSingleton(i => new SensorServer.ServerConfiguration {
    Ip = listen_ip,
    Port = int.Parse(listen_port)
});
builder.Services.AddGrpc(); 
builder.Services.AddSingleton<Sensors.Server.Db.SensorsDatabase>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(i => {
    i.MapGrpcService<global::SensorServer.GrpcServer>().AllowAnonymous();
    i.MapGrpcService<global::SensorServer.GrpcSensorServer>().AllowAnonymous();
});

app.UseAuthorization();

app.MapRazorPages();

app.Run();
