using DevMS.Business.Models;
using DevMS.Business.Interfaces;
using DevMS.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevMS.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(MeuDbContext meuDbContext) : base(meuDbContext) { }

        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
        {
            return await Db.Enderecos.AsNoTracking()
                .Include(e => e.Fornecedor)
                .FirstOrDefaultAsync(e => e.FornecedorId == fornecedorId);
        }
    }
}
