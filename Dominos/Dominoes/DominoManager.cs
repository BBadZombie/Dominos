using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Dominoes
{
    /**
     * Purpose: Manage Dominoes by creating and shuffling them
     * Authors: Anthony Lopez
     * Date: 12.23.24
     * Modifications: 
     * Notes: 
     *  - Need to fix coyote time for dragging Dominoes
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

        /// <summary>
        /// Update method for class DominoManager
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Drag(gameTime);
        }

        /// <summary>
        /// Draw method for class DominoManager
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // temporary draw code to display Dominoess
            SpriteFont font = UI_Manager.SmallFont;

            // draw Dominoes
            foreach (Domino domino in dominoList)
            {
                domino.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Initializes the set of Dominoes
        /// </summary>
        public void CreateDominoes()
        {
            dominoList = new LinkedList<Domino>();
            Texture2D texture = UI_Manager.GetTexture("scaled_" + 1 + " | " + 1);

            int x = 0;
            int y = 0;

            for (int i = 0; i <= 6; i++)
            {
                for (int j = i; j <= 6; j++)
                {
                    texture = UI_Manager.GetTexture("scaled_" + i + " | " + j);

                    if (x > 350)
                    {
                        x = 0;
                        y += texture.Height+ 5;
                    }

                    x += texture.Width + 5;

                    dominoList.AddLast(new Domino(i, j, texture, 375 + x, y, texture.Width, texture.Height));
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
                Debug.Print(domino.ToString(), Debug.Level.Low);
            }
        }

        /// <summary>
        /// Checks and handles drag collision for all Dominoes
        /// </summary>
        public void Drag(GameTime gameTime)
        {
            bool drag = false;
            Domino domino1 = null;

            // coyote time duration in seconds
            double coyoteTime = 0.3;  // 0.3 seconds

            foreach (Domino domino in dominoList)
            {
                bool dominoSelected = domino.Position.Contains(InputManager.MousePosition);

                // detect if the left mouse button is pressed and the domino is selected
                if (InputManager.MousePressed && dominoSelected)
                {
                    drag = true;
                }
                // continue dragging even after the mouse is released, within the coyote time window
                else if (InputManager.MousePressed && domino1 != null)
                {
                    drag = true;
                    timer -= gameTime.ElapsedGameTime.TotalSeconds;
                }
                // if mouse is released, we start counting the coyote time
                else if (InputManager.MouseReleased || timer <= 0)
                {
                    // if the mouse has been released for less than coyote time, keep dragging
                    if (gameTime.TotalGameTime.TotalSeconds - timer <= coyoteTime)
                    {
                        drag = true;
                    }
                    else
                    {
                        drag = false;
                        timer = 10; // reset timer
                        domino1 = null;
                    }
                }

                if (drag)
                {
                    domino1 = domino;

                    // get the current mouse position
                    Vector2 mousePosition = new Vector2(InputManager.MousePosition.X, InputManager.MousePosition.Y);

                    // update the domino position to the mouse position with an offset
                    domino1.X = (int)mousePosition.X - domino1.Position.Width / 2;
                    domino1.Y = (int)mousePosition.Y - domino1.Position.Height / 2;

                    Debug.Print("Mouse Position: (" + mousePosition.X + "," + mousePosition.Y + ")", Debug.Level.Low);
                    Debug.Print("Domino Position: (" + domino.X + "," + domino.Y + ")", Debug.Level.Low);
                    Debug.Print("Timer: " + timer, Debug.Level.Low);
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
