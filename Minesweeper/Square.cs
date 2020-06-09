using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public class Square
    {
        /// <summary>
        /// An event that gets raised when the square is left clicked.
        /// </summary>
        public event eSquareLeftClicked LSQEvent;
        /// <summary>
        /// An event that gets raised when the square is right clicked.
        /// </summary>
        public event eSquareRightClicked RSQEvent;
        public delegate void eSquareLeftClicked(Square s, Button b);
        public delegate void eSquareRightClicked(Square s, Button b);

        /// <summary>
        /// The button belonging to this square (as reference).
        /// </summary>
        public Button ButtonControl;
        /// <summary>
        /// Determines if this square is a mine or not.
        /// </summary>
        public bool HasMine;
        /// <summary>
        /// Determines if this square has been clicked already or not.
        /// </summary>
        public bool IsVisible;

        /// <summary>
        /// Determines if this square has been marked with a flag, questionmark or nothing.
        /// </summary>
        public SquareState MarkState;
        /// <summary>
        /// The location or coordinates of this square.
        /// </summary>
        public Point Location;
        private int _surroundingMines;
        /// <summary>
        /// The amount of mines surrounding this square. -1 if the square is a mine itself.
        /// </summary>
        public int SurroundingMines
        {
            get
            {
                if (HasMine) { return -1; }
                return _surroundingMines;
            }
            set
            {
                if (HasMine) { _surroundingMines = -1; }
                else { _surroundingMines = value < 0 ? 0 : value > 8 ? 8 : value; }
            }
        }

        /// <summary>
        /// Creates a new square.
        /// </summary>
        /// <param name="location">The location or coordinates of the square.</param>
        /// <param name="hasMine">Determines if this square has a mine or not.</param>
        public Square(Point location, bool hasMine = false)
        {
            this.Location = location;
            this.HasMine = hasMine;
            this.IsVisible = false;
            this.MarkState = SquareState.Unmarked;
        }

        public void SquareLeftClicked(object sender, MouseEventArgs e)
        {
            //Checks for left button click.
            if (e.Button != MouseButtons.Left) { return; }
            //If the square is already visible, ignore left click.
            if (IsVisible) { return; }
            //If the square is marked, ignore left click.
            if (MarkState == SquareState.Flagged) { return; }
            //Check the sender.
            if (!(sender is Button)) { return; }
            if (ButtonControl != sender) { ButtonControl = (Button)sender; }

            if (LSQEvent == null) { return; }
            LSQEvent(this, ButtonControl);
        }
        public void SquareRightClicked(object sender, MouseEventArgs e)
        {
            //Checks for right button click.
            if (e.Button != MouseButtons.Right) { return; }
            //If the square is already visible, ignore right click.
            if (IsVisible) { return; }
            //Check the sender.
            if (!(sender is Button)) { return; }
            if (ButtonControl != sender) { ButtonControl = (Button)sender; }

            if (RSQEvent == null) { return; }
            RSQEvent(this, ButtonControl);
        }
    }
}
