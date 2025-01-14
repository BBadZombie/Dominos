using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


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
                System.Diagnostics.Debug.WriteLine("Given domino: {0} does not match first domino: {1}.", domino, head);
                return false;
            }

            // domino has already been added, but this covers both cases for successful insertion
            System.Diagnostics.Debug.WriteLine("Adding domino to first");

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
                System.Diagnostics.Debug.WriteLine("Given domino: {0} does not match last domino: {1}.", domino, tail);
                return false;
            }

            System.Diagnostics.Debug.WriteLine("Adding domino to last");

            // increment the count
            Count++;
            return true;
        }

        /// <summary>
        /// Clears the board.
        /// </summary>
        public void Clear()
        {
            // because the dominoes are tied to each other, setting the head & tail
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
        /// Returns true if given dominoes are compatible, false elsewise.
        /// </summary>
        private bool Match(Domino domino1, Domino domino2)
        {
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

            // check when both dominoes are not double
            if (domino1.Top == domino2.Top ||
                domino1.Top == domino2.Bottom ||
                domino1.Bottom == domino2.Top ||
                domino1.Bottom == domino2.Bottom)
            {
                match = true;
            }

            // output statement depending on result
            if (match)
                System.Diagnostics.Debug.WriteLine("Domino 1: {0} MATCHES domino 2: {1}", domino1, domino2);
            if (!match)
                System.Diagnostics.Debug.WriteLine("Domino 1: {0} DOES NOT MATCH domino 2: {1}", domino1, domino2);

            return match;
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
