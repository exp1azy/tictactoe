namespace TicTacToe
{
    internal struct PlayerMark(int x, int y, CellState cellState)
    {
        public int X = x;
        public int Y = y;
        public CellState CellState = cellState;
    }
}
