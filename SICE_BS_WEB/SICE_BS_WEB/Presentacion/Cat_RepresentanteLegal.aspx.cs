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
using System.Drawing;

namespace SICE_BS_WEB.Presentacion
{
    public partial class Cat_RepresentanteLegal : System.Web.UI.Page
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

                    dtc = catalogo.Traer_Representantes_Legales(lblCadena.Text, ref mensaje);
                    if (dtc != null && dtc.Rows.Count > 0)
                    {
                        Grid.DataSource = Session["Grid"] = dtc;
                        Grid.DataBind();
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

        private void ConfigureExport(ASPxGridView grid)
        {
            PrintingSystemBase ps = new PrintingSystemBase();
            ps.ExportOptions.Xlsx.SheetName = "Datos Generales";
            
            ps.XlSheetCreated += ps_XlSheetCreated;
            PrintableComponentLinkBase link1 = new PrintableComponentLinkBase(ps);
            link1.PaperName = "DatosGenerales";
            link1.Component = Exporter;

            CompositeLinkBase compositeLink = new CompositeLinkBase(ps);
            compositeLink.Links.AddRange(new object[] { link1 });
            compositeLink.CreatePageForEachLink();
            
            ps.Dispose();
            grid.Settings.ShowColumnHeaders = true;           
        }

        void ps_XlSheetCreated(object sender, XlSheetCreatedEventArgs e)
        {
            if (e.Index == 0) e.SheetName = "Representante Legal";
            if (e.Index == 1) e.SheetName = "Representante Legal";
        }

        //Metodo que llama al toolbar para exportar datos
        protected void Grid_ToolbarItemClick(object source, ASPxGridToolbarItemClickEventArgs e)
        {

            if (Grid.VisibleRowCount > 0)
            {
                //ConfigureExport(Grid);

                switch (e.Item.Name)
                {
                    case "ExportToPDF":
                        Exporter.WritePdfToResponse("Catálogo Representante Legal");
                        break;
                    case "ExportToXLS":
                        Exporter.WriteXlsToResponse("Catálogo Representante Legal", new XlsExportOptionsEx() { SheetName = "Catálogo Representante Legal" });
                        break;
                    case "ExportToXLSX":
                        Exporter.WriteXlsxToResponse("Catálogo Representante Legal", new XlsxExportOptionsEx() { SheetName = "Catálogo Representante Legal" });
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
            ModalTitulo.InnerText = "Nuevo Representate Legal";
            DataBind();

            //Limpiar Campos
            txt_Nombre.Text = string.Empty;
            txt_Paterno.Text = string.Empty;
            txt_Materno.Text = string.Empty;
            txt_rfc.Text = string.Empty;
            txt_tel.Text = string.Empty;
            txt_correo.Text = string.Empty;
            ApartirDe.Text = DateTime.Now.ToShortDateString();
            Hasta.Text = DateTime.Now.ToShortDateString();

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
                    ModalTitulo.InnerText = "Editar Representate Legal";
                    DataBind();

                    //Asignar valor al campo txt
                    Session["REPRESENTANTEKEY"] = Grid.GetSelectedFieldValues("REPRESENTANTEKEY")[0].ToString().Trim();

                    txt_Nombre.Text = Grid.GetSelectedFieldValues("NOMBRE")[0].ToString().Trim();
                    txt_Paterno.Text = Grid.GetSelectedFieldValues("APELLIDOPATERNO")[0].ToString().Trim();
                    txt_Materno.Text = Grid.GetSelectedFieldValues("APELLIDOMATERNO")[0].ToString().Trim();
                    txt_rfc.Text = Grid.GetSelectedFieldValues("RFC")[0].ToString().Trim();
                    txt_tel.Text = Grid.GetSelectedFieldValues("TELEFONO")[0].ToString().Trim();
                    txt_correo.Text = Grid.GetSelectedFieldValues("CORREOELECTRONICO")[0].ToString().Trim();
                    string v_apartir = Grid.GetSelectedFieldValues("APARTIRDE")[0].ToString().Trim();
                    ApartirDe.Text = v_apartir.Substring(0,10);
                    string v_hasta = Grid.GetSelectedFieldValues("HASTA")[0].ToString().Trim();
                    Hasta.Text = v_hasta.Substring(0, 10);
                    if (Grid.GetSelectedFieldValues("FIRMA")[0].ToString().Trim().Length > 0)                   
                        BinaryImage.Value = (byte[])Grid.GetSelectedFieldValues("FIRMA")[0];                
                    else
                        BinaryImage.Value = null; 
                    valida_select = 1;
                }
            }

            if(valida_select == 0)
                AlertError("Debe seleccionar un representante legal para poder editar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Session["Cadena"] == null)
            {
                string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                Session["Tab"] = "Salir";
                Response.Write(alerta);
                return;
            }

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

            int total_representantes = 0;


            if (Session["Grid"] != null && ((DataTable)Session["Grid"]).Rows.Count > 0)
                total_representantes = ((DataTable)Session["Grid"]).Rows.Count;


            //Obtener valores de fechas
            DateTime? dateDesde = string.IsNullOrEmpty(ApartirDe.Text) ? (DateTime?)null : DateTime.Parse(ApartirDe.Text);
            DateTime? dateHasta = string.IsNullOrEmpty(Hasta.Text) ? (DateTime?)null : DateTime.Parse(Hasta.Text);


            string valida = string.Empty;

            //Validar que el dato a guardar o modificar no exista 
            if (txt_Nombre.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un nombre";
            }

            else if (txt_Paterno.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un apellido paterno";
            }

            else if (txt_Materno.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un apellido materno";
            }

            else if (txt_rfc.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un RFC valido";
            }

            //Validar fechas que existan
            else if ((dateDesde == null || dateHasta == null))
            {
                valida = "Debe seleccionar un rango de fechas validas";                
            }
            
            //Validar fechas rangos correctos
            else if ((dateDesde != null && dateHasta != null) && dateDesde > dateHasta)
            {
                valida = "La fecha a partir de, no puede ser mayor a la fecha hasta";                
            }

            else if (Session["Grid"] != null && ((DataTable)(Session["Grid"])).Rows.Count > 0)
            {
                foreach (DataRow fila in ((DataTable)(Session["Grid"])).Rows)
                {
                    DateTime? dateDesde_f = string.IsNullOrEmpty(fila["APARTIRDE"].ToString().Trim()) ? (DateTime?)null : DateTime.Parse(fila["APARTIRDE"].ToString().Trim());
                    DateTime? dateHasta_f = string.IsNullOrEmpty(fila["HASTA"].ToString().Trim()) ? (DateTime?)null : DateTime.Parse(fila["HASTA"].ToString().Trim());

                    //Nuevo
                    if (ModalTitulo.InnerText.Contains("Nuevo") && 
                        fila["NOMBRE"].ToString().ToUpper().Trim() == txt_Nombre.Text.ToUpper().Trim() &&
                        fila["APELLIDOPATERNO"].ToString().ToUpper().Trim() == txt_Paterno.Text.ToUpper().Trim() &&
                        fila["APELLIDOMATERNO"].ToString().ToUpper().Trim() == txt_Materno.Text.ToUpper().Trim() &&
                        fila["RFC"].ToString().ToUpper().Trim() == txt_rfc.Text.ToUpper().Trim() &&
                        dateDesde_f < dateDesde && dateHasta_f > dateDesde)
                    {
                        valida = "Ya existe este representante legal en un rango de fechas a las seleccionadas: Nombre(" + txt_Nombre.Text.Trim() + "), Ap Paterno(" + txt_Paterno.Text.Trim() +
                            "), Ap Materno(" + txt_Materno.Text.Trim() + "), RFC(" + txt_rfc.Text.Trim() + "), Fecha apartir de(" + fila["APARTIRDE"].ToString().Trim() + "), Fecha hasta(" + fila["HASTA"].ToString().Trim() + ") ";
                        break;
                    }


                    //Editar
                    if (ModalTitulo.InnerText.Contains("Editar") &&
                        fila["REPRESENTANTEKEY"].ToString().ToUpper().Trim() != Session["REPRESENTANTEKEY"].ToString().Trim().ToUpper() &&
                        fila["NOMBRE"].ToString().ToUpper().Trim() == txt_Nombre.Text.ToUpper().Trim() &&
                        fila["APELLIDOPATERNO"].ToString().ToUpper().Trim() == txt_Paterno.Text.ToUpper().Trim() &&
                        fila["APELLIDOMATERNO"].ToString().ToUpper().Trim() == txt_Materno.Text.ToUpper().Trim() &&
                        fila["RFC"].ToString().ToUpper().Trim() == txt_rfc.Text.ToUpper().Trim() &&
                        dateDesde_f < dateDesde && dateHasta_f > dateDesde)
                    {
                        valida = "Ya existe este representante legal en un rango de fechas a las seleccionadas: Nombre(" + txt_Nombre.Text.Trim() + "), Ap Paterno(" + txt_Paterno.Text.Trim() +
                            "), Ap Materno(" + txt_Materno.Text.Trim() + "), RFC(" + txt_rfc.Text.Trim() + "), Fecha apartir de(" + fila["APARTIRDE"].ToString().Trim() + "), Fecha hasta(" + fila["HASTA"].ToString().Trim() + ") ";
                        break;
                    }

                }
            }


            if (valida.Length > 0)
            {
                //Mantiene el modal
                MostrarModal();                

                AlertError(valida);
                return;
            }

            var imagen = BinaryImage.Value;

            //Guardar
            string mensaje = "";
            DataTable dt = new DataTable();

            if (ModalTitulo.InnerText.Contains("Nuevo"))
                dt = catalogo.Guardar_Representante_Legal(txt_Nombre.Text.ToUpper().Trim(), txt_Paterno.Text.ToUpper().Trim(), txt_Materno.Text.ToUpper().Trim(),
                                                          txt_rfc.Text.ToUpper().Trim(), txt_correo.Text.Trim(), txt_tel.Text.Trim(), dateDesde, dateHasta,
                                                          imagen, lblCadena.Text, ref mensaje);
            else if (ModalTitulo.InnerText.Contains("Editar"))
                dt = catalogo.Editar_Representante_Legal(int.Parse(Session["REPRESENTANTEKEY"].ToString()), txt_Nombre.Text.ToUpper().Trim(), txt_Paterno.Text.ToUpper().Trim(), 
                                                         txt_Materno.Text.ToUpper().Trim(),txt_rfc.Text.ToUpper().Trim(), txt_correo.Text.Trim(), txt_tel.Text.Trim(), dateDesde,
                                                         dateHasta, imagen, lblCadena.Text, ref mensaje);

            if (dt != null && dt.Rows.Count > 0)
            {
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
                Grid.Settings.VerticalScrollableHeight = 330;
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid.SettingsPager.PageSize = 20;

                //Selecccionar el primer registro del grid
                if (Session["Grid"] != null)
                    Grid.Selection.SelectRow(0);
                
                //AlertSuccess("El representante legal se " + (ModalTitulo.InnerText.Contains("Editar") ? "actualizó" : "agregó") + ".");
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
                    int id = int.Parse(Grid.GetSelectedFieldValues("REPRESENTANTEKEY")[0].ToString().Trim());
                    dt = catalogo.Eliminar_Representante_Legal(id, lblCadena.Text, ref mensaje);
                   
                    Grid.DataSource = Session["Grid"] = dt;
                    Grid.DataBind();
                    Grid.SettingsPager.PageSize = 20;

                    //Selecccionar el primer registro del grid
                    if (Session["Grid"] != null)
                        Grid.Selection.SelectRow(0);

                    AlertSuccess("El representnte legal se eliminó con éxito.");
                    valida_select = 1;
                    
                    Grid.Settings.VerticalScrollableHeight = 330;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar un representante legal para poder eliminar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }


        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            if (Grid.VisibleRowCount > 0)
            {
                Exporter.WriteXlsToResponse("Catálogo Representante Legal", new XlsExportOptionsEx() { SheetName = "Catálogo Representante Legal" });
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
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid.SettingsPager.PageSize = 20;
                Grid.Settings.VerticalScrollableHeight = 330;
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