using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.View
{
    public class GameFullInfoCard : ItemInfo<GameField, Game, GameFieldGroup>
    {
        private ControlLayouts<GameField> FillTrophiesLayouts()
        {
            ClearLayoutTemplate();
            Layouter.Template.Left = 92;
            Layouter.Template.Parent = TrophysetPanel;
            Layouter.Template.MaximumLabelWidth = 200;

            ControlLayouts<GameField> result = new();

            if (Item != null && Item.TrophysetAvailable)
            {
                result.Add(Layouter.AddFromTemplate(GameField.TrophysetType, 2));
                result.Add(Layouter.AddFromTemplate(GameField.AppliesTo, 2));
                result.Add(Layouter.AddFromTemplate(GameField.Difficult, 2));
                result.Add(Layouter.AddFromTemplate(GameField.CompleteTime, 2));

                if (Item.Trophyset.Type != TrophysetType.NoSet)
                {
                    bool firstLayout = true;

                    foreach (var field in FieldHelper.TrophiesFields)
                        if (DAO.IntValue(Item?[field]) > 0)
                        {
                            ControlLayout<GameField> trophyLayout = 
                                result.Add(
                                    Layouter.AddFromTemplate(field, firstLayout ? 18 : 2)
                                );
                            trophyLayout.FontColor = TypeHelper.FontColor(FieldHelper.TrophyTypeByField(field));
                            trophyLayout.LabelColor = trophyLayout.FontColor;
                            firstLayout = false;
                        }
                }
            }
            return result;
        }

        private readonly GameFieldHelper FieldHelper = TypeHelper.Helper<GameFieldHelper>();

        private ControlLayouts<GameField> FillReleaseLayouts()
        {
            ClearLayoutTemplate();
            Layouter.Template.Left = 95;
            Layouter.Template.Parent = ReleasePanel;
            Layouter.Template.MaximumLabelWidth = 200;

            return new ControlLayouts<GameField>()
                {
                    Layouter.AddFromTemplate(GameField.Developer),
                    Layouter.AddFromTemplate(GameField.Publisher, 2),
                    Layouter.AddFromTemplate(GameField.Year, 2),
                    Layouter.AddFromTemplate(GameField.Pegi, 2),
                    Layouter.AddFromTemplate(GameField.ReleasePlatforms, 2)
                };
        }

        private ControlLayouts<GameField> FillLinksLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Dock = DockStyle.Top;
            Layouter.Template.BackColor = BaseColor;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            Layouter.Template.Parent = LinksPanel;

            return new ControlLayouts<GameField>()
            {
                Layouter.AddFromTemplate(GameField.Links)
            };
        }

        protected override void WrapTextControls() => 
            WrapControl(GameField.ReleasePlatforms);

        protected override void PrepareLayouts()
        {
            FillImageLayout();
            LayoutsLists.Add(BasePanel, FillBaseLayouts1());
            LayoutsLists.Add(BasePanel2, FillBaseLayouts2());
            LayoutsLists.Add(StockPanel, FillStockLayouts());
            LayoutsLists.Add(TrophysetPanel, FillTrophiesLayouts());
            LayoutsLists.Add(ReleasePanel, FillReleaseLayouts());
            LayoutsLists.Add(LinksPanel, FillLinksLayout());

            if (Item != null)
            {
                Layouter[GameField.CriticScore]!.FontColor =
                    TypeHelper.Helper<CriticRangeHelper>().FontColor(Item.CriticScore);
                Layouter[GameField.Pegi]!.FontColor = TypeHelper.FontColor(Item.Pegi);
            }
        }

        private ControlLayouts<GameField> FillStockLayouts()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = StockPanel;
            Layouter.Template.Left = 92;

            return new ControlLayouts<GameField>()
                {
                    Layouter.AddFromTemplate(GameField.Edition),
                    Layouter.AddFromTemplate(GameField.Dlcs, 2),
                    Layouter.AddFromTemplate(GameField.Installations, 12),
                    Layouter.AddFromTemplate(GameField.RelatedGames, 12)
                };
        }

        private ControlLayouts<GameField> FillBaseLayouts1()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = BasePanel;
            Layouter.Template.Top = 4;
            ControlLayout<GameField>? imageLayout = Layouter[GameField.Image];
            Layouter.Template.Left = (imageLayout != null ? imageLayout.Right : 0) + 72;
            return new ControlLayouts<GameField>()
            {
                Layouter.AddFromTemplate(GameField.Licensed),
                Layouter.AddFromTemplate(GameField.Owner, 2),
                Layouter.AddFromTemplate(GameField.Source, 2),
                Layouter.AddFromTemplate(GameField.Platform, 2),
            };
        }

        private ControlLayouts<GameField> FillBaseLayouts2()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = BasePanel2;
            Layouter.Template.Left = 92;

            return new ControlLayouts<GameField>() {
                Layouter.AddFromTemplate(GameField.Region),
                Layouter.AddFromTemplate(GameField.Code, 2),
                Layouter.AddFromTemplate(GameField.Serieses, 2),
                Layouter.AddFromTemplate(GameField.CriticScore, 2),
                Layouter.AddFromTemplate(GameField.Genre, 2),
                Layouter.AddFromTemplate(GameField.Devices, 2),
                Layouter.AddFromTemplate(GameField.Tags, 2),
            };
        }

        private void FillImageLayout()
        {
            ControlLayout<GameField> imageLayout = Layouter.AddFromTemplate(GameField.Image);
            imageLayout.Parent = BasePanel;
            imageLayout.CaptionVariant = ControlCaptionVariant.None;
            imageLayout.Height = 97;

            if (Item?.Image != null && 
                Item.Image.GetPixel(0, 0).A == 0)
            {
                imageLayout.Left = 12;
                imageLayout.Width = 176;
                imageLayout.BackColor = Colors.Darker();
            }
            else
                imageLayout.Width = 200;
        }


        protected override string GetTitle() =>
            Item != null 
                ? $"{Item.Name} ({TypeHelper.ShortName(Item.PlatformType)})" 
                : "Unknown Game";

        protected override void PreparePanels()
        {
            PreparePanel(LinksPanel, "Links");
            PreparePanel(ReleasePanel, "Release");
            PreparePanel(TrophysetPanel, "Trophyset");
            PreparePanel(StockPanel, "Stock");
            PreparePanel(BasePanel2, "Info");
            PreparePanel(BasePanel, string.Empty);
        }

        private readonly OxPanel BasePanel = new();
        private readonly OxPanel BasePanel2 = new();
        private readonly OxPanel StockPanel = new();
        private readonly OxPanel TrophysetPanel = new();
        private readonly OxPanel ReleasePanel = new();
        private readonly OxPanel LinksPanel = new();

        public GameFullInfoCard() : base() { }
    }
}