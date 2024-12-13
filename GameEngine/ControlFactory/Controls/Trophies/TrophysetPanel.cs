using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Geometry;
using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls.Trophies;

public class TrophysetPanel : OxPanel
{
    private readonly ControlBuilder<GameField, Game> builder = DataManager.Builder<GameField, Game>(ControlScope.Editor);
    private readonly IControlAccessor typeControl;
    private readonly IControlAccessor? appliesToControl;
    private readonly IControlAccessor difficultControl;
    private readonly IControlAccessor completeTimeControl;
    private readonly List<TrophiesPanel> trophiesPanels = new();
    private TrophiesPanel AvailableTrophiesPanel = default!;
    private readonly OxButton addButton = new("Add account", OxIcons.Plus)
    { 
        ToolTipText = "Add account for earn trophies"
    };
    private readonly OxLabel trophysetTypeLabel = new()
    { 
        Left = 8,
        Text = "Type"
    };
    private readonly OxLabel appliesToLabel = new()
    {
        Left = 8,
        Text = "Applies to"
    };
    private readonly OxLabel difficultLabel = new()
    {
        Left = 8,
        Text = "Difficult"
    };
    private readonly OxLabel completeTimeLabel = new()
    { 
        Left = 8, 
        Text = "Time" 
    };

    private void PrepareAccessor(IControlAccessor accessor, OxLabel label, short top, short width, EventHandler OnChangeHandler)
    {
        accessor.Parent = this;
        accessor.Left = 88;
        accessor.Top = OxSH.Add(top, 4);
        accessor.Width = width;
        accessor.ValueChangeHandler += OnChangeHandler;
        label.Parent = this;
        OxControlHelper.AlignByBaseLine(accessor.Control, label);
    }

    private readonly bool IsDLCPanel;

    public TrophysetPanel(bool forDLC)
    {
        IsDLCPanel = forDLC;
        typeControl = IsDLCPanel
            ? builder.Accessor("DLC:TrophysetType", FieldType.Enum) 
            : builder[GameField.TrophysetType];

        if (!IsDLCPanel)
            appliesToControl = IsDLCPanel
                ? builder.Accessor("DLC:TrophysetAppliesTo", FieldType.Enum) 
                : builder[GameField.AppliesTo];

        difficultControl = IsDLCPanel
            ? builder.Accessor("DLC:Difficult", FieldType.Enum) 
            : builder[GameField.Difficult];
        completeTimeControl = IsDLCPanel
            ? builder.Accessor("DLC:CompleteTime", FieldType.Enum) 
            : builder[GameField.CompleteTime];
        PrepareAccessor(typeControl, trophysetTypeLabel, 4, OxSH.Short(IsDLCPanel ? 122 : 178), TypeChangeHandler);

        if (appliesToControl is not null)
            PrepareAccessor(appliesToControl, appliesToLabel, typeControl.Bottom, 178, AppliesToChandeHandler);

        PrepareAccessor(
            difficultControl, 
            difficultLabel, 
            appliesToControl is null 
                ? typeControl.Bottom 
                : appliesToControl.Bottom, 
            64, 
            DifficultChangeHandler
        );
        PrepareAccessor(completeTimeControl, completeTimeLabel, difficultControl.Bottom, 88, CompleteTimeChangeHandler);
        CreateTrophiesPanels();
        AccountSelector = new(this);
        SetMinimumSize();
    }

    private void AppliesToChandeHandler(object? sender, EventArgs e) =>
        ValueChanged?.Invoke(this, e);

    public TrophysetAccountEditor AccountSelector;

    private void CompleteTimeChangeHandler(object? sender, EventArgs e) =>
        ValueChanged?.Invoke(this, e);

    private void DifficultChangeHandler(object? sender, EventArgs e) =>
        ValueChanged?.Invoke(this, e);

