namespace TicTacToe
{
    internal static class PlayerFactory
    {
        public static PlayerMark CreatePlayer(string input, CellState cellStateIfX, CellState cellStateIfO) => input switch
        {
            "X" => new PlayerMark(0, 0, cellStateIfX),
            "O" => new PlayerMark(0, 0, cellStateIfO),
            _ => throw new Exception()
        };
    }
}
