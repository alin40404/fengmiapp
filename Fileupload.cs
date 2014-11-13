using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.IO;
using fengmiapp.Models;
using System.Collections;
using System.Drawing.Imaging;

namespace fengmiapp
{
    public static class Fileupload
    {

        public static char DirSeparator = Path.DirectorySeparatorChar;
        public static string FilesPath = HttpContext.Current.Server.MapPath(string.Format("..{0}Content{0}UploadFiles{1}", DirSeparator, DirSeparator));

        public static string UploadFileOutUrl(HttpPostedFileBase file, string SavePath)
        {
            //设置上传路径
            //string SavePath = "";
            //设置返回上传路径
            //string uploadSaveUrl="";
            //读取文件名和扩展名，如123.jpg
            string fileName = Path.GetFileName(file.FileName);
            //读取文件扩展名，如jpg
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            //获取文件上传的大小
            int fileLength = file.ContentLength;
            //初始化返回图片上传的路径
            string uploadPath = "";
            //初始化错误信息
            string message = "";
            //初始化上传文件的最大值
            int maxLength = 1024;
            //设置扩展名
            ICollection ic = new string[] { ".gif", ".jpg", ".GIF", ".JPG",".png",".PNG" };
            ArrayList imgTypes = new ArrayList(ic);

            if (fileName == "")
            {
                return message = "The picture is unexist.";
            }
            if (!imgTypes.Contains(fileExtension))
            {

                return message = "Only accept .gif .jpg .png";
            }
            if (fileLength > maxLength * 1024)
            {
                return message = " The size is too big";
            }
            //
            try
            {
                string picPath = "~" + SavePath;
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmffff") + fileExtension;
                string date = DateTime.Now.ToString("yyyyMM");
                string newPicPath = System.Web.HttpContext.Current.Server.MapPath(picPath + date + "/");
                DirectoryInfo directoryInfo = new DirectoryInfo(newPicPath);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                file.SaveAs(newPicPath + newFileName);
                uploadPath = SavePath.Substring(1) + date + "/" + newFileName;
                //thumbPath = SavePath.Substring(1) + "Pictures_Small100" + date + "/" + newFileName;
                //refName = date + "/" + fileName;
                //GetThumbnailImage(picPath, date + "/", newFileName, width, height);
            }
            catch
            {
                return message = "Failure.";
            }

            return uploadPath;
        }

        public static string GetThumbnailImageOutUrl(HttpPostedFileBase file, string SavePath, int width, int height)
        {

            //读取文件名和扩展名，如123.jpg
            string fileName = Path.GetFileName(file.FileName);
            //读取文件扩展名，如jpg
            string fileExtension = Path.GetExtension(file.FileName);
            //获取文件上传的大小
            int fileLength = file.ContentLength;
            //初始化返回图片上传的路径
            string uploadPath = "";
            //初始化错误信息
            string message = "";
            //初始化上传文件的最大值
            int maxLength = 1024;
            //设置扩展名
            ICollection ic = new string[] { ".gif", ".jpg", ".GIF", ".JPG" };
            ArrayList imgTypes = new ArrayList(ic);

            if (fileName == "")
            {
                return message = "The picture is unexist.";
            }
            if (!imgTypes.Contains(fileExtension))
            {

                return message = "Only accept .gif .jpg";
            }
            if (fileLength > maxLength * 1024)
            {
                return message = " The size is too big";
            }
            //
            try
            {
                string picPath = "~" + SavePath;
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmffff") + fileExtension;
                string date = DateTime.Now.ToString("yyyyMM");
                string newPicPath = System.Web.HttpContext.Current.Server.MapPath(picPath + date + "/");
                //设置自定义图路径
                string strRelativeSmallPath = picPath + "PicSmall" + date + "/";
                string smallnewPicPath = System.Web.HttpContext.Current.Server.MapPath(strRelativeSmallPath + newFileName);

                string machpath = System.Web.HttpContext.Current.Server.MapPath(picPath + date + "/" + newFileName);
                DirectoryInfo directoryInfo = new DirectoryInfo(newPicPath);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                //保存原图
                file.SaveAs(newPicPath + newFileName);
                //创建自定义图目录
                string isDir = smallnewPicPath.Substring(0, smallnewPicPath.LastIndexOf('\\'));
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(isDir);
                if (!di.Exists)
                {
                    di.Create();
                }
                //设置图片大小
                //读取上传后的原图进行重新设置图片大小
                Bitmap img = new Bitmap(machpath);  //read picture to memory

                int h = img.Height;
                int w = img.Width;
                int ss, os;// source side and objective side
                double temp1, temp2;
                //compute the picture''s proportion
                temp1 = (h * 1.0D) / height;
                temp2 = (w * 1.0D) / width;
                if (temp1 < temp2)
                {
                    ss = w;
                    os = width;
                }
                else
                {
                    ss = h;
                    os = height;
                }

                double per = (os * 1.0D) / ss;
                if (per < 1.0D)
                {
                    h = (int)(h * per);
                    w = (int)(w * per);
                }
                // create the thumbnail image
                System.Drawing.Image.GetThumbnailImageAbort callBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallBack);
                System.Drawing.Image imag2 = img.GetThumbnailImage(w, h, callBack, IntPtr.Zero);
                Bitmap tempBitmap = new Bitmap(w, h);
                System.Drawing.Image tempImg = System.Drawing.Image.FromHbitmap(tempBitmap.GetHbitmap());
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(tempImg);
                g.Clear(Color.White);

                int x, y;

                x = (tempImg.Width - imag2.Width) / 2;
                y = (tempImg.Height - imag2.Height) / 2;

                g.DrawImage(imag2, x, y, imag2.Width, imag2.Height);
                //
                try
                {
                    if (img != null)
                        img.Dispose();
                    if (imag2 != null)
                        imag2.Dispose();
                    if (tempBitmap != null)
                        tempBitmap.Dispose();

                    string smallfileExtension = System.IO.Path.GetExtension(machpath).ToLower();
                    // 
                    switch (smallfileExtension)
                    {
                        case ".gif": tempImg.Save(smallnewPicPath, ImageFormat.Gif); break;
                        case ".jpg": tempImg.Save(smallnewPicPath, ImageFormat.Jpeg); break;
                        case ".bmp": tempImg.Save(smallnewPicPath, ImageFormat.Bmp); break;
                        case ".png": tempImg.Save(smallnewPicPath, ImageFormat.Png); break;
                    }
                }
                catch
                {
                    throw new Exception("failuer");
                }
                finally
                {
                    //
                    if (tempImg != null)
                        tempImg.Dispose();
                    if (g != null)
                        g.Dispose();
                }
                //把使用完的原图删除
                Fileupload.DeleteImg("/" + newPicPath + newFileName);

                uploadPath = SavePath.Substring(1) + "PicSmall" + date + "/" + newFileName;
                //thumbPath = SavePath.Substring(1) + "Pictures_Small100" + date + "/" + newFileName;
                //refName = date + "/" + fileName;
                //GetThumbnailImage(picPath, date + "/", newFileName, width, height);
            }
            catch
            {
                return message = "Failure.";
            }

