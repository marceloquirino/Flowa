using System.ComponentModel.DataAnnotations;

namespace OrderGenerator.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxDecimalPlacesAttribute(int decimalPlaces) : ValidationAttribute
    {
        private readonly int _decimalPlaces = decimalPlaces;

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult($"The field {validationContext.MemberName} is mandatory");

            decimal number = (decimal)value;

            int[] bits = decimal.GetBits(number);
            int scale = (bits[3] >> 16) & 31;

            if (scale > _decimalPlaces)
                return new ValidationResult($"The field {validationContext.MemberName} cant contains more then {_decimalPlaces} decimal places");

            return ValidationResult.Success!;
        }
    }
}
