using Imposto.Core.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Imposto.Core.Data
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        public bool GerarArquivoXML(NotaFiscal notaFiscal)
        {
            try
            {
                var caminho = ConfigurationManager.AppSettings.Get("CaminhoArquivoXMLNotaFiscal");

                System.Xml.Serialization.XmlSerializer writer =
                    new System.Xml.Serialization.XmlSerializer(typeof(NotaFiscal),
                    extraTypes: new Type[] { typeof(NotaFiscalItem) });

                var path = caminho + "//NotaFiscal_" + notaFiscal.Serie + ".xml";
                using (FileStream file = System.IO.File.Create(path))
                {
                    writer.Serialize(file, notaFiscal);
                }

                return true;
            }
            catch (Exception)
            {

            }

            return false;
        }

        public int Salvar(NotaFiscal notaFiscal)
        {
            int notaFiscalId = 0;

            // Create the connection.
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
            {
                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand sqlCommand = new SqlCommand("P_NOTA_FISCAL", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    MapperNotaFiscal(notaFiscal, sqlCommand);

                    try
                    {
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                       
                        int.TryParse(sqlCommand.Parameters["@pId"].Value.ToString(), out notaFiscalId);
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

            return notaFiscalId;
        }

        private static void MapperNotaFiscal(NotaFiscal notaFiscal, SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.Add(new SqlParameter("@pNumeroNotaFiscal", notaFiscal.NumeroNotaFiscal));
            sqlCommand.Parameters.Add(new SqlParameter("@pSerie ", notaFiscal.Serie));
            sqlCommand.Parameters.Add(new SqlParameter("@pNomeCliente", notaFiscal.NomeCliente));
            sqlCommand.Parameters.Add(new SqlParameter("@pEstadoDestino", notaFiscal.EstadoDestino));
            sqlCommand.Parameters.Add(new SqlParameter("@pEstadoOrigem", notaFiscal.EstadoOrigem));

            sqlCommand.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int));
            sqlCommand.Parameters["@pId"].Direction = ParameterDirection.InputOutput;
            sqlCommand.Parameters["@pId"].Value = 0;
        }
    }
}
