namespace AgileApp.Services.Chat
{
    public interface IChatService
    {
        public List<string> Load();

        public bool SendMessage(string message);
    }
}
