namespace WindowsFormsApp
{
    partial class Form
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.targetSpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellophaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shipsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extraShipScoreDIPSwitchOnBit3OfInputPort2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(267, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.targetSpeedToolStripMenuItem,
            this.cellophaneToolStripMenuItem,
            this.shipsToolStripMenuItem,
            this.extraShipScoreDIPSwitchOnBit3OfInputPort2ToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // targetSpeedToolStripMenuItem
            // 
            this.targetSpeedToolStripMenuItem.Name = "targetSpeedToolStripMenuItem";
            this.targetSpeedToolStripMenuItem.Size = new System.Drawing.Size(365, 22);
            this.targetSpeedToolStripMenuItem.Text = "Emulator Target Speed";
            // 
            // cellophaneToolStripMenuItem
            // 
            this.cellophaneToolStripMenuItem.Name = "cellophaneToolStripMenuItem";
            this.cellophaneToolStripMenuItem.Size = new System.Drawing.Size(365, 22);
            this.cellophaneToolStripMenuItem.Text = "Emulator Cellophane";
            // 
            // shipsToolStripMenuItem
            // 
            this.shipsToolStripMenuItem.Name = "shipsToolStripMenuItem";
            this.shipsToolStripMenuItem.Size = new System.Drawing.Size(365, 22);
            this.shipsToolStripMenuItem.Text = "Ship Count (DIP switches in bits 0 and 1 of input port 2)";
            // 
            // extraShipScoreDIPSwitchOnBit3OfInputPort2ToolStripMenuItem
            // 
            this.extraShipScoreDIPSwitchOnBit3OfInputPort2ToolStripMenuItem.Name = "extraShipScoreDIPSwitchOnBit3OfInputPort2ToolStripMenuItem";
            this.extraShipScoreDIPSwitchOnBit3OfInputPort2ToolStripMenuItem.Size = new System.Drawing.Size(365, 22);
            this.extraShipScoreDIPSwitchOnBit3OfInputPort2ToolStripMenuItem.Text = "Extra Ship Score (DIP switch on bit 3 of input port 2)";
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 322);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form";
            this.Text = "Space Invaders Emulator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem targetSpeedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shipsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extraShipScoreDIPSwitchOnBit3OfInputPort2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cellophaneToolStripMenuItem;
    }
}

