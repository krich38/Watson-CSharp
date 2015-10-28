using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Watson.Message;
using Watson.Message.Handler;

namespace Watson
{
    class Protocol
    {

        public static List<MessageListener> LISTENERS = new List<MessageListener>(new MessageListener[] {
            new ConnectedHandler(),
            new PingHandler(),
            new CommandListener(),
            new MarkovListener(),
            new ProtocolHandler(),
            new KickHandler(),
            new LoginListener()
        });



        public static Stream Secure(IRCServer server, Stream stream)
        {
            RemoteCertificateValidationCallback certValidation = delegate { return true; };

            RemoteCertificateValidationCallback certValidationWithIrcAsSender = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return certValidation(server, certificate, chain, sslPolicyErrors);
            };
            LocalCertificateSelectionCallback selectionCallback = delegate (object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
            {
                if (localCertificates == null || localCertificates.Count == 0)
                {
                    return null;
                }
                return localCertificates[0];
            };
            SslStream sslStream = new SslStream(stream, false, certValidationWithIrcAsSender, selectionCallback);
            sslStream.AuthenticateAsClient(server.IP);
            return sslStream;
        }
    }
}
