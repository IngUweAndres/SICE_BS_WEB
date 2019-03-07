using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SICE_BS_WEB.Negocios;
using System.Configuration;


namespace SICE_BS_WEB.Presentacion
{
    public partial class AcercaDe : System.Web.UI.Page
    {
        ControlExcepciones excepcion = new ControlExcepciones();

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
                Response.Redirect("Login.aspx");
            }

        }


        //Descarga del archivo
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            try
            {
                string FilePath = string.Empty;
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                FilePath = HttpContext.Current.Request.Url.AbsoluteUri.Replace("AcercaDe.aspx", "Manual.pdf");                

                Response.Write("<script>window.open('" + FilePath + "','_blank') </script>");
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {                
                AlertError("Error al descargar el archivo pdf : " + ex.Message);
            }
        }


        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModalError.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
        }
    }
}