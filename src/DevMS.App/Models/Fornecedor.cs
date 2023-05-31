using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppMVC.Inicial.Models
{
    public class Fornecedor : Entity
    {
        public string Nome { get; set; }
        public TipoFornecedor TipoFornecedor { get; set; }
        public Endereco Endereco { get; set; }
        public bool Ativo{ get; set; }
        
        public IEnumerable<Produto> Produtos { get; set; }
    }
}
