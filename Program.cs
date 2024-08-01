/*
// TO DO:
//
// be able to add stuff to revealed array
// be able to find largest group of whitespace
// be able have user flag, and add blocks to revealed arr
//
// if user reveals a whitespace break all nearby
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
            Board board = new Board(26,36,50);
            board.BoardMaker();
            Console.Clear();

            board.DrawBoard();
    
            Console.ReadLine();
        }
    }
}