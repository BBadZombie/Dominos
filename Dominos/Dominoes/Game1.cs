using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dominoes
{
    /**
     * Purpose: 
     * Authors: Anthony Lopez
     * Date: 12.16.24
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
        private string output;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            gameManager = new GameManager();
            output = " ";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("Arial20");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            if (SingleKeyPress(Keys.Space))
            {
                output = gameManager.TestGame();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // draw player information
            spriteBatch.DrawString(font, "Players:", new Vector2(5, 5), Color.White);
            for (int i = 0; i < gameManager.PlayerList.Count; i++)
            {
                string playerInfo = "#" + (i + 1) + ": " + gameManager.PlayerList[i].ToString() + " ";
                spriteBatch.DrawString(font, playerInfo, new Vector2(5, 30 + (30 * i)), Color.White);
            }

            // draw domino board information
            spriteBatch.DrawString(font, "Domino Board:", new Vector2(5, 200), Color.White);
            string boardInfo = gameManager.GetDominoBoard() + " ";
            spriteBatch.DrawString(font, boardInfo, new Vector2(5, 230), Color.White);

            spriteBatch.DrawString(font, output, new Vector2(5, 300), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool SingleKeyPress(Keys key)
        {
            return currentKBState.IsKeyDown(key) && previousKBState.IsKeyUp(key);
        }
    }
}
