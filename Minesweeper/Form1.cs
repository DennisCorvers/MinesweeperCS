using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// The current playing field instance.
        /// </summary>
        private Field m_field;
        /// <summary>
        /// The offset to the left of the playing field.
        /// </summary>
        private const int XoffSet = 20;
        /// <summary>
        /// The offset to the top of the playing field.
        /// </summary>
        private const int YoffSet = 50;
        /// <summary>
        /// The offset between the gamefield and the numeric controls.
        /// </summary>
        private const int numoffSet = 20;
        /// <summary>
        /// The size of every square on the field.
        /// </summary>
        private const int MySize = 21;
        /// <summary>
        /// A list of all of the buttons simulating a square.
        /// </summary>
        private List<Button> SquareFields;
        private const int MinimumWidth = 275;
        private const int MinimumHeight = 275;

        public Form1()
        {
            SquareFields = new List<Button>();
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        /// <summary>
        /// Creates a new playing field and starts generating it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            gamepanel.SuspendLayout();

            //Remove the GUI buttons from any last sessions.
            gamepanel.Visible = false;
            foreach (Button b in SquareFields)
                b.Dispose();

            SquareFields.Clear();

            GeneratePlayingField();

            gamepanel.Visible = true;
            gamepanel.ResumeLayout();
        }

        private void GeneratePlayingField()
        {
            int xsize = Convert.ToInt32(numxSize.Value);
            int ysize = Convert.ToInt32(numySize.Value);
            int mines = Convert.ToInt32(numMines.Value);
            //Create new field instance with all squares
            m_field = new Field(xsize, ysize, mines, CreateButtonForSquare);
            m_field.OnCounterChanged += UpdateCounter;
            m_field.OnGameLost += GameLost;
            m_field.OnGameWon += GameWon;

            SetupForm(m_field.XSize, m_field.YSize, m_field.AmountOfMines);
            gamepanel.Controls.AddRange(SquareFields.ToArray());
        }

        private Button CreateButtonForSquare(Point location)
        {
            Button b = new Button
            {
                BackColor = ColorSettings.DefaultSQ,
                Location = new Point((location.X) * MySize, (location.Y) * MySize),
                Size = new Size(MySize, MySize)
            };
            b.FlatStyle = FlatStyle.Standard;
            b.FlatAppearance.BorderColor = Color.Black;
            b.Font = ColorSettings.ButtonFont;

            SquareFields.Add(b);

            return b;
        }

        /// <summary>
        /// Sets up the controls on the form. Handles resizing and posotioning of controls.
        /// </summary>
        /// <param name="xSize">The xsize of the playing field.</param>
        /// <param name="ySize">The y size of the playing field.</param>
        /// <param name="Mines">The amount of mines on the playing field.</param>
        private void SetupForm(int xSize, int ySize, int Mines)
        {
            //Set the amount of mines manually the first time since the events are set after the generation.
            lMinesLeft.Text = Mines.ToString();
            Point PanelLocation = new Point(0, 0);
            Size FormSize = new Size(0, 0);
            if (xSize * MySize + XoffSet * 2 + 10 < MinimumWidth)
            {
                PanelLocation.X = MinimumWidth / 2 - (xSize * MySize / 2) - 10;
                FormSize.Width = MinimumWidth;
            }
            else
            {
                PanelLocation.X = XoffSet;
                FormSize.Width = xSize * MySize + XoffSet * 2 + 10;
            }
            if (ySize * MySize + YoffSet + numoffSet + 40 > MinimumHeight)
                FormSize.Height = ySize * MySize + numoffSet + 40 * 2 + YoffSet;
            else
                FormSize.Height = MinimumHeight;

            MinimumSize = FormSize;
            MaximumSize = FormSize;

            numMines.Location = new Point(numMines.Location.X, FormSize.Height - 70);
            numxSize.Location = new Point(numxSize.Location.X, FormSize.Height - 70);
            numySize.Location = new Point(numySize.Location.X, FormSize.Height - 70);

            lxsize.Location = new Point(lxsize.Location.X, FormSize.Height - 86);
            lysize.Location = new Point(lysize.Location.X, FormSize.Height - 86);
            lmines.Location = new Point(lmines.Location.X, FormSize.Height - 86);

            PanelLocation.Y = YoffSet;
            gamepanel.Location = PanelLocation;
            gamepanel.Size = new Size(xSize * MySize, ySize * MySize);
            gamepanel.BorderStyle = BorderStyle.None;
            gamepanel.Visible = true;
        }

        /// <summary>
        /// Automacially adjusts the maximum amount of mines based on the x and y size of the field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericSizeChange(object sender, EventArgs e)
        {
            if (!(sender is NumericUpDown))
                return;

            numMines.Maximum = numxSize.Value * numySize.Value - 1;
        }

        private void UpdateCounter(int FlagsLeft)
            => lMinesLeft.Text = FlagsLeft.ToString();

        private void GameWon()
        {
            m_field.OnGameWon -= GameWon;
            MessageBox.Show("All " + m_field.AmountOfMines.ToString() + " mines cleared!");
            DisableAllButtons();
        }

        private void GameLost()
        {
            m_field.OnGameLost -= GameLost;
            MessageBox.Show("You lost! Try again!");
            DisableAllButtons();
        }

        private void DisableAllButtons()
        {
            foreach (Button B in SquareFields)
                B.Enabled = false;
        }
    }
}
