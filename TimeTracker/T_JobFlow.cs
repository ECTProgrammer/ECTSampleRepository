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
    
    public partial class T_JobFlow
    {
        public T_JobFlow()
        {
            this.M_JobTypeFlows = new HashSet<T_JobTypeFlow>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public string Acronym { get; set; }
        public int Position { get; set; }
    
        public virtual ICollection<T_JobTypeFlow> M_JobTypeFlows { get; set; }
    }
}
