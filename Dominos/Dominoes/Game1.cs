using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Dominoes
{
    /**
     * Purpose: 
     * Authors: Anthony Lopez
     * Date: 1.9.25
     * Modifications: 
     * Notes: 
     *  - Added boolean to toggle auto play on/off for easier testing
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

        // random for testing purposes
        Random rng;
        bool flag;

        // bool to toggle auto play on/off
        bool auto = false;

        /// <summary>
        /// Constructor for objects of Game1
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // set window size
            graphics.PreferredBackBufferWidth = 1000;  // width
            graphics.PreferredBackBufferHeight = 500; // height
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

            // random for testing purposes
            rng = new Random();
            flag = true;

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

            // get index of winning player
            // gameManager.GetWinner();

            if (auto)
                AutoTest(); // for automated testing
            else
                GetPlayerInput(); // for manual testing

            // domino manager update
            gameManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Checks for player input, if A or D is pressed and game end condition
        /// has not been met. 
        /// </summary>
        private void GetPlayerInput()
        {
            // if game end condition isnt met
            if (gameManager.GameOver != false)
            {
                // TODO: Add your update logic here
                if (InputManager.SingleKeyPress(Keys.A))
                {
                    output = gameManager.TestGame(true);
                }
                if (InputManager.SingleKeyPress(Keys.D))
                {
                    output = gameManager.TestGame(false);
                }
            }
        }

        /// <summary>
        /// Automatically plays the game. Used for testing purposes.
        /// </summary>
        private void AutoTest()
        {
            // if game end condition isnt met
            if (gameManager.GameOver != false)
            {
                flag = rng.Next(2) == 0;
                Debug.Print(flag.ToString(), Debug.Level.Low);

                if (flag)
                    output = gameManager.TestGame(true);
                else
                    output = gameManager.TestGame(false);
            }
            else 
            {
                gameManager.GetWinningPlayer();
            }
        }

        /// <summary>
        /// Draws content for the game
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // draw game manager stuff
            gameManager.Draw(spriteBatch, gameTime, output);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
