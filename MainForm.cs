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
            Size screenSize = Screen.GetWorkingArea(this).Size;
            SetContentSize(700, 480);
            Left = (screenSize.Width - 700) / 2;
            Top = (screenSize.Height - 480) / 2;
            
            mainTabControl = new OxTabControl
            {
                Dock = DockStyle.Fill,
                Parent = this,
                Font = Styles.DefaultFont,
                BaseColor = MainPanel.BaseColor,
                TabHeaderSize = new(140, 32),
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
            loadingPanel = new OxLoadingPanel()
            {
                Parent = MainPanel,
                UseParentColor = false
            };
            loadingPanel.Margins.SetSize(OxSize.None);
            loadingPanel.Borders.SetSize(OxSize.None);
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

                Size screenSize = Screen.GetWorkingArea(this).Size;
                SetContentSize(
                    Math.Min(1600, screenSize.Width),
                    Math.Min(800, screenSize.Height));

                Left = (screenSize.Width - Width) / 2;
                Top = (screenSize.Height - Height) / 2;

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

        public override Size WantedMinimumSize => new(1240, 720);

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
        private readonly OxToolBar toolBar;
    }
}