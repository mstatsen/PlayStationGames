using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Dialogs;
using OxLibrary.Panels;
using OxDAOEngine.Data;
using OxDAOEngine.Settings;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.AccountEngine.Data;
using OxDAOEngine.Settings.Part;

namespace PlayStationGames
{
    public partial class MainForm : OxForm, IDataReceiver
    {
        public void DataModifiedHandler(DAO dao, DAOModifyEventArgs e) =>
            toolBar.Actions[OxToolbarAction.Save].Enabled |= e.Modified;

        public override Bitmap? FormIcon =>
            Resources.PS.ToBitmap();

        public MainForm()
        {
            DataReceivers.Register(this);
            InitializeComponent();

            Size = MinimumSize;
            OxControlHelper.CenterForm(this);
            
            mainTabControl = new OxTabControl
            {
                Dock = OxDock.Fill,
                Parent = MainPanel,
                Font = Styles.DefaultFont,
                BaseColor = MainPanel.BaseColor,
                TabHeaderSize = new(OxWh.W140, OxWh.W32),
                TabPosition = OxDock.Top
            };
            mainTabControl.Margin.Size = OxWh.W2;

            toolBar = new OxToolBar<OxButton>()
            {
                Parent = MainPanel,
                Dock = OxDock.Top,
                BaseColor = MainPanel.BaseColor,
                Visible = false,
                ToolbarActionClick = MainToolBarActoinClickHandler
            };

            toolBar.AddButton(OxToolbarAction.Save).Enabled = false;
            toolBar.AddButton(OxToolbarAction.Settings, true, OxDock.Right);
            toolBar.SendToBack();
            toolBar.Borders.Top = OxWh.W0;
            toolBar.Padding.Top = OxWh.W2;
            loadingPanel = new OxLoadingPanel()
            {
                Parent = MainPanel,
                UseParentColor = false
            };
            loadingPanel.Margin.Size = OxWh.W0;
            loadingPanel.Borders.Size = OxWh.W0;
            loadingPanel.StartLoading();
        }

        private bool CheckModifiedAndSave()
        {
            if (DataManager.Modified)
            {
                DialogResult userConfirm = OxMessage.ShowWarning(
                    "You have uncommitted changes. Do you want to save it?",
                    this
               );

                switch (userConfirm)
                {
                    case DialogResult.Cancel:
                        return false;
                    case DialogResult.Yes:
                        Save();
                        break;
                }
            }

            return true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = !CheckModifiedAndSave();
            DataReceivers.SaveSettings();
            DataManager.SaveSystemData();
        }

        private void MainFormShow(object? sender, EventArgs e)
        {
            try
            {
                Update();
                InitializeDataManager();
                DataManager.Load();

                foreach (OxPane face in DataManager.Faces)
                    face.BaseColor = MainPanel.BaseColor;

                OxSize screenSize = OxControlHelper.ScreenSize(this);
                Size = new(
                    OxWh.Min(1600, screenSize.Width),
                    OxWh.Min(OxWh.W800, screenSize.Height));

                Left = OxWh.Div(OxWh.Sub(screenSize.Width, Width), OxWh.W2);
                Top = OxWh.Div(OxWh.Sub(screenSize.Height, Height), OxWh.W2);

                PlaceComponents();
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

        public override OxSize WantedMinimumSize => new(OxWh.W(1240), OxWh.W(720));

        private void PlaceComponents()
        {
            foreach (OxPane face in DataManager.Faces)
                mainTabControl.AddPage(face);

            mainTabControl.ActivateFirstPage();
        }

        private void MainToolBarActoinClickHandler(object? sender, OxActionEventArgs<OxToolbarAction> EventArgs)
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
                mainTabControl.ActivePage is null 
                    ? null 
                    : DataManager.Controller(mainTabControl.ActivePage);

            if (activeController is not null)
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
            AccountsController.Init();
            ConsolesController.Init();
            GamesController.Init();
        }

        public void FillData() { }

        public void ApplySettings(bool firstLoad)
        {
            WindowState = SettingsManager.Settings<GeneralSettings>().MainFormState;
            IDataController? controller = DataManager.Controller(SettingsManager.Settings<GeneralSettings>().CurrentController);

            if (controller is null)
                mainTabControl.ActivateFirstPage();
            else
                mainTabControl.ActivePage = controller.Face;
        }

        public void SaveSettings()
        {
            SettingsManager.Settings<GeneralSettings>().MainFormState = WindowState;
            SettingsManager.Settings<GeneralSettings>().CurrentController = DataManager.Controller(mainTabControl.ActivePage!).GetType().Name;
        }

        private readonly OxLoadingPanel loadingPanel;
        private readonly OxTabControl mainTabControl;
        private readonly OxToolBar<OxButton> toolBar;
    }
}