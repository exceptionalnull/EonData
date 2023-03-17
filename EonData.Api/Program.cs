using Amazon.S3;
using EonData.CloudControl.AWS;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
    {
        options.UseSystemd();
    });

builder.Logging.AddAWSProvider();

builder.Services
    // improved systemd integration
    .AddSystemd()
    // add AWS services
    .AddAWSService<IAmazonS3>()
    .AddScoped<S3FileStorageService>()
    // configure controller routing
    .AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();