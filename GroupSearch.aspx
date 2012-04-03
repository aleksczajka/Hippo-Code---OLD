<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false"
 ValidateRequest="false"  AutoEventWireup="true" 
CodeFile="GroupSearch.aspx.cs" Inherits="GroupSearch" Title="Search local groups | HippoHappenings " %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/AdSearchElements.ascx" TagName="AdSearchElements" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ MasterType TypeName="MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<style type="text/css">
        .RadPanelBar_Default .rpExpandable .rpText
        {
            background-position: 20px 70% !important;
        }
</style>
<script type="text/javascript">
    function OnClientClose(oWnd, args)
            {
                if(args != null && args != undefined)
                    window.location = args;
            }
</script>
<script type="text/javascript">
    function onPanelItemClicking(sender, eventArgs)       
    {       
        eventArgs.get_domEvent().stopPropagation();       
    }  

</script>


<div style="float: left;" class="EventDiv">
    <div style="background-repeat: no-repeat; padding-left: 20px; padding-top: 25px;width: 448px; height: 50px; margin-top: -28px; margin-left: -20px;">
        <asp:Label runat="server" CssClass="SearchHeader" ID="EventName" Text="Search Local Groups"></asp:Label>
        <div style="margin-top: -5px; line-height: 20px;width: 130px; float: right; padding-right: 45px; font-family: Arial; font-size: 11px; color: White;">
            <a style="color: White;text-decoration: none;" href="EnterGroup.aspx">Create a <span style="color: #A76CC7;">Group</span></a> <br />
            <a style="color: White;text-decoration: none;" href="my-account?G=true#groups">Configure group <span style="color: #A76CC7;">Settings</span> </a>
        </div>
    </div>


<telerik:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false" Skin="Black" 
                BorderStyle="solid"  Behaviors="none"
            ID="MessageRadWindowManager" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <telerik:RadWindow Width="300" ClientCallBackFunction="OnClientClose"  
                    EnableEmbeddedSkins="true" VisibleTitlebar="false"
                    VisibleStatusbar="false" Skin="Black" Height="300" ID="MessageRadWindow" 
                    Title="Alert" runat="server">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
<%--         <rad:RadPanelBar runat="server" ID="SearchPanel" CssClass="HandleOverflow" AllowCollapseAllItems="true" Width="420px" ExpandMode="SingleExpandedItem">
            <Items>
                <rad:RadPanelItem Expanded="true" Text="<span style='font-family: Arial; font-size: 20px; color: White;'>Enter Search Criteria: </span>">
                    <Items>
                                <rad:RadPanelItem runat="server" Expanded="false">
                                    <Items>
                                        <rad:RadPanelItem></rad:RadPanelItem>
                                    </Items>
                                    <ItemTemplate>
