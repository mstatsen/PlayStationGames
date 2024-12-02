using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using OxLibrary.Forms;

namespace PlayStationGames.ConsoleEngine.Editor
{
    public class InstallationPlaceSelector : OxPanel
    {
        internal readonly OxComboBox storageControl = new();
        internal readonly OxComboBox folderControl = new();
        private readonly OxLabel storageLabel;
        private readonly OxLabel folderLabel;

        private static ControlBuilder<ConsoleField, PSConsole> Builder =>
            DataManager.Builder<ConsoleField, PSConsole>(ControlScope.Editor);

        public InstallationPlaceSelector() : base()
        {
            int lastBottom = PrepareControl(storageControl);
            PrepareControl(folderControl, lastBottom);
            storageLabel = CreateLabel("Storage", storageControl);
            folderLabel = CreateLabel("Folder", folderControl);
            Text = "Installation placement";
        }

        public int GamesCount = 0;
        public Game? Game;

        protected override void PrepareDialog(OxPanelViewer dialog)
        {
            base.PrepareDialog(dialog);
            Renew();
            dialog.Text = $"Install {Game} into:";
            dialog.MainPanel.Size = new(
                OxWh.W600,
                OxWh.Add(
                    generationHelper.FolderSupport(Generation)
                        ? folderControl.Bottom
                        : storageControl.Bottom
                    ,
                    OxWh.W22
                )
            );
            dialog.DialogButtons = OxDialogButton.Apply | OxDialogButton.Cancel;

            if (GamesCount > 1)
                dialog.DialogButtons |= OxDialogButton.ApplyForAll;

            dialog.GetEmptyMandatoryFieldName = GetEmptyMandatoryFieldHandler;
        }

        private OxLabel CreateLabel(string caption, Control control) => 
            OxControlHelper.AlignByBaseLine(control,
                new OxLabel()
                {
                    Parent = this,
                    AutoSize = true,
                    Left = OxWh.W8,
                    Text = caption,
                    Font = OxStyles.DefaultFont
                }
            )!;

        public Guid SelectedStorageId => 
            storageControl.SelectedItem is Storage storage 
                ? storage.Id 
                : Guid.Empty;

        public string SelectedFolderName =>
            folderControl.SelectedItem is Folder folder
                ? folder.Name
                : string.Empty;

        private int PrepareControl(Control control, int lastBottom = -1)
        {
            control.Parent = this;
            control.Left = 80;
            control.Top = lastBottom is -1 ? 8 : lastBottom + 4;
            control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            control.Width = OxWh.Int(OxWh.Sub(OxWh.Sub(Width, control.Left), OxWh.W8));
            control.Height = 32;
            return control.Bottom;
        }

        private string GetEmptyMandatoryFieldHandler() =>
            storageControl is not null
            && SelectedStorageId.Equals(Guid.Empty)
                ? "Storage"
                : string.Empty;

        private readonly ConsoleGenerationHelper generationHelper = 
            TypeHelper.Helper<ConsoleGenerationHelper>();

        private static ConsoleGeneration Generation => 
            Builder.Value<ConsoleGeneration>(ConsoleField.Generation);

        private void SetStorageAndFolderVisible()
        {
            bool storageSupported = generationHelper.StorageSupport(Generation);
            storageControl.Visible = storageSupported;
            storageLabel.Visible = storageSupported;

            bool folderSopported = generationHelper.FolderSupport(Generation);
            folderControl.Visible = folderSopported;
            folderLabel.Visible = folderSopported;
        }

        internal void Renew()
        {
            storageControl.Items.Clear();
            folderControl.Items.Clear();
            SetStorageAndFolderVisible();

            if (generationHelper.StorageSupport(Generation))
                foreach (Storage storage in Builder.Value<Storages>(ConsoleField.Storages)!)
                    storageControl.Items.Add(storage);

            if (generationHelper.FolderSupport(Generation))
                foreach (Folder folder in Builder.Value<Folders>(ConsoleField.Folders)!)
                    folderControl.Items.Add(folder);
        }
    }
}