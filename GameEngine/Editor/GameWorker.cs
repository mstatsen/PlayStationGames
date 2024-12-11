using OxLibrary.Controls;
using OxLibrary.Geometry;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Filter.Types;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Editor;
using PlayStationGames.AccountEngine.ControlFactory.Accessors;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.GameEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using PlayStationGames.GameEngine.ControlFactory.Controls.Trophies;

namespace PlayStationGames.GameEngine.Editor;

public class GameWorker : DAOWorker<GameField, Game, GameFieldGroup>
{
    public GameWorker() : base() { }

    protected override void AfterColorizeControls()
    {
        base.AfterColorizeControls();
        Builder.Control<OxPictureContainer>(GameField.Image).BaseColor = Editor.Colors.Lighter();
        Builder.Control<TrophysetPanel>(GameField.Trophyset).BaseColor = Editor.Colors.Lighter();
    }

    protected override void AfterFillControlsAndSetHandlers()
    {
        base.AfterFillControlsAndSetHandlers();
        SetRelatedGamesFilter();
    }

    protected override void AfterLayoutControls()
    {
        base.AfterLayoutControls();
        SetMaximumPlayersConstraints();
    }

    private void SetMaximumPlayersConstraints()
    {
        Builder[GameField.MaximumPlayers].MinimumValue = 2;
        Builder[GameField.MaximumPlayers].MaximumValue =
            TypeHelper.Helper<PlatformTypeHelper>().
                MaximumPlayersCount(Builder[GameField.Platform].EnumValue<PlatformType>());
    }

    protected override FieldGroupPanels<GameField, GameFieldGroup> GetFieldGroupFrames() =>
        new()
        {
            Editor.Groups,
            { GameFieldGroup.System, Editor.Footer }
        };

    protected override void AfterAlignLabels()
    {
        base.AfterAlignLabels();
        Control licenseCheckBox = Builder.Control<CheckBox>(GameField.Licensed);
        Control nameControl = Builder.Control<OxTextBox>(GameField.Name);
        licenseCheckBox.Left =
            (Layouter.PlacedControl(GameField.Source)!.Control.Right +
             nameControl.Right -
             licenseCheckBox.Width) / 2;
        licenseCheckBox.Top = nameControl.Top + (nameControl.Height - licenseCheckBox.Height) / 2;
        Builder.Control<OxPictureContainer>(GameField.Image).HiddenBorder = true;
        Builder[GameField.AvailablePlatinum].Text = "";
    }

    private void SetRelatedGamesFilter()
    {
        if (Item is null)
            return;

        Filter<GameField, Game> relatedGameFilter = new(FilterConcat.AND);
        relatedGameFilter.AddFilter(GameField.Id, FilterOperation.NotEquals, Item.Id, FilterConcat.AND);

        foreach (RelatedGame relatedGame in Builder.Value<RelatedGames>(GameField.RelatedGames)!)
            relatedGameFilter.AddFilter(
                GameField.Id, FilterOperation.NotEquals, relatedGame.GameId, FilterConcat.AND
            );

        Builder.Control<RelatedGamesControl>(GameField.RelatedGames).Filter = relatedGameFilter;
    }

    protected override bool SyncFieldValue(GameField field, bool byUser)
    {
        switch (field)
        {
            case GameField.RelatedGames:
                CurrentItem[GameField.RelatedGames] = Builder.Value(GameField.RelatedGames);
                SetRelatedGamesFilter();
                FillControls();
                break;
            case GameField.Platform:
                GenerateCurrentPlatformAsList();

                if (byUser)
                    SyncFormatWithPlatform();

                RemoveUnavailableDevices();
                RemoveUnavailableAppliesTo(field, byUser);
                FixReleasePlatform();
                FixAppliesTo();
                AddCurrentPlatformToAppliesTo();
                SyncOwnerWithControls(byUser);
                SetCoachMultiplayerVisible();
                //ShowMaximumPlayersControl();
                SetGenreControlsVisible();
                SetMaximumPlayersConstraints();
                break;
            case GameField.Licensed:
                SyncOwnerWithControls(byUser);
                SetTrophysetAvailability();
                break;
            case GameField.Source:
                SyncOwnerWithControls(byUser);
                break;
            case GameField.CoachMultiplayer:
                SetGenreControlsVisible(); 
                //ShowMaximumPlayersControl();
                break;
            case GameField.ReleasePlatforms:
                RemoveUnavailableAppliesTo(field, byUser);
                break;
        }

        return field is 
            GameField.Licensed or
            GameField.Platform or
            GameField.Format or
            GameField.Trophyset or
            GameField.Source or
            GameField.Verified or
            GameField.CoachMultiplayer;
    }

