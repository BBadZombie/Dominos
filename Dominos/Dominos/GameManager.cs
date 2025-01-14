using System;
using System.Collections.Generic;
using System.Drawing;
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

        // update method
        public void Update(GameTime gameTime)
        {
            dominoManager.Update(gameTime);
        }

        /// <summary>
        /// Temporary method that simulates a game of Dominoes; used for testing purposes. 
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
                    return TurnInfo() + " \nCow has been played";
            }

            // make sure player index is correct
            currentPlayerIndex = (currentPlayerIndex == 3) ? 0 : currentPlayerIndex + 1;

            // now, play the first domino that matches the right side of the board
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
                            return TurnInfo() + "\nSuccessful play";
                        }
                    }
                    else if (!side)
                    {
                        if (board.IsTailPlayable(currentPlayerHand[i]) && AddDominoToBoard(currentPlayerHand[i], side))
                        {
                            currentPlayerHand.RemoveAt(i); // remove played domino from hand
                            turn += 1;
                            return TurnInfo() + " \nSuccessful play";
                        }
                    }
                }
            }

            // if no match is found, maybe handle the case where no move is possible
            turn += 1;

            if (!GameEnd())
                return TurnInfo() + " \nGame End" + ScoreInfo();

            return TurnInfo() + " \nNo matching domino found for this player";
        }

        /// <summary>
        /// Returns a boolean indicating whether or not the game can continue
        /// TODO: Come back to this method and clean up
        /// </summary>
        public bool GameEnd()
        {
            bool gameContinue = false;

            // for each player
            for (int i = 0; i < playerManager.PlayerList.Count; i++)
            {
                Player currentPlayer = playerManager.PlayerList[i];

                if (currentPlayer.PlayerHand.Count <= 0)
                    return gameContinue;

                for (int j = 0; j < currentPlayer.PlayerHand.Count; j++)
                {
                    if (board.IsHeadPlayable(currentPlayer.PlayerHand[j]) || board.IsTailPlayable(currentPlayer.PlayerHand[j]))
                    {
                        gameContinue = true;
                        // return here since there are no more checks necessary
                        return gameContinue;
                    }
                }
            }

            return gameContinue;
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
        /// Returns the final score of all players
        /// </summary>
        public string ScoreInfo()
        {
            string output = "";

            for (int i = 0; i < playerManager.PlayerList.Count; i++)
            {
                Player currentPlayer = playerManager.PlayerList[i];

                output += "\nPlayer #" + (i + 1) + " Score: " + currentPlayer.TotalScore();
            }

            return output;
        }

        /// <summary>
        /// Returns information about the current turn
        /// </summary>
        public string TurnInfo()
        {
            return "Turn: " + turn + "\nPlayer #" + (currentPlayerIndex + 1);
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
