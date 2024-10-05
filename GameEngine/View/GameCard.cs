using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Decorator;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.View
{
    public class GameCard : ItemCard<GameField, Game, GameFieldGroup>
    {
        protected override int CardWidth => 440;
        protected override int CardHeight => 240;

        public GameCard(ItemViewMode viewMode) : base(viewMode)
        {
            trophiesPanel = new OxFrame
            {
                Parent = this
            };
            trophiesPanel.Paddings.SetSize(OxSize.Large);
            PrepareColors();
        }

        protected override void PrepareColors()
        {
            base.PrepareColors();
            SetPaneBaseColor(trophiesPanel, BaseColor);
        }

        private void FillTrophiesLayouts()
        {
            ClearLayoutTemplate();
            Layouter.Template.Left = 67;
            Layouter.Template.Top = 0;
            Layouter.Template.Parent = trophiesPanel;
            Layouter.Template.AutoSize = true;

            AvailableTrophiesExist = Item != null && new GameCalculations(Item).AvailableTrophiesExist();
            trophiesLayouts.Clear();

            /*
            if (AvailableTrophiesExist && Item != null)
            {
                Dictionary<GameField, GameField> trophiesFieldsMap = new()
                {
                    [GameField.AvailablePlatinum] = GameField.FullPlatinum,
                    [GameField.AvailableGold] = GameField.FullGold,
                    [GameField.AvailableSilver] = GameField.FullSilver,
                    [GameField.AvailableBronze] = GameField.FullBronze,
                    [GameField.AvailableFromDLC] = GameField.FullFromDLC
                };

                foreach (var item in trophiesFieldsMap)
                    if (DAO.IntValue(Item[item.Key]) > 0)
                        trophiesLayouts.Add(
                            Layouter.AddFromTemplate(item.Value, -8)
                        );
            }
            
            else
            */
        }

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
            WrapReleasePlatforms();
        }

        private void WrapReleasePlatforms()
        {
            OxLabel? platformsControl =
                (OxLabel?)Layouter.PlacedControl(GameField.ReleasePlatforms)?.Control;

            if (platformsControl != null)
            {
                platformsControl.MaximumSize = new Size(240, 120);
                platformsControl.TextAlign = ContentAlignment.TopLeft;
            }
        }

        private void CalcLinksPanelLayout()
        {
            OxPane? linksControl = (OxPane?)Layouter.PlacedControl(GameField.Links)?.Control;

            if (linksControl != null)
            {
                linksControl.Left = SavedWidth - linksControl.Width;
                linksControl.Top = SavedHeight - linksControl.Height;
            }
        }

        protected override void AlignControls()
        {
            Layouter.AlignLabels(trophiesLayouts);
            Layouter.AlignLabels(baseLayouts);
            Layouter.AlignLabels(releaseLayouts);

            foreach (GameField field in baseLayouts.Fields)
                Layouter.PlacedControl(field)!.Control.Left -= 8;

            foreach (GameField field in releaseLayouts.Fields)
                Layouter.PlacedControl(field)!.Control.Left -= 8;

            foreach (GameField field in trophiesLayouts.Fields)
                Layouter.PlacedControl(field)!.Control.Left -= 4;
        }

        private void CalcTrophiesPanelSize()
        {
            int maximumTrophiesLabelRight = 0;

            foreach (GameField field in trophiesLayouts.Fields)
                maximumTrophiesLabelRight = Math.Max(
                    maximumTrophiesLabelRight,
                    Layouter.PlacedControl(field)!.Control.Right);

            ControlLayout<GameField>? lastTrophyLayout = trophiesLayouts.Last;

            if (lastTrophyLayout != null)
                trophiesPanel.SetContentSize(
                    maximumTrophiesLabelRight + 
                        trophiesPanel.Paddings.Left + trophiesPanel.Paddings.Right,
                    Layouter.PlacedControl(lastTrophyLayout.Field)!.Control.Bottom + 
                        trophiesPanel.Paddings.Top + trophiesPanel.Paddings.Bottom
                );

            trophiesPanel.Left = SavedWidth - trophiesPanel.Width;
            trophiesPanel.Top = 0;
        }

        protected override void PrepareLayouts()
        {
            FillImageLayout();
            FillBaseLayouts();
            FillReleaseLayouts();
            FillTrophiesLayouts();
            FillLinksLayout();
        }

        private void FillReleaseLayouts()
        {
            ClearLayoutTemplate();
            ControlLayouts<GameField> result = new();
            Layouter.Template.Left = 82;
            releaseLayouts.Clear();
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.FullGenre))
                .OffsetVertical(Layouter[GameField.CompleteTime], 2);
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.CriticScore, -8));
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Developer, -2));
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Publisher, -8));
            ControlLayout<GameField> yearLayout = result.Add(Layouter.AddFromTemplate(GameField.Year, -8));
            ControlLayout<GameField> pegiLayout = Layouter.AddFromTemplate(GameField.Pegi);
            pegiLayout.Top = yearLayout.Top;
            pegiLayout.Left += 88;
            result.Add(Layouter.AddFromTemplate(GameField.ReleasePlatforms, -8));
        }

        private void FillBaseLayouts()
        {
            ClearLayoutTemplate();
            baseLayouts.Clear();
            Layouter.Template.Left = Layouter[GameField.Image]!.Right + 72;
            baseLayouts.Add(Layouter.AddFromTemplate(GameField.Platform));
            baseLayouts.Add(Layouter.AddFromTemplate(GameField.Source, -12));
            //TODO: result.Add(layouts.AddFromTemplate(GameField.Installation, -12));
            baseLayouts.Add(Layouter.AddFromTemplate(GameField.Difficult, -6));
            baseLayouts.Add(Layouter.AddFromTemplate(GameField.CompleteTime, -12));
            Layouter[GameField.CompleteTime]!.CaptionVariant = ControlCaptionVariant.None;
            Layouter[GameField.Difficult]!.Visible = Item!.Licensed;
            Layouter[GameField.CompleteTime]!.Visible = Item!.Licensed;
            Layouter[GameField.Source]!.CaptionVariant = ControlCaptionVariant.None;
            Layouter[GameField.Source]!.FontStyle = FontStyle.Regular;
            //TODO: layouts[GameField.Installation].CaptionVariant = ControlCaptionVariant.None;
            //TODO: layouts[GameField.Installation].FontStyle = FontStyle.Regular;
        }

        private void FillImageLayout()
        {
            ControlLayout<GameField> imageLayout = Layouter.AddFromTemplate(GameField.Image);
            imageLayout.CaptionVariant = ControlCaptionVariant.None;
            imageLayout.Width = 140;
            imageLayout.Height = 80;

            if (Item != null && Item.Image != null && Item.Image.GetPixel(0, 0).A == 0)
                imageLayout.BackColor = Colors.Darker();
        }

        protected override void ClearLayouts()
        {
            baseLayouts.Clear();
            trophiesLayouts.Clear();
            releaseLayouts.Clear();
        }


        protected override string GetTitle() =>
            Item != null
                ? $"{Item.Name} ({TypeHelper.ShortName(Item.PlatformType)})"
                : "Game";

        private readonly OxFrame trophiesPanel;
        private readonly ControlLayouts<GameField> trophiesLayouts = new();
        private readonly ControlLayouts<GameField> baseLayouts = new();
        private readonly ControlLayouts<GameField> releaseLayouts = new();
        private bool AvailableTrophiesExist;
    }
}