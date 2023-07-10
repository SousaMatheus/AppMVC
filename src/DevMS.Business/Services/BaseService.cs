using DevMS.Business.Models;
using FluentValidation;
using FluentValidation.Results;

namespace DevMS.Business.Services
{
    public abstract class BaseService
    {
        protected void Notificar(string message)
        {
            //propagar para camada de apresentacao
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach(var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected bool ExecutarValidacao<TV, TE>(TV validator, TE entity) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validation = validator.Validate(entity);

            if(validation.IsValid) return true;

            Notificar(validation);
            return false;
        }
    }
}
