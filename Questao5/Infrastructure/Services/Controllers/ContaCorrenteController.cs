using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Services;
using Questao5.ViewModels;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaCorrenteController : ApiController
{
    private readonly IContaCorrenteService _contaCorrenteService;

    public ContaCorrenteController(IContaCorrenteService contaCorrenteService)
    {
        _contaCorrenteService = contaCorrenteService;
    }

    [HttpPost("movimentar")]
    public async Task<IActionResult> Movimentar([FromBody] MovimentoViewModel movimentoViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
                BadRequest(movimentoViewModel);

            var response = await _contaCorrenteService.MovimentarContaCorrente(movimentoViewModel);

            return response.HasErrors() ? CustomResponse(response.ValidationResult) : Ok(response);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpGet("saldo/{idContaCorrente}")]
    public async Task<IActionResult> ConsultarSaldo(Guid idContaCorrente)
    {
        try
        {
            if(idContaCorrente == Guid.Empty)
                return NotFound();

            var response = await _contaCorrenteService.ConsultarSaldo(idContaCorrente);

            return response.HasErrors() ? CustomResponse(response.ValidationResult) : Ok(response);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}