using SharpDX.Direct2D1;
using SharpDX.Direct3D9;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SharpDX.Direct2D1.Effects;

#nullable enable

namespace Dominoes
{
    /**
     * Purpose: GameObject class for game objects such as Dominoes and buttons
     * Author: Anthony Lopez
     * Date: 1.8.25
     * Modifications:
     *  - Added Draw() method that rotates game objects
     *  - Added Draw() method that rotates game objects and takes color parameter
     * Notes: 
     */
    public class GameObject
    {
        // fields
        private Texture2D texture;
        private Rectangle position;
        private bool isVisible;

        // neighbors
        private Domino? previous;
        private Domino? next;

        // which side does this domino and its neighbor share
        // (which sides match)
        private string? previousMatch;
        private string? nextMatch;

        // properties
        public Texture2D Texture => texture;
        public Rectangle Position => position;

        public int X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public int Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        /// <summary>
        /// Constructor for objects of class GameObject
        /// </summary>
        public GameObject(Texture2D texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            this.position = new Rectangle(x, y, width, height);
            this.isVisible = false;
        }

        /// <summary>
        /// Draw method for objects of class GameObject
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            if (isVisible)
            {
                sb.Draw(texture, position, Color.White);
            }
        }

        /// <summary>
        /// Draw method for objects of class GameObject that draws
        /// game object at a given position
        /// </summary>
        public void Draw(SpriteBatch sb, int x, int y)
        {
            this.X = x;
            this.Y = y;

            if (isVisible)
            {
                sb.Draw(texture, position, Color.White);
            }
        }

        /// <summary>
        /// Draw method for objects of class GameObject that draws
        /// game object at a given position
        /// </summary>
        public void Draw(SpriteBatch sb, int x, int y, float rotationAngle)
        {
            this.X = x;
            this.Y = y;

            if (isVisible)
            {
                Vector2 newOrigin = new Vector2(position.Width / 2, position.Height / 2);
                sb.Draw(texture, this.Position, null, Color.White, rotationAngle, newOrigin, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Draw method for objects of class GameObject that draws
        /// game object at a given position
        /// </summary>
        public void DrawColor(SpriteBatch sb, int x, int y, float rotationAngle, Color color)
        {
            this.X = x;
            this.Y = y;

            if (isVisible)
            {
                Vector2 newOrigin = new Vector2(position.Width / 2, position.Height / 2);
                sb.Draw(texture, this.Position, null, color, rotationAngle, newOrigin, SpriteEffects.None, 0f);
            }
        }

        public void DrawWithSelector(SpriteBatch sb, int x, int y, float rotationAngle, int selectedDominoIndex)
        {
            // draw this game object
            Draw(sb, x, y, rotationAngle);

            // draw selector above this game object
            if (isVisible)
            {
                Vector2 newOrigin = new Vector2(position.X - position.Width / 4 + 1, position.Y - 55);
                sb.DrawString(UI_Manager.SmallFont, "V", newOrigin, Color.White);
            }
        }
    }
}
