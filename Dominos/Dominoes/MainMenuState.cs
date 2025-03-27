using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    internal class MainMenuState
    {
        private GraphicsDevice graphicsDevice;
        Button titleText;
        Button startButton;

        List<Button> buttons;

        public MainMenuState(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Initialize()
        {
            titleText = new Button(graphicsDevice, new Rectangle(400, 100, 200, 100), "DOMINOS", UI_Manager.LargeFont, Color.White);

            startButton = new Button(graphicsDevice, new Rectangle(480, 350, 40, 40), "Start", UI_Manager.SmallFont, Color.White);
            startButton.OnLeftButtonClick += StartButtonClick;

            buttons = new List<Button>();
            buttons.Add(titleText);
            buttons.Add(startButton);
        }

        public void StartButtonClick()
        {
            Game1.currentState = State.Game;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Button button in buttons)
            {
                if (button != null) 
                    button.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttons)
            {
                if (button != null)
                    button.Draw(spriteBatch);
            }
        }
    }
}
