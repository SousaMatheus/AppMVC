using DevMS.Business.Interfaces;
using DevMS.Business.Models;
using DevMS.Business.Models.Validations;

namespace DevMS.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        public async Task Adicionar(Fornecedor fornecedor)
        {
            if(!ExecutarValidacao(new FornecedorValidation(), fornecedor) &&
                ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return;

            return;
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if(!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return;

            return;
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if(!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            return;
        }

        public async Task Remover(Guid id)
        {
            return; 
        }
    }
}
