namespace InternshipPlatform.Application.Dtos.File
{
    public class FileDownloadResult
    {
        public Stream Stream { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }
    }
}
