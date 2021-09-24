using System;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
           
            char[,] startBoard = {
            { '1', '2', '3' },
            { '4', '5', '6' },
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
                    // TODO: player 1 move
                    player1Move = false;
                }
                else
                {
                    // TODO: player 2 move
                    player1Move = true;
                }
                if (gameEnded)
                    break;
            }
            
            // End the game

            Console.Clear();
            Draw(gameBoard);
            Console.Write("Game ended!");

            // TODO: print who won

        static void Draw(char[,] board)
            {
                for (int i = 0; i < board.GetLength(0); i++) // for length
                {
                    for (int j = 0; j < board.GetLength(1); i++) // for width
                        Console.Write(board[i, j]); // get element from the [i,j] - width and lenght
                    Console.WriteLine(); // print whole row [i,j] then jump to another line
                    
                    // if "writeline" every element in other line, I want whole row
                }
            }
        }
    }
}
