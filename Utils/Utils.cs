using IS220_PROJECT.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IS220_PROJECT.Utils
{
    public static class Utils
    {
        public static string ToVnd(this double donGia)
        {
            return donGia.ToString("#,##0") + " đ";
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static int PAGE_SIZE = 20;
        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }
        public static string formatVNString(string VNStr)
        {
            VNStr = VNStr.ToLower();
            VNStr = Regex.Replace(VNStr, @"[áàạảãâấầậẩẫăắằặẳẵ]", "a");
            VNStr = Regex.Replace(VNStr, @"[éèẹẻẽêếềệểễ]", "e");
            VNStr = Regex.Replace(VNStr, @"[óòọỏõôốồộổỗơớờợởỡ]", "o");
            VNStr = Regex.Replace(VNStr, @"[íìịỉĩ]", "i");
            VNStr = Regex.Replace(VNStr, @"[ýỳỵỉỹ]", "y");
            VNStr = Regex.Replace(VNStr, @"[úùụủũưứừựửữ]", "u");
            VNStr = Regex.Replace(VNStr, @"[đ]", "d");

            //2. Chỉ cho phép nhận:[0-9a-z-\s]
            VNStr = Regex.Replace(VNStr.Trim(), @"[^0-9a-z-\s]", "").Trim();
            //xử lý nhiều hơn 1 khoảng trắng --> 1 kt
            VNStr = Regex.Replace(VNStr.Trim(), @"\s+", "-");
            //thay khoảng trắng bằng -
            VNStr = Regex.Replace(VNStr, @"\s", "-");
            while (true)
            {
                if (VNStr.IndexOf("--") != -1)
                {
                    VNStr = VNStr.Replace("--", "-");
                }
                else
                {
                    break;
                }
            }
            return VNStr;
        }
        public static async Task<string> UploadFile(IFormFile file, string sDirectory, string newname = null)
        {
            try
            {
                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory);
                CreateIfMissing(path);
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory, newname);
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower())) /// Khác các file định nghĩa
                {
                    return null;
                }
                else
                {
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newname;
                }
            }
            catch
            {
                return null;
            }
        }

        public static JObject readApi(string url)
        {
            string text = File.ReadAllText(Path.GetFullPath(url));
            JObject json = JObject.Parse(text);
            return json;
        }
    }
}