            return uploadPath;
        }
        
        private static bool ThumbnailCallBack()
        {
            return true;
        }
        
        public static string UploadFile(HttpPostedFileBase file)
        {
            // Check if we have a file
            if (null == file) return "";
            // Make sure the file has content
            if (!(file.ContentLength > 0)) return "";
            string fileName = file.FileName;
            string fileExt = Path.GetExtension(file.FileName).ToLower();
            // Make sure we were able to determine a proper
            // extension
            if (null == fileExt) return "";
            // Check if the directory we are saving to exists
            if (!Directory.Exists(FilesPath))
            {
                // If it doesn't exist, create the directory
                Directory.CreateDirectory(FilesPath);
            }
            // Set our full path for saving
            string path = FilesPath + DirSeparator + fileName;
            // Save our file
            file.SaveAs(Path.GetFullPath(path));
            // Save our thumbnail as well
            //ResizeImage(file, 150, 100);
            // Return the filename
            return path;
        }
        
        public static bool UploadFile(HttpPostedFileBase file, out string uploadPath)
        {
            //初始化返回图片上传的路径
            uploadPath = string.Empty;
            bool isSuccess = false;

            //读取文件名和扩展名，如123.jpg
            string fileName = Path.GetFileName(file.FileName);
            //读取文件扩展名，如jpg
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            //获取文件上传的大小
            int fileLength = file.ContentLength;
             //初始化错误信息
            string message = "";
            //初始化上传文件的最大值
            int maxLength = 1024;
            //设置扩展名
            ICollection ic = new string[] { ".gif", ".jpg", ".GIF", ".JPG", ".png", ".PNG" };
            ArrayList imgTypes = new ArrayList(ic);

            if (fileName == "")
            {
                 message = "The picture is unexist.";
            }
            if (!imgTypes.Contains(fileExtension))
            {
                message = "Only accept .gif .jpg .png";
            }
            if (fileLength > maxLength * 1024)
            {
                 message = " The size is too big";
            }

            try
            {
                string SavePath = "Content/UploadFiles/";
                string picPath = "../" + SavePath;
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmffff") + fileExtension;
                string date = DateTime.Now.ToString("yyyyMM");
                string newPicPath = System.Web.HttpContext.Current.Server.MapPath(picPath + date + "/");
                DirectoryInfo directoryInfo = new DirectoryInfo(newPicPath);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                //ResizeImage(file, 150, 100);
                file.SaveAs(newPicPath + newFileName);
                uploadPath = SavePath + date + "/" + newFileName;
                isSuccess = true;
                //thumbPath = SavePath.Substring(1) + "Pictures_Small100" + date + "/" + newFileName;
                //refName = date + "/" + fileName;
                //GetThumbnailImage(picPath, date + "/", newFileName, width, height);
            }
            catch
            {
                message = "Failure.";
            }
            
            return isSuccess;
        }


        public static void DeleteImg(string path)
        {
            RemoveFile("/" + path);
        }

