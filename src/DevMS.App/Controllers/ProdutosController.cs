using AutoMapper;
using DevMS.App.Extensions;
using DevMS.App.ViewModels;
using DevMS.Business.Interfaces;
using DevMS.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevMS.App.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutoRepository produtoRepository,
                                  IMapper mapper,
                                  IFornecedorRepository fornecedorRepository,
                                  IProdutoService produtoService,
                                  ILogger<ProdutosController> logger,
                                  INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _logger = logger;
            _produtoService = produtoService;
        }

        [AllowAnonymous]
        [Route("listar-produtos")]
        public async Task<IActionResult> Index()
        {
            _logger.LogTrace("index-produto");

            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores()));
        }

        [AllowAnonymous]
        [Route("detalhes-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if(produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto","Adicionar")]
        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        [HttpPost]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);

            if(!ModelState.IsValid)
                return View(produtoViewModel);

            var prefixo = Guid.NewGuid().ToString() + "_";

            if(! await UploadImagem(produtoViewModel.ImagemUpload, prefixo))
            {
                return View(produtoViewModel);
            }

            produtoViewModel.Imagem = $"{prefixo}{produtoViewModel.ImagemUpload.FileName}";
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            if(!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if(produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if(id != produtoViewModel.Id)
                return NotFound();

            var produtoAtualizacao = await ObterProduto(id);
            produtoViewModel.Fornecedor = produtoAtualizacao.Fornecedor;
            produtoViewModel.Imagem = produtoAtualizacao.Imagem;

            if(!ModelState.IsValid)
                return View(produtoViewModel);

            if(produtoViewModel.ImagemUpload != null)
            {
                var prefixo = Guid.NewGuid().ToString() + "_";

                if(!await UploadImagem(produtoViewModel.ImagemUpload, prefixo))
                {
                    return View(produtoViewModel);
                }

                produtoAtualizacao.Imagem = $"{prefixo}{produtoViewModel.ImagemUpload.FileName}";
            }

            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            if(!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if(produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if(produtoViewModel == null)
                return NotFound();

            await _produtoService.Remover(id);

            if(!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid fornecedorId)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(fornecedorId));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<bool> UploadImagem(IFormFile imagemUpload, string prefixo)
        {
            if(imagemUpload.Length <= 0 || imagemUpload ==null) return false;

            var path = Path.Combine($@"{Directory.GetCurrentDirectory()}\wwwroot\imagens\{prefixo}{imagemUpload.FileName}");

            if(System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Nome do arquivo já existe");
                return false;
            }

            using(var fs = new FileStream(path, FileMode.Create))
            {
                await imagemUpload.CopyToAsync(fs);
            }

            return true;
        }
    }
}
