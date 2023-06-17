using Arch.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arch.UI.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet]
        public IActionResult FillCard(int id)
        {
            ViewBag.compId = id;
            return View();
        }

        [HttpPost]
        public IActionResult FillCard(CardModel card)
        {
            if (ModelState.IsValid)
            {
                // Formdan gelen verileri işleme
                // card.CardNumber, card.ExpirationDate, card.SecurityCode gibi özelliklere erişebilirsiniz

                // Verileri işleyin veya veritabanına kaydedin

                return RedirectToAction("Success");
            }

            // Formda hatalar varsa tekrar FillCard view'ını göster
            return View(card);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
