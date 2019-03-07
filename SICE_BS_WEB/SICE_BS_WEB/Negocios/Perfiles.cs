using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;

namespace SICE_BS_WEB.Negocios
{
    public class Perfiles
    {
        ControlExcepciones excepcion = new ControlExcepciones();

        public DataTable PerfilConsultar(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spPerfilConsultar";
            
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

        public DataTable ModuloConsultar(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spModuloConsultar";
            
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

        public DataTable PerfilModuloAccionConsultar(int idPerfil, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spPerfilModuloAccionConsultar";
            
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pIdPerfil", SqlDbType.Int).Value = idPerfil;
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

        public DataTable ModuloAccionConsultar(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spModuloAccionConsultar";
            
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

        public void PermisoAgregar(int idUsuarioActualiza, int idUsuario, int idModulo, bool agregar, bool consultar, bool editar, bool eliminar, bool exportar, string cadena, ref string mensaje)
        {
            string sp = "spPermisoAgregar";
            
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
                        cmd.Parameters.Add("@pIdModulo", SqlDbType.Int).Value = idModulo;
                        cmd.Parameters.Add("@pAgregar", SqlDbType.Int).Value = agregar;
                        cmd.Parameters.Add("@pConsultar", SqlDbType.Int).Value = consultar;
                        cmd.Parameters.Add("@pEditar", SqlDbType.Int).Value = editar;
                        cmd.Parameters.Add("@pEliminar", SqlDbType.Int).Value = eliminar;
                        cmd.Parameters.Add("@pExportar", SqlDbType.Int).Value = exportar;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idUsuarioActualiza;
                        cmd.ExecuteNonQuery();
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
        }

        public void PerfilModuloAccionAgregar(int idUsuarioActualiza, int idPerfil, int idModulo, bool agregar, bool consultar, bool editar, bool eliminar, bool exportar, string cadena, ref string mensaje)
        {
            string sp = "spPerfilModuloAccionAgregar";
            
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pIdPerfil", SqlDbType.Int).Value = idPerfil;
                        cmd.Parameters.Add("@pIdModulo", SqlDbType.Int).Value = idModulo;
                        cmd.Parameters.Add("@pAgregar", SqlDbType.Int).Value = agregar;
                        cmd.Parameters.Add("@pConsultar", SqlDbType.Int).Value = consultar;
                        cmd.Parameters.Add("@pEditar", SqlDbType.Int).Value = editar;
                        cmd.Parameters.Add("@pEliminar", SqlDbType.Int).Value = eliminar;
                        cmd.Parameters.Add("@pExportar", SqlDbType.Int).Value = exportar;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idUsuarioActualiza;
                        cmd.ExecuteNonQuery();
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
        }

        public void PerfilAgregar(int idUsuarioActualiza, ref string idPerfil, string nombre, string clave, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spPerfilAgregar";
            
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pNombre", SqlDbType.VarChar).Value = nombre;
                        cmd.Parameters.Add("@pClave", SqlDbType.VarChar).Value = clave;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idUsuarioActualiza;
                        cmd.Parameters.Add("@pIdPerfil", SqlDbType.Int).Direction = ParameterDirection.Output;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                        idPerfil = cmd.Parameters["@pIdPerfil"].Value.ToString();
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

            excepcion.RegresarMensaje(dt, ref mensaje);
        }

        public void PerfilEditar(int idUsuarioActualiza, int idPerfil, string nombre, string clave, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spPerfilEditar";
            
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pIdPerfil", SqlDbType.Int).Value = idPerfil;
                        cmd.Parameters.Add("@pNombre", SqlDbType.VarChar).Value = nombre;
                        cmd.Parameters.Add("@pClave", SqlDbType.VarChar).Value = clave;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idUsuarioActualiza;
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

            excepcion.RegresarMensaje(dt, ref mensaje);
        }

        public void PerfilEliminar(int idUsuarioActualiza, int idPerfil, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spPerfilEliminar";
            
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pIdPerfil", SqlDbType.Int).Value = idPerfil;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idUsuarioActualiza;
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

            excepcion.RegresarMensaje(dt, ref mensaje);
        }
    }
}