using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    /**
     * Purpose: Manage domino board
     * Authors: Anthony Lopez
     * Date: 12.16.24
     * Modifications: 
     * Notes: 
     */

    internal class DominoBoard
    {
        // variable declarations
        LinkedList<Domino> dominoBoard;

        public DominoBoard(LinkedList<Domino> dominoBoard)
        {
            this.dominoBoard = dominoBoard;
        }

        // adds a domino to the domino board using game logic
        public void Add(Domino domino)
        {
            // handle case where board is empty
            if (dominoBoard.Count == 0)
            {
                if (!domino.IsCow())
                    throw new Exception("First domino in board must be cow (6 | 6)");
                else
                    dominoBoard.AddFirst(domino);
            }
            else
            {
                
            }

        }
    }
}
