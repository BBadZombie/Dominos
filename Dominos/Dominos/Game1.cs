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

        private SpriteFont font;

        // domino variables
        private GameManager gameManager;
        private UI_Manager uiManager;
        private string output;

        // test button variable
        private Button testButton;

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

            // initialize test button
            Rectangle rectangle = new Rectangle(550, 325, 100, 50);
            testButton = new Button(graphics.GraphicsDevice, rectangle, "Lol", UI_Manager.SmallFont, Color.White);

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
            // only input that is not handled by InputManager is Exit() because Game object reference is required
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // input manager update
            InputManager.Update(gameTime);

            // TODO: Add your update logic here
            if (InputManager.SingleKeyPress(Keys.A))
            {
                output = gameManager.TestGame(true);
            }
            if (InputManager.SingleKeyPress(Keys.D))
            {
                output = gameManager.TestGame(false);
            }

            // test button update
            testButton.Update(gameTime);
            // domino manager update
            gameManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws content for the game
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // temporary draw code to display Dominoes
            int x = 0;
            int y = 50;

            // draw Dominoes
            List<Domino> dominoList = gameManager.Board.ToList();
            for (int i = 0; i < dominoList.Count; i++)
            {
                dominoList[i].Draw(spriteBatch);
            }

            // draw player information
            spriteBatch.DrawString(font, "Players:", new Vector2(5, 5), Color.White);
            for (int i = 0; i < gameManager.PlayerList.Count; i++)
            {
                string playerInfo = "#" + (i + 1) + ": " + gameManager.PlayerList[i].ToString() + " ";
                spriteBatch.DrawString(font, playerInfo, new Vector2(5, 30 + (30 * i)), Color.White);
            }

            // draw domino board information (Dominoes that are on the board)
            string boardInfo = gameManager.ToString() + " ";
            spriteBatch.DrawString(font, "Domino Board:", new Vector2(5, 150), Color.White);
            spriteBatch.DrawString(font, boardInfo, new Vector2(5, 175), Color.White);

            // draw output from game manager
            spriteBatch.DrawString(font, output, new Vector2(5, 325), Color.White);

            // draw test button
            testButton.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
