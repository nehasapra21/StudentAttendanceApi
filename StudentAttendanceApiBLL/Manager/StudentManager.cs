using Microsoft.Extensions.Logging;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Repository;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.Manager
{
    public class StudentManager : IStudentManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IStudentRepository _studentRepository;

        #endregion

        #region | Controller |

        public StudentManager(IStudentRepository studentRepository,
                                ILogger<StudentManager> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

    
        public async Task<Student> SaveStudent(Student student)
        {
            _logger.LogInformation($"UserManager : Bll : SaveSuperAdmin : Started");

            return await _studentRepository.SaveStudent(student);
        }

        public async Task<StudentDetailDto> GetStudentById(int id)
        {
            _logger.LogInformation($"UserManager : Bll : GetUser : Started");
            Student student= await _studentRepository.GetStudentById(id);
            StudentDetailDto studentDetailDto = null;
            if(student!=null)
            {
                studentDetailDto=new StudentDetailDto();
                studentDetailDto = StudentConvertor.ConvertStudentDetailDtoToStudentDto(student);
            }
            return studentDetailDto;
        }

        public async Task<Student> GetStudentByCenterId(int centerId)
        {
            _logger.LogInformation($"UserManager : Bll : GetUser : Started");

            return await _studentRepository.GetStudentByCenterId(centerId);
        }

        public async Task<Student> UpdateStudentActiveOrInactive(int id, int status)
        {
            _logger.LogInformation($"UserManager : Bll : UpdateStudentActiveOrInactive : Started");

            return await _studentRepository.UpdateStudentActiveOrInactive(id,status);
        }

  

        public async Task<StudentPresentClassDto> GetTotalStudentPresent()
        {
            _logger.LogInformation($"UserManager : Bll : GetTotalStudentPresent : Started");
            Dictionary<int, int> listOfStudents=await _studentRepository.GetTotalStudentPresent();
            Dictionary<int, int> listOfClasses = await _studentRepository.GetActiveClass();
            Dictionary<int, int> upcomingAndCompletedCount = await _studentRepository.GetTotalUpComingAndCompletedClass();
            int cancelClassCount= await _studentRepository.GetCancelClassCount();
            StudentPresentClassDto studentPresentClassDto = new StudentPresentClassDto();
            studentPresentClassDto.TotalStudents = listOfStudents.ToList()[0].Value;
            studentPresentClassDto.TotalClasses= listOfClasses.ToList()[0].Value;
            studentPresentClassDto.PresentStudents = listOfStudents.ToList()[0].Key;
            studentPresentClassDto.TotalActiveClasses = listOfClasses.ToList()[0].Key;

            studentPresentClassDto.CompletedClassCount = upcomingAndCompletedCount.ToList()[0].Key;
            studentPresentClassDto.UpComingClassCount = upcomingAndCompletedCount.ToList()[0].Value;
            studentPresentClassDto.CancelClassCount = cancelClassCount;
          //  studentPresentClassDto.time= TimeZoneInfo.Local;
            return studentPresentClassDto;
        }

        #endregion
    }
}
