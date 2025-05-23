﻿using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.DXGI;
using Color = Microsoft.Xna.Framework.Color;

using Microsoft.Xna.Framework.Input;

namespace Dominoes
{
    /**
     * Purpose: Manage gameplay state
     * Authors: Anthony Lopez
     * Date: 1.9.25
     * Modifications: 
     *  - Modified SetPlayerTurn() method to be more modular
     *  - Rewrote get only properties to be more concise
     * Notes: 
     *  - Revisit naming convention for game end and continue method/variables
     *  - Revisit GetWinner() method to properly handle case where there is no
     *    winner and when there is a tie.
     *  - Revisit AutoPlay() method to clean up
     *  - Drawing player hands should be done in another class but im not sure which
     */

    internal class GameManager
    {
        #region Variables
        private DominoManager dominoManager;
        private PlayerManager playerManager;
        private DominoBoard board;

        // game state variables
        private int currentPlayerIndex;
        private int turn;
        private bool gameOver;
        private bool printWinner;
        private int selectedDominoIndex;
        private bool selectedSide = false;
        #endregion

        #region Properties
        public DominoManager DominoManager => dominoManager;
        public DominoBoard Board => board;
        public List<Player> PlayerList => playerManager.PlayerList;
        public bool GameOver => gameOver;
        #endregion

        #region Initialization & Game Loop
        /// <summary>
        /// Constructor for objects of class GameManager
        /// </summary>
        public GameManager()
        {
            dominoManager = new DominoManager();
            playerManager = new PlayerManager(dominoManager.GetDominoQueue());

            board = new DominoBoard();
            // starts at 2 because cow is automatically played (for now)
            turn = 2;
            gameOver = true;
            printWinner = true;
            currentPlayerIndex = getFirstPlayerIndex();
            selectedDominoIndex = 0;
            Debug.Print("" + currentPlayerIndex, Debug.Level.High);
        }

        /// <summary>
        /// Update method for class GameManager
        /// </summary>
        public void Update(GameTime gameTime)
        {
            dominoManager.Update(gameTime);
            GetPlayerInput();
        }

        /// <summary>
        /// Draw method for class GameManager
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, string output)
        {
            // temporary draw code to display Dominoes
            SpriteFont font = UI_Manager.SmallFont;

            // draw Dominoes
            // dominoManager.Draw(spriteBatch, gameTime);
            board.Draw(spriteBatch, gameTime);

            // draw player information
            spriteBatch.DrawString(font, "Players:", new Vector2(5, 5), Color.White);
            for (int i = 0; i < PlayerList.Count; i++)
            {
                string playerInfo = "#" + (i + 1) + ": " + PlayerList[i].ToString() + " ";

                // draw player information and highlight current player
                if (i == currentPlayerIndex)
                    spriteBatch.DrawString(font, playerInfo, new Vector2(5, 30 + (30 * i)), Color.DarkGreen);
                else
                    spriteBatch.DrawString(font, playerInfo, new Vector2(5, 30 + (30 * i)), Color.White);
            }

            // draw domino board information (Dominoes that are on the board)
            string boardInfo = this.ToString() + " ";
            spriteBatch.DrawString(font, "Domino Board:", new Vector2(5, 165), Color.White);
            spriteBatch.DrawString(font, boardInfo, new Vector2(5, 185), Color.White);

            // draw output from game manager
            spriteBatch.DrawString(font, output, new Vector2(5, 300), Color.White);

            DrawPlayerHand(spriteBatch, gameTime, currentPlayerIndex);
        }

        /// <summary>
        /// Checks for player input, if A or D is pressed and game end condition
        /// has not been met. 
        /// </summary>
        private string GetPlayerInput()
        {
            string output = "";

            if (InputManager.SingleKeyPress(Keys.A))
            {
                output = TestGame(true);
            }
            if (InputManager.SingleKeyPress(Keys.D))
            {
                output = TestGame(false);
            }

            if (InputManager.SingleKeyPress(Keys.Left))
            {
                SelectDomino(true);
            }
            if (InputManager.SingleKeyPress(Keys.Right))
            {
                SelectDomino(false);
            }

            return output;
        }

        /// <summary>
        /// Draws hand of the given player index
        /// </summary>
        public void DrawPlayerHand(SpriteBatch spriteBatch, GameTime gameTime, int playerIndex)
        {
            spriteBatch.DrawString(UI_Manager.SmallFont, "Player #" + (currentPlayerIndex + 1), new Vector2(450, 370), Color.White);
            PlayerList[currentPlayerIndex].DrawHand(spriteBatch, gameTime, currentPlayerIndex, selectedDominoIndex);
        }
        #endregion

