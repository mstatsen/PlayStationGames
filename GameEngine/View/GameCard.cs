using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.View
{
    public class GameCard : ItemCard<GameField, Game, GameFieldGroup>
    {
        protected override int CardWidth => 440;
        protected override int CardHeight => 358;

        public GameCard(ItemViewMode viewMode) : base(viewMode)
        {
            trophiesPanel = new OxFrameWithHeader()
            {
                Parent = this,
                Text = "Trophyset"
            };
            trophiesPanel.Header.SetContentSize(1, 18);
            trophiesPanel.Paddings.SetSize(OxSize.Large);
            PrepareColors();
        }

        protected override void PrepareColors()
        {
            base.PrepareColors();
            SetPaneBaseColor(trophiesPanel, Colors.Darker());
        }

        private void FillTrophiesLayouts()
        {
            ClearLayoutTemplate();
            Layouter.Template.Left = 67;
            Layouter.Template.Top = 0;
            Layouter.Template.Parent = trophiesPanel;
            Layouter.Template.AutoSize = true;

            trophiesLayouts.Clear();

            trophiesLayouts.Add(Layouter.AddFromTemplate(GameField.TrophysetType));
            Layouter.AddFromTemplate(GameField.AppliesTo, -6);
            trophiesLayouts.Add(Layouter.AddFromTemplate(GameField.Difficult, 2));
            trophiesLayouts.Add(Layouter.AddFromTemplate(GameField.CompleteTime, -8));

            Layouter[GameField.TrophysetType]!.Left = 8;
            Layouter[GameField.TrophysetType]!.CaptionVariant = ControlCaptionVariant.None;

            if (!Item!.TrophysetAvailable 
                || Item!.Trophyset.Type == TrophysetType.NoSet)
                return;

            Layouter[GameField.AppliesTo]!.Left = 12;
            Layouter[GameField.AppliesTo]!.FontSize = Layouter[GameField.AppliesTo]!.FontSize - 2;
            Layouter[GameField.AppliesTo]!.FontStyle = FontStyle.Regular;
            Layouter[GameField.AppliesTo]!.CaptionVariant = ControlCaptionVariant.None;

            bool firstTrophy = true;
            foreach (GameField field in TypeHelper.Helper<GameFieldHelper>().TrophiesFields)
            {
                if (DAO.IntValue(Item![field]) > 0)
                {
                    ControlLayout<GameField> trophyLayout = trophiesLayouts.Add(
                        Layouter.AddFromTemplate(field), firstTrophy ? 2 : - 8
                    );

                    if (field == GameField.AvailablePlatinum)
                        trophyLayout.CaptionVariant = ControlCaptionVariant.None;

                    firstTrophy = false;
                }
            }

            if (Item.ExistsDLCsWithTrophyset)
            {
                ControlLayout<GameField> withDLCLayout = trophiesLayouts.Add(Layouter.AddFromTemplate(GameField.ExistsDLCsWithTrophyset, -8));
                withDLCLayout.CaptionVariant = ControlCaptionVariant.None;
                withDLCLayout.Left = 8;
            }
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
            Layouter.PlacedControl(GameField.TrophysetType)!.Control.Left =
                Layouter.PlacedControl(GameField.Difficult)!.LabelLeft;

            PlacedControl<GameField>? platinumControl = 
                Layouter.PlacedControl(GameField.AvailablePlatinum);

            if (platinumControl != null)
                platinumControl.Control.Left =
                    Layouter.PlacedControl(GameField.Difficult)!.LabelLeft;

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
            Layouter.Template.Top = Layouter[GameField.Image]!.Bottom + 8;
            releaseLayouts.Clear();
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Platform));
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Source, -8));
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Region, -8));

            if (Item!.Edition != string.Empty)
                releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Edition, -8));

            if (Item!.Serieses.Count != 0)
                releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Serieses, 8));

            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Genre, Item!.Serieses.Count != 0 ? -8 : 8));

            if (Item!.Devices.Count > 0)
                releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Devices, -8));

            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Developer, 8));
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.Publisher, -8));
            ControlLayout<GameField> yearLayout = result.Add(Layouter.AddFromTemplate(GameField.Year, -8));
            ControlLayout<GameField> pegiLayout = Layouter.AddFromTemplate(GameField.Pegi);
            pegiLayout.Top = yearLayout.Top;
            pegiLayout.Left += 88;
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.CriticScore, -8));
            releaseLayouts.Add(Layouter.AddFromTemplate(GameField.ReleasePlatforms, -8));
        }

        private void FillBaseLayouts()
        {
            ClearLayoutTemplate();
            baseLayouts.Clear();
            Layouter.Template.Left = Layouter[GameField.Image]!.Right + 86;

            if (Item!.Licensed )
            {
                ControlLayout<GameField> licensedLayout = baseLayouts.Add(Layouter.AddFromTemplate(GameField.Licensed));
                licensedLayout.CaptionVariant = ControlCaptionVariant.None;
                licensedLayout.Left = Layouter[GameField.Image]!.Right + 12;
            }
            else
            if (Item!.Owner != Guid.Empty)
                baseLayouts.Add(Layouter.AddFromTemplate(GameField.Owner));

            if (Item!.Installed)
            {
                ControlLayout<GameField> installedLayout = baseLayouts.Add(Layouter.AddFromTemplate(GameField.Installed, -8));
                installedLayout.CaptionVariant = ControlCaptionVariant.None;
                installedLayout.Left = Layouter[GameField.Image]!.Right + 12;
            }
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
                ? $"{Item.FullTitle()})"
                : "Game";

        private readonly OxFrameWithHeader trophiesPanel;
        private readonly ControlLayouts<GameField> trophiesLayouts = new();
        private readonly ControlLayouts<GameField> baseLayouts = new();
        private readonly ControlLayouts<GameField> releaseLayouts = new();
    }
}