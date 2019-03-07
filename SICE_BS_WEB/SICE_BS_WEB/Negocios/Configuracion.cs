using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;

namespace SICE_BS_WEB.Negocios
{
    public class Configuracion
    {
        //Singleton singleton = Singleton.Global;
        ControlExcepciones excepcion = new ControlExcepciones();

        #region Ventanilla Única

        public DataTable ConsultarSettings(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_SETTINGS_CONSULTAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
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

        public string EditarSettings(string pasword, string clavellave, string rutakeyfile, string rutacerfile, byte[] keyfile, byte[] cerfile, int idusario, string cadena, ref string mensaje)
        {
            string valor = "";
            string sp = "SICEWEB_SETTINGS_EDITAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value = pasword;
                        cmd.Parameters.Add("@CLAVEABRIRLLAVE", SqlDbType.VarChar).Value = clavellave;
                        cmd.Parameters.Add("@KEYFILE", SqlDbType.VarChar).Value = rutakeyfile;
                        cmd.Parameters.Add("@CERFILE", SqlDbType.VarChar).Value = rutacerfile;
                        cmd.Parameters.Add("@FKEYFILE", SqlDbType.VarBinary).Value = keyfile == null ? Convert.DBNull : keyfile;
                        cmd.Parameters.Add("@FCERFILE", SqlDbType.VarBinary).Value = cerfile == null ? Convert.DBNull : cerfile;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idusario;
                        cmd.ExecuteNonQuery();
                        valor = "Ok";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);

                valor = ex.Message;
            }
            return valor;
        }

        #endregion



        #region Colores

        public DataTable Traer_Colores(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_COLORES_CONSULTAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
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


        public string Guardar_Colores(string menufondo, string menutxt, string fondoseleccionado, string btnfondo, string btntxt, string btnseleccionado,
                                      string cadena, ref string mensaje)
        {
            string valor = "";
            string sp = "SICEWEB_COLORES_AGREGAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@MenuFondo", SqlDbType.VarChar).Value = menufondo;
                        cmd.Parameters.Add("@MenuTxt", SqlDbType.VarChar).Value = menutxt;
                        cmd.Parameters.Add("@MenuFondoSelected", SqlDbType.VarChar).Value = fondoseleccionado;
                        cmd.Parameters.Add("@BtnFondo", SqlDbType.VarChar).Value = btnfondo;
                        cmd.Parameters.Add("@BtnTxt", SqlDbType.VarChar).Value = btntxt;
                        cmd.Parameters.Add("@BtnFondoSelected", SqlDbType.VarChar).Value = btnseleccionado;
                        cmd.ExecuteNonQuery();
                        valor = "Ok";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);

                valor = ex.Message;
            }
            return valor;
        }

        #endregion

    }
}