using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentOrderService
{ 
    public class Subject
    {
        [Key]
        public Guid guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Core { get; set; }
        public bool IsScheduled { get; set; }

        public int SemesterOffered { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
}
    //public class StudentCollection
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public List<Subject> StudentCollectionProperty = new List<Subject>();
    //}

    public class Course
    {
      
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Subject> Subjects { get; set; }

    }
}