using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Dialogs;
using OxLibrary.Panels;
using OxXMLEngine;
using OxXMLEngine.Data;
using OxXMLEngine.Settings;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.GameEngine.Data;

namespace PlayStationGames
{
    public partial class MainForm : OxForm, IDataReceiver
    {
        public void DataModifiedHandler(DAO dao, DAOModifyEventArgs e) =>
            toolBar.Actions[OxToolbarAction.Save].Enabled |= e.Modified;

        public override Bitmap FormIcon =>
            Resources.PS.ToBitmap();

        public MainForm()
        {
            InitializeDataManager();
            DataManager.Load();
            InitializeComponent();

            loadingPanel = new OxLoadingPanel()
            {
                Parent = MainPanel,
                UseParentColor = false
            };
            loadingPanel.Margins.SetSize(OxSize.None);
            loadingPanel.Borders.SetSize(OxSize.None);

            mainTabControl = new OxTabControl
            {
                Dock = DockStyle.Fill,
                Parent = this,
                Font = EngineStyles.DefaultFont,
                BaseColor = MainPanel.BaseColor,
                TabHeaderSize = new Size(140, 32),
                TabPosition = OxDock.Top
            };
            mainTabControl.Margins.SetSize(OxSize.Medium);

            toolBar = new OxToolBar()
            {
                Parent = this,
                Dock = DockStyle.Top,
                BaseColor = MainPanel.BaseColor,
                Visible = false,
                ToolbarActionClick = MainToolBarActoinClickHandler
            };

            toolBar.AddButton(OxToolbarAction.Save).Enabled = false;
            toolBar.AddButton(OxToolbarAction.Settings, true, DockStyle.Right);
            toolBar.SendToBack();
            toolBar.Borders.TopOx = OxSize.None;
            toolBar.Paddings.TopOx = OxSize.Medium;

            foreach (OxPane face in DataManager.Faces)
                face.BaseColor = MainPanel.BaseColor;

            Size screenSize = Screen.GetWorkingArea(this).Size;
            SetContentSize(
                Math.Min(1600, screenSize.Width),
                Math.Min(800, screenSize.Height));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (DataManager.Modified)
            {
                DialogResult userConfirm = OxMessage.ShowWarning(
                    "You have uncommitted changes. Do you want to save it?"
                );

                switch (userConfirm)
                {
                    case DialogResult.Yes:
                        Save();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }

            if (!e.Cancel)
            {
                DataReceivers.SaveSettings();
                DataManager.SaveSystemData();
            }
        }

        private void MainFormShow(object? sender, EventArgs e)
        {
            try
            {
                Update();
                DataManager.SetModifiedHandler(DataModifiedHandler);
                DataReceivers.FillData();
            }
            finally
            {
                SuspendLayout();

                try
                {
                    toolBar.Visible = true;
                }
                finally
                {
                    ResumeLayout();
                }

                loadingPanel.EndLoading();
            }
        }

        private void MainFormLoad(object? sender, EventArgs e)
        {
            loadingPanel.StartLoading();
            PlaceComponents();
        }

        public override Size WantedMinimumSize => new(1240, 720);

        private void PlaceComponents()
        {
            foreach (OxPane face in DataManager.Faces)
                mainTabControl.AddPage(face);

            mainTabControl.ActivateFirstPage();
        }

        private void MainToolBarActoinClickHandler(object? sender, ToolbarActionEventArgs EventArgs)
        {
            switch (EventArgs.Action)
            {
                case OxToolbarAction.Save:
                    Save();
                    break;
                case OxToolbarAction.Settings:
                    ShowSettings();
                    break;
            }
        }

        private void ShowSettings()
        {
            ISettingsController settings = SettingsManager.Settings<GeneralSettings>();
            SettingsPart part = SettingsPart.Full;
            IDataController? activeController = 
                mainTabControl.ActivePage == null 
                    ? null 
                    : DataManager.Controller(mainTabControl.ActivePage);

            if (activeController != null)
            {
                settings = activeController.Settings;
                part = activeController.ActiveSettingsPart;
            }

            SettingsForm.ShowSettings(settings, part);
        }

        private void Save()
        {
            DataManager.Save();
            toolBar.Actions[OxToolbarAction.Save].Enabled = false;
        }

        private static void InitializeDataManager()
        {
            ConsolesController.Init();
            GamesController.Init();
        }

        public void FillData() { }

        public void ApplySettings() => 
            WindowState = SettingsManager.Settings<GeneralSettings>().MainFormState;

        public void SaveSettings() => 
            SettingsManager.Settings<GeneralSettings>().MainFormState = WindowState;

        private readonly OxLoadingPanel loadingPanel;
        private readonly OxTabControl mainTabControl;
        private readonly OxToolBar toolBar;
    }
}