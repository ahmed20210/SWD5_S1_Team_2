using Business.Services.PaymentService;
using Microsoft.AspNetCore.Mvc;
using Business.ViewModels.PaymentViewModels;
namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAllAsync();
            return View(payments);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _paymentService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var editModel = await _paymentService.GetByIdForUpdateAsync(id);
            if (editModel == null) return NotFound();

            return View(editModel);
        }
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
            {
                return NotFound($"Payment with ID {id} not found.");
            }
            return Ok(payment);
        }




        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updatedPayment = await _paymentService.EditAsync(id, model);
                if (updatedPayment == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null) return NotFound();

            return View(payment);
        }



    }
}