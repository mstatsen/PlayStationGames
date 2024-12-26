using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Forms;
using OxLibrary.Handlers;
using OxLibrary.Panels;
using OxDAOEngine.Data;
using OxDAOEngine.Settings;
using OxDAOEngine.Settings.Part;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.AccountEngine.Data;
using OxLibrary.Geometry;

namespace PlayStationGames
{
    public partial class MainForm : OxForm, IDataReceiver
    {
        public void DataModifiedHandler(DAO dao, DAOModifyEventArgs e) =>
            toolBar.Actions[OxToolbarAction.Save].SetEnabled(
                toolBar.Actions[OxToolbarAction.Save].IsEnabled || e.Modified
            );

        public override Bitmap? FormIcon =>
            Resources.PS.ToBitmap();

        public MainForm()
        {
            DataReceivers.Register(this);
            InitializeComponent();

            Size = MinimumSize;
            MoveToScreenCenter();
            
            mainTabControl = new OxTabControl
            {
                Dock = OxDock.Fill,
                Parent = FormPanel,
                Font = OxStyles.DefaultFont,
                BaseColor = BaseColor,
                TabHeaderSize = new(140, 32),
                TabPosition = OxDock.Top
            };
            mainTabControl.Margin.Size = 2;

            toolBar = new OxToolBar<OxButton>()
            {
                Parent = FormPanel,
                Dock = OxDock.Top,
                BaseColor = BaseColor,
                Visible = OxB.F,
                ToolbarActionClick = MainToolBarActoinClickHandler
            };

            toolBar.AddButton(OxToolbarAction.Save).Enabled = OxB.F;
            toolBar.AddButton(OxToolbarAction.Settings, true, OxDock.Right);
            toolBar.SendToBack();
            toolBar.Borders.Top = 0;
            toolBar.Padding.Top = 2;
            loadingPanel = new OxLoadingPanel()
            {
                Parent = FormPanel,
                UseParentColor = false
            };
            loadingPanel.Margin.Size = 0;
            loadingPanel.Borders.Size = 0;
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

                foreach (OxPanel face in DataManager.Faces)
                    face.BaseColor = BaseColor;

                OxSize screenSize = OxControlHelper.CurrentScreenSize(this);
                Size = new(
                    OxSh.Min(1600, screenSize.Width),
                    OxSh.Min(800, screenSize.Height));

                MoveToScreenCenter();
                PlaceComponents();
                Update();
                DataManager.SetModifiedHandler(DataModifiedHandler);
                DataReceivers.FillData();
            }
            finally
            {
                WithSuspendedLayout(
                    () => toolBar.Visible = OxB.T
                );
                loadingPanel.EndLoading();
            }
        }

        public override OxSize WantedMinimumSize => new((1240), (720));

        private void PlaceComponents()
        {
            foreach (OxPanel face in DataManager.Faces)
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
            toolBar.Actions[OxToolbarAction.Save].Enabled = OxB.F;
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