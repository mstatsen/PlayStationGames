using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Editor;

namespace PlayStationGames.ConsoleEngine.Editor
{
    public class ConsoleEditorLayoutsGenerator
        : EditorLayoutsGenerator<ConsoleField, PSConsole, ConsoleFieldGroup>
    {
        public ConsoleEditorLayoutsGenerator(FieldGroupFrames<ConsoleField, ConsoleFieldGroup> groupFrames,
            ControlLayouter<ConsoleField, PSConsole> layouter) : base(groupFrames, layouter) { }

        protected override List<ConsoleField> ControlsWithoutLabel() => 
            new()
            {
                ConsoleField.Folders,
                ConsoleField.Storages,
                ConsoleField.Accessories
            };

        protected override List<ConsoleField> FillDockFields() => 
            new()
            {
                ConsoleField.Folders,
                ConsoleField.Storages,
                ConsoleField.Accessories
            };

        protected override List<ConsoleField> OffsettingFields() =>
            new()
            {
                ConsoleField.Generation,
                ConsoleField.Model,
                ConsoleField.Firmware
            };

        protected override int Top(ConsoleField field) => 8;

        protected override int Left(ConsoleField field) => 94;

        protected override int Width(ConsoleField field) => 
            field == ConsoleField.Name ? 180 : 100;

        protected override int Offset(ConsoleField field) =>
            field switch
            {
                ConsoleField.Generation => 4,
                _ => base.Offset(field)
            };
    }
}