    private void FixAppliesTo()
    {
        ((ICustomItemsControl<Platform, Platforms>)
            Builder[GameField.AppliesTo].Control).FixedItems = 
            CurrentPlatformAsList;
    }

    private Platforms? CurrentPlatformAsList;

    private void GenerateCurrentPlatformAsList()
    {
        CurrentPlatformAsList ??= new Platforms();
        CurrentPlatformAsList.Clear();
        CurrentPlatformAsList.Add(
            new Platform()
            {
                Type = Builder.Value<PlatformType>(GameField.Platform)
            }
        );
    }

    private void FixReleasePlatform() =>
        ((ICustomItemsControl<Platform, Platforms>)
            Builder[GameField.ReleasePlatforms].Control).FixedItems =
            CurrentPlatformAsList;

    private void RemoveUnavailableDevices()
    {
        ListDAO<Device> devices = new();
        DeviceTypeHelper deviceTypeHelper = TypeHelper.Helper<DeviceTypeHelper>();

        foreach(Device device in (ListDAO<Device>)Builder[GameField.Devices].Value!)
            if (deviceTypeHelper.Available(
                    Builder.Value<PlatformType>(GameField.Platform)
                ).Contains(device.Type))
                devices.Add(device);

        Builder[GameField.Devices].Value = devices;
    }

    private void SetTrophysetAvailability()
    {
        if (!AvailableTrophyset)
            Builder.Control<TrophysetPanel>(GameField.Trophyset).Type = TrophysetType.NoSet;
    }

    private void RemoveUnavailableAppliesTo(GameField field, bool byUser)
    {
        Platforms appliesTo = (Platforms)Builder[GameField.AppliesTo].Value!;
        
        if (field is GameField.Platform 
            && byUser)
            appliesTo.Remove(p => p.Type.Equals(CurrentItem.PlatformType));

        appliesTo.Remove(
            p => !p.Type.Equals(CurrentItem.PlatformType)
                && !Builder[GameField.ReleasePlatforms].DAOValue<Platforms>()!.Contains(
                    p2 => p2.Type.Equals(p.Type)
            )
        );

        Builder[GameField.AppliesTo].Value = appliesTo;
    }

    private void AddCurrentPlatformToAppliesTo()
    {
        Platforms appliesTo = (Platforms)Builder[GameField.AppliesTo].Value!;
        appliesTo.Add(Builder[GameField.Platform].EnumValue<PlatformType>());
        Builder[GameField.AppliesTo].Value = appliesTo;
    }

    private bool SupportCoathMultiplayer =>
        TypeHelper.Helper<PlatformTypeHelper>().SupportCoachMultiplayer(
            Builder[GameField.Platform].EnumValue<PlatformType>());

    private void SetCoachMultiplayerVisible()
    {
        Builder[GameField.CoachMultiplayer].Visible = SupportCoathMultiplayer;

        if (!SupportCoathMultiplayer)
            Builder[GameField.CoachMultiplayer].Value = false;
    }

    private readonly SourceHelper sourceHelper = TypeHelper.Helper<SourceHelper>();
    private readonly GameFieldGroupHelper groupHelper = TypeHelper.Helper<GameFieldGroupHelper>();
    private readonly GameFormatHelper formatHelper = TypeHelper.Helper<GameFormatHelper>();

    protected override bool SetGroupsAvailability(bool afterSyncValues = false)
    {
        Editor.Groups[GameFieldGroup.Trophyset].Visible = AvailableTrophyset;
        Editor.Groups[GameFieldGroup.Installations].Visible =
            sourceHelper.InstallationsSupport(Builder.Value<Source>(GameField.Source));

        SetAsEmulatorVisible();

        bool verified = IsVerified;
        List<GameField> unverifiedFields = GameFieldHelper.UnverifiedFields();

        if (!verified)
            foreach (GameFieldGroup group in groupHelper.VerifiedGroups)
                foreach (GameField field in groupHelper.Fields(group))
                    Builder[field].ReadOnly = false;

        SetEditionAndSeriesesVisible();
        SetGenreControlsVisible();
        SetCodeVisible();

        if (verified)
            foreach (GameFieldGroup group in groupHelper.VerifiedGroups)
                foreach (GameField field in groupHelper.Fields(group))
                    Builder[field].ReadOnly = !unverifiedFields.Contains(field);

        if (!verified)
            Builder[GameField.Owner].Enabled = AccountAvailable();

        if (afterSyncValues && ownerReadOnlyChanged)
            Builder[GameField.Owner].SetDefaultValue();

        Editor.Groups[GameFieldGroup.DLC].Visible = Builder.Value<bool>(GameField.Licensed) 
            && (!verified || !Builder[GameField.Dlcs].IsEmpty);
        return true;
    }

