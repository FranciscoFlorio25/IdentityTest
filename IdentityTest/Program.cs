using IdentityTest.Models;
using IdentityTest.Web;
using IdentityTest.Web.Interfaces;
using IdentityTest.Web.Interfaces.Internal;
using IdetityTest.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IClaimRolesService, RoleClaimService>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped<IUserRolesService, UserRolesService>();
builder.Services.AddScoped<IClaimsService, ClaimsService>();

builder.Services.AddDbContext<CiscoShopContext>(o =>
o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<CiscoShopContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

