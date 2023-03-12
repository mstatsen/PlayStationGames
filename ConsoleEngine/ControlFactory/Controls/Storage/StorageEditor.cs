using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Controls;
using OxXMLEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public partial class StorageEditor : ListItemEditor<Storage, ConsoleField, PSConsole>
    {
        private int PrepareControl(IControlAccessor accessor, 
            string caption, int lastBottom = -1, bool fullRow = true)
        {
            accessor.Parent = this;
            accessor.Left = 80;
            accessor.Top = lastBottom == -1 ? 8 : lastBottom + 4;
            accessor.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            if (fullRow)
            {
                accessor.Anchor |= AnchorStyles.Right;
                accessor.Width = MainPanel.ContentContainer.Width - accessor.Left - 8;
            }
            else
                accessor.Width = 64;

            accessor.Height = 24;
            CreateLabel(caption, accessor);
            return accessor.Bottom;
        }

        protected override void CreateControls()
        {
            //TODO: replace with builder.Accessor
            nameControl = new TextAccessor<ConsoleField, PSConsole>(Context.Builder.Context(ConsoleField.Storages));
            sizeControl = new TextAccessor<ConsoleField, PSConsole>(Context.Builder.Context(ConsoleField.Storages));
            freeSizeControl = new TextAccessor<ConsoleField, PSConsole>(Context.Builder.Context(ConsoleField.Storages));
            placementControl = new EnumAccessor<ConsoleField, PSConsole, StoragePlacement>(Context.Builder.Context(ConsoleField.Storages));

            int lastBottom = PrepareControl(nameControl, "Name");
            lastBottom = PrepareControl(placementControl, "Placement", lastBottom);
            lastBottom = PrepareControl(sizeControl, "Size", lastBottom, false);
            PrepareControl(freeSizeControl, "Free size", lastBottom, false);
            CreateLabel("Gb", sizeControl, true);
            CreateLabel("Gb", freeSizeControl, true);
            placementControl.ValueChangeHandler += OnChangePlacement;
        }

        private void OnChangePlacement(object? sender, EventArgs e) =>
            SyncNameAndPlacenent();

        private void SyncNameAndPlacenent()
        {
            string? newPlacementName = TypeHelper.Name(placementControl!.EnumValue);

            if (nameControl!.IsEmpty || nameControl.StringValue == lastPlacementName)
                nameControl.Value = newPlacementName;

            lastPlacementName = newPlacementName;
        }

        protected override void FillControls(Storage item)
        {
            nameControl!.Value = item.Name;
            placementControl!.Value = item.Placement;
            lastPlacementName = TypeHelper.Name(item.Placement);
            SyncNameAndPlacenent();
            sizeControl!.Value = item.Size;
            freeSizeControl!.Value = item.FreeSize;
        }

        protected override void GrabControls(Storage item)
        {
            item.Name = nameControl!.StringValue;
            item.Placement = placementControl!.EnumValue;
            item.Size = sizeControl!.StringValue;
            item.FreeSize = freeSizeControl!.StringValue;
        }

        protected override string Title => "Storage";

        protected override string EmptyMandatoryField() => 
            nameControl!.IsEmpty ? "Name"
                : placementControl!.IsEmpty ? "Placement"
                : base.EmptyMandatoryField();

        protected override int ContentHeight => freeSizeControl!.Bottom + 8;

        private EnumAccessor<ConsoleField, PSConsole, StoragePlacement>? placementControl;
        private IControlAccessor? nameControl;
        private IControlAccessor? sizeControl;
        private IControlAccessor? freeSizeControl;
        private string? lastPlacementName;
    }
}