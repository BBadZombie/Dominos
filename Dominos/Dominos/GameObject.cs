using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dominoes
{
    /**
     * Purpose: GameObject class for game objects such as Dominoes and buttons
     * Author: Anthony Lopez
     * Date: 12.23.24
     * Modifications:
     * Notes: 
     */
    public class GameObject
    {
        // fields
        private Texture2D texture;
        private Rectangle position;

        // properties
        public Rectangle Position
        {
            get { return position; }
        }

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

        /// <summary>
        /// Constructor for objects of class GameObject
        /// </summary>
        public GameObject(Texture2D texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            this.position = new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Draw method for objects of class GameObject
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        /// <summary>
        /// Draw method for objects of class GameObject that takes an x and y
        /// </summary>
        public void Draw(SpriteBatch sb, int x, int y)
        {
            this.X = (int) x;
            this.Y = (int) y;

            sb.Draw(texture, new Vector2(x, y), Color.White);
        }

        /// <summary>
        /// Draw method for objects of class GameObject that takes a Vector2
        /// </summary>
        public void Draw(SpriteBatch sb, Vector2 newPosition)
        {
            this.X = (int) newPosition.X;
            this.Y = (int) newPosition.Y;

            sb.Draw(texture, position, Color.White);
        }
    }
}
