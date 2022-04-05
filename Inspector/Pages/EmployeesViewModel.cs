using Simplified;
using System.Collections.ObjectModel;

namespace Inspector.Pages
{
    public class EmployeesViewModel : BaseInpc
    {
        public ObservableCollection<Сотрудник> Equipments { get; } = new ObservableCollection<Сотрудник>();

        private Сотрудник _editableEquipment;
        private ViewMode _mode;
        private Сотрудник _selectedEquipment;

        public Сотрудник EditableEquipment { get => _editableEquipment; set => Set(ref _editableEquipment, value); }
        public Сотрудник SelectedEquipment { get => _selectedEquipment; set => Set(ref _selectedEquipment, value); }

        /// <summary>Режим просмотра.</summary>
        public ViewMode Mode { get => _mode; set => Set(ref _mode, value); }
    }
}
