﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace User.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Data_ProjectManageEntities1 : DbContext
    {
        public Data_ProjectManageEntities1()
            : base("name=Data_ProjectManageEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MANAGER_TASK> MANAGER_TASK { get; set; }
        public virtual DbSet<PROJECT> PROJECT { get; set; }
        public virtual DbSet<SCREENSHOT> SCREENSHOT { get; set; }
        public virtual DbSet<TASK> TASK { get; set; }
        public virtual DbSet<WORK> WORK { get; set; }
        public virtual DbSet<ADMIN> ADMIN { get; set; }
        public virtual DbSet<ROLE> ROLE { get; set; }
        public virtual DbSet<USER> USER { get; set; }
    }
}