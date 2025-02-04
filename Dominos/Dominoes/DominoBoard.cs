using System;
using System.Collections.Generic;
// using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System.DirectoryServices.ActiveDirectory;


namespace Dominoes
{
    /**
     * Purpose: Manage domino board
     * Authors: Anthony Lopez
     * Date: 12.23.24
     * Modifications: 
     * Notes: 
     */

    internal class DominoBoard
    {
        // variable declarations
        private Domino head = null;
        private Domino tail = null;

        // properties
        public Domino Head => head;

        public Domino Tail => tail;

        public int Count { get; private set; }

        /// <summary>
        /// Indexer property for the board that gets data in the node 
        /// at a specific index. If the index is invalid, throws an 
        /// IndexOutOfRangeException exception.
        /// </summary>
        public Domino this[int index]
        {
            get
            {
                // make sure index is within range
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();

                // declare current card and then move onto see if its the head or tail of deck
                Domino currentDomino;

                // determine if the given index is closer to head or tail
                if (index < Count / 2)
                {
                    // we can set the current card to head even if theres a null warning because we know 
                    // the index is within range, so there must be a head/tail
                    currentDomino = head;

                    // iterate at start of list
                    for (int i = 0; i < index; i++)
                    {
                        // null check
                        if (currentDomino != null)
                            currentDomino = currentDomino.Next;
                    }
                }
                else
                {
                    currentDomino = tail;

                    // iterate at bottom of list
                    for (int i = Count - 1; i > index; i--)
                    {
                        // null check
                        if (currentDomino != null)
                            currentDomino = currentDomino.Previous;
                    }
                }

                return currentDomino;
            }
        }

        /// <summary>
        /// Constructor for objects of class DominoBoard
        /// </summary>
        public DominoBoard()
        {

        }

        /// <summary>
        /// Draw method for class DominoBoard
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // draw Dominoes
            Texture2D texture = UI_Manager.GetTexture("scaled_" + 1 + " | " + 1);
            int x = 450;
            int y = 150;

            for (int i = 0; i < Count; i++)
            {
                // this condition could also be true if the dominos previous field is not null
                if (i >= 1)
                {
                    // this code determines where domino should be drawn
                    if (!this[i].IsDouble() && !this[i - 1].IsDouble()) // current and previous are not doubles
                    {
                        x = this[i - 1].Position.Right + texture.Width;
                    }
                    else if (this[i].IsDouble() && !this[i - 1].IsDouble()) // current is double previous is not
                    {
                        x = this[i - 1].Position.Right + texture.Width / 2;
                    }
                    else if (!this[i].IsDouble() && this[i - 1].IsDouble()) // current is not double previous is
                    {
                        x = this[i - 1].Position.Right + texture.Width / 2;
                    }
                }

                if (x > 950)
                {
                    x = 450;
                    y += texture.Height + 5;
                }

                // this code determines how to rotate based off if the domino is a double or not
                if (!this[i].IsDouble())
                {
                    // x += texture.Width * 2;
                    this[i].Draw(spriteBatch, x, y, MathHelper.ToRadians(90));
                }
                else
                {
                    // x += (int) (texture.Width * 1.5f);
                    this[i].Draw(spriteBatch, x, y, 0f);
                }
            }
        }

        /// <summary>
        /// Adds a given domino to the left side of the board. 
        /// </summary>
        public bool AddFirst(Domino domino)
        {
            // if the deck is empty
            if (head == null)
            {
                // set the new card to both the head and the tail
                head = domino;
                tail = domino;
            }
            else if (MatchesHead(domino))
            {
                // set the tail to the new card
                domino.Next = head;
                // set the tail to the new cards previous
                head.Previous = domino;
                // update the decks new tail as the card to add
                head = domino;
            }
            else
            {
                Debug.Print("Given domino: " + domino + " does not match first domino: " + head + ".", Debug.Level.Low);
                return false;
            }

            // domino has already been added, but this covers both cases for successful insertion
            Debug.Print("Adding domino to first.", Debug.Level.Low);

            // increment the count
            Count++;
            return true;
        }

