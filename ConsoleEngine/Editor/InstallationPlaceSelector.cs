﻿using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Dialogs;
using OxLibrary.Panels;
using OxXMLEngine;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;

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
            dialog.MainPanel.SetContentSize(600,
                (generationHelper.FolderSupport(Generation)
                    ? folderControl.Bottom
                    : storageControl.Bottom
                )
                + 22);
            dialog.DialogButtons = OxDialogButton.Apply | OxDialogButton.Cancel;

            if (GamesCount > 1)
                dialog.DialogButtons |= OxDialogButton.ApplyForAll;

            dialog.GetEmptyMandatoryFieldName = GetEmptyMandatoryFieldHandler;
        }

        private OxLabel CreateLabel(string caption, Control control) => 
            (OxLabel)OxControlHelper.AlignByBaseLine(control,
                new OxLabel()
                {
                    Parent = this,
                    AutoSize = true,
                    Left = 8,
                    Text = caption,
                    Font = new Font(EngineStyles.DefaultFont, FontStyle.Regular)
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
            control.Top = lastBottom == -1 ? 8 : lastBottom + 4;
            control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            control.Width = Width - control.Left - 8;
            control.Height = 32;
            return control.Bottom;
        }

        private string GetEmptyMandatoryFieldHandler() =>
            storageControl != null && SelectedStorageId == Guid.Empty
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