using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    /**
     * Purpose: Represent a domino
     * Authors: Anthony Lopez
     * Date: 12.16.24
     * Modifications: 
     * Notes: 
     */

    internal class Domino
    {
        // variable declarations
        int top;
        int bottom;

        // properties
        public int Top
        {
            get { return top; }
        }

        public int Bottom
        {
            get { return bottom; }
        }

        // constructor for objects of Domino
        public Domino(int top, int bottom)
        {
            this.top = top;
            this.bottom = bottom;
        }

        public bool IsCow()
        {
            return top == 6 && bottom == 6;
        }

        public override string ToString()
        {
            return "(" + top + " | " + bottom + ")";
        }
    }
}
