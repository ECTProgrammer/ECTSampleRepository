﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeTracker
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TimeTrackerEntities : DbContext
    {
        public TimeTrackerEntities()
            : base("name=TimeTrackerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<T_Department> T_Departments { get; set; }
        public DbSet<T_JobTracker> T_JobTrackers { get; set; }
        public DbSet<T_JobType> T_JobTypes { get; set; }
        public DbSet<T_Position> T_Positions { get; set; }
        public DbSet<T_User> T_Users { get; set; }
        public DbSet<T_Time> T_Times { get; set; }
        public DbSet<T_Module> T_Modules { get; set; }
        public DbSet<T_PositionModuleAccess> T_PositionModuleAccesses { get; set; }
    }
}
