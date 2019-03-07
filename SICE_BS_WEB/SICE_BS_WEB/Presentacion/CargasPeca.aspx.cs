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

namespace SICE_BS_WEB.Presentacion
{
    public partial class CargasPeca : System.Web.UI.Page
    {
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
                    

                    //ds_filesDS.ConnectionString = lblCadena.Text;

                    nombreArchivo = Request.Path.Substring(Request.Path.LastIndexOf("/") + 1);
                    if (Session["Permisos"] != null)
                    {
                        DataTable dt = ((DataTable)Session["Permisos"]).Select("Archivo like '%" + nombreArchivo + "%'").CopyToDataTable();
                        h1_titulo.InnerText = dt.Rows[0]["NombreModulo"].ToString();
                    }

                    //Llenar la Session["Grid"] si es que hay información en el grid
                    UpdateGrid();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);

                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "CargasPeca-Page_Load", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        //Metodo para guardar archivos con fileupload
        protected void ASPxUploadControl1_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            int bandera = 0;

            try
            {
                //Validar que no se repitan los archivos
                string valida = string.Empty;

                if (Session["Grid"] != null && ((DataTable)(Session["Grid"])).Rows.Count > 0)
                {
                    foreach (DataRow fila in ((DataTable)(Session["Grid"])).Rows)
                    {
                        if (fila["PECANAME"].ToString().Trim().ToUpper() == e.UploadedFile.FileName.Trim().ToUpper())
                        {
                            valida = "Ya existe el archivo: " + e.UploadedFile.FileName.Trim();  
                            break;
                        }                    
                    }
                }

                if (valida.Length > 0)
                {                
                    AlertError(valida);
                    return;
                }

                if (Session["Cadena"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                //ds_filesDS.ConnectionString = Session["Cadena"].ToString();

                string cmdString = "INSERT INTO [dbo].[FILESPECA] (FPKEY,[PECANAME],[ARCHIVO],[STATUSPECA]) VALUES (NEWID(),@val1,@file,@val2)";
                //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["BD"].ConnectionString;
                string connString = Session["Cadena"].ToString();


                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = cmdString;
                        comm.Parameters.AddWithValue("@val1",  e.UploadedFile.FileName);
                        comm.Parameters.AddWithValue("@val2", "NUEVO");
                        comm.Parameters.Add("@File", SqlDbType.VarBinary, e.UploadedFile.FileBytes.Length).Value = e.UploadedFile.FileBytes;
                
                            conn.Open();
                            comm.ExecuteNonQuery();
                            bandera = 1;
                    }
                }

                //Se actualiza grid
                UpdateGrid();
            }
            catch (Exception ex)
            {
                if (bandera.Equals(0))
                    AlertError("No se cargó el archivo. " + ex.Message);
                else
                    AlertError(ex.Message);

                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "CargasPeca-ASPxUploadControl1_FileUploadComplete", ex, Session["Cadena"].ToString(), ref mensaje);
            }

        }

        //Metodo que se ejecuta cuando se da clic en botón eliminar, descargar archivo o procesar archivo
        protected void ASPxGridView1_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                //ds_filesDS.ConnectionString = Session["Cadena"].ToString();

                string cmdString = string.Empty;
                string connString = Session["Cadena"].ToString();

                ASPxGridView grid = sender as ASPxGridView;
                var tmp = grid.GetRowValues(e.VisibleIndex, grid.KeyFieldName);

                DataTable dt = new DataTable();

                if (e.ButtonID == "btnEliminar")
                {
                    cmdString = "DELETE FROM [FILESPECA] WHERE [FPKEY] = @FPKEY";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandText = cmdString;
                            comm.Parameters.Add("@FPKEY", SqlDbType.UniqueIdentifier).Value = tmp;
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

                    //Se actualiza grid
                    UpdateGrid();
                    AlertSuccess("Archivo eliminado con éxito");
                }
                else if (e.ButtonID == "ID_DOWNLOAD")
                {
                    byte[] bytes = null;
                    string nombreArchivo = "";
                    var parent = System.IO.Directory.GetParent(Environment.CurrentDirectory);
                    string path = parent.Parent + "\\DS";
                    string target = path + "";

                    //Crea un directorio
                    if (!Directory.Exists(target))
                    {
                        Directory.CreateDirectory(target);
                    }

                    cmdString = "SELECT TOP 1 ARCHIVO, PECANAME FROM [FILESPECA] WHERE [FPKEY] = @val1 ";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        using (SqlCommand comm = new SqlCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandText = cmdString;
                            comm.Parameters.Add("@val1", SqlDbType.UniqueIdentifier).Value = tmp;

                            conn.Open();
                            SqlDataReader reader = comm.ExecuteReader();

                            if (!reader.HasRows)
                                dt = null;
                            else
                            {
                                reader.Read();
                                bytes = (byte[])reader["ARCHIVO"];
                                nombreArchivo = (string)reader["PECANAME"];

                                File.WriteAllBytes(path + "\\" + nombreArchivo, bytes);
                                AlertSuccess("Se descargo el archivo: " + nombreArchivo);
                            }

                        }

                        //(sender as ASPxGridView).JSProperties["cpResult"] = "DS\\" + nombreArchivo;
                        conn.Close();
                    }
                }

                if (e.ButtonID == "ID_PROCESAR")
                {
                    // AQUI SE DEBE MANDAR A EJECUTAR EL SP QUE PROCESA LOS ARCHIVOS.

                    try
                    {
                        string rfc = Session["RFC"].ToString().Trim();
                        if (rfc == null)
                        {
                            Response.Redirect("Login.aspx");
                            return;
                        }

                        TRespuesta res;
                        IExpedienteComercioservice v1 = new IExpedienteComercioservice();
                        res = v1.Procesapeca(tmp.ToString(), rfc);
                        string respuesta = res.ToString();

                    }
                    catch (Exception exc)
                    {
                        int idusuario = 0;
                        string mensaje = "";
                        if (Session["IdUsuario"] != null)
                            idusuario = int.Parse(Session["IdUsuario"].ToString());
                        excepcion.RegistrarExcepcion(idusuario, "ASPxGridView1_CustomButtonCallback", exc, lblCadena.Text, ref mensaje);

                        AlertError("Error al conectarse con el WebServices, intentelo más tarde");
                    }

                    //Se actualiza grid
                    UpdateGrid();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);

                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "CargasPeca-ASPxGridView1_CustomButtonCallback", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Por cada renglon en el grid entrará al metodo
        protected void ASPxGridView1_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.CellType == GridViewTableCommandCellType.Filter)
                return;


            if (e.ButtonID == "ID_PROCESAR")
            {
                ASPxGridView grid = sender as ASPxGridView;
                var status = grid.GetRowValues(e.VisibleIndex, "STATUSPECA");

                if (status.ToString().ToUpper().Trim().Equals("PROCESADO"))
                    e.Enabled = false;
                else
                    e.Enabled = true;
            }
        }

        #region GridPageSize, Page_Init, Grid_CustomCallback, PagerCombo_Load

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
            if (Session["Grid"] != null)
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

        //Metodo que llama al combo box al seleccionar la cantidad de registros en el page
        protected void PagerCombo_Load(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = Grid.SettingsPager.PageSize;
        }

        #endregion

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


        //Metodo que actualiza el grid en un datatable
        protected void UpdateGrid()
        {
            //Llenar la Session["Grid"] si es que hay información en el grid
            DataTable dt = new DataTable();

            string cmdString = "SELECT [FPKEY], [PECANAME], [STATUSPECA], [OBSERVACIONES] FROM [FILESPECA] ORDER BY [PECANAME], [FPKEY]";
            string connString = Session["Cadena"].ToString();

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

            if (dt != null && dt.Rows.Count > 0)
            {
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //Selecccionar el primer registro del grid
                if (Session["Grid"] != null)
                    Grid.Selection.SelectRow(0);
            }
            else
            {
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
            }

            Grid.Settings.VerticalScrollableHeight = 330;
            Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
        }

        //Metodo para exportar a excel
        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            if (Grid.VisibleRowCount > 0)
            {
                Exporter.WriteXlsToResponse("CargasPeca", new XlsExportOptionsEx() { SheetName = "CargasPeca" });
            }
            else
                AlertError("No hay información por exportar");

        }

        //Metodo que actualiza el grid
        protected void lkb_Actualizar_Click(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        //Método que limpia los filtros del grid
        protected void lkb_LimpiarFiltros_Click(object sender, EventArgs e)
        {
            foreach (GridViewColumn column in Grid.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    Grid.AutoFilterByColumn(column, "");
                    Grid.FilterExpression = String.Empty;
                    Grid.ClearSort();
                }
            }
        }
    }
}