    private void TypeChangeHandler(object? sender, EventArgs e)
    {
        if (Type is TrophysetType.NoSet)
            ClearValues();

        bool isTrophysetExists = Type is not TrophysetType.NoSet;

        if (appliesToControl is not null)
            appliesToControl.Visible = isTrophysetExists;

        AddCurrentPlatformToAppliesTo();
        appliesToLabel.Visible = isTrophysetExists;
        difficultControl.Visible = isTrophysetExists;
        completeTimeControl.Visible = isTrophysetExists;
        difficultLabel.Visible = isTrophysetExists;
        completeTimeLabel.Visible = isTrophysetExists;
        addButton.Visible = isTrophysetExists;
        RecalcTrophiesPanels();
        ValueChanged?.Invoke(this, e);
    }

    private void AddCurrentPlatformToAppliesTo()
    {
        if (IsDLCPanel 
            || appliesToControl is null
            || Type is TrophysetType.NoSet)
            return;

        appliesToControl.Value = new Platforms
        {
            new Platform(
                builder.Value<PlatformType>(GameField.Platform)
            )
        };
    }

    private void SetMinimumSize()
    {
        MinimumSize = new(
            OxSH.Add(trophiesPanels.Last().Right, 8),
            Type is TrophysetType.NoSet
                ? typeControl.Bottom
                : VisiblePanels.Count > 0
                    ? VisiblePanels.Last().Bottom
                    : addButton.Bottom
        );
        MaximumSize = MinimumSize;
    }


    public override void PrepareColors()
    {
        base.PrepareColors();

        if (typeControl is not null)
        {
            ControlPainter.ColorizeControl(typeControl, Colors.Darker());

            if (appliesToControl is not null)
                ControlPainter.ColorizeControl(appliesToControl, Colors.Darker());

            ControlPainter.ColorizeControl(difficultControl, Colors.Darker());
            ControlPainter.ColorizeControl(completeTimeControl, Colors.Darker());
            ControlPainter.ColorizeControl(addButton, Colors.Darker());

            foreach (TrophiesPanel panel in trophiesPanels)
                panel.BaseColor = BaseColor;

            AccountSelector.BaseColor = Colors.Darker();
        }
    }
    private void CreateTrophiesPanels()
    {
        AvailableTrophiesPanel = CreateTrophiesPanel(null, IsDLCPanel);
        addButton.Parent = this;
        addButton.Left = OxSH.Sub(AvailableTrophiesPanel.Right, addButton.Width);
        addButton.Top = OxSH.Add(AvailableTrophiesPanel.Bottom, 6);
        addButton.Click += AddButtonClickHandler;

        foreach (Account account in DataManager.FullItemsList<AccountField, Account>())
            CreateTrophiesPanel(account, IsDLCPanel);
    }

    private void AddButtonClickHandler(object? sender, EventArgs e)
    {
        if (!AccountSelector.ShowDialogIsOK(this)
            || AccountSelector.SelectedAccountId.Equals(Guid.Empty))
            return;

        TrophiesPanel newPanel = AvailableTrophiesPanel.DependedPanels.Find(t =>
            t.Account!.Id.Equals(AccountSelector.SelectedAccountId))!;
        newPanel.Value = new();
        VisiblePanels.Add(newPanel);
        RecalcTrophiesPanels();
        ValueChanged?.Invoke(this, EventArgs.Empty);

    }

    private void RecalcTrophiesPanels()
    {
        AvailableTrophiesPanel.Visible = Type is not TrophysetType.NoSet;

        foreach (TrophiesPanel trophiesPanel in AvailableTrophiesPanel.DependedPanels)
            trophiesPanel.Visible = 
                Type is not TrophysetType.NoSet
                && VisiblePanels.Contains(trophiesPanel);

        short lastTop = OxSH.Sub(addButton.Bottom, 2);

        foreach (TrophiesPanel trophiesPanel in AvailableTrophiesPanel.DependedPanels)
        {
            if (!VisiblePanels.Contains(trophiesPanel))
                continue;

            trophiesPanel.Top = OxSH.Add(lastTop, 8);
            lastTop = OxSH.Add(trophiesPanel.Bottom, 4);
        }

        SetMinimumSize();
    }

