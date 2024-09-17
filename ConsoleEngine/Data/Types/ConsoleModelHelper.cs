using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Types
{
    public class ConsoleModelHelper : FieldAccordingHelper<ConsoleField, ConsoleModel>
    {
        public override string GetName(ConsoleModel value) => 
            value switch
            {
                ConsoleModel.PS5Fat or 
                ConsoleModel.PS4Fat or 
                ConsoleModel.PS3Fat or 
                ConsoleModel.PSVitaFat or 
                ConsoleModel.PS2Fat or 
                ConsoleModel.PSPFat or 
                ConsoleModel.PS1Fat => 
                    "Fat",
                ConsoleModel.PS4Slim or 
                ConsoleModel.PS3Slim or 
                ConsoleModel.PSVitaSlim or 
                ConsoleModel.PS2Slim => 
                    "Slim",
                ConsoleModel.PSPSlimLite =>
                    "Slim and Lite",
                ConsoleModel.PS4Pro => 
                    "Pro",
                ConsoleModel.PS3SuperSlim => 
                    "Super Slim",
                ConsoleModel.PSVitaTV => 
                    "Vita TV",
                ConsoleModel.PSPBright => 
                    "Bright",
                ConsoleModel.PSPStreet => 
                    "Street",
                ConsoleModel.PSPGO => 
                    "GO",
                ConsoleModel.PS1One => 
                    "One",
                _ => 
                    "Unknown",
            };

        public override string GetShortName(ConsoleModel value) => 
            value switch
            {
                ConsoleModel.PS5Fat or 
                ConsoleModel.PS4Fat or 
                ConsoleModel.PS3Fat or 
                ConsoleModel.PSVitaFat or 
                ConsoleModel.PS2Fat or 
                ConsoleModel.PSPFat or 
                ConsoleModel.PS1Fat => 
                    "Fat",
                ConsoleModel.PS4Slim or 
                ConsoleModel.PS3Slim or 
                ConsoleModel.PSVitaSlim or 
                ConsoleModel.PS2Slim or 
                ConsoleModel.PSPSlimLite => 
                    "Slim",
                ConsoleModel.PS4Pro => 
                    "Pro",
                ConsoleModel.PS3SuperSlim => 
                    "SuperSlim",
                ConsoleModel.PSVitaTV => 
                    "TV",
                ConsoleModel.PSPBright => 
                    "FlatSlim",
                ConsoleModel.PSPStreet => 
                    "Street",
                ConsoleModel.PSPGO => 
                    "GO",
                ConsoleModel.PS1One => 
                    "One",
                _ => 
                    "Unknown"
            };

        public override string GetXmlValue(ConsoleModel value) => 
            value.ToString();

        public override ConsoleModel EmptyValue() =>
            ConsoleModel.Unknown;

        public override ConsoleModel DefaultValue() =>
            ConsoleModel.Unknown;

        public override object DependsOnValue(ConsoleModel value) => 
            value switch
            {
                ConsoleModel.PS5Fat => 
                    ConsoleGeneration.PS5,
                ConsoleModel.PS4Fat or 
                ConsoleModel.PS4Slim or 
                ConsoleModel.PS4Pro => 
                    ConsoleGeneration.PS4,
                ConsoleModel.PS3Fat or 
                ConsoleModel.PS3Slim or 
                ConsoleModel.PS3SuperSlim => 
                    ConsoleGeneration.PS3,
                ConsoleModel.PSVitaFat or 
                ConsoleModel.PSVitaSlim or 
                ConsoleModel.PSVitaTV => 
                    ConsoleGeneration.PSVita,
                ConsoleModel.PS2Fat or 
                ConsoleModel.PS2Slim => 
                    ConsoleGeneration.PS2,
                ConsoleModel.PSPFat or 
                ConsoleModel.PSPSlimLite or 
                ConsoleModel.PSPBright or 
                ConsoleModel.PSPStreet or 
                ConsoleModel.PSPGO => 
                    ConsoleGeneration.PSP,
                ConsoleModel.PS1Fat or 
                ConsoleModel.PS1One => 
                    ConsoleGeneration.PS1,
                _ => 
                    ConsoleGeneration.Unknown
            };

        public override List<ConsoleModel> DependedList(ConsoleField field, object value)
        {
            switch (field)
            {
                case ConsoleField.Generation:
                    if (value is ConsoleGeneration generation)
                    { 
                        List<ConsoleModel> result = new();

                        foreach (ConsoleModel model in All())
                            if (DependsOnValue<ConsoleGeneration>(model) == generation)
                                result.Add(model);

                        return result;
                    }

                    break;
            }
            return base.DependedList(field, value);
        }

        public override Color GetBaseColor(ConsoleModel value) => default;

        public override Color GetFontColor(ConsoleModel value) => default;
    }
}
