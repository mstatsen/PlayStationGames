using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.View;

public class GameCard : ItemCard<GameField, Game, GameFieldGroup>
{
    protected override short CardWidth => 440;
    protected override short CardHeight => 300;

    public GameCard(ItemViewMode viewMode) : base(viewMode)
    {
        TrophysetPanel = new()
        {
            Parent = this,
            Text = "Trophyset",
            HeaderHeight = 18
        };
        TrophysetPanel.Padding.Size = 4;
        TrophiesPanel = new()
        {
            Parent = TrophysetPanel,
            Text = "Trophyset",
            Top = 0
        };
        PrepareColors();
    }

    public override void PrepareColors()
    {
        base.PrepareColors();
        TrophysetPanel.BaseColor = BaseColor;
        TrophiesPanel.BaseColor = BaseColor;
    }

    private void FillTrophysetLayouts()
    {
        trophysetLayouts.Clear();
        difficultLayouts.Clear();
        ClearLayoutTemplate();
        Layouter.Template.Left = 67;
        Layouter.Template.Top = 0;
        Layouter.Template.Parent = TrophysetPanel;
        Layouter.Template.AutoSize = true;
        ControlLayout<GameField> trophysetTypeLayout = trophysetLayouts.Add(
            Layouter.AddFromTemplate(GameField.TrophysetType)
        );
        trophysetTypeLayout.Left = 8;
        trophysetTypeLayout.CaptionVariant = ControlCaptionVariant.None;

        TrophysetPanel.HeaderVisible = Item!.Trophyset.TrophysetExists;

        if (!Item!.Trophyset.TrophysetExists)
        {
            trophysetTypeLayout.FontStyle = FontStyle.Bold | FontStyle.Italic;
            return;
        }

        ControlLayout<GameField> appliesToLayout = trophysetLayouts.Add(
            Layouter.AddFromTemplate(GameField.AppliesTo, -6)
        );
        appliesToLayout.CaptionVariant = ControlCaptionVariant.None;
        appliesToLayout.Left = 12;
        appliesToLayout.FontSize -= 2;
        appliesToLayout.FontStyle = FontStyle.Regular;

        difficultLayouts.Add(Layouter.AddFromTemplate(GameField.Difficult, 2));
        difficultLayouts.Add(Layouter.AddFromTemplate(GameField.CompleteTime, -8));
    }

    private void FillTrophiesLayouts()
    {
        ClearLayoutTemplate();
        RenewTrophiesIcons();
        Layouter.Template.Parent = TrophiesPanel;
        Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
        Layouter.Template.Left = 24;
        Layouter.Template.Top = 4;
        trophiesLayouts.Clear();
        bool firstTrophy = true;

        foreach (GameField field in FieldHelper.TrophiesFields)
        {
            TrophyType trophyType = FieldHelper.TrophyTypeByField(field);
            ControlLayout<GameField> trophyLayout;

            if (firstTrophy)
                trophyLayout = trophiesLayouts.Add(Layouter.AddFromTemplate(field));
            else
                trophyLayout = trophiesLayouts.Add(Layouter.AddFromTemplate(field), -8);
            
            trophyLayout.FontColor = TypeHelper.FontColor(trophyType);
            trophyLayout.LabelColor = trophyLayout.FontColor;
            firstTrophy = false;
        }
    }

    private void RenewTrophiesIcons()
    {
        foreach (OxPicture icon in trophiesIcons.Values)
            icon.Dispose();

        trophiesIcons.Clear();

        foreach(TrophyType trophyType in trophyHelper.All())
            trophiesIcons.Add(
                trophyType,
                new()
                {
                    Image = trophyHelper.Icon(trophyType),
                    Parent = TrophiesPanel,
                    Left = 0
                }
            );
    }

    private readonly GameFieldHelper FieldHelper = TypeHelper.Helper<GameFieldHelper>();
    private readonly TrophyTypeHelper trophyHelper = TypeHelper.Helper<TrophyTypeHelper>();

    private void FillLinksLayout()
    {
        ClearLayoutTemplate();
        Layouter.Template.Width = 120;
        Layouter.Template.Anchors = AnchorStyles.Right | AnchorStyles.Bottom;
        Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
        Layouter.Template.BackColor = BaseColor;
        Layouter.AddFromTemplate(GameField.Links);
    }

