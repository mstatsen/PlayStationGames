using OxXMLEngine;
using OxXMLEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class ScreenViewHelper : AbstractTypeHelper<ScreenView>
    {
        public override string GetName(ScreenView value) => 
            value switch
            {
                ScreenView.sv2D => "2D",
                ScreenView.sv3D => "3D",
                ScreenView.svIsometric => "Isometric",
                ScreenView.svFP => "First-person",
                _ => "Other",
            };

        public override string GetShortName(ScreenView value) => 
            value switch
            {
                ScreenView.sv2D => "2D",
                ScreenView.sv3D => "3D",
                ScreenView.svIsometric => "2,5D",
                ScreenView.svFP => "FP",
                _ => Consts.Short_Unknown,
            };

        public override ScreenView EmptyValue() => 
            ScreenView.svOther;
    }
}