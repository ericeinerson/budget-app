using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using budget_app.Data;
using Microsoft.EntityFrameworkCore;
using budget_app.Options;
using budget_app.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.Configure<Authentication>(builder.Configuration.GetSection("Authentication"));
var connectionString = builder.Configuration.GetConnectionString("Dev") ?? string.Empty;

builder.Services.AddDbContextFactory<BudgetAppDbContext>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version())));
builder.Services.AddScoped<StateContainer>();
var app = builder.Build();

//Do not do this in production, just development
await EnsureDatabaseIsMigrated(app.Services);

async Task EnsureDatabaseIsMigrated(IServiceProvider services)
{
    using var scope = services.CreateScope();
    using var ctx = scope.ServiceProvider.GetService<BudgetAppDbContext>();
    if(ctx is not null)
    {    
        await ctx.Database.MigrateAsync();
    }
}

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
