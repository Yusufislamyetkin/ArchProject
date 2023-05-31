using Arch.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Web;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.UI.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly RoleManager<AppRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<IActionResult> Profile()
        {
            var value = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(value);
        }
        public IActionResult EditPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (await _userManager.CheckPasswordAsync(user, model.OldPassword))
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (!result.Succeeded)
                    {
                        result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                        return View(model);
                    }
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, true);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditProfile()
        {
            var userDetail = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(userDetail);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.PhoneNumber = model.PhoneNumber;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                    return View(model);
                }
                await _userManager.UpdateSecurityStampAsync(user);
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, true);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UpdatePassword(string userId, string token)
        {
            ViewBag.userId = userId;
            ViewBag.token = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model, string userId, string token)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(token), model.Password);
            if (result.Succeeded)
            {
                ViewBag.State = true;
                await _userManager.UpdateSecurityStampAsync(user);
            }
            else
                ViewBag.State = false;
            return View();
        }
        public IActionResult PasswordReset()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordReset(ResetPasswordViewModel model)  // Mail gönderiminde Controller seçiminde ve ssl sertifikasında hata
        {
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);


                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.To.Add(user.Email);
                mail.From = new MailAddress("google_support@gmail.com", "Şifre Güncelleme", System.Text.Encoding.UTF8);
                mail.Subject = "Şifre Güncelleme Talebi";

                mail.Body = $"<a target=\"_blank\" href=\"http://localhost:5137{Url.Action("UpdatePassword", "Account", new { userId = user.Id, token = HttpUtility.UrlEncode(resetToken) })}\">Yeni şifre talebi için tıklayınız</a>";
                mail.IsBodyHtml = true;
                SmtpClient smp = new SmtpClient();
                smp.Credentials = new NetworkCredential("yusufislamyetkin131@gmail.com", "gxuixazodcckpszz");
                smp.Port = 587;
                smp.Host = "smtp.gmail.com";
                smp.EnableSsl = true;
                smp.Send(mail);

                ViewBag.State = true;
            }
            else
                ViewBag.State = false;

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            //var role = new AppRole { Name = "Customer" }; // AppRole kullanarak rol oluşturun
            //role.Id = "1";

            //var result = await _roleManager.CreateAsync(role);
            //var role2 = new AppRole { Name = "Designer" }; // AppRole kullanarak rol oluşturun

            //role2.Id = "2";
            //var result2 = await _roleManager.CreateAsync(role2);
  
            //var role3 = new AppRole { Name = "Admin" }; // AppRole kullanarak rol oluşturun
            //role3.Id = "3";
            //var result3 = await _roleManager.CreateAsync(role3);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {


            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    //İlgili kullanıcıya dair önceden oluşturulmuş bir Cookie varsa siliyoruz.
                    await _signInManager.SignOutAsync();  // Logout çalışmıyor.

                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);



                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user); //Önceki hataları girişler neticesinde +1 arttırılmış tüm değerleri 0(sıfır)a çekiyoruz.



                        if (user != null)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            if (roles.Contains("Customer"))
                            {
                                return RedirectToAction("Index", "Competition");
                            }
                        }
                        return RedirectToAction("Profile", "MyProfile");
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user); //Eğer ki başarısız bir account girişi söz konusu ise AccessFailedCount kolonundaki değer +1 arttırılacaktır. 

                        int failcount = await _userManager.GetAccessFailedCountAsync(user); //Kullanıcının yapmış olduğu başarısız giriş deneme adedini alıyoruz.
                        if (failcount == 3)
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(1))); //Eğer ki başarısız giriş denemesi 3'ü bulduysa ilgili kullanıcının hesabını kitliyoruz.
                            ModelState.AddModelError("Locked", "Art arda 3 başarısız giriş denemesi yaptığınızdan dolayı hesabınız 1 dk kitlenmiştir.");
                        }
                        else
                        {
                            if (result.IsLockedOut)
                                ModelState.AddModelError("Locked", "Art arda 3 başarısız giriş denemesi yaptığınızdan dolayı hesabınız 1 dk kitlenmiştir.");
                            else
                                ModelState.AddModelError("NotUser2", "E-posta veya şifre yanlış.");
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("NotUser", "Böyle bir kullanıcı bulunmamaktadır.");
                    ModelState.AddModelError("NotUser2", "E-posta veya şifre yanlış.");
                }
            }
            return View(model);
        }
        [Authorize]
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(AppUserViewModel appUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var appUser = new AppUser
                {
                    UserName = appUserViewModel.UserName,
                    Email = appUserViewModel.Email
                };

                appUser.Id = Guid.NewGuid().ToString();

                IdentityResult result = await _userManager.CreateAsync(appUser, appUserViewModel.Sifre);

                if (result.Succeeded)
                {

                    if (appUserViewModel.Role == "Designer")
                    {
                        await _userManager.AddToRoleAsync(appUser, AppRole.Designer);
                    }
                    else if (appUserViewModel.Role == "Admin")
                    {
                        await _userManager.AddToRoleAsync(appUser, AppRole.Admin);
                    }
                    else if (appUserViewModel.Role == "Customer")
                    {
                        await _userManager.AddToRoleAsync(appUser, AppRole.Customer);
                    }

                    return RedirectToAction("Login");
                }
                else
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                }
            }

            return View();
        }
    }
}
