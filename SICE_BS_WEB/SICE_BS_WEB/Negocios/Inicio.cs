using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;

namespace SICE_BS_WEB.Negocios
{
    public class Inicio
    {
        ControlExcepciones excepcion = new ControlExcepciones();

        public DataTable DatosSesion(string usuario, string contrasena, int opcion, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spIniciarSesion";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pUsuario", SqlDbType.VarChar).Value = usuario;
                        cmd.Parameters.Add("@pPassword", SqlDbType.VarChar).Value = contrasena;
                        cmd.Parameters.Add("@pOpcion", SqlDbType.VarChar).Value = opcion;
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

        public DataTable DatosPermisos(int idUsuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spPermisoConsultar";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pIdUsuario", SqlDbType.Int).Value = idUsuario;
                        cmd.Parameters.Add("@pTipoModulo", SqlDbType.VarChar).Value = 'W';
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


        #region DatosPermisos anterior
        //public DataTable DatosPermisos(int idUsuario, string tipoModulo, ref string mensaje)
        //{
        //    DataTable dt = new DataTable();
        //    string sp = "spPermisoConsultar";

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Singleton.cadenaConexion))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sp, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@pIdUsuario", SqlDbType.VarChar).Value = idUsuario;
        //                cmd.Parameters.Add("@pTipoModulo", SqlDbType.VarChar).Value = tipoModulo;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (!reader.HasRows)
        //                        dt = null;
        //                    else
        //                        dt.Load(reader);
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        excepcion.RegistrarExcepcion(0, sp, ex, ref mensaje);
        //        if (mensaje.Length == 0)
        //            mensaje = "Error: " + excepcion.SerializarExMessage(ex);
        //    }
        //    return dt;
        //}
        #endregion

        #region Gráficas

        //public DataTable TraerTotalCoves(DateTime desde, DateTime hasta, ref string mensaje)
        //{
        //    DataTable dt = new DataTable();
        //    string sp = "SICEWEB_INTRO_TOTAL_COVES";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Singleton.cadenaConexion))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sp, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = desde;
        //                cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = hasta;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (!reader.HasRows)
        //                        dt = null;
        //                    else
        //                        dt.Load(reader);
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        excepcion.RegistrarExcepcion(0, sp, ex, ref mensaje);
        //        if (mensaje.Length == 0)
        //            mensaje = "Error: " + excepcion.SerializarExMessage(ex);
        //    }
        //    return dt;
        //}

        //public DataTable TraerTotalCovesDescargados(DateTime desde, DateTime hasta, ref string mensaje)
        //{
        //    DataTable dt = new DataTable();
        //    string sp = "SICEWEB_INTRO_COVES_DESCARGADOS";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Singleton.cadenaConexion))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sp, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = desde;
        //                cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = hasta;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (!reader.HasRows)
        //                        dt = null;
        //                    else
        //                        dt.Load(reader);
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        excepcion.RegistrarExcepcion(0, sp, ex, ref mensaje);
        //        if (mensaje.Length == 0)
        //            mensaje = "Error: " + excepcion.SerializarExMessage(ex);
        //    }
        //    return dt;
        //}

        //public DataTable TraerTotalEdocs(DateTime desde, DateTime hasta, ref string mensaje)
        //{
        //    DataTable dt = new DataTable();
        //    string sp = "SICEWEB_INTRO_TOTAL_EDOCS";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Singleton.cadenaConexion))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sp, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = desde;
        //                cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = hasta;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (!reader.HasRows)
        //                        dt = null;
        //                    else
        //                        dt.Load(reader);
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        excepcion.RegistrarExcepcion(0, sp, ex, ref mensaje);
        //        if (mensaje.Length == 0)
        //            mensaje = "Error: " + excepcion.SerializarExMessage(ex);
        //    }
        //    return dt;
        //}

        //public DataTable TraerTotalEdocsDescargados(DateTime desde, DateTime hasta, ref string mensaje)
        //{
        //    DataTable dt = new DataTable();
        //    string sp = "SICEWEB_INTRO_EDOCS_DESCARGADOS";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Singleton.cadenaConexion))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sp, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = desde;
        //                cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = hasta;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (!reader.HasRows)
        //                        dt = null;
        //                    else
        //                        dt.Load(reader);
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        excepcion.RegistrarExcepcion(0, sp, ex, ref mensaje);
        //        if (mensaje.Length == 0)
        //            mensaje = "Error: " + excepcion.SerializarExMessage(ex);
        //    }
        //    return dt;
        //}

        //public DataTable TraerCoveGrafica(DateTime desde, DateTime hasta, ref string mensaje)
        //{
        //    DataTable dt = new DataTable();
        //    string sp = "SICEWEB_INTRO_COVES_GRAFICA";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Singleton.cadenaConexion))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sp, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = desde;
        //                cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = hasta;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (!reader.HasRows)
        //                        dt = null;
        //                    else
        //                        dt.Load(reader);
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        excepcion.RegistrarExcepcion(0, sp, ex, ref mensaje);
        //        if (mensaje.Length == 0)
        //            mensaje = "Error: " + excepcion.SerializarExMessage(ex);
        //    }
        //    return dt;
        //}

        //public DataTable TraerEdGrafica(DateTime desde, DateTime hasta, ref string mensaje)
        //{
        //    DataTable dt = new DataTable();
        //    string sp = "SICEWEB_INTRO_EDOCS_GRAFICA";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Singleton.cadenaConexion))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sp, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = desde;
        //                cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = hasta;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (!reader.HasRows)
        //                        dt = null;
        //                    else
        //                        dt.Load(reader);
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        excepcion.RegistrarExcepcion(0, sp, ex, ref mensaje);
        //        if (mensaje.Length == 0)
        //            mensaje = "Error: " + excepcion.SerializarExMessage(ex);
        //    }
        //    return dt;
        //}

        //public DataTable TraerTipoOperacion(DateTime desde, DateTime hasta, ref string mensaje)
        //{
        //    DataTable dt = new DataTable();
        //    string sp = "SICEWEB_INTRO_OPERACION_PATENTE";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Singleton.cadenaConexion))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sp, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = desde;
        //                cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = hasta;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (!reader.HasRows)
        //                        dt = null;
        //                    else
        //                        dt.Load(reader);
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        excepcion.RegistrarExcepcion(0, sp, ex, ref mensaje);
        //        if (mensaje.Length == 0)
        //            mensaje = "Error: " + excepcion.SerializarExMessage(ex);
        //    }
        //    return dt;
        //}

        //public DataTable TraerValorOperacion(DateTime desde, DateTime hasta, ref string mensaje)
        //{
        //    DataTable dt = new DataTable();
        //    string sp = "SICEWEB_INTRO_VALOR_PATENTE";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Singleton.cadenaConexion))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sp, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = desde;
        //                cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = hasta;
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (!reader.HasRows)
        //                        dt = null;
        //                    else
        //                        dt.Load(reader);
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        excepcion.RegistrarExcepcion(0, sp, ex, ref mensaje);
        //        if (mensaje.Length == 0)
        //            mensaje = "Error: " + excepcion.SerializarExMessage(ex);
        //    }
        //    return dt;
        //}

        #endregion


    }
}