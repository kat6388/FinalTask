using System.IO;

namespace FinalTask
{
    class StringUtils
    {
        public static string GenerateUniqueSubject()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            var subject = path.Substring(0, 4);
            return subject;
        }
        public string subjectToSend = GenerateUniqueSubject();
    }
}
