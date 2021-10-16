using System;

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

            // TODO: print who won
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
    }

    class HumanPlayer : Player, IMoving // inherit after class Player and implement interface IMoving 
    {
        public bool MakeMove(char[,] startBoard, char[,] gameBoard)
        {
            // TODO: human move         
            bool result = CheckIfPlayerWon(gameBoard);
            return result;
        }
    }
    class ComputerPlayer : Player, IMoving // inherit after class Player and implement interface IMoving 
    {
        public bool MakeMove(char[,] startBoard, char[,] gameBoard)
        {
            // TODO: computer move

            bool result = CheckIfPlayerWon(gameBoard);
            return result;
        }
    }
}

