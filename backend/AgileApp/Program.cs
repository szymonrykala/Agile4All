using AgileApp;
using AgileApp.Repository.Files;
using AgileApp.Repository.Projects;
using AgileApp.Repository.Tasks;
using AgileApp.Repository.Users;
using AgileApp.Services.Files;
using AgileApp.Services.Projects;
using AgileApp.Services.Tasks;
using AgileApp.Services.Users;
using AgileApp.Utils.Authorization;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using AgileApp.Repository.Chat;
using AgileApp.Services.Chat;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(
    options =>
    {
        options.AllowedCertificateTypes = CertificateTypes.All;
        options.RevocationMode = X509RevocationMode.NoCheck;

        options.Events = new CertificateAuthenticationEvents
        {
            OnCertificateValidated = context =>
            {
                var claims = new[]
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer),
                    new Claim(
                        ClaimTypes.Name,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer)
                };

                context.Principal = new ClaimsPrincipal(
                    new ClaimsIdentity(claims, context.Scheme.Name));
                context.Success();

                return Task.CompletedTask;
            }
        };
    });

builder.Services.Configure<KestrelServerOptions>(options =>
{
    var cert = new X509Certificate2(Path.Combine("ESD", "certS.pfx"), "1234");
    options.ConfigureHttpsDefaults(o =>
    {
        o.ServerCertificate = cert;
        //o.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AgileDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<ICookieHelper, CookieHelper>();

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileRepository, FileRepository>();

builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
);

builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

// <snippet_UseWebSockets>
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);
// </snippet_UseWebSockets>

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseCors();
app.UseStaticFiles();

app.UsePathBase(new PathString("/api"));
app.UseRouting();

app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
