using WordleClash.Core;
using WordleClash.Core.DataAccess;
using WordleClash.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connString == null) throw new ArgumentNullException($"Connection string cannot be null");
builder.Services.AddScoped<IDataAccess>(s => new DataAccess(connString));
builder.Services.AddScoped<Game>(s => new Game(6, s.GetRequiredService<IDataAccess>()));

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

app.MapRazorPages();

app.Run();