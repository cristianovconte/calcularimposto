using Imposto.Core.Data;
using Imposto.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Service
{
    public class NotaFiscalService
    {
        INotaFiscalRepository _notaFiscalRepository;
        INotaFiscalItemRepository _notaFiscalItemRepository;
        public NotaFiscalService()
        {
            _notaFiscalRepository = new NotaFiscalRepository();
            _notaFiscalItemRepository = new NotaFiscalItemRepository();
        }

        public void GerarNotaFiscal(Domain.Pedido pedido)
        {
            NotaFiscal notaFiscal = new NotaFiscal();
            notaFiscal.EmitirNotaFiscal(pedido);

            var gerouXML = _notaFiscalRepository.GerarArquivoXML(notaFiscal);

            if (gerouXML)
            {
                var notaFiscalId = _notaFiscalRepository.Salvar(notaFiscal);

                if (notaFiscalId > 0)
                {
                    foreach (var item in notaFiscal.ItensDaNotaFiscal)
                    {
                        item.IdNotaFiscal = notaFiscalId;
                        _notaFiscalItemRepository.Salvar(item);
                    }
                }
            }
        }
    }
}
