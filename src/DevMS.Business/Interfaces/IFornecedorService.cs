using DevMS.Business.Models;

namespace DevMS.Business.Interfaces
{
    public interface IFornecedorService : IDisposable
    {
        Task Adicionar (Fornecedor fornecedor);
        Task Atualizar(Fornecedor forncedor);
        Task AtualizarEndereco (Endereco endereco);
        Task Remover(Guid id);
    }
}
