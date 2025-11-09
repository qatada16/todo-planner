using Microsoft.EntityFrameworkCore;
using todo_planner.Data;
using todo_planner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add DbContext with SQLite with better configuration
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), 
        sqliteOptions =>
        {
            sqliteOptions.CommandTimeout(60); // Increase command timeout
        });
});

// Add Session for authentication
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Register our services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TaskService>();

// Add HttpContextAccessor (important for session)
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Add session middleware BEFORE routing
app.UseSession();

app.UseRouting();

// Remove or comment out UseAuthorization since we're not using Identity
// app.UseAuthorization();

app.MapRazorPages();

// Redirect root to login
app.MapGet("/", () => Results.Redirect("/Login"));

// Initialize database on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // This applies any pending migrations
    
    // Seed initial data if needed
    await SeedData.Initialize(scope.ServiceProvider);
}

app.Run();

// Seed data class (add this at the end of Program.cs file)
public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

        // Ensure database is created and migrations are applied
        await context.Database.MigrateAsync();

        // You can add initial seed data here if needed
    }
}