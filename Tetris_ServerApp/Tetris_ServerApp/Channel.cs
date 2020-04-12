using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_ServerApp
{

    class Channel
    {
        public int code;
        private int nPlayer;
        private const int maxPlayer = 2;
        List<Client> remoteClients = new List<Client>();
        public Channel()
        { 
        }
        public Channel(int code)
        {
            this.code = code;

        }

        public bool AddPlayer(Client player)
        {
            if (nPlayer <= maxPlayer)
            {
                remoteClients.Add(player);
                remoteClients[remoteClients.IndexOf(player)].DataReceived += RemoteClient_DataReceived;
                nPlayer++;
                return true;
            }
            else
            {
                return false;
            }
            
        }

        #region Raising event methods

        /* Quelque soit l'event déclenché, le principe reste le même. On vérifie dans un premier temps s'il y a 
         * des abonnés à l'event (!=null). Les objets graphiques (Controls) n'acceptent en général pas d'être
         * modifiés par un thread autre que celui sur lequel ils ont été créés (cfr.Thread-Safety). Or dans notre
         * cas, à l'exception de ServerStarted, les events sont déclenchés dans une fonction callback, exécutée
         * par un autre thread. On ne peut donc pas exécuter directement les fonctions liées aux events si elles 
         * impliquent des objets graphiques. Pour le savoir on vérifie si l'objet qui détient la méthode associée
         * à l'event (Target) est de type Control (tous les objets graphiques héritent de la classe Control). Si 
         * c'est le cas on demande au thread gérant ce control d'exécuter la méthode associée à l'event (Invoke).
         */

        private void RemoteClient_DataReceived(Client client, object data)
        {
            /* Lorse que le serveur reçoit de données, celui doit les transmettre au prochain client. L'ordre des clients dans la liste
             * remoteClients correspond à l'ordre par lesquelles doivent passer chaque forme. On pourrait donc tout simplement envoyer
             * les données au client ayant l'indice du client d'ou provient les données (client source) + 1. Ceci pose problème lorse 
             * que le client source est le dernier de la liste. Dans ce cas il faut transmettre les données au premier client de la liste
             * (la forme vient de faire le tour du monde...), sinon une erreur "index out of bounds" est levée.
             * Une manière élégante de calculer cet indice consiste à prendre comme indice le reste de la division entière de l'indice du 
             * client source +1 par le nombre de clients connectés.
             */
            //int nextClientIndex = (remoteClients.IndexOf(client) + 1) % remoteClients.Count;
            if (data is String)
            {
                Console.WriteLine(data);
                if ((String)data == "giveMeCode")
                {
                    int chanel;
                    Random random = new Random();
                    chanel = random.Next(1, 9999);

                    remoteClients[remoteClients.IndexOf(client)].Send(chanel.ToString());
                }
            }
            else if (data is byte[])
            {
            }
            else
            {
                int nextClientIndex = (remoteClients.IndexOf(client) + 1) % remoteClients.Count;
                remoteClients[nextClientIndex].Send(data);

                //remoteClients[remoteClients.IndexOf(client)].Send(data);
                //TODO: envoyer sur le bon channel
            }
            //monitorServerMessages.AddMessage("data sent from " + client.ClientSocket.RemoteEndPoint + " to " + remoteClients[nextClientIndex].ClientSocket.RemoteEndPoint);
        }
        #endregion

    }
}