using System;

namespace NoughtsAndCrosses
{
    class Program
    {
        static void Main(string[] args)
        {

            int[,] board = new int[3, 3]
            {
                {1, 2, 3 },
                {4, 5, 6 },
                {7 ,8, 9 }
            };

            string[,] playerPostitions = new string[3, 3]
            {
                {null, null, null },
                {null, null, null },
                {null, null, null }
            };

            bool player1Turn = true;
            bool playerWon = false;
            Boolean gameOver = false;

            do
            {
                PrintBoard(board, playerPostitions);
                int[] chosenPosition = TakePlayerInput(player1Turn ? 1 : 2, board, playerPostitions);
                // now update the position
                playerPostitions = MarkPlayerPosition(player1Turn, playerPostitions, chosenPosition);


                // now check if we have won the game (or the unlikely event that no one won)
                bool[] gameReport = IsGameOver(playerPostitions, player1Turn);

                if (gameReport[0])
                {
                    gameOver = true;
                    playerWon = gameReport[1];

                }
                else
                {
                    player1Turn = !player1Turn;
                }


            } while (!gameOver);

            PrintBoard(board, playerPostitions);

            Console.Write("Game Over!");

            if (playerWon)
            {
                Console.Write($" Player: {(player1Turn ? "1" : "2") } was the winner.");
            }
        }

        static bool[] IsGameOver(string[,] positions, bool lastPlayerWas1)
        {
            // possible winning positions [1,2,3], [4, 5, 6], [7, 8, 9]
            // vertical [1, 4, 7], [2, 5, 8] , [3, 6, 9]
            // diagonal [1, 5, 9], [3, 5, 7]

            string playerChar = lastPlayerWas1 ? "O" : "X";

            int[,,] winningPositions = new int[8, 3, 2]
            {
                {{0,0 }, {0,1 }, {0,2 }},
                {{1,0 }, {1,1 }, {1, 2} },
                {{2,0 }, {2,1 }, {2,2 }},
                {{0,0 }, {1,0 }, {2,0 }},
                {{0,1 }, {1,1 }, {2,1 }},
                {{0,2 }, {1, 2 }, {2,2 }},
                {{0,0 }, {1,1 }, {2,2 }},
                {{0,2 }, {1,1 }, {2,0 }}
            };

            for (int i = 0; i < winningPositions.GetLength(0); i++)
            {
                // check that we have the same string across all three positions
                bool match = true;
                for (int j = 0; j < winningPositions.GetLength(1); j++)
                {
                    if (
                        positions[winningPositions[i, j, 0], winningPositions[i, j, 1]] != playerChar
                        )
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    bool[] report = new bool[] { true, true };
                    return report;
                }
            }

            // now check if all positions are filled

            for (int i = 0; i < positions.GetLength(0); i++)
            {
                for (int j = 0; j < positions.GetLength(1); j++)
                {
                    if (positions[i, j] == null)
                    {
                        bool[] reportContinue = new bool[] { false, true };
                        return reportContinue;
                    }
                }
            }


            bool[] reportFail = new bool[] { false, false };
            return reportFail;
        }

        static string[,] MarkPlayerPosition(bool player1, string[,] positions, int[] chosenPosition)
        {

            if (player1)
            {
                positions[chosenPosition[0], chosenPosition[1]] = "O";
            }
            else
            {
                positions[chosenPosition[0], chosenPosition[1]] = "X";
            }

            return positions;
        }

        static int[] TakePlayerInput(int currentPlayer, int[,] board, string[,] positions)
        {

            bool validInt = false;
            int chosenPosition;
            int[] position = new int[] { 0, 0 };

            do
            {
                Console.WriteLine($"Player {currentPlayer} turn:");
                string input = Console.ReadLine();

                validInt = int.TryParse(input, out chosenPosition);

                if (validInt)
                {
                    // now attempt to find the position on the board
                    position = FindPositionOnBoard(chosenPosition, board);

                    // now check if the position is occupied
                    if (positions[position[0], position[1]] != null)
                    {
                        Console.WriteLine("Position Already Taken!");
                        validInt = false;
                    }

                }
                else
                {
                    Console.WriteLine("Not a valid Position!");
                }

            } while (!validInt);

            return position;
        }

        static int[] FindPositionOnBoard(int position, int[,] board)
        {

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == position)
                    {
                        int[] pos = new int[] { i, j };
                        return pos;
                    }
                }
            }

            return null;
        }

        static void PrintBoard(int[,] board, string[,] positions)
        {
            Console.Clear();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (positions[i, j] != null)
                    {
                        Console.Write($" {positions[i, j]} ");
                    }
                    else
                    {
                        Console.Write($" {board[i, j]} ");
                    }

                    if (j < board.GetLength(1) - 1)
                    {
                        Console.Write(" | ");
                    }
                }
                Console.Write("\n");
                if (i < board.GetLength(0) - 1)
                {
                    Console.WriteLine("----------------");
                }
            }
        }
    }
}
