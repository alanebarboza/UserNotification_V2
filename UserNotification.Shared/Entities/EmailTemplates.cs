using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserNotification.Shared.Entities
{
    public static class EmailTemplates
    {
        public static string Default() => "This is a Default Email Template.";

        public static string UsersBills(string description, Nullable<DateTime> paymentDate, string barCode, decimal value)
        {
            return $"This is a reminder of '{description}' that expires at {paymentDate?.ToString("dd/MM/yyyy HH:mm:ss")} \r\n" +
                   $"BarCode: {barCode} \r\n" +
                   $"Value: {value}";
        }

        public static string UsersNotificationOnly(string description, Nullable<DateTime> paymentDate) 
            =>  $"This is a reminder of '{description}' at {paymentDate?.ToString("dd/MM/yyyy HH:mm:ss")} \r\n";
    }
}
