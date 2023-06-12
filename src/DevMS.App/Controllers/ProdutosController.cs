using AutoMapper;
using DevMS.App.ViewModels;
using DevMS.Business.Interfaces;
using DevMS.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevMS.App.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository,
                                  IMapper mapper,
                                  IFornecedorRepository fornecedorRepository)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores()));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if(produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if(produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            await _produtoRepository.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if(produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if(produtoViewModel == null)
                return NotFound();

            await _produtoRepository.Remover(id);

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
