﻿using OxLibrary;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    partial class SeriesEditor
    {
        
        /// Required designer variable.
        
        private System.ComponentModel.IContainer components = null;

        
        /// Clean up any resources being used.
        
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

        
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Size = new OxSize(OxWh.W390, OxWh.W230);
            this.MainPanel.Text = "Tag";
            // 
            // LinkEditor
            // 
            this.ClientSize = new System.Drawing.Size(390, 230);
            this.MaximumSize = new System.Drawing.Size(390, 230);
            this.MinimumSize = new System.Drawing.Size(390, 230);
            this.Name = "TagEditor";
            this.Text = "Tag";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
