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

namespace SICE_BS_WEB.Presentacion
{
    public partial class Cat_TiposActivoFijo : System.Web.UI.Page
    {
        Catalogos catalogo = new Catalogos();
        Inicio inicio = new Inicio();
        Perfiles perfiles = new Perfiles();
        ControlExcepciones excepcion = new ControlExcepciones();
        static string nombreArchivo = string.Empty;
        static string tituloPagina = string.Empty;
        protected static string tituloPanel = string.Empty;
        static bool permisoConsultar = false;
        static bool permisoAgregar = false;
        static bool permisoEditar = false;
        static bool permisoEliminar = false; 
        static bool permisoExportar = false;        
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
                        tituloPagina = dt.Rows[0]["NombreModulo"].ToString();
                        permisoConsultar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Consultar"].ToString()));
                        if (!permisoConsultar)
                            Response.Redirect("Default.aspx");
                        permisoAgregar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Agregar"].ToString()));
                        permisoEditar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Editar"].ToString()));
                        permisoEliminar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Eliminar"].ToString()));
                        permisoExportar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Exportar"].ToString()));
                        Page.Title = tituloPagina;
                    }

                    Session["Grid"] = null;
                    TituloPanel(string.Empty);

                    string mensaje = "";
                    DataTable dtc = new DataTable();
                                        
                    //Traer información para el grid
                    dtc = new DataTable();
                    dtc = catalogo.TraerTiposActivoFijo(lblCadena.Text, ref mensaje);
                    if (dtc != null && dtc.Rows.Count > 0)
                    {
                        Grid.DataSource = Session["Grid"] = dtc;
                        Grid.DataBind();
                        Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                        Grid.SettingsPager.PageSize = 20;

                        //Selecccionar el primer registro del grid
                        if (Session["Grid"] != null)
                            Grid.Selection.SelectRow(0);
                    }
                    else
                    {
                        Grid.DataSource = Session["Grid"] = dtc;
                        Grid.DataBind();
                        AlertError("No hay información o intentelo de nuevo");
                    }

                    Grid.Settings.VerticalScrollableHeight = 330;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                    //Validamos los permisos
                    var lnk_Agregar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Nuevo");
                    var lnk_Editar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Editar");
                    var lnk_Eliminar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Eliminar");
                    var lkb_Excel = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Excel");

                    Session["permisoAgregar"] = permisoAgregar;
                    Session["permisoEditar"] = permisoEditar;
                    Session["permisoEliminar"] = permisoEliminar;
                    Session["permisoExportar"] = permisoExportar;

                    lnk_Agregar.Visible = permisoAgregar;
                    lnk_Editar.Visible = permisoEditar;
                    lnk_Eliminar.Visible = permisoEliminar;
                    lkb_Excel.Visible = permisoExportar;

                    SetColores();
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

        private void TituloPanel(string descripcion)
        {
            h1_titulo.InnerText = tituloPanel = tituloPagina + descripcion;
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

        //Metodo que llama al combo box al seleccionar la cantidad de registros en el page
        protected void PagerCombo_Load(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = Grid.SettingsPager.PageSize;
        }

        protected void Grid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            string commandName = e.CommandArgs.CommandName;
           
            if (commandName.Equals("Nuevo"))
            {

            }
        }

        //Metodo que llama al toolbar para exportar datos
        protected void Grid_ToolbarItemClick(object source, ASPxGridToolbarItemClickEventArgs e)
        {

            if (Grid.VisibleRowCount > 0)
            {
                switch (e.Item.Name)
                {
                    case "ExportToPDF":
                        Exporter.WritePdfToResponse("Catálogo Tipos de Activo Fijo");
                        break;
                    case "ExportToXLS":
                        Exporter.WriteXlsToResponse("Catálogo Tipos de Activo Fijo", new XlsExportOptionsEx() { SheetName = "Catálogo Tipos de Activo Fijo" });
                        break;
                    case "ExportToXLSX":
                        Exporter.WriteXlsxToResponse("Catálogo Tipos de Activo Fijo", new XlsxExportOptionsEx() { SheetName = "Catálogo Tipos de Activo Fijo" });
                        break;
                    default:
                        break;
                }
            }
            else
                AlertError("No hay información por exportar");
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

        //Metodo que muestra el modal
        private void MostrarModal()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnModal').click(); </script> ", false);
        }

        protected void lkb_Nuevo_Click(object sender, EventArgs e)
        {
            //Abre Modal
            MostrarModal();

            //Titulo del Modal
            ModalTitulo.InnerText = "Nuevo Tipo de Activo Fijo";
            DataBind();

            //Limpiar Campo
            txt_TipoActivoFijo.Text = string.Empty;

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Editar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            DataTable dt = new DataTable();
            int valida_select = 0;

            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                if (Grid.Selection.IsRowSelected(i))
                {
                    //Abre Modal
                    MostrarModal();

                    //Titulo del Modal
                    ModalTitulo.InnerText = "Editar Tipo de Activo Fijo";
                    DataBind();

                    //Asignar valores
                    Session["TIPOACTIVOKEY"] = Grid.GetSelectedFieldValues("TIPOACTIVOKEY")[0].ToString().Trim();
                    txt_TipoActivoFijo.Text = Grid.GetSelectedFieldValues("TIPOACTIVOFIJO")[0].ToString().Trim();                                                    

                    valida_select = 1;
                }
            }

