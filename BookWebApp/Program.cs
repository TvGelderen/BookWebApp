using BookWebApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Database
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DatabaseConnection")
    ));
// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "GoogleOpenID";
})
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Denied";
    })
    .AddOpenIdConnect("GoogleOpenID", options =>
    {
        options.Authority = "https://accounts.google.com";
        options.ClientId = "420271345597-mdtc6hu6ci92mhop85rp64envkh0b8l4.apps.googleusercontent.com";
        options.ClientSecret = "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";
        options.CallbackPath = "/Login";
        options.SaveTokens = true;
        options.Events = new OpenIdConnectEvents()
        {
            OnTokenValidated = context =>
            {
                var principal = context.Principal;
                if (principal != null)
                {
                    var principalClaimId = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                    if (principalClaimId != null && principalClaimId.Value == "108888913904334650561")
                    {
                        var claim = new Claim(ClaimTypes.Role, "Admin");
                        if (principal.Identity is ClaimsIdentity claimsIdentity)
                            claimsIdentity.AddClaim(claim);
                    }
                }

                return Task.CompletedTask;
            }
        };
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
