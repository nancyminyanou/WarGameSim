using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Simulation of the game war between two players.
/// Each start with a hand of 26 and play a card, higher rank takes both.
/// If cards are of equal value, War begins. Each player plays two cards 
/// with the second being compared. Player with the higher rank wins the 4 cards.
/// If there is another tie, War continues. 
/// 
/// Nancy Minyanou
/// 11/11/2016
/// 
/// TODO: For some reason, occasionally the count is off. Seems to be related to keeping track of cards after War rounds.
/// </summary>
namespace WarCardGame
{
    enum RANK
    {
        TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE
    }

    enum SUITE
    {
        DIAMONDS, SPADES, HEARTS, CLUBS
    }
    class WarCardGame
    {
        static Player user;
        static Player comp;
        static bool userWon;
        static bool compWon;

       


        static void Main(string[] args)
        {
            SetupGame();
            Console.WriteLine("Press any key to start.");
            Console.ReadKey();
            StartGame();
        }

        /// <summary>
        /// User and computer players are initialized and their decks filled.
        /// </summary>
        public static void SetupGame()
        {
            Deck mainDeck = new Deck();
            user = new Player("User");
            comp = new Player("Computer");

            // deal first round of cards
            for (var i = 0; i < 26; i++)
            {
                user.deck.AddCard(mainDeck.DrawCard());
                comp.deck.AddCard(mainDeck.DrawCard());
            }
        }

        /// <summary>
        /// Play the turns and keep tracking of whos winning.
        /// </summary>
        public static void StartGame()
        {
            userWon = false;
            compWon = false;
            int turnCount = 0;
            while (!userWon && !compWon)
            {
                PlayTurn(turnCount);
                checkWinner();
                turnCount++;
            }

            if (userWon) { Console.WriteLine("Congratulations User, you've won!"); }
            if (compWon) { Console.WriteLine("Sorry User, you've lost!"); }
            Console.WriteLine("Press any key to play again.");
            Console.ReadKey();
            SetupGame();
            StartGame();
        }

        /// <summary>
        /// Helper function to check if anyone has run out of cards or has all the cards
        /// </summary>
        private static void checkWinner()
        {
            if (user.deck.NumCards() == 52 || comp.deck.NumCards() == 0)
            {
                userWon = true;
            }
            else if (comp.deck.NumCards() == 52 || user.deck.NumCards() == 0)
            {
                compWon = true;
            }
        }

        /// <summary>
        /// Each player plays one card, if ranks are even WarTurn is called. Otherwise, higher rank takes the cards.
        /// To prevent empty queue errors, need to check if hand has more than 2 cards before starting a War.
        /// </summary>
        /// <param name="turn">Count of which turn is being played</param>
        private static void PlayTurn(int turn)
        {
            checkWinner();
            Card userPlay = new Card();
            Card compPlay = new Card();
            Console.WriteLine("Round " + turn);
            userPlay = user.deck.DrawCard();
            Console.WriteLine("User Plays: " + userPlay);
            compPlay = comp.deck.DrawCard();
            Console.WriteLine("Computer Plays: " + compPlay);

            if (userPlay.r > compPlay.r)
            {
                user.deck.AddCard(compPlay);
                user.deck.AddCard(userPlay);
                Console.WriteLine("User won round " + (turn) + "!");
                printScore();
                checkWinner();
            }
            else if (userPlay.r < compPlay.r)
            {
                comp.deck.AddCard(userPlay);
                comp.deck.AddCard(compPlay);
                Console.WriteLine("Computer won round " + (turn) + "!");
                printScore();
                checkWinner();
            }
            else
            {
                if (user.deck.NumCards() >= 2 && comp.deck.NumCards() >= 2)
                {
                    Console.WriteLine("This means War! Each player will draw two cards, only playing the second.");
                    WarTurn(userPlay, compPlay);
                }
                else
                {
                    checkWinner();
                }
            }
        }