    protected override void AfterLayoutControls()
    {
        base.AfterLayoutControls();
        CalcLinksPanelLayout();
        CalcTrophiesPanelSize();
        CalcTrophysetPanelSize();
    }

    private void CalcLinksPanelLayout()
    {
        OxPanel? linksControl = (OxPanel?)Layouter.PlacedControl(GameField.Links)?.Control;

        if (linksControl is null)
            return;
        
        linksControl.Left = (short)(Width - linksControl.Width);
        linksControl.Top = (short)(Height - linksControl.Height);
    }

    protected override void AlignControls()
    {
        Layouter.AlignLabels(baseLayouts, 8);
        Layouter.AlignLabels(releaseLayouts, 8);
        Layouter.AlignLabels(difficultLayouts, 4);
        AlignPegiAndCritic();
        AlignTrophiesIcons();
    }

    private void AlignPegiAndCritic()
    {
        PlacedControl<GameField> yearControl = Layouter.PlacedControl(GameField.Year)!;
        PlacedControl<GameField> pegiControl = Layouter.PlacedControl(GameField.Pegi)!;
        PlacedControl<GameField> criticControl = Layouter.PlacedControl(GameField.CriticScore)!;

        yearControl.Control.Left = yearControl.LabelRight;

        pegiControl.LabelLeft = yearControl.Control.Right + 16;
        pegiControl.Control.Left = pegiControl.LabelRight;

        criticControl.LabelLeft = pegiControl.Control.Right + 16;
        criticControl.Control.Left = criticControl.LabelRight;
    }

    private void AlignTrophiesIcons()
    {
        foreach (KeyValuePair<TrophyType, OxPicture> icon in trophiesIcons)
            OxControlHelper.AlignByBaseLine(
                Layouter.PlacedControl(trophyHelper.Field(icon.Key))!.Control,
                icon.Value
                );
    }

    private void CalcTrophiesPanelSize()
    {
        TrophiesPanel.Visible = Item!.Trophyset.Available.Count > 0;
        TrophiesPanel.Size = new(
            50, 
            (short)(16 + trophiesLayouts.Count * 18)
        );
    }

    private void CalcTrophysetPanelSize()
    {
        PlacedControl<GameField>? trophysetTypeControl = Layouter.PlacedControl(GameField.TrophysetType);

        if (trophysetTypeControl is null)
            return;

        PlacedControl<GameField>? difficultControl = Layouter.PlacedControl(GameField.Difficult);

        if (difficultControl is null)
            return;

        trophysetTypeControl.Control.Left =
            difficultControl.LabelLeft;

        int maximumTrophysetLabelRight = 0;

        foreach (GameField field in trophysetLayouts.Fields)
            maximumTrophysetLabelRight = Math.Max(
                maximumTrophysetLabelRight,
                Layouter.PlacedControl(field)!.Control.Right);

        foreach (GameField field in difficultLayouts.Fields)
            maximumTrophysetLabelRight = Math.Max(
                maximumTrophysetLabelRight,
                Layouter.PlacedControl(field)!.Control.Right);

        TrophiesPanel.Left = (short)(maximumTrophysetLabelRight - 4);

        short lastBottom = 0;

        if (difficultLayouts.Last is not null)
            lastBottom = (short)Layouter.PlacedControl(difficultLayouts.Last.Field)!.Control.Bottom;

        if (trophysetLayouts.Last is not null)
            lastBottom = Math.Max(
                (short)Layouter.PlacedControl(trophysetLayouts.Last.Field)!.Control.Bottom,
                lastBottom
            );

        TrophysetPanel.Size = new(
            (short)((maximumTrophysetLabelRight)
                + (Item!.Trophyset.Available.Count > 0 ? TrophiesPanel.Width : 0)
                + TrophysetPanel.Padding.Right),
            lastBottom
        );

        TrophysetPanel.Left = (short)(Width - TrophysetPanel.Width);
        TrophysetPanel.Top = 32;
    }

