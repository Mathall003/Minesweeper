/*
// TO DO:
//
// if a whitespace is revelaed break all next to it
// be able to find largest group of whitespace & reveal it
//
//Be able to dig on numbers, and if correct amount of flags around dig tiles around it
//
// Have win condition when all non-mine spaces are revealed
// have loss condition when a mine is dug
//     also have it reveal the whole board with flag color changing if correct or not
*/


namespace Minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
                    
            Console.WriteLine("Press any key to create the board");
            Console.ReadKey();
            Board board = new Board(21,36,50);
            board.BoardMaker();
            Console.Clear();
            while (true)
            {

                board.DrawBoard();
                DetectCommand(board);
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.Clear();
            }
        }

        //Asks for a command, checks that it is valid, then executes given command
        static void DetectCommand(Board board)
        {
            bool needAnswer = true;
            while(needAnswer)
            {
                Console.WriteLine("Commands:\nDig X,Y\tFlag X,Y");
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
                        if (!board.FlaggedBoardArr[y,x])
                        {
                            board.AddToRevealedArray(y,x);
                        } else
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
                        
                        if(board.RevealedBoardArr[Y,X] == false)
                        {
                            //sets or clears flag
                            board.SetFlag(Y,X);
                        } 
                        else
                        {
                            Console.Write("You cannot flag an already dug tile!\nPress Any key to type again. . .");
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
        static void ClearLine() //clears the last written line
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
}