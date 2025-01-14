using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    /**
     * Purpose: Manage debug statements throughout the project
     * Authors: Anthony Lopez
     * Date: 1.8.25
     * Modifications: 
     * Notes: 
     *  - This class will make debugging and outputting to console much easier
     *  - It will print debug statements based on what level the statement is, and
     *    the current level of debug statements that are allowed to be outputted.
     *  - If the level is low, all debug statements will be printed. If the level is
     *    medium, then all medium and high level debug statements will be printed. 
     *    If the level is high, then only high debug statements will be printed. 
     *  - Another feature in this class that could be implemented is the type of 
     *    debug statement, represented as an enum. This would allow for even more 
     *    control over what debug statements will be printed with minimum effort.
     */
    internal static class Debug
    {
        // this enum represents how important a debug message is
        public enum Level
        {
            Low,
            Medium,
            High
        }

        // this variable represents the current level of debug messages that 
        // are being printed
        static Level currentLevel = Level.Medium;

        /// <summary>
        /// Prints the given input string if the debug level is set to
        /// the current debug level or higher
        /// </summary>
        public static void Print(string input, Level level)
        {
            if (LevelToInt(level) >= LevelToInt(currentLevel))
                System.Diagnostics.Debug.Print(input);
        }

        /// <summary>
        /// Sets the current debug level 
        /// </summary>
        public static void SetLevel(Level level)
        {
            currentLevel = level;
        }

        /// <summary>
        /// Returns a number that corresponds with the given debug level.
        /// Retuns -1 by default (this shouldn't happen though)
        /// </summary>
        private static int LevelToInt(Level level)
        {
            int levelValue = -1;

            if (level == Level.Low)
                levelValue = 0;

            if (level == Level.Medium)
                levelValue = 1;

            if (level == Level.High)
                levelValue = 2;

            return levelValue;
        }
    }
}
