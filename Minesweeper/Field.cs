using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public class Field
    {
        /// <summary>
        /// An event that gets raised after the losing conditions have been met.
        /// </summary>
        public event GameWon eGameWon;
        public delegate void GameWon();
        /// <summary>
        /// An event that gets raised after the winning conditions have been met.
        /// </summary>
        public event GameLost eGameLost;
        public delegate void GameLost();
        /// <summary>
        /// An event that gets raised each time the player places or removes a flag.
        /// </summary>
        public event CounterChanged eCounterChanged;
        public delegate void CounterChanged(int newCount);

        /// <summary>
        /// A list of all of the squares on the field.
        /// </summary>
        public Dictionary<int, Dictionary<int, Square>> SquareCollection;
        /// <summary>
        /// A list of all of the empty squares on the field.
        /// </summary>
        private List<Square> EmptySquares;

        private int _XSize;
        private int _YSize;
        private int _AmountOfMines;
        private int _FlagsLeft;

        /// <summary>
        /// The XSize or width of the field,
        /// </summary>
        public int XSize
        {
            get { return _XSize; }
            set { _XSize = value < 5 ? 5 : value > 30 ? 30 : value; }
        }
        /// <summary>
        /// The YSize of height of the field.
        /// </summary>
        public int YSize
        {
            get { return _YSize; }
            set { _YSize = value < 5 ? 5 : value > 30 ? 30 : value; }
        }
        /// <summary>
        /// The current amount of mines on the field.
        /// </summary>
        public int AmountOfMines
        {
            get { return _AmountOfMines; }
            set
            {
                if (value >= XSize * YSize)
                { _AmountOfMines = XSize * YSize - 1; }
                else { _AmountOfMines = value < 1 ? 1 : value; }
            }
        }
        /// <summary>
        /// The amount of flags the player has left to place.
        /// </summary>
        public int FlagsLeft
        {
            get { return _FlagsLeft; }
            set
            {
                _FlagsLeft = value < 0 ? 0 : value > AmountOfMines ? AmountOfMines : value;
                if (eCounterChanged != null) { eCounterChanged(_FlagsLeft); }
            }
        }

        /// <summary>
        /// Determines if the firstclick has occured yet. False after instantiating.
        /// </summary>
        private bool FirstClick;

        /// <summary>
        /// Creates a new playing field.
        /// </summary>
        /// <param name="xSize">The xSize or Width of the field.</param>
        /// <param name="ySize">The ySize of Height of the field.</param>
        /// <param name="amountOfMines">The amount of mines in the field.</param>
        public Field(int xSize, int ySize, int amountOfMines)
        {
            //Sets all of the properties.
            XSize = xSize;
            YSize = ySize;
            AmountOfMines = amountOfMines;
            FlagsLeft = AmountOfMines;
            FirstClick = true;

            //Creates a new squarecollection and list that keeps track of the empty (mine-less) squares.
            SquareCollection = new Dictionary<int, Dictionary<int, Square>>();
            EmptySquares = new List<Square>();

            //Creates squares based on the given width and heigth.
            for (int x = 1; x < XSize + 1; x++)
            {
                //Adds one X row.
                SquareCollection.Add(x, new Dictionary<int, Square>());
                for (int y = 1; y < YSize + 1; y++)
                {
                    Square S = new Square(new System.Drawing.Point(x, y), false);
                    EmptySquares.Add(S);
                    SquareCollection[x].Add(y, S);

                    //Adds events to the squares being clicked.
                    S.LSQEvent += SquareLeftClicked;
                    S.RSQEvent += SquareRightClicked;
                }
            }
        }

        /// <summary>
        /// Generates mines in the playing field. Only generates mines in a list of empty squares.
        /// </summary>
        /// <param name="Amount"></param>
        private void AddExtraMines(int Amount)
        {
            Random R = new Random();
            while (Amount > 0)
            {
                //Picks a random square from the list of empty squares and fills it with a mine.
                //Removes the square from the list after it has been given a mine.
                int i = R.Next(0, EmptySquares.Count - 1);
                EmptySquares[i].HasMine = true;
                EmptySquares.RemoveAt(i);
                Amount--;
            }
        }
        /// <summary>
        /// Excludes certain squares from containing a mine. Occurs directly after the player clicks the first square on the field.
        /// </summary>
        /// <param name="FirstSquare"></param>
        private void FirstClickGeneration(Square FirstSquare)
        {
            //Excludes the first clicked square from generation.
            EmptySquares.Remove(FirstSquare);


            //Finds all 8 squares around the first click and excludes these from generation (if possible).
            int i = XSize * YSize - AmountOfMines - 1;
            List<Square> FirstAdjacentSquares = LocateSurrounding(FirstSquare);

            foreach (Square S in FirstAdjacentSquares)
            {
                if (i <= 0) { break; }
                //Excludes square from generation.
                EmptySquares.Remove(S);
                i--;
            }

            //Adds mines to all available, empty squares.
            AddExtraMines(this.AmountOfMines);

            //Gets the surrounding mine count for empty squares on the field.
            foreach (Square S in EmptySquares)
            {
                if (S == null) { continue; }
                S.SurroundingMines = CountSurroundingMines(S);
            }

            //AFTER generation, count how many mines surround the first square.
            FirstSquare.SurroundingMines = CountSurroundingMines(FirstSquare);
            //Count all surrounding mines for the squares directly adjacent to the first square.
            foreach (Square S in FirstAdjacentSquares)
            {
                if (S.HasMine) { continue; }
                S.SurroundingMines = CountSurroundingMines(S);
                EmptySquares.Add(S);
            }
        }

        /// <summary>
        /// Counts the number of mines surrounding the given square.
        /// </summary>
        /// <param name="S">The square from which you want to know the amount of adjacent mines.</param>
        /// <returns></returns>
        private int CountSurroundingMines(Square S)
        { return LocateSurrounding(S).Count(x => x.HasMine); }

        /// <summary>
        /// Returns a square based on its location on the playing field.
        /// </summary>
        /// <param name="x">The x coordination of the square.</param>
        /// <param name="y">The y coordination of the square.</param>
        /// <returns></returns>
        public Square GetSquare(int x, int y)
        {
            if (x < 1 || y < 1) { return null; }
            if (x > XSize || y > YSize) { return null; }

            try
            { return SquareCollection[x][y]; }
            catch { return null; }
        }
        /// <summary>
        /// Returns a square based on its location on the playing field.
        /// </summary>
        /// <param name="P">The point or coordinates of the square.</param>
        /// <returns></returns>
        public Square GetSquare(Point P)
        { return GetSquare(P.X, P.Y); }
        /// <summary>
        /// Returns a list of all the squares surrounding the given square.
        /// </summary>
        /// <param name="S">The square from which you want to find the surrounding squares.</param>
        /// <returns></returns>
        public List<Square> LocateSurrounding(Square S)
        {
            List<Square> SurroundingSquares = new List<Square>();
            List<Point> SurroundingPoints = new List<Point>();
            #region Surroundingsquares
            //Left-Up
            SurroundingPoints.Add(new Point(S.Location.X - 1, S.Location.Y - 1));
            //Up
            SurroundingPoints.Add(new Point(S.Location.X, S.Location.Y - 1));
            //Right-Up
            SurroundingPoints.Add(new Point(S.Location.X + 1, S.Location.Y - 1));
            //Left
            SurroundingPoints.Add(new Point(S.Location.X - 1, S.Location.Y));
            //Right
            SurroundingPoints.Add(new Point(S.Location.X + 1, S.Location.Y));
            //Left-Down
            SurroundingPoints.Add(new Point(S.Location.X - 1, S.Location.Y + 1));
            //Down
            SurroundingPoints.Add(new Point(S.Location.X, S.Location.Y + 1));
            //Right-Down
            SurroundingPoints.Add(new Point(S.Location.X + 1, S.Location.Y + 1));
            #endregion

            foreach (Point p in SurroundingPoints)
            { SurroundingSquares.Add(GetSquare(p)); }

            //Removes all instances that are null or already clicked.
            SurroundingSquares.RemoveAll(x => x == null);
            SurroundingSquares.RemoveAll(x => x.IsVisible);
            return SurroundingSquares;
        }

        /// <summary>
        /// Handles left clicking on a square.
        /// </summary>
        /// <param name="S">The square matching the clicked button.</param>
        /// <param name="B">The button that was left clicked.</param>
        private void SquareLeftClicked(Square S, Button B)
        {
            if (S == null) { return; }
            if (B == null) { return; }

            //If this is the first square clicked, start generating mines around this point.
            if (FirstClick)
            {
                FirstClickGeneration(S);
                FirstClick = false;
            }

            //Removes events from this square since it's being clicked (and thus can't ever be clicked again).
            S.LSQEvent -= SquareLeftClicked;
            S.RSQEvent -= SquareRightClicked;
            B.MouseClick -= S.SquareLeftClicked;
            B.MouseUp -= S.SquareRightClicked;

            //If a mine is clicked, display where all the mines are and lose the game.
            if (S.HasMine)
            { ShowAllMines(); return; }

            //If the square is marked with a questionmark, remove it.
            if(S.MarkState == SquareState.Question)
            { B.BackgroundImage = null; }

            //Change button to appear clicked.
            B.BackColor = ColorSettings.ClickedSQ;
            B.ForeColor = ColorSettings.GetColor(S.SurroundingMines);
            B.FlatStyle = FlatStyle.Flat;
            B.Text = S.SurroundingMines == 0 ? "" : S.SurroundingMines.ToString();
            EmptySquares.Remove(S);
            S.IsVisible = true;


            //Checks all surrounding squares.
            //If THIS square is 0, all surrounding squares will be clicked regardless of values.
            //If the adjacent square is 0, but this square has another value, the adjacent square will be clicked.
            List<Square> SurroundingSquares = LocateSurrounding(S);
            foreach (Square s in SurroundingSquares)
            {
                if (S.SurroundingMines == 0)
                { s.SquareLeftClicked(s.ButtonControl, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0)); }
            }

            //If there are no empty squares left, win the game.
            if(EmptySquares.Count<=0)
            {
                if(eGameWon!=null)
                { eGameWon(); }
            }
        }
        /// <summary>
        /// Handles right clicking on a square.
        /// </summary>
        /// <param name="S">The square matching the clicked button.</param>
        /// <param name="B">The button that was right clicked.</param>
        private void SquareRightClicked(Square S, Button B)
        {
            if (S == null) { return; }
            if (B == null) { return; }

            //Handles the different squarestates of the given square.
            switch(S.MarkState)
            {
                //If the square is unmarked and clicked, flag the square.
                case SquareState.Unmarked:
                    {
                        //Stop the player from marking squares when they are out of flags.
                        if (FlagsLeft > 0)
                        {
                            FlagsLeft--;
                            B.BackgroundImage = ColorSettings.FlagImage;
                            B.BackgroundImageLayout = ImageLayout.Stretch;
                            S.MarkState = SquareState.Flagged;
                        }
                        break;
                    }
                //If the square is flagged and clicked, mark it with a questionmark.
                case SquareState.Flagged:
                    {
                        B.BackgroundImage = ColorSettings.QuestionMarkImage;
                        FlagsLeft++;
                        S.MarkState = SquareState.Question;
                        break;
                    }
                //If the square is marked with a questionmark and clicked, unmark it.
                case SquareState.Question:
                    {

                        B.BackgroundImage = null;
                        S.MarkState = SquareState.Unmarked;
                        break;
                    }
            }

            //If all the mines are marked, win the game.
            //if (MinesLeft <= 0)
            //{
            //    if (eGameWon != null)
            //    { eGameWon(); }
            //}
        }

        /// <summary>
        /// Displays all mines and automatically raises a losegame event.
        /// </summary>
        private void ShowAllMines()
        {
            //Finds all squares with a mine and sets them to visible.
            foreach(var XPair in SquareCollection)
            {
                foreach(var YPair in XPair.Value)
                {
                    if (!YPair.Value.HasMine) { continue; }
                    //Sets the mines visible. This happens manually, not via an event.
                    YPair.Value.ButtonControl.BackgroundImage = ColorSettings.MineImage;
                    YPair.Value.ButtonControl.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }

            //Loses the game.
            if (eGameLost != null)
            { eGameLost(); }
        }
    }
}
