using System;

namespace Tetrominoes
{
    public class MatchScore
    {
        TimeSpan _totalTime;
        public TimeSpan TotalTime
        {
            get => _totalTime;
            private set
            {
                _totalTime = value;
                TotalTimeAsText = TotalTime.ToString("hh\\:mm\\:ss");
                FractionalSecondsAsText = TotalTime.ToString("fff");
            }
        }
        public string TotalTimeAsText { get; private set; } = "";
        public string FractionalSecondsAsText { get; private set; } = "";

        int _totalScore;
        public int TotalScore
        {
            get => _totalScore;
            private set
            {
                _totalScore = value;
                TotalScoreAsText = $"{TotalScore,6:0}";
            }
        }
        public string TotalScoreAsText { get; private set; } = "     0";

        int _totalRows;
        public int TotalRows
        {
            get => _totalRows;
            private set
            {
                _totalRows = value;
                TotalRowsAsText = $"{TotalRows,6:0}";
                Level = (TotalRows / 10) + 1;
            }
        }
        public string TotalRowsAsText { get; private set; } = "     0";

        int _level = 1;
        public int Level
        {
            get => _level;
            private set
            {
                if (_level == value) return;

                _level = value;
                LevelAsText = $"{Level,6:0}";

                Speed = Level switch
                {
                    1 => 500,
                    2 => 450,
                    3 => 400,
                    4 => 350,
                    5 => 300,
                    6 => 250,
                    7 => 200,
                    8 => 150,
                    9 => 100,
                    _ => 50
                };

                LevelChanged?.Invoke(this);
            }
        }
        public string LevelAsText { get; private set; } = "     1";
        public int Speed { get; private set; } = 500;

        public Match Match { get; }

        public event Action<MatchScore>? LevelChanged;
        public event Action<MatchScore, TetrominoPiece>? PieceLocked;
        public event Action<MatchScore, int>? RowsCleared;
        public event Action<MatchScore, MovementType>? PieceMoved;
        public event Action<MatchScore, RotationDirection>? PieceRotated;
        public event Action<MatchScore>? PieceSwapped;

        public MatchScore(Match match)
        {
            Match = match ?? throw new ArgumentNullException(nameof(match));
        }

        public void AddMovement(MovementType type)
        {
            PieceMoved?.Invoke(this, type);
        }
        public void AddRotation(RotationDirection direction)
        {
            PieceRotated?.Invoke(this, direction);
        }
        public void AddPieceLocked(TetrominoPiece piece)
        {
            TotalScore += (int)piece * 5;
            PieceLocked?.Invoke(this, piece);
        }
        public void AddRowsCleared(int rowCount)
        {
            TotalRows += rowCount;
            TotalScore += rowCount switch
            {
                1 => 100,
                2 => 200,
                3 => 500,
                4 => 1000,
                _ => 0
            };
            RowsCleared?.Invoke(this, rowCount);
        }
        public void AddSwap()
        {
            PieceSwapped?.Invoke(this);
        }
        public void AddTime(TimeSpan delta)
        {
            TotalTime += delta;
        }
    }
}
