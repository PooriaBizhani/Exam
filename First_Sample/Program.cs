using First_Sample.Application.Security.DynamicRole;
using First_Sample.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data; // اضافه کردن این خط

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.ConfigureFirstSampleServices(builder.Configuration);

builder.Services.AddRazorPages();

//builder.Services.AddControllersWithViews(option =>
//{
//    option.Filters.Add<DynamicRoleAuthorizeFilter>();
//    option.Filters.Add<LogActionFilter>();
//});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

var columnOptions = new ColumnOptions
{
    AdditionalColumns = new List<SqlColumn>
    {
        new SqlColumn { ColumnName = "UserId", DataType = SqlDbType.VarChar, DataLength = 50 },
        new SqlColumn { ColumnName = "UserName", DataType = SqlDbType.VarChar, DataLength = 100 },
        new SqlColumn { ColumnName = "IPAddress", DataType = SqlDbType.VarChar, DataLength = 100 }
    }
};

// دریافت connection string از تنظیمات
var connectionString = builder.Configuration.GetConnectionString("FirstSampleConnectionString");

// پیکربندی Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        tableName: "Logs",
        autoCreateSqlTable: true,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        columnOptions: columnOptions)
    .Filter.ByIncludingOnly(logEvent =>
        logEvent.Properties.ContainsKey("Action") &&
        (logEvent.Properties["Action"].ToString().Contains("Login") ||
         logEvent.Properties["Action"].ToString().Contains("LogOut")) &&
        logEvent.Properties.ContainsKey("Controller") &&
        logEvent.Properties["Controller"].ToString().Contains("Account"))
    .CreateLogger();

// استفاده از Serilog به عنوان لاگر
builder.Host.UseSerilog();


builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
{
    option.Password.RequiredUniqueChars = 0;
    option.User.RequireUniqueEmail = true;
    option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);

}).AddEntityFrameworkStores<First_Sample_Context>().AddDefaultTokenProviders();

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("DynamicRole", policy =>
    policy.Requirements.Add(new DynamicRoleRequirement()));
});


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // زمان انقضای سشن
    options.Cookie.HttpOnly = true; // افزایش امنیت کوکی
    options.Cookie.IsEssential = true; // ضروری بودن کوکی برای عملکرد اپلیکیشن
});
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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // For MVC controllers

app.MapRazorPages();

app.Run();
