using ELearning.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.Entities.TeachingCourse
{
    public class ViedosInCourseItemContent:IEntityBase
    {
        public Guid Id { get; set; }

        public CourseItemContent CourseItemContent { get; set; }
        public BusinessVideo Video { get; set; }

        public ViedosInCourseItemContent()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
