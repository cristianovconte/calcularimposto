using Imposto.Core.Domain;

namespace Imposto.Core.Data
{
    public interface INotaFiscalItemRepository
    {
        bool Salvar(NotaFiscalItem notaFiscalItem);
    }
}