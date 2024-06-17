using static System.Net.Mime.MediaTypeNames;
using System.IO.Compression;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace MultipleAreas_BlazorTemplate.Utilities
{
    public static class Helper
    {
        // 1. Validar una dirección de correo electrónico
        public static bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // 2. Generar una cadena aleatoria
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // 3. Calcular la edad a partir de una fecha de nacimiento
        public static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        // 4. Convertir una cadena de base64 a imagen
        //public static Image Base64ToImage(string base64String)
        //{
        //    var imageBytes = Convert.FromBase64String(base64String);
        //    using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        //    {
        //        ms.Write(imageBytes, 0, imageBytes.Length);
        //        return Image.FromStream(ms, true);
        //    }
        //}

        // 5. Comprimir una cadena usando GZip
        public static byte[] CompressString(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            using (var ms = new MemoryStream())
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzip.Write(bytes, 0, bytes.Length);
                }
                return ms.ToArray();
            }
        }

        // 6. Descomprimir una cadena usando GZip
        public static string DecompressString(byte[] compressedText)
        {
            using (var ms = new MemoryStream(compressedText))
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(gzip))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        // 7. Convertir un objeto a JSON
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }

        // 8. Convertir JSON a un objeto
        public static T FromJson<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException("Input JSON string cannot be null or empty.", nameof(json));
            }

            var result = JsonConvert.DeserializeObject<T>(json);

            if (result == null)
            {
                throw new InvalidOperationException("Deserialization returned null. Ensure the JSON string is correctly formatted and matches the target type.");
            }

            return result;
        }

        // 9. Guardar un archivo de texto
        public static void SaveToFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        // 10. Leer un archivo de texto
        public static string ReadFromFile(string path)
        {
            return File.ReadAllText(path);
        }
    }

}
