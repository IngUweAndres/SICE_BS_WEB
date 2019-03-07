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
    public partial class ImagenLogo : System.Web.UI.Page
    {
        Configuracion config = new Configuracion();
        ControlExcepciones excepcion = new ControlExcepciones();
        static string nombreArchivo = string.Empty;
        const string PageSizeSessionKey = "ed5e843d-cff7-47a7-815e-832923f7fb09";

        ///SE CAMBIA EL COLOR DEL TEMA DEFINIDO EN EL WEB CONFIG
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
                    

                    h1_titulo.InnerText = "Configurar Imagen de Logo";


                    //Se actualiza grid
                    DataTable dt = new DataTable();
                    dt = TraerLogos();

                    Grid.DataSource = dt;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid.Settings.VerticalScrollableHeight = 330;
                    Grid.DataBind();                    
                                       
                }
            }
            catch(Exception ex)
            {
                AlertError(ex.Message);

                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ConfigImagenLogo-Page_Load", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        //Metodo del checkbox por fila izq
        protected void chkConsultarIzq_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarIzq{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarIzqClick(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ConfigImagenLogo-chkConsultarIzq_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Metodo del checkbox por fila der
        protected void chkConsultarDer_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarDer{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarDerClick(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ConfigImagenLogo-chkConsultarDer_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        protected void lkb_Nuevo_Click(object sender, EventArgs e)
        {
            MostrarModal();

            //Titulo del Modal
            ModalTitulo.InnerText = "Nuevo Logo";

            BinaryImage.Value = null;
            txt_FileName.Text = string.Empty;

        }

        protected void lkb_Aceptar_Click(object sender, EventArgs e)
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

                DataTable dtL = new DataTable();
                DataTable dtD = new DataTable();
                int idlogoI = 0;
                int idlogoD = 0;
                string cmdString = string.Empty;
                string connString = Session["Cadena"].ToString();


                for (int i = 0; i < Grid.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultarIzq = Grid.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid.Columns["ValidaIzquierdo"], "chkConsultarIzq") as ASPxCheckBox;
                    ASPxCheckBox chkConsultarDer = Grid.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid.Columns["ValidaDerecho"], "chkConsultarDer") as ASPxCheckBox;

                    if (chkConsultarIzq.Checked)
                    {
                        idlogoI = int.Parse(Grid.GetRowValues(i, "IdLogo").ToString());

                        //Actualiza cual imagen se usará y trae datos del Logo
                        cmdString = "UPDATE [LOGO] SET ValidaIzquierdo = 0; UPDATE [LOGO] SET ValidaIzquierdo = 1 WHERE [IdLogo] = @IdLogo; SELECT * FROM [LOGO] WHERE [IdLogo] = @IdLogo";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                comm.Parameters.Add("@IdLogo", SqlDbType.Int).Value = idlogoI;
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

                            //Descargar imagen en la carpeta si existe lo sobreescribe
                            fileN = dtL.Rows[0]["FileName"].ToString();
                            bytes = (byte[])dtL.Rows[0]["Logo"];
                            File.WriteAllBytes(pathName, bytes);
                        }
                    }


                    if (chkConsultarDer.Checked)
                    {
                        idlogoD = int.Parse(Grid.GetRowValues(i, "IdLogo").ToString());

                        //Actualiza cual imagen se usará y trae datos del Logo
                        cmdString = "UPDATE [LOGO] SET ValidaDerecho = 0; UPDATE [LOGO] SET ValidaDerecho = 1 WHERE [IdLogo] = @IdLogo; SELECT * FROM [LOGO] WHERE [IdLogo] = @IdLogo";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                comm.Parameters.Add("@IdLogo", SqlDbType.Int).Value = idlogoD;
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

                            //Descargar imagen en la carpeta si existe lo sobreescribe
                            fileN = dtD.Rows[0]["FileName"].ToString();
                            bytes = (byte[])dtD.Rows[0]["Logo"];
                            File.WriteAllBytes(pathName, bytes);
                        }
                    }
                }

                if (idlogoI == 0)
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

                    try
                    {
                        //Actualiza Logo Izquierdo
                        cmdString = "UPDATE [LOGO] SET ValidaIzquierdo = 0";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                comm.ExecuteNonQuery();
                            }
                            conn.Close();
                        }
                    }
                    catch(Exception ex){}
                }

                if (idlogoD == 0)
                {
                    //string path = string.Empty;
                    //string pathName1 = string.Empty;
                    //string pathName3 = string.Empty;
                    //string fileN = string.Empty;
                    //string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                    //path = Server.MapPath(FolderPath);
                    //path = path + "\\Logo";
                    //pathName1 = path + "\\vacia.bmp";
                    //pathName3 = path + "\\logo_derecho.bmp";

                    //File.Copy(pathName1, pathName3, true);

                    try
                    {
                        //Actualiza Logo Derecho
                        cmdString = "UPDATE [LOGO] SET ValidaDerecho = 0";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                comm.ExecuteNonQuery();
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception ex) { }
                }

                //Vuevle a crear el menú para llamar a la imagen nueva del logo derecho
                CreaMenu();

                //Trae imagenes activas solo si existen
                TraerImagenesActivas();
                (this.Master as Principal).ColoresMenuFooter();

                //Guid g = Guid.NewGuid();
                //string GuidString = Convert.ToBase64String(g.ToByteArray());
                //GuidString = GuidString.Replace("=", "");
                //GuidString = GuidString.Replace("+", "");

                //Image1.ImageUrl = "/path/to/new/image.jpg?r=" + GuidString;
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);

                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ConfigImagenLogo-lkb_Aceptar_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Eliminar y Editar Logo
        protected void ASPxGridView1_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
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

                string valida = string.Empty;
                string cmdString = string.Empty;
                string connString = Session["Cadena"].ToString();

                ASPxGridView grid = sender as ASPxGridView;
                Session["ImagenKey"] = grid.GetRowValues(e.VisibleIndex, grid.KeyFieldName).ToString();
                int key = int.Parse(Session["ImagenKey"].ToString());

                DataTable dt = new DataTable();

                if (e.ButtonID == "btnEliminar")
                {
                    cmdString = "DELETE FROM [LOGO] WHERE [IdLogo] = @IdLogo";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandText = cmdString;
                            comm.Parameters.Add("@IdLogo", SqlDbType.Int).Value = key;
                            comm.ExecuteNonQuery();
                            valida = "Ok";
                        }
                        conn.Close();
                    }

                    //Se actualiza grid
                    dt = new DataTable();
                    dt = TraerLogos();

                    Grid.DataSource = dt;
                    Grid.DataBind();

                    //Cambiar a imagenes de sice_m3
                    if(dt == null || dt.Rows.Count == 0)
                    {
                        string path = string.Empty;
                        string pathName = string.Empty;
                        string pathName1 = string.Empty;
                        string pathName2 = string.Empty;
                        string pathName3 = string.Empty;
                        string fileN = string.Empty;
                        string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                        path = Server.MapPath(FolderPath);
                        path = path + "\\Logo";
                        //pathName = path + "\\vacia.bmp";
                        pathName1 = path + "\\sice_m3.jpg";                        
                        pathName2 = path + "\\logo_izquierdo.bmp";
                        //pathName3 = path + "\\logo_derecho.bmp";

                        File.Copy(pathName1, pathName2, true);
                        //File.Copy(pathName, pathName3, true);
                    }

                    //Vuevle a crear el menú para llamar a la imagen nueva del logo derecho
                    CreaMenu();

                    //Trae imagenes activas solo si existen
                    TraerImagenesActivas();
                    (this.Master as Principal).ColoresMenuFooter();

                    if (valida == "Ok")
                        AlertSuccess("Archivo eliminado con éxito");
                }
                else if (e.ButtonID == "btnEditar")
                {
                    cmdString = "SELECT * FROM [LOGO] WHERE [IdLogo] = @IdLogo";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandText = cmdString;
                            comm.Parameters.Add("@IdLogo", SqlDbType.Int).Value = key;
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                    dt = null;
                                else
                                    dt.Load(reader);
                            }
                        }
                        conn.Close();
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        MostrarModal();

                        //Titulo del Modal
                        ModalTitulo.InnerText = "Editar Logo";

                        BinaryImage.Value = dt.Rows[0]["Logo"];
                        txt_FileName.Text = dt.Rows[0]["FileName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);

                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ConfigImagenLogo-ASPxGridView1_CustomButtonCallback", ex, lblCadena.Text, ref mensaje);
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

        private DataTable TraerLogos()
        {
            DataTable dt = new DataTable();
            try
            {
                string cmdString = string.Empty;
                string connString = Session["Cadena"].ToString();

                cmdString = "SELECT * FROM [LOGO] ORDER BY 1 ASC";
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
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);

                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ConfigImagenLogo-TraerLogos", ex, lblCadena.Text, ref mensaje);
            }

            return dt;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string valida = string.Empty;
            try
            {
                var img = BinaryImage.Value;

                string cmdString = string.Empty;
                string connString = Session["Cadena"].ToString();

                if (ModalTitulo.InnerText.Contains("Nuevo"))
                {
                    if (txt_FileName.Text.Trim().Length > 0 && BinaryImage.Value != null)
                    {
                        cmdString = "INSERT INTO [LOGO] (FileName,Logo,ValidaIzquierdo,ValidaDerecho) VALUES(@Nombre, @Logo, @ValidaIzq, @ValidaDer)";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                comm.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = txt_FileName.Text.Trim();
                                comm.Parameters.Add("@Logo", SqlDbType.Image).Value = img;
                                comm.Parameters.Add("@ValidaIzq", SqlDbType.Bit).Value = 0;
                                comm.Parameters.Add("@ValidaDer", SqlDbType.Bit).Value = 0;
                                comm.ExecuteNonQuery();
                                valida = "Ok";

                            }
                            conn.Close();
                        }
                    }
                    else
                    {
                        MostrarModal();
                        ModalTitulo.InnerText = "Nuevo Logo";
                        AlertError("Agregue una imagen valida y escriba un nombre para la imagen");

                    }
                }
                else if (ModalTitulo.InnerText.Contains("Editar"))
                {
                    if (txt_FileName.Text.Trim().Length > 0 && BinaryImage.Value != null)
                    {
                        cmdString = "UPDATE [LOGO] SET FileName = @Nombre, Logo = @Logo WHERE [IdLogo] = @IdLogo;";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                comm.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = txt_FileName.Text.Trim();
                                comm.Parameters.Add("@Logo", SqlDbType.Image).Value = img;
                                comm.Parameters.Add("@IdLogo", SqlDbType.Int).Value = int.Parse(Session["ImagenKey"].ToString());
                                comm.ExecuteNonQuery();
                                valida = "Ok";

                            }
                            conn.Close();
                        }
                    }
                    else
                    {
                        MostrarModal();
                        ModalTitulo.InnerText = "Editar Logo";
                        AlertError("Agregue una imagen valida y escriba un nombre para la imagen");

                    }
                }


                //Se actualiza grid
                DataTable dt = new DataTable();
                dt = TraerLogos();

                Grid.DataSource = dt;
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid.Settings.VerticalScrollableHeight = 330;
                Grid.DataBind();
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);

                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ConfigImagenLogo-btnGuardar_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        private void TraerImagenesActivas()
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
                    pathName = path + "\\logo_izquierdo.bmp";
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

                    //Descargar imagen en la carpeta
                    fileN = dtD.Rows[0]["FileName"].ToString();
                    pathName = path + "\\logo_derecho.bmp";
                    bytes = (byte[])dtD.Rows[0]["Logo"];
                    File.WriteAllBytes(pathName, bytes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                excepcion.RegistrarExcepcion(idusuario, "ImagenLogo-CreaMenu", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
            }
        }

    }
}