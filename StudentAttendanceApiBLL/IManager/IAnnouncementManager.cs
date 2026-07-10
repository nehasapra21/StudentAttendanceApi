using StudentAttendanceApi.FCM;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IAnnouncementManager
    {
        Task<Announcement> SaveAnnouncement(Announcement announcement);
        Task<List<Announcement>> GetAnnouncement();
    }
}
