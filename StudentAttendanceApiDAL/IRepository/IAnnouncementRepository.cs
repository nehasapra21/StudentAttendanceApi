
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IAnnouncementRepository
    {
        Task<Announcement> SaveAnnouncement(Announcement announcement);
        Task<List<Announcement>> GetAnnouncement();
    }
}
