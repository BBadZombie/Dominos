using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable enable

namespace Dominoes
{
    /**
     * Purpose: Represent a domino
     * Authors: Anthony Lopez
     * Date: 12.23.24
     * Modifications: 
     * Notes: 
     *  - The => operator is a more concise way to write a get only property
     */

    internal class Domino : GameObject
    {
        // variable declarations
        // represents numbers the Dominoes hold
        private int top;
        private int bottom;

        // neighbors
        private Domino? previous;
        private Domino? next;

        // which side does this domino and its neighbor share
        // (which sides match)
        public string? previousMatch;
        public string? nextMatch;

        // properties
        public int Top => top;
        public int Bottom => bottom;

        public int Total => top + bottom;

        // neighbor properties
        public Domino? Previous
        {
            get { return previous; }
            set
            {
                previous = value;
                previousMatch = value != null ? GetMatchingSideString(value) : string.Empty;
            }
        }

        public Domino? Next
        {
            get { return next; }
            set 
            {
                next = value;
                nextMatch = value != null ? GetMatchingSideString(value) : string.Empty;
            }
        }

        /// <summary>
        /// Constructor for objects of class Domino
        /// </summary>
        public Domino(int top, int bottom, Texture2D texture, int x, int y, int width, int height) : base(texture, x, y, width / 3, height / 3)
        {
            this.top = top;
            this.bottom = bottom;
            Previous = null;
            Next = null;
            previousMatch = string.Empty;
            nextMatch = string.Empty; ;
        }

        /// <summary>
        /// Returns a string indicating which side of this domino matches with the given domino
        /// </summary>
        public string GetMatchingSideString(Domino domino)
        {
            string result = string.Empty;

            if (this != null && domino != null)
            {
                if (this.Top == domino.Top || this.Top == domino.Bottom)
                {
                    result = "Top";
                }
                else if (this.Bottom == domino.Top || this.Bottom == domino.Bottom)
                {
                    result = "Bottom";
                }
            }

            if (result != string.Empty)
            {
                System.Diagnostics.Debug.WriteLine("This domino: {0} shares {1} side with given domino: {2}.", this, result, domino);
            }

            return result;
        }

        /// <summary>
        /// Returns an int of the side of this domino that matches with the given domino
        /// </summary>
        public int GetMatchingSide(Domino domino)
        {
            if (this != null && domino != null)
            {
                if (this.Top == domino.Top || this.Top == domino.Bottom)
                {
                    return this.Top;
                }
                else if (this.Bottom == domino.Top || this.Bottom == domino.Bottom)
                {
                    return this.Bottom;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns a boolean indicating if this domino is a cow. EX: (6 | 6)
        /// </summary>
        public bool IsCow()
        {
            return top == 6 && bottom == 6;
        }

        /// <summary>
        /// Returns a boolean indicating if this domino is a cow. EX: (0 | 0)
        /// </summary>
        public bool IsDouble()
        {
            return top == bottom;
        }

        /// <summary>
        /// Returns information about this domino as a string
        /// </summary>
        public override string ToString()
        {
            return "(" + top + " | " + bottom + ")";
        }
    }
}
