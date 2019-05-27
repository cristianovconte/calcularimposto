using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Domain
{
    [Serializable]
    public class NotaFiscal
    {
        public int Id { get; set; }
        public int NumeroNotaFiscal { get; set; }
        public int Serie { get; set; }
        public string NomeCliente { get; set; }

        public string EstadoDestino { get; set; }
        public string EstadoOrigem { get; set; }

        public List<NotaFiscalItem> ItensDaNotaFiscal { get; set; }

        public NotaFiscal()
        {
            ItensDaNotaFiscal = new List<NotaFiscalItem>();
        }

        public void EmitirNotaFiscal(Pedido pedido)
        {
            this.NumeroNotaFiscal = 99999;
            this.Serie = new Random().Next(Int32.MaxValue);
            this.NomeCliente = pedido.NomeCliente;

            this.EstadoDestino = pedido.EstadoDestino;
            this.EstadoOrigem = pedido.EstadoOrigem;

            foreach (PedidoItem itemPedido in pedido.ItensDoPedido)
            {
                MapearNotaFiscalItem(itemPedido);
            }
        }

        private void MapearNotaFiscalItem(PedidoItem itemPedido)
        {
            NotaFiscalItem notaFiscalItem = new NotaFiscalItem();

            notaFiscalItem.NomeProduto = itemPedido.NomeProduto;
            notaFiscalItem.CodigoProduto = itemPedido.CodigoProduto;
            notaFiscalItem.BaseCalculoIPI = itemPedido.ValorItemPedido;

            notaFiscalItem.DefinirCfop(this.EstadoOrigem, this.EstadoDestino);

            notaFiscalItem.AplicaRegraMesmoEstadoOrigemDestino(this.EstadoOrigem, this.EstadoDestino, itemPedido.Brinde);

            notaFiscalItem.AplicaRegraBaseIcms(itemPedido.ValorItemPedido);

            notaFiscalItem.AplicaAliquotaIPI(itemPedido.Brinde);

            notaFiscalItem.AplicaDesconto(this.EstadoDestino);

            this.ItensDaNotaFiscal.Add(notaFiscalItem);
        }


    }
}
