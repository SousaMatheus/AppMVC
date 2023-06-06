using DevMS.Business.Models;

namespace DevMS.Business.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task <IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId);
        Task <IEnumerable<Produto>> ObterProdutosFornecedores();
        Task <Produto> ObterProdutoFornecedor(Guid fornecedorId);
    }
}