    private void SetCodeVisible() => 
        Builder.SetVisible(GameField.Code,
            !IsEmulator
            && (!IsVerified || !Builder[GameField.Code].IsEmpty)
        );

    private void SetGenreControlsVisible()
    {
        bool verified = IsVerified;
        bool singlePlayerVisible = !verified
            || Builder[GameField.SinglePlayer].BoolValue;
        bool coachMultiplayerVisible = !verified
            || Builder[GameField.CoachMultiplayer].BoolValue;
        bool maxPlayersVisible = coachMultiplayerVisible &&
            Builder[GameField.CoachMultiplayer].BoolValue;
        bool onlineMultiplayerVisible = !verified
            || Builder[GameField.OnlineMultiplayer].BoolValue;

        int lastBottom = singlePlayerVisible
            ? Builder[GameField.SinglePlayer].Bottom
            : Builder[GameField.Genre].Bottom;

        Builder.SetVisible(GameField.SinglePlayer, singlePlayerVisible);
        Builder.SetVisible(GameField.CoachMultiplayer, coachMultiplayerVisible);
        Builder[GameField.CoachMultiplayer].Top = OxSH.Add(lastBottom, 4);
        Builder.SetVisible(GameField.MaximumPlayers, maxPlayersVisible);

        if (coachMultiplayerVisible)
            lastBottom = Builder[GameField.CoachMultiplayer].Bottom;

        Builder[GameField.MaximumPlayers].Top = OxSH.Add(lastBottom, 4);

        if (maxPlayersVisible)
            lastBottom = Builder[GameField.MaximumPlayers].Bottom;

        Builder.SetVisible(GameField.OnlineMultiplayer, onlineMultiplayerVisible);
        Builder[GameField.OnlineMultiplayer].Top = OxSH.Add(lastBottom, 4);

        if (onlineMultiplayerVisible)
            lastBottom = Builder[GameField.OnlineMultiplayer].Bottom;

        Builder.SetVisible(GameField.Devices,
            TypeHelper.Helper<DeviceTypeHelper>().
                Available(Builder.Value<PlatformType>(GameField.Platform)).Count > 0
                && !verified || !Builder[GameField.Devices].IsEmpty);
        Builder[GameField.Devices].Top = OxSH.Add(lastBottom, 4);
    }

    private bool IsVerified =>
        Builder.Value<bool>(GameField.Verified);

    private bool IsEmulator =>
        GameFormat.Emulator.Equals(Builder.Value(GameField.Format));

    private void SetEditionAndSeriesesVisible()
    {
        bool verified = IsVerified;
        bool editionVisible =
            !IsEmulator
            && (!verified || !Builder[GameField.Edition].IsEmpty);
        Builder.SetVisible(GameField.Edition, editionVisible);
        Builder.SetVisible(GameField.Serieses,
            !IsEmulator
            && (!verified || !Builder[GameField.Serieses].IsEmpty)
        );
        Builder[GameField.Serieses].Top = 
            OxSH.Add(
                editionVisible
                    ? Builder[GameField.Edition].Bottom
                    : Builder[GameField.Edition].Top,
                Generator!.Offset(GameField.Serieses)
            );
    }

    private void SetAsEmulatorVisible()
    {
        bool isEmulator = IsEmulator;
        Editor.Groups[GameFieldGroup.Emulator].Visible = isEmulator;
        Editor.Groups[GameFieldGroup.Genre].Visible = !isEmulator;
        Editor.Groups[GameFieldGroup.RelatedGames].Visible = !isEmulator;
        Editor.Groups[GameFieldGroup.ReleaseBase].Visible = !isEmulator;

        Builder.SetVisible(GameField.Region, !isEmulator);
        Builder.SetVisible(GameField.Language, !isEmulator);
        Builder.SetVisible(GameField.Code, !isEmulator);
        Builder.SetVisible(GameField.EmulatorType, isEmulator);
    }

    private bool AvailableTrophyset =>
        new Game()
        {
            PlatformType = Builder[GameField.Platform].EnumValue<PlatformType>(),
            Format = Builder[GameField.Format].EnumValue<GameFormat>(),

        }.TrophysetAvailable;

