using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dominoes
{
    /**
     * Purpose: Handles all game assets, including both images and texts. Also
     *          handles all UI related responsibilities, such as drawing buttons. 
     * Author: Anthony Lopez
     * Date: 12.23.24
     * Modifications: 
     * Notes:
     */
    public class UI_Manager
    {
        // field declarations
        private static Dictionary<string, Texture2D> texturesDict = new Dictionary<string, Texture2D>();

        // field declarations for text handling
        private static SpriteFont smallFont;
        private static SpriteFont largeFont;
        private SpriteBatch spriteBatch;
        private static Dictionary<string, string> UIFontTextDict = new Dictionary<string, string>();

        // properties
        public static SpriteFont LargeFont
        {
            get { return largeFont; }
        }

        public static SpriteFont SmallFont
        {
            get { return smallFont; }
        }

        /// <summary>
        /// Constructor for objects of UI_Manager class
        /// </summary>
        public UI_Manager(SpriteBatch spriteBatch, ContentManager content)
        {
            this.spriteBatch = spriteBatch;

            LoadAssets(content);
        }

        /// <summary>
        /// Loads all font and texture assets for the game.
        /// </summary>
        /// <param name="content"></param>
        public void LoadAssets(ContentManager content)
        {
            // load fonts
            smallFont = content.Load<SpriteFont>("arial15");
            largeFont = content.Load<SpriteFont>("arial20");

            // load domino textures
            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j <= 6; j++)
                {
                    texturesDict[i + " | " + j] = content.Load<Texture2D>("StitchedImages/" + i + " " + j);
                }
            }

            // load text
            // UIFontTextDict["start"] = "START";
        }

        /// <summary>
        /// Returns the associated image with the given string key.
        /// Returns null if the asset does not exist. 
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public static Texture2D GetTexture(string imageName)
        {
            if (texturesDict.TryGetValue(imageName, out Texture2D texture))
            {
                return texture;
            }
            else
            {
                Debug.WriteLine($"Couldn't find - " + imageName);
                return null;
            }
        }

        /// <summary>
        /// Returns associated text for the given string key. 
        /// </summary>
        /// <param name="textName"></param>
        /// <returns></returns>
        public static string GetText(string textName)
        {
            if (UIFontTextDict.TryGetValue(textName, out string text))
                return text;

            return null;
        }

        /// <summary>
        /// Draws associated text for the given string key at a given position.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pos"></param>
        /// <param name="color"></param>
        public void DrawText(string key, Vector2 pos, Color color)
        {
            if (UIFontTextDict.TryGetValue(key, out string text))
            {
                spriteBatch.DrawString(largeFont, text, pos, color);
            }
        }

        /// <summary>
        /// Draws associated text for the given string key at the center of a rectangle.
        /// This is intended for drawing text on buttons. 
        /// </summary>
        /// <param name="textName"></param>
        /// <param name="rectPos"></param>
        /// <param name="color"></param>
        public void DrawText(string textName, Rectangle rectPos, Color color)
        {
            string text = GetText(textName);
            if (text != null)
            {
                //Step 1: find the size of the text
                Vector2 textArea = smallFont.MeasureString(text);

                //step 2: calculate the center position of the text using the rectangle element of a button
                Vector2 textPos = new Vector2(rectPos.X + (rectPos.Width - textArea.X) / 2, rectPos.Y + (rectPos.Height - textArea.Y) / 2);

                //step 3: draw the string with the new text position data we acquire above
                spriteBatch.DrawString(smallFont, text, textPos, color);
            }
        }
    }
}