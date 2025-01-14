using System;
using System.Collections.Generic;
// using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// using SharpDX.Direct2D1;
using Color = Microsoft.Xna.Framework.Color;

namespace Dominoes
{
    /**
     * Purpose: Manage Dominoes by creating and shuffling them
     * Authors: Anthony Lopez
     * Date: 12.23.24
     * Modifications: 
     * Notes: 
     *  - Need to add coyote time for dragging Dominoes
     */

    internal class DominoManager
    {
        // variable declarations
        LinkedList<Domino> dominoList;
        // timer for drag logic
        double timer;

        /// <summary>
        /// Constructor for objects of class DominoManager
        /// </summary>
        public DominoManager()
        {
            timer = 10;

            CreateDominoes();
            // PrintDominoList();
        }

        public void Update(GameTime gameTime)
        {
            Drag(gameTime);
        }

        /// <summary>
        /// Initializes the set of Dominoes
        /// </summary>
        public void CreateDominoes()
        {
            dominoList = new LinkedList<Domino>();
            Texture2D texture = UI_Manager.GetTexture(1 + " | " + 1);

            int x = 0;
            int y = 0;

            for (int i = 0; i <= 6; i++)
            {
                for (int j = i; j <= 6; j++)
                {
                    texture = UI_Manager.GetTexture(i + " | " + j);

                    if (x > 400)
                    {
                        x = 0;
                        y += (texture.Height / 3) + 5;
                    }

                    x += (texture.Width / 3) + 5;

                    dominoList.AddLast(new Domino(i, j, texture, 325 + x, y, texture.Width, texture.Height));
                }
            }
        }

        /// <summary>
        /// Prints list of Dominoes to debug output
        /// </summary>
        public void PrintDominoList()
        {
            foreach (Domino domino in dominoList)
            {
                System.Diagnostics.Debug.WriteLine(domino.ToString());
            }
        }

        /// <summary>
        /// Checks and handles drag collision for all Dominoes
        /// </summary>
        public void Drag(GameTime gameTime)
        {
            bool drag = false;
            Domino domino1 = null;

            foreach (Domino domino in dominoList)
            {
                bool Dominoeselected = domino.Position.Contains(InputManager.MousePosition);

                // detect if left mouse button is being held down
                if (InputManager.MousePressed && Dominoeselected)
                {
                    drag = true;
                }
                else if (InputManager.MousePressed && domino1 != null)
                {
                    drag = true;
                    timer -= gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (InputManager.MouseReleased || timer <= 0)
                {
                    drag = false;
                    timer = 10;
                    domino1 = null;
                    System.Diagnostics.Debug.WriteLine("Drag variables reset.");
                }

                if (drag)
                {
                    domino1 = domino;

                    // get mouse position
                    Vector2 mousePosition = new Vector2(InputManager.MousePosition.X, InputManager.MousePosition.Y);
                    // update button position to mouse position + an offset based on domino dimensions
                    domino1.X = (int)mousePosition.X - domino1.Position.Width / 2;
                    domino1.Y = (int)mousePosition.Y - domino1.Position.Height / 2;

                    // System.Diagnostics.Debug.WriteLine("Mouse Position: ({0}, {1})", mousePosition.X, mousePosition.Y);
                    // System.Diagnostics.Debug.WriteLine("Domino Position: ({0}, {1})", domino.X, domino.Y);

                    // System.Diagnostics.Debug.WriteLine("Timer: {0}", timer);
                }
            }
        }

        /// <summary>
        /// Returns a queue of shuffled Dominoes
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
