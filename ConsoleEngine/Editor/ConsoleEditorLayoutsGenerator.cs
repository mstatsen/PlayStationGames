using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Editor;
using OxLibrary;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.Editor
{
    public class ConsoleEditorLayoutsGenerator
        : EditorLayoutsGenerator<ConsoleField, PSConsole, ConsoleFieldGroup>
    {
        public ConsoleEditorLayoutsGenerator(FieldGroupPanels<ConsoleField, ConsoleFieldGroup> groupFrames,
            ControlLayouter<ConsoleField, PSConsole> layouter) : base(groupFrames, layouter) { }

        public override List<ConsoleField> ControlsWithoutLabel() =>
            new()
            {
                ConsoleField.Name,
                ConsoleField.Folders,
                ConsoleField.Storages,
                ConsoleField.Accessories,
                ConsoleField.Accounts
            };

        public override List<ConsoleField> FillDockFields() =>
            new()
            {
                ConsoleField.Name,
                ConsoleField.Folders,
                ConsoleField.Storages,
                ConsoleField.Accessories,
                ConsoleField.Accounts
            };

        public override List<ConsoleField> OffsettingFields() =>
            new()
            {
                ConsoleField.Model,
                ConsoleField.ModelCode,
                ConsoleField.FirmwareName,
                ConsoleField.FirmwareVersion,
            };

        public override short Top(ConsoleField field) => 8;

        public override short Left(ConsoleField field) =>
            field switch
            {
                ConsoleField.Model or
                ConsoleField.ModelCode or
                ConsoleField.FirmwareName or
                ConsoleField.FirmwareVersion =>
                    154,
                _ => 94
            };


        public override short Width(ConsoleField field) =>
            field switch
            {
                ConsoleField.Generation or
                ConsoleField.Firmware =>
                    240,
                _ => 180
            };

        public override List<ConsoleField> TitleAccordionFields() => new() 
        { 
            ConsoleField.Name 
        };

        public override ConsoleField BackColorField => ConsoleField.Firmware;
    }
}