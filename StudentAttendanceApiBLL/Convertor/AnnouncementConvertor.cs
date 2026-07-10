using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class AnnouncementConvertor
    {
        public static Announcement ConvertAnnouncementtoToAnnouncement(AnnouncementDto announcementDto)
        {
            Announcement announcement = new Announcement();
            announcement.Id = announcementDto.Id;
            announcement.Title = announcementDto.Title;
            announcement.Image = announcementDto.Image;
            announcement.Description = announcementDto.Image;
            announcement.Description = announcementDto.Description;
            announcement.CreatedOn = announcementDto.CreatedOn;
            announcement.CreatedBy = announcementDto.CreatedBy;

            return announcement;

        }

    }
}
