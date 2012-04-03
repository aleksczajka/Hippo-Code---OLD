<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" CodeFile="EventSearch.aspx.cs" EnableEventValidation="false" 
Inherits="EventSearch" Title="Search local events | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/SearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ MasterType TypeName="MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<style type="text/css">
        .RadPanelBar_Default .rpExpandable .rpText
        {
            background-position: 20px 70% !important;
        }
</style>
<div id="testdiv" runat="server"></div>

<script type="text/javascript">/* <![CDATA[ */document.onclick=check;function check(e){var target=(e&&e.target)||(event&&event.srcElement);var obj=document.getElementById('DropDiv');var obj2=document.getElementById('Div1');var parent=checkParent(target);if(parent){obj.style.display='none'}}function checkParent(t){while(t.parentNode){if(t==document.getElementById('SuperTd')){return false}t=t.parentNode}return true}function onPanelItemClicking(sender,eventArgs){eventArgs.get_domEvent().stopPropagation()} /* ]]> */</script>    
               
        <div style="float: left; width: 430px;" class="EventDiv">
        
        <div style="background-repeat: no-repeat; padding-left: 20px; padding-top: 25px;width: 448px; height: 50px; margin-top: -28px; margin-left: -20px;">
            <asp:Label runat="server" CssClass="SearchHeader" ID="EventTitle" Text="Search Events"></asp:Label>
            <div style="margin-top: -5px; line-height: 20px;width: 170px; float: right; padding-right: 45px; font-family: Arial; font-size: 11px; color: White;">
            <span style="line-height: 12px; color: #A5C13A; font-style: italic; font-weight: bold; ">Can't find events in your favorite city or venue? <a class="AddLink" href="EnterEvent.aspx">Post them</a> or 
            tell us to <a class="AddLink" href="ContactUs.aspx">Fill them in</a>.</span>
<%--                <a style="color: White;text-decoration: none;" href="BlogEvent.aspx">Add an <span style="color: #1fb6e7;">Event</span></a> <br />
                <a style="color: White;text-decoration: none;" href="User.aspx">View your <span style="color: #1fb6e7;">Recommended Events</span> </a>
--%>            </div>
        </div>

<%--<table width="420px" cellpadding="0"  cellspacing="0">
            <tr>
                <td align="right" width="420px">
                    <div align="right" class="EventDiv" style=" font-weight: bold; color: #145769; font-family: Arial; font-size: 12px;">
                        <button id="Button2" runat="server" onserverclick="GoToLogin" style=" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;" 
                        onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" >Add an Event</button>
                    </div>
                </td>
            </tr>
        </table>--%>
<%--        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
        <ContentTemplate>
--%>        <%--<a id="SearchTag"></a>--%>
        
<%--         <rad:RadPanelBar runat="server" ID="SearchPanel" CssClass="HandleOverflow" 
         AllowCollapseAllItems="false" Width="440px" ExpandMode="singleExpandedItem">
            <Items>
                <rad:RadPanelItem Expanded="true" Text="<span style='font-family: Arial; font-size: 20px; color: White;'>Enter Search Criteria: </span>">
                    <Items>
                                <rad:RadPanelItem runat="server" Expanded="false">
                                    <Items>
                                        <rad:RadPanelItem></rad:RadPanelItem>
                                    </Items>
                                    <ItemTemplate>