--%>                                    <asp:Panel ID="Panel2" runat="server"  DefaultButton="SearchButton">
                                <div style="width: 420px;">
                                <div style="float: right;">
                                
                                        </div>
                                        </div>
                                        <asp:Label runat="server" ID="Label1" CssClass="AddLink" ForeColor="Red"></asp:Label>

                                         <table width="400px">
                                           
                                       <tr>
                                        <td>
                                            <label><span style="color: #ce90f9; font-weight: bold;">What City, State or Zip are you searching in?</span> </label><br />
                                            <div style="padding-top: 10px;">
                                            <div style="float: left; width: 150px; ">
                                                        <rad:RadComboBox Skin="WebBlue" runat="server" OnSelectedIndexChanged="ChangeState" 
                                                            DataSourceID="SqlDataSourceCountry" DataTextField="country_name" 
                                                            DataValueField="country_id" Width="150px"  AutoPostBack="true"  ID="CountryDropDown">
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
                                                                                    <rad:RadComboBox Text="State" e EnableItemCaching="true" EmptyMessage="State" runat="server" 
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
                                                       <rad:RadPanelItem Text="<div style='color: #cccccc; font-weight: bold;height: 40px; text-align: center;'><span style='color: #ce90f9; font-weight: bold; font-size: 18px;'>OR</span> <br/>Using Only Zip and Radius</div>">
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
                                        <td>
                                            <div style="padding-bottom: 10px;padding-top: 10px;"><label><span style="color: #ce90f9; font-weight: bold;">Give us some distinguishing keywords (ex: part of a name)</span></label></div>
                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="SearchButton" >
                                                    <div style="padding-left: 20px;">
                                                    <ctrl:HippoTextBox runat="server" ID="SearchTextBox" TEXTBOX_WIDTH="200" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                    <%--<button id="Button1" runat="server" onserverclick="Search" 
                                                    onclick="this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';" 
                                                    style=" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;" 
                                                    onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                    onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" >Search</button>
                                                    <asp:Button onclientclick="this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';" 
                                                    runat="server" ID="fakeSearchButton" CssClass="FakeButton" OnClick="Search" />--%>
                                                    <asp:Button runat="server" ID="SearchButton" Text="Search" CssClass="SearchButton" 
                                                    OnClick="Search"
                                                        onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                        onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                        onclientclick="this.value = 'Working...';" />
                                                    </div>
                                                </asp:Panel>
                                          <div style="padding-bottom: 5px;">
                                            <asp:Label runat="server" ID="MessageLabel" CssClass="AddLink" ForeColor="Red"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                            <tr>
                                                <td colspan="3">
                                                <div style="padding-bottom: 5px;padding-top: 10px;"><label><span style="color: #ce90f9; font-weight: bold;">What type of groups are you looking for?</span></label></div>
                                                    <div class="EventDiv" style="border: solid 3px #999999;background-color: #333333; width: 420px; padding-left: 5px; padding-top: 5px; color: #cccccc; min-height: 200px;">
                                                    <table>
                                                        <tr>
                                                            <td valign="top">
                                                                    <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                                    
                                                                    </asp:CheckBoxList>--%>
                                                                    <div class="FloatLeft" style="width: 200px;">
                                                                        <rad:RadTreeView ForeColor="#cccccc" Width="200px" runat="server"  
                                                                        ID="CategoryTree" DataFieldID="ID" DataValueField="ID" DataSourceID="SqlDataSource1" 
                                                                         Skin="WebBlue"
                                                                        CheckBoxes="true">
                                                                        <DataBindings>
                                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                             TextField="GroupName" ValueField="ID" Expanded="false" />
                                                                         </DataBindings>
                                                                        </rad:RadTreeView>
                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                                            SelectCommand="SELECT * FROM GroupCategories WHERE ID <= 12 ORDER BY GroupName ASC"
                                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                        </asp:SqlDataSource>
                                                                    </div>
                                                                    
                                                               
                                                            </td>
                                                            <td valign="top">
                                                                <div style=" width: 200px;">
                                                                    <rad:RadTreeView Width="200px" ForeColor="#cccccc" runat="server"  
                                                                    ID="RadTreeView1" DataFieldID="ID" DataValueField="ID" DataSourceID="SqlDataSource2" 
                                                                     Skin="WebBlue" 
                                                                    CheckBoxes="true">
                                                                    <DataBindings>
                                                                         <rad:RadTreeNodeBinding  Checkable="true" ValueField="ID" Checked="false" 
                                                                         TextField="GroupName" Expanded="false" />
                                                                     </DataBindings>
                                                                    </rad:RadTreeView>
                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                                        SelectCommand="SELECT * FROM GroupCategories WHERE ID > 12 ORDER BY GroupName ASC"
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
<%--                                    </ItemTemplate>
                                </rad:RadPanelItem>
                            </Items>
                          
                </rad:RadPanelItem>
                <rad:RadPanelItem Expanded="true" Width="420px" Text="<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(0 Records Found)</div></div>">
                    <Items>
                        <rad:RadPanelItem runat="server" Expanded="false">
                            <Items>
                                <rad:RadPanelItem></rad:RadPanelItem>
                            </Items>
                            <ItemTemplate>
                                <asp:Label runat="server" CssClass="ResultsLabel" ID="ResultsLabel"></asp:Label>
                                <br />

                                  <ctrl:AdSearchElements runat="server" ID="SearchElements2" />
                            </ItemTemplate>
                        </rad:RadPanelItem>
                    </Items>
                    
                </rad:RadPanelItem>
            </Items>
        </rad:RadPanelBar>
      
</ContentTemplate>
</asp:UpdatePanel>
--%>        </div>
<%--    <rad:RadAjaxPanel runat="server" EnableViewState="true">
--%>            
            <div style="float: right;">
                <div style="clear: both;">
                <%--    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
            codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" 
            width="431" height="575" title="banner" wmode="transparent">  
            <param name="movie" value="gallery.swf" />  
            <param name="quality" value="high" />
            <param name="wmode" value="transparent"/>   
                <embed src="gallery.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" 
                type="application/x-shockwave-flash" width="431" height="575" wmode="transparent">
                </embed>
            </object>--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                   <ctrl:Ads ID="Ads1" runat="server" />
                   </ContentTemplate>
                </asp:UpdatePanel>
                </div>
                <div style="padding-left: 5px; width: 419px;">
                    
                   
                </div>
            </div>
<%--    </rad:RadAjaxPanel>
--%></asp:Content>
