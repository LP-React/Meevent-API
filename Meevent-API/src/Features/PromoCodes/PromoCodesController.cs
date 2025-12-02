using Meevent_API.Features.PromoCodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meevent_API.Controllers
{
    [Route("api/promo-codes")]
    [ApiController]
    public class PromoCodesController : ControllerBase
    {
        private readonly PromoCodeService _promoCodeService;

        public PromoCodesController(PromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        // GET: api/promo-codes
        [HttpGet]
        public async Task<IActionResult> GetAllPromoCodes()
        {
            var promoCodes = await _promoCodeService.GetAllPromoCodeAsync();

            if (promoCodes == null || !promoCodes.Any())
            {
                return NoContent();
            }
            return Ok(promoCodes);
        }

        // GET: api/promo-codes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromoCodeById(int id)
        {
            var promoCode = await _promoCodeService.GetPromoCodeByIdAsync(id);
            if (promoCode == null)
            {
                return NotFound();
            }
            return Ok(promoCode);
        }
    }
}
