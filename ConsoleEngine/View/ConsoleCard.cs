using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using OxLibrary;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;


namespace PlayStationGames.ConsoleEngine.View
{
    public class ConsoleCard : ItemCard<ConsoleField, PSConsole, ConsoleFieldGroup>
    {
        protected override OxWidth CardWidth => OxWh.W400;
        protected override OxWidth CardHeight => OxWh.W320;

        public ConsoleCard(ItemViewMode viewMode) : base(viewMode) { }

        protected override void PrepareLayouts()
        {
            FillIconLayout();
            FillBaseLayouts();
            FillBottomLayouts();
        }

        private void FillIconLayout()
        {
            if (Item is null)
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
            ControlLayout<ConsoleField> iconLayout = Layouter[ConsoleField.Icon]!;
            ControlLayout<ConsoleField> modelLayout = Layouter.AddFromTemplate(ConsoleField.FullModel);
            modelLayout.Top = 2;
            modelLayout.Left = iconLayout.Right + 6;
            modelLayout.CaptionVariant = ControlCaptionVariant.None;

            baseLayouts.Add(modelLayout);

            Layouter.Template.Left = iconLayout.Right + 68;
            baseLayouts.Add(Layouter.AddFromTemplate(ConsoleField.FullFirmware, -8));

            if (Item!.Accounts.Count > 0)
                baseLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Accounts, -8));
        }

        private static readonly ConsoleGenerationHelper generationHelper = TypeHelper.Helper<ConsoleGenerationHelper>();

        private void FillBottomLayouts()
        {
            ClearLayoutTemplate();
            bottomLayouts.Clear();

            if (Item is null)
                return;

            Layouter.Template.Left = Layouter[ConsoleField.Icon]!.Left + 84;
            Layouter.Template.Top = Layouter[ConsoleField.Icon]!.Bottom + 6;

            bool needOffsetAccessories = false;

            if (generationHelper.StorageSupport(Item.Generation) 
                && Item.Storages.Count > 0)
            {
                bottomLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Storages));

                if (generationHelper.FolderSupport(Item.Generation)
                    && Item.Folders.Count > 0)
                    bottomLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Folders, -8));

                if (Item.GamesCount > 0)
                    bottomLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Games, -8));

                needOffsetAccessories = true;
            }

            if (needOffsetAccessories)
                bottomLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Accessories, 8));
            else
                bottomLayouts.Add(Layouter.AddFromTemplate(ConsoleField.Accessories));
        }

        protected override void AlignControls()
        {
            Layouter.AlignLabels(baseLayouts, 8);
            Layouter.AlignLabels(bottomLayouts);
        }

        protected override void ClearLayouts()
        {
            baseLayouts.Clear();
            bottomLayouts.Clear();
        }

        protected override string GetTitle() =>
            Item is not null 
                ? Item.Name 
                : string.Empty;

        private readonly ControlLayouts<ConsoleField> baseLayouts = new();
        private readonly ControlLayouts<ConsoleField> bottomLayouts = new();
    }
}