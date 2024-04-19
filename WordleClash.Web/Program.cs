using WordleClash.Core;
using WordleClash.Core.DataAccess;
using WordleClash.Data;
using WordleClash.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connString == null) throw new ArgumentNullException($"Connection string cannot be null");
builder.Services.AddSingleton<IDataAccess>(s => new DataAccess(connString));
builder.Services.AddSingleton<GameService>(s => new GameService(s.GetRequiredService<IDataAccess>()));
builder.Services.AddScoped<SessionService>();

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

app.MapRazorPages();

app.Run();