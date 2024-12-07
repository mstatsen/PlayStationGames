using OxLibrary;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    partial class DeviceEditor
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
            // FormPanel
            // 
            this.FormPanel.Size = new OxSize(OxWh.W390, OxWh.W230);
            // 
            // DeviceEditor
            // 
            this.ClientSize = new OxSize(OxWh.W390, OxWh.W230);
            this.MaximumSize = new OxSize(OxWh.W390, OxWh.W230);
            this.MinimumSize = new OxSize(OxWh.W390, OxWh.W230);
            this.Name = "DeviceEditor";
            this.Text = "Device";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
