using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    /**
     * Purpose: Manage dominoes
     * Authors: Anthony Lopez
     * Date: 12.16.24
     * Modifications: 
     * Notes: 
     */

    internal class DominoManager
    {
        // variable declarations
        LinkedList<Domino> dominoList;

        // constructor for objects of DominoManager
        public DominoManager()
        {
            CreateDominoes();
            PrintDominoList();
        }

        // creates a standard set of 27 dominoes
        public void CreateDominoes()
        {
            dominoList = new LinkedList<Domino>();

            for (int i = 0; i <= 6; i++)
            {
                for (int j = i; j <= 6; j++)
                {
                    dominoList.AddLast(new Domino(i, j));
                }
            }
        }

        // prints dominoes for testing purposes
        public void PrintDominoList()
        {
            foreach (Domino domino in dominoList)
            {
                System.Diagnostics.Debug.WriteLine(domino.ToString());
            }
        }

        // returns a shuffled set of dominoes for adding to player hand
        public Queue<Domino> GetDominoList()
        {
            // convert LinkedList to List for easier LINQ manipulation
            List<Domino> dominoListAsList = dominoList.ToList();

            // use LINQ to shuffle the list by sorting based on random GUIDs
            var shuffledDominoList = dominoListAsList
                .OrderBy(x => Guid.NewGuid()) // generate a new random GUID for each item
                .ToList(); // convert back to List<Domino>

            // convert the shuffled List to a Queue
            Queue<Domino> shuffledQueue = new Queue<Domino>(shuffledDominoList);

            return shuffledQueue;
        }
    }
}
