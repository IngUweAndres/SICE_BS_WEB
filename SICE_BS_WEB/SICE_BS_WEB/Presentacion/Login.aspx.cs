using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SICE_BS_WEB.Negocios;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace SICE_BS_WEB.Presentacion
{
    public partial class Login : System.Web.UI.Page
    {
        Inicio login = new Inicio();
        ControlExcepciones excepcion = new ControlExcepciones();

        //Colocar el rango de versión actualización a mostrar
        //  (1)    Versión principal
        //  (2)    Versión secundaria 
        //  (3)    Número de compilación
        //  (4)    Revisión
        protected string version = Assembly.GetExecutingAssembly().GetName().Version.ToString(4);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logged"] != null)
            {
                try
                {
                    string valor = Request.QueryString["valor"].ToString();
                    if (valor.Equals("cerrar"))
                    {
                        Session["Tab"] = "Salir";
                        Session["MenuBackColor"] = Session["MenuFontColor"] = Session["MenuBackSelectedColor"] =
                        Session["ButtonBackColor"] = Session["ButtonFontColor"] = Session["ButtonBackSelectedColor"] =
                        Session["ImagenKey"] = null;
                    }
                }
                catch { }
                

            }
            txtUsuario.Focus();

//#if DEBUG
//            txtUsuario.Text = "admin";
//            txtPassword.Text = "datacode";
//            LinkButton1_Click(null, null);
//#endif
            if (!Page.IsPostBack)
            {
                DataBind();
            }
        }

        //Metodo botón Iniciar
        //protected void LinkButton1_Click(object sender, EventArgs e)
        protected void btnInicio_Click(object sender, EventArgs e)
        {
            try
            {
                LoadingPanel1.ContainerElementID = "PanelBody";

                DataTable dt = new DataTable();
                string mensaje = string.Empty;
                if (txtUsuario.Text.Trim().Equals(string.Empty) || txtPassword.Text.Trim().Equals(string.Empty))
                {
                    AlertError("Para iniciar sesión debe ingresar usuario y contraseña");
                    return;
                }


                //Si se abre el modal el valor en txtPassword desaparece, por eso se pasa el valor a la session.
                Session["pwd"] = txtPassword.Text.Trim();

                //lee el archivo Conexion.txt y trae las cadenas de conexion
                Dictionary<string, string> dictionary = ConectarBD.TotalConexiones();
                foreach (var pair in dictionary)
                {
                    //lee la primera cadena de conexion
                    Session["Cadena"] = pair.Value;
                    break;                    
                }
                
                //Session["Cadena"] = ConfigurationManager.ConnectionStrings["SICE_WEB_Connection"].ToString();

                if (Session["Cadena"] != null)
                    lblCadena.Text = Session["Cadena"].ToString();
                else
                {
                    AlertError("No existe la cadena de conexión, revisar archivo Conexion.txt");
                    return;
                }

                //Se quita de la cadena la parte de XpoProvider para poder realizar la conexión, si es que existe
                if (Session["Cadena"].ToString().Contains("XpoProvider=MSSqlServer;"))
                    Session["Cadena"] = Session["Cadena"].ToString().Replace("XpoProvider=MSSqlServer;", "");
                

                //Usuario y Contraseña
                dt = login.DatosSesion(txtUsuario.Text.Trim(), txtPassword.Text.Trim(), 0, lblCadena.Text, ref mensaje);
                if (!mensaje.Equals(string.Empty))
                {
                    AlertError(mensaje);
                }
                else
                {
                    if (dt != null) //Usuario y Contraseña Existen
                    {
                        if (dt.Rows.Count == 1)
                        {
                            foreach (DataRow fila in dt.Rows)
                            {
                                foreach (var pair in dictionary)
                                {
                                    if (fila["BD"].ToString().ToUpper().Trim() == pair.Key.ToString().ToUpper().Trim())
                                    {
                                        Session["Cadena"] = lblCadena.Text= pair.Value;
                                        break;
                                    }
                                }
                            }

                            //Cambia la cadena de conexión del Web.config
                            var settings = ConfigurationManager.ConnectionStrings["SICE_WEB_Connection"];
                            var fi = typeof(ConfigurationElement).GetField("_bReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                            fi.SetValue(settings, false);
                            settings.ConnectionString = lblCadena.Text;


                            //Iniciar sesión
                            Session.Add("Logged", true);
                            Session.Add("IdUsuario", Convert.ToInt32(dt.Rows[0]["IdUsuario"].ToString()));
                            Session.Add("Nombre", dt.Rows[0]["Nombre"].ToString());
                            Session.Add("IdTipoUsuario", dt.Rows[0]["IdTipoUsuario"].ToString());
                            Session.Add("IdPerfil", Convert.ToInt32(dt.Rows[0]["IdPerfil"].ToString()));
                            //Session.Add("BaseDeDatos", ddlBase.SelectedItem.Text);
                            Session.Add("RFC", dt.Rows[0]["RFC"].ToString());
                            Session.Add("EMPRESA", dt.Rows[0]["EMPRESA"].ToString());
                            CreaMenu();


                            if (Session["Menu"] != null)
                                if (Request.QueryString["redirect"] == null)
                                    Response.Redirect("Default.aspx");
                                else
                                    Response.Redirect(Request.QueryString["redirect"]);

                            return;
                        }
                        else if (dt.Rows.Count > 1)
                        {
                            DataTable dtBD = new DataTable();
                            dtBD.Columns.Add("TextoBD", typeof(string));
                            dtBD.Columns.Add("ValorBD", typeof(string));

                            foreach (DataRow fila in dt.Rows)
                            {
                                foreach (var pair2 in dictionary)
                                {
                                    if (fila["BD"].ToString().ToUpper().Trim() == pair2.Key.ToString().ToUpper().Trim())
                                    {
                                        dtBD.Rows.Add(pair2.Key, pair2.Value);
                                        break;
                                    }
                                }
                            }

                            //Abre Modal
                            MostrarModalBases();

                            //Titulo del Modal
                            ModalBDTitulo.InnerText = "Bienvenido " + txtUsuario.Text.Trim();
                            DataBind();

                            //Borra items en el combo
                            cmbBD.Items.Clear();

                            //Agrega al combo "cmbBD" las bases de datos que pertenecen a la empresa                
                            cmbBD.DataSource = dtBD;


                            //Se pintan las bases de datos en el combo y por default no se selecciona ninguna
                            cmbBD.DataBind();
                            cmbBD.SelectedIndex = -1;

                            return;
                        }
                    }
                    else
                        AlertError("Usuario o contraseña no válido");
                }                
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Login-btnInicio_Click", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);

                AlertError(mensaje);
            }            
        }

        //Metodo que muestra el modal
        private void MostrarModalBases()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalBases", "<script> document.getElementById('btnModalBD').click(); </script> ", false);
        }

        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                string mensaje = string.Empty;

                if (cmbBD.SelectedIndex.Equals(-1))
                {
                    MostrarModalBases();
                    AlertError("Debe seleccionar una base de datos para iniciar sesión");
                    return;
                }

                Session["Cadena"] = lblCadena.Text = cmbBD.Value.ToString().Trim();

                //Cambia la cadena de conexión del Web.config llamada "SICE_WEB_Connection"
                var settings = ConfigurationManager.ConnectionStrings["SICE_WEB_Connection"];
                var fi = typeof(ConfigurationElement).GetField("_bReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                fi.SetValue(settings, false);               
                settings.ConnectionString = lblCadena.Text;


                dt = login.DatosSesion(txtUsuario.Text.Trim(), Session["pwd"].ToString().Trim(), 1, lblCadena.Text, ref mensaje);
                if (!mensaje.Equals(string.Empty))
                {
                    MostrarModalBases();
                    AlertError(mensaje);
                }
                else
                {
                    if (dt != null) //Sesión existe
                    {
                        //Iniciar sesión
                        Session.Add("Logged", true);
                        Session.Add("IdUsuario", Convert.ToInt32(dt.Rows[0]["IdUsuario"].ToString()));
                        Session.Add("Nombre", dt.Rows[0]["Nombre"].ToString());
                        Session.Add("IdTipoUsuario", dt.Rows[0]["IdTipoUsuario"].ToString());
                        Session.Add("IdPerfil", Convert.ToInt32(dt.Rows[0]["IdPerfil"].ToString()));
                        Session.Add("RFC", dt.Rows[0]["RFC"].ToString());
                        Session.Add("EMPRESA", dt.Rows[0]["EMPRESA"].ToString());
                        CreaMenu();

                        if (Session["Menu"] != null)
                            if (Request.QueryString["redirect"] == null)
                                Response.Redirect("Default.aspx", false);
                            else
                                Response.Redirect(Request.QueryString["redirect"]);
                    }
                    else
                    {
                        MostrarModalBases();
                        AlertError("Usuario o contraseña no válido");
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Login-btnIniciar_Click", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);

                AlertError(mensaje);
            } 
        }

        private void CreaMenu()
        {
            try
            {
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

                //Traerse Valores de la tabla Colores
                Configuracion con = new Configuracion();
                DataTable dtColor = new DataTable();
                string mjs = string.Empty;
                dtColor = con.Traer_Colores(lblCadena.Text, ref mjs);
                if (dtColor != null && dtColor.Rows.Count > 0)
                {
                    Session["MenuBackColor"] = dtColor.Rows[0]["MenuBackColor"].ToString();
                    Session["MenuFontColor"] = dtColor.Rows[0]["MenuFontColor"].ToString();
                    Session["MenuBackSelectedColor"] = dtColor.Rows[0]["MenuBackSelectedColor"].ToString();
                    Session["ButtonBackColor"] = dtColor.Rows[0]["ButtonBackColor"].ToString();
                    Session["ButtonFontColor"] = dtColor.Rows[0]["ButtonFontColor"].ToString();
                    Session["ButtonBackSelectedColor"] = dtColor.Rows[0]["ButtonBackSelectedColor"].ToString();
                }
                else
                {
                    //Valores de default
                    Session["MenuBackColor"] = "#F8F8F8";
                    Session["MenuFontColor"] = "black";
                    Session["MenuBackSelectedColor"] = "#E7E7E7";
                    Session["ButtonBackColor"] = "#F8F8F8";
                    Session["ButtonFontColor"] = "black";
                    Session["ButtonBackSelectedColor"] = "#E7E7E7";
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
                literal.Text += "</ul><a class='navbar-brand espacio_menu'></a>";

                Random rnd = new Random();

                //Crear opciones para perfil y crea logo del cliente
                literal.Text += @"<ul class='nav navbar-nav navbar-right'>
                <li class='dropdown'>
                    <a href='#' class='dropdown-toggle' data-toggle='dropdown' style='width:130px;height:50px;border:0; padding:0px;'>";
                if(valida_vacio.Equals(0))
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
                excepcion.RegistrarExcepcion(idusuario, "Login-CreaMenu", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
            } 
        }

        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModal.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
        }
    }
}