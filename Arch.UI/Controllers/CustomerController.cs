using Arch.BussinessLayer.Abstract;
using Arch.EntityLayer.Entities;
using Arch.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Arch.UI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly SignInManager<Customer> signInManager;

        public CustomerController(SignInManager<Customer> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Customer");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Hata işleme
            }

            // Kullanıcının var olan bir hesapla eşleşip eşleşmediğini kontrol edin
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                // Giriş başarılı, ana sayfaya yönlendirme
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Kullanıcının uygulamada yeni bir hesap oluşturması gerekiyor
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                // Gerekli işlemlerle yeni bir hesap oluşturun ve giriş yapın
                // Örneğin, Customer nesnesini oluşturun ve veritabanına kaydedin
                // Ardından giriş yapın
                var customer = new Customer
                {
                    Email = email,
                    // Diğer kullanıcı bilgilerini doldurun
                };
                // Veritabanına kaydetme işlemi

                var result = await signInManager.PasswordSignInAsync(customer, null, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Giriş başarılı, ana sayfaya yönlendirme
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Hata işleme
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
}