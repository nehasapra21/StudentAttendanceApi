using Microsoft.Extensions.Logging;
using StudentAttendanceApi.FCM;
using StudentAttendanceApiBLL.Dto;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiBLL.NotificationData1;
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
    public class AnnouncementManager : IAnnouncementManager
    {
        #region | Properties |

        private readonly ILogger<AnnouncementManager> _logger;
        private readonly IAnnouncementRepository _announcementRepository;
       
        #endregion

        #region | Controller |

        public AnnouncementManager(IAnnouncementRepository announcementRepository,
                                ILogger<AnnouncementManager> logger)
        {
            _announcementRepository = announcementRepository;
            _logger = logger;
        }

        #endregion

        #region | Public Methods |

    
        public async Task<Announcement> SaveAnnouncement(Announcement announcement)
        {
            _logger.LogInformation($"UserManager : Bll : SaveAnnouncement : Started");
          
            return await _announcementRepository.SaveAnnouncement(announcement);
        }

        public async Task<List<Announcement>> GetAnnouncement()
        {
            _logger.LogInformation($"DitrictManager : Bll : GetAnnouncement : Started");
            var announcement = await _announcementRepository.GetAnnouncement();
            _logger.LogInformation($"DitrictManager : Bll : GetAnnouncement : End");
            return announcement;
        }

        #endregion
    }
}
