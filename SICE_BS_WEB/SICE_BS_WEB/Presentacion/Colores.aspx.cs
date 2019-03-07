using DevExpress;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SICE_BS_WEB.Negocios;
using System.Reflection;
using SICE_BS_WEB.WebReference;
using System.IO.Compression;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using System.Configuration;


namespace SICE_BS_WEB.Presentacion
{
    public partial class Colores : System.Web.UI.Page
    {
        Configuracion config = new Configuracion();
        ControlExcepciones excepcion = new ControlExcepciones();
        static string nombreArchivo = string.Empty;
        const string PageSizeSessionKey = "ed5e843d-cff7-47a7-815e-832923f7fb09";

        //SE CAMBIA EL COLOR DEL TEMA DEFINIDO EN EL WEB CONFIG
        protected void Page_PreInit(object sender, EventArgs e)
        {
            (this.Master as Principal).ColoresMenuFooter();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null)
                {
                    //string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";                    
                    //Response.Write(alerta);
                    Session["Tab"] = "Salir";
                    Response.Redirect("Login.aspx", false);
                    return;
                }
                else
                {
                    lblCadena.Text = Session["Cadena"].ToString();
                    Session["Tab"] = "Inicio";
                }

                if (!Page.IsPostBack)
                {
                    

                    MostrarModal();
                    ModalTitulo.InnerHtml = "Configurar Colores";

                    ce_MenuBackColor.Text = Session["MenuBackColor"].ToString();
                    ce_MenuFontColor.Text = Session["MenuFontColor"].ToString();
                    ce_MenuBackSelectedColor.Text = Session["MenuBackSelectedColor"].ToString();
                    ce_btnBackColor.Text = Session["ButtonBackColor"].ToString();
                    ce_btnFontColor.Text = Session["ButtonFontColor"].ToString();
                    ce_btnBackSelectedColor.Text = Session["ButtonBackSelectedColor"].ToString();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);

                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ConfigColores-Page_Load", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        private void MostrarModal()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnModal').click(); </script> ", false);
        }

        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModal.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "AlertError", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
        }

        //Metodo que muestra ventana de satisfactorio
        public void AlertSuccess(string mensaje)
        {
            pModalSucces.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnSuccess').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnSuccess').click(); </script> ", false);
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }
                else
                {
                    lblCadena.Text = Session["Cadena"].ToString();
                    Session["Tab"] = "Inicio";
                }

                string valida = string.Empty;
                string mensaje = string.Empty;

                valida = config.Guardar_Colores(ce_MenuBackColor.Text.Trim(), ce_MenuFontColor.Text.Trim(), ce_MenuBackSelectedColor.Text.Trim(),
                                                ce_btnBackColor.Text.Trim(), ce_btnFontColor.Text.Trim(), ce_btnBackSelectedColor.Text.Trim(),
                                                Session["Cadena"].ToString().Trim(), ref mensaje);

                if (valida.Contains("Ok"))
                {
                    string path = ConfigurationManager.AppSettings["FolderPath"];
                    string folderPath = Server.MapPath(path);
                    folderPath = Path.GetDirectoryName(folderPath);
                    folderPath = folderPath + @"\Content\style.css";

                    string ceBck = ce_MenuBackColor.Text.Trim();
                    string ceTxt = ce_MenuFontColor.Text.Trim();
                    string ceBckF = ce_MenuBackSelectedColor.Text.Trim();
                    string ceBBck = ce_btnBackColor.Text.Trim();
                    string ceBTxt = ce_btnFontColor.Text.Trim();
                    string ceBBckF = ce_btnBackSelectedColor.Text.Trim();

                    if (ceBck.Length.Equals(4))
                        ceBck = ceBck + ceBck.Substring(ceBck.Length - 3, 3);

                    if (ceTxt.Length.Equals(4))
                        ceTxt = ceTxt + ceTxt.Substring(ceTxt.Length - 3, 3);

                    if (ceBckF.Length.Equals(4))
                        ceBckF = ceBckF + ceBckF.Substring(ceBckF.Length - 3, 3);

                    if (ceBBck.Length.Equals(4))
                        ceBBck = ceBBck + ceBBck.Substring(ceBBck.Length - 3, 3);

                    if (ceBTxt.Length.Equals(4))
                        ceBTxt = ceBTxt + ceBTxt.Substring(ceBTxt.Length - 3, 3);

                    if (ceBBckF.Length.Equals(4))
                        ceBBckF = ceBBckF + ceBBckF.Substring(ceBBckF.Length - 3, 3);

                    if (File.Exists(folderPath))
                    {

                        //Version de linea por linea
                        var linea_txt = File.ReadLines(folderPath);
                        List<string> linesToWrite = new List<string>();

                        string linea = string.Empty;
                        int total_lineas = linea_txt.Count();

                        for (int i = 0; i < total_lineas; i++)
                        {
                            //Color fondo Menú y Footer
                            if (linea_txt.ToList()[i].ToString().Contains("/*M*/"))
                            {
                                string valor = linea_txt.ToList()[i].Substring(linea_txt.ToList()[i].IndexOf("#"), 7);
                                linea = linea_txt.ToList()[i].Replace(valor, ceBck);
                                linesToWrite.Add(linea);
                            }

                            //Color texto Menú y Footer
                            else if (linea_txt.ToList()[i].ToString().Contains("/*T*/"))
                            {
                                string valor = linea_txt.ToList()[i].Substring(linea_txt.ToList()[i].IndexOf("#"), 7);
                                linea = linea_txt.ToList()[i].Replace(valor, ceTxt);
                                linesToWrite.Add(linea);
                            }

                            //Color fondo Selected Menú y Footer
                            else if (linea_txt.ToList()[i].ToString().Contains("/*S*/"))
                            {
                                string valor = linea_txt.ToList()[i].Substring(linea_txt.ToList()[i].IndexOf("#"), 7);
                                linea = linea_txt.ToList()[i].Replace(valor, ceBckF);
                                linesToWrite.Add(linea);
                            }

                            //Color fondo Títulos y Botones
                            else if (linea_txt.ToList()[i].ToString().Contains("/*B*/"))
                            {
                                string valor = linea_txt.ToList()[i].Substring(linea_txt.ToList()[i].IndexOf("#"), 7);
                                linea = linea_txt.ToList()[i].Replace(valor, ceBBck);
                                linesToWrite.Add(linea);
                            }

                            //Color texto Títulos y Botones
                            else if (linea_txt.ToList()[i].ToString().Contains("/*BT*/"))
                            {
                                string valor = linea_txt.ToList()[i].Substring(linea_txt.ToList()[i].IndexOf("#"), 7);
                                linea = linea_txt.ToList()[i].Replace(valor, ceBTxt);
                                linesToWrite.Add(linea);
                            }

                            //Color fondo Selected Títulos y Botones
                            else if (linea_txt.ToList()[i].ToString().Contains("/*BS*/"))
                            {
                                string valor = linea_txt.ToList()[i].Substring(linea_txt.ToList()[i].IndexOf("#"), 7);
                                linea = linea_txt.ToList()[i].Replace(valor, ceBBckF);
                                linesToWrite.Add(linea);
                            }
                            else
                                linesToWrite.Add(linea_txt.ToList()[i].ToString());
                        }

                        File.WriteAllLines(folderPath, linesToWrite);

                        Session["MenuBackColor"] = ce_MenuBackColor.Text.Trim();
                        Session["MenuFontColor"] = ce_MenuFontColor.Text.Trim();
                        Session["MenuBackSelectedColor"] = ce_MenuBackSelectedColor.Text.Trim();
                        Session["ButtonBackColor"] = ce_btnBackColor.Text.Trim();
                        Session["ButtonFontColor"] = ce_btnFontColor.Text.Trim();
                        Session["ButtonBackSelectedColor"] = ce_btnBackSelectedColor.Text.Trim();

                        //Borrar cache
                        //string[] Cookies = System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
                        //foreach (string currentFile in Cookies)
                        //{
                        //    try
                        //    {
                        //        System.IO.File.Delete(currentFile);
                        //    }

                        //    catch (Exception ex)
                        //    {
                        //        AlertError(ex.Message);
                        //    }
                        //}
                        //string[] InterNetCache = System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
                        //foreach (string currentFile in InterNetCache)
                        //{
                        //    try
                        //    {
                        //        System.IO.File.Delete(currentFile);
                        //    }

                        //    catch (Exception ex)
                        //    {
                        //        AlertError(ex.Message);
                        //    }
                        //}

                        //string[] CommonPictures = System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
                        //foreach (string currentFile in CommonPictures)
                        //{
                        //    try
                        //    {
                        //        System.IO.File.Delete(currentFile);
                        //    }

                        //    catch (Exception ex)
                        //    {
                        //        AlertError(ex.Message);
                        //    }
                        //}


                        CreaMenu();     //Entra en este método para reepintar el background del menu, footer y texto en ellos.
                        (this.Master as Principal).ColoresMenuFooter();
                        AlertSuccess("Cambio guardado con éxito");
                    }
                }
                else
                {
                    MostrarModal();
                    AlertError(valida);
                    return;
                }

                Response.Redirect("Default.aspx", false);

            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Default.aspx", false);
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }


        private void CreaMenu()
        {
            try
            {
                Inicio login = new Inicio();

                //Traer Logo Izq
                DataTable dtL = new DataTable();
                string cmdString = string.Empty;
                string connString = lblCadena.Text;
                cmdString = "SELECT * FROM [LOGO] WHERE [ValidaIzquierdo] = 1";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = cmdString;
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dtL = null;
                            else
                                dtL.Load(reader);
                        }
                    }
                    conn.Close();
                }

                if (dtL != null && dtL.Rows.Count > 0)
                {

                    byte[] bytes = null;
                    string path = string.Empty;
                    string pathName = string.Empty;
                    string fileN = string.Empty;
                    string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                    path = Server.MapPath(FolderPath);
                    path = path + "\\Logo";
                    pathName = path + "\\logo_izquierdo.bmp";

                    //Si no existe la carpeta la crea, si existe la borra con su contenido y vuelve a crear la carpeta
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //else
                    //{
                    //    Directory.Delete(path, true);
                    //    Directory.CreateDirectory(path);
                    //}    

                    //Descargar imagen en la carpeta
                    fileN = dtL.Rows[0]["FileName"].ToString();
                    bytes = (byte[])dtL.Rows[0]["Logo"];
                    File.WriteAllBytes(pathName, bytes);

                }
                else
                {
                    string path = string.Empty;
                    string pathName1 = string.Empty;
                    string pathName2 = string.Empty;
                    string fileN = string.Empty;
                    string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                    path = Server.MapPath(FolderPath);
                    path = path + "\\Logo";
                    pathName1 = path + "\\sice_m3.jpg";
                    pathName2 = path + "\\logo_izquierdo.bmp";
                    File.Copy(pathName1, pathName2, true);
                }


                //Traer Logo Der
                int valida_vacio = 0;
                DataTable dtD = new DataTable();
                string cmdStringD = string.Empty;
                cmdStringD = "SELECT * FROM [LOGO] WHERE [ValidaDerecho] = 1";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = cmdStringD;
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dtD = null;
                            else
                                dtD.Load(reader);
                        }
                    }
                    conn.Close();
                }

                if (dtD != null && dtD.Rows.Count > 0)
                {

                    byte[] bytes = null;
                    string path = string.Empty;
                    string pathName = string.Empty;
                    string fileN = string.Empty;
                    string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                    path = Server.MapPath(FolderPath);
                    path = path + "\\Logo";
                    pathName = path + "\\logo_derecho.bmp";

                    //Descargar imagen en la carpeta
                    fileN = dtD.Rows[0]["FileName"].ToString();
                    bytes = (byte[])dtD.Rows[0]["Logo"];
                    File.WriteAllBytes(pathName, bytes);

                }
                else
                {
                    valida_vacio = 1;
                }


                DataTable dt = new DataTable();
                DataTable dwModulos = new DataTable();
                string mensaje = string.Empty;
                LiteralControl literal = new LiteralControl();
                dt = login.DatosPermisos(int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);
                if (!mensaje.Equals(string.Empty))
                {
                    AlertError(mensaje);
                    return;
                }
                //Consulta modulos padre (encabezados de menu)
                try
                {
                    dwModulos = dt.Select("IdModuloPadre IS NULL AND Consultar = '1'", "Orden ASC").CopyToDataTable();
                    Session.Add("Permisos", dt);
                }
                catch (Exception ex)
                {
                    //AlertError(excepcion.SerializarExMessage(lblCadena.Text,ex));
                }
                //Crear menu html
                literal.Text += @"<ul class='nav navbar-nav'>";
                if (dwModulos.Rows.Count > 0)
                {
                    foreach (DataRow rowModulos in dwModulos.Rows)
                    {
                        ////SI EL NOMBRE DEL MENÚ ES DATA STAGE QUITARLE PROPIEDADES DE CLASE
                        //if (rowModulos["RibbonName"].ToString().ToUpper().Contains("DATA STAGE"))
                        //    literal.Text += @"<li><a href='" + rowModulos["Archivo"].ToString() + "'>" + rowModulos["RibbonName"].ToString();
                        //else
                        //literal.Text += @"<li class='dropdown'><a href='" + rowModulos["Archivo"].ToString() + "' class='dropdown-toggle' data-toggle='dropdown' style='background-color:" + Session["MenuBackColor"].ToString() + "'><font color='" + Session["MenuFontColor"].ToString() + @"'>" + rowModulos["RibbonName"].ToString();
                        literal.Text += @"<li class='dropdown'><a href='#' class='dropdown-toggle' data-toggle='dropdown' style='background-color:" + Session["MenuBackColor"].ToString() + "'><font color='" + Session["MenuFontColor"].ToString() + @"'>" + rowModulos["RibbonName"].ToString();

                        DataTable dtModulos2 = new DataTable();
                        //Consulta submodulos de modulos padre
                        try
                        {
                            dtModulos2 = dt.Select("IdModuloPadre = '" + rowModulos["IdModulo"].ToString() + "' AND Consultar = '1'", "Orden ASC").CopyToDataTable();
                        }
                        catch (Exception ex)
                        {
                            //AlertError(excepcion.SerializarExMessage(lblCadena.Text, ex));
                        }

                        if (dtModulos2.Rows.Count > 0)
                        {
                            literal.Text += @"&nbsp;&nbsp;<span class='caret'></span></font></a>";

                            //SI EL NOMBRE DEL MENÚ ES Informes Generales PONERLE LA CLASE QUE LLAMA A UN DROPDOWN dropdown-InformesGenerales
                            if (rowModulos["RibbonName"].ToString().Equals("Informes Generales"))
                                literal.Text += @"<ul class='dropdown-InformesGenerales'>";
                            else
                                literal.Text += @"<ul class='dropdown-menu'>";
                            foreach (DataRow rowModulos2 in dtModulos2.Rows)
                            {
                                //Consulta submodulos de submodulos
                                DataTable dtModulos3 = new DataTable();
                                try
                                {
                                    dtModulos3 = dt.Select("IdModuloPadre = '" + rowModulos2["IdModulo"].ToString() + "' AND Consultar = '1'", "Orden ASC").CopyToDataTable();
                                }
                                catch (Exception ex)
                                {
                                    //AlertError(excepcion.SerializarExMessage(lblCadena.Text, ex));
                                }

                                if (dtModulos3.Rows.Count > 0)
                                {
                                    literal.Text += "<li class='dropdown-submenu'><a href='" + rowModulos2["Archivo"].ToString() + "'><img src='../img/" + rowModulos2["Icono"].ToString() + "' style='height: 2em;' />&nbsp;&nbsp;" + rowModulos2["RibbonName"].ToString() + @"</a>
                                    <ul class='dropdown-menu'>";
                                    foreach (DataRow rowModulos3 in dtModulos3.Rows)
                                        literal.Text += "<li><a href='" + rowModulos3["Archivo"].ToString() + "'><img src='../img/" + rowModulos3["Icono"].ToString() + "' style='height: 2em;' />&nbsp;&nbsp;" + rowModulos3["RibbonName"].ToString() + "</a></li>";

                                    literal.Text += "</ul></li>";
                                }
                                else
                                    literal.Text += "<li><a href='" + rowModulos2["Archivo"].ToString() + "'><img src='../img/" + rowModulos2["Icono"].ToString() + "' style='height: 2em;' />&nbsp;&nbsp;" + rowModulos2["RibbonName"].ToString() + "</a></li>";
                            }
                            literal.Text += "</ul>";
                        }
                        else
                            literal.Text += "</a>";

                        literal.Text += "</li>";
                    }
                }
                literal.Text += "</ul>";

                Random rnd = new Random();

                //Crear opciones para perfil y crea logo del cliente
                literal.Text += @"<ul class='nav navbar-nav navbar-right'>
                <li class='dropdown'>
                    <a href='#' class='dropdown-toggle' data-toggle='dropdown' style='width:130px;height:50px;border:0; padding:0px;'>";
                if (valida_vacio.Equals(0))
                    literal.Text += @"<img src='../Presentacion/Logo/logo_derecho.bmp?" + rnd.Next() + @"' class='imagen-logo-right' />";
                else
                    literal.Text += @"";
                literal.Text += @"</a>
                    <ul class='dropdown-menu'>
                        <li><a>&nbsp;&nbsp;" + Session["Nombre"].ToString().Trim() + @"</a></li>
                        <li role='separator' class='divider'></li>";
                if (Session["Nombre"] != null && Session["Nombre"].ToString().ToUpper().Trim().Contains("ADMINISTRADOR"))
                    literal.Text += @"<li><a href='Colores.aspx'><span class='glyphicon glyphicon-th'></span>&nbsp;&nbsp;Colores</a></li>
                        <li role='separator' class='divider'></li><li><a href='ImagenLogo.aspx'><span class='glyphicon glyphicon-th'></span>&nbsp;&nbsp;Logos</a></li>
                        <li role='separator' class='divider'></li>";

                literal.Text += @"<li><a href='AcercaDe.aspx'><span class='glyphicon glyphicon-info-sign'></span>&nbsp;&nbsp;Acerca de</a></li>
                        <li role='separator' class='divider'></li>
                        <li><a href='Login.aspx?valor=cerrar'><span class='glyphicon glyphicon-log-out'></span>&nbsp;&nbsp;Cerrar Sesión</a></li>
                        
                    </ul>
                </li>                
            </ul>";
                Session["Menu"] = literal.Text;
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Colores-CreaMenu", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
            }
        }
    }
}