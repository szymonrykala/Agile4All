namespace AgileApp.Repository.Chat
{
    public class ChatRepository : IChatRepository
    {
        private readonly AgileDbContext _dbContext;

        public ChatRepository(AgileDbContext agileDbContext)
        {
            _dbContext = agileDbContext;
        }

        public string GetMessage()
        {
            throw new NotImplementedException();
        }

        public List<string> Load()
        {
            var response = new List<string>();

            var messages = _dbContext.Messages.Skip(Math.Max(0, _dbContext.Messages.Count() - 30)).Take(30);
            if (messages.Count() > 0)
                foreach (var message in messages)
                {
                    response.Add(message.Json_Text);
                }

            return response;
        }

        public bool SendMessage(string message)
        {
            _dbContext.Messages.Add(new Models.MessageDb { Json_Text = message });
            try
            {
                return _dbContext.SaveChanges() == 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
