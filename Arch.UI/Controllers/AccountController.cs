using Arch.UI.Helper;
using Arch.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.IO;
using static Arch.EntityLayer.Entities.Auth.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;

namespace Arch.UI.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly RoleManager<AppRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        private readonly string accountSid = "AC02f3abd692f633421bc425d73c5aaed8";
        private readonly string authToken = "b1feb5625afd5f94a4e92dc285e50fce";
        private readonly string verifySid = "VA2040f7dd63f10bb0e0eb6b708639fd56";
        //private readonly string verifiedNumber = "+905389351189";

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
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
            string filePath = Path.Combine(_env.ContentRootPath, "city.json");
            string json = System.IO.File.ReadAllText(filePath);
            Dictionary<string, string> cityData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            ViewBag.CityData = new SelectList(cityData, "Key", "Value");
            var value = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(UserDetailViewModel model, IFormFile? file)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            // Telefon numarası doğrulanmamışsa doğrulama kodu modalını göster
            //if (!user.PhoneNumberConfirmed)
            //{
            //    return RedirectToAction("SendVerificationCode");
            //}

            if (ModelState.IsValid)
            {

                user.PhoneNumber = model.PhoneNumber;
                user.City = model.City;
                if (file != null && file.Length > 0)
                {
                    var webRootPath = _env.WebRootPath;
                    var uploadsFolder = Path.Combine(webRootPath, "UserFiles"); // UserFiles klasörünün yolunu belirtin

                    // Eğer UserFiles klasörü yoksa oluşturun
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    user.ProfilPhoto = "/" + Path.Combine("UserFiles", fileName).Replace("\\", "/"); // Dosya yolunu /UserFiles/fileName şeklinde belirtin
                }

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                    return Json(false); // İşlem başarısız olduğunda false döndür
                }

                await _userManager.UpdateSecurityStampAsync(user);
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, true);

                return Json(true); // İşlem başarılı olduğunda true döndür
            }

            return Json(false); // ModelState geçerli değilse veya hata varsa false döndür
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
        public async Task<IActionResult> Login(int? status)
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

            if (status == 1)
            {
                ViewBag.Status = 1;
                ViewBag.Message = "Kayıt işlemi tamamlandı";
            }

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
                        return RedirectToAction("AllCompetitions", "Competition");
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
                    Email = appUserViewModel.Email,
                    PhoneNumber = appUserViewModel.PhoneNumber,
                    City = "0",
                    ProfilPhoto = "/UserFiles/defaultPhoto.png"

                };

                appUser.Id = Guid.NewGuid().ToString();

                IdentityResult result = await _userManager.CreateAsync(appUser, appUserViewModel.Sifre);

                if (result.Succeeded)
                {
                    // Rol atama
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


                    // Sms Gönderme
                    TwilioClient.Init(accountSid, authToken);

                    var verification = VerificationResource.Create(
                        to: FormatHelper.FormatPhoneNumber(appUser.PhoneNumber),
                        channel: "sms",
                        pathServiceSid: verifySid
                    );

                    return RedirectToAction("VerifyCode", appUser);
                }
                else
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult VerifyCode(AppUser? appUser)
        {
            ViewBag.appUser = appUser.Id;
            ViewBag.phone = appUser.PhoneNumber;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCodeAuth(string digit1, string digit2, string digit3, string digit4, string digit5, string digit6, string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);

            TwilioClient.Init(accountSid, authToken);

            var verificationCheck = VerificationCheckResource.Create(
                to: FormatHelper.FormatPhoneNumber(user.PhoneNumber),
                code: digit1 + digit2 + digit3 + digit4 + digit5 + digit6,
                pathServiceSid: verifySid
            );

            if (verificationCheck.Status == "approved")
            {
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);
            }

            return Json(false);
        }



    }
}
