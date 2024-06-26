using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Caching.Memory;
using WordleClash.Core.Interfaces;
using WordleClash.Core;
using WordleClash.Core.Services;
using WordleClash.Data;
using WordleClash.Web.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddServerSentEvents();

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connString == null) throw new ArgumentNullException($"Connection string cannot be null");
builder.Services.AddScoped<IWordRepository>(_ => new WordRepository(connString));
builder.Services.AddScoped<IUserRepository>(_ => new UserRepository(connString));
builder.Services.AddScoped<IGameLogRepository>(_ => new GameLogRepository(connString));

builder.Services.AddScoped<GameService>(s => new GameService(s.GetRequiredService<IWordRepository>(), s.GetRequiredService<IMemoryCache>()));
builder.Services.AddScoped<LobbyService>(s => new LobbyService(s.GetRequiredService<IWordRepository>(), s.GetRequiredService<IMemoryCache>()));
builder.Services.AddScoped<UserService>(s => new UserService(s.GetRequiredService<IUserRepository>(), s.GetRequiredService<IGameLogRepository>()));

builder.Services.AddTransient<SessionManager>();
builder.Services.AddTransient<ServerEvents>();

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

app.UseAuthorization();

app.UseSession();
app.UseMiddleware<LobbyMiddleware>();
app.UseMiddleware<GameMiddleware>();

app.MapServerSentEvents("/updates");
app.MapRazorPages();

app.Run();