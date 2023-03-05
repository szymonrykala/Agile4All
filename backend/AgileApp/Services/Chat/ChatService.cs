using AgileApp.Repository.Chat;

namespace AgileApp.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public List<string> Load() => _chatRepository.Load();

        public bool SendMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                return _chatRepository.SendMessage(message);
            }

            return false;
        }
    }
}
