using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Dominoes
{
    /**
     * Purpose: Manage dominoes by creating and shuffling them
     * Authors: Anthony Lopez
     * Date: 12.23.24
     * Modifications: 
     * Notes: 
     */

    internal class DominoManager
    {
        // variable declarations
        LinkedList<Domino> dominoList;

        /// <summary>
        /// Constructor for objects of class DominoManager
        /// </summary>
        public DominoManager()
        {
            CreateDominoes();
            // PrintDominoList();
        }

        /// <summary>
        /// Initializes the set of dominoes
        /// </summary>
        public void CreateDominoes()
        {
            dominoList = new LinkedList<Domino>();
            Texture2D texture;

            for (int i = 0; i <= 6; i++)
            {
                for (int j = i; j <= 6; j++)
                {
                    texture = UI_Manager.GetTexture(i + " | " + j);

                    dominoList.AddLast(new Domino(i, j, texture, 0, 0, texture.Width, texture.Height));
                }
            }
        }

        /// <summary>
        /// Prints list of dominoes to debug output
        /// </summary>
        public void PrintDominoList()
        {
            foreach (Domino domino in dominoList)
            {
                System.Diagnostics.Debug.WriteLine(domino.ToString());
            }
        }

        /// <summary>
        /// Returns a queue of shuffled dominoes
        /// </summary>
        public Queue<Domino> GetDominoQueue()
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