    protected override void PrepareLayouts()
    {
        FillImageLayout();
        FillBaseLayouts();
        FillReleaseLayouts();
        FillTrophysetLayouts();
        FillTrophiesLayouts();
        FillLinksLayout();
    }

    private void FillReleaseLayouts()
    {
        ClearLayoutTemplate();
        Layouter.Template.Left = 74;
        Layouter.Template.Top = (short)(Layouter[GameField.Image]!.Bottom + 8);
        releaseLayouts.Clear();
        releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Platform));
        releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Source, -8));
        releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Region, -8));

        if (!Item!.Edition.Equals(string.Empty))
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Edition, -8));

        if (Item!.Serieses.Count is not 0)
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Serieses, 8));

        releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Genre, Item!.Serieses.Count is not 0 ? -8 : 8));

        if (Item!.Devices.Count > 0)
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Devices, -8));

        ControlLayout<GameField> yearLayout = releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Year, 8));
        ControlLayout<GameField> pegiLayout = Layouter.AddFromTemplate(GameField.Pegi);
        pegiLayout.Top = yearLayout.Top;
        pegiLayout.FontColor = TypeHelper.FontColor(Item.Pegi);
        ControlLayout<GameField> criticLayout = Layouter.AddFromTemplate(GameField.CriticScore);
        criticLayout.FontColor = TypeHelper.Helper<CriticRangeHelper>().FontColor(Item.CriticScore);
        criticLayout.Top = yearLayout.Top;
        releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Developer, -8));
        releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Publisher, -8));
    }

    private void FillBaseLayouts()
    {
        ClearLayoutTemplate();
        baseLayouts.Clear();
        ControlLayout<GameField> imageLayout = Layouter[GameField.Image]!;
        Layouter.Template.Top = imageLayout.Top;
        Layouter.Template.Left = (short)(imageLayout.Right + 86);

        if (Item!.Licensed)
        {
            ControlLayout<GameField> licensedLayout = baseLayouts.Add(
                Layouter.AddFromTemplate(GameField.Licensed)
            );
            licensedLayout.CaptionVariant = ControlCaptionVariant.None;
            licensedLayout.Left = (short)(Layouter[GameField.Image]!.Right + 12);
        }
        else
        if (!Item!.Owner.Equals(Guid.Empty))
            baseLayouts.Add(Layouter.AddFromTemplate(GameField.Owner));

        if (Item!.Installed)
        {
            ControlLayout<GameField> installedLayout = baseLayouts.Add(Layouter.AddFromTemplate(GameField.Installed, -8));
            installedLayout.CaptionVariant = ControlCaptionVariant.None;
            installedLayout.Left = (short)(Layouter[GameField.Image]!.Right + 12);
        }
    }

    private void FillImageLayout()
    {
        ClearLayoutTemplate();
        Layouter.Template.Top = 2;
        Layouter.Template.Left = 2;
        ControlLayout<GameField> imageLayout = Layouter.AddFromTemplate(GameField.Image);
        imageLayout.CaptionVariant = ControlCaptionVariant.None;
        imageLayout.Width = 140;
        imageLayout.Height = 80;

        if (Item is not null
            && Item.Image is not null
            && Item.Image.GetPixel(0, 0).A is 0)
            imageLayout.BackColor = Colors.Darker();
    }

    protected override void ClearLayouts()
    {
        baseLayouts.Clear();
        trophiesLayouts.Clear();
        difficultLayouts.Clear();
        trophysetLayouts.Clear();
        releaseLayouts.Clear();
    }

    protected override string GetTitle() =>
        Item is not null
            ? $"{Item.FullTitle()}"
            : "Game";

    private readonly Dictionary<TrophyType, OxPicture> trophiesIcons = new();
    private readonly OxFrameWithHeader TrophysetPanel;
    private readonly OxFrame TrophiesPanel;
    private readonly ControlLayouts<GameField> trophysetLayouts = new();
    private readonly ControlLayouts<GameField> difficultLayouts = new();
    private readonly ControlLayouts<GameField> trophiesLayouts = new();
    private readonly ControlLayouts<GameField> baseLayouts = new();
    private readonly ControlLayouts<GameField> releaseLayouts = new();
}