--%>                                <asp:Panel ID="Panel2" runat="server" DefaultButton="SearchButton">
                               
                                <%--<button id="Button1" runat="server" onserverclick="Search" 
                                onclick="this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';" 
                                style=" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;" 
                                onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" >Search</button>--%>
                        
                                <table width="400px">
                                
                                    <tr>
                                        <td colspan="3">
                                            <label><span style="color: #1FB6E7; font-weight: bold;">What City, State or Zip are you searching in?</span> </label><br />
                                            <div style="padding-top: 10px;">
                                            <div style="float: left; width: 150px; padding-right: 10px;">
                                                        <rad:RadComboBox Skin="WebBlue" runat="server" OnSelectedIndexChanged="ChangeState" 
                                                            DataSourceID="SqlDataSourceCountry" DataTextField="country_name" 
                                                            DataValueField="country_id" Width="150px" AutoPostBack="true"  ID="CountryDropDown">
                                                            </rad:RadComboBox>
                                                    
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                                        ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                            
                                             </div>           
                                             <div style="float: left;">       
                                                       
                                                <rad:RadPanelBar Width="271px" runat="server" CollapseAnimation-Duration="50" CollapseAnimation-Type="linear" 
                                            ExpandAnimation-Type="linear" ExpandAnimation-Duration="50" BackColor="#393939" OnClientItemClicking="onPanelItemClicking" 
                                            ID="locationSearchPanel" CssClass="HandleOverflow" 
                                            AllowCollapseAllItems="true" ExpandMode="SingleExpandedItem">
                                                <Items>
                                                    <rad:RadPanelItem Expanded="true" Text="<div style='color: #cccccc; font-weight: bold; text-align: center;height: 10px; '>Using Only City and State</div>">
                                                        <Items>
                                                            <rad:RadPanelItem runat="server" Expanded="true">
                                                                <Items>
                                                                    <rad:RadPanelItem BackColor="#393939"></rad:RadPanelItem>
                                                                </Items>
                                                                <ItemTemplate>
                                                                <div style="padding-left: 20px; padding-top: 7px;">
                                                                    <table cellspacing="0" bgcolor="#393939"  width="100%" >
                                                                        <tr>
                                                                            
                                                                            <td>
                                                                                <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                                                    <rad:RadTextBox EmptyMessage="State" Skin="WebBlue"  ID="StateTextBox" Width="100" runat="server"></rad:RadTextBox>
                                                                                </asp:Panel>
                                                                                <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                                                    <div style="width: 100px;">
                                                                                    <rad:RadComboBox Text="State" EnableItemCaching="true" EmptyMessage="State" runat="server" 
                                                                                    Width="100px" Skin="WebBlue" ID="StateDropDown"></rad:RadComboBox>
                                                                                    </div>
                                                                                </asp:Panel>
                                                                            </td>
                                                                            <td>
                                                                                    <rad:RadTextBox EmptyMessage="City" Skin="WebBlue" Width="100" 
                                                                                    runat="server" ID="CityTextBox"></rad:RadTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                    </table>
                                                                </div>
                                                        </ItemTemplate>
                                                                </rad:RadPanelItem>
                                                        </Items>
                                                       </rad:RadPanelItem>
                                                       <rad:RadPanelItem Text="<div style='color: #cccccc; font-weight: bold;height: 40px; text-align: center;'><span style='color: #1FB6E7; font-weight: bold; font-size: 18px;'>OR</span> <br/>Using Only Zip and Radius</div>">
                                                        <Items>
                                                            <rad:RadPanelItem runat="server" Expanded="false">
                                                                <Items>
                                                                    <rad:RadPanelItem BackColor="#393939"></rad:RadPanelItem>
                                                                </Items>
                                                                <ItemTemplate>
                                                                    <div style="padding-left: 20px;">
                                                                        <table cellspacing="0" bgcolor="#393939"  width="100%" >
                                                                            <%--<tr>
                                                                                <td>
                                                                                    <div style="float: left;">enter zip</div> <div style="float: left;"><asp:Panel runat="server" ID="USLabelPanel"><label>
                                                                                    <span style="font-size: 10px;">(5 digits)</span></label></asp:Panel></div>
                                                                                </td>
                                                                                <asp:Panel runat="server" ID="RadiusTitlePanel">
                                                                                <td>
                                                                                    radius
                                                                                </td>
                                                                                </asp:Panel>
                                                                            </tr>--%>
                                                                            <tr>
                                                                                <td>
                                                                                    <rad:RadTextBox EmptyMessage="Zip(5 digits)" Skin="WebBlue"  ID="ZipTextBox" Width="70" runat="server"></rad:RadTextBox>
                                                                                </td>
                                                                                <asp:Panel runat="server" ID="RadiusDropPanel">
                                                                                <td>
                                                                                    <rad:RadComboBox Skin="WebBlue"  runat="server" ID="RadiusDropDown">
                                                                                        <Items>
                                                                                            <rad:RadComboBoxItem Value="0" Text="Just Zip Code (0 miles)" />
                                                                                            <rad:RadComboBoxItem Value="1" Text="1 mile" />
                                                                                            <rad:RadComboBoxItem Value="5" Text="5 miles" />
                                                                                            <rad:RadComboBoxItem Value="10" Text="10 miles" />
                                                                                            <rad:RadComboBoxItem Value="15" Text="15 miles" />
                                                                                            <rad:RadComboBoxItem Value="30" Text="30 miles" />
                                                                                            <rad:RadComboBoxItem Value="50" Text="50 miles" />
                                                                                        </Items>
                                                                                    </rad:RadComboBox>
                                                                                </td>
                                                                                </asp:Panel>
                                                                            </tr>
                                                                        </table>
                                                                    </div>    
                                                                </ItemTemplate>
                                                                </rad:RadPanelItem>
                                                        </Items>
                                                       </rad:RadPanelItem>
                                                 </Items>
                                             </rad:RadPanelBar>
                                            </div>       
                                                
                                           </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:CheckBox Checked="true" runat="server" ID="GroupCheckBox" Text="Include Group Events" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="Panel1" DefaultButton="SearchButton" runat="server"><div style="padding-bottom: 5px;padding-top: 10px;"><label><span style="color: #1FB6E7; font-weight: bold;">Give us some distinguishing keywords (ex: part of a name)</span></label></div><div style="padding-left: 20px;"><ctrl:HippoTextBox runat="server" ID="SearchTextBox" TEXTBOX_WIDTH="200" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" /></div>
                                            <%--                                <asp:Button runat="server" ID="SearchButton" OnClick="Search" Text="Search" CssClass="SearchButton"
                                                                            onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                            onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                            onclientclick="this.text = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';" 
                                                                                />
                                            --%>                                    </asp:Panel>
                                            <asp:Button runat="server" ID="SearchButton" Text="Search" CssClass="SearchButton" OnClick="Search"
                                            onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                            onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                             onclientclick="this.value = 'Working...';" 
                                             />
                                             <br />
                                            <div style="padding-bottom: 5px;">
                                               <asp:Label runat="server" ID="MessageLabel" CssClass="AddLink" ForeColor="Red"></asp:Label>
                                               </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Lowest Rating:</label>
                                        </td>
                                        <td>
                                            <label>Time Frame:</label>
                                        </td>
                                        <td>
                                            <label>Venue:</label>
                                            <asp:LinkButton runat="server" OnClientClick="this.style.cursor = 'wait';" OnClick="StateChanged" ID="GoLabel" CssClass="AddLink" Text="GO"></asp:LinkButton>
                                            <rad:RadToolTip Width="200px" AutoCloseDelay="100000" Position="MiddleRight" RelativeTo="Element" runat="server" 
                                            TargetControlID="GoLabel" 
                                            Animation="None" ><label style="color: #cccccc;">Click here to fill venue drop down 
                                            with your selected location. (If the drop down does not fill, please make sure you have included a correct Zip Code or City and State.</label></rad:RadToolTip>
                                        </td>
                                    </tr>
                                    <tr>
        
                                        <td valign="top">
                                            <rad:RadComboBox EmptyMessage="Lowest Rating" 
                                            runat="server" Skin="WebBlue" 
                                            ID="RatingDropDown" >
                                                <Items>
                                                        <rad:RadComboBoxItem Text="Lowest Rating" Value="-1" />
                                                        <rad:RadComboBoxItem Text="Don't Care" Value="0" />
                                                        <rad:RadComboBoxItem Text="1" Value="1" />
                                                        <rad:RadComboBoxItem Text="2" Value="2" />
                                                        <rad:RadComboBoxItem Text="3" Value="3" />
                                                        <rad:RadComboBoxItem Text="4" Value="4" />
                                                        <rad:RadComboBoxItem Text="5" Value="5" />
                                                        <rad:RadComboBoxItem Text="6" Value="6" />
                                                        <rad:RadComboBoxItem Text="7" Value="7" />
                                                        <rad:RadComboBoxItem Text="8" Value="8" />
                                                        <rad:RadComboBoxItem Text="9" Value="9" />
                                                        <rad:RadComboBoxItem Text="10" Value="10" />
                                                    </Items>
                                          </rad:RadComboBox>
                                        </td>
                                        <td id="SuperTd" valign="top">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <input id="HiddenMode" type="hidden" runat="server" />
                                                
                                                <div style="display: none;" id="DataDiv"></div>
                                                 
                                                 <asp:Panel runat="server" Visible="false">
           <%--                                                                                  <script type="text/javascript">
        

    //            /* <![CDATA[ */
    //            function onPanelItemClicking(sender, eventArgs)       
    //            {       
    //                eventArgs.get_domEvent().stopPropagation();       
    //            }  
    //            function fillDrop(theVar)
    //            {
    //                var combo2 = document.getElementById('ctl00_ContentPlaceHolder1_SearchPanel_i0_i0_DateDropDown_Input');
    //                combo2.value = theVar;
    //                combo2.defaultValue = theVar;
    //                combo2.className = '';
    //                var comboBig = document.getElementById('ctl00_ContentPlaceHolder1_SearchPanel_i0_i0_DateDropDown_DropDown');
    //                //comboBig.parentNode.style.display = 'none';
    //                //comboBig.parentNode.style.zIndex = '1';
    //                //comboBig.parentNode.style.overflow = 'hidden';
    //                //comboBig.firstChild.style.overflow = 'hidden';
    //                //comboBig.firstChild.style.display = 'none';
    //                //comboBig.style.display = 'none';
    //                setCookie("dateValue",theVar,1);
    //            }
    //            
    //            function StopPropagation(e)
    //            {
    //                //cancel bubbling
    //                e.cancelBubble = true;
    //                if (e.stopPropagation)
    //                {
    //                    e.stopPropagation();
    //                }
    //            }

    //function ondateselected(sender, e) {
    //            if (e.get_newDate() != null) {
    //                fillDrop(e.get_newDate().toDateString());
    //            }
    //        }



    //        /* ]]> */

    </script>  
    //                                          <%--  <rad:RadComboBox EmptyMessage="Time Frame" 
    //                                            runat="server" Width="160" Skin="Black" 
    //                                            ID="DateDropDown" >
    //                                                <Items>
    //                                                    <rad:RadComboBoxItem Value="0" />
                                                       <%-- <rad:RadComboBoxItem Value="0" />
                                                       <rad:RadComboBoxItem Text="Date" />
                                                        <rad:RadComboBoxItem Text="Today" />
                                                        <rad:RadComboBoxItem Text="Tomorrow" />
                                                        <rad:RadComboBoxItem Text="This Week" />
                                                        <rad:RadComboBoxItem Text="This Weekend" />
                                                        <rad:RadComboBoxItem Text="Next Week" />
                                                        <rad:RadComboBoxItem Text="This Month" />
                                                        <rad:RadComboBoxItem Text="All Past Events" />
                                                        <rad:RadComboBoxItem Text="All Future Events" />--%>
                                                        
                                                        
    <%--                                                </Items>
                                                    <ItemTemplate>
                                                       
    --%>                                                    <%--<asp:LinkButton OnClientClick="StopPropagation(event);fillDrop('Today');" runat="server" CssClass="AddWhiteLink" Text="Today" Font-Bold="false" 
                                                        ForeColor="#cccccc" OnClick="SetDateBoxToday"></asp:LinkButton><br />--%>
    <%--                                                    <a class="AddWhiteLink" style="color: #cccccc;" onclick="StopPropagation(event);fillDrop('Today');">Today</a><br />
                                                        <a class="AddWhiteLink" style="color: #cccccc;"   onclick="StopPropagation(event);fillDrop('Tomorrow');">Tomorrow</a><br />
    --%>                                                    <%--<asp:LinkButton OnClientClick="fillDrop('Tomorrow');"  ID="LinkButton2" runat="server" CssClass="AddWhiteLink" Font-Bold="false"  
                                                        ForeColor="#cccccc"  Text="Tomorrow" ></asp:LinkButton><br />--%>
    <%--                                                    <a class="AddWhiteLink" style="color: #cccccc;"   onclick="StopPropagation(event);fillDrop('This Weekend');">This Weekend</a><br />
    --%>                                                    <%--<asp:LinkButton OnClientClick=""  ID="LinkButton3" runat="server" CssClass="AddWhiteLink" Font-Bold="false"  
                                                        ForeColor="#cccccc"  Text="This Weekend" OnClick="SetDateBoxThisWeekend"></asp:LinkButton><br />--%>
    <%--                                                    <a class="AddWhiteLink" style="color: #cccccc;"   onclick="StopPropagation(event);fillDrop('This Week');">This Week</a><br />
    --%><%--                                                    <asp:LinkButton OnClientClick=""  ID="LinkButton4" runat="server" CssClass="AddWhiteLink" Font-Bold="false"  ForeColor="#cccccc"  Text="This Week" OnClick="SetDateBoxThisWeek"></asp:LinkButton><br />
    --%>                                                    
    <%--                                                    <a class="AddWhiteLink" style="color: #cccccc;"   onclick="StopPropagation(event);fillDrop('Next Week');">Next Week</a><br />
    --%><%--<asp:LinkButton OnClientClick=""  ID="LinkButton5" runat="server" CssClass="AddWhiteLink" Font-Bold="false"  ForeColor="#cccccc"  Text="Next Week" OnClick="SetDateBoxNextWeek"></asp:LinkButton><br />
    --%>                                                   
    <%--<a class="AddWhiteLink" style="color: #cccccc;"   onclick="StopPropagation(event);fillDrop('This Month');">This Month</a><br />
    --%><%-- <asp:LinkButton OnClientClick=""  ID="LinkButton6" runat="server" CssClass="AddWhiteLink" Font-Bold="false"  ForeColor="#cccccc"  Text="This Month" OnClick="SetDateBoxThisMonth"></asp:LinkButton><br />
    --%>                                                    
    <%--<a class="AddWhiteLink" style="color: #cccccc;"   onclick="StopPropagation(event);fillDrop('All Past Events');">All Past Events</a><br />
    --%><%--<asp:LinkButton OnClientClick=""  ID="LinkButton7" runat="server" CssClass="AddWhiteLink" Font-Bold="false"  ForeColor="#cccccc"  Text="All Past Events" OnClick="SetDateBoxAllPastEvents"></asp:LinkButton><br />
    --%>                                                   
    <%--<a class="AddWhiteLink" style="color: #cccccc;"   onclick="StopPropagation(event);fillDrop('All Future Events');">All Future Events</a><br />
    --%><%-- <asp:LinkButton OnClientClick=""  ID="LinkButton8" runat="server" CssClass="AddWhiteLink" Font-Bold="false"  ForeColor="#cccccc"  Text="All Future Events" OnClick="SetDateBoxAllFutureEvents"></asp:LinkButton>
    --%>
                                                       <%-- <div>--%>
    <%--                                                        <a onmouseover="this.backgroundColor = '#ffffff';" onclick="OnClientNodeClickingHandler('Today');">Today</a><br />
                                                            <a  onclick="OnClientNodeClickingHandler('Tomorrow', this);">Tomorrow</a><br />
                                                           <a  onclick="OnClientNodeClickingHandler('This Weekend', this);">This Weekend</a><br />
                                                           <a  onclick="OnClientNodeClickingHandler('This Week', this);">This Week</a><br />
                                                           <a  onclick="OnClientNodeClickingHandler('Next Week', this);">Next Week</a><br />
                                                           <a  onclick="OnClientNodeClickingHandler('This Month', this);">This Month</a><br />
                                                           <a  onclick="OnClientNodeClickingHandler('All Past Events', this);">All Past Events</a><br />
                                                           <a  onclick="OnClientNodeClickingHandler('All Future Events', this);">All Future Events</a><br />
    --%>                                                        <%--<rad:RadDatePicker ClientEvents-OnDateSelected="ondateselected" runat="server" ID="RadDatePicker2">
                                                                <DateInput ID="DateInput1" runat="server" EmptyMessage="Select Time Frame"></DateInput>
                                                                
                                                            </rad:RadDatePicker>
                                                        </div>
                                                    </ItemTemplate>
                                                </rad:RadComboBox>--%>
                                            </asp:Panel>
                                                
                                                <script type="text/javascript">function CloseDiv(){var theDiv=document.getElementById('DropDiv');theDiv.style.display='none'}function OpenCloseDiv(){var theDiv=document.getElementById('DropDiv');if(theDiv.style.display=='none')theDiv.style.display='block';else theDiv.style.display='none'}function fillDrop(theText){var theDiv=document.getElementById('ctl00_ContentPlaceHolder1_TimeFrameDiv');theDiv.innerHTML=theText;__doPostBack('ctl00$ContentPlaceHolder1$doTime',theText);CloseDiv()}
                                                
                                                function ondayselected(sender,e)
                                                {
                                                    fillDrop('Next '+sender._originalText+' Days');
                                                }
                                                
                                                function ondateselected(sender,e){if(e.get_newDate()!=null){fillDrop(e.get_newDate().toDateString())}}</script>
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:LinkButton runat="server" ID="doTime" OnClick="setTimeSession"></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                <div style="position: relative;">
                                                <div id="DropDiv" style="padding: 3px;bottom: 26px;position: absolute;display: none; width: 150px; height: 180px;background-color: #333333;border: solid 2px #1fb6e7;">
                                                            <a class="AddWhiteLink" style="color: #cccccc;" onclick="fillDrop('Today');">Today</a><br />
                                                            <a class="AddWhiteLink" style="color: #cccccc;"   onclick="fillDrop('Tomorrow');">Tomorrow</a><br />
                                                            <a class="AddWhiteLink" style="color: #cccccc;"   onclick="fillDrop('This Weekend');">This Weekend</a><br />
                                                            <a class="AddWhiteLink" style="color: #cccccc;"   onclick="fillDrop('This Week');">This Week</a><br />
                                                            <a class="AddWhiteLink" style="color: #cccccc;"   onclick="fillDrop('Next Week');">Next Week</a><br />
                                                            <a class="AddWhiteLink" style="color: #cccccc;"   onclick="fillDrop('This Month');">This Month</a><br />
                                                            <a class="AddWhiteLink" style="color: #cccccc;"   onclick="fillDrop('All Past Events');">All Past Events</a><br />
                                                            <a class="AddWhiteLink" style="color: #cccccc;"   onclick="fillDrop('All Future Events');">All Future Events</a><br />
                                                            <div class="AddWhiteLink" style="color: #cccccc; padding-top: 5px;padding-bottom: 5px !important;" >Next 
                                                                <rad:RadComboBox Width="50px" Skin="WebBlue" runat="server" OnClientSelectedIndexChanged="ondayselected" ID="TimeRad">
                                                                <Items>
                                                                    <rad:RadComboBoxItem Value="1" Text="1" />
                                                                    <rad:RadComboBoxItem Value="2" Text="2"/>
                                                                    <rad:RadComboBoxItem Value="3" Text="3"/>
                                                                    <rad:RadComboBoxItem Value="4" Text="4"/>
                                                                    <rad:RadComboBoxItem Value="5" Text="5"/>
                                                                    <rad:RadComboBoxItem Value="6" Text="6"/>
                                                                    <rad:RadComboBoxItem Value="7" Text="7"/>
                                                                    <rad:RadComboBoxItem Value="8" Text="8"/>
                                                                    <rad:RadComboBoxItem Value="9" Text="9"/>
                                                                    <rad:RadComboBoxItem Value="10" Text="10"/>
                                                                    <rad:RadComboBoxItem Value="11" Text="11"/>
                                                                    <rad:RadComboBoxItem Value="12" Text="12"/>
                                                                    <rad:RadComboBoxItem Value="13" Text="13"/>
                                                                    <rad:RadComboBoxItem Value="14" Text="14"/>
                                                                    <rad:RadComboBoxItem Value="15" Text="15"/>
                                                                    <rad:RadComboBoxItem Value="16" Text="16"/>
                                                                    <rad:RadComboBoxItem Value="17" Text="17"/>
                                                                    <rad:RadComboBoxItem Value="18" Text="18"/>
                                                                    <rad:RadComboBoxItem Value="19" Text="19"/>
                                                                    <rad:RadComboBoxItem Value="20" Text="20"/>
                                                                    <rad:RadComboBoxItem Value="21" Text="21"/>
                                                                    <rad:RadComboBoxItem Value="22" Text="22"/>
                                                                    <rad:RadComboBoxItem Value="23" Text="23"/>
                                                                    <rad:RadComboBoxItem Value="24" Text="24"/>
                                                                    <rad:RadComboBoxItem Value="25" Text="25"/>
                                                                    <rad:RadComboBoxItem Value="26" Text="26"/>
                                                                    <rad:RadComboBoxItem Value="27" Text="27"/>
                                                                    <rad:RadComboBoxItem Value="28" Text="28"/>
                                                                    <rad:RadComboBoxItem Value="29" Text="29"/>
                                                                    <rad:RadComboBoxItem Value="30" Text="30"/>
                                                                    <rad:RadComboBoxItem Value="31" Text="31" />
                                                                </Items>
                                                                </rad:RadComboBox> Days  or
                                                            </div>
                                                            <div class="SpecialCalendar" style="position: absolute;">
                                                               <rad:RadDatePicker Skin="WebBlue" ClientEvents-OnDateSelected="ondateselected" runat="server" ID="RadDatePicker2">
                                                                    <DateInput ID="DateInput1" runat="server" EmptyMessage="Select Time Frame"></DateInput>
                                                                    
                                                                </rad:RadDatePicker>
                                                            </div>
                                                     </div>
                                                    <div runat="server" onblur="CloseDiv();" onclick="OpenCloseDiv();" 
                                                    style="cursor: pointer;width: 154px;  margin-top: -3px; font-family: Arial; font-size: 12px;" id="Div1">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="background: url(WebResource.axd?d=tNXezzmGKa2NolE9-r64R1r5K1Ew5Ttj4hsRzFxbmsRSilWM5JplU8wba56SCb-aoCaV4efW6oglO3iP9-S0NQ2&t=634038585057567693) no-repeat scroll 0 0 transparent;">
                                                                <div runat="server"
                                                    style="margin-top: 3px; margin-left: 5px; color: #000000;cursor: pointer;width: 120px; height: 18px;" id="TimeFrameDiv">
                                                                Time Frame
                                                                </div>
                                                            </td>
                                                            <td width="24px" style="background: url(WebResource.axd?d=tNXezzmGKa2NolE9-r64R1r5K1Ew5Ttj4hsRzFxbmsRSilWM5JplU8wba56SCb-aOU6QTDbv1z9orbTGVQC5oPxMBQPaClPQWpXj3yu0Bnk1&t=634038585057567693) no-repeat scroll 0 0 transparent;">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </div>
                                                    
                                                 </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        </td>
                                       <td valign="top">
                                       <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                        <ContentTemplate>
                                         <script type="text/javascript">function fillVen(theText,theID){var theDiv=document.getElementById('ctl00_ContentPlaceHolder1_VenueDiv');theDiv.innerHTML=theText.replace("&&","'");var theidDiv=document.getElementById('ctl00_ContentPlaceHolder1_VenueIDDIV');theidDiv.innerHTML=theID;var tooltip=Telerik.Web.UI.RadToolTip.getCurrent();if(tooltip)tooltip.hide();__doPostBack('ctl00$ContentPlaceHolder1$doVenue',theID+';'+theText.replace("&&","'"))}function HideAllDivs(){var theDiv=document.getElementById('contentDivA');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivA');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivB');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivB');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivC');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivC');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivD');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivD');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivE');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivE');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivF');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivF');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivG');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivG');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivH');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivH');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivI');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivI');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivJ');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivJ');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivK');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivK');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivL');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivL');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivM');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivM');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivN');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivN');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivO');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivO');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivP');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivP');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivQ');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivQ');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivR');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivR');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivS');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivS');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivT');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivT');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivU');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivU');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivV');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivV');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivW');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivW');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivX');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivX');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivY');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivY');theDiv.className='AddWhiteLink'}theDiv=document.getElementById('contentDivZ');if(theDiv!=null&&theDiv!=undefined){theDiv.style.display='none';theDiv=document.getElementById('titleDivZ');theDiv.className='AddWhiteLink'}}function SelectLetterDiv(letter){HideAllDivs();var theDiv=document.getElementById('contentDiv'+letter);theDiv.style.display='block';theDiv=document.getElementById('titleDiv'+letter);theDiv.className="AddGreenLink"}</script>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                                <ContentTemplate>
                                                    <asp:LinkButton runat="server" ID="doVenue" OnClick="setVenueSession"></asp:LinkButton>
                                                    <asp:Linkbutton runat="server" ID="clearVenue" OnClick="clearVenueSession"></asp:Linkbutton>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div style="width: 200px;">
                                           
                                                <rad:RadToolTip RelativeTo="Element" Position="TopCenter" ManualClose="true" runat="server" ID="Tip1" TargetControlID="Div2" ShowEvent="OnClick">
                                                    <span style="color: #cccccc;">Select a location and click 'Go' to fill this space with Venues.</span>
                                                </rad:RadToolTip>
                                                <div runat="server" id="VenueIDDIV" style="display: none;"></div>
                                                <div runat="server" style="float; left;cursor: pointer;width: 110px; margin-top: -3px;" id="Div2">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <div style="float: left;border: solid 2px #628E02;padding: 2px;background-color: transparent;color: White; font-weight: bold; font-family: Arial;font-size: 12px;" runat="server" id="VenueDiv">
                                                                Select Venue >
                                                            </div>
