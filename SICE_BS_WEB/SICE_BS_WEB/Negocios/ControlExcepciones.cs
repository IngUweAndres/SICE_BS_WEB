using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace SICE_BS_WEB.Negocios
{
    public class ControlExcepciones
    {
        public const string Existe = "EXISTE";
        public const string IpExiste = "IPEXISTE";
        public const string Ok = "OK";

        public void RegistrarExcepcion(int idUsuario, string funcion, Exception exepcion, string cadena, ref string mensajeCritico)
        {
            string sp = "spLogMovimientoAgregar";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pIdUsuarioActualiza", SqlDbType.Int).Value = idUsuario;
                        cmd.Parameters.Add("@pFuncionalidad", SqlDbType.VarChar).Value = funcion;
                        cmd.Parameters.Add("@pIncidente", SqlDbType.VarChar).Value = exepcion.Message.Trim();
                        cmd.Parameters.Add("@pDetalleIncidente", SqlDbType.VarChar).Value = exepcion.StackTrace.Trim();
                        cmd.Parameters.Add("@pEsError", SqlDbType.Bit).Value = 1;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                mensajeCritico = SerializarExMessage(cadena, ex);
            }
        }

        public string SerializarExMessage(string cadena, Exception ex)
        {
            SqlConnection v_con = ConectarBD.getConexion(cadena);

            string mensaje = string.Empty;
            if (ex.Message.Contains("error: 26"))
                mensaje = "No se puede establecer la conexión al servidor SQL o la instancia es incorrecta.\\nContacte a un administrador. ";
            else if (ex.Message.Contains("error: 0"))
                mensaje = "Se agotó el tiempo de espera de respuesta del servidor. Intente nuevamente.";
            else if (ex.Message.Contains("login failed"))
                mensaje = "No se puede establecer la conexión a la base de datos o no tiene permisos para acceder a ella.\\nContacte a un administrador. ";
            else if (cadena == string.Empty)
                mensaje = "No se encuentra la cadena de conexión o está vacía.\\nContacte a un administrador. ";

            if (mensaje.Equals(string.Empty))
                mensaje = new JavaScriptSerializer().Serialize(ex.Message.ToString());

            return mensaje;
        }

        public string SerializarExStackTrace(Exception ex)
        {
            return new JavaScriptSerializer().Serialize(ex.StackTrace.ToString());
        }

        public void RegresarMensaje(DataTable dt, ref string mensaje)
        {
            if (dt != null)
                if (mensaje.Equals(string.Empty))
                    mensaje = dt.Rows[0][0].ToString();
        }
    }
}