        private static void printScore()
        {
            Console.WriteLine("The score is: \nUser: " + user.deck.NumCards() + "\nComputer: " + comp.deck.NumCards() + "\n");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userCard">user's original card</param>
        /// <param name="compCard">computer's original card</param>
        /// <param name="isDeepWar">are we more than one war deep?</param>
        /// <param name="userPlays">list of cards played in previous war round</param>
        /// <param name="compPlays"></param>
        private static void WarTurn(Card userCard, Card compCard, bool isDeepWar = false, List<Card> userPlays = null,
            List<Card> compPlays = null)
        {


            Card userPlay1 = new Card();
            Card userPlay2 = new Card();

            Card compPlay1 = new Card();
            Card compPlay2 = new Card();

            userPlay1 = user.deck.DrawCard();
            userPlay2 = user.deck.DrawCard();
            Console.WriteLine("User Plays: " + userPlay2);

            compPlay1 = comp.deck.DrawCard();
            compPlay2 = comp.deck.DrawCard();
            Console.WriteLine("Computer Plays: " + compPlay2);

            if (userPlay2.r > compPlay2.r)
            {
                Console.WriteLine("user count " + user.deck.NumCards());
                user.deck.AddCard(compCard);
                user.deck.AddCard(userCard);
                user.deck.AddCard(compPlay1);
                user.deck.AddCard(userPlay1);
                user.deck.AddCard(compPlay2);
                user.deck.AddCard(userPlay2);
                Console.WriteLine("user count " + user.deck.NumCards());
                if (isDeepWar)
                {
                    foreach (Card card in userPlays)
                    {
                        user.deck.AddCard(card);
                    }

                    foreach (Card card in compPlays)
                    {
                        user.deck.AddCard(card);
                    }
                }
                Console.WriteLine("User won this war!");
                printScore();
                checkWinner();

            }
            else if (userPlay2.r < compPlay2.r)
            {
                comp.deck.AddCard(compCard);
                comp.deck.AddCard(userCard);
                comp.deck.AddCard(userPlay1);
                comp.deck.AddCard(compPlay1);
                comp.deck.AddCard(userPlay2);
                comp.deck.AddCard(compPlay2);
                if (isDeepWar)
                {
                    foreach (Card card in userPlays)
                    {
                        comp.deck.AddCard(card);
                    }

                    foreach (Card card in compPlays)
                    {
                        comp.deck.AddCard(card);
                    }
                }
                Console.WriteLine("Computer won this war!");
                printScore();
                checkWinner();
            }
            else
            {
                if (user.deck.NumCards() >= 2 && comp.deck.NumCards() >= 2)
                {
                    Console.WriteLine("This means even MORE War! Each player will draw two cards, only playing the second.");
                    if (isDeepWar)
                    {
                        userPlays.Add(userPlay1);
                        compPlays.Add(compPlay1);
                        WarTurn(userPlay2, compPlay2, true, userPlays, compPlays);
                    }
                    else
                    {
                        userPlays = new List<Card>();
                        compPlays = new List<Card>();
                        userPlays.Add(userPlay1);
                        compPlays.Add(compPlay1);
                        WarTurn(userPlay2, compPlay2, true, userPlays, compPlays);
                    }
                }
                else
                {
                    checkWinner();
                }
            }

        }
    }

    /// <summary>
    /// Represents a single player.
    /// </summary>
    class Player
    {

        public string name { get; set; }

        public Deck deck;

        public Player(string name)
        {
            this.name = name;
            deck = new Deck(true);

        }
    }

   
    /// <summary>
    /// Represents a single playing card.
    /// </summary>
    class Card
    {

        public RANK r { get; set; }
        public SUITE s { get; set; }

        public Card() { }

        public Card(RANK rank, SUITE suite)
        {
            r = rank;
            s = suite;
        }

        public override string ToString()
        {
            return r.ToString() + " of " + s.ToString();
        }
    }

    /// <summary>
    /// Represents a deck of 52 cards. 
    /// </summary>
    class Deck
    {
        int DECK_MAX_VAL = 52;

        Queue<Card> deck;

        /// <summary>
        /// Constructor for all decks. Player decks are initialized to empty. Game decks are filled.
        /// </summary>
        /// <param name="forPlayer">Is this a player's deck?</param>
        public Deck(bool forPlayer = false)
        {
            deck = new Queue<Card>();

            if (!forPlayer)
            {
                fill();
            }
        }

        /// <summary>
        /// For initializing the game's deck
        /// </summary>
        private void fill()
        {
            int count = 0;
            foreach (SUITE s in Enum.GetValues(typeof(SUITE)))
            {
                foreach (RANK r in Enum.GetValues(typeof(RANK)))
                {
                    deck.Enqueue(new Card(r, s));
                    count++;
                }
            }

            Shuffle();
            // once more for good measure
            Shuffle();
        }

        /// <summary>
        /// Adds a card to the bottom of the deck
        /// </summary>
        /// <param name="c">Card to add</param>
        public void AddCard(Card c)
        {
            deck.Enqueue(c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Card at the top of the deck</returns>
        public Card DrawCard()
        {
            Card topCard = deck.Dequeue();

            return topCard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>number of cards in the deck</returns>
        public int NumCards()
        {
            return deck.Count();
        }

        /// <summary>
        /// Shuffle the deck by first generating numbers 0 to 51, with no repeats
        /// then use selection sort style method to swap cards
        /// </summary>
        public void Shuffle()
        {
            Card[] tempDeck = new Card[DECK_MAX_VAL];
            deck.CopyTo(tempDeck, 0);
            Random randomNum = new Random();
            int[] randomNumArr = new int[DECK_MAX_VAL];

            // fill with zeros so number generator can safely create
            // a 0 without infinite looping when checking for unique values
            for (var i = 0; i < randomNumArr.Length; i++)
            {
                randomNumArr[i] = -1;
            }

            // generate a list of unique numbers between 0 and 51
            for (var i = 0; i < DECK_MAX_VAL; i++)
            {
                int num = randomNum.Next(0, DECK_MAX_VAL);
                bool exist = Array.Exists(randomNumArr, number => number == num);
                if (!exist)
                {
                    randomNumArr[i] = num;
                }
                else
                {
                    i--;
                }
            }

            // modeling shuffling using selection sort to place
            // cards randomly accoring to list of random numbers
            for (var i = 0; i < 52; i++)
            {
                Card temp = tempDeck[i];
                tempDeck[i] = tempDeck[randomNumArr[i]];
                tempDeck[randomNumArr[i]] = temp;
            }

            // copying over shuffled deck to queue for easier management
            deck.Clear();
            for (var i = 0; i < tempDeck.Length; i++)
            {
                deck.Enqueue(tempDeck.ElementAt(i));
            }
        }

        /// <summary>
        /// Prints deck as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder deckString = new StringBuilder();

            for (var i = 0; i < deck.Count(); i++)
            {
                deckString.Append(deck.ElementAt(i) + "\n");
            }

            return deckString.ToString();
        }
    }
}
