//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class T_JobType
    {
        public T_JobType()
        {
            this.M_JobTrackers = new HashSet<T_JobTracker>();
            this.M_JobTrackerHistories = new HashSet<T_JobTrackerHistory>();
            this.M_JobTypeDepartments = new HashSet<T_JobTypeDepartment>();
            this.M_JobTypeFlows = new HashSet<T_JobTypeFlow>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<bool> RequiredJobId { get; set; }
        public Nullable<bool> ComputeTime { get; set; }
        public Nullable<int> Position { get; set; }
        public Nullable<bool> ShowInJobOverview { get; set; }
        public string Acronym { get; set; }
    
        public virtual ICollection<T_JobTracker> M_JobTrackers { get; set; }
        public virtual ICollection<T_JobTrackerHistory> M_JobTrackerHistories { get; set; }
        public virtual ICollection<T_JobTypeDepartment> M_JobTypeDepartments { get; set; }
        public virtual ICollection<T_JobTypeFlow> M_JobTypeFlows { get; set; }
    }
}
