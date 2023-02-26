namespace StockControlProject.WebUI.Models
{
    public class Upload
    {
        public static string ImageUpload(List<IFormFile> files,IWebHostEnvironment env, out bool result)
        {
            result= false;

            var uploads = Path.Combine(env.WebRootPath, "Uploads");
            foreach (var file in files)
            {
                if (file.ContentType.Contains("image"))
                {

                    // Content Type
                    // image/jpeg
                    // image/tiff
                    // image/png

                    if (file.Length <= 4194304) // 4 MB
                    {
                        string uniqueName = $"{ Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{file.ContentType.Split('/')[1]}"; // Split sonrası array oluşur. Oluşan array => {"image","jpeg"} gibidir. 1.indexteki bizim için uzantıdır.


                        //~/Uploads/d65h_45g6.jpg gibi
                        var filePath =Path.Combine(uploads, uniqueName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            result = true;
                            return filePath.Substring(filePath.IndexOf("\\Uploads\\"));
                        }
                    }
                    else
                    {
                        return "Size can not be more than 4 MB.";
                    }
                }
                else
                {
                    return "Please choose a file that is image format.";
                }
            }
            return "File is not choosen.";
        }
    }
}
