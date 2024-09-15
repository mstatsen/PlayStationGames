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
                ConsoleFieldGroup.Storages,
                ConsoleFieldGroup.Accounts,
                ConsoleFieldGroup.Base
            };

        public override string GetName(ConsoleFieldGroup value) => 
            value switch
            {
                ConsoleFieldGroup.Base => "Console",
                ConsoleFieldGroup.Accessories => "Accessories",
                ConsoleFieldGroup.Storages => "Storages",
                ConsoleFieldGroup.Folders => "Folders",
                ConsoleFieldGroup.Accounts => "Accounts",
                _ => string.Empty,
            };

        public override ConsoleFieldGroup Group(ConsoleField field) => 
            field switch
            {
                ConsoleField.Storages => ConsoleFieldGroup.Storages,
                ConsoleField.Folders => ConsoleFieldGroup.Folders,
                ConsoleField.Accessories => ConsoleFieldGroup.Accessories,
                ConsoleField.Accounts => ConsoleFieldGroup.Accounts,
                _ => ConsoleFieldGroup.Base,
            };

        public override int GroupWidth(ConsoleFieldGroup group) =>
            group switch
            { 
                ConsoleFieldGroup.Folders or
                ConsoleFieldGroup.Accessories =>
                    450,
                _=> 300
            };

        public override int DefaultGroupHeight(ConsoleFieldGroup group) => 
            group switch
            {
                ConsoleFieldGroup.Accessories => 198,
                ConsoleFieldGroup.Accounts or
                ConsoleFieldGroup.Storages => 84,
                _ => 140,
            };

        public override bool IsCalcedHeightGroup(ConsoleFieldGroup group) => 
            group == ConsoleFieldGroup.Base;

        public override DockStyle GroupDock(ConsoleFieldGroup group) => 
            group switch
            {
                ConsoleFieldGroup.Base or
                ConsoleFieldGroup.Storages or
                ConsoleFieldGroup.Accounts =>
                    DockStyle.Top,
                ConsoleFieldGroup.Folders => 
                    DockStyle.Fill,
                ConsoleFieldGroup.Accessories => 
                    DockStyle.Bottom,
                _ => 
                    base.GroupDock(group),
            };
    }
}