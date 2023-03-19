using Amazon.S3;
using EonData.CloudControl.AWS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
    {
        options.UseSystemd();
    });

//builder.Logging
//    .AddAWSProvider(new AWS.Logger.AWSLoggerConfig("eondataweb") { LogStreamNamePrefix = "webapi" });

// improved systemd integration
builder.Services.AddSystemd();

// authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options => {
        // jwtbearer options
        builder.Configuration.Bind("AzureAdB2C", options);
    },
    options => {
        // ms identity options
        builder.Configuration.Bind("AzureAdB2C", options);
    });

// add AWS services
builder.Services
    .AddAWSService<IAmazonS3>()
    .AddScoped<S3FileStorageService>();

builder.Services
    .AddCors(options =>
    {
        options.AddDefaultPolicy(options =>
        {
            //options.WithOrigins("https://www.eondata.net")
            options.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
    })
    // map configure controller routing
    .AddControllers();

var app = builder.Build();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();