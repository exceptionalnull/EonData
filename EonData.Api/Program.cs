using Amazon.DynamoDBv2;
using Amazon.S3;

using EonData.ContactForm.Services;
using EonData.FileShare.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Identity.Web;


var builder = WebApplication.CreateBuilder(args);

// configure kestrel to support using unix socket files provided by systemd
builder.WebHost.ConfigureKestrel((context, options) => { options.UseSystemd(); });

// setup some general services
builder.Services
    .AddSystemd()       // improved systemd integration
    .AddHttpClient();   // allow proper httpclient DI

// add authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        // jwtbearer options
        builder.Configuration.Bind("AzureAdB2C", options);
    },
    options =>
    {
        // ms identity options
        builder.Configuration.Bind("AzureAdB2C", options);
    });

// add cloudwatch logging
builder.Logging.AddAWSProvider();

// add AWS services
builder.Services
    .AddAWSService<IAmazonDynamoDB>()
    .AddAWSService<IAmazonS3>();

// add application services
builder.Services
    .AddTransient<IContactFormService, ContactFormService>()
    .AddTransient<IEonShareService, EonShareService>();

// configure forwarded headers so that client details are correctly configured through the reverse proxy
builder.Services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; });

// add CORS and routing
builder.Services
    .AddCors(options =>
    {
        options.AddDefaultPolicy(options =>
        {
            string corsOrigin = builder.Configuration["Cors:Origin"] ?? string.Empty;
            if (string.IsNullOrEmpty(corsOrigin))
            {
                throw new ArgumentNullException(nameof(corsOrigin));
            }

            options.WithOrigins(corsOrigin)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    })
    .AddControllers();

builder.Services.AddRateLimiter(rlo => rlo
        // rate limit for sending contact messages
        .AddTokenBucketLimiter(policyName: "contactMessageLimit", options =>
        {
            options.ReplenishmentPeriod = TimeSpan.FromHours(32);
            options.TokenLimit = 5;
            options.TokensPerPeriod = 1;
            options.QueueLimit = 0;
        })
    );

// build app
var app = builder.Build();
app.UseCors();
app.UseForwardedHeaders();
app.UseRateLimiter();

// add a custom header to every response to show the api version
string apiver = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "Unknown";
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-EonDataApi-Version", apiver);
    await next.Invoke();
});

// finish building and run
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();