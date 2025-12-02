using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Meevent_API.Features.PromoCodes
{
    public class PromoCodeDtos
    {
        // Request para crear un nuevo PromoCode
        public class CreatePromoCodeRequest
        {
            [Required]
            [StringLength(50, ErrorMessage = "Code must not exceed 50 characters")]
            [JsonPropertyName("code")]
            public string Code { get; set; } = string.Empty;

            [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [Required(ErrorMessage = "Discount type is required")]
            
            [JsonPropertyName("discount_type")]
            public string DiscountType { get; set; } = "Percentage";

            [Required(ErrorMessage = "Discount value is required")]
            [Range(0.01, double.MaxValue, ErrorMessage = "Discount value must be greater than 0")]
            [JsonPropertyName("discount_value")]
            public decimal DiscountValue { get; set; }

            [Range(0.01, double.MaxValue, ErrorMessage = "Minimum purchase must be greater than 0")]
            [JsonPropertyName("minimum_purchase")]
            public decimal? MinimumPurchase { get; set; }

            [Range(0.01, double.MaxValue, ErrorMessage = "Maximum discount must be greater than 0")]
            [JsonPropertyName("maximum_discount")]
            public decimal? MaximumDiscount { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Usage limit must be at least 1")]
            [JsonPropertyName("usage_limit")]
            public int? UsageLimit { get; set; }

            [Required(ErrorMessage = "Start date is required")]
            [JsonPropertyName("start_date")]
            public DateTime StartDate { get; set; }

            [Required(ErrorMessage = "End date is required")]
            [JsonPropertyName("end_date")]
            public DateTime EndDate { get; set; }
        }

        // Request para actualizar un PromoCode
        public class UpdatePromoCodeRequest
        {
            [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [Range(0.01, double.MaxValue, ErrorMessage = "Minimum purchase must be greater than 0")]
            [JsonPropertyName("minimum_purchase")]
            public decimal? MinimumPurchase { get; set; }

            [Range(0.01, double.MaxValue, ErrorMessage = "Maximum discount must be greater than 0")]
            [JsonPropertyName("maximum_discount")]
            public decimal? MaximumDiscount { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Usage limit must be at least 1")]
            [JsonPropertyName("usage_limit")]
            public int? UsageLimit { get; set; }

            [JsonPropertyName("end_date")]
            public DateTime? EndDate { get; set; }

            [JsonPropertyName("is_active")]
            public bool? IsActive { get; set; }
        }

        // ================== RESPONSES ==================

        // Response con datos del PromoCode
        public class PromoCodeResponse
        {
            [JsonPropertyName("promo_code_id")]
            public int PromoCodeId { get; set; }

            [JsonPropertyName("code")]
            public string Code { get; set; } = string.Empty;

            [JsonPropertyName("description")]
            public string? Description { get; set; }

            [JsonPropertyName("discount_type")]
            public string DiscountType { get; set; } = string.Empty;

            [JsonPropertyName("discount_value")]
            public decimal DiscountValue { get; set; }

            [JsonPropertyName("minimum_purchase")]
            public decimal? MinimumPurchase { get; set; }

            [JsonPropertyName("maximum_discount")]
            public decimal? MaximumDiscount { get; set; }

            [JsonPropertyName("usage_limit")]
            public int? UsageLimit { get; set; }

            [JsonPropertyName("usage_count")]
            public int UsageCount { get; set; }

            [JsonPropertyName("start_date")]
            public DateTime StartDate { get; set; }

            [JsonPropertyName("end_date")]
            public DateTime EndDate { get; set; }

            [JsonPropertyName("is_active")]
            public bool IsActive { get; set; }
        }
    }
}
