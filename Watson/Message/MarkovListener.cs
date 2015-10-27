using Watson.Commands;
using Watson.Commands.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson.Message
{
    class MarkovListener : MessageListener
    {

        public void Handle(IncomingMessage message)
        {
            if (message.IsDestChannel())
            {
                MarkovDatabaseAdapter.MarkovLearn(message.GetMessage());
                if (CommandManager.RANDOM.Next() * 100 <= Markov.REPLY_RATE)
                {
                    string markov = MarkovDatabaseAdapter.MarkovGenerate();
                    if (markov != null)
                    {
                        // send markov here
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
            if(!MarkovDatabaseAdapter.Setup())
            {
                // oh no
            }
        }
    }

}
