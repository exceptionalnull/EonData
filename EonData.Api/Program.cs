using Amazon.S3;
using EonData.CloudControl.AWS;


const string socketFile = "/run/eondata-api.sock";


var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    // use unix socket file
    .UseSockets()
    .ConfigureKestrel((context, serverOptions) => {
        serverOptions.ListenUnixSocket(socketFile);
    });

builder.Services
    // improved systemd integration
    .AddSystemd()
    // add AWS services
    .AddAWSService<IAmazonS3>()
    .AddScoped<S3FileStorageService>();


var app = builder.Build();
app.Run();