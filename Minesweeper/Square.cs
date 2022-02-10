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
    public class Square
    {
        private int m_surroundingMines;
        private Button m_buttonControl;

        /// <summary>
        /// An event that gets raised when the square is left clicked.
        /// </summary>
        public event Action<Square> LSQEvent;
        /// <summary>
        /// An event that gets raised when the square is right clicked.
        /// </summary>
        public event Action<Square> RSQEvent;

        /// <summary>
        /// Determines if this square is a mine or not.
        /// </summary>
        public bool HasMine
        { get; set; }

        /// <summary>
        /// Determines if this square has been clicked already or not.
        /// </summary>
        public bool IsVisible
        { get; set; }

        /// <summary>
        /// Determines if this square has been marked with a flag, questionmark or nothing.
        /// </summary>
        public SquareState MarkState
        { get; set; }
        /// <summary>
        /// The location or coordinates of this square.
        /// </summary>
        public Point Location
        { get; }


        /// <summary>
        /// The amount of mines surrounding this square. -1 if the square is a mine itself.
        /// </summary>
        public int SurroundingMines
        {
            get
            {
                if (HasMine) { return -1; }
                return m_surroundingMines;
            }
            set
            {
                if (HasMine) { m_surroundingMines = -1; }
                else { m_surroundingMines = MathUtils.Clamp(value, 0, 8); }
            }
        }

        /// <summary>
        /// Creates a new square.
        /// </summary>
        /// <param name="location">The location or coordinates of the square.</param>
        /// <param name="hasMine">Determines if this square has a mine or not.</param>
        public Square(Point location, Button button, bool hasMine = false)
        {
            m_buttonControl = button;
            Location = location;
            HasMine = hasMine;
            IsVisible = false;
            MarkState = SquareState.Unmarked;

            m_buttonControl.MouseClick += SquareLeftClicked;
            m_buttonControl.MouseUp += SquareRightClicked;
        }

        public void MarkLeftClicked()
        {
            //If the square is already visible, ignore left click.
            if (IsVisible)
                return;

            //If the square is marked, ignore left click.
            if (MarkState == SquareState.Flagged)
                return;

            LSQEvent?.Invoke(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MarkRightClicked()
        {
            //If the square is already visible, ignore right click.
            if (IsVisible)
                return;

            RSQEvent?.Invoke(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SquareLeftClicked(object sender, MouseEventArgs e)
        {
            //Checks for left button click.
            if (e.Button != MouseButtons.Left)
                return;

            MarkLeftClicked();
        }

        public void SquareRightClicked(object sender, MouseEventArgs e)
        {
            //Checks for right button click.
            if (e.Button != MouseButtons.Right)
                return;

            MarkRightClicked();
        }

        public void RemoveClickEvents()
        {
            m_buttonControl.MouseClick += SquareLeftClicked;
            m_buttonControl.MouseUp += SquareRightClicked;
        }

        public void UpdateText()
        {
            m_buttonControl.BackColor = ColorSettings.ClickedSQ;
            m_buttonControl.ForeColor = ColorSettings.GetColor(m_surroundingMines);
            m_buttonControl.FlatStyle = FlatStyle.Flat;
            m_buttonControl.Text = m_surroundingMines == 0 ? "" : m_surroundingMines.ToString();
        }

        public void SetImage(Image mineImage)
        {
            m_buttonControl.BackgroundImage = mineImage;
            m_buttonControl.BackgroundImageLayout = ImageLayout.Stretch;
        }

        public void ClearImage()
        {
            m_buttonControl.BackgroundImage = null;
        }
    }
}
