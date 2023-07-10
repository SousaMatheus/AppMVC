using DevMS.Business.Interfaces;
using DevMS.Business.Models;
using DevMS.Business.Models.Validations;

namespace DevMS.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        public async Task Adicionar(Produto produto)
        {
            if(!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            return;
        }

        public async Task Atualizar(Produto produto)
        {
            if(!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            return;
        }

        public async Task Remover(Guid id)
        {
            return;
        }
    }
}
