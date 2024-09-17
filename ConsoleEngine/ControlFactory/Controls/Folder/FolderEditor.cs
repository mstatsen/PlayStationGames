using OxLibrary;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public partial class FolderEditor : ListItemEditor<Folder, ConsoleField, PSConsole>
    {
        protected override void FillControls(Folder item) => 
            folderControl!.Value = item.Name;

        protected override int ContentHeight => 60;

        protected override void GrabControls(Folder item) => 
            item.Name = folderControl!.StringValue;

        protected override string Title => "Folder";

        protected override void CreateControls()
        {
            folderControl = new TextAccessor<ConsoleField, PSConsole>(Context.Builder.Context(ConsoleField.Folders))
            {
                Parent = this,
                Dock = DockStyle.Fill
            };
        }

        protected override void SetPaddings() => 
            MainPanel.Paddings.SetSize(OxSize.Large);

        protected override string EmptyMandatoryField() =>
            folderControl!.IsEmpty ? "Folder name" : base.EmptyMandatoryField();

        private TextAccessor<ConsoleField, PSConsole>? folderControl;
    }
}