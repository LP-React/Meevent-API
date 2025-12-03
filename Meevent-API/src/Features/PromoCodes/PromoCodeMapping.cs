using Meevent_API.src.Core.Entities;
using static Meevent_API.Features.PromoCodes.PromoCodeDtos;

namespace Meevent_API.Features.PromoCodes
{
    public static class PromoCodeMapping
    {
        public static PromoCodeResponse MapToResponse(this PromoCode promoCode)
        {
            return new PromoCodeResponse
            {
                PromoCodeId = promoCode.Id,
                Code = promoCode.Code,
                Description = promoCode.Description,
                DiscountType = promoCode.DiscountType,
                DiscountValue = promoCode.DiscountValue,
                MinimumPurchase = promoCode.MinimumPurchase,
                MaximumDiscount = promoCode.MaximumDiscount,
                UsageLimit = promoCode.UsageLimit,
                UsageCount = promoCode.UsageCount,
                StartDate = promoCode.StartDate,
                EndDate = promoCode.EndDate,
                IsActive = promoCode.IsActive,
            };
        }

        //public static PromoCode MapToEntity(this CreatePromoCodeRequest request)
        //{
        //    return new PromoCode
        //    {
        //        Code = request.Code.ToUpper(),
        //        Description = request.Description,
        //        DiscountType = request.DiscountType,
        //        DiscountValue = request.DiscountValue,
        //        MinimumPurchase = request.MinimumPurchase,
        //        MaximumDiscount = request.MaximumDiscount,
        //        UsageLimit = request.UsageLimit,
        //        StartDate = request.StartDate,
        //        EndDate = request.EndDate,
        //    };
        //}
    }
}
