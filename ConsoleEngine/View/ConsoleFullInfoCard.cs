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

            Layouter.Template.Left = imageLayout.Right + 90;

            ControlLayout<ConsoleField> generationLayout = Layouter.AddFromTemplate(ConsoleField.Generation);
            generationLayout.Top = 2;

            return new ControlLayouts<ConsoleField>()
            {
                imageLayout,
                generationLayout,
                Layouter.AddFromTemplate(ConsoleField.Model, -2),
                Layouter.AddFromTemplate(ConsoleField.Firmware, -2)
            };
        }

        protected override void AfterControlLayout() 
        {
            OxLabel? foldersControl =
                (OxLabel?)Layouter.PlacedControl(ConsoleField.Folders)?.Control;

            if (foldersControl != null)
            {
                foldersControl.MaximumSize = new Size(460, 280);
                foldersControl.TextAlign = ContentAlignment.TopLeft;
            }
        }

        private readonly ConsoleGenerationHelper generationHelper = 
            TypeHelper.Helper<ConsoleGenerationHelper>();

        private void FillStoragesLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = StoragesPanel;
            Layouter.Template.Top = 12;
            Layouter.Template.Left = 12;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            Layouter.AddFromTemplate(ConsoleField.Storages);
        }

        private void FillFoldersLayout()
        {
            ClearLayoutTemplate();
            Layouter.Template.Parent = FoldersPanel;
            Layouter.Template.Top = 12;
            Layouter.Template.Left = 12;
            Layouter.Template.CaptionVariant = ControlCaptionVariant.None;
            Layouter.AddFromTemplate(ConsoleField.Folders);
        }

        protected override void PrepareLayouts()
        {
            LayoutsLists.Add(FillConsoleLayout());

            if (Item != null && generationHelper.StorageSupport(Item.Generation))
            {
                Headers[StoragesPanel].Visible = true;
                StoragesPanel.Visible = true;
                FillStoragesLayout();
            }
            else
            {
                Headers[StoragesPanel].Visible = false;
                StoragesPanel.Visible = false;
            }

            if (Item != null && generationHelper.FolderSupport(Item.Generation))
            {
                Headers[FoldersPanel].Visible = true;
                FoldersPanel.Visible = true;
                FillFoldersLayout();
            }
            else
            {
                Headers[FoldersPanel].Visible = false;
                FoldersPanel.Visible = false;
            }
        }

        protected override string GetTitle() =>
            Item == null ? "Unknown Console" : Item.Name;

        protected override void PreparePanels()
        {
            PreparePanel(FoldersPanel, "Folders");
            PreparePanel(StoragesPanel, "Storages");
            PreparePanel(ConsolePanel, string.Empty);
        }

        private readonly OxPanel ConsolePanel = new(new Size(200, 90));
        private readonly OxPanel StoragesPanel = new(new Size(200, 160));
        private readonly OxPanel FoldersPanel = new(new Size(200, 200));

        public ConsoleFullInfoCard() : base() { }
    }
}