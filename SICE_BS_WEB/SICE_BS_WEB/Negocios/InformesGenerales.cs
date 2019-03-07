using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;

namespace SICE_BS_WEB.Negocios
{
    public class InformesGenerales
    {
        //Singleton singleton = Singleton.Global;
        ControlExcepciones excepcion = new ControlExcepciones();


        public DataTable ConsultarDatosGenerales(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("DatosGenerales");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarTransporteMercancia(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("TransporteMercancia");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarGuias(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("Guias");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarContenedores(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("Contenedores");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarFacturas(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("Facturas");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PEDIMENTOARMADO) ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarFechasPedimento(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("FechasPedimento");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarCasosPedimento(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("CasosPedimento");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarCuentasAduanerasGarantiaPedimento(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("CuentasAduanerasGarantiaPedimento");
            

            try
            {
               using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarTasaPedimento(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("TasaPedimento");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PEDIMENTOARMADO) ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarContribucionesDelPedimento(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ContribucionesDelPedimento");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PEDIMENTOARMADO) ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarObservacionesPedimento(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ObservacionesPedimento");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PEDIMENTOARMADO) ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarDescargosMercancia(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("DescargoMercancias");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PEDIMENTOARMADO) ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarDestinatarioMercancia(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("DestinatarioMercancia");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PEDIMENTOARMADO) ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarPartidas(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("Partidas");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PEDIMENTOARMADO) ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarPermisosPartidas(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("PermisoPartidas");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PEDIMENTOARMADO) ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarMercanciaDatosVehiculos(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("MercanciaDatosVehiculos");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PEDIMENTOARMADO) ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarCasosPartida(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("CasosPartida");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PEDIMENTOARMADO) ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarCuentasAduanerasGarantia(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("CuentasAduanerasGarantia");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarTasasContribucionesPartidas(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("TasasContribucionesPartidas");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarContribucionesPartidas(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ContribucionesPartidas");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarObservacionesPartidas(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ObservacionesPartidas");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarRectificaciones(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("Rectificaciones");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarDiferenciasContribucionesPedimento(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("DiferenciasContribucionesPedimento");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarImpuestos(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("Impuestos");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarContribucionesPorPedimento(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ContribucionesPorPedimento");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarDocumentales(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("Documentales");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarComercioExterior(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ComercioExterior");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarEstadoCuenta(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("EstadoCuenta");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarCadenaRectificaciones(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("CadenaRectificaciones");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarValorAgregadoMaquila(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ValorAgregadoMaquila");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarStatusPDFVsPedimento(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("StatusPDFVsPedimento");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public DataTable ConsultarTasasPorPartida(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("TasasPorPartida");
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PATENTES) ? Convert.DBNull : PATENTES;
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

        public string NombreSP(string modulo)
        {
            string sp = "";
            switch (modulo)
            {
                case "DatosGenerales":
                    sp = "SICEWEB_INFORME_DATOS_GENERALES";
                    break;
                case "TransporteMercancia":
                    sp = "SICEWEB_INFORME_TRANSPORTE_DE_LAS_MERCANCIAS";
                    break;
                case "Guias":
                    sp = "SICEWEB_INFORME_GUIAS";
                    break;
                case "Contenedores":
                    sp = "SICEWEB_INFORME_CONTENEDORES";
                    break;
                case "Facturas":
                    sp = "SICEWEB_INFORME_FACTURAS";
                    break; 
                case "FechasPedimento":
                    sp = "SICEWEB_INFORME_FECHAS";
                    break;
                case "CasosPedimento":
                    sp = "SICEWEB_INFORME_CASOS_PEDIMENTO";
                    break;
                case "TasaPedimento":
                    sp = "SICEWEB_INFORME_TASAS_DEL_PEDIMENTO";
                    break;
                case "ContribucionesDelPedimento":
                    sp = "SICEWEB_INFORME_CONTRIBUCIONES_DEL_PEDIMENTO";
                    break;
                case "ObservacionesPedimento":
                    sp = "SICEWEB_INFORME_OBSERVACIONES_DEL_PEDIMENTO";
                    break;
                case "DescargoMercancias":
                    sp = "SICEWEB_INFORME_DESCARGO_DE_MERCANCIAS";
                    break;
                case "DestinatarioMercancia":
                    sp = "SICEWEB_INFORME_DESTINATARIOS";
                    break;
                case "Partidas":
                    sp = "SICEWEB_INFORME_PARTIDAS";
                    break;
                case "PermisoPartidas":
                    sp = "SICEWEB_INFORME_PERMISOS_PARTIDA";
                    break;
                case "MercanciaDatosVehiculos":
                    sp = "SICEWEB_INFORME_MERCANCIAS_(DATOS_VEHICULOS)";
                    break;
                case "CuentasAduanerasGarantiaPedimento":
                    sp = "SICEWEB_INFORME_CUENTAS_ADUANERAS_DE_GARANTIA_DEL_PEDIMENTO";
                    break;
                case "CasosPartida":
                    sp = "SICEWEB_INFORME_CASOS_DE_LA_PARTIDA";
                    break;
                case "CuentasAduanerasGarantia":
                    sp = "SICEWEB_INFORME_CUENTAS_ADUANERAS_DE_GARANTIA";
                    break;
                case "TasasContribucionesPartidas":
                    sp = "SICEWEB_INFORME_TASAS_DE_LAS_CONTRIBUCIONES_DE_LAS_PARTIDAS";
                    break;
                case "ContribucionesPartidas":
                    sp = "SICEWEB_INFORME_CONTRIBUCIONES_DE_LA_PARTIDA";
                    break;
                case "ObservacionesPartidas":
                    sp = "SICEWEB_INFORME_OBSERVACIONES_DE_LA_PARTIDA";
                    break;
                case "Rectificaciones":
                    sp = "SICEWEB_INFORME_RECTIFICACIONES";
                    break;
                case "DiferenciasContribucionesPedimento":
                    sp = "SICEWEB_INFORME_DIFERENCIAS_EN_CONTRIBUCIONES_DEL_PEDIMENTO";
                    break;
                case "Impuestos":
                    sp = "SICEWEB_INFORME_IMPUESTOS";
                    break;
                case "ContribucionesPorPedimento":
                    sp = "SICEWEB_INFORME_CONTRIBUCIONES_POR_PEDIMENTO";
                    break;
                case "Documentales":
                    sp = "SICEWEB_INFORME_DOCUMENTALES";
                    break;
                case "ComercioExterior":
                    sp = "SICEWEB_INFORME_DE_COMERCIO_EXTERIOR";
                    break;
                case "EstadoCuenta":
                    sp = "SICEWEB_INFORME_ESTADO_DE_CUENTA";
                    break;
                case "CadenaRectificaciones":
                    sp = "SICEWEB_INFORME_CADENA_DE_RECTIFICACIONES";
                    break;
                case "ValorAgregadoMaquila":
                    sp = "SICEWEB_INFORME_VALOR_AGREGADO_MAQUILA";
                    break;
                case "StatusPDFVsPedimento":
                    sp = "SICEWEB_INFORME_STATUS_PDF_VS_PEDIMENTO";
                    break;
                case "TasasPorPartida":
                    sp = "SICEWEB_INFORME_TASAS_POR_PARTIDA";
                    break;                    
            }
            return sp;
        }

        //public DataTable ConsultarInformesGenerales(string MODULO, string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, ref string mensaje)
        //{
        //    mensaje = "";
        //    DataTable dt = new DataTable();
        //    string sp = NombreSP(MODULO);
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(Singleton.cadenaConexion))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(sp, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
        //                cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
        //                cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
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

    }
}