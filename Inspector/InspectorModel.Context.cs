﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inspector
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbMalukovEntities : DbContext
    {
        public dbMalukovEntities()
            : base("name=dbMalukovEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Выдача> Выдача { get; set; }
        public virtual DbSet<Должность> Должность { get; set; }
        public virtual DbSet<Кабинет> Кабинет { get; set; }
        public virtual DbSet<Подразделение> Подразделение { get; set; }
        public virtual DbSet<Сотрудник> Сотрудник { get; set; }
        public virtual DbSet<Техника> Техника { get; set; }
        public virtual DbSet<Списание> Списание { get; set; }
        public virtual DbSet<Security> Security { get; set; }
    }
}