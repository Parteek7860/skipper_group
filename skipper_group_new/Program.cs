
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using skipper_group_new.Interface;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using skipper_group_new.Repositories;
using skipper_group_new.Repositories;
using skipper_group_new.Service;
using skipper_group_new.Service;
using System.Net;
using university.Repositories;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//builder.Services.AddControllersWithViews();  // ✅ Add this line
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IHome, clsHome>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IHomePage, clsHomePage>();
builder.Services.AddScoped<IHomePageRepository, HomsPageRepository>();
builder.Services.AddScoped<clsMainMenuList>();
builder.Services.AddScoped<IBackofficePage, clsBackofficePage>();
builder.Services.AddScoped<IBackofficePageRepository, BackofficePageRepository>();
builder.Services.AddScoped<IProducts, serProduct>();
builder.Services.AddScoped<IProductRepository, ProductRepo>();
builder.Services.AddScoped<IBlog, BlogService>();
builder.Services.AddScoped<IBlogRepo, BlogRepository>();
builder.Services.AddScoped<IManagement, ManagementService>();
builder.Services.AddScoped<IManagementRepo, ManagementRepository>();
builder.Services.AddScoped<clsMainMenuList>(provider =>
    new clsMainMenuList(provider.GetRequiredService<IHomePage>()));

builder.Services.AddScoped<IInvestor, InvestorService>();
builder.Services.AddScoped<IInvestorRepository, InvestorRepository>();

builder.Services.AddScoped<IBacofficeProject, clsBackofficeProject>();
builder.Services.AddScoped<IBackofficeProjectRepository, BackofficeProjectRepository>();

builder.Services.AddScoped<MenuDataService>();
builder.Services.AddScoped<ISkipperHomeRepository, SkipperHomeRepository>();
builder.Services.AddScoped<ISkipperHome, clsSkipperHome>();
builder.Services.AddScoped<ISkipperInvestorPage, SkipperInvestorService>();
builder.Services.AddScoped<ISkipperInvestorRepo, SkipperInvestorRepository>();
builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<EmailService>();
System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var crypto = new Enc_Decyption();

var encrypted = builder.Configuration.GetConnectionString("DefaultConnection");
var decrypted = crypto.AES_Decrypt(encrypted, crypto.encrptdecrpt);

decrypted = decrypted.Replace(@"\\", @"\");




// register provider
builder.Services.AddSingleton<IDbConnectionProvider>(
    new DbConnectionProvider(decrypted));






builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
    });




var app = builder.Build();



app.UseResponseCompression();

// -------------------------------------------------
// 2️⃣ Developer Exception Page
// -------------------------------------------------
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/SkipperHome/Error");
//    app.UseStatusCodePagesWithReExecute("/Error/Handle/{0}");
//    app.UseHsts();
//}
app.MapGet("/robots.txt", async context =>
{
    context.Response.ContentType = "text/plain";

    await context.Response.WriteAsync(
@"User-Agent: *
Disallow:
Sitemap: https://www.skipperlimited.com/sitemap.xml"
    );
});

// Catch malformed requests
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (BadHttpRequestException)
    {
        context.Response.Redirect("/Error/Handle/400");
    }
});

var forceWWW = builder.Configuration.GetValue<bool>("UrlSettings:ForceWWW");

if (forceWWW)
{
    app.Use(async (context, next) =>
    {
        var host = context.Request.Host;

        if (!host.Host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
        {
            var newHost = "www." + host.Host;

            // Port only if exists
            if (host.Port.HasValue)
            {
                newHost += ":" + host.Port.Value;
            }

            var newUrl = $"{context.Request.Scheme}://{newHost}{context.Request.Path}{context.Request.QueryString}";

            context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
            context.Response.Headers.Location = newUrl;
            return;
        }

        await next();
    });
}
var forceHttps = builder.Configuration.GetValue<bool>("UrlSettings:ForceHttps");

if (forceHttps)
{
    app.UseHttpsRedirection();
}

// static media redirect
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();

    if (path.StartsWith("/media/") || path.StartsWith("/repository/") || path.StartsWith("/investor-relations/") && path.EndsWith(".pdf"))
    {
        context.Response.Redirect(
            "/investor-relations/",
            permanent: true
        );
        return;
    }

    await next();
});


// -------------------------------------------------
// 3️⃣ Standard Middleware Order
// -------------------------------------------------

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;

    if (!string.IsNullOrEmpty(path) && path.Any(char.IsUpper))
    {
        context.Response.Redirect(
            path.ToLowerInvariant() + context.Request.QueryString,
            permanent: true
        );
        return;
    }

    await next();
});

var allowedFiles = builder.Configuration
    .GetSection("AllowedHtmlFiles")
    .Get<List<string>>();


//app.UseStaticFiles();


app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
            "Cache-Control", "public,max-age=604800"); // 7 days
    }
});


app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value.ToLower();
    var nonce = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    context.Items["CSPNonce"] = nonce;

    // Skip CSP for back office
    if (!path.StartsWith("/admin") && !path.StartsWith("/backoffice"))
    {


        var cspTemplate = builder.Configuration["SecurityHeaders:CSP"];
        var finalPolicy = cspTemplate.Replace("{nonce}", $"'nonce-{nonce}'");

        context.Response.Headers["Content-Security-Policy"] = finalPolicy;


    }
    await next();
});




app.UseRouting();



app.UseSession();
app.UseAuthorization();



// -------------------------------------------------
// 6️⃣ Endpoint Mapping
// -------------------------------------------------
app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
     name: "backoffice route",
     pattern: "{controller=Backoffice}/{action=dashboard}/{name}/{pageid?}");

    endpoints.MapControllerRoute(
        name: "backoffice",
        pattern: "{controller=Backoffice}/{action=Signin}/{id?}");

    endpoints.MapControllerRoute(
         name: "default",
         pattern: "{controller=SkipperHome}/{action=Index}/{id?}");



    endpoints.MapControllerRoute(
        name: "dynamic",
        pattern: "{*url}",
        defaults: new { controller = "SkipperHome", action = "DynamicRoute" });




});

app.Run();

