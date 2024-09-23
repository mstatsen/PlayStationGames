using OxLibrary.Dialogs;

namespace PlayStationGames
{
    partial class MainForm : OxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            SuspendLayout();
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(747, 554);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Location = new Point(0, 0);
            Margin = new Padding(4, 3, 4, 3);
            MaximumSize = new Size(2240, 1200);
            MinimumSize = new Size(700, 462);
            Name = "MainForm";
            Text = "Playstation Games";
            Load += MainFormLoad;
            Shown += MainFormShow;
            ResumeLayout(false);
        }

        #endregion
    }
}

