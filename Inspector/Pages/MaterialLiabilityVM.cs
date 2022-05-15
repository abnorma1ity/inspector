using Simplified;
using System.Collections.ObjectModel;

namespace Inspector.Pages
{
    public class MaterialLiabilityVM : ViewModelBase // выдача
    {
        public ObservableCollection<Выдача> Выдачи { get; }
        public ObservableCollection<Сотрудник> Сотрудники { get; }
        public ObservableCollection<Техника> Техники { get; }
        public ObservableCollection<Кабинет> Кабинеты { get; }
        public ObservableCollection<Списание> Списания { get; }


        private Выдача _editableEquipment;
        private ViewMode _mode;
        private Выдача _selectedEquipment;
        private Техника _editableTechnika;
        private Сотрудник _выбранныйсотрудник;
        private string _поискТехники;
        private string _поискПараметры;
        private bool еслиЭксплуатация;
        private Кабинет _выбранныйкабинет;

        public Выдача РедактируемаяВыдача { get => _editableEquipment; set => Set(ref _editableEquipment, value); }
        public Выдача SelectedEquipment { get => _selectedEquipment; set => Set(ref _selectedEquipment, value); }
        public Сотрудник ВыбранныйСотрудник { get => _выбранныйсотрудник; set => Set(ref _выбранныйсотрудник, value); }
        public Кабинет ВыбранныйКабинет { get => _выбранныйкабинет; set => Set(ref _выбранныйкабинет, value); }
        public Техника EditableTechnika { get => _editableTechnika; set => Set(ref _editableTechnika, value); }
        public string ПоискТехники { get => _поискТехники; set => Set(ref _поискТехники, value); }
        public string ПоискПараметры { get => _поискПараметры; set => Set(ref _поискПараметры, value); }
        public bool ЕслиЭксплуатация { get => еслиЭксплуатация; set => Set(ref еслиЭксплуатация, value); }


        /// <summary>Режим просмотра.</summary>
        public ViewMode Mode { get => _mode; set => Set(ref _mode, value); }

        public MaterialLiabilityVM()
        {
            try
            {
                Кабинеты = DB.Кабинеты;
                Выдачи = DB.Выдачи;
                Сотрудники = DB.Сотрудники;
                Техники = DB.Техники;
                Списания = DB.Списания;
            }
            catch
            {

            }
        }
    }
}
