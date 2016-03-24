<%@ WebHandler Language="C#" Class="Download" %>

using System;
using System.Web;
using System.Security;
using System.IO;
using System.Configuration;

public class Download : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string filename = context.Request.QueryString["file"];
        string year = context.Request.QueryString["year"];
        //get filespath
        if (context.User.Identity.IsAuthenticated)
        {
            string username = context.User.Identity.Name;
            UserInfos userinfos = UserInfos.getUserInfos(username);
            string FilesPath = ConfigurationManager.AppSettings["filepath"] + userinfos.AsocId +"\\"+year+"\\";
            if (!string.IsNullOrEmpty(filename) && File.Exists(FilesPath + filename))
            {
                context.Response.ContentType = "application/octet-stream";
                context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", filename));
                context.Response.WriteFile(FilesPath + filename);
            }
        }
        else
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Invalid filename");
        }

        
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}