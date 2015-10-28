using Watson.Commands;
using Watson.Commands.Actors;

namespace Watson.Message
{
    class MarkovListener : MessageListener
    {
        public void Handle(IncomingMessage message)
        {
            if (message.IsDestChannel())
            {
                MarkovDatabaseAdapter.MarkovLearn(message.Message);
                if (CommandManager.RANDOM.Next() * 100 <= Markov.REPLY_RATE)
                {
                    string markov = MarkovDatabaseAdapter.MarkovGenerate();
                    if (markov != null)
                    {
                        //message.SendChat(markov);
                    }
                }
            }
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            return true;
        }

        public MarkovListener()
        {

            if (!MarkovDatabaseAdapter.Setup())
            {
                // oh no
            }
        }
    }
}
