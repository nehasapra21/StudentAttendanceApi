using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StudentAttendanceApi.FCM;
using StudentAttendanceApiDAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.NotificationData
{
    public class SendNotification1
    {
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;
        public SendNotification1(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        //public async Task<bool> SendNotificationType(int type, bool superAdminStatus, bool RegionaladminStatus, JObject data, bool storeStatus)
        //{
        //    bool issend = false;
            
        //        //dynamic JsonObject = new JObject();
        //        //JsonObject.Data = new JArray() as dynamic;
        //        //JsonObject.Data = data;
        //        //call when cancel by teacher when type=1
        //        //type = 1;//kis type ki jani h
        //        //superAdminStatus = true;//
        //        //RegionaladminStatus = true;
        //        //data = "helo";//
        //        //storeStatus = false;
        //        List<string> superAdminTokens = new List<string>();
        //    NotificationModel model = new NotificationModel();
        //    if (superAdminStatus)
        //    {
        //        superAdminTokens = await _userRepository.GetAllSuperAdminToken();
        //        if (superAdminTokens != null && superAdminTokens.Count > 0)
        //        {
        //            foreach (var item in superAdminTokens)
        //            {
        //                model.DeviceId = model.DeviceId + item + ",";
        //            }
        //        }
        //    }

        //    model.DeviceId.TrimEnd(',');
        //    //int centeid = data.centerid;
        //    //int regionaladminid = 1;//from center 
        //    //int techeradminid = 1;//from center 
        //    //int supeadminDeviceid =
        //    string token = "";//deviceid;

        //   // if (type == 1)//Class cancel by teacher 
        //   // {
        //        if (RegionaladminStatus)
        //        {
        //            //if (data != null)
        //            //{
        //            //    string tokenVal = await _userRepository.GetUserTokenByUserId(JsonObject.Data.center.AssignedTeachers);
        //            //    model.DeviceId = model.DeviceId + tokenVal + ",";
        //            //}

        //        }

        //        model.Title = "class cacle by teaher";
        //        model.Body = data;
        //        //model.DeviceId = token;

        //        issend = await SendNotificationData(model);

        //    //}
        //    return issend;
        //}

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
