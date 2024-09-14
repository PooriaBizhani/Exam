using First_Sample.Infrastructure.Ioc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureFirstSampleServices(builder.Configuration);
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
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
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // For MVC controllers

app.MapRazorPages();

app.Run();
