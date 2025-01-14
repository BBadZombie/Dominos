using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dominoes
{
    /**
     * Purpose: GameObject class for game objects such as dominoes and buttons
     * Author: Anthony Lopez
     * Date: 12.23.24
     * Modifications:
     * Notes: 
     */
    public class GameObject
    {
        // fields
        private Texture2D texture;
        private Rectangle rect;

        // properties
        public Rectangle Rect
        {
            get { return rect; }
        }

        public int X
        {
            get { return rect.X; }
            set { rect.X = value; }
        }

        public int Y
        {
            get { return rect.Y; }
            set { rect.Y = value; }
        }

        /// <summary>
        /// Constructor for objects of class GameObject
        /// </summary>
        public GameObject(Texture2D texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            this.rect = new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Draw method for objects of class GameObject
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, rect, Color.White);
        }

        /// <summary>
        /// Draw method for objects of class GameObject that takes a position
        /// </summary>
        public void Draw(SpriteBatch sb, Vector2 position)
        {
            this.X = (int) position.X;
            this.Y = (int) position.Y;

            sb.Draw(texture, rect, Color.White);
        }
    }
}
