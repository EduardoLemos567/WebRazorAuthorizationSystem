using Project.Startup;

var builder = WebApplication.CreateBuilder(args);
Startup.AddServices(builder.Services, builder.Configuration);
var app = builder.Build();
Startup.SetupSeedData(app.Services);
Startup.AddMiddlewares(app);
Startup.AddInitialData(app);
app.Run();
