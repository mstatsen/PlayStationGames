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
        public ConsoleEditorLayoutsGenerator(FieldGroupFrames<ConsoleField, ConsoleFieldGroup> groupFrames,
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

        public override OxWidth Top(ConsoleField field) => OxWh.W8;

        public override OxWidth Left(ConsoleField field) =>
            field switch
            {
                ConsoleField.Model or
                ConsoleField.ModelCode or
                ConsoleField.FirmwareName or
                ConsoleField.FirmwareVersion =>
                    OxWh.W154,
                _ => OxWh.W94
            };


        public override OxWidth Width(ConsoleField field) =>
            field switch
            {
                ConsoleField.Generation or
                ConsoleField.Firmware =>
                    OxWh.W240,
                _ => OxWh.W180
            };

        public override List<ConsoleField> TitleAccordionFields() => new() 
        { 
            ConsoleField.Name 
        };

        public override ConsoleField BackColorField => ConsoleField.Firmware;
    }
}