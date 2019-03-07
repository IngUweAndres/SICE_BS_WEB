using System.IO;
using System.Linq;
using System.Web;
using System;
using System.IO.Compression;

namespace SICE_BS_WEB.Negocios
{
    public class FileDownloadHandlerMV : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
         
            var archivo = context.Server.MapPath("~/Presentacion/Temp/archivo_mv.zip");
            string name = context.User.Identity.Name.ToString();

            // create a new archive
            ZipFile.CreateFromDirectory(context.Server.MapPath("~/Presentacion/Temp/MV"), archivo);
            //ZipFile.ExtractToDirectory(archivo, temp);

            context.Response.ContentType = "application/zip";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=archivo_mv.zip");
            context.Response.TransmitFile(archivo);         

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}