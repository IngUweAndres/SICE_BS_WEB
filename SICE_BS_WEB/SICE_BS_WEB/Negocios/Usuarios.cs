using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;

namespace SICE_BS_WEB.Negocios
{
    public class Usuarios
    {
        ControlExcepciones excepcion = new ControlExcepciones();

        public DataTable UsuarioConsultar(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spUsuarioConsultar";            

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

        public DataTable TipoUsuarioConsultar(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spTipoUsuarioConsultar";
            
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

        public void UsuarioAgregar(int idUsuarioActualiza, ref string idUsuario, string usuario, string nombre, string contrasena, int idPerfil, int idTipo, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spUsuarioAgregar";
            
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
                        cmd.Parameters.Add("@pNombre", SqlDbType.VarChar).Value = nombre;
                        cmd.Parameters.Add("@pContrasena", SqlDbType.VarChar).Value = contrasena;
                        cmd.Parameters.Add("@pIdPerfil", SqlDbType.Int).Value = idPerfil;
                        cmd.Parameters.Add("@pIdTipoUsuario", SqlDbType.Int).Value = idTipo;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idUsuarioActualiza;
                        cmd.Parameters.Add("@pIdUsuario", SqlDbType.Int).Direction = ParameterDirection.Output;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                        idUsuario = cmd.Parameters["@pIdUsuario"].Value.ToString();
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

        public void UsuarioEditar(int idUsuarioActualiza, int idUsuario, string usuario, string nombre, string contrasena, int idPerfil, int idTipo, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spUsuarioEditar";
            
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
                        cmd.Parameters.Add("@pUsuario", SqlDbType.VarChar).Value = usuario;
                        cmd.Parameters.Add("@pNombre", SqlDbType.VarChar).Value = nombre;
                        cmd.Parameters.Add("@pContrasena", SqlDbType.VarChar).Value = contrasena.Equals(string.Empty) ? Convert.DBNull : contrasena;
                        cmd.Parameters.Add("@pIdPerfil", SqlDbType.Int).Value = idPerfil;
                        cmd.Parameters.Add("@pIdTipoUsuario", SqlDbType.Int).Value = idTipo;
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

        public void UsuarioEliminar(int idUsuarioActualiza, int idUsuario, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "spUsuarioEliminar";
            
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