        public static void DeleteImgandsmall(string path, string smallpath)
        {
            RemoveFile("/" + path);
            RemoveFile("/" + smallpath);
        }

        public static void DeleteFile(string fileName)
        {
            // Don't do anything if there is no name
            if (fileName.Length == 0) return;
            // Set our full path for deleting
            string path = FilesPath + DirSeparator + fileName;
            string thumbPath = FilesPath + DirSeparator +
            "Thumbnails" + DirSeparator + fileName;
            RemoveFile(path);
            RemoveFile(thumbPath);
        }
        private static void RemoveFile(string path)
        {
            // Check if our file exists
            string delpath = System.Web.HttpContext.Current.Server.MapPath(path);

            if (File.Exists(delpath))
            {
                // Delete our file
                File.Delete(delpath);
            }
        }
        public static string ResizeImage(HttpPostedFileBase file, int width, int height)
        {
            //string fileExtension = Path.GetExtension(file.FileName);//获取文件扩展名
            //string newFileName = DateTime.Now.ToString("yyyyMMddHHmmffff") + fileExtension;//重置上传的文件名

            string thumbnailDirectory =
            String.Format(@"{0}{1}{2}", FilesPath,
            DirSeparator, "Thumbnails");
            // Check if the directory we are saving to exists
            if (!Directory.Exists(thumbnailDirectory))
            {
                // If it doesn't exist, create the directory
                Directory.CreateDirectory(thumbnailDirectory);
            }
            // Final path we will save our thumbnail
            string imagePath =
            String.Format(@"{0}{1}{2}", thumbnailDirectory,
            DirSeparator, file.FileName);

            string refilepath = "System/Uploads/HomeImg/Thumbnails/" + file.FileName;
            // Create a stream to save the file to when we're
            // done resizing
            FileStream stream = new FileStream(Path.GetFullPath(
            imagePath), FileMode.OpenOrCreate);
            // Convert our uploaded file to an image
            Image OrigImage = Image.FromStream(file.InputStream);
            // Create a new bitmap with the size of our
            // thumbnail
            Bitmap TempBitmap = new Bitmap(width, height);
            // Create a new image that contains quality
            // information
            Graphics NewImage = Graphics.FromImage(TempBitmap);
            NewImage.CompositingQuality =
            CompositingQuality.HighQuality;
            NewImage.SmoothingMode =
            SmoothingMode.HighQuality;
            NewImage.InterpolationMode =
            InterpolationMode.HighQualityBicubic;
            // Create a rectangle and draw the image
            Rectangle imageRectangle = new Rectangle(0, 0,
            width, height);
            NewImage.DrawImage(OrigImage, imageRectangle);
            // Save the final file
            TempBitmap.Save(stream, OrigImage.RawFormat);
            // Clean up the resources
            NewImage.Dispose();
            TempBitmap.Dispose();
            OrigImage.Dispose();
            stream.Close();
            stream.Dispose();
            return refilepath;
        }

        public static string ResizeImagePDC(HttpPostedFileBase file, int width, int height)
        {
            //string fileExtension = Path.GetExtension(file.FileName);//获取文件扩展名
            //string newFileName = DateTime.Now.ToString("yyyyMMddHHmmffff") + fileExtension;//重置上传的文件名


            string thumbnailDirectory =
            String.Format(@"{0}{1}{2}", FilesPath,
            DirSeparator, "PDC");
            // Check if the directory we are saving to exists
            //if (!Directory.Exists(thumbnailDirectory))
            //{
            //    // If it doesn't exist, create the directory
            //    Directory.CreateDirectory(thumbnailDirectory);
            //}
            // Final path we will save our thumbnail
            string imagePath =
            String.Format(@"{0}{1}{2}", thumbnailDirectory,
            DirSeparator, file.FileName);

            string refilepath = "Production/Uploads/HomeImg/PDC/" + file.FileName;
            // Create a stream to save the file to when we're
            // done resizing
            FileStream stream = new FileStream(Path.GetFullPath(
            imagePath), FileMode.OpenOrCreate);
            // Convert our uploaded file to an image
            Image OrigImage = Image.FromStream(file.InputStream);
            // Create a new bitmap with the size of our
            // thumbnail
            Bitmap TempBitmap = new Bitmap(width, height);
            // Create a new image that contains quality
            // information
            Graphics NewImage = Graphics.FromImage(TempBitmap);
            NewImage.CompositingQuality =
            CompositingQuality.HighQuality;
            NewImage.SmoothingMode =
            SmoothingMode.HighQuality;
            NewImage.InterpolationMode =
            InterpolationMode.HighQualityBicubic;
            // Create a rectangle and draw the image
            Rectangle imageRectangle = new Rectangle(0, 0,
            width, height);
            NewImage.DrawImage(OrigImage, imageRectangle);
            // Save the final file
            TempBitmap.Save(stream, OrigImage.RawFormat);
            // Clean up the resources
            NewImage.Dispose();
            TempBitmap.Dispose();
            OrigImage.Dispose();
            stream.Close();
            stream.Dispose();
            return refilepath;
        }
    }
}