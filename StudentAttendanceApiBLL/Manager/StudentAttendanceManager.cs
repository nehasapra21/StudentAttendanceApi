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
    public class StudentAttendanceManager : IStudentAttendanceManager
    {
        #region | Properties |

        private readonly ILogger _logger;
        private readonly IStudentAttendanceRepository _studentAttendanceRepository;

        #endregion

        #region | Controller |

        public StudentAttendanceManager(IStudentAttendanceRepository studentAttendanceRepository,
                                ILogger<StudentAttendanceManager> logger)
        {
            _studentAttendanceRepository = studentAttendanceRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

    
        public async Task<int> SaveStudentAttendance(StudentAttendance studentAttendance)
        {
            _logger.LogInformation($"UserManager : Bll : SaveStudentAttendance : Started");

            return await _studentAttendanceRepository.SaveStudentAttendance(studentAttendance);
        }

        public async Task<List<StudentAttendanceDetailDto>> GetAllStudentWihAvgAttendance(int centerId)
        {
            _logger.LogInformation($"UserManager : Bll : SaveStudentAttendance : Started");
            List<StudentAttendanceDetailDto> studentAttendanceDetailDtos = new List<StudentAttendanceDetailDto>();
            List<Student> students= await _studentAttendanceRepository.GetAllStudentWihAvgAttendance(centerId);
            if (students != null)
            {
                foreach (var item in students)
                {
                    StudentAttendanceDetailDto studentAttendanceDetailDto = new StudentAttendanceDetailDto();
                    studentAttendanceDetailDto = StudentAttendanceConvertor.ConvertStudentToStudentAttendanceDeatilDto(item);
                    studentAttendanceDetailDtos.Add(studentAttendanceDetailDto);
                }
            }
            return studentAttendanceDetailDtos;
        }

        public async Task<List<StudentAttendanceDetailDto>> GetAllStudentAttendancStatus(int centerId, string classDate)
        {
            _logger.LogInformation($"UserManager : Bll : SaveStudentAttendance : Started");
            List<StudentAttendanceDetailDto> studentAttendanceDetailDtos = new List<StudentAttendanceDetailDto>();
            List<Student> students = await _studentAttendanceRepository.GetAllStudentAttendancStatus(centerId, classDate);
            if (students != null)
            {
                foreach (var item in students)
                {
                    StudentAttendanceDetailDto studentAttendanceDetailDto = new StudentAttendanceDetailDto();
                    studentAttendanceDetailDto = StudentAttendanceConvertor.ConvertStudentToStudentAttendanceDeatilDto(item);
                    studentAttendanceDetailDtos.Add(studentAttendanceDetailDto);
                }
            }
            return studentAttendanceDetailDtos;
        }


        public async Task<List<StudentAttendanceDetailDto>> GetAllStudentAttendancByMonth(int centerId,int studentId, int month)
        {
            _logger.LogInformation($"UserManager : Bll : SaveStudentAttendance : Started");
            List<StudentAttendanceDetailDto> studentAttendanceDetailDtos = new List<StudentAttendanceDetailDto>();
            List<Student> students = await _studentAttendanceRepository.GetAllStudentAttendancByMonth(centerId,studentId, month);
            if (students != null)
            {
                foreach (var item in students)
                {
                    StudentAttendanceDetailDto studentAttendanceDetailDto = new StudentAttendanceDetailDto();
                    studentAttendanceDetailDto = StudentAttendanceConvertor.ConvertStudentToStudentAttendanceDeatilDto(item);
                    studentAttendanceDetailDtos.Add(studentAttendanceDetailDto);
                }
            }
            return studentAttendanceDetailDtos;
        }
        #endregion
    }
}
