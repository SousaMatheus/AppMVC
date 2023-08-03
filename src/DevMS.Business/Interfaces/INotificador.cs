using DevMS.Business.Notificacoes;

namespace DevMS.Business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void HandleNotificacoes(Notificacao notificacao);
    }
}
