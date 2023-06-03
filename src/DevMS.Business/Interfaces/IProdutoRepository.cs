using AppMVC.Inicial.Models;

namespace DevMS.Business.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task <IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId);
        Task <IEnumerable<Produto>> ObterProdutosFornecedores(Guid fornecedorId);
        Task <Produto> ObterProdutoFornecedor(Guid fornecedorId);
    }
}
