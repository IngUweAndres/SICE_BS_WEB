using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using SICE_BS_WEB.Negocios;
using DevExpress.Web;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using System.IO;

namespace SICE_BS_WEB.Presentacion
{
    public partial class VentanillaUnica : System.Web.UI.Page
    {
        Configuracion config = new Configuracion();
        ControlExcepciones excepcion = new ControlExcepciones();
        static string nombreArchivo = string.Empty;
        static string tituloPagina = string.Empty;
        static bool permisoConsultar = false;        
        static bool permisoEditar = false;        
        bool isExport = false;
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
                    
                    nombreArchivo = Request.Path.Substring(Request.Path.LastIndexOf("/") + 1);
                    if (Session["Permisos"] != null)
                    {
                        DataTable dt = ((DataTable)Session["Permisos"]).Select("Archivo like '%" + nombreArchivo + "%'").CopyToDataTable();
                        h1_titulo.InnerText = tituloPagina = dt.Rows[0]["NombreModulo"].ToString();
                        permisoConsultar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Consultar"].ToString()));
                        if (!permisoConsultar)
                            Response.Redirect("Default.aspx");
                        permisoEditar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Editar"].ToString()));
                        btnEdtar.Visible = permisoEditar;
                        Page.Title = tituloPagina;
                    }


                    Session["Grid"] = null;
                    string mensaje = string.Empty;
                    DataTable dts = new DataTable();
                    dts = config.ConsultarSettings(lblCadena.Text.Trim(), ref mensaje);