            if(valida_select == 0)
                AlertError("Debe dar clic a un tipo de activo fijo para poder editar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

            string valida = string.Empty;

            //Validar que el dato a guardar o modificar no exista             
            if (txt_TipoActivoFijo.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un tipo de activo";
            }

            else if (Session["Grid"]!= null && ((DataTable)(Session["Grid"])).Rows.Count > 0)
            {
                foreach (DataRow fila in ((DataTable)(Session["Grid"])).Rows)
                {
                    //Al guardar
                    if (ModalTitulo.InnerText.Contains("Nuevo") &&  fila["TIPOACTIVOFIJO"].ToString().ToUpper().Trim() == txt_TipoActivoFijo.Text.ToUpper().Trim())
                    {
                        valida = "Ya existe el tipo de activo fijo: " + txt_TipoActivoFijo.Text.ToUpper().Trim();
                        break;
                    }

                    //Al editar
                    if (ModalTitulo.InnerText.Contains("Editar") && Session["TIPOACTIVOKEY"] != null &&
                        fila["TIPOACTIVOFIJO"].ToString().ToUpper().Trim() == txt_TipoActivoFijo.Text.ToUpper().Trim() &&
                        fila["TIPOACTIVOKEY"].ToString().Trim() != Session["TIPOACTIVOKEY"].ToString().Trim())
                    {
                        valida = "Ya existe el tipo de activo fijo: " + txt_TipoActivoFijo.Text.ToUpper().Trim();
                        break;
                    }
                }
            }

            if (valida.Length > 0)
            {
                //Mantiene el modal
                MostrarModal();

                //Titulo del Modal
                if (ModalTitulo.InnerText.Contains("Nuevo"))
                    ModalTitulo.InnerText = "Nuevo Tipo de Activo Fijo";
                else
                    ModalTitulo.InnerText = "Editar Tipo de Activo Fijo";
                DataBind();

                AlertError(valida);
                return;
            }

            //Guardar
            string mensaje = "";
            DataTable dt = new DataTable();           

            if (ModalTitulo.InnerText.Contains("Nuevo"))
                dt = catalogo.GuardarTiposActivoFijo(txt_TipoActivoFijo.Text.ToUpper().Trim(), int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);
            else if (ModalTitulo.InnerText.Contains("Editar"))
                dt = catalogo.EditarTiposActivoFijo(Int64.Parse(Session["TIPOACTIVOKEY"].ToString()), txt_TipoActivoFijo.Text.ToUpper().Trim(),
                    int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);

            if (dt != null && dt.Rows.Count > 0)
            {
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                Grid.SettingsPager.PageSize = 20;

                //Selecccionar el primer registro del grid
                if (Session["Grid"] != null)
                    Grid.Selection.SelectRow(0);

                AlertSuccess("El tipo de activo fijo se " + (ModalTitulo.InnerText.Contains("Editar") ? "actualizó" : "agregó") + ".");
            }
            else
            {
                Grid.DataSource = Session["Grid"];
                Grid.DataBind();
            }

            Grid.Settings.VerticalScrollableHeight = 330;
            Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Eliminar_Click(object sender, EventArgs e)
        {
            int valida_select = 0;

            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                if (Grid.Selection.IsRowSelected(i))
                {
                    string mensaje = string.Empty;
                    DataTable dt = new DataTable();
                    Int64 id = int.Parse(Grid.GetSelectedFieldValues("TIPOACTIVOKEY")[0].ToString().Trim());
                    dt = catalogo.EliminarTiposActivoFijo(id, int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);
                    
                    //Validación
                    if (dt.Rows.Count > 0)
                    {
                        if(dt.Rows[0]["TIPOACTIVOKEY"].ToString().Contains("-1"))
                        {
                            AlertError("No se puede eliminar el tipo de activo, está en uso en control activo fijo");
                            return;
                        }
                    }

                    Grid.DataSource = Session["Grid"] = dt;
                    Grid.DataBind();
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                    Grid.SettingsPager.PageSize = 20;

                    //Selecccionar el primer registro del grid
                    if (Session["Grid"] != null)
                        Grid.Selection.SelectRow(0);

                    AlertSuccess("El tipo de activo fijo se eliminó con éxito.");
                    valida_select = 1;

                    Grid.Settings.VerticalScrollableHeight = 330;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;                    
                }
            }

            if (valida_select == 0)
                AlertError("Debe dar clic a un tipo de activo fijo para poder eliminar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            if (Grid.VisibleRowCount > 0)
            {
                Exporter.WriteXlsToResponse("Catálogo Tipos de Activo Fijo", new XlsExportOptionsEx() { SheetName = "Catálogo Tipos de Activo Fijo" });
            }
            else
                AlertError("No hay información por exportar");
            
            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Actualizar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                Grid.DataSource = Session["Grid"];
                Grid.DataBind();
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                Grid.SettingsPager.PageSize = 20;
            }

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
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

        protected void PermisosUsuario()
        {
            if (Session["permisoAgregar"] != null)
            {
                //Validamos los permisos
                var lnk_Agregar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Nuevo");
                var lnk_Editar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Editar");
                var lnk_Eliminar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Eliminar");
                var lkb_Excel = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Excel");

                lnk_Agregar.Visible = bool.Parse(Session["permisoAgregar"].ToString());
                lnk_Editar.Visible = bool.Parse(Session["permisoEditar"].ToString());
                lnk_Eliminar.Visible = bool.Parse(Session["permisoEliminar"].ToString());
                lkb_Excel.Visible = bool.Parse(Session["permisoExportar"].ToString());
            }
        }

        protected void SetColores()
        {
            try
            {
                if (Session["ButtonBackColor"] != null)
                {
                    //Grid.Styles.Header.BackColor = System.Drawing.ColorTranslator.FromHtml(Session["ButtonBackColor"].ToString());
                    //Grid.Styles.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["ButtonFontColor"].ToString());
                    Grid.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml(Session["ButtonBackColor"].ToString());
                    Grid.Styles.SelectedRow.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["ButtonFontColor"].ToString());
                }
            }
            catch (Exception ex)
            { }
        }

    }
}