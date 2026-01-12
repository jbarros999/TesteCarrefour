using Lancamentos.Api.Contracts;
using Lancamentos.Api.Domain.Enums;

namespace Lancamentos.Api.Services;

public static class LancamentoValidatorService
{
    public static (bool valido, string erro) Validar(CriarLancamentoRequest req)
    {
        if (req.Valor <= 0)
            return (false, "Valor do lançamento deve ser maior que zero.");

        if (!Enum.IsDefined(typeof(TipoLancamento), req.Tipo))
            return (false, "Tipo do lançamento inválido.");

        if (req.Data == default)
            return (false, "Data do lançamento é obrigatória.");

        if (req.Data > DateTime.UtcNow.AddDays(1))
            return (false, "Data do lançamento não pode ser futurada demais.");

        if (req.Descricao?.Length > 200)
            return (false, "Descrição não pode exceder 200 caracteres.");

        return (true, string.Empty);
    }
}
