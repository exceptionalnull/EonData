using Amazon.S3;
using EonData.CloudControl.AWS;

var builder = WebApplication.CreateBuilder(args);

// configure AWS services
builder.Services
    .AddAWSService<IAmazonS3>()
    .AddScoped<S3FileStorageService>();

var app = builder.Build();
app.Run();
