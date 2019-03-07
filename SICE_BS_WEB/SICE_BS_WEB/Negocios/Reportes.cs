using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;

namespace SICE_BS_WEB.Negocios
{
    public class Reportes
    {
        ControlExcepciones excepcion = new ControlExcepciones();

        public DataTable MovimientoConsultar(DateTime? fechaIni, DateTime? fechaFin, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spLogMovimientoConsultar";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pFechaIni", SqlDbType.Date).Value = fechaIni == null ? Convert.DBNull : fechaIni;
                        cmd.Parameters.Add("@pFechaFin", SqlDbType.Date).Value = fechaFin == null ? Convert.DBNull : fechaFin;
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

        public DataTable EntradaConsultar(string articulo, DateTime? fechaIni, DateTime? fechaFin, string proveedor, string usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spEntradaConsultar";

            try
            {
                 using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pArticulo", SqlDbType.VarChar).Value = articulo.Equals(string.Empty) ? Convert.DBNull : articulo;
                        cmd.Parameters.Add("@pFechaIni", SqlDbType.Date).Value = fechaIni == null ? Convert.DBNull : fechaIni;
                        cmd.Parameters.Add("@pFechaFin", SqlDbType.Date).Value = fechaFin == null ? Convert.DBNull : fechaFin;
                        cmd.Parameters.Add("@pProveedor", SqlDbType.VarChar).Value = proveedor.Equals(string.Empty) ? Convert.DBNull : proveedor;
                        cmd.Parameters.Add("@pUsuario", SqlDbType.VarChar).Value = usuario.Equals(string.Empty) ? Convert.DBNull : usuario;
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



        /// <summary>
        /// REPORTE HOJA DE CÁLCULO  ANEXO      
        /// </summary>                
        public DataTable Traer_Anexo(Guid hckey, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = string.Empty;

            try
            {
                sp = "HOJACALCULO_HCKEY_SELECT";
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Original_hckey", SqlDbType.UniqueIdentifier).Value = hckey;                        
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

                throw ex;
            }
            return dt;
        }


        /// <summary>
        /// REPORTE HOJA DE CÁLCULO DETALLE DEL ANEXO   
        /// </summary>                
        public DataTable Traer_Detalle_Anexo(Guid hckey, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = string.Empty;

            try
            {
                sp = "HOJACALCULO_HCKEY_DETALLE_SELECT";
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = hckey;
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

                throw ex;
            }
            return dt;
        }



    }
}