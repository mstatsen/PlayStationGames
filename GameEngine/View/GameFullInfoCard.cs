using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using OxLibrary;

namespace PlayStationGames.GameEngine.View
{
    public class GameFullInfoCard : ItemInfo<GameField, Game, GameFieldGroup>
    {
        private ControlLayouts<GameField> FillTrophiesLayouts()
        {
            ControlLayouts<GameField> result = new();
            /*
            ClearLayoutTemplate();
            Layouter.Template.Left = 75;
            Layouter.Template.Parent = TrophiesPanel;

            bool AvailableTrophiesExist = Item != null 
                && new GameCalculations(Item).AvailableTrophiesExist();

            ControlLayouts<GameField> result = new()
            {
                Layouter.AddFromTemplate(GameField.AvailableBronze)
            };

            if (AvailableTrophiesExist)
            {
                List<GameField> trophiesFieldsMap = new()
                {
                    GameField.AvailablePlatinum,
                    GameField.AvailableGold,
                    GameField.AvailableSilver,
                    GameField.AvailableBronze
                };

                bool firstLayout = true;

                foreach (var item in trophiesFieldsMap)
                    if (DAO.IntValue(Item?[item]) > 0)
                    { 
                        result.Add(
                            Layouter.AddFromTemplate(item, firstLayout ? 2 : -10)
                        );
                        firstLayout = false;
                    }
            }
            */

            return result;
        }

        private ControlLayouts<GameField> FillDifficultLayouts()
        {
            ClearLayoutTemplate();
            Layouter.Template.Left = 300;
            Layouter.Template.Parent = TrophiesPanel;
            Layouter.Template.MaximumLabelWidth = 200;

            return new ControlLayouts<GameField>()
                {
                Layouter.AddFromTemplate(GameField.Difficult),
                Layouter.AddFromTemplate(GameField.CompleteTime, -10)
            };
        }

        private ControlLayouts<GameField> FillReleaseLayouts()
        {
            ClearLayoutTemplate();
            Layouter.Template.Left = 95;
            Layouter.Template.Parent = ReleasePanel;
            Layouter.Template.MaximumLabelWidth = 200;

            ControlLayout<GameField> developerLayout = Layouter.AddFromTemplate(GameField.Developer);
            ControlLayout<GameField> publisherLayout = Layouter.AddFromTemplate(GameField.Publisher, -10);
            ControlLayout<GameField> yearLayout = Layouter.AddFromTemplate(GameField.Year, -10);
            ControlLayout<GameField> platformsLayout = Layouter.AddFromTemplate(GameField.ReleasePlatforms, -10);

            ControlLayout<GameField> pegiLayout = Layouter.AddFromTemplate(GameField.Pegi);
            pegiLayout.Left = yearLayout.Right + 40;
            pegiLayout.Top = yearLayout.Top;

            return new ControlLayouts<GameField>()
                {
                    developerLayout,
                    publisherLayout,
                    yearLayout,
                    platformsLayout
                };
        }

        private void FillLinksLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Dock = DockStyle.Top;
            Layouter.Template.BackColor = BaseColor;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            Layouter.Template.Parent = LinksPanel;
            Layouter.AddFromTemplate(GameField.Links);
        }

        protected override void WrapTextControls()
        {
            WrapControl(GameField.ReleasePlatforms);
        }

        protected override void PrepareLayouts()
        {
            FillImageLayout();
            LayoutsLists.Add(FillBaseLayouts1());
            LayoutsLists.Add(FillBaseLayouts2());
            LayoutsLists.Add(FillStockLayouts());
            LayoutsLists.Add(FillTrophiesLayouts());
            LayoutsLists.Add(FillDifficultLayouts());
            LayoutsLists.Add(FillReleaseLayouts());
            FillLinksLayout();
        }

        private ControlLayouts<GameField> FillStockLayouts()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = StockPanel;
            Layouter.Template.Left = 92;

            return new ControlLayouts<GameField>()
                {
                    Layouter.AddFromTemplate(GameField.Edition),
                    Layouter.AddFromTemplate(GameField.Dlcs, -10),
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
                    Layouter.AddFromTemplate(GameField.Source),
                    Layouter.AddFromTemplate(GameField.Platform, -6),
                    Layouter.AddFromTemplate(GameField.Format, -6),
                };
        }

        private ControlLayouts<GameField> FillBaseLayouts2()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = BasePanel;
            Layouter.Template.Left = 92;
            ControlLayout<GameField>? imageLayout = Layouter[GameField.Image];

            Layouter.Template.Top = (imageLayout != null ? imageLayout.Bottom : 0) + 8;

            return new ControlLayouts<GameField>() {
                Layouter.AddFromTemplate(GameField.Serieses),
                Layouter.AddFromTemplate(GameField.CriticScore, -10),
                Layouter.AddFromTemplate(GameField.FullGenre, -2),
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
            PreparePanel(TrophiesPanel, "Trophies");
            PreparePanel(StockPanel, "Stock");
            PreparePanel(BasePanel, string.Empty);
        }

        private readonly OxPanel BasePanel = new(new Size(200, 216));
        private readonly OxPanel StockPanel = new(new Size(200, 160));
        private readonly OxPanel TrophiesPanel = new(new Size(200, 156));
        private readonly OxPanel ReleasePanel = new(new Size(200, 140));
        private readonly OxPanel LinksPanel = new(new Size(200, 48));

        public GameFullInfoCard() : base() { }
    }
}