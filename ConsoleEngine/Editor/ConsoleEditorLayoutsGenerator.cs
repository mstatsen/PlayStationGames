using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Editor;
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
                ConsoleField.Folders,
                ConsoleField.Storages,
                ConsoleField.Accessories,
                ConsoleField.Accounts
            };

        public override List<ConsoleField> FillDockFields() => 
            new()
            {
                ConsoleField.Folders,
                ConsoleField.Storages,
                ConsoleField.Accessories,
                ConsoleField.Accounts
            };

        public override List<ConsoleField> OffsettingFields() =>
            new()
            {
                ConsoleField.Generation,
                ConsoleField.Model,
                ConsoleField.Firmware
            };

        public override int Top(ConsoleField field) => 8;

        public override int Left(ConsoleField field) => 94;

        public override int Width(ConsoleField field) => 
            field == ConsoleField.Name ? 180 : 100;

        public override int Offset(ConsoleField field) =>
            field switch
            {
                ConsoleField.Generation => 4,
                _ => base.Offset(field)
            };

        public override List<ConsoleField> TitleAccordionFields() => new() 
        { 
            ConsoleField.Name 
        };

        public override ConsoleField BackColorField => ConsoleField.Firmware;
    }
}