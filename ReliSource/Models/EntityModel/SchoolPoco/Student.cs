//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReliSource.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        public long StudentID { get; set; }
        public int SchoolClassID { get; set; }
        public int SchoolID { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public System.DateTime JoinedDate { get; set; }
        public System.DateTime LeaveDate { get; set; }
        public double CurrentGrade { get; set; }
    
        public virtual School School { get; set; }
        public virtual SchoolClass SchoolClass { get; set; }
    }
}
