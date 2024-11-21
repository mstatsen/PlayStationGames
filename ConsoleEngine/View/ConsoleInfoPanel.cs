using OxLibrary.Controls;
using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using OxLibrary;


namespace PlayStationGames.ConsoleEngine.View
{
    public class ConsoleInfoPanel : ItemInfo<ConsoleField, PSConsole, ConsoleFieldGroup>
    {
        private ControlLayouts<ConsoleField> FillConsoleLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = ConsolePanel;

            ControlLayout<ConsoleField> imageLayout = Layouter.AddFromTemplate(ConsoleField.Icon);
            imageLayout.CaptionVariant = ControlCaptionVariant.None;
            imageLayout.Left = OxWh.W10;
            imageLayout.Width = OxWh.W96;
            imageLayout.Height = OxWh.W60;
            
            ControlLayout<ConsoleField> modelLayout = Layouter.AddFromTemplate(ConsoleField.FullModel);
            modelLayout.Top = OxWh.W20;
            modelLayout.Left = imageLayout.Right + 2;
            modelLayout.CaptionVariant = ControlCaptionVariant.None;

            ControlLayout<ConsoleField> firmwareLayout = Layouter.AddFromTemplate(ConsoleField.FullFirmware, 2);
            firmwareLayout.Left = imageLayout.Right + 72;

            return new ControlLayouts<ConsoleField>()
            {
                imageLayout,
                modelLayout,
                firmwareLayout
            };
        }

        protected override void AfterControlLayout() 
        {
            OxLabel? foldersControl =
                (OxLabel?)Layouter.PlacedControl(ConsoleField.Folders)?.Control;

            if (foldersControl is not null)
            {
                foldersControl.MaximumSize = new(460, 280);
                foldersControl.TextAlign = ContentAlignment.TopLeft;
            }
        }

        private ControlLayouts<ConsoleField> CreateFillDockLayouts(ConsoleField field, OxPane parent)
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = parent;
            Layouter.Template.Top = OxWh.W12;
            Layouter.Template.Left = OxWh.W12;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            return new ControlLayouts<ConsoleField>()
            {
                Layouter.AddFromTemplate(field)
            };
        }

        private void AddFillDockLayout(OxPane parentPanel, ConsoleField field) =>
            LayoutsLists.Add(parentPanel, CreateFillDockLayouts(field, parentPanel));

        protected override void PrepareLayouts()
        {
            LayoutsLists.Add(ConsolePanel, FillConsoleLayout());
            AddFillDockLayout(AccountsPanel, ConsoleField.Accounts);
            AddFillDockLayout(StoragesPanel, ConsoleField.Storages);
            AddFillDockLayout(FoldersPanel, ConsoleField.Folders);
            AddFillDockLayout(AccessoriesPanel, ConsoleField.Accessories);
        }

        protected override string GetTitle() =>
            Item is null 
                ? "Unknown Console" 
                : Item.Name;

        protected override void PreparePanels()
        {
            PreparePanel(FoldersPanel, "Folders");
            PreparePanel(StoragesPanel, "Storages");
            PreparePanel(AccountsPanel, "Accounts");
            PreparePanel(AccessoriesPanel, "Accessories");
            PreparePanel(ConsolePanel, string.Empty);
        }

        private readonly OxPane ConsolePanel = new();
        private readonly OxPane StoragesPanel = new();
        private readonly OxPane FoldersPanel = new();
        private readonly OxPane AccountsPanel = new();
        private readonly OxPane AccessoriesPanel = new();

        public ConsoleInfoPanel() : base() { }
    }
}