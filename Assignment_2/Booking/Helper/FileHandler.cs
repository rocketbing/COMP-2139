namespace Booking.Helper;

public class FileHandler
{
    public static string UploadFile(IFormFile file, string uploadsFolder, string folderpath)
    {
        string filePath = string.Empty;
        string imagePath = string.Empty;
        string datatime = DateTime.Now.ToString("MMddyyyyHHmmss");
        if (file != null)
        {
            bool exists = System.IO.Directory.Exists(Path.Combine(uploadsFolder));

            if (!exists)
                System.IO.Directory.CreateDirectory(Path.Combine(uploadsFolder));


            filePath = Path.Combine(uploadsFolder, Path.GetFileNameWithoutExtension(file.FileName) + datatime + Path.GetExtension(file.FileName));
            if (!System.IO.File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                //file.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            imagePath = "/" + folderpath + "/" + Path.GetFileNameWithoutExtension(file.FileName) + datatime + Path.GetExtension(file.FileName);
        }
        return imagePath;
    }
}
