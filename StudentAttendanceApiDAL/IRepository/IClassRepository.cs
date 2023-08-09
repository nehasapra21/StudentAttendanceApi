using Newtonsoft.Json.Linq;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IClassRepository
    {
        Task<Class> SaveClass(Class classval);
        Task<Class> UpdateEndClassTime(Class cls);
        Task<Dictionary<int, int>> GetActiveClass();
        Task<Class> CancelClass(Class cls);
        Task<ClassCancelTeacher> CancelClassByTeacher(ClassCancelTeacher cls);
        Task<string> GetClassCurrentStatus(int centerId, int teacherId);
        Task<Class> DeleteClassByTeacherId(int teacherId);
        Task<Class> GetLiveClassDetail(int teacherId);
        Task<Class> UpdateClassSubStatus(int classId);

    }
}
