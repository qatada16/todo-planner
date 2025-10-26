using Microsoft.EntityFrameworkCore;
using todo_planner.Data;
using todo_planner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add DbContext with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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
// Add session middleware
app.UseSession();
app.UseRouting();
app.UseAuthorization();


app.MapRazorPages();
// Redirect root to login
app.MapGet("/", () => Results.Redirect("/Login"));

// Initialize database on startup (REMOVE the Users.Any() check)
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     dbContext.Database.Migrate(); // This applies any pending migrations
// }

app.Run();