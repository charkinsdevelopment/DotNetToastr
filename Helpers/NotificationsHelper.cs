using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Helpers
{
    public static class NotificationsHelper
    {
        public static string RenderNotifications()
        {
            if (HttpContext.Current.Session["Notifications"] == null) return null;
            var jsBody = new StringBuilder();
            var notifications = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Notification>>(HttpContext.Current.Session["Notifications"].ToString());

            jsBody.AppendLine("<script>");
            foreach(var note in notifications)
            {
                jsBody.AppendLine($"toastr.{note.NotificationType.ToString().ToLower()}('{note.Message}', '{note.Title}');");
            }
            jsBody.AppendLine("</script>");
            
            Clear();
            return jsBody.ToString();
        }

        /// <summary>
        /// Clears session["Notifications"] object
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Session["Notifications"] = null;
        }

        public static void AddNotification(Notification notification)
        {
            var note = new Notification()
            {
                Message = notification.Message,
                NotificationType = notification.NotificationType,
                Title = notification.Title
            };
            var notifications = new List<Notification>();

            if (HttpContext.Current.Session["Notifications"] != null) {
                notifications = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Notification>>(HttpContext.Current.Session["Notifications"].ToString());
            }
            notifications.Add(note);

            HttpContext.Current.Session["Notifications"] = JsonConvert.SerializeObject(notifications);
        }
    }


    public enum NotificationType
    {
        Success,
        Info,
        Warning,
        Error
    }

    public class Notification
    {
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Title { get; set; }
    }
}