                    Grid.DataSource = Session["Grid"] = dts;
                    Grid.DataBind();
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Page_Load", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
                //Response.Redirect("Login.aspx");
            }
        }
 
        //Propiedad GridPageSize
        protected int GridPageSize
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }


        //Metodo inicial de pantalla
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["Grid"] = null;
            }

            Grid.SettingsPager.PageSize = GridPageSize;
            
            //Cuando se quiera filtrar el Grid entra en el if
            if(Session["Grid"] != null)
            {
               Grid.DataSource = Session["Grid"];
               Grid.DataBind();
               Grid.SettingsPager.PageSize = GridPageSize;
            }

        }


        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize = int.Parse(e.Parameters);
            Grid.SettingsPager.PageSize = GridPageSize;
            Grid.DataBind();
        }

        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModal.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
        }


        //Metodo que muestra ventana de satisfactorio
        public void AlertSuccess(string mensaje)
        {
            pModalSucces.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnSuccess').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnSuccess').click(); </script> ", false);
        }


        private void MostrarModalSettings()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalSettings", "<script> document.getElementById('btnModalSettings').click(); </script> ", false);

            //Se ejecuta en un script para colocar valores en los cookies "CookieValorFK" y "CookieValorFC"
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "CookieValorFK", "<script> document.cookie='CookieValorFK=no_guarda;'; </script> ", false);
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "CookieValorFC", "<script> document.cookie='CookieValorFC=no_guarda;'; </script> ", false);
        }


        //Botón Editar
        protected void btnEdtar_OnClick(object sender, EventArgs e)
        {
            try
            {
                MostrarModalSettings();
                Session["keyfile"] = null;
                Session["cerfile"] = null;

                ModalSettingsTitulo.InnerText = "Editar Ventanilla Única";

                string mensaje = string.Empty;

                if (Session["Grid"] != null && ((DataTable)(Session["Grid"])).Rows.Count > 0)
                {
                    DataTable dt = ((DataTable)(Session["Grid"]));

                    txt_settings_password.Attributes.Add("value", dt.Rows[0]["PASSWORD"].ToString());
                    txt_settings_claveabrirllave.Attributes.Add("value", dt.Rows[0]["CLAVEABRIRLLAVE"].ToString());
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "VentanillaUnica-btnEdtar_OnClick", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Guardar en una tabla el archivo de FKeyfile
        //Este evento se genera cuando se da clic en botón subir archivo
        protected void ucFKeyfile_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                System.IO.FileInfo fileinfo = new System.IO.FileInfo(e.UploadedFile.FileName);
                string fileNameComplete = e.UploadedFile.FileName;
                string name = System.IO.Path.GetFileNameWithoutExtension(fileNameComplete);
                string ruta = MapPath(fileNameComplete);
                string directory = "C:/KEYFILE/" + fileNameComplete;

                //Crea el directorio C:/KEYFILE/
                var parent = System.IO.Directory.GetParent(Environment.CurrentDirectory);
                string path = parent.Parent + "\\KEYFILE";
                string target = path + "";

                //Crea un directorio
                if (Directory.Exists(target))
                {
                    //SI existe un archivo anterior lo borra mientras este en la session
                    if (Session["keyfile"] != null)
                    {
                        try { File.Delete(Session["keyfile"].ToString()); }
                        catch { };
                    }
                }
                else
                    Directory.CreateDirectory(target);

                e.UploadedFile.SaveAs(directory);
                Session["keyfile"] = directory;

                //Se limpia la sesion file y se le agrega nueva información en un datatable
                Session["FilesFK"] = null;
                DataTable dt = new DataTable();
                dt.Columns.Add("nombre_completo", typeof(string));
                dt.Columns.Add("nombre", typeof(string));
                dt.Columns.Add("ruta", typeof(string));
                dt.Columns.Add("directorio", typeof(string));
                dt.Columns.Add("file", typeof(byte[]));
                DataRow row;
                row = dt.NewRow();
                row["nombre_completo"] = fileNameComplete;
                row["nombre"] = name;
                row["ruta"] = ruta;
                row["directorio"] = directory;
                row["file"] = e.UploadedFile.FileBytes;
                dt.Rows.Add(row);
                Session["FilesFK"] = dt;

                e.CallbackData = fileNameComplete;
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "VentanillaUnica-ucFKeyfile_FileUploadComplete", ex, lblCadena.Text, ref mensaje);
            }
        }

        
        //Guardar en una tabla el archivo de ucfcfile
        //Este evento se genera cuando se da clic en botón subir archivo
        protected void ucfcfile_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                string fileNameComplete = e.UploadedFile.FileName;
                string name = System.IO.Path.GetFileNameWithoutExtension(fileNameComplete);
                string ruta = MapPath(fileNameComplete);
                string directory = "C:/CERFILE/" + fileNameComplete;

                //Crea el directorio C:/KEYFILE/
                var parent = System.IO.Directory.GetParent(Environment.CurrentDirectory);
                string path = parent.Parent + "\\CERFILE";
                string target = path + "";

                //Crea un directorio
                if (Directory.Exists(target))
                {
                    //SI existe un archivo anterior lo borra mientras este en la session
                    if (Session["cerfile"] != null)
                    {
                        try { File.Delete(Session["cerfile"].ToString()); }
                        catch { };
                    }
                }
                else
                    Directory.CreateDirectory(target);

                e.UploadedFile.SaveAs(directory);
                Session["cerfile"] = directory;

                //Se limpia la sesion file y se le agrega nueva información en un datatable
                Session["FilesFC"] = null;
                DataTable dt = new DataTable();
                dt.Columns.Add("nombre_completo", typeof(string));
                dt.Columns.Add("nombre", typeof(string));
                dt.Columns.Add("ruta", typeof(string));
                dt.Columns.Add("directorio", typeof(string));
                dt.Columns.Add("file", typeof(byte[]));
                DataRow row;
                row = dt.NewRow();
                row["nombre_completo"] = fileNameComplete;
                row["nombre"] = name;
                row["ruta"] = ruta;
                row["directorio"] = directory;                
                row["file"] = e.UploadedFile.FileBytes;
                dt.Rows.Add(row);
                Session["FilesFC"] = dt;

                e.CallbackData = fileNameComplete;
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "VentanillaUnica-ucfcfile_FileUploadComplete", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Evento que guardará en BD los cambios de algunos campos en la tabla settings
        protected void btnGuardarSettings_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = "";
                string valor = string.Empty;
                string password = txt_settings_password.Text.Trim();
                string clave = txt_settings_claveabrirllave.Text.Trim();
                string rkeyfile = string.Empty;
                string rcerfile = string.Empty;
                byte[] bytesFK = null;
                byte[] bytesFC = null;

                string cookieFK = Request.Cookies["CookieValorFK"].Value;
                if (!cookieFK.Equals("no_guarda") && Session["FilesFK"] != null && ((DataTable)(Session["FilesFK"])).Rows.Count > 0)
                {
                    foreach (DataRow fila in ((DataTable)(Session["FilesFK"])).Rows)
                    {
                        rkeyfile = fila["directorio"].ToString();
                        bytesFK = (byte[])fila["file"];
                        break;
                    }
                }

                string cookieFC = Request.Cookies["CookieValorFC"].Value;
                if (!cookieFC.Equals("no_guarda") && Session["FilesFC"] != null && ((DataTable)(Session["FilesFC"])).Rows.Count > 0)
                {
                    foreach (DataRow fila in ((DataTable)(Session["FilesFC"])).Rows)
                    {
                        rcerfile = fila["directorio"].ToString();
                        bytesFC = (byte[])fila["file"];
                        break;
                    }
                }

                //Guarda los cambios
                valor = config.EditarSettings(password, clave, rkeyfile, rcerfile, bytesFK, bytesFC, int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);

                //Valida resultado
                if (valor.Equals("Ok"))
                {
                    AlertSuccess("Cambios guardados con éxito");
                }
                else
                {
                    AlertError("No se pudo guardar los cambios, intentelo de nuevo");
                }


                //Se actualiza grid
                DataTable dts = new DataTable();
                dts = config.ConsultarSettings(lblCadena.Text.Trim(), ref mensaje);

                Grid.DataSource = Session["Grid"] = dts;
                Grid.DataBind();
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "VentanillaUnica-btnGuardarSettings_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        protected void btncerrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["keyfile"] != null)
                    File.Delete(lblkeyfile.Text);

                if (Session["cerfile"] != null)
                    File.Delete(lblcerfile.Text);
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "VentanillaUnica-btnGuardarSettings_Click", ex, lblCadena.Text, ref mensaje);
            }
        }


    }
}