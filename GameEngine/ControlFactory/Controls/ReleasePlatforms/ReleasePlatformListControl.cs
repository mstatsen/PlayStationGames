using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Geometry;
using OxLibrary.Interfaces;
using OxLibrary.Panels;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls;

public class ReleasePlatformListControl : CustomItemsControl<GameField, Game, Platforms, Platform>
{
    private readonly Dictionary<PlatformType, OxCheckBox> CheckBoxes = new();
    private readonly OxFrame mainPanel = new();

    protected override void ClearValue()
    {
        foreach (OxCheckBox checkBox in CheckBoxes.Values)
            checkBox.Checked = false;
    }

    protected override IOxControl GetControl() =>
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
        mainPanel.Size = PrepareCheckBoxes();
        MinimumSize = mainPanel.Size;
    }

    private OxSize PrepareCheckBoxes()
    {
        short top = 0;
        short left = 0;
        short calcedHeight = 0;
        short calcedWidth = 0;
        OxCheckBox? longestFamilyCheckBox = null;
        PlatformFamily oldFamily = TypeHelper.EmptyValue<PlatformFamily>();

        foreach (PlatformType platformType in TypeHelper.Actual<PlatformType>())
        {
            PlatformFamily family = TypeHelper.DependsOnValue<PlatformType, PlatformFamily>(platformType);

            if (!oldFamily.Equals(family))
            {
                top = 0;
                left =
                    OxSh.Short(
                        longestFamilyCheckBox is not null
                            ? OxSh.Add(longestFamilyCheckBox!.Right, 30)
                            : 0
                    );
                longestFamilyCheckBox = null;
            }

            OxCheckBox checkBox = CreateCheckBox(platformType, top, left);

            if (longestFamilyCheckBox is null
                || longestFamilyCheckBox.Right < checkBox.Right)
                longestFamilyCheckBox = checkBox;

            top = checkBox.Bottom;
            oldFamily = family;
            calcedWidth = Math.Max(calcedWidth, checkBox.Right);
            calcedHeight = Math.Max(calcedHeight, checkBox.Bottom);
        }

        return new(calcedWidth, calcedHeight);
    }


    protected override void OnSetFixedItems() 
    {
        foreach (OxCheckBox checkBox in CheckBoxes.Values)
        {
            checkBox.ReadOnly = OxB.F;
            checkBox.Font = new(checkBox.Font, FontStyle.Regular);
        }

        if (FixedItems is null)
            return;

        foreach (Platform platform in FixedItems)
        {
            OxCheckBox checkBox = CheckBoxes[platform.Type];
            checkBox.Checked = true;
            checkBox.ReadOnly = OxB.T;
            checkBox.Font = new(checkBox.Font, FontStyle.Bold);
        }
    }

    private void PrepareMainPanel()
    {
        mainPanel.Parent = this;
        mainPanel.Padding.Horizontal = 12;
        mainPanel.Padding.Top = 6;
        mainPanel.Padding.Bottom = 5;
    }

    private OxCheckBox CreateCheckBox(PlatformType platformType, short top, short left)
    {
        OxCheckBox checkBox = new()
        {
            Parent = mainPanel,
            Top = top,
            Left = left,
            Text = TypeHelper.ShortName(platformType),
            AutoSize = OxB.T
        };

        checkBox.CheckedChanged += CheckBoxCheckedChangedHandler;
        CheckBoxes.Add(platformType, checkBox);
        return checkBox;
    }

    private void CheckBoxCheckedChangedHandler(object? sender, EventArgs e) => 
        ValueChangeHandler?.Invoke(this, EventArgs.Empty);

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

    protected override OxBool GetReadOnly() => 
        readOnly;

    protected override void SetReadOnly(OxBool value)
    {
        readOnly = value;

        foreach (OxCheckBox checkBox in CheckBoxes.Values)
            checkBox.ReadOnly = readOnly;
    }

    private OxBool readOnly;

    protected override void SetItemEdited(EventHandler? value)
    {
        foreach (OxCheckBox checkBox in CheckBoxes.Values)
            checkBox.CheckedChanged -= ItemEdited;

        base.SetItemEdited(value);

        foreach (OxCheckBox checkBox in CheckBoxes.Values)
            checkBox.CheckedChanged += ItemEdited;
    }

    protected override bool IndentItems => true;

    public override object? PrepareValueToReadOnly(Platforms? value) => 
        "Released on\r\n" + base.PrepareValueToReadOnly(value);
}