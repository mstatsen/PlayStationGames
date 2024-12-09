using OxLibrary;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    partial class InstallationEditor
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
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            this.FormPanel.Size = new OxSize(OxWh.W220, OxWh.W257);
            // 
            // DLCEditor
            // 
            this.ClientSize = new(220, 257);
            this.MaximumSize = new OxSize(OxWh.W220, OxWh.W257);
            this.MinimumSize = new OxSize(OxWh.W220, OxWh.W257);
            this.Name = "DLCEditor";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
