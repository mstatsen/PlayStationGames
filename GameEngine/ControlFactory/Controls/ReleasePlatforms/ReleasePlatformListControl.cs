using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Panels;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class ReleasePlatformListControl : CustomListControl<GameField, Game, Platforms, Platform>
    {
        private readonly Dictionary<PlatformType, OxCheckBox> CheckBoxes = new();
        private readonly OxFrame mainPanel = new();

        protected override void ClearValue()
        {
            foreach (OxCheckBox checkBox in CheckBoxes.Values)
                checkBox.Checked = false;
        }

        protected override Control GetControl() =>
            mainPanel;

        protected override void GrabList(Platforms list)
        {
            foreach (var item in CheckBoxes)
                if (item.Value.Checked)
                    list.Add(item.Key);
        }

        protected override void InitComponents()
        {
            PrepareMainPanel();
            Size CalcedSize = PrepareCheckBoxes();
            mainPanel.SetContentSize(CalcedSize);
            MinimumSize = mainPanel.Size;
        }

        private Size PrepareCheckBoxes()
        {
            int top = 0;
            int left = -1;
            int calcedHeight = 0;
            int calcedWidth = 0;
            OxCheckBox? longestFamilyCheckBox = null;
            PlatformFamily oldFamily = TypeHelper.EmptyValue<PlatformFamily>();

            foreach (PlatformType platformType in TypeHelper.Actual<PlatformType>())
            {
                PlatformFamily family = TypeHelper.DependsOnValue<PlatformType, PlatformFamily>(platformType);

                if (oldFamily != family)
                {
                    top = 0;
                    left = longestFamilyCheckBox == null 
                        ? 0 : 
                        longestFamilyCheckBox.Right + 30;
                    longestFamilyCheckBox = null;
                }

                OxCheckBox checkBox = CreateCheckBox(platformType, top, left);

                if (longestFamilyCheckBox == null || 
                    longestFamilyCheckBox.Right < checkBox.Right)
                    longestFamilyCheckBox = checkBox;

                top = checkBox.Bottom + 2;
                oldFamily = family;
                calcedWidth = Math.Max(calcedWidth, checkBox.Right);
                calcedHeight = Math.Max(calcedHeight, checkBox.Bottom);
            }

            return new Size(calcedWidth + 12, calcedHeight);
        }

        private void PrepareMainPanel()
        {
            mainPanel.Parent = this;
            mainPanel.Paddings.Horizontal = 12;
            mainPanel.Paddings.VerticalOx = OxSize.Extra;
        }

        private OxCheckBox CreateCheckBox(PlatformType platformType, int top, int left)
        {
            OxCheckBox checkBox = new()
            {
                Parent = mainPanel,
                Top = top,
                Left = left,
                Text = TypeHelper.ShortName(platformType),
                AutoSize = true
            };

            CheckBoxes.Add(platformType, checkBox);
            return checkBox;
        }

        protected override void SetValuePart(Platform valuePart) => 
            CheckBoxes[valuePart.Type].Checked = true;

        protected override Color GetControlColor() => 
            mainPanel.BaseColor;

        protected override void SetControlColor(Color value)
        {
            BaseColor = value;
            BackColor = Colors.Lighter(8);
            mainPanel.BaseColor = value;

            foreach (OxCheckBox checkBox in CheckBoxes.Values)
                checkBox.BackColor = Colors.Lighter(7);
        }

        protected override string GetText() => "Release on platforms";

        protected override bool GetReadOnly() => 
            readOnly;

        protected override void SetReadOnly(bool value)
        {
            readOnly = value;

            foreach (OxCheckBox checkBox in CheckBoxes.Values)
                checkBox.ReadOnly = readOnly;
        }

        private bool readOnly = false;
    }
}