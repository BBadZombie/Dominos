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
     * Date: 1.9.25
     * Modifications: 
     *  - Maybe add a property for score instead of a method?
     * Notes: 
     */

    internal class Player
    {
        // variable declaration
        List<Domino> hand;

        // properties
        public List<Domino> Hand
        { 
            get { return hand; }
        }


        // constructor for objects of class Player
        public Player()
        {
            hand = new List<Domino>();
        }
        
        // returns cow if this player has it, returns null elsewise
        public Domino HasCow()
        {
            foreach (Domino domino in hand)
            {
                if (domino.IsCow())
                    return domino;
            }

            return null;
        }

        // returns a domino and removes it from this players hand
        public Domino PlayDomino(Domino domino)
        {
            Domino domino1 = domino;
            hand.Remove(domino1);

            return domino1;
        }

        public int TotalScore()
        {
            int score = 0;

            foreach (Domino domino in hand)
            {
                score += domino.Total;
            }

            return score;
        }

        public override string ToString()
        {
            string playerInfo = string.Empty;

            foreach (Domino domino in hand)
            {
                playerInfo += domino.ToString() + ", ";
            }

            return playerInfo;
        }
    }
}
