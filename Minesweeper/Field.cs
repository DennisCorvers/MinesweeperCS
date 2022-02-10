using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public class Field
    {
        public event Action OnGameWon;
        public event Action OnGameLost;
        public event Action<int> OnCounterChanged;

        private int m_emptySquaresLeft;
        private int m_amountOfMines;
        private int m_flagsLeft;
        private bool m_firstClick;
        public Square[] SquareCollection { get; }

        /// <summary>
        /// The XSize or width of the field,
        /// </summary>
        public int XSize
        {
            get; private set;
        }
        /// <summary>
        /// The YSize of height of the field.
        /// </summary>
        public int YSize
        {
            get; private set;
        }
        /// <summary>
        /// The current amount of mines on the field.
        /// </summary>
        public int AmountOfMines
        {
            get { return m_amountOfMines; }
            private set
            {
                if (value >= XSize * YSize)
                    m_amountOfMines = XSize * YSize - 1;
                else
                    m_amountOfMines = value < 1 ? 1 : value;
            }
        }
        /// <summary>
        /// The amount of flags the player has left to place.
        /// </summary>
        public int FlagsLeft
        {
            get { return m_flagsLeft; }
            private set
            {
                m_flagsLeft = value < 0 ? 0 : value > AmountOfMines ? AmountOfMines : value;
                OnCounterChanged?.Invoke(m_flagsLeft);
            }
        }

        /// <summary>
        /// Creates a new playing field.
        /// </summary>
        /// <param name="xSize">The xSize or Width of the field.</param>
        /// <param name="ySize">The ySize of Height of the field.</param>
        /// <param name="amountOfMines">The amount of mines in the field.</param>
        public Field(int xSize, int ySize, int amountOfMines, Func<Point, Button> buttonFactory)
        {
            if (xSize < 5 || xSize > 30)
                throw new ArgumentOutOfRangeException(nameof(xSize), "XSize needs to be between 5 and 30.");

            if (ySize < 5 || ySize > 30)
                throw new ArgumentOutOfRangeException(nameof(ySize), "XSize needs to be between 5 and 30.");

            //Sets all of the properties.
            XSize = xSize;
            YSize = ySize;
            AmountOfMines = amountOfMines;
            FlagsLeft = AmountOfMines;
            m_firstClick = true;

            SquareCollection = new Square[XSize * YSize];

            //Creates squares based on the given width and heigth.
            for (int x = 0; x < XSize; x++)
            {
                for (int y = 0; y < YSize; y++)
                {
                    var location = new Point(x, y);
                    var button = buttonFactory(location);
                    var square = new Square(location, button, false);
                    SquareCollection[(XSize * y) + x] = square;

                    //Adds events to the squares being clicked.
                    square.LSQEvent += SquareLeftClicked;
                    square.RSQEvent += SquareRightClicked;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Square GetSquare(int x, int y)
            => SquareCollection[(XSize * y) + x];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGetSquare(int x, int y, out Square value)
        {
            if (x < 0 || y < 0 || x >= XSize || y >= YSize)
            {
                value = default;
                return false;
            }

            value = GetSquare(x, y);
            return true;
        }

        /// <summary>
        /// Generates mines in the playing field. Only generates mines in a list of empty squares.
        /// </summary>
        /// <param name="amount"></param>
        private static void GenerateMines(IList<Square> emptySquares, int amount)
        {
            Random R = new Random();
            while (amount > 0)
            {
                //Picks a random square from the list of empty squares and fills it with a mine.
                //Removes the square from the list after it has been given a mine.
                int i = R.Next(0, emptySquares.Count - 1);
                emptySquares[i].HasMine = true;
                emptySquares.RemoveAt(i);
                amount--;
            }
        }
        /// <summary>
        /// Excludes certain squares from containing a mine. Occurs directly after the player clicks the first square on the field.
        /// </summary>
        /// <param name="FirstSquare"></param>
        private void FirstClickGeneration(Square FirstSquare)
        {
            //Excludes the first clicked square from generation.
            var emptySquares = new List<Square>(SquareCollection);
            emptySquares.Remove(FirstSquare);


            //Finds all 8 squares around the first click and excludes these from generation (if possible).
            int i = XSize * YSize - AmountOfMines - 1;
            var FirstAdjacentSquares = LocateSurrounding(FirstSquare);

            foreach (Square S in FirstAdjacentSquares)
            {
                if (i <= 0)
                    break;
                //Excludes square from generation.
                emptySquares.Remove(S);
                i--;
            }

            //Adds mines to all available, empty squares.
            GenerateMines(emptySquares, AmountOfMines);
            m_emptySquaresLeft = emptySquares.Count + 1;

            //Gets the surrounding mine count for empty squares on the field.
            foreach (Square S in emptySquares)
                S.SurroundingMines = CountSurroundingMines(S);

            //AFTER generation, count how many mines surround the first square.
            FirstSquare.SurroundingMines = CountSurroundingMines(FirstSquare);
            //Count all surrounding mines for the squares directly adjacent to the first square.
            foreach (Square S in FirstAdjacentSquares)
            {
                if (S.HasMine)
                    continue;
                S.SurroundingMines = CountSurroundingMines(S);
                m_emptySquaresLeft++;
            }
        }

        /// <summary>
        /// Counts the number of mines surrounding the given square.
        /// </summary>
        /// <param name="centerSquare">The square from which you want to know the amount of adjacent mines.</param>
        /// <returns></returns>
        private int CountSurroundingMines(Square centerSquare)
        {
            int surroundingCount = 0;
            ExecuteOnSurrounding(centerSquare, x =>
            {
                if (x.HasMine)
                    surroundingCount++;
            });

            return surroundingCount;
        }

        /// <summary>
        /// Returns a list of all the squares surrounding the given square.
        /// </summary>
        /// <param name="centerSquare">The square from which you want to find the surrounding squares.</param>
        /// <returns></returns>
        private ICollection<Square> LocateSurrounding(Square centerSquare)
        {
            var surroundingSquares = new List<Square>();
            ExecuteOnSurrounding(centerSquare, x =>
            {
                if (!x.IsVisible)
                    surroundingSquares.Add(x);
            });

            return surroundingSquares;
        }

        private void ExecuteOnSurrounding(Square centerSquare, Action<Square> nextSquareDelegate)
        {
            Square square;
            // Left-Up
            if (TryGetSquare(centerSquare.Location.X - 1, centerSquare.Location.Y - 1, out square)) nextSquareDelegate(square);
            // Up
            if (TryGetSquare(centerSquare.Location.X, centerSquare.Location.Y - 1, out square)) nextSquareDelegate(square);
            // Right-Up
            if (TryGetSquare(centerSquare.Location.X + 1, centerSquare.Location.Y - 1, out square)) nextSquareDelegate(square);
            // Left
            if (TryGetSquare(centerSquare.Location.X - 1, centerSquare.Location.Y, out square)) nextSquareDelegate(square);
            // Right
            if (TryGetSquare(centerSquare.Location.X + 1, centerSquare.Location.Y, out square)) nextSquareDelegate(square);
            // Left-Down
            if (TryGetSquare(centerSquare.Location.X - 1, centerSquare.Location.Y + 1, out square)) nextSquareDelegate(square);
            // Down
            if (TryGetSquare(centerSquare.Location.X, centerSquare.Location.Y + 1, out square)) nextSquareDelegate(square);
            // Right-Down
            if (TryGetSquare(centerSquare.Location.X + 1, centerSquare.Location.Y + 1, out square)) nextSquareDelegate(square);
        }

        private void SquareLeftClicked(Square square)
        {
            //If this is the first square clicked, start generating mines around this point.
            if (m_firstClick)
            {
                FirstClickGeneration(square);
                m_firstClick = false;
            }

            //Removes events from this square since it's being clicked (and thus can't ever be clicked again).
            square.LSQEvent -= SquareLeftClicked;
            square.RSQEvent -= SquareRightClicked;
            square.RemoveClickEvents();

            //If a mine is clicked, display where all the mines are and lose the game.
            if (square.HasMine)
            { LoseGame(); return; }

            //If the square is marked with a questionmark, remove it.
            if (square.MarkState == SquareState.Question)
                square.SetImage(null);

            //Change button to appear clicked.
            square.UpdateText();
            m_emptySquaresLeft--;
            square.IsVisible = true;


            //Checks all surrounding squares.
            //If THIS square is 0, all surrounding squares will be clicked regardless of values.
            //If the adjacent square is 0, but this square has another value, the adjacent square will be clicked.
            var SurroundingSquares = LocateSurrounding(square);
            foreach (Square s in SurroundingSquares)
            {
                if (square.SurroundingMines == 0)
                    s.SquareLeftClicked(null, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            }

            //If there are no empty squares left, win the game.
            if (m_emptySquaresLeft <= 0)
                OnGameWon?.Invoke();
        }

        private void SquareRightClicked(Square square)
        {
            //Handles the different squarestates of the given square.
            switch (square.MarkState)
            {
                //If the square is unmarked and clicked, flag the square.
                case SquareState.Unmarked:
                    {
                        //Stop the player from marking squares when they are out of flags.
                        if (FlagsLeft > 0)
                        {
                            FlagsLeft--;
                            square.SetImage(ColorSettings.FlagImage);
                            square.MarkState = SquareState.Flagged;
                        }
                        break;
                    }
                //If the square is flagged and clicked, mark it with a questionmark.
                case SquareState.Flagged:
                    {
                        square.SetImage(ColorSettings.QuestionMarkImage);
                        FlagsLeft++;
                        square.MarkState = SquareState.Question;
                        break;
                    }
                //If the square is marked with a questionmark and clicked, unmark it.
                case SquareState.Question:
                    {
                        square.SetImage(null);
                        square.MarkState = SquareState.Unmarked;
                        break;
                    }
            }
        }

        /// <summary>
        /// Displays all mines and automatically raises a losegame event.
        /// </summary>
        private void LoseGame()
        {
            //Finds all squares with a mine and sets them to visible.
            foreach (var square in SquareCollection)
            {
                if (!square.HasMine)
                    continue;

                square.SetImage(ColorSettings.MineImage);
            }

            OnGameLost?.Invoke();
        }
    }
}
