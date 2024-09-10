using OxXMLEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.Data.Fields
{
    public class ConsoleFieldGroupHelper : FieldGroupHelper<ConsoleField, ConsoleFieldGroup>
    {
        public override ConsoleFieldGroup EmptyValue() =>
            ConsoleFieldGroup.Base;

        public override List<ConsoleFieldGroup> EditedList() =>
            new()
            {
                ConsoleFieldGroup.Folders,
                ConsoleFieldGroup.Accessories,
                ConsoleFieldGroup.Base,
                ConsoleFieldGroup.Storages
            };

        public override string GetName(ConsoleFieldGroup value) => 
            value switch
            {
                ConsoleFieldGroup.Base => "Console",
                ConsoleFieldGroup.Accessories => "Accessories",
                ConsoleFieldGroup.Storages => "Storages",
                ConsoleFieldGroup.Folders => "Folders",
                _ => string.Empty,
            };

        public override ConsoleFieldGroup Group(ConsoleField field) => 
            field switch
            {
                ConsoleField.Storages => ConsoleFieldGroup.Storages,
                ConsoleField.Folders => ConsoleFieldGroup.Folders,
                ConsoleField.Accessories => ConsoleFieldGroup.Accessories,
                _ => ConsoleFieldGroup.Base,
            };

        public override int GroupWidth(ConsoleFieldGroup group) => 300;

        public override int DefaultGroupHeight(ConsoleFieldGroup group) => 
            group switch
            {
                ConsoleFieldGroup.Folders => 280,
                ConsoleFieldGroup.Accessories => 200,
                _ => 140,
            };

        public override bool IsCalcedHeightGroup(ConsoleFieldGroup group) => false;

        public override DockStyle GroupDock(ConsoleFieldGroup group) => 
            group switch
            {
                ConsoleFieldGroup.Base or 
                ConsoleFieldGroup.Folders => 
                    DockStyle.Fill,
                ConsoleFieldGroup.Storages or
                ConsoleFieldGroup.Accessories => 
                    DockStyle.Bottom,
                _ => 
                    base.GroupDock(group),
            };
    }
}