using OxLibrary.Controls;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;


namespace PlayStationGames.ConsoleEngine.View
{
    public class ConsoleCard : ItemCard<ConsoleField, PSConsole, ConsoleFieldGroup>
    {
        protected override int CardWidth => 350;
        protected override int CardHeight => 260;

        public ConsoleCard(ItemViewMode viewMode) : base(viewMode) { }

        protected override void PrepareLayouts()
        {
            FillIconLayout();
            FillBaseLayouts();
            FillBottomLayouts();
        }

        private void FillIconLayout()
        {
            if (Item == null)
                return;

            ControlLayout<ConsoleField> imageLayout = Layouter.AddFromTemplate(ConsoleField.Icon);
            imageLayout.CaptionVariant = ControlCaptionVariant.None;
            imageLayout.Width = 108;
            imageLayout.Height = 60;
        }

        private void FillBaseLayouts()
        {
            ClearLayoutTemplate();
            baseLayouts.Clear();
            Layouter.Template.Left = Layouter[ConsoleField.Icon]!.Right + 84;
            baseLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Generation));
            baseLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Model, -6));
            baseLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Firmware, -6));
        }

        private static readonly ConsoleGenerationHelper generationHelper = TypeHelper.Helper<ConsoleGenerationHelper>();

        private void FillBottomLayouts()
        {
            ClearLayoutTemplate();
            bottomLayouts.Clear();

            if (Item == null)
                return;

            Layouter.Template.Left = Layouter[ConsoleField.Icon]!.Left + 72;
            Layouter.Template.Top = Layouter[ConsoleField.Icon]!.Bottom + 12;
            bottomLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Storages))
                .Visible = generationHelper.StorageSupport(Item.Generation);
            bottomLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Games, 16))
                .Visible = generationHelper.StorageSupport(Item.Generation);
            bottomLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Folders, 16))
                .Visible = generationHelper.FolderSupport(Item.Generation);
            
        }

        protected override void AlignControls()
        {
            Layouter.AlignLabels(baseLayouts);
            Layouter.AlignLabels(bottomLayouts);

            foreach (ConsoleField field in baseLayouts.Fields)
                Layouter.PlacedControl(field)!.Control.Left -= 8;
        }

        protected override void ClearLayouts()
        {
            baseLayouts.Clear();
            bottomLayouts.Clear();
        }

        protected override string GetTitle() =>
            Item != null ? Item.Name : string.Empty;

        protected override void AfterLayoutControls()
        {
            base.AfterLayoutControls();
            WrapStoragesAndFolders();
        }

        private void WrapStoragesAndFolders()
        {
            OxLabel? storagesControl =
                (OxLabel?)Layouter.PlacedControl(ConsoleField.Storages)?.Control;

            if (storagesControl != null)
            {
                storagesControl.MaximumSize = new Size(290, 46);
                storagesControl.TextAlign = ContentAlignment.TopLeft;
            }

            OxLabel? foldersControl =
                (OxLabel?)Layouter.PlacedControl(ConsoleField.Folders)?.Control;

            if (foldersControl != null)
            {
                foldersControl.MaximumSize = new Size(290, 120);
                foldersControl.TextAlign = ContentAlignment.TopLeft;
            }
        }

        private readonly ControlLayouts<ConsoleField> baseLayouts = new();
        private readonly ControlLayouts<ConsoleField> bottomLayouts = new();
    }
}