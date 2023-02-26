using Microsoft.AspNetCore.Authentication.Cookies;

namespace StockControlProject.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); // Runtime de compile etme --> manage nuget packages runtimecompilation

            builder.Services.AddSession(option => option.IdleTimeout = TimeSpan.FromMinutes(30)); // Oturumu tutacak. Cookie de�ildir.

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz ki�ilerin g�rmek istedi�inde y�nlendirilece�i sayfa
                option.ExpireTimeSpan= TimeSpan.FromDays(30); // Cookie ne zaman sona erecek ?
                option.SlidingExpiration = true; // �telemeli zaman a��m�n� aktif eder. ��lem yapt�k�a yenilenir.
                option.LoginPath = "/Account/Login"; // Kullan�c� giri�i sayfas�
                option.Events.OnRedirectToLogin = context => // Giri� sayfas�na nereden geliyoruz.
                { 
                    context.Response.Redirect(context.RedirectUri); // Clienta ge�ici bir cevap veriyor.
                    return Task.CompletedTask; // Tamamlanm�� i�lem y�nlendirmesi
                };
                //option.ReturnUrlParameter = "returnURL"; // Kullan�c�n�n geldi�i yeri al�caz. Buray� yapmam�z laz�m. �stteki koda g�re, arkada tutarsa ok.
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
            // SIRA �NEML� !
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}