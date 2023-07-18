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
        #endregion
    }
}
