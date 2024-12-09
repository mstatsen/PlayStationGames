using OxDAOEngine.Data.Fields;
using OxLibrary;

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
                ConsoleFieldGroup.Games,
                ConsoleFieldGroup.Accessories,
                ConsoleFieldGroup.Storages,
                ConsoleFieldGroup.Accounts,
                ConsoleFieldGroup.Firmware,
                ConsoleFieldGroup.GenerationAndModel,
                ConsoleFieldGroup.Base
            };

        public override string GetName(ConsoleFieldGroup value) => 
            value switch
            {
                ConsoleFieldGroup.Base => "Name",
                ConsoleFieldGroup.GenerationAndModel => "Generation and model",
                ConsoleFieldGroup.Firmware => "Firmware",
                ConsoleFieldGroup.Accessories => "Accessories",
                ConsoleFieldGroup.Storages => "Storages",
                ConsoleFieldGroup.Folders => "Folders",
                ConsoleFieldGroup.Accounts => "Accounts",
                ConsoleFieldGroup.Games => "Installed games",
                _ => string.Empty,
            };

        public override ConsoleFieldGroup Group(ConsoleField field) => 
            field switch
            {
                ConsoleField.Storages => 
                ConsoleFieldGroup.Storages,
                ConsoleField.Folders => 
                    ConsoleFieldGroup.Folders,
                ConsoleField.Accessories => 
                    ConsoleFieldGroup.Accessories,
                ConsoleField.Accounts => 
                    ConsoleFieldGroup.Accounts,
                ConsoleField.Generation or
                ConsoleField.Model or
                ConsoleField.ModelCode =>
                    ConsoleFieldGroup.GenerationAndModel,
                ConsoleField.Firmware or
                ConsoleField.FirmwareName or
                ConsoleField.FirmwareVersion => 
                    ConsoleFieldGroup.Firmware,
                _ => 
                    ConsoleFieldGroup.Base,
            };

        public override short GroupWidth(ConsoleFieldGroup group) =>
            group switch
            { 
                ConsoleFieldGroup.Folders or
                ConsoleFieldGroup.Accessories =>
                    450,
                _=> 360
            };

        public override short DefaultGroupHeight(ConsoleFieldGroup group) => 
            group switch
            {
                ConsoleFieldGroup.Base => 
                    42,
                ConsoleFieldGroup.Games => 
                    36,
                ConsoleFieldGroup.Accessories => 
                    198,
                ConsoleFieldGroup.Accounts or
                ConsoleFieldGroup.Storages => 
                    84,
                _ => 
                140,
            };

        public override bool IsCalcedHeightGroup(ConsoleFieldGroup group) => 
            group is ConsoleFieldGroup.GenerationAndModel or
                ConsoleFieldGroup.Firmware;

        public override OxDock GroupDock(ConsoleFieldGroup group) => 
            group switch
            {
                ConsoleFieldGroup.Base or
                ConsoleFieldGroup.GenerationAndModel or
                ConsoleFieldGroup.Firmware or
                ConsoleFieldGroup.Storages or
                ConsoleFieldGroup.Accounts =>
                    OxDock.Top,
                ConsoleFieldGroup.Folders => 
                    OxDock.Fill,
                ConsoleFieldGroup.Games or
                ConsoleFieldGroup.Accessories => 
                    OxDock.Bottom,
                _ => 
                    base.GroupDock(group),
            };
    }
}