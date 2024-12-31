using TicTacToe.EventArgs;
using TicTacToe.Exceptions;

namespace TicTacToe
{
    internal class BattleField
    {
        private readonly int _fieldSize;
        private readonly CellState[,] _battleFieldMatrix;
        private bool _gameOver;
        private readonly List<(int, int)> _emptyCells;

        public BattleField()
        {
            _fieldSize = 3;
            _gameOver = false;
            _battleFieldMatrix = new CellState[_fieldSize, _fieldSize];
            _emptyCells = [];

            for (int x = 0; x < _fieldSize; x++)
            {
                for (int y = 0; y < _fieldSize; y++)
                {
                    _battleFieldMatrix[x, y] = CellState.Empty;
                    _emptyCells.Add((x, y));
                }   
            }

            DrawField();
        }

        public bool GameOver => _gameOver;
        public int FieldSize => _fieldSize;

        public void Move(PlayerMark playerMark)
        {
            if (!CorrectInput(playerMark.X))
                throw new IncorrectInputException();
            if (!CorrectInput(playerMark.Y))
                throw new IncorrectInputException();

            var cell = _battleFieldMatrix[playerMark.X, playerMark.Y];

            if (cell != CellState.Empty)
                throw new CellAlreadyMarkedException();

            _battleFieldMatrix[playerMark.X, playerMark.Y] = playerMark.CellState;
            _emptyCells.Remove((playerMark.X, playerMark.Y));

            DrawField();

            var gameState = GetGameState();
            if (gameState != GameState.NotFinished)
            {
                OnGameOver?.Invoke(this, new GameOverEventArgs 
                { 
                    GameState = gameState, 
                    Winner = gameState == GameState.Draw ? null : playerMark.CellState 
                });

                _gameOver = true;
            }
        }

        private GameState GetGameState()
        {
            var lastIndex = _fieldSize - 1;
            var preLastIndex = _fieldSize - 2;

            var byCrossCell = _battleFieldMatrix[0, 0];

            if (byCrossCell != CellState.Empty && 
                byCrossCell == _battleFieldMatrix[lastIndex, lastIndex] && 
                byCrossCell == _battleFieldMatrix[preLastIndex, preLastIndex])
            {
                return GameState.Finished;
            }

            var byReversedCrossCell = _battleFieldMatrix[0, lastIndex];

            if (byReversedCrossCell != CellState.Empty &&
                byReversedCrossCell == _battleFieldMatrix[lastIndex, lastIndex] &&
                byReversedCrossCell == _battleFieldMatrix[lastIndex, 0])
            {
                return GameState.Finished;
            }

            for (int i = 0; i < _fieldSize; i++)
            {
                var byColsCell = _battleFieldMatrix[0, i];

                if (byColsCell != CellState.Empty &&
                    byColsCell == _battleFieldMatrix[lastIndex, i] && 
                    byColsCell == _battleFieldMatrix[preLastIndex, i])
                {
                    return GameState.Finished;
                }

                var byRowsCell = _battleFieldMatrix[i, 0];
                if (byRowsCell != CellState.Empty &&
                    byRowsCell == _battleFieldMatrix[i, lastIndex] && 
                    byRowsCell == _battleFieldMatrix[i, preLastIndex])
                {
                    return GameState.Finished;
                }
            }

            if (_emptyCells.Count == 0)
                return GameState.Draw;

            return GameState.NotFinished;
        }

        private bool CorrectInput(int input)
        {
            if (input + 1 > _fieldSize || input < 0)
                return false;

            return true;
        }

        private void DrawField()
        {
            Console.Clear();

            for (int i = 0; i < _fieldSize; i++)
            {
                for (int j = 0; j < _fieldSize; j++)
                {
                    string cell = _battleFieldMatrix[i, j] switch
                    {
                        CellState.Empty => "[ ]",
                        CellState.Cross => "[X]",
                        CellState.Zero => "[O]",
                        _ => throw new Exception()
                    };

                    Console.Write(" " + cell + " ");
                }

                Console.WriteLine();
            }
        }

        public event EventHandler<GameOverEventArgs> OnGameOver;
    }
}
