using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
using PlayStationGames.ConsoleEngine.Data.Types;
using System.Xml;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Accessory : DAO
    {
        public AccessoryType Type
        {
            get => type;
            set => type = ModifyValue(type, value);
        }

        public JoystickType JoystickType
        {
            get => joystickType;
            set => joystickType = ModifyValue(joystickType, value);
        }

        public string Color
        {
            get => color;
            set => color = StringValue(ModifyValue(color, value));
        }

        public string CoverColor
        {
            get => coverColor;
            set => coverColor = StringValue(ModifyValue(coverColor, value));
        }

        public bool WithCover
        {
            get => withCover;
            set => withCover = BoolValue(ModifyValue(withCover, value));
        }

        public bool WithStickCovers
        {
            get => withStickCovers;
            set => withStickCovers = BoolValue(ModifyValue(withStickCovers, value));
        }

        public string Description
        {
            get => description;
            set => description = StringValue(ModifyValue(description, value));
        }

        public int Count
        {
            get => count;
            set => count = IntValue(ModifyValue(count, value));
        }

        public override void Clear()
        {
            type = TypeHelper.DefaultValue<AccessoryType>();
            JoystickType = TypeHelper.DefaultValue<JoystickType>();
            color = "Black";
            withCover = false;
            withStickCovers = false;
            coverColor = "Black";
            description = string.Empty;
            count = 1;
        }

        public override void Init() { }

        protected override void LoadData(XmlElement element)
        {
            type = XmlHelper.Value<AccessoryType>(element, XmlConsts.Type);
            joystickType = XmlHelper.Value<JoystickType>(element, XmlConsts.JoystickType);
            color = XmlHelper.Value(element, XmlConsts.Color);
            coverColor = XmlHelper.Value(element, XmlConsts.CoverColor);
            withCover = XmlHelper.ValueBool(element, XmlConsts.WithCover);
            withStickCovers = XmlHelper.ValueBool(element, XmlConsts.WithStickCovers);

            count = XmlHelper.ValueInt(element, XmlConsts.Count);

            if (count == 0)
                count = 1;

            description = XmlHelper.Value(element, XmlConsts.Description);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Type, type);

            if (type == AccessoryType.Joystick)
            {
                XmlHelper.AppendElement(element, XmlConsts.JoystickType, joystickType);

                if (TypeHelper.Helper<JoystickTypeHelper>().IsColored(joystickType))
                    XmlHelper.AppendElement(element, XmlConsts.Color, color);

                if (withCover)
                {
                    XmlHelper.AppendElement(element, XmlConsts.WithCover, withCover);
                    XmlHelper.AppendElement(element, XmlConsts.CoverColor, coverColor);
                }

                if (withStickCovers
                    && TypeHelper.Helper<JoystickTypeHelper>().WithSticks(joystickType))
                        XmlHelper.AppendElement(element, XmlConsts.WithStickCovers, withStickCovers);
            }

            if (count > 1)
                XmlHelper.AppendElement(element, XmlConsts.Count, count);

            if (description != string.Empty)
                XmlHelper.AppendElement(element, XmlConsts.Description, description);
        }

        public override string ToString()
        {
            string countStr = Count > 1 ? Count.ToString() + " " : string.Empty;
            string joystickTypeName = Type == AccessoryType.Joystick ? TypeHelper.FullName(JoystickType) + " ": string.Empty;
            string typeStr = JoystickType == JoystickType.Other ? TypeHelper.FullName(Type) : string.Empty;
            string colorStr = Color != string.Empty ? Color + " " : string.Empty;
            string withCoverStr = withCover ? $" with {coverColor} cover" : string.Empty;
            return $"{countStr}{colorStr}{joystickTypeName}{typeStr}{withCoverStr}";
        }

        public override int CompareTo(DAO? other)
        {
            if (Equals(other))
                return 0;

            if (other == null)
                return 1;

            Accessory otherAccessory = (Accessory)other;

            int result = Type.CompareTo(otherAccessory.Type);

            if (result != 0)
                return result;

            result = JoystickType.CompareTo(otherAccessory.JoystickType);

            if (result != 0)
                return result;

            result = Color.CompareTo(otherAccessory.Color);

            if (result != 0)
                return result;

            result = WithCover.CompareTo(otherAccessory.WithCover);

            if (result != 0) 
                return result;

            result = CoverColor.CompareTo(otherAccessory.CoverColor);

            if (result != 0)
                return result;

            result = WithStickCovers.CompareTo(otherAccessory.WithStickCovers);

            return result != 0 
                ? result : 
                description.CompareTo(otherAccessory.Description);
        }

        public override bool Equals(object? obj) => 
            base.Equals(obj) 
            || (obj is Accessory acessory
                    && type.Equals(acessory.Type)
                    && joystickType.Equals(acessory.JoystickType)
                    && color.Equals(acessory.Color)
                    && coverColor.Equals(acessory.CoverColor)
                    && withCover.Equals(acessory.WithCover)
                    && withStickCovers.Equals(acessory.WithStickCovers)
                    && description.Equals(acessory.Description)
                    && count.Equals(acessory.Count)
                );

        private AccessoryType type = default!;
        private JoystickType joystickType = default!;
        private string color = "Black";
        private string coverColor = "Black";
        private bool withCover = false;
        private bool withStickCovers = false;
        private string description = string.Empty;
        private int count = 1;

        public override int GetHashCode() => 
            Type.GetHashCode() ^ (count * color.GetHashCode()) 
            + joystickType.GetHashCode() ^ withCover.GetHashCode() 
            + withStickCovers.GetHashCode() * description.GetHashCode() 
            + coverColor.GetHashCode();
    }
}