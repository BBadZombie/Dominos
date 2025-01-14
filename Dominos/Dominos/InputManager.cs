using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dominoes
{
    /**
     * Purpose: Manage all input related things for the program
     * Authors: Anthony Lopez
     * Date: 12.26.24
     * Modifications: 
     * Notes: Need to finish class
     */
    internal static class InputManager
    {
        // mouse fields
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        // keyboard fields
        private static KeyboardState currentKBState;
        private static KeyboardState previousKBState;

        // keyboard properties
        public static bool KeyPressed { get; private set; }

        // mouse properties
        public static Vector2 MousePosition => Mouse.GetState().Position.ToVector2();
        public static bool MouseClicked { get; private set; }
        public static bool MouseReleased { get; private set; }
        public static bool MousePressed { get; private set; }


        // update logic for inputs
        public static void Update(GameTime gameTime)
        {
            // set keyboard properties
            

            // set mouse properties
            MouseClicked = LeftMouseClick();
            MouseReleased = LeftMouseRelease();
            MousePressed = LeftMouseHeld();

            // set keyboard states
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            // set mouse states
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Returns true if the left mouse button is clicked
        /// </summary>
        private static bool LeftMouseClick()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed) && (previousMouseState.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Returns true if the left mouse button is clicked
        /// </summary>
        private static bool LeftMouseRelease()
        {
            return (currentMouseState.LeftButton == ButtonState.Released) && (previousMouseState.LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Returns true if the left mouse button is being held
        /// </summary>
        private static bool LeftMouseHeld()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Returns true if the given key is pressed
        /// </summary>
        public static bool SingleKeyPress(Keys key)
        {
            return currentKBState.IsKeyDown(key) && previousKBState.IsKeyUp(key);
        }
    }
}