        /// <summary>
        /// Temporary method that simulates a game of Dominos; used for testing purposes.
        /// </summary>
        /// <param name="side"></param>
        public string TestGame(bool side)
        {
            string output = string.Empty;
            // handle case where board is empty
            bool cowPlayed = false;

            // if no match is found, maybe handle the case where no move is possible
            gameOver = IsGameOver();

            // check if game is over
            if (!gameOver)
            {
                output = TurnInfo() + " \nGame End." + ScoreInfo();
                Debug.Print(output, Debug.Level.High);
                Game1.currentState = State.Win;
                return output;
            }

            // try to play cow domino
            cowPlayed = TryPlayCowDomino(side);
            if (cowPlayed)
            {
                IncrementPlayerTurn();
                output = $"{TurnInfo()}\nCow has been played.";
                Debug.Print(output, Debug.Level.High);
                turn++;
                return output;
            }

            if (gameOver)
            {
                // now, play the first domino that matches the right side of the board
                IncrementPlayerTurn();
                output = AutoPlayDomino(side);
                // output = ManuallyPlayDomino(side);
            }

            // automatically adjust domino index for current players hand
            // TODO: address repeated code
            int prevHandCount = PlayerList[currentPlayerIndex].Hand.Count;

            if (selectedDominoIndex >= prevHandCount)
                selectedDominoIndex = prevHandCount - 1;

            turn++;

            if (!string.IsNullOrEmpty(output))
                return output;

            output = TurnInfo() + " \nNo matching domino found for this player";
            Debug.Print(output, Debug.Level.High);
            return output;
        }

        // TODO: comback to and refactor so turns where a player cannot play
        // are still incremented and printing to debug console
        public string AutoPlayDomino(bool side)
        {
            string output = string.Empty;
            bool played = TryPlayDominoFromHand(PlayerList[currentPlayerIndex], side);

            if (played)
                output = $"{TurnInfo()}\nSuccessful play";
            else
                output = $"{TurnInfo()}\nTurn skipped.";

            Debug.Print(output, Debug.Level.High);

            return output;
        }

        public string ManuallyPlayDomino(bool side)
        {
            string output = string.Empty;
            Player previousPlayer = PlayerList[GetPreviousPlayerIndex()];
            bool played = PlayGivenDomino(previousPlayer, side, selectedDominoIndex);

            if (played)
                output = $"{TurnInfo()}\nSuccessful play";
            else
                output = $"{TurnInfo()}\nTurn skipped.";

            Debug.Print(output, Debug.Level.High);

            return output;
        }

        public bool TryPlayDominoFromHand(Player player, bool side)
        {
            List<Domino> currentHand = player.Hand;

            foreach (var domino in currentHand)
            {
                bool canPlay = side ? board.IsHeadPlayable(domino) : board.IsTailPlayable(domino);

                if (canPlay)
                {
                    bool dominoAdded = AddDominoToBoard(domino, side);
                    if (dominoAdded)
                    {
                        currentHand.Remove(domino);
                        return true;
                    }
                }
            }

            return false;
        }

        // takes an index for a specific domino to play
        // TODO: address repeated code from select domino index
        public bool PlayGivenDomino(Player player, bool side, int dominoIndex)
        {
            List<Domino> currentHand = player.Hand;
            Domino domino = currentHand[dominoIndex];

            Debug.Print("Given Domino Index: " + dominoIndex, Debug.Level.High);
            Debug.Print("Current Hand Count: " + currentHand.Count, Debug.Level.High);

            bool canPlay = side ? board.IsHeadPlayable(domino) : board.IsTailPlayable(domino);

            if (canPlay)
            {
                bool dominoAdded = AddDominoToBoard(domino, side);
                if (dominoAdded)
                {
                    currentHand.Remove(domino);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Moves on to the next players turn. Also sets all the dominos in the 
        /// current players hand to visible, and all the previous players dominos
        /// isVisible to false;
        /// </summary>
        private void IncrementPlayerTurn()
        {
            // previous player
            foreach (Domino domino in PlayerList[currentPlayerIndex].Hand)
            {
                domino.IsVisible = false;
            }

            currentPlayerIndex = (currentPlayerIndex + 1) % PlayerList.Count;

            // current player
            foreach (Domino domino in PlayerList[currentPlayerIndex].Hand)
            {
                domino.IsVisible = true;
            }

            Debug.Print("Player Turn Incremented: " + currentPlayerIndex, Debug.Level.High);
        }

        public void SelectDomino(bool direction)
        {
            int prevHandCount = PlayerList[currentPlayerIndex].Hand.Count;

            if (selectedDominoIndex >= prevHandCount)
            {
                selectedDominoIndex = prevHandCount - 1;
            }
            else if (!direction && selectedDominoIndex < prevHandCount)
            {
                selectedDominoIndex = (selectedDominoIndex + 1) % prevHandCount;
            }
            else if (direction && selectedDominoIndex > 0)
            {
                selectedDominoIndex -= 1;
            }

            Debug.Print("Domino Index: " + selectedDominoIndex, Debug.Level.High);
        }

        /// <summary>
        /// Returns the number of the winning player. If there is no
        /// winning player, returns -1.
        /// </summary>
        public int GetWinningPlayer()
        {
            // for each player
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].Hand.Count <= 0)
                {
                    if (printWinner)
                    {
                        string output = "Player " + (i + 1) + " Wins.";
                        Debug.Print(output, Debug.Level.High);
                        printWinner = true;
                    }

                    return i + 1;
                }
            }

            if (printWinner && !gameOver)
                Debug.Print("No Winning Player.", Debug.Level.Low);

            return -1;
        }

