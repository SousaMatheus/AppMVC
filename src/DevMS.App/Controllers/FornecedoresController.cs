﻿using AutoMapper;
using DevMS.App.Extensions;
using DevMS.App.ViewModels;
using DevMS.Business.Interfaces;
using DevMS.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevMS.App.Controllers
{
    [Authorize]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FornecedoresController> _logger;

        public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                      IProdutoRepository produtoRepository,
                                      IMapper mapper,
                                      IFornecedorService fornecedorService,
                                      ILogger<FornecedoresController> logger,
                                      INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _logger = logger;
            _fornecedorService = fornecedorService;
        }

        [AllowAnonymous]
        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            _logger.LogTrace("index-fornecedores");

            var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return View(fornecedores);
        }

        [AllowAnonymous]
        [Route("dados-de-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);

            if(fornecedorViewModel == null)
                return NotFound();

            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor","Adicionar")]
        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Fornecedor", "Adicionar")]
        [Route("novo-fornecedor")]
        [HttpPost]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if(!ModelState.IsValid)
                return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Adicionar(fornecedor);

            if(!OperacaoValida()) return View(fornecedorViewModel);

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Fornecedor", "Editar")]
        [Route("editar-fornecedor")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);

            if(fornecedorViewModel == null)
                return NotFound();

            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Editar")]
        [Route("editar-fornecedor")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {

            if(!ModelState.IsValid)
                return View(fornecedorViewModel);

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));

            if(!OperacaoValida()) return View(fornecedorViewModel);

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Fornecedor", "Excluir")]
        [Route("excluir-fornecedor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if(fornecedorViewModel == null)
                return NotFound();

            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Excluir")]
        [Route("excluir-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedor = ObterFornecedorEndereco(id);

            if(fornecedor == null)
                return Problem("O fornecedor não existe");

            await _fornecedorService.Remover(id);
            if(!OperacaoValida()) return View(fornecedor);

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Fornecedor", "Editar")]
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if(fornecedor == null)
            {
                return NotFound();
            }

            return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
        }

        [ClaimsAuthorize("Fornecedor", "Editar")]
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if(!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));

            if(!OperacaoValida()) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            var url = Url.Action("ObterEndereco", "Fornecedores", new {id = fornecedorViewModel.Endereco.FornecedorId});
            return Json(new { success = true, url });
        }

        [AllowAnonymous]
        [Route("obter-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null)  return NotFound();

            return PartialView("_DetalhesEndereco", fornecedor);
        }
        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id)); 
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }
    }
}
