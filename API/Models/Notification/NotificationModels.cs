using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Notification
{
    public class NotificationModels
    {
    }
    public class NotificationRequestModel
    {
        public string Content { get; set; }
        public bool IsChecked { get; set; }
        public int UserId { get; set; }
        public int Type { get; set; }
        public int TestId { get; set; }
    }
    public class GlobalNotificationRequestModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
    public class NotificationRequestCreateModel
    {
        public string Content { get; set; }
        public int TestId { get; set; }
    }

    public class NotificationResponseModel
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
        public string Content { get; set; }
        public bool IsChecked { get; set; }
        public int UserId { get; set; }
        public int Type { get; set; }
        public NotificationTestType TestType { get; set; }
    }
    public class GlobalNotificationResponseModel
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
    }
    public enum NotificationTestType
    {
        Mathematic = 1,
        Science = 2,
        History = 3,
        Geographic = 4,
        Social = 5,
        English = 6,
        AssTest = 7
    }
}
