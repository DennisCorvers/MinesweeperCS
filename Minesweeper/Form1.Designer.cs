namespace Minesweeper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.bStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lMinesLeft = new System.Windows.Forms.Label();
            this.gamepanel = new System.Windows.Forms.Panel();
            this.numxSize = new System.Windows.Forms.NumericUpDown();
            this.numySize = new System.Windows.Forms.NumericUpDown();
            this.numMines = new System.Windows.Forms.NumericUpDown();
            this.lxsize = new System.Windows.Forms.Label();
            this.lysize = new System.Windows.Forms.Label();
            this.lmines = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numxSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numySize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMines)).BeginInit();
            this.SuspendLayout();
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(174, 12);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(53, 23);
            this.bStart.TabIndex = 0;
            this.bStart.Text = "Start";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Flags Left";
            // 
            // lMinesLeft
            // 
            this.lMinesLeft.AutoSize = true;
            this.lMinesLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMinesLeft.Location = new System.Drawing.Point(108, 9);
            this.lMinesLeft.Name = "lMinesLeft";
            this.lMinesLeft.Size = new System.Drawing.Size(19, 20);
            this.lMinesLeft.TabIndex = 3;
            this.lMinesLeft.Text = "0";
            this.lMinesLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gamepanel
            // 
            this.gamepanel.BackColor = System.Drawing.SystemColors.Control;
            this.gamepanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gamepanel.Location = new System.Drawing.Point(20, 50);
            this.gamepanel.Name = "gamepanel";
            this.gamepanel.Size = new System.Drawing.Size(207, 98);
            this.gamepanel.TabIndex = 4;
            this.gamepanel.Visible = false;
            // 
            // numxSize
            // 
            this.numxSize.Location = new System.Drawing.Point(20, 205);
            this.numxSize.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numxSize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numxSize.Name = "numxSize";
            this.numxSize.Size = new System.Drawing.Size(47, 20);
            this.numxSize.TabIndex = 0;
            this.numxSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numxSize.ValueChanged += new System.EventHandler(this.NumericSizeChange);
            // 
            // numySize
            // 
            this.numySize.Location = new System.Drawing.Point(110, 205);
            this.numySize.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numySize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numySize.Name = "numySize";
            this.numySize.Size = new System.Drawing.Size(47, 20);
            this.numySize.TabIndex = 5;
            this.numySize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numySize.ValueChanged += new System.EventHandler(this.NumericSizeChange);
            // 
            // numMines
            // 
            this.numMines.Location = new System.Drawing.Point(200, 205);
            this.numMines.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numMines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMines.Name = "numMines";
            this.numMines.Size = new System.Drawing.Size(47, 20);
            this.numMines.TabIndex = 6;
            this.numMines.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lxsize
            // 
            this.lxsize.AutoSize = true;
            this.lxsize.Location = new System.Drawing.Point(13, 189);
            this.lxsize.Name = "lxsize";
            this.lxsize.Size = new System.Drawing.Size(62, 13);
            this.lxsize.TabIndex = 7;
            this.lxsize.Text = "Field X-Size";
            // 
            // lysize
            // 
            this.lysize.AutoSize = true;
            this.lysize.Location = new System.Drawing.Point(103, 189);
            this.lysize.Name = "lysize";
            this.lysize.Size = new System.Drawing.Size(62, 13);
            this.lysize.TabIndex = 8;
            this.lysize.Text = "Field Y-Size";
            // 
            // lmines
            // 
            this.lmines.AutoSize = true;
            this.lmines.Location = new System.Drawing.Point(197, 189);
            this.lmines.Name = "lmines";
            this.lmines.Size = new System.Drawing.Size(55, 13);
            this.lmines.TabIndex = 9;
            this.lmines.Text = "No. Mines";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 237);
            this.Controls.Add(this.lmines);
            this.Controls.Add(this.lysize);
            this.Controls.Add(this.lxsize);
            this.Controls.Add(this.numMines);
            this.Controls.Add(this.numySize);
            this.Controls.Add(this.numxSize);
            this.Controls.Add(this.gamepanel);
            this.Controls.Add(this.lMinesLeft);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(275, 275);
            this.MinimumSize = new System.Drawing.Size(275, 275);
            this.Name = "Form1";
            this.Text = "MineSweeper";
            ((System.ComponentModel.ISupportInitialize)(this.numxSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numySize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lMinesLeft;
        private System.Windows.Forms.Panel gamepanel;
        private System.Windows.Forms.NumericUpDown numxSize;
        private System.Windows.Forms.NumericUpDown numySize;
        private System.Windows.Forms.NumericUpDown numMines;
        private System.Windows.Forms.Label lxsize;
        private System.Windows.Forms.Label lysize;
        private System.Windows.Forms.Label lmines;
    }
}

