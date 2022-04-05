using Simplified;
using System.Collections.ObjectModel;

namespace Inspector.Pages
{
    public class WarehouseViewModel : BaseInpc
    {
        public ObservableCollection<Техника> Equipments { get; } = new ObservableCollection<Техника>();

        private Техника _editableEquipment;
        private ViewMode _mode;
        private Техника _selectedEquipment;

        public Техника EditableEquipment { get => _editableEquipment; set => Set(ref _editableEquipment, value); }
        public Техника SelectedEquipment { get => _selectedEquipment; set => Set(ref _selectedEquipment, value); }

        /// <summary>Режим просмотра.</summary>
        public ViewMode Mode { get => _mode; set => Set(ref _mode, value); }
    }
}
