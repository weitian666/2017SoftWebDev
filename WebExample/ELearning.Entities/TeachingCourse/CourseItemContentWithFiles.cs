using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.Entities.TeachingCourse
{
    public class CourseItemContentWithFiles : IEntityBase
    {
        public Guid Id { get; set; }
        public Guid BusinessFileId { get; set; }
        public Guid BusinessImageId { get; set; }
        public Guid BusinessVideoId { get; set; }
        public int OrderNumber { get; set; }

        public CourseItemContentWithFiles()
        {
            this.Id = Guid.NewGuid();

        }

    }
}
