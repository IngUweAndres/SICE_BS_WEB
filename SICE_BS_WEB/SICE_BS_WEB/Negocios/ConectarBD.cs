using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;
using System.IO;
using System.Reflection;

namespace SICE_BS_WEB.Negocios
{
    public class ConectarBD
    {
        private static string StringConnection;
        private static SqlConnection Connection;
        private static FileInfo v_MyFile;
        private static StreamReader v_Lector;
        private static string total_bases;


        //Traer conexión
        public static SqlConnection getConexion(string cadena)
        {
            if (Connection == null)
                Connection = new SqlConnection(cadena);
            return Connection;
        }


        //Cerrar la conexión
        public static void CerrarConexion(ref SqlConnection sqlCon)
        {
            if (sqlCon != null)
                sqlCon.Close();
        }


        /// <summary>
        /// Lee archivo de configuración para obtener total de conexiones a BD 
        /// </summary>
        /// <param name="rutaArchivo">Ruta del archivo a leer</param>
        /// <returns>Regresa el valor de conexiones o la cadena "Error" en caso de haber un problema</returns>
        public static Dictionary<string, string> TotalConexiones()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            ControlExcepciones excepcion = new ControlExcepciones();
            string valorConfig = "", linea = "";
            string rutaArchivo = ObtenerRutaInstalacion() + @"\Conexion.txt";


            FileStream fs;
            StreamReader r;
            try
            {
                fs = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
                r = new StreamReader(fs);
                linea = r.ReadLine();
                while (linea != null)
                {
                    dictionary.Add(linea.Substring(0, linea.IndexOf("=")), linea.Substring(linea.IndexOf("=") + 1, linea.Length - (linea.IndexOf("=") + 1)));
                    linea = r.ReadLine();
                }
                r.Close();

            }
            catch (Exception ex)
            {
                //Obtener cadena de conexión
                string cadena = string.Empty;
                foreach (var pair in dictionary)
                {
                    cadena =  pair.Value;
                    break;
                }

                string mensaje = string.Empty;
                excepcion.RegistrarExcepcion(0, "ConectarBD-LeerConexiones", ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    valorConfig = excepcion.SerializarExMessage(cadena, ex);
            }

            return dictionary;
        }


        /// <summary>
        /// Método que regresa el directorio de instalación
        /// </summary>
        /// <returns>Ruta de instalación</returns>
        public static string ObtenerRutaInstalacion()
        {
            //return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

    }
}