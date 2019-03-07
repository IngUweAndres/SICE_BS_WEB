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
    public partial class NumeroParte : System.Web.UI.Page
    {
        Inicio inicio = new Inicio();
        Perfiles perfiles = new Perfiles();
        ControlExcepciones excepcion = new ControlExcepciones();
        static string nombreArchivo = string.Empty;
        static string tituloPagina = string.Empty;
        protected static string tituloPanel = string.Empty;
        const string PageSizeSessionKey = "ed5e843d-cff7-47a7-815e-832923f7fb09";
        BoNumeroParte np = new BoNumeroParte();

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

                    //Titulo pantalla
                    h1_titulo.InnerText = "Número de Parte";                                        

                    Session["Grid"] = null;                    
                    string mensaje = string.Empty;

                    DataTable dtGrid = new DataTable();
                    dtGrid = np.Consultar_NP(Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);
                    Grid.DataSource = Session["Grid"] = dtGrid;
                    Grid.DataBind();
                    Grid.Settings.VerticalScrollableHeight = 150;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;


                    Session["Grid2"] = null;
                    mensaje = string.Empty;
                    DataTable dtGrid2 = new DataTable();                   
                    Grid2.DataSource = Session["Grid2"] = dtGrid2;
                    Grid2.DataBind();
                    Grid2.Settings.VerticalScrollableHeight = 200;
                    Grid2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                }

                //ASPxLabel lblPedimento = Grid.Toolbars.FindByName("Toolbar1").Items.FindByName("Links").FindControl("lblPedimento") as ASPxLabel;
                //lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();

                var lblPedimento = (ASPxLabel)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);                
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

        //Propiedad Grid2PageSize
        protected int Grid2PageSize
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid2.SettingsPager.PageSize;
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

            //Cuando se quiera filtrar el Grid2 entra en el if
            if (Session["Grid2"] != null)
            {
                Grid2.DataSource = Session["Grid2"];
                Grid2.DataBind();
                Grid2.SettingsPager.PageSize = Grid2PageSize;
            }

        }

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize = int.Parse(e.Parameters);
            Grid.SettingsPager.PageSize = GridPageSize;
            Grid.DataBind();
        }

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void Grid2_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Grid2PageSize = int.Parse(e.Parameters);
            Grid2.SettingsPager.PageSize = Grid2PageSize;
            Grid2.DataBind();
        }

        //Metodo que llama al combo box al seleccionar la cantidad de registros en el page
        protected void PagerCombo_Load(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = Grid.SettingsPager.PageSize;
        }

        private void ConfigureExport(ASPxGridView grid)
        {
            PrintingSystemBase ps = new PrintingSystemBase();
            ps.ExportOptions.Xlsx.SheetName = "Datos Generales";
            
            ps.XlSheetCreated += ps_XlSheetCreated;
            PrintableComponentLinkBase link1 = new PrintableComponentLinkBase(ps);
            link1.PaperName = "NúmeroDeParte";
            link1.Component = Exporter;

            CompositeLinkBase compositeLink = new CompositeLinkBase(ps);
            compositeLink.Links.AddRange(new object[] { link1 });
            compositeLink.CreatePageForEachLink();
            
            ps.Dispose();
            grid.Settings.ShowColumnHeaders = true;           
        }

        void ps_XlSheetCreated(object sender, XlSheetCreatedEventArgs e)
        {
            if (e.Index == 0) e.SheetName = "Número Parte";
            if (e.Index == 1) e.SheetName = "Número Parte";
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

        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            if (Grid.VisibleRowCount > 0)
            {
                Exporter.WriteXlsToResponse("Número de Parte", new XlsExportOptionsEx() { SheetName = "Número de Parte" });
            }
            else
                AlertError("No hay información por exportar");

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

        //Botón Regresar
        protected void lkb_Regresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Documentos.aspx", false);
            return;
        }

        //Botón Validar
        protected void bbValidar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }

                string mensaje = "";
                DataTable dt = new DataTable();
                dt = np.Validar_NP(Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);


                mensaje = "";
                dt = new DataTable();
                dt = np.Errores_Detalle_NP(Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1)
                        AlertError("Existe un error al validar el pedimento: " + Session["PEDIMENTOARMADO"].ToString());
                    else if (dt.Rows.Count > 0)
                        AlertError("Existen errores al validar el pedimento: " + Session["PEDIMENTOARMADO"].ToString());

                    ASPxPageControl1.ActiveTabIndex = 1;
                }
                else
                {
                    ASPxPageControl1.ActiveTabIndex = 0;
                    //AlertSuccess("Validación exitosa");
                }

                Session["Grid3"] = null;
                mensaje = string.Empty;
                Grid3.DataSource = Session["Grid3"] = dt;
                Grid3.DataBind();
                Grid3.Settings.VerticalScrollableHeight = 200;
                Grid3.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
            catch (Exception ex) 
            {
                AlertError(ex.Message);
            }
        }


        #region RadioButton

        //Metodo del radiobuton para seleccionar por fila en Grid
        protected void rbConsultar_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxRadioButton rb = sender as ASPxRadioButton;
                GridViewDataItemTemplateContainer container = rb.NamingContainer as GridViewDataItemTemplateContainer;

                rb.ClientInstanceName = String.Format("rbConsultar{0}", container.VisibleIndex);
                rb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ rbConsultarClick(s, e, {0}); }}", container.VisibleIndex);

                var lblPedimento = (ASPxLabel)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }


        //Evento del radio button en Grid
        protected void rbConsultar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }

                ASPxRadioButton rbBox = sender as ASPxRadioButton;

                if (rbBox.Checked)
                {
                    //Valor del campo ITEM_KEY del Grid
                    string key = ((GridViewDataItemTemplateContainer)rbBox.NamingContainer).KeyValue.ToString();

                    //Obtener la secuencia seleccioanda
                    string v_secuencia = string.Empty;


                    foreach (DataRow fila in ((DataTable)Session["Grid"]).Rows)
                    {
                        if (fila["PARTIDA_DETALLEKEY"].ToString().Trim() == key)
                        {
                            Session["Secuencia"] = v_secuencia = fila["Partida"].ToString();
                            break;
                        }
                    }


                    //Obtener datos en Grid2
                    string mensaje = string.Empty;
                    DataTable dt = new DataTable();

                    //Grid Descarga
                    dt = np.Consultar_NP_Detalle(v_secuencia, Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);

                    Grid2.DataSource = Session["Grid2"] = dt;
                    Grid2.DataBind();
                    Grid2.Settings.VerticalScrollableHeight = 200;
                    Grid2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                    //Selecccionar el primer registro del grid2
                    if (Session["Grid2"] != null)
                        Grid2.Selection.SelectRow(0);

                    var lblPedimento = (ASPxLabel)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                    lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();
                }

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                //excepcion.RegistrarExcepcion(idusuario, "Descargas-rbConsultar_CheckedChanged", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        #endregion

        #region RadioButton2

        //Metodo del radiobuton para seleccionar por fila en Grid2
        protected void rbConsultar2_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxRadioButton rb = sender as ASPxRadioButton;
                GridViewDataItemTemplateContainer container = rb.NamingContainer as GridViewDataItemTemplateContainer;

                rb.ClientInstanceName = String.Format("rbConsultar2{0}", container.VisibleIndex);
                rb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ rbConsultar2Click(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                //excepcion.RegistrarExcepcion(idusuario, "Descargas-rbConsultar_Init", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        //Evento del radio button en Grid2
        protected void rbConsultar2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxRadioButton rbBox = sender as ASPxRadioButton;
                //GridViewDataItemTemplateContainer container = rbBox.NamingContainer as GridViewDataItemTemplateContainer;
                //String valor;
                //valor = container.KeyValue.ToString();
                if (rbBox.Checked)
                {
                    //Valor del campo ITEM_KEY del Grid
                    string key = ((GridViewDataItemTemplateContainer)rbBox.NamingContainer).KeyValue.ToString();
                    Session["DescargaKey"] = key;

                    ////Obtener sku y cantidad del Grid
                    //string v_sku = string.Empty;
                    ////string v_cantidad = string.Empty;
                    //decimal d_cantidad = 1;

                    //foreach (DataRow fila in ((DataTable)Session["Grid"]).Rows)
                    //{
                    //    if (fila["ITEM_KEY"].ToString() == key)
                    //    {
                    //        v_sku = fila["SKU"].ToString();
                    //        //v_cantidad = fila["CANTIDAD DE MANIPULACION"].ToString();
                    //        break;
                    //    }
                    //}

                    //if (v_cantidad.Length > 0)
                    //    d_cantidad = decimal.Parse(v_cantidad);


                    ////Obtener datos en Grid2, Grid3 y Grid4
                    //string mensaje = string.Empty;
                    //DataTable dt = new DataTable();

                    ////Grid Descarga
                    //dt = declare.Traer_Descarga(Int64.Parse(key), Session["Cadena"].ToString(), ref mensaje);

                    //Grid2.DataSource = Session["Grid2"] = dt;
                    //Grid2.DataBind();
                    //Grid2.Settings.VerticalScrollableHeight = 190;
                    //Grid2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                }

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                //excepcion.RegistrarExcepcion(idusuario, "Descargas-rbConsultar_CheckedChanged", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        #endregion

        //Metodo que muestra el modal
        private void MostrarModal()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnModal').click(); </script> ", false);
        }

        //Agregar en Grid2
        protected void bbAgregar_OnClick(object sender, EventArgs e)
        {
            //Abre Modal
            MostrarModal();

            //Titulo del Modal
            ModalTitulo.InnerText = "Agregar";
            DataBind();

            //Limpiar variables
            txt_cove.Text = string.Empty;
            dateEdit_Fecha.Text = string.Empty;
            txt_NumParte.Text = string.Empty;
            se_Cantidad.Text = "0";
            txt_UMC.Text = string.Empty;
            se_ValorDolares.Text = "0";
            txt_Descripcion.Text = string.Empty;
            txt_ClienteProv.Text = string.Empty;
            txt_PO.Text = string.Empty;
        }

        //Editar en Grid2
        protected void bbEditar_OnClick(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            DataTable dt = new DataTable();
            int valida_select = 0;

            for (int i = 0; i < Grid2.VisibleRowCount; i++)
            {
                if (Grid2.Selection.IsRowSelected(i))
                {
                    //Abre Modal
                    MostrarModal();

                    //Titulo del Modal
                    ModalTitulo.InnerText = "Editar";
                    DataBind();

                    //Asignar valores
                    Session["key"] = Grid2.GetSelectedFieldValues("DETALLENPKEY")[0].ToString().Trim();
                    //txt_RFC.Text = Grid2.GetSelectedFieldValues("RFC")[0].ToString().Trim();
                    //txt_RFC.Text = Grid2.GetSelectedFieldValues("RFC")[0].ToString().Trim();
                    //txt_RFC.Text = Grid2.GetSelectedFieldValues("RFC")[0].ToString().Trim();
                    valida_select = 1;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar un registro para poder editar");
        }

        //Eliminar en Grid2
        protected void bbBorrar_OnClick(object sender, EventArgs e)
        {
            if (Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null || Session["Secuencia"] == null)
            {
                string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                Session["Tab"] = "Salir";
                Response.Write(alerta);
                return;
            }

            int valida_select = 0;

            for (int i = 0; i < Grid2.VisibleRowCount; i++)
            {
                if (Grid2.Selection.IsRowSelected(i))
                {
                    string mensaje = string.Empty;
                    DataTable dt = new DataTable();
                    Int64 key = Int64.Parse(Grid2.GetSelectedFieldValues("DETALLENPKEY")[0].ToString().Trim());

                    dt = np.Eliminar_Detalle_NP(key, Session["Secuencia"].ToString(), Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);

                    Grid2.DataSource = Session["Grid2"] = dt;
                    Grid2.DataBind();
                    Grid2.Settings.VerticalScrollableHeight = 150;
                    Grid2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                    //Selecccionar el primer registro del grid2
                    if (Session["Grid2"] != null)
                        Grid2.Selection.SelectRow(0);

                    AlertSuccess("El registro se eliminó con éxito.");
                    valida_select = 1;

                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar un registro para poder eliminar");
        }


        //Guardar Cambios de Grid2
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //Valida Sesiones
                if (ModalTitulo.InnerText.Contains("Agregar") && (Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null) || Session["Secuencia"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }

                if (ModalTitulo.InnerText.Contains("Editar") && (Session["key"] == null || Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null || Session["Secuencia"] == null))
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }
                

                //Valida campos en modal
                DateTime? dateFecha = string.IsNullOrEmpty(dateEdit_Fecha.Text) ? (DateTime?)null : DateTime.Parse(dateEdit_Fecha.Text);
                decimal cantidad = string.IsNullOrEmpty(se_Cantidad.Text.Trim()) ? 0 : decimal.Parse(se_Cantidad.Text.Trim());
                decimal dolar = string.IsNullOrEmpty(se_ValorDolares.Text.Trim()) ? 0 : decimal.Parse(se_ValorDolares.Text.Trim());
                string valida = string.Empty;

                if(string.IsNullOrEmpty(txt_NumParte.Text.Trim()))
                {
                    MostrarModal();
                    valida = "Debe agregar un número de parte para poder " + (ModalTitulo.InnerText.Contains("Agregar") ? "guardar" : "editar");
                    AlertError(valida);
                    return;
                }
                if (cantidad == 0)
                {
                    MostrarModal();
                    valida = "Debe agregar una cantidad comercial para poder " + (ModalTitulo.InnerText.Contains("Agregar") ? "guardar" : "editar");
                    AlertError(valida);
                    return;
                }
                if (string.IsNullOrEmpty(txt_UMC.Text.Trim()))
                {
                    MostrarModal();
                    valida = "Debe agregar un UMC para poder " + (ModalTitulo.InnerText.Contains("Agregar") ? "guardar" : "editar");
                    AlertError(valida);
                    return;
                }
                if (dolar == 0)
                {
                    MostrarModal();
                    valida = "Debe agregar un valor en doláres para poder " + (ModalTitulo.InnerText.Contains("Agregar") ? "guardar" : "editar");
                    AlertError(valida);
                    return;
                }
                if (string.IsNullOrEmpty(txt_ClienteProv.Text.Trim()))
                {
                    MostrarModal();
                    valida = "Debe agregar una clave cliente/proveedor para poder " + (ModalTitulo.InnerText.Contains("Agregar") ? "guardar" : "editar");
                    AlertError(valida);
                    return;
                }


                //Guardar
                string mensaje = "";
                DataTable dt = new DataTable();

                if (ModalTitulo.InnerText.Contains("Agregar"))
                    dt = np.Agregar_Detalle_NP(txt_cove.Text.Trim(), txt_Factura.Text.Trim(), dateFecha, txt_NumParte.Text.Trim(), cantidad, txt_UMC.Text.Trim(), txt_Descripcion.Text.Trim(), dolar,
                                               txt_ClienteProv.Text.Trim(), txt_PO.Text.Trim(), Session["Secuencia"].ToString(), Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);
                else if (ModalTitulo.InnerText.Contains("Editar"))
                    dt = np.Editar_Detalle_NP(Int64.Parse(Session["key"].ToString().Trim()), txt_cove.Text.Trim(), txt_Factura.Text.Trim(), dateFecha, txt_NumParte.Text.Trim(), cantidad, txt_UMC.Text.Trim(), txt_Descripcion.Text.Trim(),
                                              dolar, txt_ClienteProv.Text.Trim(), txt_PO.Text.Trim(), Session["Secuencia"].ToString(), Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);


                Grid2.DataSource = Session["Grid2"] = dt;
                Grid2.DataBind();
                Grid2.Settings.VerticalScrollableHeight = 150;
                Grid2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //Selecccionar el primer registro del grid2
                if (Session["Grid2"] != null)
                    Grid2.Selection.SelectRow(0);

                AlertSuccess("El registro se " + (ModalTitulo.InnerText.Contains("Editar") ? "actualizó" : "agregó") + " con éxito.");

            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }



    }
}