using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Concrete;
using Arch.DataAccessLayer.Abstract;
using Arch.DataAccessLayer.Concrete.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Konfig�rasyonu y�kleyin
builder.Configuration.AddJsonFile("appsettings.json");

// Veritaban� ba�lant� dizesini al�n
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContextOptions'� olu�turun ve veritaban� ba�lant�s�n� ayarlay�n
builder.Services.AddDbContext<ArchDbContext>(options =>
    options.UseSqlServer(connectionString));

// Repository ve Service s�n�flar�n� ekleyin
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICustomerDal, Arch.DataAccessLayer.EntityFramework.EFCustomerDal>();
builder.Services.AddScoped<ICustomerService, CustomerService>();



var app = builder.Build();

// Migration i�lemini ger�ekle�tirin
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ArchDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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