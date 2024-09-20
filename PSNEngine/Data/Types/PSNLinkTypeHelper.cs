using OxDAOEngine.Data.Types;

namespace PlayStationGames.PSNEngine.Data.Types
{
    public abstract class PSNLinkTypeHelper<TField> : FieldAccordingHelper<TField, PSNLinkType>, ILinkHelper<TField>
        where TField : notnull, Enum
    {
        public TField ExtractFieldName => 
            GetExtractFieldName();

        protected abstract TField GetExtractFieldName();

        public override PSNLinkType EmptyValue() => PSNLinkType.Other;

        public override Color GetBaseColor(PSNLinkType value) =>
            value switch
            {
                PSNLinkType.PSN =>
                    Color.FromArgb(130, 180, 130),
                PSNLinkType.Stratege =>
                    Color.FromArgb(180, 170, 130),
                PSNLinkType.PSNProfiles =>
                    Color.FromArgb(130, 160, 180),
                _ =>
                    Color.FromArgb(170, 150, 200),
            };

        public override Color GetFontColor(PSNLinkType value) =>
            value switch
            {
                PSNLinkType.PSN =>
                    Color.FromArgb(130, 180, 130),
                PSNLinkType.Stratege =>
                    Color.FromArgb(180, 170, 130),
                PSNLinkType.PSNProfiles =>
                    Color.FromArgb(130, 160, 180),
                _ =>
                    Color.FromArgb(170, 150, 200),
            };

        public override string GetName(PSNLinkType value) =>
            value switch
            {
                PSNLinkType.PSN => "PSN",
                PSNLinkType.Stratege => "Stratege",
                PSNLinkType.PSNProfiles => "PSNProfiles",
                _ => "Other"
            };

        public bool IsMandatoryLink(object value) => !PSNLinkType.Other.Equals(value);
    }
}