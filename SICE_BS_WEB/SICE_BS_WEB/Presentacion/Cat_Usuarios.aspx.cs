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
    public partial class Cat_Usuarios : System.Web.UI.Page
    {
        Inicio inicio = new Inicio();
        Usuarios usuarios = new Usuarios();
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

                    CargarCmbPerfil();
                    //CargarCmbTipo();
                    CargarGrid2();

                    //Agregamos los permisos
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
                TxtContrasena.Attributes.Add("value", TxtContrasena.Text);

                
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Cat_Usuario-Page_Load", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
                //Response.Redirect("Login.aspx");
            }
        }

        protected void CmbPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string mensaje = string.Empty;
            ASPxComboBox combobox = sender as ASPxComboBox;
            if (combobox.SelectedIndex.Equals(-1))
                return;
            //Marca los permisos del perfil seleccionado
            int idPerfil = Convert.ToInt32(combobox.Value);
            if (idPerfil > 0)
            {
                dt = perfiles.PerfilModuloAccionConsultar(idPerfil, lblCadena.Text, ref mensaje);
                if (!mensaje.Equals(string.Empty))
                    AlertError(mensaje);
                else
                {
                    CargarPermisos(dt);
                    MostrarModal();
                }
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] == null)
            {
                string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                Session["Tab"] = "Salir";
                Response.Write(alerta);
                return;
            }

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

            string mensaje = string.Empty;
            if (ValidarCampos(ref mensaje))
            {
                string idUsuario = string.Empty;
                if (HfIdUsuario.Value.Length > 0)
                    usuarios.UsuarioEditar(
                        Convert.ToInt32(Session["IdUsuario"])
                        , Convert.ToInt32(HfIdUsuario.Value)
                        , TxtUsuario.Text.Trim()
                        , TxtNombre.Text.Trim()
                        , TxtContrasena.Text.Trim()
                        , Convert.ToInt32(CmbPerfil.Value)
                        , 1 //Convert.ToInt32(CmbTipo.Value)
                        , lblCadena.Text
                        , ref mensaje);
                else
                    usuarios.UsuarioAgregar(
                        Convert.ToInt32(Session["IdUsuario"])
                        , ref idUsuario
                        , TxtUsuario.Text.Trim()
                        , TxtNombre.Text.Trim()
                        , TxtContrasena.Text.Trim()
                        , Convert.ToInt32(CmbPerfil.Value)
                        , 1 //Convert.ToInt32(CmbTipo.Value)
                        , lblCadena.Text
                        , ref mensaje);

                if (mensaje.Equals(ControlExcepciones.Ok))
                {
                    //Agregar permisos
                    string mensaje2 = string.Empty;
                    for (int i = 0; i < Grid2.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Consultar"], "chkConsultar") as ASPxCheckBox;
                        ASPxCheckBox chkAgregar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Agregar"], "chkAgregar") as ASPxCheckBox;
                        ASPxCheckBox chkEditar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Editar"], "chkEditar") as ASPxCheckBox;
                        ASPxCheckBox chkEliminar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Eliminar"], "chkEliminar") as ASPxCheckBox;
                        ASPxCheckBox chkExportar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Exportar"], "chkExportar") as ASPxCheckBox;

                        perfiles.PermisoAgregar(
                           Convert.ToInt32(Session["IdUsuario"])
                           , Convert.ToInt32(idUsuario.Equals(string.Empty) ? HfIdUsuario.Value : idUsuario)
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
                    AlertSuccess("El usuario se " + (HfIdUsuario.Value.Length > 0 ? "actualizó" : "agregó") + ".");
                    Page_Init(null, null);
                    HfIdUsuario.Value = string.Empty;
                }
                else if (mensaje.Equals(ControlExcepciones.Existe))
                    AlertError("El usuario \"" + TxtUsuario.Text + "\" ya existe.");
                else
                    AlertError(mensaje);
            }
            else
                // AlertError("Datos incompletos. " + mensaje);
                AlertError(mensaje);

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        private void CargarCmbPerfil()
        {
            DataTable dt = new DataTable();
            string mensaje = string.Empty;
            dt = perfiles.PerfilConsultar(lblCadena.Text, ref mensaje);
            if (!mensaje.Equals(string.Empty))
                AlertError(mensaje);

            CmbPerfil.DataSource = dt == null ? new DataTable() : dt;
            CmbPerfil.DataBind();
        }

        private void CargarCmbTipo()
        {
            DataTable dt = new DataTable();
            string mensaje = string.Empty;
            dt = usuarios.TipoUsuarioConsultar(lblCadena.Text, ref mensaje);
            if (!mensaje.Equals(string.Empty))
                AlertError(mensaje);

            //CmbTipo.DataSource = dt == null ? new DataTable() : dt;
            //CmbTipo.DataBind();
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
                            if (chkConsultar != null) chkConsultar.Visible = Convert.ToBoolean(rowAccion["Consultar"]);
                            if (chkAgregar != null) chkAgregar.Visible = Convert.ToBoolean(rowAccion["Agregar"]);
                            if (chkEditar != null) chkEditar.Visible = Convert.ToBoolean(rowAccion["Editar"]);
                            if (chkEliminar != null) chkEliminar.Visible = Convert.ToBoolean(rowAccion["Eliminar"]);
                            if (chkExportar != null) chkExportar.Visible = Convert.ToBoolean(rowAccion["Exportar"]);
                        }
                    }
        }

        private void CargarPermisosUsuario()
        {
            //Marca los permisos del usuario seleccionado
            DataTable dt = new DataTable();
            string mensaje = string.Empty;
            int idUsuario = Convert.ToInt32(HfIdUsuario.Value);
            dt = inicio.DatosPermisos(idUsuario, lblCadena.Text, ref mensaje);
            if (!mensaje.Equals(string.Empty))
                AlertError(mensaje);
            else
                CargarPermisos(dt);
        }
        
        private void TituloPanel(string descripcion)
        {
            h1_titulo.InnerText = tituloPanel = tituloPagina + descripcion;
        }
        
        private bool ValidarCampos(ref string mensaje)
        {
            string html = string.Empty;
            //html = "<p align=\"left\">";
            if (TxtNombre.Text.Trim().Equals(string.Empty))
                mensaje += " -Campo Nombre obligatorio.";


            if (TxtUsuario.Text.Trim().Length < 6)
                mensaje += " - Campo Usuario mínimo 6 carácteres. ";

            if ((TxtContrasena.Text.Trim().Length > 0 && TxtContrasena.Text.Trim().Length < 6)
                || (TxtContrasena.Text.Trim().Length == 0 && HfIdUsuario.Value.Equals(string.Empty)))
                mensaje += " - Campo Contraseña mínimo 6 carácteres. ";

            if (TxtContrasena.Text.Trim().Length == 0)
                mensaje += " - Campo Contraseña obligatotrio. ";

            //if (CmbTipo.SelectedIndex < 0)
            //    mensaje += " - Seleccione un Tipo de usuario. ";

            if (CmbPerfil.SelectedIndex < 0)
                mensaje += " - Seleccione un Perfil. ";

            if (mensaje.Length > 0)
            {
                //mensaje = html + mensaje + "</p>";
                return false;
            }
            else
                return true;
        }

        private void CargarPermisos(DataTable permisos)
        {
            bool marcarTodo = true;
            LimpiarPermisos(true);
            //Marca los permisos
            if (permisos != null)
            {
                HabiliarPermisos(true);
                
                foreach (DataRow renglonPermiso in permisos.Rows)
                {
                    for (int i = 0; i < Grid2.VisibleRowCount; i++)
                    {
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
            //LblMarcarTodo.Style["background-color"] = Session["MenuBackColor"].ToString();
            //LblMarcarTodo.Style["color"] = Session["MenuFontColor"].ToString();
        }

        private void HabiliarPermisos(bool habilitar)
        {
            //Habilita / Deshabilita los permisos
            for (int i = 0; i < Grid2.VisibleRowCount; i++)
            {
                ASPxCheckBox chkTodo = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Todo"], "chkTodo") as ASPxCheckBox;
                ASPxCheckBox chkConsultar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Consultar"], "chkConsultar") as ASPxCheckBox;
                ASPxCheckBox chkAgregar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Agregar"], "chkAgregar") as ASPxCheckBox;
                ASPxCheckBox chkEditar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Editar"], "chkEditar") as ASPxCheckBox;
                ASPxCheckBox chkEliminar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Eliminar"], "chkEliminar") as ASPxCheckBox;
                ASPxCheckBox chkExportar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Exportar"], "chkExportar") as ASPxCheckBox;
                chkTodo.Enabled = chkConsultar.Enabled = chkAgregar.Enabled = chkEditar.Enabled = chkEliminar.Enabled = chkExportar.Enabled = habilitar;
            }
        }

        private void LimpiarCampos()
        {
            HfIdUsuario.Value = TxtNombre.Text = TxtUsuario.Text = string.Empty;
            TxtContrasena.Attributes.Add("value", string.Empty);
            //CmbTipo.SelectedIndex = -1;
            CmbPerfil.SelectedIndex = -1;
            LimpiarPermisos(false);
        }

        private void LimpiarPermisos(bool enable)
        {
            //Desmarca todos los permisos            
            for (int i = 0; i < Grid2.VisibleRowCount; i++)
            {
                //Session["GET"] = Grid.GetDataRow(i).ToString().Trim();
                //Grid2.GetRowValues()
                ASPxCheckBox chkTodo = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Todo"], "chkTodo") as ASPxCheckBox;
                ASPxCheckBox chkConsultar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Consultar"], "chkConsultar") as ASPxCheckBox;
                ASPxCheckBox chkAgregar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Agregar"], "chkAgregar") as ASPxCheckBox;
                ASPxCheckBox chkEditar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Editar"], "chkEditar") as ASPxCheckBox;
                ASPxCheckBox chkEliminar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Eliminar"], "chkEliminar") as ASPxCheckBox;
                ASPxCheckBox chkExportar = Grid2.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2.Columns["Exportar"], "chkExportar") as ASPxCheckBox;

                if (chkTodo != null) { chkTodo.Checked = false; chkTodo.Enabled = enable; }
                if (chkConsultar != null) { chkConsultar.Checked = false; chkConsultar.Enabled = enable; }
                if (chkAgregar != null) { chkAgregar.Checked = false; chkAgregar.Enabled = enable; }
                if (chkEditar != null) { chkEditar.Checked = false; chkEditar.Enabled = enable; }
                if (chkEliminar != null) { chkEliminar.Checked = false; chkEliminar.Enabled = enable; }
                if (chkExportar != null) { chkExportar.Checked = false; chkExportar.Enabled = enable; }

            }
        }

        private void OcultarModal()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "OcultarModal", "<script> $('#Modal1').modal('hide');</script>", false);
        }

        private void MostrarModal()
        {
            if (HfIdUsuario.Value.Length > 0)
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnEditarH').click(); </script> ", false);
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnNuevoH').click(); </script> ", false);

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

            //Trae informción de la bd de usuarios dados de alta
            DataTable dt = new DataTable();
            string mensaje = "";
            Session["Grid"] = dt = usuarios.UsuarioConsultar(Session["Cadena"].ToString(), ref mensaje);

            if (!mensaje.Equals(string.Empty))
            {
                AlertError(mensaje);
                return;
            }

            Grid.DataSource = dt == null ? new DataTable() : dt;
            Grid.DataBind();
            Grid.SettingsPager.PageSize = GridPageSize;
        }

        //no se usa
        private void HabilitarEditarEliminar(string value)
        {
            //Valida que si hay una fila seleccionada los botones de Editar, Eliminar son habilitados
            //si no hay fila seleccionada estos botones son deshabilitados

            if (value.Equals("1"))
            {
                int x = 0;
                for (int i = 0; i < Grid.VisibleRowCount; i++)
                {
                    if (Grid.Selection.IsRowSelected(i))
                    {
                        x = 1;
                        break;
                    }
                }

                var lnk_Editar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Editar");
                var lnk_Eliminar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Eliminar");
                lnk_Editar.Enabled = x == 0 ? false : true;
                lnk_Eliminar.Enabled = x == 0 ? false : true;

                //GridViewToolbarItem toolbarItem = Grid.Toolbars[0].Items.FindByName("Links");
                //var lnk_Editar1 = (LinkButton)toolbarItem.FindControl("lkb_Editar");
                //lnk_Editar1.Enabled = x == 0 ? false : true;
            }
            else
            {
                var lnk_Editar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Editar");
                var lnk_Eliminar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Eliminar");
                lnk_Editar.Enabled = false;
                lnk_Eliminar.Enabled = false;

            }

        }

        //no se usa - se crean controles en el header de las columnas del grid
        public class HeaderCaptionTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                GridViewHeaderTemplateContainer c = container
                    as GridViewHeaderTemplateContainer;
                ASPxLabel lbl = new ASPxLabel();
                lbl.Text = c.Column.Caption;

                container.Controls.Add(lbl);
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

        protected void lkb_Nuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            LimpiarPermisos(true);
            MostrarModal();
            Modal1Titulo.InnerText = "Nuevo Usuario";

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Editar_Click(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] == null)
            {
                string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                Session["Tab"] = "Salir";
                Response.Write(alerta);
                return;
            }

            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                if (Grid.Selection.IsRowSelected(i))
                {
                    LimpiarCampos();
                    HfIdUsuario.Value = Session["IdUsuario"].ToString().Trim();
                    TxtUsuario.Text = Session["Usuario"].ToString().Trim();
                    TxtNombre.Text = Session["Nombre"].ToString().Trim();
                    //CmbTipo.SelectedIndex = int.Parse(Session["IdTipoUsuario"].ToString().Trim());
                    CmbPerfil.Value =  Session["IdPerfil"].ToString().Trim();
                    CargarPermisosUsuario();
                    if (!HfIdUsuario.Value.Equals("1"))
                    {
                        MostrarModal();
                        Modal1Titulo.InnerText = "Editar Usuario";
                        break;
                    }
                    else
                        AlertError("Este usuario no es editable");
                    break;
                }
            }

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Eliminar_Click(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] == null)
            {
                string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                Session["Tab"] = "Salir";
                Response.Write(alerta);
                return;
            }

            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                if (Grid.Selection.IsRowSelected(i))
                {
                    string mensaje = string.Empty;
                    HfIdUsuario.Value = Session["IdUsuario"].ToString().Trim();

                    if (!HfIdUsuario.Value.Equals("1"))
                    {
                        usuarios.UsuarioEliminar(Convert.ToInt32(Session["IdUsuario"]), Convert.ToInt32(HfIdUsuario.Value), lblCadena.Text, ref mensaje);
                        if (mensaje.Equals(ControlExcepciones.Ok))
                        {
                            Page_Init(null, null);
                            AlertSuccess("El usuario se eliminó.");
                        }
                        else
                            AlertError(mensaje);
                        break;
                    }
                    else
                        AlertError("Este usuario no se puede eliminar");
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
                Exporter.WriteXlsToResponse("Catálogo de Usuarios", new XlsExportOptionsEx() { SheetName = "Catálogo de Usuarios" });
            }
            else
                AlertError("No hay información por exportar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void Grid_SelectionChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                if (Grid.Selection.IsRowSelected(i))
                {
                    Session["IdUsuario"] = Grid.GetSelectedFieldValues("IdUsuario")[0].ToString().Trim();
                    Session["Usuario"] = Grid.GetSelectedFieldValues("Usuario")[0].ToString().Trim();
                    Session["Nombre"] = Grid.GetSelectedFieldValues("Nombre")[0].ToString().Trim();
                    Session["IdTipoUsuario"] = Grid.GetSelectedFieldValues("IdTipoUsuario")[0].ToString().Trim();
                    Session["IdPerfil"] = Grid.GetSelectedFieldValues("IdPerfil")[0].ToString().Trim();
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
                if (Session["MenuBackColor"] != null)
                {
                    //Header
                    //divHeader.Style.Value = "background-color: " + Session["MenuBackColor"].ToString() + "; ";
                    //divHeader.Attributes["style"] = "color: " + Session["MenuFontColor"].ToString() + "; ";

                    //divHeader.Style["background-color"] = Session["MenuBackColor"].ToString();
                    //divHeader.Style["color"] = Session["MenuFontColor"].ToString();

                    ////Botones
                    //lkb_Nuevo.BackColor = System.Drawing.ColorTranslator.FromHtml(Session["MenuBackColor"].ToString());
                    //lkb_Editar.BackColor = System.Drawing.ColorTranslator.FromHtml(Session["MenuBackColor"].ToString());
                    //lkb_Eliminar.BackColor = System.Drawing.ColorTranslator.FromHtml(Session["MenuBackColor"].ToString());
                    //lkb_Excel.BackColor = System.Drawing.ColorTranslator.FromHtml(Session["MenuBackColor"].ToString());
                    //lkb_LimpiarFiltros.BackColor = System.Drawing.ColorTranslator.FromHtml(Session["MenuBackColor"].ToString());


                    //lkb_Nuevo.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["MenuFontColor"].ToString());
                    //lkb_Editar.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["MenuFontColor"].ToString());
                    //lkb_Eliminar.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["MenuFontColor"].ToString());
                    //lkb_Excel.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["MenuFontColor"].ToString());
                    //lkb_LimpiarFiltros.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["MenuFontColor"].ToString());

                    
                    Grid.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml(Session["ButtonBackColor"].ToString());
                    Grid.Styles.SelectedRow.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["ButtonFontColor"].ToString());

                    //LblMarcarTodo.Style["background-color"] = Session["MenuBackColor"].ToString();
                    //LblMarcarTodo.Style["color"] = Session["MenuFontColor"].ToString();

                    //btnGuardar.Style["background-color"] = Session["MenuBackColor"].ToString();
                    //btnGuardar.Style["color"] = Session["MenuFontColor"].ToString();

                }
            }
            catch (Exception ex)
            { }
        }

    }
}