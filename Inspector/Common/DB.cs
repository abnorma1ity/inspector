using Simplified;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspector
{
    class DB
    {
        public static readonly dbMalukovEntities Connection;

        public static ObservableCollection<Техника> Техники { get; }
        public static ObservableCollection<Выдача> Выдачи { get; }
        public static ObservableCollection<Сотрудник> Сотрудники { get; }
        public static ObservableCollection<Кабинет> Кабинеты { get; }
        public static ObservableCollection<Подразделение> Подразделения { get; }
        public static ObservableCollection<Должность> Должности { get; }
        public static ObservableCollection<Списание> Списания { get; }

        public static Сотрудник ВсеСотрудники { get; }
        public static Кабинет ВсеКабинеты { get; }
        static DB()
        {
            ВсеКабинеты = new Кабинет()
            {
                Название = "Все кабинеты"
            };
            ВсеСотрудники = new Сотрудник()
            {
                ФИО_сотр = "Все сотрудники"
            };
            if (ViewModelBase.IsInDesignModeStatic)
            {
                Техники = new ObservableCollection<Техника>()
                {
                    new Техника(){Код = 123,Модель = "Ryzen11"},
                    new Техника(){Код = 321,Модель = "Ryzen12"},
                };
                Выдачи = new ObservableCollection<Выдача>() {
                    new Выдача() {Код_техники = 123, Дата_выдачи = DateTime.Now },
                    new Выдача() {Код_техники = 321, Дата_выдачи = DateTime.Now },
                };
                Сотрудники = new ObservableCollection<Сотрудник>()
                {
                    new Сотрудник() {Дата_рождения = DateTime.Now, ФИО_сотр = "Тест Ф.И."},
                    new Сотрудник() {Дата_рождения = DateTime.Now, ФИО_сотр = "Тест И.О"},
                };
                //Сотрудники.Insert(0, ВсеСотрудники);

                Кабинеты = new ObservableCollection<Кабинет>()
                {
                    new Кабинет() {Название = "Тестовый", Номер_кабинета = 121 },
                    new Кабинет() {Название = "ТестовыйКабинет", Номер_кабинета = 221 },
                };
                Подразделения = new ObservableCollection<Подразделение>()
                {
                    new Подразделение() {Подразделение1 = "Тестовый", Код_подразделения = 45 },
                    new Подразделение() {Подразделение1 = "Тестовый1", Код_подразделения = 55 },
                };
                Должности = new ObservableCollection<Должность>()
                {
                    new Должность() {Должность1 = "Тестовая", Код_должности = 133 },
                    new Должность() {Должность1 = "Тестовая1", Код_должности = 233 },
                };
            }
            else
            {
                Connection = new dbMalukovEntities();
                Техники = new ObservableCollection<Техника>(Connection.Техника.ToList());
                Выдачи = new ObservableCollection<Выдача>(Connection.Выдача.ToList());
                Сотрудники = new ObservableCollection<Сотрудник>(Connection.Сотрудник.OrderBy(x => x.ФИО_сотр).ToList());
                //Сотрудники.Insert(0, ВсеСотрудники);

                Кабинеты = new ObservableCollection<Кабинет>(Connection.Кабинет.OrderBy(x => x.Название).ToList());
                Подразделения = new ObservableCollection<Подразделение>(Connection.Подразделение.OrderBy(x => x.Подразделение1).ToList());
                Должности = new ObservableCollection<Должность>(Connection.Должность.OrderBy(x => x.Должность1).ToList());
                Списания = new ObservableCollection<Списание>(Connection.Списание.ToList());
            }
        }
    }
}
