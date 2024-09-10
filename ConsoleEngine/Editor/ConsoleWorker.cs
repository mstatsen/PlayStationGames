using OxLibrary;
using OxLibrary.Controls;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Data.Types;
using OxXMLEngine.Editor;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Editor
{
    public class ConsoleWorker : DAOWorker<ConsoleField, PSConsole, ConsoleFieldGroup>
    {
        public ConsoleWorker() : base() { }

        protected override void PrepareStyles()
        {
            Editor.MainPanel.BaseColor = new OxColorHelper(
                TypeHelper.BackColor(Builder.Value<FirmwareType>(ConsoleField.Firmware))
            ).Darker(7);
            ControlPainter.ColorizeControl(installationsButton, Editor.MainPanel.BaseColor);
            installationsWorker.BaseColor = Editor.MainPanel.BaseColor;
        }

        protected override bool SyncFieldValues(ConsoleField field, bool byUser)
        {
            if (new List<ConsoleField>{
                    ConsoleField.Name}
                .Contains(field))
                FillFormCaptionFromControls();

            return field == ConsoleField.Generation;
        }

        private readonly ConsoleGenerationHelper generationHelper = TypeHelper.Helper<ConsoleGenerationHelper>();

        protected override void SetGroupsAvailability()
        {
            base.SetGroupsAvailability();
            Editor.Groups[ConsoleFieldGroup.Folders].Visible = generationHelper.FolderSupport(Generation);
            Editor.Groups[ConsoleFieldGroup.Storages].Visible = generationHelper.StorageSupport(Generation);
            installationsButton.Visible = generationHelper.StorageSupport(Generation);
        }

        private void FillFormCaptionFromControls() =>
            FillFormCaption(Item);

        protected override EditorLayoutsGenerator<ConsoleField, PSConsole, ConsoleFieldGroup> 
            CreateLayoutsGenerator(FieldGroupFrames<ConsoleField, ConsoleFieldGroup> frames, 
                ControlLayouter<ConsoleField, PSConsole> layouter) =>
                new ConsoleEditorLayoutsGenerator(frames, layouter);

        private readonly OxButton installationsButton = new("Installations", null)
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
            Font = new Font(Styles.FontFamily, Styles.DefaultFontSize)
        };

        protected override void AfterLayoutControls()
        {
            base.AfterLayoutControls();

            Control? firmwareControl = Layouter.PlacedControl(ConsoleField.Firmware)?.Control;

            if (firmwareControl != null)
            {
                installationsButton.Parent = firmwareControl.Parent;
                installationsButton.Top = firmwareControl.Bottom + 16;
                installationsButton.Left = 8;
                installationsButton.SetContentSize(
                    installationsButton.Parent.Width - installationsButton.Left * 2 - 2,
                    40);
                installationsButton.Click -= InstallationsClickHandler;
                installationsButton.Click += InstallationsClickHandler;
            }
            else installationsButton.Visible = false;

            installationsWorker.Renew(Item);
        }

        private void InstallationsClickHandler(object? sender, EventArgs e)
        {
            if (Item != null)
                installationsWorker.Show();
        }

        private readonly InstallationsWorker installationsWorker = new();

        protected override List<List<ConsoleField>> LabelGroups =>
            new()
            {
                new List<ConsoleField> {
                    ConsoleField.Name,
                    ConsoleField.Generation,
                    ConsoleField.Model,
                    ConsoleField.Firmware
                }
            };

        private ConsoleGeneration Generation =>
            Builder.Value<ConsoleGeneration>(ConsoleField.Generation);

        protected override void BeforeGrabControls()
        {
            base.BeforeGrabControls();

            if (!generationHelper.FolderSupport(Generation))
                Builder[ConsoleField.Folders].Clear();

            if (!generationHelper.StorageSupport(Generation))
                Builder[ConsoleField.Storages].Clear();
        }

        protected override void AfterGrabControls()
        {
            base.AfterGrabControls();
            installationsWorker.Save();
        }
    }
}