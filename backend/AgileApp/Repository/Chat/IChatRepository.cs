namespace AgileApp.Repository.Chat
{
    public interface IChatRepository
    {
        public List<string> Load();

        public bool SendMessage(string message);

        public string GetMessage();
    }
}
