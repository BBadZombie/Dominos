using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.DirectoryServices.ActiveDirectory;

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

        /// <summary>
        /// Draws hand of the given player index
        /// </summary>
        public void DrawHand(SpriteBatch spriteBatch, GameTime gameTime, int playerIndex, int selectedDominoIndex)
        {
            int x = 380;
            int y = 450;

            // draw given player hand
            for (int i = 0; i < hand.Count; i++)
            {
                x += hand[i].Texture.Width;

                if (x > 950)
                {
                    x = 380;
                    y += hand[i].Texture.Height + 5;
                }

                if (i == selectedDominoIndex)
                    hand[i].DrawWithSelector(spriteBatch, x, y, 0f, selectedDominoIndex);
                else
                    hand[i].Draw(spriteBatch, x, y, 0f);
            }
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
