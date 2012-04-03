<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Restaurant Reason</title>
    <style type="text/css">
    
        .RadScheduler .rsContentTable
        {
            width: 100%;
        }
        .RadScheduler .rsHorizontalHeaderTable
        {
            width: 100%;
        }
        .RadScheduler .rsAllDayTable
        {
            width: 100%;
        }
        #paneContainer
        {
            width: 741px;
            height: 907px;
            background: transparent url('images/panel.gif') no-repeat 0 0;
            position: relative;
        }

        #paneContainer .header
        {
            font: 15px/20px Verdana, sans-serif;
            color: #fff;
            margin: 0;
            padding: 0;
        }

        #paneContainer .caption
        {
            font: 13px/22px Verdana, sans-serif;
            color: #0e7bba;
            text-transform:uppercase;
            margin: 0;
            padding: 14px 0 5px;
        }

        #unassignedTasksPane .header,
        #myTasksPane .header
        {
            position: absolute;
            left: 25px;
            top: 20px;
        }

        #myTasksPane .header
        {
            left: 203px;
        }

        #unassignedTasksPane,
        #myTasksPane
        {
            margin-top: 56px;
        }

        #unassignedTasksPane
        {
            float: left;
            width: 160px;
            padding: 16px 0 0 10px;
            margin-left: 5px;
        }

        * html #unassignedTasksPane { margin-left: 6px; }

        #myTasksPane
        {
            float: left;
            width: 512px;
            margin-left: 20px;
        }

        #scheduledTasksPane
        {
            height: 465px;
        }

        #unscheduledTasksPane .container
        {
            padding: 12px 0 7px;
        }

        #RadScheduler1 .rsTopWrap
        {
            border: 0;
        }

        #RadScheduler1 .rsAllDayHeader
        {
            text-align: center;
        }

        .rsAptContent
        {
            text-indent: 5px;
        }

        input.simpleButton
        {
            border: 1px solid #5B7AA2;
            background: #EEF3F9 url('images/unlink.png') no-repeat 50% 50%;
            margin: 2px 0 3px 3px;
            width: 37px;
            height: 15px;
            outline: 0;
            display: inline-block;
            vertical-align: middle;
        }

        .simpleButton:hover
        {
            background-color: #D0D7E5;
            cursor: pointer;
        }

        #RadScheduler1Panel
        {
            height: 100%;
        }
        
        #RAD_SPLITTER_PANE_CONTENT_BottomPane
        {
            height: auto !important;
        }
        
        .simpleButton
        {   
            position: absolute;
            bottom: 5px;
            left: 5px;
            background-color: red !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
<asp:Label runat="server" ID="testingLabel"></asp:Label>
        <script type="text/javascript">
        
            function rowDropping(sender, eventArgs) {
                // Fired when the user drops a grid row
                var htmlElement = eventArgs.get_destinationHtmlElement();
                var scheduler = $find('<%= RadScheduler1.ClientID %>');

                if (isPartOfSchedulerAppointmentArea(htmlElement)) {
                    // The row was dropped over the scheduler appointment area
                    // Find the exact time slot and save its unique index in the hidden field
                    var timeSlot = scheduler._activeModel.getTimeSlotFromDomElement(htmlElement);

                    $get("TargetSlotHiddenField").value = timeSlot.get_index();

                    // The HTML needs to be set in order for the postback to execute normally
                    eventArgs.set_destinationHtmlElement("TargetSlotHiddenField");
                }
                else {
                    // The node was dropped elsewhere on the document
                    eventArgs.set_cancel(true);
                }
            }

            function isPartOfSchedulerAppointmentArea(htmlElement) {
                // Determines if an html element is part of the scheduler appointment area
                // This can be either the rsContent or the rsAllDay div (in day and week view)
                return $telerik.$(htmlElement).parents().is("div.rsAllDay") ||
                            $telerik.$(htmlElement).parents().is("div.rsContent")
            }

            function onRowDoubleClick(sender, args) {
                sender.get_masterTableView().editItem(args.get_itemIndexHierarchical());
            }
        </script>

    </telerik:RadScriptBlock>
    <input type="hidden" runat="server" id="TargetSlotHiddenField" />
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <ajaxsettings>
                <telerik:AjaxSetting AjaxControlID="RadScheduler1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadScheduler1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </ajaxsettings>
    </telerik:RadAjaxManager>
    <telerik:RadSplitter runat="server" ID="RadSplitter1" Orientation="Vertical" Height="552px" Width="1000px"
        CssClass="exampleContainer" Skin="Office2007">
        <telerik:RadPane runat="server" ID="TopPane" Height="400px" Width="700px" Scrolling="None">
            <telerik:RadScheduler ID="RadScheduler1" runat="server" Skin="Office2007" Height="100%" Width="100%"
             RowHeaderWidth="52" OverflowBehavior="Scroll" ShowFooter="false"
             DataSourceID="SchedulerDataSource" DataKeyField="AppointmentID" DataStartField="Start" DataEndField="End"
             DataSubjectField="Subject" RowHeight="37px"
                OnAppointmentCommand="RadScheduler1_AppointmentCommand" OnAppointmentInsert="RadScheduler1_AppointmentInsert">
                <AppointmentTemplate>
                    <%# Eval("Subject") %>
<asp:Button runat="server" ID="UnscheduleAppointment" CssClass="simpleButton" CommandName="Unschedule"
Text="&nbsp;" ToolTip="Unschedule this waiter/waitress" />
                </AppointmentTemplate>                
            </telerik:RadScheduler>
        </telerik:RadPane>
        <telerik:RadSplitBar runat="Server" ID="RadSplitBar1"  />
        <telerik:RadPane runat="server" ID="BottomPane">
            <div style="border: none;">
                <telerik:RadGrid runat="server" ID="RadGrid1" DataSourceID="GridDataSource" GridLines="None"
                    AutoGenerateColumns="False" OnRowDrop="RadGrid1_RowDrop" Skin="Office2007" Style="border: none; outline: 0" Height="100%" AllowAutomaticInserts="True" AllowAutomaticUpdates="true"
                    ShowFooter="true" OnItemCreated="RadGrid1_ItemCreated" OnItemCommand="RadGrid1_ItemCommand"
                    AllowSorting="true">
                    <clientsettings allowrowsdragdrop="True">
                            <Selecting AllowRowSelect="True" />
                            <ClientEvents OnRowDropping="rowDropping" OnRowDblClick="onRowDoubleClick" />
                        </clientsettings>
                    <mastertableview datakeynames="AppointmentID" insertitemdisplay="Bottom" editmode="InPlace">
                            <Columns>
                                <telerik:GridTemplateColumn DataField="Subject" HeaderText="Waiters/Waitresses">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Label1" Text='<%# Bind("Subject") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton Text="Add new person" CommandName="<%# RadGrid.InitInsertCommandName %>"
                                            runat="server" ID="LinkButton1"></asp:LinkButton>
                                    </FooterTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="Server" ID="TextBox1" Text='<%# Bind("Subject") %>' Width="100%"></asp:TextBox>
                                        <asp:LinkButton ID="btnUpdate" Text="Save" runat="server" CommandName='<%# (Container is GridDataInsertItem) ? RadGrid.PerformInsertCommandName : RadGrid.UpdateCommandName %>'></asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" Text="Cancel" runat="server" CommandName='<%# RadGrid.CancelCommandName %>'></asp:LinkButton>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </mastertableview>
                </telerik:RadGrid>
            </div>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <asp:SqlDataSource ID="SchedulerDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Connection %>"
        SelectCommand="SELECT [AppointmentID], [Start], [End], [Subject] FROM [Appointments_DragAndDrop] WHERE ([Start] IS NOT NULL) AND ([End] IS NOT NULL)"
        InsertCommand="INSERT INTO [Appointments_DragAndDrop] ([Subject], [Start], [End]) VALUES (@Subject, @Start, @End)"
        UpdateCommand="UPDATE [Appointments_DragAndDrop] SET [Start] = @Start, [End] = @End WHERE AppointmentID = @AppointmentID"
        DeleteCommand="DELETE FROM [Appointments_DragAndDrop] WHERE [AppointmentID] = @AppointmentID">
        <insertparameters>
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Start" Type="DateTime" />
                <asp:Parameter Name="End" Type="DateTime" />
            </insertparameters>
        <updateparameters>
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Start" Type="DateTime" />
                <asp:Parameter Name="End" Type="DateTime" />
                <asp:Parameter Name="AppointmentID" Type="Int32" />
            </updateparameters>
        <deleteparameters>
                <asp:Parameter Name="AppointmentID" Type="Int32" />
            </deleteparameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="GridDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Connection %>"
        SelectCommand="SELECT [AppointmentID], [Start], [End], [Subject] FROM [Appointments_DragAndDrop] WHERE ([Start] IS NULL) AND ([End] IS NULL)"
        UpdateCommand="UPDATE [Appointments_DragAndDrop] SET [Start] = @Start, [End] = @End WHERE AppointmentID = @AppointmentID"
        InsertCommand="INSERT INTO [Appointments_DragAndDrop] ([Subject]) VALUES (@Subject)">
        <updateparameters>
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Start" Type="DateTime" />
                <asp:Parameter Name="End" Type="DateTime" />
                <asp:Parameter Name="AppointmentID" Type="Int32" />
            </updateparameters>
        <insertparameters>
                <asp:Parameter Name="Subject" Type="String" />
            </insertparameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
