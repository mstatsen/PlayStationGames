using OxLibrary;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    partial class RelatedGameEditor
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
            this.MainPanel.Size = new OxSize(OxWh.W408, OxWh.W213);
            this.MainPanel.Text = "Related Game";
            // 
            // RelatedGameEditor
            // 
            this.ClientSize = new OxSize(OxWh.W408, OxWh.W213);
            this.MaximumSize = new OxSize(OxWh.W408, OxWh.W213);
            this.MinimumSize = new OxSize(OxWh.W408, OxWh.W213);
            this.Name = "RelatedGameEditor";
            this.Text = "Related Game";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
