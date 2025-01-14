using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    /**
     * Purpose: Manage players
     * Authors: Anthony Lopez
     * Date: 12.16.24
     * Modifications: 
     * Notes: 
     */

    internal class PlayerManager
    {
        // variable declaration
        List<Player> playerList;

        // properties
        public List<Player> PlayerList
        { 
            get { return playerList; }
        }

        // constructor for objects of class PlayerManager
        public PlayerManager(Queue<Domino> dominoSet)
        {
            SetPlayerHands(dominoSet);
            PrintPlayerList();
        }

        // initializes players and player hands
        public void SetPlayerHands(Queue<Domino> dominoSet)
        {
            playerList = new List<Player>();

            // add 4 players to the player list
            for (int i = 0; i < 4; i++)
            {
                playerList.Add(new Player());
            }

            // for each domino in given domino set, add to player hand
            for (int i = 0; i < playerList.Count; i++)
            {
                // while the player hand does not have 7 dominoes
                while (playerList[i].PlayerHand.Count < 7)
                {
                    playerList[i].PlayerHand.Add(dominoSet.Dequeue());
                }
            }
        }

        public void PrintPlayerList()
        {
            // for each player
            for (int i = 0; i < playerList.Count;i++)
            {
                System.Diagnostics.Debug.WriteLine("\nPlayer #{0}", i + 1);

                // print player hand
                for (int j = 0; j < playerList[i].PlayerHand.Count; j++)
                {
                    System.Diagnostics.Debug.WriteLine(" - {0}", playerList[i].PlayerHand[j]);

                }
            }
        }
    }
}
