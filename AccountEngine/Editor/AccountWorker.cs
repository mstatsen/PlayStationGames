using OxLibrary;
using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Editor;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data.Types;
using OxDAOEngine.Data;
using OxLibrary.Handlers;

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
            field is AccountField.Type;

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
            consolesLabel.Left = OxWh.W8;
            consolesButton.Parent = Editor.Groups[AccountFieldGroup.Consoles];
            consolesButton.Dock = OxDock.Right;
            consolesButton.Size = new(OxWh.Div(consolesButton.Parent.Width, OxWh.W3), OxWh.W38);
            consolesButton.Click -= ConsoleButtonClickHandler;
            consolesButton.Click += ConsoleButtonClickHandler;

            gamesLabel.Parent = Editor.Groups[AccountFieldGroup.Games];
            gamesLabel.Left = OxWh.W8;
            gamesButton.Parent = Editor.Groups[AccountFieldGroup.Games];
            gamesButton.Dock = OxDock.Fill;
            gamesButton.Dock = OxDock.Right;
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

        private void ConsolesSizeChangedHandler(object sender, OxSizeChangedEventArgs args)
        {
            consolesButton.Size = new(
                OxWh.Div(consolesButton.Parent!.Width, OxWh.W3), 
                OxWh.W38
            );
            consolesLabel.Top = OxWh.Div(OxWh.Sub(consolesButton.Parent.Height, consolesLabel.Height), 2);
        }

        private void GamesSizeChangedHandler(object sender, OxSizeChangedEventArgs args)
        {
            gamesButton.Size = new(
                OxWh.Div(consolesButton.Parent!.Width, OxWh.W3), 
                OxWh.W38);
            gamesLabel.Top = OxWh.Div(OxWh.Sub(consolesButton.Parent.Height, gamesLabel.Height), 2);
        }

        private void GamesButtonClickHandler(object? sender, EventArgs e)
        {
            if (Item is null)
                return;

            gamesWorker.Show(Editor);
            RenewGamesLabel();
        }

        private readonly GamesWorker gamesWorker = new();
        private readonly ConsolesWorker consolesWorker = new();

        private void ConsoleButtonClickHandler(object? sender, EventArgs e)
        {
            if (Item is null)
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
            if (Item is not null 
                && Item.DefaultAccount)
                foreach (Account account in DataManager.FullItemsList<AccountField, Account>()
                    .FindAll(a => 
                        a.DefaultAccount 
                        && !a.Id.Equals(Item.Id)
                    )
                )
                    account.DefaultAccount = false;
        }

        private readonly OxButton consolesButton = new("Consoles", OxIcons.Share)
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
            Font = OxStyles.DefaultFont
        };

        private readonly OxButton gamesButton = new("Games", OxIcons.LinkedItems)
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
            Font = OxStyles.DefaultFont
        };

        private readonly OxLabel consolesLabel = new()
        {
            Text = "Consoles count",
            Font = OxStyles.DefaultFont
        };

        private readonly OxLabel gamesLabel = new()
        {
            Text = "Games count",
            Font = OxStyles.DefaultFont
        };

        protected override FieldGroupFrames<AccountField, AccountFieldGroup> GetFieldGroupFrames() =>
            new()
                {
                    Editor.Groups,
                    { AccountFieldGroup.System, Editor.MainPanel.Footer }
                };

        protected override bool SetGroupsAvailability(bool afterSyncValues = false)
        {
            if (Builder.Value<AccountType>(AccountField.Type) is AccountType.PSN)
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