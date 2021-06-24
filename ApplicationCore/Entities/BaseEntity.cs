using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

        public BaseEntity()
        {
            CreatedOnUtc = DateTime.UtcNow;
        }
    }
    #region enum
    public enum ConnectionEnum
    {
        OffLine = 0,
        Online = 1
    }

    public enum StatusEnum
    {
        Disable = 0,
        Enable = 1
    }

    public enum LogEnum
    {
        Failure = 0,
        Success = 1
    }

    public enum JobStatusEnum
    {
        [Description("Message pending send queue")] Pending = 0,
        [Description("Message sent queue success")] Accepted = 1,
        [Description("Client received message")] InProgress = 2,
        [Description("Client print completed")] Completed = 3,
        [Description("Error")] Error = 4
    }

    public enum ComputerErrorEnum
    {
        [Description("Not found this computer")] ComputerNotFound = 1001,
        [Description("Not found this printer")] PrinterNotFound = 1002,
        [Description("Printer is dupplicate")] PrinterDupplicate = 1003
    }
    #endregion
}
