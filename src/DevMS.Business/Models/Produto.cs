using System.ComponentModel.DataAnnotations;

namespace AppMVC.Inicial.Models
{
    public class Produto : Entity
    {
        public Guid FornecedorId { get; set; }
        public string Nome { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string Imagem { get; set; } = null!;
        public decimal Valor{ get; set; }
        public DateTime DataCadastro{ get; set; }
        public bool Ativo { get; set; }

        public Fornecedor Fornecedor { get; set; } = null!;


    }
}
