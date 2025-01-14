using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dominoes
{
    /**
     * Purpose: Manage gameplay state
     * Authors: Anthony Lopez
     * Date: 12.16.24
     * Modifications: 
     * Notes: 
     */

    internal class GameManager
    {
        // variable declarations
        DominoManager dominoManager;
        PlayerManager playerManager;

        DominoBoard board;

        int currentPlayerIndex;
        int turn;

        // properties
        public DominoManager DominoManager
        {
            get { return dominoManager; }
        }

        public DominoBoard Board
        {
            get { return board; }
        }

        public List<Player> PlayerList
        {
            get { return playerManager.PlayerList; }
        }

        /// <summary>
        /// Constructor for objects of class GameManager
        /// </summary>
        public GameManager()
        {
            // initialize managers
            dominoManager = new DominoManager();
            playerManager = new PlayerManager(dominoManager.GetDominoQueue());

            // initialize board
            board = new DominoBoard();
            turn = 0;
        }

        /// <summary>
        /// Temporary method that simulates a game of dominoes; used for testing purposes. 
        /// </summary>
        /// <param name="side"></param>
        public string TestGame(bool side)
        {
            // handle case where board is empty
            bool cowPlayed = false;
            for (int i = 0; i < playerManager.PlayerList.Count; i++)
            {
                // print player hand
                for (int j = 0; j < playerManager.PlayerList[i].PlayerHand.Count; j++)
                {
                    Domino cow = playerManager.PlayerList[i].HasCow();

                    if (cow != null)
                    {
                        // add domino to board and remove it from player hand
                        AddDominoToBoard(playerManager.PlayerList[i].PlayDomino(cow), side);

                        // set the first player index
                        currentPlayerIndex = i;
                        cowPlayed = true;
                        turn += 1;
                    }
                }

                // exit the loop once cow has been played
                if (cowPlayed)
                    return "Turn: " + turn + "\nPlayer #" + (currentPlayerIndex + 1) + " \nCow has been played";
            }

            // make sure player index is correct
            currentPlayerIndex = (currentPlayerIndex == 3) ? 0 : currentPlayerIndex + 1;

            // now, try to play a domino that matches the right side of the board
            List<Domino> currentPlayerHand = playerManager.PlayerList[currentPlayerIndex].PlayerHand;
            for (int i = 0; i < currentPlayerHand.Count; i++)
            {
                if (board.Count > 0)
                {
                    if (side)
                    {
                        if (board.IsHeadPlayable(currentPlayerHand[i]) && AddDominoToBoard(currentPlayerHand[i], side))
                        {
                            currentPlayerHand.RemoveAt(i); // remove played domino from hand
                            turn += 1;
                            return "Turn: " + turn + "\nPlayer #" + (currentPlayerIndex + 1) + "\nSuccessful play";
                        }
                    }
                    else if (!side)
                    {
                        if (board.IsTailPlayable(currentPlayerHand[i]) && AddDominoToBoard(currentPlayerHand[i], side))
                        {
                            currentPlayerHand.RemoveAt(i); // remove played domino from hand
                            turn += 1;
                            return "Turn: " + turn + "\nPlayer #" + (currentPlayerIndex + 1) + " \nSuccessful play";
                        }
                    }
                }
            }

            // if no match is found, maybe handle the case where no move is possible
            turn += 1;
            return "Turn: " + turn + " \nNo matching domino found for player #" + (currentPlayerIndex + 1);
        }

        /// <summary>
        /// Adds a domino to the board based on the given side
        /// </summary>
        /// <param name="domino"></param>
        /// <param name="side"></param>
        public bool AddDominoToBoard(Domino domino, bool side)
        {
            bool added = false;

            if (side == true)
                added = board.AddFirst(domino);
            else if (side == false)
                added = board.AddLast(domino);

            return added;
        }

        /// <summary>
        /// Returns information about the game state as a string
        /// </summary>
        public override string ToString()
        {
            return board.ToString();
        }
    }
}
