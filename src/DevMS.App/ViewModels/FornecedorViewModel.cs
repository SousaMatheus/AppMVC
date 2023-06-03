using AppMVC.Inicial.Models;
using AppMVC.Inicial;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DevMS.App.ViewModels
{
    public class FornecedorViewModel
    {
        [Key]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(14, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public string Documento { get; set; }

        public int TipoFornecedor { get; set; }
        public EnderecoViewModel Endereco { get; set; }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }

        public IEnumerable<ProdutoViewModel> Produtos { get; set; }
    }
}
