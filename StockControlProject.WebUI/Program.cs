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

            builder.Services.AddSession(option => option.IdleTimeout = TimeSpan.FromMinutes(30)); // Oturumu tutacak. Cookie deðildir.

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz kiþilerin görmek istediðinde yönlendirileceði sayfa
                option.ExpireTimeSpan= TimeSpan.FromDays(30); // Cookie ne zaman sona erecek ?
                option.SlidingExpiration = true; // Ötelemeli zaman aþýmýný aktif eder. Ýþlem yaptýkça yenilenir.
                option.LoginPath = "/Account/Login"; // Kullanýcý giriþi sayfasý
                option.Events.OnRedirectToLogin = context => // Giriþ sayfasýna nereden geliyoruz.
                { 
                    context.Response.Redirect(context.RedirectUri); // Clienta geçici bir cevap veriyor.
                    return Task.CompletedTask; // Tamamlanmýþ iþlem yönlendirmesi
                };
                //option.ReturnUrlParameter = "returnURL"; // Kullanýcýnýn geldiði yeri alýcaz. Burayý yapmamýz lazým. Üstteki koda göre, arkada tutarsa ok.
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
            // SIRA ÖNEMLÝ !
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