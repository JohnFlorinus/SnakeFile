using Microsoft.AspNetCore.HttpOverrides;
using Oci.Common;
using Oci.Common.Auth;
using Oci.ObjectstorageService;
using SnakeFileAPI.Repositories;
using System.Threading.RateLimiting;

namespace SnakeFileAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var allCORS = "frontend";
            var builder = WebApplication.CreateBuilder(args);

            // CORS för frontend integration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allCORS,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://snakefile.com").AllowAnyMethod().AllowAnyHeader();
                                  });
            });

            // Rate limiting för UPLOAD
            builder.Services.AddRateLimiter(options => 
            {
                options.AddPolicy("UploadLimit", context =>
                {
                    // X-forwarded-for, annars remote ip adress
                    var clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString();
                    return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 4,  // 4 uploads
                        Window = TimeSpan.FromHours(1),  // per 1 timme
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0  // Släng iväg fler requests direkt
                    });
                });
            });

            // OCI Cloud DI
            string privatekey = @"-----BEGIN PRIVATE KEY-----
FILL PRIVATE KEY HERE
-----END PRIVATE KEY-----
OCI_API_KEY";

            builder.Services.AddSingleton<ObjectStorageClient>(provider =>
            {
                // auth provider detaljer
                var authProvider = new SimpleAuthenticationDetailsProvider();
                authProvider.TenantId = "FILL TENANT ID HERE";
                authProvider.UserId = "FILL USER ID HERE";
                authProvider.Fingerprint = "FILL FINGERPRINT HERE";
                authProvider.Region = Region.EU_STOCKHOLM_1;
                authProvider.PrivateKeySupplier = new PrivateKeySupplier(privatekey);
                return new ObjectStorageClient(authProvider);
            });

            builder.Services.AddSingleton<FileRepository>();
            builder.Services.AddScoped<IFileRepository, FileRepository>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // NGINX för vidare IP via X Forwarded For
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            app.UseCors(allCORS);
            app.UseAuthorization();
            app.UseRateLimiter();
            app.MapControllers();
            app.Run();
        }
    }
}