    private TrophiesPanel CreateTrophiesPanel(Account? account, bool forDLC)
    {
        TrophiesPanel result = new(account, forDLC)
        {
            Parent = this,
            Left = 8,
            Top = OxSH.Add(
                trophiesPanels.Count is 1
                    ? OxSH.Sub(addButton.Bottom, 2)
                    : trophiesPanels.Count is 0
                        ? OxSH.Add(completeTimeControl.Bottom, 16)
                        : OxSH.Add(trophiesPanels.Last().Bottom, 4),
                8
            )
        };
        result.OnRemove += OnRemovePanelHandler;
        trophiesPanels.Add(result);

        if (AvailableTrophiesPanel is not null 
            && account is not null)
            AvailableTrophiesPanel.DependedPanels.Add(result);

        return result;
    }

    private void OnRemovePanelHandler(object? sender, EventArgs e)
    {
        if (sender is null)
            return;

        VisiblePanels.Remove((TrophiesPanel)sender);
        RecalcTrophiesPanels();
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }

    public Trophyset? Value
    {
        get
        {
            Trophyset result = new()
            {
                Type = typeControl.EnumValue<TrophysetType>(),
                Difficult = difficultControl.EnumValue<Difficult>(),
                CompleteTime = completeTimeControl.EnumValue<CompleteTime>(),
            };

            if (!IsDLCPanel
                && appliesToControl is not null)
                result.AppliesTo.CopyFrom(appliesToControl.DAOValue<ListDAO<Platform>>());

            result.Available.CopyFrom(AvailableTrophiesPanel.Value);

            foreach (TrophiesPanel trophiesPanel in VisiblePanels)
            {
                EarnedTrophies earnedTrophies = new()
                {
                    AccountId = trophiesPanel.Account!.Id
                };
                earnedTrophies.Trophies.CopyFrom(trophiesPanel.Value);
                result.EarnedTrophies.Add(earnedTrophies);
            }
            
            return result;
        }
        set
        {
            if (value is null)
            {
                ClearValues();
                return;
            }

            typeControl.Value = value.Type;

            if (appliesToControl is not null)
                appliesToControl.Value = value.AppliesTo;

            difficultControl.Value = value.Difficult;
            completeTimeControl.Value = value.CompleteTime;
            AvailableTrophiesPanel.Value = value.Available;

            VisiblePanels.Clear();

            foreach (EarnedTrophies trophies in value.EarnedTrophies)
            {
                TrophiesPanel trophiesPanel = AvailableTrophiesPanel.DependedPanels.Find(t => 
                    t.Account!.Id.Equals(trophies.AccountId)
                )!;
                VisiblePanels.Add(trophiesPanel);
                trophiesPanel.Value = trophies.Trophies;
            }

            RecalcTrophiesPanels();
        }
    }

    public readonly List<TrophiesPanel> VisiblePanels = new();

    public bool ReadOnly
    {
        get => typeControl.ReadOnly;
        set
        {
            typeControl.ReadOnly = value;

            if (appliesToControl is not null)
                appliesToControl.ReadOnly = value;

            difficultControl.ReadOnly = value;
            completeTimeControl.ReadOnly = value;
            AvailableTrophiesPanel!.ReadOnly = value;
        }
    }
    public TrophysetType Type 
    { 
        get => typeControl.EnumValue<TrophysetType>();
        set => typeControl.Value = value;
    }
    public EventHandler? ValueChanged { get; set; }

    public void ClearValues()
    {
        typeControl.Value = TrophysetType.NoSet;
        appliesToControl?.Clear();
        difficultControl.Value = Difficult.Unknown;
        completeTimeControl.Value = CompleteTime.Unknown;

        foreach (TrophiesPanel trophiesPanel in trophiesPanels)
            trophiesPanel.ClearValue();
    }
}