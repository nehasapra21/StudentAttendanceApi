using Microsoft.Extensions.Options;
using StudentAttendanceApi.FCM;
using static StudentAttendanceApi.FCM.NotificationModel.GoogleNotification;
using static StudentAttendanceApi.FCM.NotificationModel;
using System.Net.Http.Headers;
using System.Runtime;
using CorePush.Google;

namespace StudentAttendanceApi.Services
{
    public interface INotificationService
    {
        Task<ResponseModel> SendNotification(NotificationModel notificationModel);
    }

    public class NotificationService : INotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;

        public NotificationService(IOptions<FcmNotificationSetting> settings)
        {
            _fcmNotificationSetting = settings.Value;
        }

        public async Task<ResponseModel> SendNotification(NotificationModel notificationModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {

                /* FCM Sender (Android Device) */
                FcmSettings settings = new FcmSettings()
                {
                    SenderId = _fcmNotificationSetting.SenderId,
                    ServerKey = _fcmNotificationSetting.ServerKey
                };
                HttpClient httpClient = new HttpClient();

                List<string> registrationTokens = new List<string>
        {
            "token1",
            "token2",
            // Add more tokens here
        };
                string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                string deviceToken = notificationModel.DeviceId;

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                httpClient.DefaultRequestHeaders.Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                DataPayload dataPayload = new DataPayload();
                dataPayload.Title = notificationModel.Title;
                dataPayload.Body = Convert.ToString(notificationModel.Body);

                GoogleNotification notification = new GoogleNotification();
                notification.Data = dataPayload;
                notification.Notification = dataPayload;

                var fcm = new FcmSender(settings, httpClient);
                List<string> responseVal = new List<string>();
                try
                {

                    if (notificationModel.ListOfDeviceIds.Count > 0)
                    {
                        var i = 0;
                        foreach (var item in notificationModel.ListOfDeviceIds)
                        {
                            i++;
                            var fcmSendResponse = await fcm.SendAsync(item, notification);
                            if (fcmSendResponse.IsSuccess())
                            {
                                responseVal.Add("Notification sent successfully");
                            }
                            else
                            {
                                responseVal.Add(fcmSendResponse.Results[0].Error);
                            }
                        }

                    }
                    //var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);


                    if (responseVal.Count > 0)
                    {
                        foreach (var item in responseVal)
                        {
                            response.Message = item;
                            return response;
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //else
                //{
                //    /* Code here for APN Sender (iOS Device) */
                //    //var apn = new ApnSender(apnSettings, httpClient);
                //    //await apn.SendAsync(notification, deviceToken);
                //}
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
        }
    }
}
