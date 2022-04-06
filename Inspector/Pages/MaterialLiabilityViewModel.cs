using Simplified;
using System.Collections.ObjectModel;

namespace Inspector.Pages
{
    public class MaterialLiabilityViewModel : BaseInpc
    {
        public ObservableCollection<Выдача> Equipments { get; } = new ObservableCollection<Выдача>();

        private Выдача _editableEquipment;
        private ViewMode _mode;
        private Выдача _selectedEquipment;

        public Выдача EditableEquipment { get => _editableEquipment; set => Set(ref _editableEquipment, value); }
        public Выдача SelectedEquipment { get => _selectedEquipment; set => Set(ref _selectedEquipment, value); }

        /// <summary>Режим просмотра.</summary>
        public ViewMode Mode { get => _mode; set => Set(ref _mode, value); }
    }
}
