using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;
using System.Text;

namespace SICE_BS_WEB.Negocios
{
    public class BoDocumentos
    {
        ControlExcepciones excepcion = new ControlExcepciones();

        public DataTable ConsultarFechasOperacion(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_ENCABEZADO_FECHAS";

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

        public DataTable ConsultarDocumentos(DateTime? DESDE, DateTime? HASTA, string PEDIMENTO, string CLAVE_PEDIMENTO,
                                             string ADUANA, string TEXTO, string COMODIN, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_ENCABEZADO";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@CLAVE_PEDIMENTO", SqlDbType.VarChar).Value = CLAVE_PEDIMENTO;
                        cmd.Parameters.Add("@ADUANA", SqlDbType.VarChar).Value = ADUANA;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@TEXTO", SqlDbType.VarChar).Value = TEXTO;
                        cmd.Parameters.Add("@COMODIN", SqlDbType.VarChar).Value = COMODIN;
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

        public DataTable ConsultarPartidas(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_PARTIDAS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarDatosGenerales(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_DATOS_GENERALES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarIE(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_IE";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarCV(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CV";
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarXML(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_ENCABEZADO_XML";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarIG(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_IDENTIFICADORESG";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarFletes(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_FLETES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarIncrementables(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_INCREMENTABLES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarFacturas(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_FACTURAS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarFechas(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_FECHAS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarContribucionesGenerales(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTRIBUCIONES_GENERALES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarImpuestos(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_IMPUESTOS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarTransporte(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_TRANSPORTE";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarGuias(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_GUIAS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarContenedores(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTENEDORES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarObservaciones(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_OBSERVACIONES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarDescargos(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_DESCARGOS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarRectificaciones(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_RECTIFICACIONES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable ConsultarCuentaDeGastos(string PEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CUENTAS_DE_GASTOS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
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

        public DataTable Consultar_CG_SiguienteUsuario(int idusario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_SIGUIENTEUSUARIO";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IDUSUARIO", SqlDbType.Int).Value = idusario;
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

        public DataTable Consultar_CG_GastosExtraordinarios(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_GASTOSEXTRAORDINARIOS";

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

        public DataTable CG_RevisadoAutorizadoRechazado(int accion, Int64 cgkey, int idusuario, int idusuariosiguiente, string cg, string concepto, string cadena, ref string mensaje, out string respuesta)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CUENTAGASTOS_ACCIONES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ACCION", SqlDbType.Int).Value = accion;
                        cmd.Parameters.Add("@CGKEY", SqlDbType.BigInt).Value = cgkey;
                        cmd.Parameters.Add("@USUARIO", SqlDbType.Int).Value = idusuario;
                        cmd.Parameters.Add("@SIGUIENTE", SqlDbType.Int).Value = idusuariosiguiente;
                        cmd.Parameters.Add("@CG", SqlDbType.VarChar).Value = cg;
                        cmd.Parameters.Add("@CONCEPTO", SqlDbType.VarChar).Value = concepto;
                        cmd.Parameters.Add("@RESPUESTA", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                        respuesta = cmd.Parameters["@RESPUESTA"].Value.ToString();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
                respuesta = ex.Message;
            }
            return dt;
        }

        public DataTable ConsultarExpedienteDigital(string PEDIMENTO, int USER, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_DOCUMENTOS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@USER", SqlDbType.VarChar).Value = USER;
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

        public DataTable BorrarFileExpedienteDigital(Guid IDARCHIVO, int USERID, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_ED_BORRAR_DIGITALIZADOS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IDARCHIVO", SqlDbType.UniqueIdentifier).Value = IDARCHIVO;
                        cmd.Parameters.Add("@USERID", SqlDbType.VarChar).Value = USERID.ToString();
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

        public string GuardarArchivo_ExpedienteDigital(byte[] file, string pedimento, string tipo, string nombre, int idusario, string cadena, ref string mensaje) 
        {
            string valor = "";
            string sp = "SICEWEB_ED_GUARDAR_DIGITALIZADOS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FILE", SqlDbType.VarBinary).Value = file;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = pedimento;
                        cmd.Parameters.Add("@TIPOARCHIVO", SqlDbType.VarChar).Value = tipo;
                        cmd.Parameters.Add("@NOMBREARCHIVO", SqlDbType.VarChar).Value = nombre;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idusario;
                        cmd.Parameters.Add("@USERID", SqlDbType.Int).Value = idusario;
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

        public string ConsultarDocumentosString(string TIPO, string FOLIO, string ID, string cadena, ref string mensaje)
        {
            string valor = null;
            string sp = "get_FILE";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.AddWithValue("@TIPO", TIPO);
                        cmd.Parameters.AddWithValue("@FOLIO", FOLIO == null ? Convert.DBNull : FOLIO);
                        cmd.Parameters.AddWithValue("@ID", ID == null ? Convert.DBNull : ID);
                        valor = Convert.ToString(cmd.ExecuteScalar());  
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
            return valor;
        }

        public byte[] ConsultarDocumentosBynary(string TIPO, string FOLIO, string ID, string cadena, ref string mensaje)
        {
            byte[] binaryData = null;
            string valor = null;
            string sp = "get_FILE";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.AddWithValue("@TIPO", TIPO);
                        cmd.Parameters.AddWithValue("@FOLIO", FOLIO == null ? Convert.DBNull : FOLIO);
                        cmd.Parameters.AddWithValue("@ID", ID == null ? Convert.DBNull : ID);
                        valor = Convert.ToString(cmd.ExecuteScalar());

                        binaryData = System.Text.Encoding.UTF8.GetBytes(valor);
                        valor = System.Convert.ToBase64String(binaryData);

                        //ESTE ES EL IMPORTANTE (ver si la variable valor se mete sin hacer el utf8)
                        binaryData = Convert.FromBase64String(valor);

                        //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                        //binaryData = encoding.GetBytes(valor);

                        //binaryData = Encoding.UTF8.GetBytes(valor);

                        //String to VarBinary
                        //byte[] theBytes = Encoding.UTF8.GetBytes(theString);

                        //VarBinary to String
                        //string theString = Encoding.UTF8.GetString(theBytes);  
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
            return binaryData;
        }

        public byte[] Obtener_Archivo_Digi(Guid Filekey, string tipo, string cadena, ref string mensaje)
        {
            byte[] binaryData = null;
            DataTable dt = new DataTable();
            string sp = "SICEWEB_ED_TRAER_ARCHIVOS_ED_DIGI";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Filekey", SqlDbType.UniqueIdentifier).Value = Filekey;
                        cmd.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = tipo;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }

                        binaryData = (byte[])dt.Rows[0]["ARCHIVO"];
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
            return binaryData;
        }

        #region  OPCIONES DE LA PARTIDA

        public DataTable ConsultarPartida_Detalle(string PEDIMENTO, long PARTIDA, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_PARTIDAS_DETALLE";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = PARTIDA;
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

        public DataTable ConsultarPartida_Observaciones(string PEDIMENTO, long PARTIDA, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_PARTIDAS_OBSERVACIONES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = PARTIDA;
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

        public DataTable ConsultarPartida_Gravamen(string PEDIMENTO, long PARTIDA, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_PARTIDAS_IMPUESTOS_GRAVAMEN";
            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = PARTIDA;
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

        public DataTable ConsultarPartida_Tasas(string PEDIMENTO, long PARTIDA, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_PARTIDAS_IMPUESTOS_TASAS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = PARTIDA;
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

        public DataTable ConsultarPartida_Identificadores(string PEDIMENTO, long PARTIDA, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_PARTIDAS_IDENTIFICADORES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = PARTIDA;
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

        public DataTable ConsultarPartida_Regulaciones(string PEDIMENTO, long PARTIDA, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_PARTIDAS_REGULACIONES";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = PARTIDA;
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

        public DataTable ConsultarPartida_XML(string PEDIMENTO, long PARTIDA, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_PARTIDAS_XML";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = PARTIDA;
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

        public DataTable ConsultarPartida_Codigos(string PEDIMENTO, long PARTIDA, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_PARTIDAS_CODIGOS";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = PARTIDA;
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

        #endregion

        #region  DETALLE DE LA PARTIDA

        public DataTable ConsultarPartida_IdentificadoresPartida(string PEDIMENTO, int PARTIDA, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_PARTIDAS_IDENTIFICADORES_PARTIDA";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTO;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = PARTIDA;
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

        #endregion
    }
}