        // TODO: replace instances where a domino needs to be automatically played
        // with this method
        public bool PlayDominoFromHand(Player player, bool side)
        {
            foreach (var domino in player.Hand)
            {
                if ((side ? board.IsHeadPlayable(domino) : board.IsTailPlayable(domino)) && AddDominoToBoard(domino, side))
                {
                    player.Hand.Remove(domino);
                    return true;
                }
            }

            return false;
        }

        private bool TryPlayCowDomino(bool side)
        {
            foreach (var player in PlayerList)
            {
                Domino cow = player.HasCow();
                if (cow != null)
                {
                    AddDominoToBoard(player.PlayDomino(cow), side);
                    // set current player index to player that comes after cow player
                    // currentPlayerIndex = (PlayerList.IndexOf(player) + 1) % PlayerList.Count;
                    return true;
                }
            }

            return false;
        }

        // finds which player has the 6 | 6 domino and sets currentPlayerIndex to that players index
        // returns -1 if something goes wrong
        private int getFirstPlayerIndex()
        {
            foreach (var player in PlayerList)
            {
                Domino cow = player.HasCow();
                if (cow != null)
                {
                    // set current player index to player that comes after cow player
                    return PlayerList.IndexOf(player);
                }
            }

            return 0;
        }

        private int GetPreviousPlayerIndex()
        {
            return (currentPlayerIndex == 0) ? 3 : currentPlayerIndex - 1;
        }

        /// <summary>
        /// Returns a boolean indicating whether or not the game can continue.
        /// If a player has won, or the board is unplayable, the game cannot 
        /// continue, and returns false. Elsewise, return true.
        /// </summary>
        private bool IsGameOver()
        {
            bool result = true;
            string output = string.Empty;

            // if a player has won or the board is unplayable, return
            // true because the game end condition has been met
            if (IsBoardPlayable() == false)
            {
                output = "Game end condition has been met.";
                result = false;
            }
            else if (PlayerWon() == true)
            {
                output = "Game end condition has been met.";
                result = false;
            }

            if (!string.IsNullOrEmpty(output))
                Debug.Print(output, Debug.Level.High);

            return result;
        }

        /// <summary>
        /// Returns true if a player has won (has no more dominos), and false elsewise
        /// </summary>
        public bool PlayerWon()
        {
            foreach (var player in PlayerList)
            {
                if (player.Hand.Count == 0)
                {
                    if (printWinner)
                    {
                        printWinner = false;
                        Debug.Print($"Player {PlayerList.IndexOf(player) + 1} Wins.", Debug.Level.Medium);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if the game can still be played, and false elsewise
        /// </summary>
        public bool IsBoardPlayable()
        {
            if (turn <= 2)
                return true;

            foreach (var player in PlayerList)
            {
                foreach (var domino in player.Hand)
                {
                    if (board.IsPlayable(domino))
                        return true;
                }
            }

            Debug.Print("Board is not playable.", Debug.Level.High);
            return false;
        }

        /// <summary>
        /// Adds a domino to the board based on the given side
        /// </summary>
        /// <param name="domino"></param>
        /// <param name="side"></param>
        public bool AddDominoToBoard(Domino domino, bool side)
        {
            domino.IsVisible = true;
            return side ? board.AddFirst(domino) : board.AddLast(domino);
        }

        #region Information ToString
        /// <summary>
        /// Returns the final score of all players
        /// </summary>
        public string ScoreInfo()
        {
            string output = string.Empty;
            int winningPlayerIndex = GetWinningPlayer();

            if (winningPlayerIndex >= 0)
                output += "\nPlayer " + winningPlayerIndex + " Wins.";

            for (int i = 0; i < PlayerList.Count; i++)
            {
                Player currentPlayer = PlayerList[i];

                output += "\nPlayer " + (i + 1) + " Score: " + currentPlayer.TotalScore();
            }

            return output;
        }

        /// <summary>
        /// Returns information about the current turn as a string
        /// </summary>
        public string TurnInfo()
        {
            return "\nTurn " + turn + ": Player " + (currentPlayerIndex + 1);
        }

        /// <summary>
        /// Returns information about the game state as a string
        /// </summary>
        public override string ToString()
        {
            return board.ToString();
        }
        #endregion
    }
}
