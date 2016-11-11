# War: The Card Game

## Rules
Though there are many ways to play this game, the following outline uses the following rules assuming that there are 2 players:

1. A deck of 52 cards is shuffled and each player is dealt a card until the starting deck is depleated.
2. Each player puts down a card per turn, the player with the highest card value (by rank, suites are ignored, aces high) takes the played cards to add to the bottom of their playing deck.    
  * If the cards are of equal value, a War begins.     
  * Each player will put down 2 cards in each turn of War. The first is face down and the second is face up. The player with the higher card value takes all 4 cards to add to the bottom of their deck.    
  * If the cards are of the same value, the round is repeated with two more cards until there is a winner.
3. The winner is the first to accumulate 52 cards

## The Objects
To accommodate all the peices of the game, they can be broken down by the major components and their associated properties assuming there is one computer player against a computer.

### Card
The fundamental unit of the game is a playing card. Each card consists of a rank and suite. Getters/setters manage these properties. 

### Deck
A deck is a queue of cards. Since the playstyle of the game is to add cards to the bottom and remove them from the top, enqeue/dequeue seemed logical. Several functions live here:

1. Fill() - enqueues one type of each card then calls shuffle() to use selection-sort inspiried methodology to rearrange all the cards in the deck based on a list of randomized numbers from 0 to 51 generated in an array to prevent duplicate cards from appearing in a single deck. 
2. AddCard()
3. DrawCard()
4. NumCards()
5. Shuffle()

An important note is that within the constructor these is simple logic to either fill or not fill the deck. Players should start wtih an empty deck as the game will populate them from a master deck at the begeinning. 

### Player
A player object is just a way to manage decks with a name as opposed to managing decks directly. 

## The Action
Here's the exciting bit, gameflow is as follows:

1. SetupGame() - initializes a master deck that is shuffled, then proceedes to deal a card to each player
2. StartGame() - begin keeping track of each turn of the game, checking if there is a winner after every turn
3. PlayTurn() - the bulk of the game logic lives here. After draws from each player in one turn, points are assigned based on rank of the cards played. In the event of War...
  * WarTurn() - on the first pass, War looks like a regular turn where instead of one card, two are drawn and the sceond is played. If there happens to be another tie, WarTurn() is called again. However, to keep track of the past plays within the same round of War, a list of played cards are also passed in as optional parameters. 

All of this takes place within the console and just serves as a demonstration, printing out each turn with scores and declaring a winner after all the cards are played without user interaction. 

_Note_: There appears to be an issue with how cards are being counted after a WarTurn() where multiples of 2 are "lost" in the round despite them existing within the deck when printed to console. Since the game does check if enough cards are avaiable to play another round or play a round of War, it does typically break the game. Needs further investigation.