<%--                                                            <img style="padding-top: 7px;float: left;" src="image/VenuesArrow.png" />
--%>                                                    </td>
                                                    </tr>
                                                </table>
                                                </div>
                                                <script type="text/javascript">
                                                    function ClearVenue()
                                                    {
                                                        var theDiv=document.getElementById('ctl00_ContentPlaceHolder1_VenueDiv');
                                                        theDiv.innerHTML = 'Select Venue >';
                                                        __doPostBack('ctl00$ContentPlaceHolder1$clearVenue','');
                                                    }
                                                </script>
                                                <div class="AddGreenLink" style="cursor: pointer;padding-top: 5px; float: left;" onclick="ClearVenue();">clear venue</div>
                                             </div>
                                        <%-- <rad:RadComboBox EmptyMessage="Venue" runat="server" Width="105" Skin="Black" ID="VenueDropDown" >
                                            </rad:RadComboBox>
                                                    <asp:SqlDataSource runat="server" ID="VenueDataSource" SelectCommand="SELECT * FROM Venues"
                        ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>--%>
                                        </ContentTemplate>
                                       </asp:UpdatePanel>
                                    </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="padding-top: 10px; padding-bottom: 5px;">
                                            <div style="padding-bottom: 5px;"><label><span style="color: #1FB6E7; font-weight: bold;">What type of event are you looking for?</span></label></div>
                                            <div class="EventDiv" style="border: 3px solid #999999;background-color: #333333; width: 420px; padding-left: 5px; padding-top: 5px; color: #cccccc; min-height: 200px;">
                                                <table>
                                                    <tr>
                                                        <td valign="top">
                                                                <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                                RepeatColumns="4" RepeatDirection="horizontal">
                                                                
                                                                </asp:CheckBoxList>--%>
                                                                <div class="FloatLeft" style="width: 200px;">
                                                                    <rad:RadTreeView  Width="200px" runat="server"  
                                                                    ID="CategoryTree" DataFieldID="ID" ForeColor="#cccccc" DataSourceID="SqlDataSource1" 
                                                                    DataFieldParentID="ParentID"  Skin="WebBlue"
                                                                    CheckBoxes="true">
                                                                    <DataBindings>
                                                                         <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                         TextField="Name" Expanded="false" ValueField="ID" />
                                                                     </DataBindings>
                                                                    </rad:RadTreeView>
                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                                        SelectCommand="SELECT * FROM EventCategories WHERE ID < 26 OR ID=52 OR ID=53 OR ID= 34 OR ID=40 OR ID=50 OR ID=51 OR ID=35 OR ID=47 OR ID=56 OR ID=57 OR ID=58 OR ID=61 OR ID=62 ORDER BY Name ASC"
                                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                    </asp:SqlDataSource>
                                                                </div>
                                                                
                                                           
                                                        </td>
                                                        <td valign="top">
                                                            <div style=" width: 200px;">
                                                                <rad:RadTreeView Width="200px" ForeColor="#cccccc" runat="server"  
                                                                ID="RadTreeView1" DataFieldID="ID" DataSourceID="SqlDataSource2" 
                                                                DataFieldParentID="ParentID"  Skin="WebBlue"
                                                                CheckBoxes="true">
                                                                <DataBindings>
                                                                     <rad:RadTreeNodeBinding  Checkable="true" ValueField="ID" Checked="false" 
                                                                     TextField="Name" Expanded="false" />
                                                                 </DataBindings>
                                                                </rad:RadTreeView>
                                                                <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                                    SelectCommand="SELECT * FROM EventCategories WHERE ID >= 26 AND  NOT(ID= 52) AND  NOT(ID= 53) AND NOT(ID= 34) AND NOT(ID=40) AND NOT(ID=50) AND NOT(ID=51) AND NOT (ID=35) AND NOT (ID=61) AND NOT (ID=62) AND NOT (ID=47) AND NOT (ID=56) AND NOT (ID=57) AND NOT (ID=58) ORDER BY Name ASC"
                                                                    ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                </asp:SqlDataSource>
                                                            </div>
                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                                </div>
                                        </td>
                                    </tr>
                                </table>

                                    
                                
                                </asp:Panel>
  <%--                          </ItemTemplate>
                            
                            
                            
                            
                            
                            
                            
                            
                                </rad:RadPanelItem>
                            </Items>
                          
                </rad:RadPanelItem>
