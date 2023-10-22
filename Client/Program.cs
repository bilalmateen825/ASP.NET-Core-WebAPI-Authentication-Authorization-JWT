using Client.Classes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


#region Authentication

builder.Services.AddAuthentication(Constants.CookieSchemeName).
    AddCookie(Constants.CookieSchemeName, options =>
    {
        options.Cookie.Name = Constants.CookieSchemeName;
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
    });

#endregion

#region Authorization

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.
    AddPolicy("MustBelongToAdministration"
    , policy =>
    {
        policy.RequireClaim(Constants.AdministrationUserClaimName, "true");
    });
});

#endregion

builder.Services.AddHttpClient("WebAPIClient", options =>
{
    options.BaseAddress = new Uri("http://localhost:5230/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
