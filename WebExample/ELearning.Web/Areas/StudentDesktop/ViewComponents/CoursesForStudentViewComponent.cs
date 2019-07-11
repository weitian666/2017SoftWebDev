using ELearning.DataAccess;
using ELearning.Entities.Organization;
using ELearning.Entities.TeachingCourse;
using ELearning.UserAndRole;
using ELearning.ViewModels.TeachingCourse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Web.Areas.StudentDesktop.ViewComponents
{
    public class CoursesForStudentViewComponent:ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IEntityRepository<Employee> _employeeRepository;
        private readonly IEntityRepository<Student> _studentRepository;
        private readonly IEntityRepository<Department> _departmentRepository;
        private readonly IEntityRepository<GradeAndClass> _gradeAndClassRepository;

        private readonly IEntityRepository<CourseWithRoles> _courseWithRolesRepository;
        private readonly IEntityRepository<CourseWithUsers> _courseWithUsersRepository;
        private readonly IEntityRepository<Course> _boRepository;

        private readonly IAuthorizationService _authorizationService;

        private CourseVMService _boVMService;

        public CoursesForStudentViewComponent(
           UserManager<ApplicationUser> userManager,
           RoleManager<ApplicationRole> roleManager,

           IEntityRepository<Employee> employeeRepository,
           IEntityRepository<Student> studentRepository,
           IEntityRepository<Department> departmentRepository,
           IEntityRepository<GradeAndClass> gradeAndClassRepository,
           IEntityRepository<CourseWithRoles> courseWithRolesRepository,
           IEntityRepository<CourseWithUsers> courseWithUsersRepository,
           IEntityRepository<Course> repository,
           IAuthorizationService authorizationService
           )
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _employeeRepository = employeeRepository;
            _studentRepository = studentRepository;
            _departmentRepository = departmentRepository;
            _gradeAndClassRepository = gradeAndClassRepository;
            _courseWithRolesRepository = courseWithRolesRepository;
            _courseWithUsersRepository = courseWithUsersRepository;
            _boRepository = repository;

            _authorizationService = authorizationService;

            _boVMService = new CourseVMService(_userManager, _roleManager, _employeeRepository, _studentRepository, _departmentRepository, _gradeAndClassRepository, _courseWithRolesRepository, _courseWithUsersRepository, _boRepository);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var boVMCollection = await _boVMService.GetboVMCollectionAsyn();
            return View("Default",boVMCollection);
        }
    }
}
