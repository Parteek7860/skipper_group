
using skipper_group_new.Interface;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Repositories;
using skipper_group_new.Repositories;
using skipper_group_new.Service;
using skipper_group_new.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.FileProviders;



var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();  // ✅ Add this line
builder.Services.AddRazorPages();



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

builder.Services.AddScoped<MenuDataService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// -------------------------------------------------
// 2️⃣ Developer Exception Page
// -------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/MetroHome/Error");
    app.UseStatusCodePagesWithReExecute("/Error/Handle/{0}");
    app.UseHsts();
}

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



// -------------------------------------------------
// 3️⃣ Standard Middleware Order
// -------------------------------------------------
app.UseHttpsRedirection();
app.UseStaticFiles();




app.Use(async (context, next) =>
{
    var nonce = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    context.Items["CSPNonce"] = nonce;

    // Security headers
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=()";

    // ✅ FINAL CSP - CKEditor, jQuery, Slick, Font Awesome, WOW.js, WebSocket, Google Fonts
    var csp = string.Join(" ",
        "default-src 'self';",

     $"script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdn.ckeditor.com https://cdnjs.cloudflare.com https://code.jquery.com https://cdn.jsdelivr.net https://cke4.ckeditor.com; ",   

        // ✅ Allow inline styles + external fonts & icons
        "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdnjs.cloudflare.com https://cdn.jsdelivr.net;",

        // ✅ Allow fonts from Google & cdnjs
        "font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com https://cdn.jsdelivr.net data:;",

        // ✅ Allow local + base64 images
        "img-src 'self' data: https://cdn.ckeditor.com https://cdnjs.cloudflare.com;",

        // ✅ Allow AJAX and WebSocket connections (for Live Reload / SignalR / CKEditor preview)
        "connect-src 'self' ws: wss: https://cdn.ckeditor.com https://cdnjs.cloudflare.com;",

        // ✅ Allow iframes from CKEditor (for previews, embeds, etc.)
        "frame-src 'self' https://cdn.ckeditor.com;",

        // ✅ Secure form submissions
        "form-action 'self';",

        // ✅ Prevent other sites from embedding your pages
        "frame-ancestors 'none';"
    );

    context.Response.Headers["Content-Security-Policy"] = csp;

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
        name: "backoffice",
        pattern: "{controller=Backoffice}/{action=Signin}/{id?}");

  
    endpoints.MapControllerRoute(
        name: "dynamic",
        pattern: "{*url}",
        defaults: new { controller = "MetroHome", action = "DynamicRoute" });
});

app.Run();

