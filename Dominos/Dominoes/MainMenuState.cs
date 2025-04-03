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
        Button configButton;
        Button creditButton;
        Button exitButton;

        int titleWidth = 200;
        int titleHeight = 100;

        int buttonWidth = 80;
        int buttonHeight = 40;

        List<Button> buttons;

        public MainMenuState(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Initialize()
        {
            titleText = new Button(graphicsDevice, new Rectangle(Game1.windowWidth/2 - titleWidth/2, Game1.windowHeight/5, titleWidth, titleHeight), "DOMINOS", UI_Manager.LargeFont, Color.White);

            startButton = new Button(graphicsDevice, new Rectangle(0, 0, buttonWidth, buttonHeight), "Start", UI_Manager.SmallFont, Color.White);
            startButton.OnLeftButtonClick += StartButtonClick;

            configButton = new Button(graphicsDevice, new Rectangle(0, 0, buttonWidth, buttonHeight), "Config", UI_Manager.SmallFont, Color.White);
            creditButton = new Button(graphicsDevice, new Rectangle(0, 0, buttonWidth, buttonHeight), "Credits", UI_Manager.SmallFont, Color.White);
            exitButton = new Button(graphicsDevice, new Rectangle(0, 0, buttonWidth, buttonHeight), "Exit", UI_Manager.SmallFont, Color.White);
            exitButton.OnLeftButtonClick += ExitButtonClick;

            buttons = new List<Button>();
            buttons.Add(titleText);
            buttons.Add(startButton);
            buttons.Add(configButton);
            buttons.Add(creditButton);

            buttons.Add(exitButton);

            int x = Game1.windowWidth/2 - ((buttonWidth * buttons.Count) / 2) - buttonWidth/4;
            int y = (int) (Game1.windowHeight * .8f);

            // exclude title text
            for (int i = 1; i < buttons.Count; i++)
            {
                buttons[i].SetPosition(x, y);
                x += buttonWidth + buttonWidth/2;
            }
        }

        public void StartButtonClick()
        {
            Game1.currentState = State.Game;
        }
        
        public void ExitButtonClick()
        {
            Game1.currentState = State.Exit;
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
