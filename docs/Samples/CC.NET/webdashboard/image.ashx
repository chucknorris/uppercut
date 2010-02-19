<%@ webhandler language="C#" class="ImageHandler" %>
using System;
using System.Configuration;
using System.Data; 
using System.Data.SqlClient; 
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Caching;
 
 
public class ImageHandler : IHttpHandler
{
  public bool IsReusable  { get { return true; }  }
 
  private string _imagePath = @"{0}\b{1}-r{2}\build_artifacts\ndepend\";
 
  /// <summary>
  /// Processes the request.
  /// </summary>
  /// <param name="ctx">CTX.</param>
  public void ProcessRequest(HttpContext ctx)
  {
    try
    {
      string name = ctx.Request.QueryString["name"];
      string project = ctx.Request.QueryString["project"];
      string label = ctx.Request.QueryString["label"];
      string revision = ctx.Request.QueryString["revision"];
      string cacheKey = string.Format("{0}|{1}|{2}|{3}", name, project, label, revision);
 
      if ( name != null && name.Length > 0)
      {
        Byte[] imageBytes = null;
 
        // Check if the cache contains the image.
        object cachedImageBytes = ctx.Cache.Get(cacheKey);
 
        // Use cache if possible...
        if ( cachedImageBytes != null )
        {
          imageBytes = cachedImageBytes as byte [];
        }
        else // Get the image from the project/build directory.
        {   
          // Determine the base path from config file if provided.
          string imagePath = ConfigurationSettings.AppSettings["imagePath"];
          if ( imagePath == null || imagePath.Length == 0 )
          {
            // If not provided, default to:
            imagePath = _imagePath;
          }
 
          // Replace tokens in the provided path with the project and label.
          imagePath = string.Format(imagePath, project, label, revision);
 
          // If the path is relative, combine with the current base directory.
          if ( !Path.IsPathRooted(imagePath) )
          {
            imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);          
          }
 
 
          try
          { // Get the image stream from the provided path.
            using ( FileStream fs = new FileStream(Path.Combine(imagePath, name), FileMode.Open, FileAccess.Read) )
            {
              using(Image inputImage = Image.FromStream(fs))
              {
                using(Image outputImage = new Bitmap(inputImage))
                { 
                  using(MemoryStream stream = new MemoryStream())
                  {
                    outputImage.Save(stream, ImageFormat.Jpeg);
                    imageBytes = stream.GetBuffer();
                  }
 
                  ctx.Cache.Add(cacheKey, imageBytes, null,
                    DateTime.MaxValue, new TimeSpan(2, 0, 0),
                    CacheItemPriority.Normal, null);   
                }
              }
            }
          }
          catch ( Exception )
          {
            throw;
          }
        }
 
        ctx.Response.Cache.SetCacheability(HttpCacheability.Public);
        ctx.Response.ContentType = "image/jpg";
        ctx.Response.BufferOutput = false;
        ctx.Response.OutputStream.Write(imageBytes, 0, imageBytes.Length);
      }  
    }
    catch ( Exception )
    {
      throw;
    }
    finally
    {
      ctx.Response.End();    
    }
  }
}
