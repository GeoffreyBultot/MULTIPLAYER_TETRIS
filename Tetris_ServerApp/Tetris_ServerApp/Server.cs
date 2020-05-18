using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Tetris_ServerApp
{
    /* La classe AsyncServer et AsyncClient utilisent les méthodes asynchrones de la classe Socket. En effet, les méthodes
     * concernant la communication réseau (connexion, récéption, envoi, acceptation, etc...) peuvent prendre du temps, ou
     * même être totalement bloquantes car elles dépendent de ce qui se passe à l'autre bout de la communication. Afin
     * d'éviter que l'application se fige en attendant une réponse du réseau, on exécute ces fonctions dans un thread 
     * séparé. Comme on ne peut plus prévoir le moment où ces fonctions s'exécutent, on utilise le déclenchement d'events
     * pour en avertir les classes "abonnées".
     * En pratique on exécute une méthode "Begin..." de la classe Socket qui exécutera une méthode "callback" dans un nouveau
     * thread, dans laquelle on exécute la méthode, lente ou blocante, "End...".
     */
    public class Server
    {
        /* La classe AsyncServer a été créée pour simplifier la gestion des connexion grâce à la mise à disposition
         * d'events. Elle repose entièrement sur la classe Socket. Elle se limite essentiellement à l'écoute et 
         * l'acceptation des nouvelles connexions entrantes.
         * A titre d'exemple on déclare ses propres delegates. On aurait très bien pu se contenter des delegates existants
         * EventHandler<T> du namespace System.
         */

        Socket listener;
        IPEndPoint ep;
        public bool Running { get; private set; }
        public Socket listenSocket { get { return listener; } }

        public delegate void ClientAcceptedHandler(Client client);
        public delegate void ServerStatusHandler();

        public event ClientAcceptedHandler ClientAccepted;
        public event ServerStatusHandler ServerStarted;
        public event ServerStatusHandler ServerStopped;

        public Server(string address, int port)
        {
            /* La contruction d'un asyncServer consiste à définir son interface (ip et port) d'écoute.
             * Par défaut, le serveur n'est pas démarré. Il faut pour cela appeler la méthode Start.
             */
            ep = new IPEndPoint(IPAddress.Parse(address), port);
        }

        public void Start()
        {
            /* Démarrer un serveur consiste à mettre le Socket en mode écoute après l'avoir lié à l'interface
             * réseau désirée. On lance ensuite la méthode acceptClients qui qui tournera de manière indéfinie
             * jusqu'à ce que le serveur s'eteigne (listener.Close());
             */
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine(Running);
            listener.Bind(ep);
            listener.Listen(0);
            onServerStarted();
            acceptClients();
            Console.WriteLine(Running);
        }

        public void Stop()
        {
            /* On ferme le socket. Ceci aura pour effet de lever une exception sur la méthode EndAccept
             * et teminera ainsi la méthode asynchrone d'acceptation de nouveaux clients.
             */
            listener.Close();

        }

        private void acceptClients()
        {
            /* On utilise la méthode asynchrone BeginAccept de la classe Socket. Cette méthode exécute la 
             * méthode passée en paramètre, appelée callback, dans un nouveau thread (une sous-routine). Ceci
             * permet à l'application de 
             */
            listener.BeginAccept(acceptClientCallback, null);
        }

        private void acceptClientCallback(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = listener.EndAccept(ar);
                Client client = new Client(clientSocket);
                onClientAccepted(client);
                listener.BeginAccept(acceptClientCallback, null);
            }
            catch (Exception e)
            {
                /* Lorse que le serveur se coupe (listener.close() ou autres soucis...) la méthode EndAccept se
                 * termine prématurément et une exeption est levée. On considère alors que le serveur est éteint
                 * et on déclenche l'event correspondant via la méthode onServerStopped.
                 */
                onServerStopped();
            }
        }

        #region Raising event methods

        private void onClientAccepted(Client client)
        {
            if (ClientAccepted != null)
            {
                if (ClientAccepted.Target is System.Windows.Forms.Control)
                {
                    ((System.Windows.Forms.Control)ClientAccepted.Target).Invoke(ClientAccepted, client);
                }
                else
                {
                    ClientAccepted(client);
                }
            }
        }

        private void onServerStarted()
        {
            Running = true;
            if (ServerStarted != null)
            {
                ServerStarted();
            }
        }

        private void onServerStopped()
        {
            Running = false;
            if (ServerStopped != null)
            {
                if (ServerStopped.Target is System.Windows.Forms.Control)
                {
                    ((System.Windows.Forms.Control)ServerStopped.Target).Invoke(ServerStopped);
                }
                else
                {
                    ServerStopped();
                }
            }
        }
        #endregion
    }
}
