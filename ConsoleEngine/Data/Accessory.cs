using OxXMLEngine.Data;
using OxXMLEngine.Data.Types;
using OxXMLEngine.XML;
using PlayStationGames.ConsoleEngine.Data.Types;
using System.Xml;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Accessory : DAO
    {
        public Guid Id
        {
            get => id;
            set => id = ModifyValue(id, value);
        }

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
            Id = Guid.Empty;
            type = TypeHelper.DefaultValue<AccessoryType>();
            JoystickType = TypeHelper.DefaultValue<JoystickType>();
            color = "Black";
            description = string.Empty;
            count = 1;
        }

        public override void Init() => 
            GenerateGuid();

        private void GenerateGuid() =>
            Id = Guid.NewGuid();

        protected override void LoadData(XmlElement element)
        {
            id = XmlHelper.ValueGuid(element, XmlConsts.Id);
            if (id == Guid.Empty)
                GenerateGuid();

            type = XmlHelper.Value<AccessoryType>(element, XmlConsts.Type);
            joystickType = XmlHelper.Value<JoystickType>(element, XmlConsts.JoystickType);
            color = XmlHelper.Value(element, XmlConsts.Color);
            count = XmlHelper.ValueInt(element, XmlConsts.Count);
            description = XmlHelper.Value(element, XmlConsts.Description);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
           
            XmlHelper.AppendElement(element, XmlConsts.Id, id);
            XmlHelper.AppendElement(element, XmlConsts.Type, type);

            if (type == AccessoryType.Joystick)
            {
                XmlHelper.AppendElement(element, XmlConsts.JoystickType, joystickType);

                if (TypeHelper.Helper<JoystickTypeHelper>().IsColored(joystickType))
                    XmlHelper.AppendElement(element, XmlConsts.Color, color);
            }

            XmlHelper.AppendElement(element, XmlConsts.Count, count);
            XmlHelper.AppendElement(element, XmlConsts.Description, description);
        }

        public override string ToString()
        {
            string countStr = Count > 1 ? Count.ToString() + " " : "";
            string typeStr = JoystickType == JoystickType.Other ? " " + TypeHelper.FullName(Type) : "";
            string joystickTypeName = Type == AccessoryType.Joystick ? TypeHelper.FullName(JoystickType) : "";
            return $"{countStr}{Color} {joystickTypeName}{typeStr}";
        }

        private Guid id = Guid.Empty;
        private AccessoryType type = default!;
        private JoystickType joystickType = default!;
        private string color = "Black";
        private string description = string.Empty;
        private int count = 1;
    }
}