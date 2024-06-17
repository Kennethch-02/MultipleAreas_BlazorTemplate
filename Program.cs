using MultipleAreas_BlazorTemplate;
using MultipleAreas_BlazorTemplate.Interfaces;
using MultipleAreas_BlazorTemplate.Services;
using MultipleAreas_BlazorTemplate.Services.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MultipleAreas_BlazorTemplate.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
GlobalConfigModel.configuration = builder.Configuration;

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DevPolicy", policy =>
        policy.RequireRole("Dev"));

    options.AddPolicy("RootPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Root") || context.User.IsInRole("Dev")));

    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Root") || context.User.IsInRole("Dev")));

    options.AddPolicy("UserPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("User") || context.User.IsInRole("Admin") || context.User.IsInRole("Root") || context.User.IsInRole("Dev")));
});

builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IUserService, UserDataService>();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

// Agregar servicios de controladores
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Mapear controladores
app.MapControllers();

app.Run();
