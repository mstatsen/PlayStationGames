using OxLibrary.Controls;
using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;


namespace PlayStationGames.ConsoleEngine.View
{
    public class ConsoleFullInfoCard : ItemInfo<ConsoleField, PSConsole, ConsoleFieldGroup>
    {
        private ControlLayouts<ConsoleField> FillConsoleLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = ConsolePanel;

            ControlLayout<ConsoleField> imageLayout = Layouter.AddFromTemplate(ConsoleField.Icon);
            imageLayout.CaptionVariant = ControlCaptionVariant.None;
            imageLayout.Left = 10;
            imageLayout.Width = 96;
            imageLayout.Height = 60;
            
            ControlLayout<ConsoleField> modelLayout = Layouter.AddFromTemplate(ConsoleField.FullModel);
            modelLayout.Top = 20;
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

            if (foldersControl != null)
            {
                foldersControl.MaximumSize = new(460, 280);
                foldersControl.TextAlign = ContentAlignment.TopLeft;
            }
        }

        private readonly ConsoleGenerationHelper generationHelper = 
            TypeHelper.Helper<ConsoleGenerationHelper>();

        private ControlLayouts<ConsoleField> CreateFillDockLayouts(ConsoleField field, OxPanel parent)
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = parent;
            Layouter.Template.Top = 12;
            Layouter.Template.Left = 12;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            return new ControlLayouts<ConsoleField>()
            {
                Layouter.AddFromTemplate(field)
            };
        }

        private void AddFillDockLayout(OxPanel parentPanel, ConsoleField field) =>
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
            Item == null ? "Unknown Console" : Item.Name;

        protected override void PreparePanels()
        {
            PreparePanel(FoldersPanel, "Folders");
            PreparePanel(StoragesPanel, "Storages");
            PreparePanel(AccountsPanel, "Accounts");
            PreparePanel(AccessoriesPanel, "Accessories");
            PreparePanel(ConsolePanel, string.Empty);
        }

        private readonly OxPanel ConsolePanel = new();
        private readonly OxPanel StoragesPanel = new();
        private readonly OxPanel FoldersPanel = new();
        private readonly OxPanel AccountsPanel = new();
        private readonly OxPanel AccessoriesPanel = new();

        public ConsoleFullInfoCard() : base() { }
    }
}