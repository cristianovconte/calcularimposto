using Imposto.Core.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Data
{
    public class NotaFiscalItemRepository : INotaFiscalItemRepository
    {
        public bool Salvar(NotaFiscalItem notaFiscalItem)
        {

            // Create the connection.
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
            {
                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand sqlCommand = new SqlCommand("P_NOTA_FISCAL_ITEM", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    MapperNotaFiscalItem(notaFiscalItem, sqlCommand);

                    try
                    {
                        connection.Open();
                        if (sqlCommand.ExecuteNonQuery() > 0)
                            return true;
                    }

                    catch (Exception)
                    {
                      
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return false;
        }

        private static void MapperNotaFiscalItem(NotaFiscalItem notaFiscalItem, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.Add(new SqlParameter("@pId", notaFiscalItem.Id));
            sqlCommand.Parameters.Add(new SqlParameter("@pIdNotaFiscal", notaFiscalItem.IdNotaFiscal));
            sqlCommand.Parameters.Add(new SqlParameter("@pCfop", string.IsNullOrEmpty(notaFiscalItem.Cfop) ? "0" : notaFiscalItem.Cfop));
            sqlCommand.Parameters.Add(new SqlParameter("@pTipoIcms", notaFiscalItem.TipoIcms));
            sqlCommand.Parameters.Add(new SqlParameter("@pBaseIcms", notaFiscalItem.BaseIcms));
            sqlCommand.Parameters.Add(new SqlParameter("@pAliquotaIcms", notaFiscalItem.AliquotaIcms));
            sqlCommand.Parameters.Add(new SqlParameter("@pValorIcms", notaFiscalItem.ValorIcms));
            sqlCommand.Parameters.Add(new SqlParameter("@pNomeProduto", notaFiscalItem.NomeProduto));
            sqlCommand.Parameters.Add(new SqlParameter("@pCodigoProduto", notaFiscalItem.CodigoProduto));
            sqlCommand.Parameters.Add(new SqlParameter("@pBaseIpi", notaFiscalItem.BaseCalculoIPI));
            sqlCommand.Parameters.Add(new SqlParameter("@pAliquotaIpi", notaFiscalItem.AliquotaIPI));
            sqlCommand.Parameters.Add(new SqlParameter("@pValorIpi", notaFiscalItem.ValorIPI));
            sqlCommand.Parameters.Add(new SqlParameter("@pDesconto", notaFiscalItem.Desconto));
        }
    }
}
