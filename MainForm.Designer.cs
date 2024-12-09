using OxLibrary;
using OxLibrary.Forms;

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
            ClientSize = new(747, 554);
            Location = new OxPoint(0, 0);
            MaximumSize = new OxSize(1920, 1080);
            MinimumSize = new OxSize(700, 480);
            Name = "MainForm";
            Text = "Playstation Games";
            Shown += MainFormShow;
            ResumeLayout(false);
        }

        #endregion
    }
}

