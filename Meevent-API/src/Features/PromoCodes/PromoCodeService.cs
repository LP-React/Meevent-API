using Meevent_API.src.Core.Context;
using Meevent_API.src.Core.Entities;
using Microsoft.EntityFrameworkCore;
using static Meevent_API.Features.PromoCodes.PromoCodeDtos;

namespace Meevent_API.Features.PromoCodes
{
    public class PromoCodeService
    {
        private readonly AppDbContext _context;

        public PromoCodeService(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los codigos promocionales
        public async Task<List<PromoCodeResponse>> GetAllPromoCodeAsync()
        {
            var promoCodes = await _context.PromoCodes.ToListAsync();
            return promoCodes.Select(p => p.MapToResponse()).ToList();
        }

        // Obtener un codigo promocional por ID

        //public async Task<PromoCodeResponse?> GetPromoCodeByIdAsync(int id)
        //{
        //    var promoCode = await _context.PromoCodes.FindAsync(id);
        //    return promoCode?.MapToResponse();
        //}

    }
}
