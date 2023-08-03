using DevMS.Business.Interfaces;
using DevMS.Business.Models;
using DevMS.Business.Models.Validations;

namespace DevMS.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository, 
                                 IEnderecoRepository enderecoRepository, 
                                 INotificador notificador) : base (notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            if(!ExecutarValidacao(new FornecedorValidation(), fornecedor) ||
               !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return;

            if(_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("Já existe um fornecedor com o documento informado!");
                return;
            }

            await _fornecedorRepository.Adicionar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if(!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return;

            if(_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com o documento informado!");
                return;
            }

            await _fornecedorRepository.Atualizar(fornecedor);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if(!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            await _enderecoRepository.Atualizar(endereco);
        }

        public async Task Remover(Guid id)
        {
            if(_fornecedorRepository.Buscar(f => f.Id == id).Result.Any())
            {
                Notificar("O fornecedor tem produtos cadastrados!");
                return;
            }

            await _fornecedorRepository.Remover(id);
        }

        public void Dispose()
        {
            _enderecoRepository?.Dispose();
            _fornecedorRepository?.Dispose();
        }
    }
}
