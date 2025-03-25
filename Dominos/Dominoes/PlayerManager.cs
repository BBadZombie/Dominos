using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        public PlayerManager(Queue<Domino> DominoeSet)
        {
            SetHands(DominoeSet);
            PrintPlayerList();
        }

        // initializes players and player hands
        public void SetHands(Queue<Domino> DominoeSet)
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
                // while the player hand does not have 7 Dominoes
                while (playerList[i].Hand.Count < 7)
                {
                    playerList[i].Hand.Add(DominoeSet.Dequeue());
                }
            }
        }

        public void PrintPlayerList()
        {
            // for each player
            for (int i = 0; i < playerList.Count;i++)
            {
                Debug.Print("\nPlayer " + (i + 1) + ":", Debug.Level.Medium);

                // print player hand
                for (int j = 0; j < playerList[i].Hand.Count; j++)
                {
                    Debug.Print(" - " + playerList[i].Hand[j], Debug.Level.Medium);
                }
            }
        }
    }
}
