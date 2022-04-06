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
        public static dbMalukovEntities Connection;

        public static ObservableCollection<Техника> Техники { get; }
        public static ObservableCollection<Выдача> Выдачи { get; }
        public static ObservableCollection<Сотрудник> Сотрудники { get; }
        public static ObservableCollection<Кабинет> Кабинеты { get; }
        public static ObservableCollection<Подразделение> Подразделения { get; }
        public static ObservableCollection<Должность> Должности { get; }

        public static Сотрудник ВсеСотрудники { get; }

        static DB()
        {
            Connection = new dbMalukovEntities();
            Техники = new ObservableCollection<Техника>(Connection.Техника.ToList());
            Выдачи = new ObservableCollection<Выдача>(Connection.Выдача.ToList());
            Сотрудники = new ObservableCollection<Сотрудник>(Connection.Сотрудник.OrderBy(x => x.ФИО_сотр).ToList());
            ВсеСотрудники = new Сотрудник()
            {
                ФИО_сотр = "Все сотрудники"
            };
            //Сотрудники.Insert(0, ВсеСотрудники);

            Кабинеты = new ObservableCollection<Кабинет>(Connection.Кабинет.OrderBy(x => x.Название).ToList());
            Подразделения = new ObservableCollection<Подразделение>(Connection.Подразделение.OrderBy(x => x.Подразделение1).ToList());
            Должности = new ObservableCollection<Должность>(Connection.Должность.OrderBy(x => x.Должность1).ToList());
        }
    }
}
