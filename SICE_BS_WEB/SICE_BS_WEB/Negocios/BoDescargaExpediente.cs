using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;

namespace SICE_BS_WEB.Negocios
{
    public class BoDescargaExpediente
    {
        //Singleton singleton = Singleton.Global;
        ControlExcepciones excepcion = new ControlExcepciones();


        public DataTable ConsultaExpediente(DateTime? DESDE, DateTime? HASTA, string PEDIMENTO, string PATENTES,string ADUANA, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_INFORME_EXTRACCION_EXPEDIENTE";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO == null ? Convert.DBNull : PEDIMENTO;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = PATENTES == null ? Convert.DBNull : PATENTES;
                        cmd.Parameters.Add("@ADUANA", SqlDbType.VarChar).Value = ADUANA == null ? Convert.DBNull : ADUANA;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }


        public DataTable Consulta_DATA_STAGE(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_INFORME_DESCARGA_EXPEDIENTE_DS_SELECT";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public byte[] Trae_Archivo_DATA_STAGE(Guid key, string cadena, ref string mensaje)
        {
            byte[] binaryData = null;
            string valor = null;
            DataTable dt = new DataTable();
            string sp = "SICEWEB_INFORME_DESCARGA_EXPEDIENTE_DS_FILE";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FILESDSKEY", SqlDbType.UniqueIdentifier).Value = key;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }

                        binaryData = (byte[])dt.Rows[0]["ARCHIVO"];
                        //valor = Convert.ToString(cmd.ExecuteScalar());

                        //valor = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(valor));
                        //binaryData = System.Text.Encoding.UTF8.GetBytes(valor);
                        //valor = System.Convert.ToBase64String(binaryData);

                        ////ESTE ES EL IMPORTANTE (ver si la variable valor se mete sin hacer el utf8)
                        //binaryData = Convert.FromBase64String(valor);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }

            //return dt;
            return binaryData;
        }

    }
}