using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Concrete;
using Arch.DataAccessLayer.Abstract;
using Arch.DataAccessLayer.Concrete.Repositories;
using Arch.UI.CustomValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Arch.EntityLayer.Entities.Auth.Authorization;
using Arch.BussinessLayer.Mapper;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager Configuration = builder.Configuration;
IWebHostEnvironment Environment = builder.Environment;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ArchDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper'ý yapýlandýrýn
builder.Services.AddAutoMapper(typeof(DtoMapper));
builder.Services.AddScoped<ICompetitonService, CompetitonService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IDesignerUserService, DesignerUserService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));





builder.Services.AddIdentity<AppUser, AppRole>(_ =>
{
    _.Password.RequiredLength = 5; //En az kaç karakterli olmasý gerektiðini belirtiyoruz.
    _.Password.RequireNonAlphanumeric = false; //Alfanumerik zorunluluðunu kaldýrýyoruz.
    _.Password.RequireLowercase = false; //Küçük harf zorunluluðunu kaldýrýyoruz.
    _.Password.RequireUppercase = false; //Büyük harf zorunluluðunu kaldýrýyoruz.
    _.Password.RequireDigit = false; //0-9 arasý sayýsal karakter zorunluluðunu kaldýrýyoruz.

    _.User.RequireUniqueEmail = true; //Email adreslerini tekilleþtiriyoruz.
    _.User.AllowedUserNameCharacters = "abcçdefghiýjklmnoöpqrsþtuüvwxyzABCÇDEFGHIÝJKLMNOÖPQRSÞTUÜVWXYZ0123456789-._@+"; //Kullanýcý adýnda geçerli olan karakterleri belirtiyoruz.
}).AddPasswordValidator<CustomPasswordValidation>()
  .AddUserValidator<CustomUserValidation>()
  .AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<ArchDbContext>()
  //.AddRoles<IdentityRole>()
  .AddDefaultTokenProviders(); ;

//builder.Services.ConfigureApplicationCookie(_ =>
//{
//    _.LoginPath = new PathString("/Account/Login");
//    _.Cookie = new CookieBuilder
//    {
//        Name = "AspNetCoreIdentityExampleCookie", //Oluþturulacak Cookie'yi isimlendiriyoruz.
//        HttpOnly = false, //Kötü niyetli insanlarýn client-side tarafýndan Cookie'ye eriþmesini engelliyoruz.
//        //Expiration = TimeSpan.FromMinutes(2), //Oluþturulacak Cookie'nin vadesini belirliyoruz.
//        SameSite = SameSiteMode.Lax, //Top level navigasyonlara sebep olmayan requestlere Cookie'nin gönderilmemesini belirtiyoruz.
//        SecurePolicy = CookieSecurePolicy.Always //HTTPS üzerinden eriþilebilir yapýyoruz.
//    };
//    /* _.SlidingExpiration = true;*/ //Expiration süresinin yarýsý kadar süre zarfýnda istekte bulunulursa eðer geri kalan yarýsýný tekrar sýfýrlayarak ilk ayarlanan süreyi tazeleyecektir.
//    /* _.ExpireTimeSpan = TimeSpan.FromMinutes(120);*/ //CookieBuilder nesnesinde tanýmlanan Expiration deðerinin varsayýlan deðerlerle ezilme ihtimaline karþýn tekrardan Cookie vadesi burada da belirtiliyor.

//});
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie
              (CookieAuthenticationDefaults.AuthenticationScheme, opts =>
              {
                  opts.LoginPath = "/Account/Login";
                  opts.ExpireTimeSpan = TimeSpan.FromDays(60);
                  opts.SlidingExpiration = true;
                  opts.Cookie.Name = "LoginCookie";
              });



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
