using DevMS.Business.Interfaces;
using DevMS.Business.Models;
using DevMS.Business.Notificacoes;
using FluentValidation;
using FluentValidation.Results;

namespace DevMS.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        public BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }
        protected void Notificar(string message)
        {
            _notificador.HandleNotificacoes(new Notificacao(message));
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
