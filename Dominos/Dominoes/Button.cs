using System;
using System.DirectoryServices.ActiveDirectory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dominoes
{
    // delegates
    /// <summary>
    /// If the client wants to be notified when a button is clicked, it must
    /// implement a method matching this delegate and then tie that method to
    /// the button's "OnButtonClick" event.
    /// </summary>
    /// <param name="clickedButton">The delegate will be called with a reference to the clicked button</param>
    public delegate void OnButtonClickDelegate();

    /**
     * Purpose: Represents a button by adding button specific information 
     *          and behavior to the generic GameObject class.
     * Authors: Anthony Lopez
     * Date: 12.23.24
     * Modifications: 
     * Notes: 
     */
    public class Button
    {
        // button specific fields
        protected SpriteFont font;
        protected MouseState prevMState;
        protected bool enabled = true;
        private string text;
        protected Rectangle position; // button position and size
        private Vector2 textLoc;
        Texture2D buttonImg;
        private Color textColor;

        /// <summary>
        /// If the client wants to be notified when a button is clicked, it must
        /// implement a method matching OnButtonClickDelegate and then tie that method to
        /// the button's "OnButtonClick" event.
        /// 
        /// The delegate will be called with a reference to the clicked button.
        /// </summary>
        public event OnButtonClickDelegate OnLeftButtonClick;

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // TODO: Add your new event here!
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public event OnButtonClickDelegate OnRightButtonClick;

        /// <summary>
        /// Constructor for objects of class Button 
        /// </summary>
        /// <param name="position">Where to draw the button's top left corner</param>
        /// <param name="text">The text to draw on the button</param>
        /// <param name="font">The font to use when drawing the button text.</param>
        /// <param name="color">The color to tint the button when it is active. Buttons are always grayed out when disabled.</param>
        public Button(GraphicsDevice device, Rectangle position, String text, SpriteFont font, Color color)
        {
            // Save copies/references to the info we'll need later
            this.font = font;
            this.position = position;
            this.text = text;

            // Figure out where on the button to draw it
            Vector2 textSize = font.MeasureString(text);

            float x = (position.X + position.Width / 2) - textSize.X / 2;
            float y = (position.Y + position.Height / 2) - textSize.Y / 2;
            textLoc = new Vector2(x, y);

            // Invert the button color for the text color (because why not)
            this.textColor = new Color(255 - color.R, 255 - color.G, 255 - color.B);

            // Make a custom 2d texture for the button itself
            buttonImg = new Texture2D(device, position.Width, position.Height, false, SurfaceFormat.Color);
            int[] colorData = new int[buttonImg.Width * buttonImg.Height];
            Array.Fill<int>(colorData, (int)color.PackedValue);
            buttonImg.SetData<Int32>(colorData, 0, colorData.Length);
        }

        /// <summary>
        /// Each frame, update its status if it's been clicked.
        /// </summary>
        /// <param name="gameTime">Unused, but required to implement abstract class</param>
        public void Update(GameTime gameTime)
        {
            // Check/capture the mouse state regardless of whether this button
            // if active so that it's up to date next time!
            MouseState mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Released &&
                prevMState.LeftButton == ButtonState.Pressed &&
                this.position.Contains(mState.Position))
            {
                if (OnLeftButtonClick != null)
                {
                    OnLeftButtonClick();
                }
            }
            
            // click detection
            if (mState.RightButton == ButtonState.Released &&
                prevMState.RightButton == ButtonState.Pressed &&
                this.position.Contains(mState.Position))
            {
                if (OnRightButtonClick != null)
                {
                    OnRightButtonClick();
                }
            }

            // drag function
            Drag(gameTime);

            prevMState = mState;
        }

        // allows buttons to be dragged
        public void Drag(GameTime gameTime)
        {
            // detect if left mouse button is being held down
            MouseState mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && this.position.Contains(mState.Position))
            {
                // get mouse position
                Vector2 mousePosition = new Vector2(mState.X, mState.Y);
                // update button position to current position + mouse position
                this.position.X = (int) mousePosition.X - this.position.Width / 2;
                this.position.Y = (int) mousePosition.Y - this.position.Height / 2;

                Debug.Print("Mouse Position: (" + mousePosition.X + "," + mousePosition.Y + ")", Debug.Level.Low);
                Debug.Print("Button Position: (" + this.position.X + "," + this.position.Y + ")", Debug.Level.Low);
            }
        }

        /// <summary>
        /// Override the GameObject Draw() to draw the button and then
        /// overlay it with text.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch on which to draw this button. The button 
        /// assumes that Begin() has already been called and End() will be called later.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the button itself
            spriteBatch.Draw(buttonImg, position, Color.White);

            // Draw button text over the button
            spriteBatch.DrawString(font, text, textLoc, textColor);
        }

    }
}
