using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Dialogs;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Editor;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using System;

namespace PlayStationGames.AccountEngine.Editor
{
    public class AccountWorker : DAOWorker<AccountField, Account, AccountFieldGroup>
    {
        public AccountWorker() : base() { }

        protected override void PrepareStyles()
        {
            Editor.MainPanel.BaseColor = new OxColorHelper(Color.FromArgb(245, 251, 232)).Darker(7);
            ControlPainter.ColorizeControl(consolesButton, Editor.MainPanel.BaseColor);
            ControlPainter.ColorizeControl(gamesButton, Editor.MainPanel.BaseColor);
            consolesWorker.BaseColor = Editor.MainPanel.BaseColor;
            gamesWorker.BaseColor = Editor.MainPanel.BaseColor;
        }

        protected override bool SyncFieldValues(AccountField field, bool byUser)
        {
            if (new List<AccountField>{
                    AccountField.Name}
                .Contains(field))
                FillFormCaptionFromControls();

            return false;
        }

        private void FillFormCaptionFromControls() =>
            FillFormCaption(
                new Account() 
                { 
                    Name = Builder[AccountField.Name].StringValue
                });

        protected override EditorLayoutsGenerator<AccountField, Account, AccountFieldGroup> 
            CreateLayoutsGenerator(FieldGroupFrames<AccountField, AccountFieldGroup> frames, 
                ControlLayouter<AccountField, Account> layouter) =>
                new AccountEditorLayoutsGenerator(frames, layouter);

        protected override void AfterLayoutControls()
        {
            base.AfterLayoutControls();

            int buttonWidth = Editor.Groups[AccountFieldGroup.Property].Width / 2 - 26;
            consolesButton.Parent = Editor.Groups[AccountFieldGroup.Property];
            consolesButton.Top = 12;
            consolesButton.Left = 12;
            consolesButton.SetContentSize(buttonWidth, 40);
            consolesButton.Click -= ConsoleButtonClickHandler;
            consolesButton.Click += ConsoleButtonClickHandler;

            gamesButton.Parent = Editor.Groups[AccountFieldGroup.Property];
            gamesButton.Top = 12;
            gamesButton.Left = Editor.Groups[AccountFieldGroup.Property].Width / 2 - 2;
            gamesButton.SetContentSize(buttonWidth, 40);
            gamesButton.Click -= GamesButtonClickHandler;
            gamesButton.Click += GamesButtonClickHandler;

            Editor.Groups[AccountFieldGroup.Property].Height = gamesButton.Bottom;
            Editor.Groups.SetGroupsSize();
            Editor.InvalidateSize();

            consolesWorker.Renew(Item);
            gamesWorker.Renew(Item);
        }

        private void GamesButtonClickHandler(object? sender, EventArgs e)
        {
            if (Item != null)
                gamesWorker.Show();
        }

        private readonly GamesWorker gamesWorker = new();
        private readonly ConsolesWorker consolesWorker = new();

        private void ConsoleButtonClickHandler(object? sender, EventArgs e)
        {
            if (Item != null)
                consolesWorker.Show();
        }

        protected override List<List<AccountField>> LabelGroups =>
            new()
            {
                new List<AccountField> {
                    AccountField.Name,
                    AccountField.Country,
                },
                new List<AccountField> {
                    AccountField.Login,
                    AccountField.Password,
                }
            };

        protected override void BeforeGrabControls()
        {
            base.BeforeGrabControls();
        }

        protected override void AfterGrabControls()
        {
            base.AfterGrabControls();
            consolesWorker.Save();
            gamesWorker.Save();
        }

        private readonly OxButton consolesButton = new("Consoles", null)
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
            Font = new Font(Styles.FontFamily, Styles.DefaultFontSize)
        };

        private readonly OxButton gamesButton = new("Games", null)
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
            Font = new Font(Styles.FontFamily, Styles.DefaultFontSize)
        };

        protected override FieldGroupFrames<AccountField, AccountFieldGroup> GetFieldGroupFrames() =>
            new()
                {
                    Editor.Groups,
                    { AccountFieldGroup.System, Editor.MainPanel.Footer }
                };
    }
}