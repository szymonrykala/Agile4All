namespace AgileApp.Models.Files
{
    public class UploadFileRequest
    {
        public int? ProjectId { get; set; }

        public int? TaskId { get; set; }

        public IFormFile FileData { get; set; }

        public int UserId { get; set; }
    }
}
