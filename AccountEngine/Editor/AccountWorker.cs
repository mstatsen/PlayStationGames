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
using OxLibrary.Geometry;

namespace PlayStationGames.AccountEngine.Editor
{
    public class AccountWorker : DAOWorker<AccountField, Account, AccountFieldGroup>
    {
        public AccountWorker() : base() { }

        protected override void PrepareStyles()
        {
            base.PrepareStyles();
            ControlPainter.ColorizeControl(consolesButton, Editor.BaseColor);
            ControlPainter.ColorizeControl(gamesButton, Editor.BaseColor);
            consolesWorker.BaseColor = Editor.BaseColor;
            gamesWorker.BaseColor = Editor.BaseColor;
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
            consolesLabel.Left = 8;
            consolesButton.Parent = Editor.Groups[AccountFieldGroup.Consoles];
            consolesButton.Dock = OxDock.Right;
            consolesButton.Size = ButtonSize;
            consolesButton.Click -= ConsoleButtonClickHandler;
            consolesButton.Click += ConsoleButtonClickHandler;

            gamesLabel.Parent = Editor.Groups[AccountFieldGroup.Games];
            gamesLabel.Left = 8;
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

        private OxSize ButtonSize =>
            new(
                OxSH.Third(consolesButton.Parent!.Width),
                38
            );

        private void ConsolesSizeChangedHandler(object sender, OxSizeChangedEventArgs args)
        {
            consolesButton.Size = ButtonSize;
            consolesLabel.Top = OxSH.CenterOffset(consolesButton.Parent!.Height, consolesLabel.Height);
        }

        private void GamesSizeChangedHandler(object sender, OxSizeChangedEventArgs args)
        {
            gamesButton.Size = ButtonSize;
            gamesLabel.Top = OxSH.CenterOffset(consolesButton.Parent!.Height, gamesLabel.Height);
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
                    { AccountFieldGroup.System, Editor.Footer }
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