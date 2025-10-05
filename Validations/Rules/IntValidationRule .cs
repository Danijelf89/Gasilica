using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Gasilica.Validations.Rules
{
    public class IntValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse((value ?? "").ToString(), out _))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Dozvoljeni su samo brojevi");
        }
    }
}
