using System;
using System.Threading; // for thread sleep

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {

            // Create two players

            HumanPlayer p1 = new HumanPlayer();
            ComputerPlayer p2 = new ComputerPlayer();

            p1.Name = "User";
            p1.Symbol = 'X';

            p2.Name = "AI";
            p2.Symbol = 'O';

            // Create two boards - with starting and current fields

            char[,] startBoard = {
            { '1', '2', '3' }, // only i row
            { '4', '5', '6' }, // now another row in j, so there it is [,]
            { '7', '8', '9' }
            };

            char[,] gameBoard = startBoard.Clone() as char[,];

            // Flags

            bool gameEnded = false;
            bool player1Move = true; // true - player 1 move, false - player 2 move

            // Loop over rounds

            for (int round = 0; round < gameBoard.Length; round++)
            {
                Console.Clear();
                Draw(gameBoard);

                if (player1Move)
                {
                    Console.WriteLine(p1.Name + " turn");
                    gameEnded = p1.MakeMove(startBoard, gameBoard);
                    player1Move = false; // after completed move, changing to player2
                }
                else
                {
                    Console.WriteLine(p2.Name + " turn");
                    gameEnded = p2.MakeMove(startBoard, gameBoard);
                    player1Move = true; // after completed move, changing to player1 - back to if (player1Move)
                }
                if (gameEnded)
                    break;
            }

            // End the game

            Console.Clear();
            Draw(gameBoard);
            Console.Write("Game ended!");

            if (gameEnded)
            {
                if (player1Move)
                    Console.WriteLine(p2.Name + " won!");
                else
                    Console.WriteLine(p1.Name + " won!");
            }
            else
                Console.WriteLine("Draw!");
            
        }

        static void Draw(char[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++) // for height, always starting from 0 (0)
            {
                for (int j = 0; j < board.GetLength(1); j++) // for width
                    Console.Write(board[i, j]); // get element from the [i,j] - width and lenght
                Console.WriteLine(); // print whole row [i,j] then jump to another line

                // if "writeline" every element in other line, I want whole row
            }
        }
    }
    interface IMoving // interface starts with "I" on the beginning
    {
        bool MakeMove(char[,] startBoard, char[,] gameBoard);
    }

    abstract class Player // abstract means that we cannot make object to this class, share part for AI and player
    {
        public string Name { get; set; }
        public char Symbol { get; set; } // X or O

        public bool CheckIfPlayerWon (char[,] gameBoard) // checking for XXX or OOO in one row/line/diagonally
        {
            int height = gameBoard.GetLength(0);
            int width = gameBoard.GetLength(1); // to save some time, two variables
            if (height != width)
                throw new Exception("The board is not a square!"); // our board is a square, it is just protection if whatever happens

            // Check rows

            for (int i = 0; i < height; i++)
            {
                int rowSum = 0; // starting from 0
                for (int j = 0; j < width; j++)
                {
                    if (gameBoard[i, j] == Symbol)
                        rowSum++;
                }
                if (rowSum == width) // could also say 3, but we stay to width/height just like in i < board.GetLength 
                    return true; // return finish the method, executes only rowSum == width
            }

            // Check colums

            for (int j = 0; j < width; j++) // starting with j height because of checkmate in columns
            {
                int colSum = 0;
                for (int i = 0; i < height; i++)
                {
                    if (gameBoard[i, j] == Symbol)
                        colSum++;
                }
                if (colSum == height)
                    return true;
            }

            // Check diagonals

            int diagSumA = 0;
            int diagSumB = 0;
            for (int k = 0; k < width; k++)
            {
                if (gameBoard[k, k] == Symbol) // for [0,0] [1,1] [2,2]
                    diagSumA++;
                if (gameBoard[k, width - 1 - k] == Symbol) // for [0,2] [1,1] [2,0] 
                    diagSumB++; // numbers on table are 0,1,2 width is 3 so: width - 1 and also k \\\ e.g k=0 [k, 2-k] 
            }
            if (diagSumA == width || diagSumB == width) // || means or
                return true;

            // otherwise, not win yet

            return false;
        }

        public bool PlaceSymbol (char c, char[,] startBoard, char[,] gameBoard) // char c number on board with symbol
        {
            int height = gameBoard.GetLength(0);
            int width = gameBoard.GetLength(1);

            if (height != startBoard.GetLength(0) || width != startBoard.GetLength(1))
                throw new Exception("Board is not a square!");

            for (int i = 0; i < height; i++) // checking for symbol, also if is not placed yet in the same spot, to prevent double marking on the same cordinates (cheating)
                for (int j = 0; j < width; j++)                
                    if (gameBoard[i, j] == c && gameBoard[i, j] == startBoard[i, j] ) // && means and
                    {
                        gameBoard[i, j] = Symbol; // placin our symbol on the board
                        return true;
                    }

            // Otherwise, return without success // if player didnt place symbol on the board or on available place (game end?)

            return false;
            
        }
    }

    class HumanPlayer : Player, IMoving // inherit after class Player and implement interface IMoving 
    {
        public bool MakeMove(char[,] startBoard, char[,] gameBoard)
        {
            // Ask human player to enter a place untihl he picks an available one, do it until result
            
            char chosenPlace;

            do // loop do, always do first, then thinks and do it again
            {
                Console.Write("Choose an empty place: ");
                chosenPlace = Console.ReadKey().KeyChar;
                Console.WriteLine();
            }
            while (!PlaceSymbol(chosenPlace, startBoard, gameBoard)); // can plice to "while" because it returns true or false and move to further code, we placed method!
            
            // ! means negation like minus // so while is false and it breaks the loop, thats good player wont place symbol twice in a row
            // it depends on first value - true or false - then there is negation
            
            return CheckIfPlayerWon(gameBoard);            
        }
    }
    class ComputerPlayer : Player, IMoving // inherit after class Player and implement interface IMoving 
    {
        public bool MakeMove(char[,] startBoard, char[,] gameBoard)
        {
            Random rnd = new Random();
            char chosenPlace;

            do
            {
                int p = rnd.Next(1, gameBoard.Length + 1); // random 1-9
                chosenPlace = p.ToString()[0]; // convert digit to char, [0] first char of array
            }
            while (!PlaceSymbol(chosenPlace, startBoard, gameBoard));

            Thread.Sleep(2000); // wait 2 seconds for computer move         

            return CheckIfPlayerWon(gameBoard);            
        }
    }
}

