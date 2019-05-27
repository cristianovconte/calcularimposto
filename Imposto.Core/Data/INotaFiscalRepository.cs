using Imposto.Core.Domain;

namespace Imposto.Core.Data
{
    public interface INotaFiscalRepository
    {
        bool GerarArquivoXML(NotaFiscal notaFiscal);
        int Salvar(NotaFiscal notaFiscal);
    }
}