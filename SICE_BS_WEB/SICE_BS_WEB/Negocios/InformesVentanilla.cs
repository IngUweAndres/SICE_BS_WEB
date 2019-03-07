using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;

namespace SICE_BS_WEB.Negocios
{
    public class InformesVentanilla
    {
        //Singleton singleton = Singleton.Global;
        ControlExcepciones excepcion = new ControlExcepciones();


        public DataTable ConsultarCovePedimento(DateTime? DESDE, DateTime? HASTA, string PEDIMENTO, string COVE, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = "SICEWEB_INFORME_VENTANILLA_COVE_PEDIMENTO";

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
                        cmd.Parameters.Add("@COVE", SqlDbType.VarChar).Value = COVE == null ? Convert.DBNull : COVE;
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


        public DataTable ConsultarEdocument(DateTime? DESDE, DateTime? HASTA, string PEDIMENTO, string ED, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = "SICEWEB_INFORME_VENTANILLA_EDOCUMENTS";

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
                        cmd.Parameters.Add("@EDOC", SqlDbType.VarChar).Value = ED == null ? Convert.DBNull : ED;
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

        public DataTable ConsultarActivoFijo(DateTime? DESDE, DateTime? HASTA, string PEDIMENTO, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = "SICEWEB_INFORME_VENTANILLA_AF";

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
                
    }
}