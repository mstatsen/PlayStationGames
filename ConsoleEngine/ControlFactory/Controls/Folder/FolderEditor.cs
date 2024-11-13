using OxLibrary;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public partial class FolderEditor : CustomItemEditor<Folder, ConsoleField, PSConsole>
    {
        protected override void FillControls(Folder item) => 
            folderControl!.Value = item.Name;

        public override Bitmap? FormIcon => OxIcons.Folder;

        protected override int ContentHeight => 60;

        protected override void GrabControls(Folder item) => 
            item.Name = folderControl!.StringValue;

        protected override void CreateControls()
        {
            folderControl = Builder.Accessor("Folder:Name", FieldType.String);
            folderControl.Parent = this;
            folderControl.Dock = DockStyle.Fill;
        }

        protected override void SetPaddings() => 
            MainPanel.Paddings.SetSize(OxSize.Large);

        protected override string EmptyMandatoryField() =>
            folderControl!.IsEmpty 
                ? "Folder name" 
                : base.EmptyMandatoryField();

        private IControlAccessor? folderControl;
    }
}