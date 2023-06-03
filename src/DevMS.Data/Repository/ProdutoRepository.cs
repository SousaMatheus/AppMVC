using AppMVC.Inicial.Models;
using DevMS.Business.Interfaces;
using DevMS.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevMS.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(MeuDbContext dbContext) : base(dbContext) {}

        public async Task<Produto> ObterProdutoFornecedor(Guid fornecedorId)
        {
            return await Db.Produtos.AsNoTracking().Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == fornecedorId);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores(Guid fornecedorId)
        {
            return await Db.Produtos.AsNoTracking().Include(f => f.Fornecedor)
                .OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await Buscar(p => p.FornecedorId == fornecedorId);
        }
    }
}
