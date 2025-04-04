using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    internal class WinState
    {
        private GraphicsDevice graphicsDevice;
        Button winText;
        Button returnButton;

        int titleWidth = 200;
        int titleHeight = 100;

        int buttonWidth = 80;
        int buttonHeight = 40;

        List<Button> buttons;

        public WinState(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Initialize()
        {
            winText = new Button(graphicsDevice, new Rectangle(Game1.windowWidth/2 - titleWidth/2, Game1.windowHeight/5, titleWidth, titleHeight), "DOMINOS", UI_Manager.LargeFont, Color.White);

            winText = new Button(graphicsDevice, new Rectangle(0, 0, buttonWidth, buttonHeight), "Start", UI_Manager.SmallFont, Color.White);
            returnButton.OnLeftButtonClick += ReturnButtonClick;

            buttons = new List<Button>();
            buttons.Add(winText);
            buttons.Add(returnButton);

            int x = Game1.windowWidth/2 - ((buttonWidth * buttons.Count) / 2) - buttonWidth/4;
            int y = (int) (Game1.windowHeight * .8f);

            // exclude title text
            for (int i = 1; i < buttons.Count; i++)
            {
                buttons[i].SetPosition(x, y);
                x += buttonWidth + buttonWidth/2;
            }
        }

        public void ReturnButtonClick()
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
