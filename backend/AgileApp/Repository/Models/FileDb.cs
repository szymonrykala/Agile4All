namespace AgileApp.Repository.Models
{
    public class FileDb
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public DateTime Modification_Date { get; set; }

        public int User_Id { get; set; }

        public int Project_Id { get; set; }

        public int Task_Id { get; set; }
    }
}