        /// <summary>
        /// Adds a given domino to the right side of the board. 
        /// </summary>
        public bool AddLast(Domino domino)
        {
            // if the deck is empty
            if (tail == null)
            {
                // set the new card to both the head and the tail
                head = domino;
                tail = domino;
            }
            else if (MatchesTail(domino))
            {
                // set the tail to the new card
                tail.Next = domino;
                // set the tail to the new cards previous
                domino.Previous = tail;
                // update the decks new tail as the card to add
                tail = domino;
            }
            else
            {
                Debug.Print("Given domino: " + domino + " does not match last domino: " + tail + ".", Debug.Level.Low);
                return false;
            }

            Debug.Print("Adding domino to Last.", Debug.Level.Low);

            // increment the count
            Count++;
            return true;
        }

        /// <summary>
        /// Clears the board.
        /// </summary>
        public void Clear()
        {
            // because the Dominoes are tied to each other, setting the head & tail
            // to null deletes all their neighbor nodes. also reset the count back to 0

            head = null;
            tail = null;
            Count = 0;
        }

        /// <summary>
        /// Returns a boolean indicating if the given domino matches 
        /// with the  (rightmost domino) of the board.
        /// </summary>
        public bool MatchesHead(Domino domino)
        {
            return Match(domino, head);
        }

        /// <summary>
        /// Returns a boolean indicating if the given domino matches 
        /// with the tail (lefttmost domino) of the board.
        /// </summary>
        public bool MatchesTail(Domino domino)
        {
            return Match(domino, tail);
        }

        /// <summary>
        /// Returns true if given Dominoes are compatible, false elsewise.
        /// Also checks to make sure given dominos arent null.
        /// </summary>
        private bool Match(Domino domino1, Domino domino2)
        {
            if (domino1 == null || domino2 == null)
                throw new NullReferenceException("Domino cannot be null");

            bool match = false;

            // if domino 1 is a double
            if (domino1.IsDouble())
            {
                if (domino1.Top == domino2.Top || domino1.Top == domino2.Bottom)
                    match = true;
            }
            // if domino 2 is a double
            if (domino2.IsDouble())
            {
                if (domino2.Top == domino1.Top || domino2.Top == domino1.Bottom)
                    match = true;
            }

            // check when both Dominoes are not double
            if (domino1.Top == domino2.Top ||
                domino1.Top == domino2.Bottom ||
                domino1.Bottom == domino2.Top ||
                domino1.Bottom == domino2.Bottom)
            {
                match = true;
            }

            // output statement depending on result
            if (match)
                Debug.Print("Given dominos match.", Debug.Level.Low);
            if (!match)
                Debug.Print("Given dominos do not match.", Debug.Level.Low);

            return match;
        }

        /// <summary>
        /// Returns true if given domino can be played on first or last domino.
        /// </summary>
        public bool IsPlayable(Domino domino)
        {
            return (IsHeadPlayable(domino) || IsTailPlayable(domino)) ? true : false;
        }

        /// <summary>
        /// Returns true if given domino can be played on first domino.
        /// </summary>
        public bool IsHeadPlayable(Domino domino)
        {
            if (MatchesHead(domino))
            {
                // if the matching side isnt taken, return true
                if (domino.GetMatchingSide(head) != domino.GetMatchingSide(head.Next))
                    return true;
                // matching with a double always returns true
                if (head.IsDouble())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if given domino can be played on last domino.
        /// </summary>
        public bool IsTailPlayable(Domino domino)
        {
            if (MatchesTail(domino))
            {
                // if the matching side isnt taken, return true
                if (domino.GetMatchingSide(tail) != domino.GetMatchingSide(tail.Previous))
                    return true;
                // matching with a double always returns true
                if (tail.IsDouble())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns this domino board as a list.
        /// </summary>
        public List<Domino> ToList()
        {
            List<Domino> dominoList = new List<Domino>();
            Domino currentDomino = head;

            while (currentDomino != null)
            {
                dominoList.Add(currentDomino);
                currentDomino = currentDomino.Next;
            }

            return dominoList;
        }

        /// <summary>
        /// Returns information about this domino board as a string
        /// </summary>
        public override string ToString()
        {
            string boardInfo = string.Empty;
            Domino currentDomino = head;
            int tracker = 0;

            while (currentDomino != null)
            {
                boardInfo += currentDomino.ToString();

                if (tracker == 4)
                {
                    boardInfo += "\n";
                    tracker = 0;
                }
                else
                {
                    boardInfo += ", ";
                    tracker += 1;
                }

                currentDomino = currentDomino.Next;
            }

            return boardInfo;
        }
    }
}
