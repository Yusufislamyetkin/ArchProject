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

// AutoMapper'� yap�land�r�n
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
    _.Password.RequiredLength = 5; //En az ka� karakterli olmas� gerekti�ini belirtiyoruz.
    _.Password.RequireNonAlphanumeric = false; //Alfanumerik zorunlulu�unu kald�r�yoruz.
    _.Password.RequireLowercase = false; //K���k harf zorunlulu�unu kald�r�yoruz.
    _.Password.RequireUppercase = false; //B�y�k harf zorunlulu�unu kald�r�yoruz.
    _.Password.RequireDigit = false; //0-9 aras� say�sal karakter zorunlulu�unu kald�r�yoruz.

    _.User.RequireUniqueEmail = true; //Email adreslerini tekille�tiriyoruz.
    _.User.AllowedUserNameCharacters = "abc�defghi�jklmno�pqrs�tu�vwxyzABC�DEFGHI�JKLMNO�PQRS�TU�VWXYZ0123456789-._@+"; //Kullan�c� ad�nda ge�erli olan karakterleri belirtiyoruz.
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
//        Name = "AspNetCoreIdentityExampleCookie", //Olu�turulacak Cookie'yi isimlendiriyoruz.
//        HttpOnly = false, //K�t� niyetli insanlar�n client-side taraf�ndan Cookie'ye eri�mesini engelliyoruz.
//        //Expiration = TimeSpan.FromMinutes(2), //Olu�turulacak Cookie'nin vadesini belirliyoruz.
//        SameSite = SameSiteMode.Lax, //Top level navigasyonlara sebep olmayan requestlere Cookie'nin g�nderilmemesini belirtiyoruz.
//        SecurePolicy = CookieSecurePolicy.Always //HTTPS �zerinden eri�ilebilir yap�yoruz.
//    };
//    /* _.SlidingExpiration = true;*/ //Expiration s�resinin yar�s� kadar s�re zarf�nda istekte bulunulursa e�er geri kalan yar�s�n� tekrar s�f�rlayarak ilk ayarlanan s�reyi tazeleyecektir.
//    /* _.ExpireTimeSpan = TimeSpan.FromMinutes(120);*/ //CookieBuilder nesnesinde tan�mlanan Expiration de�erinin varsay�lan de�erlerle ezilme ihtimaline kar��n tekrardan Cookie vadesi burada da belirtiliyor.

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
