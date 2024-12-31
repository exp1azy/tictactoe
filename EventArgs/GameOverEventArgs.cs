namespace TicTacToe.EventArgs
{
    internal class GameOverEventArgs : System.EventArgs
    {
        public GameState GameState { get; set; }

        public CellState? Winner { get; set; }
    }
}
