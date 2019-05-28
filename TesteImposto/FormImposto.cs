using Imposto.Core.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Imposto.Core.Domain;

namespace TesteImposto
{
    public partial class FormImposto : Form
    {
        private Pedido pedido = new Pedido();
        private string[] estados = { "AC", "AL", "AM", "AP", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS",
            "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RO", "RS", "RR", "SC", "SE", "SP", "TO" };

        public FormImposto()
        {
            InitializeComponent();

            SetupGrid();
        }

        private void SetupGrid()
        {
            dataGridViewPedidos.AutoGenerateColumns = true;
            dataGridViewPedidos.DataSource = GetTablePedidos();
            ResizeColumns();
        }

        private void ResizeColumns()
        {
            double mediaWidth = dataGridViewPedidos.Width / dataGridViewPedidos.Columns.GetColumnCount(DataGridViewElementStates.Visible);

            for (int i = dataGridViewPedidos.Columns.Count - 1; i >= 0; i--)
            {
                var coluna = dataGridViewPedidos.Columns[i];
                coluna.Width = Convert.ToInt32(mediaWidth);
            }
        }

        private object GetTablePedidos()
        {
            DataTable table = new DataTable("pedidos");
            table.Columns.Add(new DataColumn("Nome do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Codigo do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Valor", typeof(decimal)));
            table.Columns.Add(new DataColumn("Brinde", typeof(bool)));

            return table;
        }

        public bool ValidadarFormulario()
        {
            DataTable table = (DataTable)dataGridViewPedidos.DataSource;

            if (string.IsNullOrWhiteSpace(txtEstadoOrigem.Text) ||
              string.IsNullOrWhiteSpace(txtEstadoDestino.Text) ||
              string.IsNullOrWhiteSpace(textBoxNomeCliente.Text) ||
              (table == null || (table != null && table.Rows.Count == 0)))
            {
                MessageBox.Show("Todos os campos são obrigatórios e deve existir ao menos um item no pedido.");
                return false;
            }

            if(!estados.Contains(txtEstadoOrigem.Text))
            {
                MessageBox.Show("Estado de origem inválido.");
                return false;
            }

            if (!estados.Contains(txtEstadoDestino.Text))
            {
                MessageBox.Show("Estado de destino inválido.");
                return false;
            }

            return true;
        }

        private void buttonGerarNotaFiscal_Click(object sender, EventArgs e)
        {
            if (!ValidadarFormulario())
                return;
        
            NotaFiscalService service = new NotaFiscalService();
            pedido.EstadoOrigem = txtEstadoOrigem.Text;
            pedido.EstadoDestino = txtEstadoDestino.Text;
            pedido.NomeCliente = textBoxNomeCliente.Text;

            DataTable table = (DataTable)dataGridViewPedidos.DataSource;

            foreach (DataRow row in table.Rows)
            {
                bool brinde = false;
                bool.TryParse(row["Brinde"].ToString(), out brinde);

                double valor = 0;
                double.TryParse(row["Valor"].ToString(), out valor);

                pedido.ItensDoPedido.Add(
                    new PedidoItem()
                    {
                        Brinde = brinde,
                        CodigoProduto = row["Codigo do produto"].ToString(),
                        NomeProduto = row["Nome do produto"].ToString(),
                        ValorItemPedido = valor
                    });
            }
            try
            {
                service.GerarNotaFiscal(pedido);

                MessageBox.Show("Operação efetuada com sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro! Error:" + ex.Message);
            }


            LimparCampos();
        }

        private void LimparCampos()
        {
            txtEstadoDestino.Text = string.Empty;
            txtEstadoOrigem.Text = string.Empty;
            textBoxNomeCliente.Text = string.Empty;

            dataGridViewPedidos.DataSource = null;
            dataGridViewPedidos.Refresh();
            SetupGrid();
        }
    }
}
