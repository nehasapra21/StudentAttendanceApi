using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentAttendanceApi.FCM;
using StudentAttendanceApiBLL.Dto;
using StudentAttendanceApiBLL.NotificationData;
using StudentAttendanceApiDAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudentAttendanceApiBLL.NotificationData1
{
    public class SendNotificationClass
    {
        private readonly IUserRepository _userRepository;

        public SendNotificationClass(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<NotificationModel> SendNotificationType(NotificationDto notificationDto, bool storeStatus)
        {
            bool issend = false;
            dynamic JsonObject = new JObject();
            JsonObject.Data = new JArray() as dynamic;
            dynamic object1 = new JObject();
            object1 = new JObject();
            List<string> superAdminToken = new List<string>();
            NotificationModel model = new NotificationModel();
            if (notificationDto.SuperAdminStatus)
            {
                superAdminToken = await _userRepository.GetAllSuperAdminToken();
                if (superAdminToken != null && superAdminToken.Count > 0)
                {
                    foreach (var item in superAdminToken)
                    {
                        model.ListOfDeviceIds.Add(item);// = model.DeviceId + item + ",";
                    }
                }

                List<string> registrationTokens = new List<string>();

            }

            if (notificationDto.Type == 1)
            {
                if (notificationDto.RegionaladminStatus)
                {
                    // JsonObject val = data;
                    //model.DeviceId = model.DeviceId + "," + notificationDto.RegionalToken + ",";
                    model.ListOfDeviceIds.Add(notificationDto.RegionalToken);
                }

                if (notificationDto.TeacherStatus)
                {
                    // JsonObject val = data;
                    //model.DeviceId = model.DeviceId + "," + notificationDto.TeacherToken;
                    model.ListOfDeviceIds.Add(notificationDto.TeacherToken);
                }
                //model.DeviceId = "[" + model.DeviceId + "]";
                //model.DeviceId =["cyFuSCbKRB6CsfcdWNt3_w:APA91bHU_2mlXUviESGwi99ONihhOotVY8Blhmr32zhQMZEhaTZaHnjwE09MLZ1q27Lv681WR68Htij5D60b0hOmxMCWwO1RrTVVTFa7XQQujA0L1nbW2OsTg3Zko0WZVwXRwL3W0bg_", "eDyN4W_gQh2-ZEMFbSR-RA:APA91bFVhWkPwMQfXPCj1iEWrV9kjmmtXDX4Cu4TbjM7iqIPbrzcHqj-WiWuNvSvGosvg4-AflOGB1wPQfZa2PdpX8_5MrWHb4adRWpTJ0tkH0fgrPLtJ8tEfY7u0WWhdBQL0uhjhE_z"]
                //if (!string.IsNullOrEmpty(model.DeviceId)) { model.DeviceId.TrimEnd(','); }

                // Create body
                object1.Name = notificationDto.Name;
                object1.CenterId = notificationDto.CenterId;
                object1.StartingDate = notificationDto.StartingDate;
                object1.EndingDate = notificationDto.EndingDate;

                model.Title = "Class cancel by teacher";
                model.Body = "Class "+ object1.Name + " cancel from " + object1.StartingDate + " to " + object1.EndingDate;
            }

            return model;
        }

        //public async Task<bool> SendNotificationData(NotificationModel notificationModel)
        //{
        //    bool isNotificationSend = false;
        //    try
        //    {
        //        //notificationModel.DeviceId = await _userRepository.GetUserDeviceByUserId(notificationModel.userId);

        //        ResponseModel result = await _notificationService.SendNotification(notificationModel);
        //        if (result != null && result.IsSuccess)
        //        {
        //            isNotificationSend = true;
        //            //return StatusCode(StatusCodes.Status200OK, new
        //            //{
        //            //    status = true,
        //            //    message = result.Message,
        //            //    code = StatusCodes.Status200OK
        //            //});
        //        }
        //        else
        //        {
        //            isNotificationSend = false;
        //            //return StatusCode(StatusCodes.Status404NotFound, new
        //            //{
        //            //    status = false,
        //            //    error = result.Message,
        //            //    code = StatusCodes.Status404NotFound
        //            //});
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.LogError(ex, $"UserController : SaveSuperAdmin ", ex);
        //        //return StatusCode(StatusCodes.Status501NotImplemented, ex.InnerException.Message);
        //    }
        //    return isNotificationSend;
        //}
    }
}
