/*
// TO DO:
//
// Be able to flag on numbers, and if correct amount of open spaces around, flag tiles around it
//
// Add command to quit board
//
*/

//hard mode is 21,36,100
namespace Minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {   
            
        Console.WriteLine("Press any key to Start the Game!");
        Console.ReadKey();
        Console.Clear();
            while (BoardPicker(out Board board))
            {
                board.BoardMaker();
                Console.Clear();

                while (board.Revealed == false)
                {
                    board.DrawBoard();
                    DetectCommand(board);
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.Clear();
                }

            }
            Console.Clear();
            Console.WriteLine("Press any key to exit. . .");
            Console.ReadKey();
        }

        //Asks for a command, checks that it is valid, then executes given command
        static void DetectCommand(Board board)
        {
            if (board.Revealed && !board.Won)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You have dug up a mine, and lost, better luck next time!");
                Console.ResetColor();
                Console.WriteLine("Press any key to reset the game. . .");
                Console.ReadKey();
            }
            else if (board.Revealed && board.Won)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Congratulations, you have cleared the Minefield!");
                Console.ResetColor();
                Console.WriteLine("Press any key to reset the game. . .");
                Console.ReadKey();
            }
            else
            {
                bool needAnswer = true;
                while(needAnswer)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Stats: \t\tFlags Remaining:{board.MineCount-board.GetFlagCount()}");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Commands:\tDig X,Y\t\tFlag X,Y");
                    Console.ResetColor();
                    Console.Write("Your Command Here: ");
                    string? command = Console.ReadLine();
                    string?[] parts = command!.Split([' ', ',']);
                    
                    if (parts.Length == 3)
                    {
                            //checks if command contains dig
                        if (parts[0]!.ToLower().Contains("d") && int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
                        {
                            //separtaes the command string to isolate the X and Y integers
                            needAnswer = false;
                            //checks to see if tile isnt flagged
                            if (!board.FlaggedBoardArr[y-1,x-1])
                            {
                                board.AddToRevealedArray(y-1,x-1, false);
                                board.DigAroundNumber(y-1,x-1);
                            } 
                            else
                            {
                                Console.Write("You cannot dig on a flagged tile!\nPress Any key to type again. . .");
                                Console.ReadKey();
                                for (int i = 0; i < 4; i++)
                                {
                                    ClearLine();
                                }
                            }
                        //checks if command contains flag
                        } 
                        else if (parts[0]!.ToLower().Contains("f") && int.TryParse(parts[1], out int X) && int.TryParse(parts[2], out int Y))
                        {
                            //separtaes the command string to isolate the X and Y integers
                            needAnswer = false;
                            
                            if(board.BoardArr[Y-1,X-1] != ' ')
                            {
                                //sets or clears flag
                                board.SetFlag(Y-1,X-1);
                                board.FlagAroundNumber(Y-1,X-1);
                            } 
                            else
                            {
                                Console.Write("You cannot flag an empty tile!\nPress Any key to type again. . .");
                                Console.ReadKey();
                                for (int i = 0; i < 4; i++)
                                {
                                    ClearLine();
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.Write("Please make sure you are inputing a valid command!\nPress Any key to type again. . .");
                        Console.ReadKey();
                        for (int i = 0; i < 4; i++)
                        {
                            ClearLine();
                        }
                    }
                }
            }
        }
        static void ClearLine() //clears the last written line
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        static bool BoardPicker(out Board board)
        {
            board = new Board(0,0,0);

            bool needChoice = true;
            while (needChoice)
            {
                Console.WriteLine("Which Board would you like?\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("(1)-Easy\t(10x10 with 10 Mines)");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("(2)-Medium\t(10x10 with 20 Mines)");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("(3)-Hard\t(15x15 with 50 Mines)");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("(4)-Impossible\t(35x20 with 200 Mines)");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("(5)-Custom\t(Your own custom board!)");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("(6)-Exit Game");
                Console.ResetColor();

                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "6":
                        return false;
                    case "1":
                        board = new Board(10,10,10);
                        needChoice = false;
                        break;
                    case "2":
                        board = new Board(10,10,20);
                        needChoice = false;
                        break;
                    case "3":
                        board = new Board(15,15,50);
                        needChoice = false;
                        break;
                    case "4":
                        board = new Board(20,35,200);
                        needChoice = false;
                        break;
                    case "5":
                            Console.Clear();
                            int rowNum = 0;
                            int columnNum = 0;
                            int mineNum = 0;

                            bool needRows = true;
                            while (needRows)
                            {   
                                Console.WriteLine("Please input how many rows you would like!");
                                string? givenRows = Console.ReadLine();

                                if (int.TryParse(givenRows, out rowNum))
                                {
                                    if (rowNum >= 99 || rowNum >= 1)
                                    {
                                        needRows = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("The rows cannot be 0 or over 99!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Make sure you input a valid number! (1-99)");
                                }
                            }

                            bool needColums = true;
                            while (needColums)
                            {
                                Console.WriteLine("Please input how many columns you would like!");
                                string? givenColumns = Console.ReadLine();
                                if (int.TryParse(givenColumns, out columnNum))
                                {
                                    if (columnNum <= 99 || columnNum >= 1)
                                    {
                                        needColums = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("The columns cannot be 0 or over 99!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Make sure you input a valid number! (1-99)");
                                }
                            }
                            bool needMines = true;
                            while (needMines)
                            {
                                Console.WriteLine("Please input how many mines you would like!");
                                string? givenMines = Console.ReadLine();
                                if (int.TryParse(givenMines, out mineNum))
                                {
                                    if (mineNum < (rowNum * columnNum)-1 || mineNum >= 1)
                                    {
                                        needMines = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("The mines cannot be 0 or over the number of squares!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Make sure you input a valid number! (1-99)");
                                }

                            }
                            board = new Board(rowNum, columnNum, mineNum);
                            needChoice = false;
                        break;
                    default:
                        Console.WriteLine("Please pick a valid option!");
                        break;
                    
                }
            }
            return true;
        }
    }
}