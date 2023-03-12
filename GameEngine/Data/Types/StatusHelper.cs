using OxXMLEngine.Data.Types;
using System.Drawing;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class StatusHelper : AbstractStyledTypeHelper<Status>
    {
        public override Color GetBaseColor(Status value) => 
            throw new System.NotImplementedException();

        public override Status EmptyValue() => 
            Status.Unknown;

        public override Color GetFontColor(Status value) => 
            value switch
            {
                Status.NotStarted => Color.FromArgb(84, 112, 25),
                Status.Started => Color.FromArgb(25, 61, 111),
                Status.Completed => Color.FromArgb(92, 170, 186),
                Status.Unknown => Color.FromArgb(129, 129, 129),
                _ => Color.Black,
            };

        public override string GetName(Status value) => 
            value switch
            {
                Status.NotStarted => "Not Started",
                Status.Started => "Started",
                Status.Completed => "Completed",
                _ => "Unknown",
            };
    }
}