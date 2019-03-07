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
    public partial class Cat_Perfiles : System.Web.UI.Page
    {
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
        static List<int> listItemSelected = new List<int>();
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
                        DataTable dt = ((DataTable)Session["Permisos"]).Select("Archivo =  '" + nombreArchivo + "'").CopyToDataTable();
                        tituloPagina = dt.Rows[0]["NombreModulo"].ToString();
                        permisoConsultar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Consultar"].ToString()));
                        if (!permisoConsultar)
                            Response.Redirect("Default.aspx");
                        permisoAgregar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Agregar"].ToString()));
                        permisoEditar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Editar"].ToString()));
                        permisoEliminar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Eliminar"].ToString()));
                        permisoExportar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Exportar"].ToString()));
                        Page.Title = tituloPagina;

                        if (Session["Grid"] != null)
                            Grid.Selection.SelectRow(0);
                    }
                    TituloPanel(string.Empty);

                    CargarGrid2();

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
                excepcion.RegistrarExcepcion(idusuario, "Cat_Perfiles-Page_Load", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
                //Response.Redirect("Login.aspx");
            }
        }
        
        private void TituloPanel(string descripcion)
        {
            h1_titulo.InnerText = tituloPanel = tituloPagina + descripcion;
        }

        private void OcultarModal()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "OcultarModal", "<script> $('#Modal1').modal('hide');</script>", false);
        }

        private void MostrarModal()
        {
            if (HfIdPerfil.Value.Length > 0)
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnEditarH').click(); </script> ", false);
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnNuevoH').click(); </script> ", false);

        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

            if (ValidarCampos())
            {
                string mensaje = string.Empty;
                string idPerfil = string.Empty;
                if (HfIdPerfil.Value.Length > 0)
                    perfiles.PerfilEditar(
                        Convert.ToInt32(Session["IdUsuario"])
                        , Convert.ToInt32(HfIdPerfil.Value)
                        , TxtNombre.Text.Trim()
                        , TxtClave.Text.Trim()
                        , lblCadena.Text
                        , ref mensaje);
                else
                    perfiles.PerfilAgregar(
                        Convert.ToInt32(Session["IdUsuario"])
                        , ref idPerfil
                        , TxtNombre.Text.Trim()
                        , TxtClave.Text.Trim()
                        , lblCadena.Text
                        , ref mensaje);

                if (mensaje.Equals(ControlExcepciones.Ok))
                {
                    //Agregar perfil modulo accion
                    string mensaje2 = string.Empty;
                    for (int i = 0; i < Grid2.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Consultar"], "chkConsultar") as ASPxCheckBox;
                        ASPxCheckBox chkAgregar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Agregar"], "chkAgregar") as ASPxCheckBox;
                        ASPxCheckBox chkEditar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Editar"], "chkEditar") as ASPxCheckBox;
                        ASPxCheckBox chkEliminar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Eliminar"], "chkEliminar") as ASPxCheckBox;
                        ASPxCheckBox chkExportar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Exportar"], "chkExportar") as ASPxCheckBox;

                        perfiles.PerfilModuloAccionAgregar(
                           Convert.ToInt32(Session["IdUsuario"])
                           , Convert.ToInt32(idPerfil.Equals(string.Empty) ? HfIdPerfil.Value : idPerfil)
                           , Convert.ToInt32(Grid2.GetRowValues(i, "IdModulo").ToString())
                           , chkAgregar.Checked
                           , chkConsultar.Checked
                           , chkEditar.Checked
                           , chkEliminar.Checked
                           , chkExportar.Checked
                           , lblCadena.Text
                           , ref mensaje2);
                        if (!mensaje2.Equals(string.Empty))
                        {
                            AlertError(mensaje2);
                            return;
                        }
                    }
                    
                    OcultarModal();
                    AlertSuccess("El perfil se " + (HfIdPerfil.Value.Length > 0 ? "actualizó" : "agregó") + ".");
                    Page_Init(null, null);
                    HfIdPerfil.Value = string.Empty;
                }
                else if (mensaje.Equals(ControlExcepciones.Existe))
                    AlertError("El perfil \"" + TxtNombre.Text + "\" ya existe.");
                else
                    AlertError(mensaje);

                //Actualiza los permisos de los botones en grid
                PermisosUsuario();
            }
            else
                AlertError("Datos incompletos.");


        }

        private void CargarGrid2()
        {
            DataTable dt = new DataTable();
            string mensaje = string.Empty;
            dt = perfiles.ModuloConsultar(lblCadena.Text, ref mensaje);

            if (!mensaje.Equals(string.Empty))
                AlertError(mensaje);

            Grid2.DataSource = dt == null ? new DataTable() : dt;
            Grid2.DataBind();
            CargarAcciones();
        }

        private void CargarAcciones()
        {
            DataTable dt = new DataTable();
            string mensaje = string.Empty;
            dt = perfiles.ModuloAccionConsultar(lblCadena.Text, ref mensaje);

            if (!mensaje.Equals(string.Empty))
                AlertError(mensaje);
            else
                foreach (DataRow rowAccion in dt.Rows)
                    for (int i = 0; i < Grid2.VisibleRowCount; i++)
                    {
                        if (rowAccion["IdModulo"].ToString().Equals(Grid2.GetRowValues(i, "IdModulo").ToString()))
                        {
                            ASPxCheckBox chkConsultar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Consultar"], "chkConsultar") as ASPxCheckBox;
                            ASPxCheckBox chkAgregar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Agregar"], "chkAgregar") as ASPxCheckBox;
                            ASPxCheckBox chkEditar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Editar"], "chkEditar") as ASPxCheckBox;
                            ASPxCheckBox chkEliminar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Eliminar"], "chkEliminar") as ASPxCheckBox;
                            ASPxCheckBox chkExportar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Exportar"], "chkExportar") as ASPxCheckBox;
                            chkConsultar.Visible = Convert.ToBoolean(rowAccion["Consultar"]);
                            chkAgregar.Visible = Convert.ToBoolean(rowAccion["Agregar"]);
                            chkEditar.Visible = Convert.ToBoolean(rowAccion["Editar"]);
                            chkEliminar.Visible = Convert.ToBoolean(rowAccion["Eliminar"]);
                            chkExportar.Visible = Convert.ToBoolean(rowAccion["Exportar"]);
                        }
                    }
        }

        private bool ValidarCampos()
        {
            bool flg = true;

            if (TxtNombre.Text.Trim().Equals(string.Empty))
                flg = false;

            return flg;
        }

        private void LimpiarCampos()
        {
            HfIdPerfil.Value = TxtNombre.Text = TxtClave.Text = string.Empty;
            LimpiarPermisos();
        }

        private void LimpiarPermisos()
        {
            //Desmarca todos los permisos            
            for (int i = 0; i < Grid2.VisibleRowCount; i++)
            {
                ASPxCheckBox chkTodo = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Todo"], "chkTodo") as ASPxCheckBox;
                ASPxCheckBox chkConsultar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Consultar"], "chkConsultar") as ASPxCheckBox;
                ASPxCheckBox chkAgregar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Agregar"], "chkAgregar") as ASPxCheckBox;
                ASPxCheckBox chkEditar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Editar"], "chkEditar") as ASPxCheckBox;
                ASPxCheckBox chkEliminar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Eliminar"], "chkEliminar") as ASPxCheckBox;
                ASPxCheckBox chkExportar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Exportar"], "chkExportar") as ASPxCheckBox;
                chkTodo.Checked = chkConsultar.Checked = chkAgregar.Checked = chkEditar.Checked = chkEliminar.Checked = chkExportar.Checked = false;
                chkTodo.Enabled = chkConsultar.Enabled = chkAgregar.Enabled = chkEditar.Enabled = chkEliminar.Enabled = chkExportar.Enabled = true;
            }
        }

        private void CargarPermisos(DataTable permisos)
        {
            bool marcarTodo = true;
            //Marca los permisos
            if (permisos != null)
            {
                foreach (DataRow renglonPermiso in permisos.Rows)
                    for (int i = 0; i < Grid2.VisibleRowCount; i++)
                        if (renglonPermiso["IdModulo"].ToString().Equals(Grid2.GetRowValues(i, "IdModulo").ToString()))
                        {
                            ASPxCheckBox chkTodo = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Todo"], "chkTodo") as ASPxCheckBox;
                            ASPxCheckBox chkConsultar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Consultar"], "chkConsultar") as ASPxCheckBox;
                            ASPxCheckBox chkAgregar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Agregar"], "chkAgregar") as ASPxCheckBox;
                            ASPxCheckBox chkEditar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Editar"], "chkEditar") as ASPxCheckBox;
                            ASPxCheckBox chkEliminar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Eliminar"], "chkEliminar") as ASPxCheckBox;
                            ASPxCheckBox chkExportar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Exportar"], "chkExportar") as ASPxCheckBox;

                            //Marca los permisos visibles (habilitados por Acción)
                            chkAgregar.Checked = chkAgregar.Visible ? Convert.ToBoolean(renglonPermiso["Agregar"]) : false;
                            chkConsultar.Checked = chkConsultar.Visible ? Convert.ToBoolean(renglonPermiso["Consultar"]) : false;
                            chkEditar.Checked = chkEditar.Visible ? Convert.ToBoolean(renglonPermiso["Editar"]) : false;
                            chkEliminar.Checked = chkEliminar.Visible ? Convert.ToBoolean(renglonPermiso["Eliminar"]) : false;
                            chkExportar.Checked = chkExportar.Visible ? Convert.ToBoolean(renglonPermiso["Exportar"]) : false;

                            //Si todos los permisos visibles de un modulo están marcados, marca el checkbox Todos
                            chkTodo.Checked =
                                (chkAgregar.Visible ? chkAgregar.Checked : true) &&
                                (chkConsultar.Visible ? chkConsultar.Checked : true) &&
                                (chkEditar.Visible ? chkEditar.Checked : true) &&
                                (chkEliminar.Visible ? chkEliminar.Checked : true) &&
                                (chkExportar.Visible ? chkExportar.Checked : true);

                            break;
                        }
            }
            //Valida checkbox Todo esta activo en todos los permisos
            for (int i = 0; i < Grid2.VisibleRowCount; i++)
                if (marcarTodo)
                {
                    ASPxCheckBox chkTodo = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Todo"], "chkTodo") as ASPxCheckBox;
                    marcarTodo = chkTodo.Checked;
                }


            CbxMarcarTodo.Checked = marcarTodo; //Asigna valor a checkbox Marcar Todo de la validación checkbox Todo en todos los permisos
            CbxMarcarTodo.Enabled = true;
            IcoMarcarTodo.Attributes["class"] = CbxMarcarTodo.Checked ? "glyphicon glyphicon-check" : "glyphicon glyphicon-unchecked";
            LblMarcarTodo.Attributes["class"] = "btn btn-primary btn-sm"; //Habilita checkbox Marcar Todo
        }

        //Metodos del GRID
        //
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
            //Valida la sesión de cadena
            if (Session["Cadena"] == null) //No hay sesión
            {
                Response.Redirect("Login.aspx?redirect=" + Request.Path.Substring(Request.Path.LastIndexOf("/") + 1));
                return;
            }

            //Trae informción de la bd de perfiles dados de alta
            DataTable dt = new DataTable();
            string mensaje = "";
            Session["Grid"] = dt = perfiles.PerfilConsultar(Session["Cadena"].ToString(), ref mensaje);

            if (!mensaje.Equals(string.Empty))
            {
                AlertError(mensaje);
                return;
            }

            Grid.DataSource = dt == null ? new DataTable() : dt;
            Grid.DataBind();
            Grid.SettingsPager.PageSize = GridPageSize;
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

        protected void lkb_Nuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            LimpiarPermisos();
            MostrarModal();
            Modal1Titulo.InnerText = "Nuevo Perfil";

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Editar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            DataTable dt = new DataTable();
            LimpiarCampos();            

            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                if (Grid.Selection.IsRowSelected(i))
                {
                    LimpiarCampos();
                    HfIdPerfil.Value = Session["IdPerfil"].ToString().Trim();
                    TxtNombre.Text = Session["Nombre"].ToString().Trim();
                    TxtClave.Text = Session["Clave"].ToString().Trim();
                    dt = perfiles.PerfilModuloAccionConsultar(Convert.ToInt32(HfIdPerfil.Value), lblCadena.Text, ref mensaje);

                    if (!mensaje.Equals(string.Empty))
                        AlertError(mensaje);
                    else
                    {
                        //Marca los permisos del perfil
                        CargarPermisos(dt);
                        Modal1Titulo.InnerText = "Editar Perfil";
                    }
                }
            }
            MostrarModal();

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Eliminar_Click(object sender, EventArgs e)
        {           
            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                if (Grid.Selection.IsRowSelected(i))
                {
                    string mensaje = string.Empty;
                    HfIdPerfil.Value = Session["IdPerfil"].ToString().Trim();

                    perfiles.PerfilEliminar(Convert.ToInt32(Session["IdUsuario"]), Convert.ToInt32(HfIdPerfil.Value), lblCadena.Text, ref mensaje);
                    if (mensaje.Equals(ControlExcepciones.Ok))
                    {
                        Page_Init(null, null);
                        AlertSuccess("El perfil se eliminó.");
                    }
                    else if(mensaje.Equals(ControlExcepciones.Existe))
                    {
                        AlertError("No se puede eliminar un perfil asignado a un usuario.");
                    }
                    else
                        AlertError(mensaje);
                    break;
                }
            }

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            if (Grid.VisibleRowCount > 0)
            {
                Exporter.WriteXlsToResponse("Catálogo de Perfiles", new XlsExportOptionsEx() { SheetName = "Catálogo de Perfiles" });
            }
            else
                AlertError("No hay información por exportar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Actualizar_Click(object sender, EventArgs e)
        {
            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void Grid_SelectionChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                if (Grid.Selection.IsRowSelected(i))
                {
                    Session["IdPerfil"] = Grid.GetSelectedFieldValues("IdPerfil")[0].ToString().Trim();
                    Session["Nombre"] = Grid.GetSelectedFieldValues("Nombre")[0].ToString().Trim();
                    Session["Clave"] = Grid.GetSelectedFieldValues("Clave")[0].ToString().Trim();
                    break;
                }
            }
        }

        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModalError.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
        }

        //Metodo que muestra ventana de Satisfactorio
        public void AlertSuccess(string mensaje)
        {
            pModalSucces.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnSuccess').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnSuccess').click(); </script> ", false);
        }

        //Metodo que muestra ventana de pregunta
        public void AlertQuestion(string mensaje, string btnUniqueId)
        {
            pModalQuestion.InnerText = mensaje;
            //btnConfirm es el id del botón que se va a presionar si el usuario da clic al botón Aceptar de la alerta de confirmación
            //string href = btnConfirm.Replace("_", "$");
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "",
                "<script> document.getElementById('btnOk').setAttribute('onclick', \"javascript:__doPostBack('" + btnUniqueId + "', '')\"); document.getElementById('btnQuestion').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestion').click(); </script> ", false);
        }


        #region Métodos que llaman a funciones en java

        protected void chkTodo_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chb = sender as ASPxCheckBox;
            GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

            chb.ClientInstanceName = String.Format("chkTodo{0}", container.VisibleIndex);
            chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ todos1Click(s, e, {0}); }}", container.VisibleIndex);
        }

        protected void chkConsultar_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chb = sender as ASPxCheckBox;
            GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

            chb.ClientInstanceName = String.Format("chkConsultar{0}", container.VisibleIndex);
            chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ permisos1Click(s, e, {0}); }}", container.VisibleIndex);
        }

        protected void chkAgregar_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chb = sender as ASPxCheckBox;
            GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

            chb.ClientInstanceName = String.Format("chkAgregar{0}", container.VisibleIndex);
            chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ permisos1Click(s, e, {0}); }}", container.VisibleIndex);
        }

        protected void chkEditar_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chb = sender as ASPxCheckBox;
            GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

            chb.ClientInstanceName = String.Format("chkEditar{0}", container.VisibleIndex);
            chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ permisos1Click(s, e, {0}); }}", container.VisibleIndex);
        }

        protected void chkEliminar_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chb = sender as ASPxCheckBox;
            GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

            chb.ClientInstanceName = String.Format("chkEliminar{0}", container.VisibleIndex);
            chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ permisos1Click(s, e, {0}); }}", container.VisibleIndex);
        }

        protected void chkExportar_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chb = sender as ASPxCheckBox;
            GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

            chb.ClientInstanceName = String.Format("chkExportar{0}", container.VisibleIndex);
            chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ permisos1Click(s, e, {0}); }}", container.VisibleIndex);
        }

        #endregion

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