    private void SyncFormatWithPlatform() =>
        Builder[GameField.Format].Value = TypeHelper.TypeObject<GameFormat>(
            formatHelper.DefaultFormat(
                Builder.Value<PlatformType>(GameField.Platform)
            )
        );
    private bool ownerReadOnlyChanged = false;

    private void SyncOwnerWithControls(bool byUser)
    {
        bool accountAvailable = AccountAvailable();
        ownerReadOnlyChanged = !accountAvailable.Equals(Builder[GameField.Owner].Enabled);

        if (byUser && !ownerReadOnlyChanged)
            return;

        if (Builder[GameField.Owner].Context.AdditionalContext is AccountAccessorParameters parameters)
        {
            parameters.UseNullable = !accountAvailable;
            parameters.OnlyNullable = !accountAvailable;
        }
        
        Builder[GameField.Owner].RenewControl(true);

        if (!accountAvailable )
            Builder[GameField.Owner].Value = null;
        else
            if (!byUser)
                Builder[GameField.Owner].Value = Item!.Owner;
    }

    private bool AccountAvailable() => 
        new Game()
        {
            Licensed = Builder[GameField.Licensed].BoolValue,
            PlatformType = Builder[GameField.Platform].EnumValue<PlatformType>(),
            SourceType = Builder[GameField.Source].EnumValue<Source>()

        }.AccountAvailable;

    protected override List<List<GameField>> LabelGroups => new()
    {
        new() {
            GameField.Owner,
            GameField.Source,
            GameField.Platform,
            GameField.Format
        },
        new() {
            GameField.Region,
            GameField.Name,
            GameField.Edition,
            GameField.Serieses,
            GameField.EmulatorType
        },
        new() {
            GameField.Year,
            GameField.Pegi,
            GameField.CriticScore,
            GameField.Developer,
            GameField.Publisher
        },
        new() {
            GameField.Genre,
            GameField.ScreenView,
            GameField.Devices
        }
    };

    protected override void BeforeGrabControls()
    {
        base.BeforeGrabControls();

        if (Item is null)
            return;

        if (!AvailableTrophyset)
            Builder[GameField.Trophyset].Clear();

        List<GameFieldGroup> invisibleGroups = new();

        if (!Builder.Value<bool>(GameField.Licensed))
            invisibleGroups.Add(GameFieldGroup.DLC);

        if (Builder.Value<GameFormat>(GameField.Format) is GameFormat.Emulator)
        {
            invisibleGroups.Add(GameFieldGroup.Genre);
            invisibleGroups.Add(GameFieldGroup.RelatedGames);
            invisibleGroups.Add(GameFieldGroup.ReleaseBase);
            invisibleGroups.Add(GameFieldGroup.ReleasePlatforms);
            Builder[GameField.Edition].Clear();
            Builder[GameField.Serieses].Clear();
        }
        else
        {
            invisibleGroups.Add(GameFieldGroup.Emulator);
            Builder[GameField.EmulatorType].Clear();
        }

        if (!sourceHelper.InstallationsSupport(Builder.Value<Source>(GameField.Source)))
            invisibleGroups.Add(GameFieldGroup.Installations);
        else SyncInstallationsWithPlatform();

        foreach (GameFieldGroup group in invisibleGroups)
            foreach (GameField field in groupHelper.Fields(group))
                Builder[field].Clear();

        Item[GameField.RelatedGames] = Builder.Value<RelatedGames>(GameField.RelatedGames);
    }

    private void SyncInstallationsWithPlatform()
    {
        ListDAO<Installation>? installations = Builder.Value<ListDAO<Installation>>(GameField.Installations);

        if (installations is null)
            return;

        RootListDAO<ConsoleField, PSConsole> availableConsoles = 
            DataManager.FullItemsList<ConsoleField, PSConsole>().FilteredList(Game.AvailableConsoleFilter(Builder));

        installations.RemoveAll(i => !availableConsoles.Contains(c => c.Id.Equals(i.ConsoleId)));
        Builder[GameField.Installations].Value = installations;
    }

    protected override void AfterGrabControls() => 
        Item?.ReleasePlatforms.Add(Item.PlatformType);

    protected override EditorLayoutsGenerator<GameField, Game, GameFieldGroup> CreateLayoutsGenerator(
        FieldGroupPanels<GameField, GameFieldGroup> frames, ControlLayouter<GameField, Game> layouter
        ) =>
        new GameEditorLayoutsGenerator(frames, layouter);
}