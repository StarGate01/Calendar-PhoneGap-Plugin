using System;
using System.Collections.Generic;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;

public class Calendar : BaseCommand
{

    public class CreateEventParams
    {
        public string title { get; set; }
        public string location { get; set; }
        public string notes { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string options { get; set; }
    }

    public static class Core
    {
        public static PluginResult createEventProxy(string options)
        {
            string[] optValues = JsonHelper.Deserialize<string[]>(options);
            CreateEventParams optObj = JsonHelper.Deserialize<CreateEventParams>(optValues[0]);
            return createEvent(optObj.title, optObj.location, optObj.notes, UnixTimeStampToDateTime(Convert.ToDouble(optObj.startTime)), UnixTimeStampToDateTime(Convert.ToDouble(optObj.endTime)));
        }

        public static PluginResult createEvent(string title, string location, string notes, DateTime startTime, DateTime endTime)
        {
            SaveAppointmentTask saveAppointmentTask = new SaveAppointmentTask();
            saveAppointmentTask.StartTime = startTime;
            saveAppointmentTask.EndTime = endTime;
            saveAppointmentTask.Subject = title;
            saveAppointmentTask.Location = location;
            saveAppointmentTask.Details = notes;

            saveAppointmentTask.IsAllDayEvent = false;
            saveAppointmentTask.Reminder = Reminder.FifteenMinutes;

            saveAppointmentTask.AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.Busy;
            saveAppointmentTask.Show();
            return new PluginResult(PluginResult.Status.OK, "");
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStampMillis)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStampMillis).ToLocalTime();
            return dtDateTime;
        }
    }

    public void createEventWithOptions(string options)
    {
        DispatchCommandResult(Core.createEventProxy(options));
    }

    public void createEventInteractively(string options)
    {
        DispatchCommandResult(Core.createEventProxy(options));
    }

    //public void listEventsInRange(string options)
    //{
    //    PluginResult result = new PluginResult(PluginResult.Status.OK, "");
    //    DispatchCommandResult(result);
    //}

    //public void findEventWithOptions(string options)
    //{
    //    PluginResult result = new PluginResult(PluginResult.Status.OK, "");
    //    DispatchCommandResult(result);
    //}

}
