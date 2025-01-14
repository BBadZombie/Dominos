using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        LinkedList<Domino> dominoBoard;

        int currentPlayerIndex;

        public List<Domino> DominoBoard
        {
            get { return dominoBoard.ToList(); }
        }

        public List<Player> PlayerList
        {
            get { return playerManager.PlayerList; }
        }

        public GameManager()
        {
            // initialize managers
            dominoManager = new DominoManager();
            playerManager = new PlayerManager(dominoManager.GetDominoList());

            // initialize board
            dominoBoard = new LinkedList<Domino>();
        }

        // adds domino to board while following game rules
        public void AddDominoToBoard(Domino domino)
        {
            dominoBoard.AddLast(domino);
        }

        // tests game logic by randomly playing
        public string TestGame()
        {
            // handle case where board is empty
            bool cowPlayed = false;
            for (int i = 0; i < playerManager.PlayerList.Count; i++)
            {
                // Print player hand
                for (int j = 0; j < playerManager.PlayerList[i].PlayerHand.Count; j++)
                {
                    Domino cow = playerManager.PlayerList[i].HasCow();

                    if (cow != null)
                    {
                        // Add domino to board and remove it from player hand
                        dominoBoard.AddLast(playerManager.PlayerList[i].PlayDomino(cow));

                        // Set the first player index
                        currentPlayerIndex = i;
                        cowPlayed = true;
                        return "Cow has been played"; // Exit once cow is played
                    }
                }

                if (cowPlayed) break; // Exit outer loop once cow is played
            }

            // Make sure player index is correct
            currentPlayerIndex = (currentPlayerIndex == 3) ? 0 : currentPlayerIndex + 1;

            // Now, try to play a domino that matches the right side of the board
            List<Domino> currentPlayerHand = playerManager.PlayerList[currentPlayerIndex].PlayerHand;
            for (int i = 0; i < currentPlayerHand.Count; i++)
            {
                if (dominoBoard.Count > 0 && (
                    currentPlayerHand[i].Top == dominoBoard.Last.Value.Top ||
                    currentPlayerHand[i].Top == dominoBoard.Last.Value.Bottom ||
                    currentPlayerHand[i].Bottom == dominoBoard.Last.Value.Top ||
                    currentPlayerHand[i].Bottom == dominoBoard.Last.Value.Bottom))
                {
                    dominoBoard.AddLast(currentPlayerHand[i]);
                    currentPlayerHand.RemoveAt(i); // Remove played domino from hand
                    return "Successful play";
                }
            }

            // If no match is found, maybe handle the case where no move is possible
            return "No matching domino found for player #" + (currentPlayerIndex + 1);
        }

        public string GetDominoBoard()
        {
            string playerInfo = string.Empty;

            foreach (Domino domino in dominoBoard)
            {
                playerInfo += domino.ToString() + ", ";
            }

            return playerInfo;
        }
    }
}
