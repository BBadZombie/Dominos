using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dominoes
{
    /**
     * Purpose: 
     * Authors: Anthony Lopez
     * Date: 12.23.24
     * Modifications: 
     * Notes: 
     */

    public class Game1 : Game
    {
        // variable declarations
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // game variables
        private KeyboardState currentKBState;
        private KeyboardState previousKBState;

        private SpriteFont font;

        // domino variables
        private GameManager gameManager;
        private UI_Manager uiManager;
        private string output;

        /// <summary>
        /// Constructor for objects of Game1
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initialize fields for class Game1
        /// </summary>
        protected override void Initialize()
        {
            // initialize managers
            uiManager = new UI_Manager(spriteBatch, Content);
            gameManager = new GameManager();

            // initialize other fields
            output = " ";
            font = UI_Manager.SmallFont;

            base.Initialize();
        }

        /// <summary>
        /// This method would usually load the content for the game, but
        /// that resposibility has been delegated to the UI_Manager class
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            if (SingleKeyPress(Keys.A))
            {
                output = gameManager.TestGame(true);
            }
            if (SingleKeyPress(Keys.D))
            {
                output = gameManager.TestGame(false);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws content for the game
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // temporary draw code to display dominoes
            int x = 0;
            int y = 50;

            // draw dominoes
            List<Domino> dominoList = gameManager.Board.ToList();
            for (int i = 0; i < dominoList.Count; i++)
            {
                if (x > 400)
                {
                    x = 0;
                    y += dominoList[i].Rect.Height + 5;
                }

                x += dominoList[i].Rect.Width + 5;

                dominoList[i].Draw(spriteBatch, new Vector2(325 + x, y));
            }

            // draw player information
            spriteBatch.DrawString(font, "Players:", new Vector2(5, 5), Color.White);
            for (int i = 0; i < gameManager.PlayerList.Count; i++)
            {
                string playerInfo = "#" + (i + 1) + ": " + gameManager.PlayerList[i].ToString() + " ";
                spriteBatch.DrawString(font, playerInfo, new Vector2(5, 30 + (30 * i)), Color.White);
            }

            // draw domino board information (dominoes that are on the board)
            string boardInfo = gameManager.ToString() + " ";
            spriteBatch.DrawString(font, "Domino Board:", new Vector2(5, 175), Color.White);
            spriteBatch.DrawString(font, boardInfo, new Vector2(5, 200), Color.White);

            // draw output from game manager
            spriteBatch.DrawString(font, output, new Vector2(5, 375), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Returns true if the given key is pressed
        /// </summary>
        private bool SingleKeyPress(Keys key)
        {
            return currentKBState.IsKeyDown(key) && previousKBState.IsKeyUp(key);
        }
    }
}
