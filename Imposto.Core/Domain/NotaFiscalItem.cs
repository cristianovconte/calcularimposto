using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Domain
{
    [Serializable]
    public class NotaFiscalItem
    {
        public int Id { get; set; }
        public int IdNotaFiscal { get; set; }
        public string Cfop { get; set; }
        public string TipoIcms { get; set; }
        public double BaseIcms { get; set; }
        public double AliquotaIcms { get; set; }
        public double ValorIcms
        {
            get
            {
                return this.BaseIcms * this.AliquotaIcms;
            }
        }
        public string NomeProduto { get; set; }
        public string CodigoProduto { get; set; }
        public double BaseCalculoIPI { get; set; }
        public int AliquotaIPI { get; set; }
        public double ValorIPI
        {
            get
            {
                return BaseCalculoIPI * AliquotaIPI;
            }
        }
        public int Desconto
        {
            get;  set;
        }

        public void AplicaDesconto(string estadoDestino)
        {
            string[] sudeste = { "SP", "RJ", "ES", "MG" };

            if (sudeste.Contains(estadoDestino))
                Desconto = 10;
        }

        public void AplicaAliquotaIPI(bool brinde)
        {
            this.AliquotaIPI = brinde ? 0 : 10;
        }

        public void AplicaRegraBaseIcms(double valorPedido)
        {
            if (this.Cfop == "6.009")
            {
                this.BaseIcms = valorPedido * 0.90; //redução de base
            }
            else
            {
                this.BaseIcms = valorPedido;
            }
        }

        public void AplicaRegraMesmoEstadoOrigemDestino(string estadoOrigem, string estadoDestino, bool brinde)
        {
            if (estadoOrigem == estadoDestino || brinde)
            {
                this.TipoIcms = "60";
                this.AliquotaIcms = 0.18;
            }
            else
            {
                this.TipoIcms = "10";
                this.AliquotaIcms = 0.17;
            }
        }

        public void DefinirCfop(string estadoOrigem, string estadoDestino)
        {
            if ((estadoOrigem == "SP") && (estadoDestino == "RJ"))
            {
                this.Cfop = "6.000";
            }
            else if ((estadoOrigem == "SP") && (estadoDestino == "PE"))
            {
                this.Cfop = "6.001";
            }
            else if ((estadoOrigem == "SP") && (estadoDestino == "MG"))
            {
                this.Cfop = "6.002";
            }
            else if ((estadoOrigem == "SP") && (estadoDestino == "PB"))
            {
                this.Cfop = "6.003";
            }
            else if ((estadoOrigem == "SP") && (estadoDestino == "PR"))
            {
                this.Cfop = "6.004";
            }
            else if ((estadoOrigem == "SP") && (estadoDestino == "PI"))
            {
                this.Cfop = "6.005";
            }
            else if ((estadoOrigem == "SP") && (estadoDestino == "RO"))
            {
                this.Cfop = "6.006";
            }
            else if ((estadoOrigem == "SP") && (estadoDestino == "SE"))
            {
                this.Cfop = "6.007";
            }
            else if ((estadoOrigem == "SP") && (estadoDestino == "TO"))
            {
                this.Cfop = "6.008";
            }
            else if ((estadoOrigem == "SP") && (estadoDestino == "SE"))
            {
                this.Cfop = "6.009";
            }
            else if ((estadoOrigem == "SP") && (estadoDestino == "PA"))
            {
                this.Cfop = "6.010";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "RJ"))
            {
                this.Cfop = "6.000";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "PE"))
            {
                this.Cfop = "6.001";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "MG"))
            {
                this.Cfop = "6.002";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "PB"))
            {
                this.Cfop = "6.003";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "PR"))
            {
                this.Cfop = "6.004";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "PI"))
            {
                this.Cfop = "6.005";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "RO"))
            {
                this.Cfop = "6.006";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "SE"))
            {
                this.Cfop = "6.007";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "TO"))
            {
                this.Cfop = "6.008";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "SE"))
            {
                this.Cfop = "6.009";
            }
            else if ((estadoOrigem == "MG") && (estadoDestino == "PA"))
            {
                this.Cfop = "6.010";
            }
        }

    }
}