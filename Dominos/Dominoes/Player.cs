using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    /**
     * Purpose: Represent a player
     * Authors: Anthony Lopez
     * Date: 12.16.24
     * Modifications: 
     * Notes: 
     */

    internal class Player
    {
        // variable declaration
        List<Domino> playerHand;

        // properties
        public List<Domino> PlayerHand
        { 
            get { return playerHand; }
        }


        // constructor for objects of class Player
        public Player()
        {
            playerHand = new List<Domino>();
        }
        
        // returns cow if this player has it, returns null elsewise
        public Domino HasCow()
        {
            foreach (Domino domino in playerHand)
            {
                if (domino.Top == 6 && domino.Bottom == 6)
                    return domino;
            }

            return null;
        }

        // returns a domino and removes it from this players hand
        public Domino PlayDomino(Domino domino)
        {
            Domino domino1 = domino;
            playerHand.Remove(domino1);

            return domino1;
        }

        public override string ToString()
        {
            string playerInfo = string.Empty;

            foreach (Domino domino in playerHand)
            {
                playerInfo += domino.ToString() + ", ";
            }

            return playerInfo;
        }
    }
}
