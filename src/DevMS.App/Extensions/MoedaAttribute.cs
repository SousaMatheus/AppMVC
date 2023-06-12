using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DevMS.App.Extensions
{
    public class MoedaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var moeda = Convert.ToDecimal(value, new CultureInfo("pt-br"));
            }
            catch(Exception)
            {
                return new ValidationResult("Moeda em formato inválido");
            }

            return ValidationResult.Success;
        }
    }
    public class MoedattributeAdpater : AttributeAdapterBase<MoedaAttribute>
    {
        public MoedattributeAdpater(MoedaAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if(context == null) throw new ArgumentNullException("context");

            MergeAttribute(context.Attributes, "dal-val", "true");
            MergeAttribute(context.Attributes, "dal-val-moeda", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "dal-val-number", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
            => "Moeda em formato inválido";
    }

    public class MoedaValidationAttributeAdpterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _provider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if(attribute is MoedaAttribute moedaAttribute)
            {
                return new MoedattributeAdpater(moedaAttribute, stringLocalizer);
            }

            return _provider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
