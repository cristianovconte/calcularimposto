using System;
using Imposto.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imposto.UnitTest
{
    [TestClass]
    public class NotaFiscalUnitTest
    {
        /// <summary>
        /// Correção BUG
        /// Para o estado de origem SP e destino RO o sistema deveria definir a CFOP como 6.006, 
        /// corrigir o erro no sistema para que seja definido a CFOP correta.
        /// </summary>
        [TestMethod]
        public void Emitir_NotaFiscal_Origem_SP_Destino_RO()
        {
            NotaFiscal nf = new NotaFiscal();

            Pedido pedido = new Pedido();
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "RO";
            pedido.NomeCliente = "teste1";


            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = true,
                    CodigoProduto = "100",
                    NomeProduto = "Produto1",
                    ValorItemPedido = 200
                });

            nf.EmitirNotaFiscal(pedido);

            Assert.AreEqual(nf.ItensDaNotaFiscal[0].Cfop, "6.006");
        }

        [TestMethod]
        public void Emitir_NotaFiscal_Aliquota_Igual_0()
        {
            NotaFiscal nf = new NotaFiscal();

            Pedido pedido = new Pedido();
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "RO";
            pedido.NomeCliente = "teste1";


            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = true,
                    CodigoProduto = "100",
                    NomeProduto = "Produto1",
                    ValorItemPedido = 200
                });

            nf.EmitirNotaFiscal(pedido);

            Assert.AreEqual(nf.ItensDaNotaFiscal[0].AliquotaIPI, 0);
        }

        [TestMethod]
        public void Emitir_NotaFiscal_Aliquota_Igual_10()
        {
            NotaFiscal nf = new NotaFiscal();

            Pedido pedido = new Pedido();
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "RO";
            pedido.NomeCliente = "teste1";


            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = false,
                    CodigoProduto = "100",
                    NomeProduto = "Produto1",
                    ValorItemPedido = 200
                });

            nf.EmitirNotaFiscal(pedido);

            Assert.AreEqual(nf.ItensDaNotaFiscal[0].AliquotaIPI, 10);
        }

        [TestMethod]
        public void Emitir_NotaFiscal_Aliquota_Valor_IPI_100()
        {
            NotaFiscal nf = new NotaFiscal();

            Pedido pedido = new Pedido();
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "RO";
            pedido.NomeCliente = "teste1";


            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = false,
                    CodigoProduto = "100",
                    NomeProduto = "Produto1",
                    ValorItemPedido = 10
                });

            nf.EmitirNotaFiscal(pedido);

            Assert.AreEqual(nf.ItensDaNotaFiscal[0].ValorIPI, 100);
        }

        [TestMethod]
        public void Emitir_NotaFiscal_Aliquota_Valor_IPI_0()
        {
            NotaFiscal nf = new NotaFiscal();

            Pedido pedido = new Pedido();
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "RO";
            pedido.NomeCliente = "teste1";


            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = true,
                    CodigoProduto = "100",
                    NomeProduto = "Produto1",
                    ValorItemPedido = 10
                });

            nf.EmitirNotaFiscal(pedido);

            Assert.AreEqual(nf.ItensDaNotaFiscal[0].ValorIPI, 0);
        }

        [TestMethod]
        public void Emitir_NotaFiscal_Estado_Origem_Igual_Estado_Destino_TipoIcms_Igual_60()
        {
            NotaFiscal nf = new NotaFiscal();

            Pedido pedido = new Pedido();
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "SP";
            pedido.NomeCliente = "teste1";


            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = true,
                    CodigoProduto = "100",
                    NomeProduto = "Produto1",
                    ValorItemPedido = 10
                });

            nf.EmitirNotaFiscal(pedido);

            Assert.AreEqual(nf.ItensDaNotaFiscal[0].TipoIcms, "60");
        }

        [TestMethod]
        public void Emitir_NotaFiscal_Estado_Origem_Igual_Estado_Destino_AliquotaIcms_Igual_18()
        {
            NotaFiscal nf = new NotaFiscal();

            Pedido pedido = new Pedido();
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "SP";
            pedido.NomeCliente = "teste1";


            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = true,
                    CodigoProduto = "100",
                    NomeProduto = "Produto1",
                    ValorItemPedido = 10
                });

            nf.EmitirNotaFiscal(pedido);

            Assert.AreEqual(nf.ItensDaNotaFiscal[0].AliquotaIcms, 0.18);
        }

        /// <summary>
        /// TODO: ERRO NA REGRA DE NEGOCIO POIS EXISTEM 2 CONDICOES PARA 
        /// (estadoOrigem == "SP") && (estadoDestino == "SE")
        /// ((estadoOrigem == "MG") && (estadoDestino == "SE"))
        /// POR ISSO O TESTE FALHA 6009 O VALOR DO ASSERT DEVERIA SER 9
        /// </summary>
        [TestMethod]
        public void Emitir_NotaFiscal_CFOP_6009()
        {
            NotaFiscal nf = new NotaFiscal();

            Pedido pedido = new Pedido();
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "SE";
            pedido.NomeCliente = "teste1";


            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = true,
                    CodigoProduto = "100",
                    NomeProduto = "Produto1",
                    ValorItemPedido = 10
                });

            nf.EmitirNotaFiscal(pedido);

            Assert.AreEqual(nf.ItensDaNotaFiscal[0].BaseIcms, 10);
        }

        [TestMethod]
        public void Emitir_NotaFiscal_Com_Desconto_Estado_Destino_Sudeste()
        {
            NotaFiscal nf = new NotaFiscal();

            Pedido pedido = new Pedido();
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "MG";
            pedido.NomeCliente = "teste1";


            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = true,
                    CodigoProduto = "100",
                    NomeProduto = "Produto1",
                    ValorItemPedido = 10
                });

            nf.EmitirNotaFiscal(pedido);

            Assert.AreEqual(nf.ItensDaNotaFiscal[0].Desconto, 10);
        }

        [TestMethod]
        public void Emitir_NotaFiscal_Sem_Desconto_Estado_Destino_Sudeste()
        {
            NotaFiscal nf = new NotaFiscal();

            Pedido pedido = new Pedido();
            pedido.EstadoOrigem = "SP";
            pedido.EstadoDestino = "SE";
            pedido.NomeCliente = "teste1";


            pedido.ItensDoPedido.Add(
                new PedidoItem()
                {
                    Brinde = true,
                    CodigoProduto = "100",
                    NomeProduto = "Produto1",
                    ValorItemPedido = 10
                });

            nf.EmitirNotaFiscal(pedido);

            Assert.AreEqual(nf.ItensDaNotaFiscal[0].Desconto, 0);
        }
    }
}
