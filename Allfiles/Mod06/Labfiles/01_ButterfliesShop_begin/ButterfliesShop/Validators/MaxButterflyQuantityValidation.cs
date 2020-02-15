using ButterfliesShop.Models;
using ButterfliesShop.Services;
using System.ComponentModel.DataAnnotations;

namespace ButterfliesShop.Validators
{
    public class MaxButterflyQuantityValidation : ValidationAttribute
    {
        private readonly int _maxAmount;

        public MaxButterflyQuantityValidation(int maxAmount)
        {
            _maxAmount = maxAmount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (IButterfliesQuantityService)validationContext.GetService(typeof(IButterfliesQuantityService));
            var butterfly = (Butterfly)validationContext.ObjectInstance;
            if (butterfly != null)
            {
                var quantity = service.GetButterflyFamilyQuantity(butterfly.ButterflyFamily.Value);
                var sumQuantity = quantity + butterfly.Quantity;
                if (sumQuantity > _maxAmount)
                {
                    return new ValidationResult($"Limit of butterflies from the same family in the store is {_maxAmount} butterflies. Currently there are {quantity}");
                }
                return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }
    }
}
