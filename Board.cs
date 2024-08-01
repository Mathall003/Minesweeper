
using System.Dynamic;
using System.Formats.Asn1;
using System.Xml.Serialization;

namespace Minesweeper
{
    class Board
    {
        readonly int speedOfReveal = 0; //mostly useless tbh, just for fun in small ones
        readonly char symbol = ' ';
        readonly char mineSymbol = 'X';

        public int Rows
        {get;set;}
        public int Columns
        {get;set;}
        public int MineCount
        {get;set;}
        char[,] BoardArr
        {get;set;}
        bool[,] revealedBoardArr
        {get;set;}
        bool revealed
        {get;set;}

        //copy board constructor
        public Board(Board s)
        {
            Rows = s.Rows;
            Columns = s.Columns;
            MineCount = s.MineCount;
            BoardArr = s.BoardArr;
            revealedBoardArr = s.revealedBoardArr;
            revealed = s.revealed;
        }

        //Board Constructor
        public Board(int rows, int columns, int mineCount)
        {
            Rows = rows;
            Columns = columns;
            MineCount = mineCount;
            BoardArr = new char[Rows,Columns];
            revealedBoardArr = new bool[Rows,Columns];
            revealed = false;
        }

        public void BoardMaker()
        {
            //for each tile in the board set it to symbol
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    BoardArr[i,j] = symbol;
                }
            }

            //generates random mines through the array
            Random random = new();
            for (int i = 0; i < MineCount; i++)
            {
                BoardArr[random.Next(0,Rows),random.Next(0,Columns)] = mineSymbol;
            }

            GenerateTileNumbers();
        }

        public void DrawBoard()
        {
            //Add space + bar before X number key
            Console.Write("  |");

            //write X number key
            for (int i = 0; i < Columns; i++)
            {
                Thread.Sleep(speedOfReveal);
                //adds X key above the columns
                if (i<10)
                {
                    Console.Write($" {i} ");
                } else if (i<99)
                {
                    Console.Write($" {i}");
                }
            }
            Console.Write("|");
            //Add divider line and cross before full divider line
            Console.Write("\n-+");

            //write divider line
            for (int i = 0; i < Columns; i++)
            {
                Thread.Sleep(speedOfReveal);
                Console.Write($"---");
            }
            Console.Write("-+--");
            //start next line
            Console.WriteLine();


            //for each tile in the board write its value
            for (int i = 0; i < Rows; i++)
            {
                Thread.Sleep(speedOfReveal);
                //adds Y key before each new row
                if (i<10)
                {
                    Console.Write($" {i}|");
                } else if (i<99)
                {
                    Console.Write($"{i}|");
                }

                for (int j = 0; j < Columns; j++)
                {
                    Thread.Sleep(speedOfReveal);

                    if (revealed)
                    {
                        ChangeColor(BoardArr[i,j]); //write each character with own color   
                    }
                    else if (revealedBoardArr[i,j] == true) //checks if tile has been revealed
                    {
                        ChangeColor(BoardArr[i,j]); //write each character with own color   
                    } else
                    {
                        ChangeColor('?'); //writes out all unknowns
                    }
                }
                //every row complete, move to the next row
                Console.Write("|");
                Console.WriteLine();
            }
            //Add divider line and cross before full divider line
            Console.Write("--+");

            //write divider line
            for (int i = 0; i < Columns; i++)
            {
                Thread.Sleep(speedOfReveal);
                Console.Write($"---");
            }
            Console.Write("+--");
            //start next line
            Console.WriteLine();
        }

        public void GenerateTileNumbers()
        {
            //for each tile in the board
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    //if tile is not a mine
                    if (BoardArr[x,y] != 'X')
                    {
                        int surroundingMineCount = 0;
                        //for each other tile 1 away from the tile
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                bool mineHasntBeenCounted = true;
                                //checks to confirm that the X tile is within borders
                                if ((x + i) < 0)
                                {
                                    mineHasntBeenCounted = false;
                                }
                                if ((x+i) >= Rows)
                                {
                                    mineHasntBeenCounted = false;
                                }
                                //checks to confirm that the X tile is within borders
                                if ((y + j) < 0)
                                {
                                    mineHasntBeenCounted = false;
                                }
                                if ((y+j) >= Columns)
                                {
                                    mineHasntBeenCounted = false;
                                }
                                //if that tile is a mine add 1 to the surrounding mine count
                                if (mineHasntBeenCounted && BoardArr[x+i,y+j] == mineSymbol)
                                {
                                    surroundingMineCount++;
                                }
                            }
                        }
                        //if the surrounding mine count is greater than 0 set the tile to its surrounding mine count
                        if(surroundingMineCount > 0)
                        {
                            BoardArr[x,y] = surroundingMineCount.ToString()[0];
                        }
                    }
                }
            }
        }
        
        public void ChangeColor(char character)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            switch(character)
            {
                case '?':
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case ' ':
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case 'X':
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case '1':
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case '2':
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case '3':
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case '4':
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case '5':
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case '6':
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case '7':
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case '8':
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    break;
            }
            Console.Write($" {character} ");
            Console.ResetColor();
        }
    }
}