<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;

public class Handler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        String callback = context.Request.QueryString["CKEditorFuncNum"].ToString();
        //context.Response.Write("<script>alert('jjj')</script>");
        //context.Response.End(); 
        ///'遍历File表单元素
        HttpFileCollection files = HttpContext.Current.Request.Files;
        for (int iFile = 0; iFile < files.Count; iFile++)
        {
            //    ///'检查文件扩展名字
            HttpPostedFile postedFile = files[iFile];
            //HttpPostedFile postedFile = files[0];
            string fileName;   //, fileExtension
            fileName = System.IO.Path.GetFileName(postedFile.FileName);
            //context.Response.Write("<script>alert('" + fileName + "')</script>");
            //context.Response.End(); 
            string fileContentType = postedFile.ContentType.ToString();
            if (fileContentType == "image/gif" ||
                fileContentType == "image/png" || fileContentType == "image/x-png" || fileContentType == "image/jpeg"
                || fileContentType == "image/pjpeg")
            {
                if (postedFile.ContentLength <= 2097152)
                {
                    string filepath = postedFile.FileName;      //得到的是文件的完整路径,包括文件名，如：C:\Documents and Settings\Administrator\My Documents\My Pictures\20022775_m.jpg
                    //string filepath = FileUpload1.FileName;               //得到上传的文件名20022775_m.jpg

                    string serverpath = context.Server.MapPath(@"~/Images/bbs/") + fileName;//取得文件在服务器上保存的位置C:\Inetpub\wwwroot\WebSite1\images\20022775_m.jpg
                    //context.Response.Write( serverpath );
                    //context.Response.End(); 
                    postedFile.SaveAs(serverpath);//上传图片到服务器指定地址

                    string host = "http://" + context.Request.Url.Host + ":" + context.Request.Url.Port;
                    string imageurl = host + "/Images/bbs/" + fileName;//我是将测试时的本地地址+放置图像的文件夹+图片名称作为返回的URL
                    //context.Response.Write(imageurl);
                    //context.Response.End(); 
                    // 返回"图像"选项卡并显示图片
                    context.Response.Write("<script type=\"text/javascript\">");
                    context.Response.Write("window.parent.CKEDITOR.tools.callFunction(" + callback
                           + ",'" + imageurl + "','')");
                    context.Response.Write("</script>");  
                }
                else
                {
                    context.Response.Write("<script>alert('이미지파일 사이즈가 2M 이하여야 합니다！')</script>");
                }
            }
            else
            {
                context.Response.Write("<script>alert('GIF、JPG、PNG 격식의 파일만 업로드 가능합니다！')</script>");
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    
   

}