using Newtonsoft.Json.Linq;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IClassManager
    {
        Task<Class> SaveClass(Class classval);
        Task<Class> UpdateEndClassTime(Class cls);
        Task<Dictionary<int, int>> GetActiveClass();
        Task<Class> CancelClass(Class cls);
        Task<ClassCancelTeacher> CancelClassByTeacher(ClassCancelTeacher cls);
        Task<string> GetClassCurrentStatus(int centerId, int teacherId);
        Task<Class> DeleteClassByTeacherId(int teacherId);
        Task<ClassLiveDetailDto> GetLiveClassDetail(int teacherId);
        Task<Class> UpdateClassSubStatus(UpdateClassSubStatusDto cls);
    }

}
