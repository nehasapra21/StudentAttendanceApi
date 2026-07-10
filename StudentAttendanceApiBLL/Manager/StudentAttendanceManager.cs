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
        public async Task<int> SaveAtuomaticStudentAttendance(StudentAttendance studentAttendance)
        {
            _logger.LogInformation($"UserManager : Bll : SaveStudentAttendance : Started");

            return await _studentAttendanceRepository.SaveAtuomaticStudentAttendance(studentAttendance);
        }


        public async Task<int> SaveManualStudentAttendance(StudentAttendance studentAttendance)
        {
            _logger.LogInformation($"UserManager : Bll : SaveStudentAttendance : Started");

            return await _studentAttendanceRepository.SaveManualStudentAttendance(studentAttendance);
        }
        public async Task<List<StudentAttendanceDetailDto>> GetAllStudentWihAvgAttendance(int centerId)
        {
            _logger.LogInformation($"UserManager : Bll : SaveStudentAttendance : Started");
            List<StudentAttendanceDetailDto> studentAttendanceDetailDtos = null;
            List<Student> students= await _studentAttendanceRepository.GetAllStudentWihAvgAttendance(centerId);
            if (students != null)
            {
                studentAttendanceDetailDtos = new List<StudentAttendanceDetailDto>();
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
            List<StudentAttendanceDetailDto> studentAttendanceDetailDtos = null;
            List<Student> students = await _studentAttendanceRepository.GetAllStudentAttendancStatus(centerId, classDate);
            if (students != null)
            {
                studentAttendanceDetailDtos = new List<StudentAttendanceDetailDto>();
                foreach (var item in students)
                {
                    StudentAttendanceDetailDto studentAttendanceDetailDto = new StudentAttendanceDetailDto();
                    studentAttendanceDetailDto = StudentAttendanceConvertor.ConvertStudentToStudentAttendanceDeatilDto(item);
                    studentAttendanceDetailDtos.Add(studentAttendanceDetailDto);
                }
            }
            return studentAttendanceDetailDtos;
        }


        public async Task<List<StudentAttendanceMonthDetailDto>> GetAllStudentAttendancByMonth(int centerId,int studentId, int month,int year)
        {
            _logger.LogInformation($"UserManager : Bll : SaveStudentAttendance : Started");
            List<StudentAttendanceMonthDetailDto> studentAttendanceDetailDtos = null;
            List<Student> students = await _studentAttendanceRepository.GetAllStudentAttendancByMonth(centerId,studentId, month, year);
            if (students != null)
            {
                studentAttendanceDetailDtos = new List<StudentAttendanceMonthDetailDto>();
                foreach (var item in students)
                {
                    StudentAttendanceMonthDetailDto studentAttendanceDetailDto = new StudentAttendanceMonthDetailDto();
                    studentAttendanceDetailDto = StudentAttendanceConvertor.ConvertStudentToStudentAttendanceMonthDeatilDto(item);
                    studentAttendanceDetailDtos.Add(studentAttendanceDetailDto);
                }
            }
            return studentAttendanceDetailDtos;
        }

        public async Task<List<StudentAbsentClassDto>> GetAllAbsentAttendance(int centerId)
        {
            _logger.LogInformation($"UserManager : Bll : SaveStudentAttendance : Started");
            List<StudentAbsentClassDto> studentAttendanceDetailDtos = null;
            List<Student> students = await _studentAttendanceRepository.GetAllAbsentAttendance(centerId);
            if (students != null)
            {
                studentAttendanceDetailDtos=new List<StudentAbsentClassDto>();
                foreach (var item in students)
                {
                    StudentAbsentClassDto studentAttendanceDetailDto = new StudentAbsentClassDto();
                    studentAttendanceDetailDto = StudentAttendanceConvertor.ConvertStudentToStudentAbsentClassDtoDto(item);
                    studentAttendanceDetailDtos.Add(studentAttendanceDetailDto);
                }
            }
            return studentAttendanceDetailDtos;
        }
        #endregion
    }
}
