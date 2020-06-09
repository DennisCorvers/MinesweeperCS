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
        private Field F;
        /// <summary>
        /// The offset to the left of the playing field.
        /// </summary>
        private int XoffSet = 20;
        /// <summary>
        /// The offset to the top of the playing field.
        /// </summary>
        private int YoffSet = 50;
        /// <summary>
        /// The offset between the gamefield and the numeric controls.
        /// </summary>
        private int numoffSet = 20;
        /// <summary>
        /// The size of every square on the field.
        /// </summary>
        private new int Size = 21;
        /// <summary>
        /// A list of all of the buttons simulating a square.
        /// </summary>
        private List<Button> SquareFields;
        private static int MinimumWidth = 275;
        private static int MinimumHeight = 275;

        public Form1()
        {
            SquareFields = new List<Button>();
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        /// <summary>
        /// Creates a new playing field and starts generating it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.gamepanel.SuspendLayout();

            //Remove the GUI buttons from any last sessions.
            gamepanel.Visible = false;
            foreach (Button b in SquareFields)
            { b.Dispose(); }

            SquareFields.Clear();

            GeneratePlayingField();

            gamepanel.Visible = true;
            this.gamepanel.ResumeLayout();
        }

        private void GeneratePlayingField()
        {
            int xsize = Convert.ToInt32(numxSize.Value);
            int ysize = Convert.ToInt32(numySize.Value);
            int mines = Convert.ToInt32(numMines.Value);
            //Create new field instance with all squares
            F = new Field(xsize,ysize, mines);
            F.eCounterChanged += UpdateCounter;
            F.eGameLost += GameLost;
            F.eGameWon += GameWon;

            SetupForm(F.XSize, F.YSize, F.AmountOfMines);

            Font NewFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
            //Generate the GUI field with new controls.
            foreach (var Xpair in F.SquareCollection)
            {
                foreach (var YPair in Xpair.Value)
                {
                    //Set all properties for the button.
                    Button b = new Button();
                    b.BackColor = ColorSettings.DefaultSQ;
                    b.Location = new Point((YPair.Value.Location.X-1) * Size, (YPair.Value.Location.Y-1) * Size);
                    b.Size = new Size(Size, Size);
                    b.MouseClick += YPair.Value.SquareLeftClicked;
                    b.MouseUp += YPair.Value.SquareRightClicked;
                    b.FlatStyle = FlatStyle.Standard;
                    b.FlatAppearance.BorderColor = Color.Black;
                    b.Font = NewFont;

                    //Add a reference from the button to the square.
                    YPair.Value.ButtonControl = b;
                    SquareFields.Add(b);
                }
            }

            this.gamepanel.Controls.AddRange(SquareFields.ToArray());
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
            if (xSize * Size + XoffSet * 2 + 10 < MinimumWidth)
            {
                PanelLocation.X = MinimumWidth / 2 - (xSize * Size / 2) - 10;
                FormSize.Width = MinimumWidth;
            }
            else
            {
                PanelLocation.X = XoffSet;
                FormSize.Width = xSize * Size + XoffSet * 2 + 10;
            }
            if (ySize * Size + YoffSet + numoffSet + 40 > MinimumHeight)
            { FormSize.Height = ySize * Size + numoffSet + 40 * 2 + YoffSet; }
            else
            { FormSize.Height = MinimumHeight; }

            this.MinimumSize = FormSize;
            this.MaximumSize = FormSize;
            numMines.Location = new Point(numMines.Location.X, FormSize.Height - 70);
            numxSize.Location = new Point(numxSize.Location.X, FormSize.Height - 70);
            numySize.Location = new Point(numySize.Location.X, FormSize.Height - 70);
            lxsize.Location = new Point(lxsize.Location.X, FormSize.Height - 86);
            lysize.Location = new Point(lysize.Location.X, FormSize.Height - 86);
            lmines.Location = new Point(lmines.Location.X, FormSize.Height - 86);
            PanelLocation.Y = YoffSet;
            gamepanel.Location = PanelLocation;
            gamepanel.Size = new Size(xSize * Size, ySize * Size);
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
            if (!(sender is NumericUpDown)) { return; }
            numMines.Maximum = numxSize.Value * numySize.Value - 1;
        }

        /// <summary>
        /// Updates the flag counter.
        /// </summary>
        /// <param name="FlagsLeft">The amount of flags left.</param>
        private void UpdateCounter(int FlagsLeft)
        { lMinesLeft.Text = FlagsLeft.ToString(); }
        /// <summary>
        /// Handles events after winning the game.
        /// </summary>
        private void GameWon()
        {
            F.eGameWon -= GameWon;
            MessageBox.Show("All " + F.AmountOfMines.ToString() + " mines cleared!");
            DisableAllButtons();
        }
        /// <summary>
        /// Handles events after losing the game.
        /// </summary>
        private void GameLost()
        {
            F.eGameLost -= GameLost;
            MessageBox.Show("You lost! Try again!");
            DisableAllButtons();
        }
        /// <summary>
        /// Disables all of the squares after finishing a game.
        /// </summary>
        private void DisableAllButtons()
        {
            foreach(Button B in SquareFields)
            {
                B.Enabled = false;
            }
        }
    }
}
