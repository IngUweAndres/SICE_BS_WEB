using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;
using System.ComponentModel;
using System.Configuration;

namespace SICE_BS_WEB.Negocios
{
    public class Catalogos
    {
        //Singleton singleton = Singleton.Global;
        ControlExcepciones excepcion = new ControlExcepciones();


        #region Catálogo RFC
        
        public DataTable TraerRFC(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_RFC";

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

        public DataTable GuardarRFC(string rfc, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_RFC_GUARDAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@RFC", SqlDbType.VarChar).Value = rfc;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EditarRFC(string rfc_anterior, string rfc, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_RFC_EDITAR";

            try
            {
                 using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@RFC_ATERIOR", SqlDbType.VarChar).Value = rfc_anterior;
                        cmd.Parameters.Add("@RFC", SqlDbType.VarChar).Value = rfc;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EliminarRFC(string rfc, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_RFC_ELIMINAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@RFC", SqlDbType.VarChar).Value = rfc;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        #region Catálogo Tipos de Archivo

        public DataTable TraerTiposArchivo(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPOS_DE_ARCHIVO";

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

        public DataTable GuardarTipoArchivo(string tipo, string sufijo, string opera, string oblig, string obligc, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPOS_DE_ARCHIVO_AGREGAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TIPOARCHIVO", SqlDbType.VarChar).Value = tipo;
                        cmd.Parameters.Add("@SUFIJO", SqlDbType.VarChar).Value = sufijo == "" ? Convert.DBNull : sufijo;
                        cmd.Parameters.Add("@TIPOOPERACION", SqlDbType.VarChar).Value = opera == "" ? Convert.DBNull : opera;
                        cmd.Parameters.Add("@OBLIGATORIO", SqlDbType.VarChar).Value = oblig == "" ? Convert.DBNull : oblig;
                        cmd.Parameters.Add("@OBLIGATORIOC", SqlDbType.VarChar).Value = oblig == "" ? Convert.DBNull : obligc;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EditarTipoArchivo(Int64 id, string tipo, string sufijo, string opera, string oblig, string obligc, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPOS_DE_ARCHIVO_EDITAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = id;
                        cmd.Parameters.Add("@TIPOARCHIVO", SqlDbType.VarChar).Value = tipo;
                        cmd.Parameters.Add("@SUFIJO", SqlDbType.VarChar).Value = sufijo == "" ? Convert.DBNull : sufijo;
                        cmd.Parameters.Add("@TIPOOPERACION", SqlDbType.VarChar).Value = opera == "" ? Convert.DBNull : opera;
                        cmd.Parameters.Add("@OBLIGATORIO", SqlDbType.VarChar).Value = oblig == "" ? Convert.DBNull : oblig;
                        cmd.Parameters.Add("@OBLIGATORIOC", SqlDbType.VarChar).Value = obligc == "" ? Convert.DBNull : obligc;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EliminarTipoArchivo(Int64 id, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPOS_DE_ARCHIVO_ELIMINAR";            

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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


        //Claves Pedimentos
        public DataTable TraeClavesPedimentos(Int64 TPKEY, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPCLAVES";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
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

        public DataTable GuardaClavePedimento(Int64 TPKEY, string CLAVEPEDIMENTO, string IDENTIFICADOR, string IDENTIFICADORP, string INCOTERM, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPCLAVES_AGREGAR";           
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@CLAVEPEDIMENTO", SqlDbType.VarChar).Value = CLAVEPEDIMENTO;
                        cmd.Parameters.Add("@IDENTIFICADOR", SqlDbType.VarChar).Value = IDENTIFICADOR;
                        cmd.Parameters.Add("@IDENTIFICADORP", SqlDbType.VarChar).Value = IDENTIFICADORP;
                        cmd.Parameters.Add("@INCOTERM", SqlDbType.VarChar).Value = INCOTERM;
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

        public DataTable ActualizaClavePedimento(Int64 TPKEY, string ANTERIOR_CLAVEPEDIMENTO, string CLAVEPEDIMENTO, string ANTERIOR_IDENTIFICADOR, string IDENTIFICADOR,
            string ANTERIOR_IDENTIFICADORP, string IDENTIFICADORP, string ANTERIOR_INCOTERM, string INCOTERM, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPCLAVES_EDITAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@ANTERIOR_CLAVEPEDIMENTO", SqlDbType.VarChar).Value = ANTERIOR_CLAVEPEDIMENTO;
                        cmd.Parameters.Add("@CLAVEPEDIMENTO", SqlDbType.VarChar).Value = CLAVEPEDIMENTO;
                        cmd.Parameters.Add("@ANTERIOR_IDENTIFICADOR", SqlDbType.VarChar).Value = ANTERIOR_IDENTIFICADOR;
                        cmd.Parameters.Add("@IDENTIFICADOR", SqlDbType.VarChar).Value = IDENTIFICADOR;
                        cmd.Parameters.Add("@ANTERIOR_IDENTIFICADORP", SqlDbType.VarChar).Value = ANTERIOR_IDENTIFICADORP;
                        cmd.Parameters.Add("@IDENTIFICADORP", SqlDbType.VarChar).Value = IDENTIFICADORP;
                        cmd.Parameters.Add("@ANTERIOR_INCOTERM", SqlDbType.VarChar).Value = ANTERIOR_INCOTERM;
                        cmd.Parameters.Add("@INCOTERM", SqlDbType.VarChar).Value = INCOTERM;
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

        public DataTable EliminaClavePedimento(Int64 TPKEY, string CLAVEPEDIMENTO, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPCLAVES_ELIMINAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@CLAVEPEDIMENTO", SqlDbType.VarChar).Value = CLAVEPEDIMENTO;
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


        //IG
        public DataTable GuardaIG(Int64 TPKEY, string IDENTIFICADOR, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPINDETIFICADORESG_AGREGAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@IDENTIFICADOR", SqlDbType.VarChar).Value = IDENTIFICADOR;
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

        public DataTable ActualizaIG(Int64 TPKEY, string ANTERIOR_IDENTIFICADOR, string IDENTIFICADOR, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPINDETIFICADORESG_EDITAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;                        
                        cmd.Parameters.Add("@ANTERIOR_IDENTIFICADOR", SqlDbType.VarChar).Value = ANTERIOR_IDENTIFICADOR;
                        cmd.Parameters.Add("@IDENTIFICADOR", SqlDbType.VarChar).Value = IDENTIFICADOR;                        
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

        public DataTable TraeIG(Int64 TPKEY, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPINDETIFICADORESG";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
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

        public DataTable EliminaIG(Int64 TPKEY, string IG, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPINDETIFICADORESG_ELIMINAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@IG", SqlDbType.VarChar).Value = IG;
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




        //IP
        public DataTable GuardaIP(Int64 TPKEY, string IDENTIFICADORP, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPIDENTIFICADORESP_AGREGAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@IDENTIFICADORP", SqlDbType.VarChar).Value = IDENTIFICADORP;
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

        public DataTable ActualizaIP(Int64 TPKEY, string ANTERIOR_IDENTIFICADORP, string IDENTIFICADORP, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPIDENTIFICADORESP_EDITAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@ANTERIOR_IDENTIFICADORP", SqlDbType.VarChar).Value = ANTERIOR_IDENTIFICADORP;
                        cmd.Parameters.Add("@IDENTIFICADORP", SqlDbType.VarChar).Value = IDENTIFICADORP;
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

        public DataTable TraeIP(Int64 TPKEY, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPIDENTIFICADORESP";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
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

        public DataTable EliminaIP(Int64 TPKEY, string IP, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPIDENTIFICADORESP_ELIMINAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@IP", SqlDbType.VarChar).Value = IP;
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




        //Incoterm
        public DataTable GuardaIN(Int64 TPKEY, string INCOTERM, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPINCOTERM_AGREGAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@INCOTERM", SqlDbType.VarChar).Value = INCOTERM;
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

        public DataTable ActualizaIN(Int64 TPKEY, string ANTERIOR_INCOTERM, string INCOTERM, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPINCOTERM_EDITAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@ANTERIOR_INCOTERM", SqlDbType.VarChar).Value = ANTERIOR_INCOTERM;
                        cmd.Parameters.Add("@INCOTERM", SqlDbType.VarChar).Value = INCOTERM;
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

        public DataTable TraeIncoterm(Int64 TPKEY, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPINCOTERM";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
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

        public DataTable EliminaIncoterm(Int64 TPKEY, string INCOTERM, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TPINCOTERM_ELIMINAR";
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TPKEY", SqlDbType.BigInt).Value = TPKEY;
                        cmd.Parameters.Add("@INCOTERM", SqlDbType.VarChar).Value = INCOTERM;
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

        #region Catálogo Autorizaciones

        public DataTable TraerUsuarios(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_AUTORIZACIONED_USUARIOS";

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

        public DataTable TraerAutorizaciones(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_AUTORIZACIONED";

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

        public DataTable GuardarAutorizacion(string autorizo, string autorizado, DateTime? fecha, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_AUTORIZACIONED_GUARDAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@USUARIOAUTORIZO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(autorizo) ? Convert.DBNull : autorizo;
                        cmd.Parameters.Add("@USUARIOAUTORIZADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(autorizado) ? Convert.DBNull : autorizado;
                        cmd.Parameters.Add("@FECHAAUTORIZACION", SqlDbType.DateTime).Value = fecha == null ? Convert.DBNull : fecha;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EditarAutorizacion(int id, string autorizo, string autorizado, DateTime? fecha, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_AUTORIZACIONED_EDITAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = id;
                        cmd.Parameters.Add("@USUARIOAUTORIZO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(autorizo) ? Convert.DBNull : autorizo;
                        cmd.Parameters.Add("@USUARIOAUTORIZADO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(autorizado) ? Convert.DBNull : autorizado;
                        cmd.Parameters.Add("@FECHAAUTORIZACION", SqlDbType.DateTime).Value = fecha == null ? Convert.DBNull : fecha;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EliminarAutorizacion(int id, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_AUTORIZACIONED_ELIMINAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        #region Catálogo Autorizaciones

        public DataTable TraerTiposActivoFijo(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPOS_AF";

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

        public DataTable GuardarTiposActivoFijo(string activo, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPOS_AF_AGREGAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ACTIVOFIJO", SqlDbType.VarChar).Value = activo;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EditarTiposActivoFijo(Int64 id, string activo, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPOS_AF_EDITAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@ACTIVOFIJO", SqlDbType.VarChar).Value = activo;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EliminarTiposActivoFijo(Int64 id, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPOS_AF_ELIMINAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        #region Catálogo Tipos Documentos

        public DataTable TraerTiposDocumentos(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPO_IMAGEN_CONSULTAR";

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

        public DataTable GuardarTiposDocumentos(string tipo, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPO_IMAGEN_AGREGAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TIPOIMAGEN", SqlDbType.VarChar).Value = tipo;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EditarTiposDocumentos(Int64 id, string tipo, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPO_IMAGEN_EDITAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TIPOIMAGEKEY", SqlDbType.BigInt).Value = id;
                        cmd.Parameters.Add("@TIPOIMAGEN", SqlDbType.VarChar).Value = tipo;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EliminarTiposDocumentos(Int64 id, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATALOGO_TIPO_IMAGEN_ELIMINAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@TIPOIMAGEKEY", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        #region Catálogo Control Activos Fijo

        public DataTable TraerActivosFijos(string pedimento, string folio, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTO", SqlDbType.VarChar).Value = pedimento;
                        cmd.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
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

        public DataTable GuardarActivoFijo(string folio, string marca, string modelo, string serie, string fraccion, string desc,
                                           string tipoactivo, decimal aduana, decimal comercial, int usuario, string cadena, string cvepais, 
                                           string desctec, string po, decimal valorme, string me, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_GUARDAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
                        cmd.Parameters.Add("@MARCA", SqlDbType.VarChar).Value = string.IsNullOrEmpty(marca) ? Convert.DBNull : marca;
                        cmd.Parameters.Add("@MODELO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(modelo) ? Convert.DBNull : modelo;
                        cmd.Parameters.Add("@SERIE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(serie) ? Convert.DBNull : serie;
                        cmd.Parameters.Add("@FRACCION", SqlDbType.VarChar).Value = string.IsNullOrEmpty(fraccion) ? Convert.DBNull : fraccion;
                        cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = string.IsNullOrEmpty(desc) ? Convert.DBNull : desc;
                        cmd.Parameters.Add("@TIPOACTIVO", SqlDbType.VarChar).Value = tipoactivo;
                        cmd.Parameters.Add("@VALORADUANA", SqlDbType.Decimal).Value = aduana.Equals(0) ? Convert.DBNull : aduana;
                        cmd.Parameters.Add("@VALORCOMERCIAL", SqlDbType.Decimal).Value = comercial.Equals(0) ? Convert.DBNull : comercial;
                        cmd.Parameters.Add("@CLAVEPAISORIGEN", SqlDbType.VarChar).Value = string.IsNullOrEmpty(cvepais) ? Convert.DBNull : cvepais;
                        cmd.Parameters.Add("@DESCRIPCIONTECNICA", SqlDbType.VarChar).Value = string.IsNullOrEmpty(desctec) ? Convert.DBNull : desctec;
                        cmd.Parameters.Add("@PO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(po) ? Convert.DBNull : po;
                        cmd.Parameters.Add("@VALORME", SqlDbType.Decimal).Value = valorme.Equals(0) ? Convert.DBNull : valorme;
                        cmd.Parameters.Add("@ME", SqlDbType.VarChar).Value = string.IsNullOrEmpty(me) ? Convert.DBNull : me;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EditarActivoFijo(Int64 id, string folio, string marca, string modelo, string serie, string fraccion, string desc,
                                          string tipoactivo, decimal aduana, decimal comercial, int usuario, string cadena, string cvepais, 
                                          string desctec, string po, decimal valorme, string me, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_EDITAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ACTIVOFIJOKEY", SqlDbType.BigInt).Value = id;
                        cmd.Parameters.Add("@FOLIO", SqlDbType.VarChar).Value = folio;
                        cmd.Parameters.Add("@MARCA", SqlDbType.VarChar).Value = string.IsNullOrEmpty(marca) ? Convert.DBNull : marca;
                        cmd.Parameters.Add("@MODELO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(modelo) ? Convert.DBNull : modelo;
                        cmd.Parameters.Add("@SERIE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(serie) ? Convert.DBNull : serie;
                        cmd.Parameters.Add("@FRACCION", SqlDbType.VarChar).Value = string.IsNullOrEmpty(fraccion) ? Convert.DBNull : fraccion;
                        cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = string.IsNullOrEmpty(desc) ? Convert.DBNull : desc;
                        cmd.Parameters.Add("@TIPOACTIVO", SqlDbType.VarChar).Value = tipoactivo;
                        cmd.Parameters.Add("@VALORADUANA", SqlDbType.Decimal).Value = aduana.Equals(0) ? Convert.DBNull : aduana;
                        cmd.Parameters.Add("@VALORCOMERCIAL", SqlDbType.Decimal).Value = comercial.Equals(0) ? Convert.DBNull : comercial;
                        cmd.Parameters.Add("@CLAVEPAISORIGEN", SqlDbType.VarChar).Value = string.IsNullOrEmpty(cvepais) ? Convert.DBNull : cvepais;
                        cmd.Parameters.Add("@DESCRIPCIONTECNICA", SqlDbType.VarChar).Value = string.IsNullOrEmpty(desctec) ? Convert.DBNull : desctec;
                        cmd.Parameters.Add("@PO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(po) ? Convert.DBNull : po;
                        cmd.Parameters.Add("@VALORME", SqlDbType.Decimal).Value = valorme.Equals(0) ? Convert.DBNull : valorme;
                        cmd.Parameters.Add("@ME", SqlDbType.VarChar).Value = string.IsNullOrEmpty(me) ? Convert.DBNull : me;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable EliminarActivoFijo(Int64 id, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_ELIMINAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public DataTable GuardarDocumentosPorActivo(Int64 activokey, byte[] file, string tipoimagen, string nombrefile, int idusario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_IMAGEN_AGREGAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ACTIVOFIJOKEY", SqlDbType.Int).Value = activokey;
                        cmd.Parameters.Add("@FILE", SqlDbType.VarBinary).Value = file;
                        cmd.Parameters.Add("@TIPOIMAGEN", SqlDbType.VarChar).Value = tipoimagen;
                        cmd.Parameters.Add("@NOMBREARCHIVO", SqlDbType.VarChar).Value = nombrefile;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idusario;
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

        public DataTable TraerDocumentosPorActivo(Int64 activo, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_IMAGEN_CONSULTAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ACTIVOFIJOKEY", SqlDbType.Int).Value = activo;
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

        public DataTable EliminarDocumentosPorActivo(Int64 activo, Guid imagenkey, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_IMAGEN_ELIMINAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ACTIVOFIJOKEY", SqlDbType.Int).Value = activo;
                        cmd.Parameters.Add("@IMAGENESKEY", SqlDbType.UniqueIdentifier).Value = imagenkey;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        public string TraerArchivoPorNombre(string nombre, string cadena, ref string mensaje)
        {
            byte[] binaryData = null;
            string valor = string.Empty;

            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_IMAGEN_TRAER";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@NOMBREARCHIVO", SqlDbType.VarChar).Value = nombre;
                        valor = Convert.ToString(cmd.ExecuteScalar());
                        binaryData = System.Text.Encoding.ASCII.GetBytes(valor);  
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

        public DataTable TraerPedimentosTodos(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_PEDIMENTOS_TODOS";

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

        public DataTable TraerPedimentosPorActivo(Int64 activo, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_PEDIMENTOS_CONSULTAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ACTIVOFIJOKEY", SqlDbType.Int).Value = activo;
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

        public DataTable GuardarPedimento(Int64 activokey, string pedimento, Int64 partida, int idusario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_PEDIMENTOS_AGREGAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ACTIVOFIJOKEY", SqlDbType.Int).Value = activokey;
                        cmd.Parameters.Add("@PEDIMENTO", SqlDbType.VarChar).Value = pedimento;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = partida == 0 ? Convert.DBNull : partida;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idusario;
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

        public DataTable EditarPedimento(Int64 key, Int64 activokey, Int64 partida, int idusario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_PEDIMENTOS_EDITAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@RELACIONPEDIMENTOKEY", SqlDbType.Int).Value = key;
                        cmd.Parameters.Add("@ACTIVOFIJOKEY", SqlDbType.Int).Value = activokey;
                        cmd.Parameters.Add("@PARTIDA", SqlDbType.Int).Value = partida == 0 ? Convert.DBNull : partida;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idusario;
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

        public DataTable EliminarPedimento(Int64 pedimentokey, Int64 activo, int usuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CONTROL_ACTIVO_FIJO_PEDIMENTOS_ELIMINAR";
            SqlConnection v_con = ConectarBD.getConexion(cadena);

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@RELACIONPEDIMENTOKEY", SqlDbType.Int).Value = pedimentokey;
                        cmd.Parameters.Add("@ACTIVOFIJOKEY", SqlDbType.Int).Value = activo;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = usuario;
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

        #region Catálogo Aduanas

        public DataTable TraerAduanas(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_ADUANAS";

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

        #endregion

        #region Catálogo Claves

        public DataTable TraerClaves(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CLAVES";

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

        #endregion

        #region Catálogo Representante Legal

        public DataTable Traer_Representantes_Legales(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_REPRESENTANTELEGAL_CONSULTAR";

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

        public DataTable Guardar_Representante_Legal(string nombre, string paterno, string materno, string rfc, string correo, string tel,  
                                                     DateTime? apartir, DateTime? hasta, Object img, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_REPRESENTANTELEGAL_AGREGAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = nombre;
                        cmd.Parameters.Add("@APELLIDOPATERNO", SqlDbType.VarChar).Value = paterno;
                        cmd.Parameters.Add("@APELLIDOMATERNO", SqlDbType.VarChar).Value = materno;
                        cmd.Parameters.Add("@RFC", SqlDbType.VarChar).Value = rfc;
                        cmd.Parameters.Add("@CORREOELECTRONICO", SqlDbType.VarChar).Value = correo;
                        cmd.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = tel;
                        cmd.Parameters.Add("@APARTIRDE", SqlDbType.DateTime).Value = apartir == null ? Convert.DBNull : apartir;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = hasta == null ? Convert.DBNull : hasta;
                        cmd.Parameters.Add("@FIRMAIMAGE", SqlDbType.Image).Value = img;
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

        public DataTable Editar_Representante_Legal(int key, string nombre, string paterno, string materno, string rfc, string correo, string tel,
                                                     DateTime? apartir, DateTime? hasta, Object img, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_REPRESENTANTELEGAL_EDITAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@REPRESENTANTEKEY", SqlDbType.Int).Value = key;
                        cmd.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = nombre;
                        cmd.Parameters.Add("@APELLIDOPATERNO", SqlDbType.VarChar).Value = paterno;
                        cmd.Parameters.Add("@APELLIDOMATERNO", SqlDbType.VarChar).Value = materno;
                        cmd.Parameters.Add("@RFC", SqlDbType.VarChar).Value = rfc;
                        cmd.Parameters.Add("@CORREOELECTRONICO", SqlDbType.VarChar).Value = correo;
                        cmd.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = tel;
                        cmd.Parameters.Add("@APARTIRDE", SqlDbType.DateTime).Value = apartir == null ? Convert.DBNull : apartir;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = hasta == null ? Convert.DBNull : hasta;
                        cmd.Parameters.Add("@FIRMAIMAGE", SqlDbType.Image).Value = img;
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

        public DataTable Eliminar_Representante_Legal(int id, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_REPRESENTANTELEGAL_ELIMINAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@REPRESENTANTEKEY", SqlDbType.Int).Value = id;
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