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

    // state enums
    public enum State
    {
        MainMenu,
        Game,
        Exit
    }

    public class Game1 : Game
    {
        // variable declarations
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SpriteFont font;

        // domino variables
        private UI_Manager uiManager;
        private string output;

        // random for testing purposes
        Random rng;
        bool flag;

        // bool to toggle auto play on/off
        bool auto = false;

        // what state to draw/update
        public static State currentState;

        MainMenuState mainMenuState;
        GameManager gameState;

        public static int windowWidth = 1000;
        public static int windowHeight = 500;

        /// <summary>
        /// Constructor for objects of Game1
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // set window size
            graphics.PreferredBackBufferWidth = windowWidth;  // width
            graphics.PreferredBackBufferHeight = windowHeight; // height
        }

        /// <summary>
        /// Initialize fields for class Game1
        /// </summary>
        protected override void Initialize()
        {
            // initialize managers
            uiManager = new UI_Manager(spriteBatch, Content);
            gameState = new GameManager();
            mainMenuState = new MainMenuState(GraphicsDevice);
            mainMenuState.Initialize();

            // initialize other fields
            output = " ";
            font = UI_Manager.SmallFont;

            // random for testing purposes
            rng = new Random();
            flag = true;

            // default state should be main menu, but will remain game for testing
            currentState = State.MainMenu;

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

            // look for enter to swap game state
            if (InputManager.SingleKeyPress(Keys.Enter))
            {
                currentState = (currentState == State.MainMenu) ? State.Game : State.MainMenu;
            }

            switch (currentState)
            {
                case State.MainMenu:
                    mainMenuState.Update(gameTime);
                    break;
                case State.Game:
                    gameState.Update(gameTime);
                    break;
                case State.Exit:
                    Exit();
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Automatically plays the game. Used for testing purposes.
        /// </summary>
        private void AutoTest()
        {
            // if game end condition isnt met
            if (gameState.GameOver != false)
            {
                flag = rng.Next(2) == 0;
                Debug.Print(flag.ToString(), Debug.Level.Low);

                if (flag)
                    output = gameState.TestGame(true);
                else
                    output = gameState.TestGame(false);
            }
            else 
            {
                gameState.GetWinningPlayer();
            }
        }

        /// <summary>
        /// Draws content for the game
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // finite state machine
            switch (currentState)
            {
                case State.MainMenu:
                    mainMenuState.Draw(spriteBatch);
                    break;
                case State.Game:
                    gameState.Draw(spriteBatch, gameTime, output);
                    break;
            }

            // draw game state
            if (currentState == State.Game)
            {
                gameState.Draw(spriteBatch, gameTime, output);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
