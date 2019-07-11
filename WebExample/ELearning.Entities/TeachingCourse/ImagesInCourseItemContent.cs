using ELearning.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.Entities.TeachingCourse
{
    public class ImagesInCourseItemContent:IEntityBase
    {
        public Guid Id { get; set; }

        public CourseItemContent CourseItemContent { get; set; }
        public BusinessImage Image { get; set; }

        public ImagesInCourseItemContent()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