--%>                <%--<rad:RadPanelItem Width="440px" Text="<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(0 Records Found)</div></div>">
                    <Items>
                        <rad:RadPanelItem runat="server" Expanded="false">
                            <Items>
                                <rad:RadPanelItem></rad:RadPanelItem>
                            </Items>
                            <ItemTemplate>
                                <asp:Panel runat="server" ID="Panel7" Width="440px">
                                <asp:Label runat="server" CssClass="ResultsLabel" ID="ResultsLabel"></asp:Label>
                                <br />
                                <asp:DropDownList runat="server" AutoPostBack="true"  
                                OnSelectedIndexChanged="SortResults" ID="SortDropDown">
                                    <asp:ListItem Text="Sort By..." Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Date Up" Value="DateTimeStart DESC"></asp:ListItem>
                                    <asp:ListItem Text="Date Down" Value="DateTimeStart ASC"></asp:ListItem>
                                    <asp:ListItem Text="Title Up" Value="Header DESC"></asp:ListItem>
                                    <asp:ListItem Text="Title Down" Value="Header ASC"></asp:ListItem>
                                    <asp:ListItem Text="Venue Up" Value="Name DESC"></asp:ListItem>
                                    <asp:ListItem Text="Venue Down" Value="Name ASC"></asp:ListItem>
                                </asp:DropDownList>
                                  <ctrl:SearchElements runat="server" ID="SearchElements2" />
                                  <br /><br />
                                    </asp:Panel>
                            </ItemTemplate>
                        </rad:RadPanelItem>
                    </Items>
                    
                </rad:RadPanelItem>--%>
           <%-- </Items>
        </rad:RadPanelBar>--%>
        
        
<%--        </ContentTemplate>
        </asp:UpdatePanel>
--%>        </div>
        <div style="float: right;">
            <div style="clear: both;">
             <%--<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
            codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" 
            width="431" height="575" title="banner" wmode="transparent">  
            <param name="movie" value="gallery.swf" />  
            <param name="quality" value="high" />
            <param name="wmode" value="transparent"/>   
                <embed src="gallery.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" 
                type="application/x-shockwave-flash" width="431" height="575" wmode="transparent">
                </embed>
            </object>--%>
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <ctrl:Ads ID="Ads1" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                
            </div>
            
        </div>

</asp:Content>

