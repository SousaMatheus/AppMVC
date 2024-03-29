﻿using DevMS.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevMS.App.Controllers
{
    public class BaseController : Controller
    {
        private readonly INotificador _notificador;
        public BaseController(INotificador notificador)
        {
            _notificador = notificador;
        }

        public bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }
    }
}
