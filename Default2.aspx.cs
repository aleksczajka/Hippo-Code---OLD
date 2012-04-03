using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Globalization;
using Telerik.Web.UI.Scheduler.Views;
using System.Collections.Specialized;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RadScheduler1_AppointmentInsert(object sender, SchedulerCancelEventArgs e)
    {
        e.Appointment.Attributes["Subject"] = e.Appointment.Subject;
    }

    protected void RadGrid1_RowDrop(object sender, GridDragDropEventArgs e)
    {
        GridDataItem dataItem = e.DraggedItems[0];

        Hashtable values = new Hashtable();
        dataItem.ExtractValues(values);

        int id = (int)dataItem.GetDataKeyValue("AppointmentID");
        string subject = (string)values["Subject"];
        string targetSlotIndex = TargetSlotHiddenField.Value;

        if (targetSlotIndex != string.Empty)
        {
            HandleSchedulerDrop(id, subject, targetSlotIndex);
            TargetSlotHiddenField.Value = string.Empty;
        }

        RadScheduler1.Rebind();
        RadGrid1.Rebind();
        RadAjaxManager1.AjaxSettings.AddAjaxSetting(RadGrid1, RadScheduler1);
    }

    private void HandleSchedulerDrop(int id, string subject, string targetSlotIndex)
    {
        RadScheduler1.Rebind();

        ISchedulerTimeSlot slot = RadScheduler1.GetTimeSlotFromIndex(targetSlotIndex);

        TimeSpan duration = TimeSpan.FromHours(1);
        if (slot.Duration == TimeSpan.FromDays(1))
        {
            duration = slot.Duration;
        }

        ScheduleAppointment(id, subject, slot.Start, slot.Start.Add(duration));
    }


    protected void RadScheduler1_AppointmentCommand(object sender, AppointmentCommandEventArgs e)
    {
        if (e.CommandName == "Unschedule")
        {
            int id = (int)e.Container.Appointment.ID;

            string subject = "testing";
            if (!string.IsNullOrEmpty(e.Container.Appointment.Attributes["Subject"]))
                subject = e.Container.Appointment.Attributes["Subject"];

            UnscheduleAppointment(id, subject);
            RadScheduler1.Rebind();
            RadGrid1.Rebind();

            RadAjaxManager1.AjaxSettings.AddAjaxSetting(RadScheduler1, RadGrid1);
        }
    }

    private void UnscheduleAppointment(int id, string title)
    {
        IDataSource dataSource = GridDataSource;
        DataSourceView view = dataSource.GetView("DefaultView");

        IOrderedDictionary data = new OrderedDictionary();
        data.Add("Start", null);
        data.Add("End", null);
        data.Add("Subject", title);

        IDictionary keys = new OrderedDictionary();
        keys.Add("AppointmentID", id);

        view.Update(keys, data, new OrderedDictionary(), OnDataSourceOperationComplete);
    }

    private void ScheduleAppointment(int id, string subject, DateTime start, DateTime end)
    {
        IDataSource dataSource = SchedulerDataSource;
        DataSourceView view = dataSource.GetView("DefaultView");

        IOrderedDictionary data = new OrderedDictionary();
        data.Add("Subject", subject);
        data.Add("Start", start);
        data.Add("End", end);

        IDictionary keys = new OrderedDictionary();
        keys.Add("AppointmentID", id);

        view.Update(keys, data, new OrderedDictionary(), OnDataSourceOperationComplete);
    }

    private static bool OnDataSourceOperationComplete(int count, Exception e)
    {
        if (e != null)
        {
            throw e;
        }
        return true;
    }

    protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem && RadGrid1.MasterTableView.IsItemInserted)
        {
            e.Item.Visible = false;
        }
    }

    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.InitInsertCommandName)
        {
            e.Canceled = true;
            //Prepare an IDictionary with the predefined values
            ListDictionary newValues = new ListDictionary();
            newValues["Subject"] = "New Person";
            //Insert the item and rebind
            e.Item.OwnerTableView.InsertItem(newValues);
        }
    }
}
