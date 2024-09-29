using OxLibrary;
using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Editor;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using OxDAOEngine.Data.Types;
using PlayStationGames.AccountEngine.Data.Types;
using OxDAOEngine.Data;

namespace PlayStationGames.AccountEngine.Editor
{
    public class AccountWorker : DAOWorker<AccountField, Account, AccountFieldGroup>
    {
        public AccountWorker() : base() { }

        protected override void PrepareStyles()
        {
            Editor.MainPanel.BaseColor = new OxColorHelper(
                TypeHelper.BackColor(Builder.Value<AccountType>(AccountField.Type))
            ).Darker(7);
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

            switch (field)
            { 
                case AccountField.Type:
                    RecalcButtonsAvailable();
                    break;
            }

            return field == AccountField.Type;
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

            
            consolesButton.Parent = Editor.Groups[AccountFieldGroup.Property];
            consolesButton.Top = 12;
            consolesButton.Left = 12;
            consolesButton.Click -= ConsoleButtonClickHandler;
            consolesButton.Click += ConsoleButtonClickHandler;

            gamesButton.Parent = Editor.Groups[AccountFieldGroup.Property];
            gamesButton.Top = 12;
            gamesButton.Left = Editor.Groups[AccountFieldGroup.Property].Width / 2 - 2;
            gamesButton.Click -= GamesButtonClickHandler;
            gamesButton.Click += GamesButtonClickHandler;
            RecalcButtonsAvailable();

            Editor.Groups[AccountFieldGroup.Property].Height = consolesButton.Bottom;
            Editor.Groups.SetGroupsSize();
            Editor.InvalidateSize();

            consolesWorker.Renew(Item);
            gamesWorker.Renew(Item);
        }

        private void RecalcButtonsAvailable()
        {
            bool gamesButtonVisible = TypeHelper.Value<AccountType>(Builder.Value(AccountField.Type)) == AccountType.PSN;
            gamesButton.Visible = gamesButtonVisible;
            int buttonWidth = gamesButtonVisible
                ? Editor.Groups[AccountFieldGroup.Property].Width / 2 - 26
                : Editor.Groups[AccountFieldGroup.Property].Width - 42;
            consolesButton.SetContentSize(buttonWidth, 40);
            gamesButton.SetContentSize(buttonWidth, 40);
        }

        private void GamesButtonClickHandler(object? sender, EventArgs e)
        {
            if (Item != null)
                gamesWorker.Show(Editor);
        }

        private readonly GamesWorker gamesWorker = new();
        private readonly ConsolesWorker consolesWorker = new();

        private void ConsoleButtonClickHandler(object? sender, EventArgs e)
        {
            if (Item != null)
                consolesWorker.Show(Editor);
        }

        protected override List<List<AccountField>> LabelGroups =>
            new()
            {
                new List<AccountField> {
                    AccountField.Name,
                    AccountField.Type,
                    AccountField.Country,
                },
                new List<AccountField> {
                    AccountField.Login,
                    AccountField.Password,
                }
            };

        protected override void AfterGrabControls()
        {
            base.AfterGrabControls();
            consolesWorker.Save();
            gamesWorker.Save();
            ResetDefaultAccountIfExists();
        }

        private void ResetDefaultAccountIfExists()
        {
            if (Item != null && Item.DefaultAccount)
                foreach (Account account in DataManager.FullItemsList<AccountField, Account>()
                    .FindAll(a => a.DefaultAccount && a.Id != Item.Id)
                )
                    account.DefaultAccount = false;
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

        protected override bool SetGroupsAvailability(bool afterSyncValues = false)
        {
            if (Builder.Value<AccountType>(AccountField.Type) == AccountType.PSN)
            {
                Editor.Groups[AccountFieldGroup.Auth].Visible = true;
                Editor.Groups[AccountFieldGroup.Links].Visible = true;
                
            }
            else
            {
                Editor.Groups[AccountFieldGroup.Auth].Visible = false;
                Editor.Groups[AccountFieldGroup.Links].Visible = false;
            }

            return afterSyncValues;
        }
    }
}