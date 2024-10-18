

using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Minesweeper
{
    class Board
    {
        readonly int speedOfReveal = 0; //mostly useless tbh, just for fun on small boards

        private int Rows
        {get;set;}
        private int Columns
        {get;set;}
        private int MineCount
        {get;set;}
        private char[,] BoardArr
        {get;set;}
        private bool[,] RevealedBoardArr
        {get;set;}
        private bool[,] FlaggedBoardArr
        {get;set;}
        private bool Revealed
        {get;set;}
        private bool Won
        {get;set;}        

        //copy board constructor
        public Board(Board s)
        {
            Rows = s.Rows;
            Columns = s.Columns;
            MineCount = s.MineCount;
            BoardArr = s.BoardArr;
            RevealedBoardArr = s.RevealedBoardArr;
            FlaggedBoardArr = s.FlaggedBoardArr;
            Revealed = s.Revealed;
            Won = s.Won;
        }

        //Board Constructor
        public Board(int rows, int columns, int mineCount)
        {
            Rows = rows;
            Columns = columns;
            MineCount = mineCount;
            BoardArr = new char[Rows,Columns];
            RevealedBoardArr = new bool[Rows,Columns];
            FlaggedBoardArr = new bool[Rows,Columns];
            Revealed = false;
            Won = false;
        }

        public void BoardMaker()
        {
            //for each tile in the board set it to white space
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    BoardArr[i,j] = ' ';
                }
            }

            //generates random mines through the array
            Random random = new();
            for (int i = 0; i < MineCount; i++)
            {
                bool needRand = true;
                while (needRand)
                {
                    int randX = random.Next(0,Rows);
                    int randY = random.Next(0,Columns);

                    if (BoardArr[randX,randY] == ' ')
                    {
                        BoardArr[randX,randY] = 'X';
                        needRand = false;
                    }

                }
                
            }

            GenerateTileNumbers();
            FindLargestWhitespace();


        }

        public void DrawBoard()
        {
            CheckForClearWhitespace();
            CheckForDugMine();
            CheckForWin();
            //Add space + bar before X number key
            Console.Write("  |");

            //write X number key
            for (int i = 1; i <= Columns; i++)
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
            Console.Write("\n--+");

            //write divider line
            for (int i = 0; i < Columns; i++)
            {
                Thread.Sleep(speedOfReveal);
                Console.Write($"---");
            }
            Console.Write("+--");
            //start next line
            Console.WriteLine();


            //for each tile in the board write its value
            for (int i = 0; i < Rows; i++)
            {
                Thread.Sleep(speedOfReveal);
                //adds Y key before each new row
                if (i+1<10)
                {
                    Console.Write($" {i+1}|");
                } else if (i+1<99)
                {
                    Console.Write($"{i+1}|");
                }

                for (int j = 0; j < Columns; j++)
                {
                    Thread.Sleep(speedOfReveal);
                    if (Revealed)
                    {
                        ChangeColor(BoardArr[i,j]); //write each character with own color   
                    }
                    else if (FlaggedBoardArr[i,j] == true)//checks if tile is flagged
                    {
                        ChangeColor('P');
                    }
                    else if (RevealedBoardArr[i,j] == true) //checks if tile has been revealed
                    {
                        ChangeColor(BoardArr[i,j]); //write each character with own color   
                    } else
                    {
                        ChangeColor('?'); //writes out all unknowns
                    }
                }
                //every row complete, move to the next row
                Console.Write($"|{i+1}");
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
            Console.Write("\n  |");

            //write X number key
            for (int i = 1; i <= Columns; i++)
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
                                if (mineHasntBeenCounted && BoardArr[x+i,y+j] == 'X')
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
                case 'C':
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    character = 'X';
                    break;
                case 'I':
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    character = 'P';
                    break;
                case 'P':
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case '?':
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case ' ':
                    Console.BackgroundColor = ConsoleColor.Black;
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
                    Console.ForegroundColor = ConsoleColor.Cyan;
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
        public void AddToRevealedArray(int x, int y, bool revealAll)
        {
            RevealedBoardArr[x,y] = true;
            if (revealAll)
            {
                Revealed = true;
            }
        }

        public void SetFlag(int x, int y)
        {
            if (!RevealedBoardArr[x,y])
            {
                if (FlaggedBoardArr[x,y] == true)
                {
                    FlaggedBoardArr[x,y] = false;
                }
                else
                {
                    FlaggedBoardArr[x,y] = true;
                }
            }
            
        }
        public void CheckForClearWhitespace()
        {
            //for each tile in the board
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    if (BoardArr[x,y] == ' ' && RevealedBoardArr[x,y])
                    {
                        ClearConnectingWhiteSpace(x,y);
                    }
                }
            }
        }

        public void ClearConnectingWhiteSpace(int x, int y)
        {
            //for each other tile 1 away from the tile
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    //checks to confirm that the X tile is within borders
                    if (x + i >= 0 && x+i < Rows && y + j >= 0 && y+j < Columns)
                    {
                        if (BoardArr[x+i,y+j] != 'X' && RevealedBoardArr[x+i,y+j] == false)
                        {
                            AddToRevealedArray(x+i,y+j,false);
                            CheckForClearWhitespace();
                        }
                    }
                }
            }
        }
        public int GetFlagCount()
        {
            int trueFlagCount = 0;
            foreach (var item in FlaggedBoardArr)
            {
                if (item)
                {
                    trueFlagCount++;
                }
            }
            return trueFlagCount;
        }

        public void CheckForDugMine()
        { 
            //for each tile in the board
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    //if the tile is a mine, & is revealed
                    if (BoardArr[x,y] == 'X' && RevealedBoardArr[x,y])
                    {
                        DoWhenMineDug();
                    }
                }
            }

        }

        public void DoWhenMineDug()
        {
            ReplaceAllFlagsWithCorrectColor();
            Revealed = true;
        }

        public void ReplaceAllFlagsWithCorrectColor()
        {
            //for each tile in the board
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    //if the tile is flagged
                    if (FlaggedBoardArr[x,y])
                    {
                        //if the tile is a mine
                        if (BoardArr[x,y] == 'X')
                        {
                            //set it to correct flag
                            BoardArr[x,y] = 'C';
                        }
                        else
                        {
                            //set it to incorrect flag
                            BoardArr[x,y] = 'I';
                        }
                    }
                }
            }
        }

        public void DigAroundNumber(int x,int y)
        { 
            int surroundingFlags = 0;
            //if tile is a number
            if(BoardArr[x,y] != 'X' && BoardArr[x,y] != ' ')
            {    
                //for each other tile 1 away from the tile
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        //checks to confirm that the X tile is within borders
                        if (x + i >= 0 && x+i < Rows && y + j >= 0 && y+j < Columns)
                        {
                            if (FlaggedBoardArr[x+i,y+j])
                            {
                                surroundingFlags++;
                            }
                        }
                    }
                }
            }
            
            if(BoardArr[x,y] != 'X' && BoardArr[x,y] != ' ')
            {    
                //for each other tile 1 away from the tile
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        //checks to confirm that the flag tile is within borders
                        if (x + i >= 0 && x+i < Rows && y + j >= 0 && y+j < Columns)
                        {
                            //confirms that the square to be revealed in not a mine or flag
                            if (FlaggedBoardArr[x+i,y+j] == false && surroundingFlags.ToString()[0] == BoardArr[x,y])
                            {
                                RevealedBoardArr[x+i,y+j] = true;
                            }
                        }
                    }
                }
            }
        }

        public void FlagAroundNumber(int x, int y)
        {
            int surroundingEmptySpaces = 0;
            //if tile is a number
            if(BoardArr[x,y] != 'X' && BoardArr[x,y] != ' ')
            {    
                //for each other tile 1 away from the tile
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        //checks to confirm that the un opened
                        if (x + i >= 0 && x+i < Rows && y + j >= 0 && y+j < Columns)
                        {
                            if (!RevealedBoardArr[x+i,y+j])
                            {
                                surroundingEmptySpaces++;
                            }
                        }

                    }
                }
            }
            
            if(BoardArr[x,y] != 'X' && BoardArr[x,y] != ' ')
            {    
                //for each other tile 1 away from the tile
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        //checks to confirm that the flag tile is within borders
                        if (x + i >= 0 && x+i < Rows && y + j >= 0 && y+j < Columns)
                        {
                            //confirms that the square to be flagged in not a revealed
                            if (RevealedBoardArr[x+i,y+j] == false && surroundingEmptySpaces.ToString()[0] == BoardArr[x,y])
                            {
                                FlaggedBoardArr[x+i,y+j] = true;
                            }
                        }
                    }
                }
            }

        }
        
            
        //if all tiles that are not mines, activate win
        public void CheckForWin()
        {
            int neededClearToWin = (Rows*Columns) - MineCount;
            //for each tile in the board
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    //if the tile is not a mine
                    if (BoardArr[x,y] != 'X')
                    {
                        //and tile is revealed
                        if (RevealedBoardArr[x,y])
                        {
                            //subtract it from needed to win
                            neededClearToWin --;
                        }
                    }
                }
            }
            if (neededClearToWin == 0)
            {
                Won = true;
                Revealed = true;
            }
        }

        //use food fill to to find largest group
        public void FindLargestWhitespace()
        {
            int[,] directions = new int[,] { {0,1}, {1,0}, {0,-1}, {-1,0}};
            bool[,] visited = new bool[Rows,Columns];
            int largestGroupSize = 0;
            (int,int)  largestGroupCoord = (-1,-1);

            //for every tile on the board
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    //if tile is a whitespace, and not visited
                    if (BoardArr[x,y] == ' ' && !visited[x,y])
                    {
                        int currentGroupSize = 0;
                        (int, int) currentCoord = (x,y);
                        Queue<(int, int)> queue = new Queue<(int, int)>();

                        //add tile to queue
                        queue.Enqueue((x, y));
                        visited[x, y] = true;

                        //while the queue is not empty
                        while (queue.Count > 0)
                        {
                            //set coordinates to var & increase the group size
                            var (r, c) = queue.Dequeue();
                            currentGroupSize++;

                            //for each coordinates add coordinates around to queue
                            for (int i = 0; i < 4; i++)
                            {
                                int nx = r + directions[i, 0];
                                int ny = c + directions[i, 1];

                                if (nx >= 0 && nx < Rows && ny >= 0 && ny < Columns && BoardArr[nx, ny] == ' ' && !visited[nx, ny])
                                {
                                    queue.Enqueue((nx, ny));
                                    visited[nx, ny] = true;
                                }
                            }
                        }
                        //if new group size is larger than last largest, set it to largest
                        if (currentGroupSize > largestGroupSize)
                        {
                        largestGroupSize = currentGroupSize;
                        largestGroupCoord = currentCoord;
                        }
                    }
                }
            }
            //after all tiles are accounted, reveal one to start chain
            AddToRevealedArray(largestGroupCoord.Item1, largestGroupCoord.Item2, false);
        }
    }
}