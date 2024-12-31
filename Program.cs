namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello! It's Tic Tac Toe! Write 1 if X or 2 if O:");

            string? input = null;
            int choice = 0;

            while (choice < 1 || choice > 2)
            {
                input = Console.ReadLine();
                choice = int.Parse(input);
            }

            Console.Clear();

            var side = choice == 1 ? "X" : "O";

            var you = PlayerFactory.CreatePlayer(side, CellState.Cross, CellState.Zero);
            var enemy = PlayerFactory.CreatePlayer(side, CellState.Zero, CellState.Cross);

            var ticTacToe = new BattleField();
            var random = new Random();

            ticTacToe.OnGameOver += (s, ea) =>
            {
                var gameState = ea.GameState;
                if (gameState == GameState.Finished)
                {
                    var winner = ea.Winner;
                    var result = winner == CellState.Cross ? "X" : "O";
                    Console.WriteLine($"Game over! The winner is {result}");
                }
                else
                {
                    Console.WriteLine("This is a draw!");
                }
            };

            bool yourMove = true;

            while (!ticTacToe.GameOver)
            {
                try
                {
                    if (yourMove)
                    {
                        Console.Write("Insert first coord: ");
                        var firstCoord = Console.ReadLine();
                        if (!int.TryParse(firstCoord, out int x))
                        {
                            Console.WriteLine("Incorrect first coord! Try again.");
                            continue;
                        } 

                        Console.Write("Insert second coord: ");
                        var secondCoord = Console.ReadLine();
                        if (!int.TryParse(secondCoord, out int y))
                        {
                            Console.WriteLine("Incorrect second coord! Try again.");
                            continue;
                        }

                        you.X = x;
                        you.Y = y;
                        
                        ticTacToe.Move(you);
                    }
                    else
                    {
                        while (true)
                        {
                            var x = random.Next(0, ticTacToe.FieldSize);
                            var y = random.Next(0, ticTacToe.FieldSize);

                            try
                            {
                                enemy.X = x;
                                enemy.Y = y;
                                ticTacToe.Move(enemy);

                                break;
                            }
                            catch (Exception) { }
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    Console.WriteLine(ex.ToString());
                }

                yourMove = !yourMove;
            }
        }
    }
}
