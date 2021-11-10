using System;
using UserNotification.Shared;
using UserNotification.Shared.Entities;
using UserNotification.Shared.Interfaces;

namespace UserNotification.Domain.Commands
{
    public sealed class EmailUsersCommand : ICommand
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public NotificationTypeEnum Type { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public string BarCode { get; set; }
        public Nullable<DateTime> PaymentDate { get; set; }
        public NotificationNotifyEnum Notify { get; set; }
    }
}
