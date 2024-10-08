using OxLibrary;
using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Editor;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data.Types;
using OxDAOEngine.Data;
using PlayStationGames.ConsoleEngine.Editor;
using PlayStationGames.GameEngine.Editor;

namespace PlayStationGames.AccountEngine.Editor
{
    public class AccountWorker : DAOWorker<AccountField, Account, AccountFieldGroup>
    {
        public AccountWorker() : base() { }

        protected override void PrepareStyles()
        {
            base.PrepareStyles();
            ControlPainter.ColorizeControl(consolesButton, Editor.MainPanel.BaseColor);
            ControlPainter.ColorizeControl(gamesButton, Editor.MainPanel.BaseColor);
            consolesWorker.BaseColor = Editor.MainPanel.BaseColor;
            gamesWorker.BaseColor = Editor.MainPanel.BaseColor;
        }

        protected override bool SyncFieldValue(AccountField field, bool byUser) => 
            field == AccountField.Type;

        protected override EditorLayoutsGenerator<AccountField, Account, AccountFieldGroup> 
            CreateLayoutsGenerator(FieldGroupFrames<AccountField, AccountFieldGroup> frames, 
                ControlLayouter<AccountField, Account> layouter) =>
                new AccountEditorLayoutsGenerator(frames, layouter);

        protected override void AfterLayoutControls()
        {
            base.AfterLayoutControls();
            Editor.Groups[AccountFieldGroup.Consoles].SizeChanged -= ConsolesSizeChangedHandler;
            Editor.Groups[AccountFieldGroup.Consoles].SizeChanged += ConsolesSizeChangedHandler;
            Editor.Groups[AccountFieldGroup.Games].SizeChanged -= GamesSizeChangedHandler;
            Editor.Groups[AccountFieldGroup.Games].SizeChanged += GamesSizeChangedHandler;

            consolesLabel.Parent = Editor.Groups[AccountFieldGroup.Consoles];
            consolesLabel.Left = 8;
            consolesButton.Parent = Editor.Groups[AccountFieldGroup.Consoles];
            consolesButton.Dock = DockStyle.Right;
            consolesButton.SetContentSize(consolesButton.Parent.Width / 3, 38);
            consolesButton.Click -= ConsoleButtonClickHandler;
            consolesButton.Click += ConsoleButtonClickHandler;

            gamesLabel.Parent = Editor.Groups[AccountFieldGroup.Games];
            gamesLabel.Left = 8;
            gamesButton.Parent = Editor.Groups[AccountFieldGroup.Games];
            gamesButton.Dock = DockStyle.Fill;
            gamesButton.Dock = DockStyle.Right;
            gamesButton.Click -= GamesButtonClickHandler;
            gamesButton.Click += GamesButtonClickHandler;

            Editor.Groups.SetGroupsSize();
            Editor.InvalidateSize();

            consolesWorker.Renew(Item);
            gamesWorker.Renew(Item);
            RenewConsolesLabel();
            RenewGamesLabel();
        }

        private void RenewConsolesLabel() => 
            consolesLabel.Text = consolesWorker.ConsolesCount > 0
                ? $"Registered on {consolesWorker.ConsolesCount} console{(consolesWorker.ConsolesCount > 1 ? "s" : string.Empty)}"
                : "Not registered on any console";

        private void RenewGamesLabel() => 
            gamesLabel.Text = gamesWorker.GamesCount > 0
                ? $"Owns {gamesWorker.GamesCount} game{(gamesWorker.GamesCount > 1 ? "s" : string.Empty)}"
                : "Not own any games";

        private void ConsolesSizeChangedHandler(object? sender, EventArgs e)
        {
            consolesButton.SetContentSize(consolesButton.Parent.Width / 3, 38);
            consolesLabel.Top = (consolesButton.Parent.Height - consolesLabel.Height) / 2;
        }

        private void GamesSizeChangedHandler(object? sender, EventArgs e)
        {
            gamesButton.SetContentSize(consolesButton.Parent.Width / 3, 38);
            gamesLabel.Top = (consolesButton.Parent.Height - gamesLabel.Height) / 2;
        }

        private void GamesButtonClickHandler(object? sender, EventArgs e)
        {
            if (Item == null)
                return;

            gamesWorker.Show(Editor);
            RenewGamesLabel();
        }

        private readonly GamesWorker gamesWorker = new();
        private readonly ConsolesWorker consolesWorker = new();

        private void ConsoleButtonClickHandler(object? sender, EventArgs e)
        {
            if (Item == null)
                return;

            consolesWorker.Show(Editor);
            RenewConsolesLabel();
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

        private readonly OxButton consolesButton = new("Consoles", OxIcons.Share)
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
            Font = new Font(Styles.FontFamily, Styles.DefaultFontSize)
        };

        private readonly OxButton gamesButton = new("Games", OxIcons.LinkedItems)
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
            Font = new Font(Styles.FontFamily, Styles.DefaultFontSize)
        };

        private readonly OxLabel consolesLabel = new()
        {
            Text = "Consoles count",
            Font = new Font(Styles.FontFamily, Styles.DefaultFontSize)
        };

        private readonly OxLabel gamesLabel = new()
        {
            Text = "Games count",
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
                Editor.Groups[AccountFieldGroup.Games].Visible = true;
            }
            else
            {
                Editor.Groups[AccountFieldGroup.Auth].Visible = false;
                Editor.Groups[AccountFieldGroup.Links].Visible = false;
                Editor.Groups[AccountFieldGroup.Games].Visible = false;
            }

            return afterSyncValues;
        }
    }
}