using ELearning.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.Entities.TeachingCourse
{
    public class FilesInCourseItemContent:IEntityBase
    {
        public Guid Id { get; set; }

        public CourseItemContent CourseItemContent { get; set; }
        public BusinessFile File { get; set; }

        public FilesInCourseItemContent()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
