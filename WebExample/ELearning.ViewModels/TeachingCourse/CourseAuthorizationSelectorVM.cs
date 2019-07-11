using ELearning.Entities.Tools.AdditionalItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.ViewModels.TeachingCourse
{
    public class CourseAuthorizationSelectorVM
    {
        public Guid CourseID { get; set; }
        public string CourseName { get; set; }
        public string KeyWord { get; set; }

        public AuthorizationTypeEnum AuthorizationType { get; set; }
        public List<AuthorizationTypeForCourseVM> AuthorizationTypeForCourseCollection { get; set; }

        public List<CourseAuthorizationVM> BeAuthorizationedItemCollection { get; set; }
        public List<CourseAuthorizationVM> ToBeAuthorizationedItemCollection { get; set; }

        public CourseAuthorizationSelectorVM()
        {
            BeAuthorizationedItemCollection = new List<CourseAuthorizationVM>();
            ToBeAuthorizationedItemCollection = new List<CourseAuthorizationVM>();
        }
    }
}
