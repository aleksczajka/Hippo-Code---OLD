<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="VenueSearch_OLD.aspx.cs" Inherits="VenueSearch" ValidateRequest="false" 
Title="Find local venues | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/VenueSearchElements.ascx" TagName="VenueSearchElements" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ MasterType TypeName="MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<style type="text/css">
        .RadPanelBar_Default .rpExpandable .rpText
        {
            background-position: 120px !important;
        }
</style>
<script type="text/javascript">
    function onPanelItemClicking(sender, eventArgs)       
    {       
        eventArgs.get_domEvent().stopPropagation();       
    }  

</script>
<asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
        <div style="float: left;" class="EventDiv">
        <div style="background-repeat: no-repeat; padding-left: 20px; padding-top: 25px;width: 448px; height: 60px; margin-top: -28px; margin-left: -20px;">
            <asp:Label runat="server" CssClass="SearchHeader" ID="EventName" Text="Search Venues"></asp:Label>
            <div style="margin-top: -5px; line-height: 20px;width: 6; float: right; padding-right: 45px; font-family: Arial; font-size: 11px; color: White;">
                <a style="color: White;text-decoration: none;" href="EnterVenue.aspx">Post a <span style="color: #7CAE0C; font-weight: bold;">Venue</span></a> <br />
                <a style="color: White;text-decoration: none;" href="User.aspx">View your <span style="color: #7CAE0C;font-weight: bold;">Favorite Venues</span> </a>
            </div>
        </div>
        <%--<table width="400px"><tr><td>
            
            </td>
                <td align="right">
                    <a style="color: #1fb6e7; font-family: Arial; font-weight: 
                    bold; font-size: 14px; text-decoration: none;" href="./EnterVenueIntro.aspx">
                    Add a new venue...</a>
                </td>
            </tr>
         </table>--%>
         <div style="width: 420px;">
        <%--<table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td align="right" width="420px">
                    <div align="right" class="EventDiv" style=" font-weight: bold; color: #145769; font-family: Arial; font-size: 12px;">
                        <button id="Button2" runat="server" onserverclick="GoToLogin" style=" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;" 
                        onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" >Add a Venue</button>
                    </div>
                </td>
            </tr>
        </table>--%>
    
    
</div>
<%--<asp:UpdatePanel runat="server" UpdateMode="Conditional">
<ContentTemplate>--%>

<a id="SearchTag"></a>
<%--         <rad:RadPanelBar runat="server" ID="SearchPanel" CollapseAnimation-Duration="50" CollapseAnimation-Type="linear" ExpandAnimation-Type="linear" ExpandAnimation-Duration="100"  CssClass="HandleOverflow" AllowCollapseAllItems="true" 
         Width="420px" ExpandMode="SingleExpandedItem">
            <Items>
                <rad:RadPanelItem Expanded="true" Text="<span style='font-family: Arial; font-size: 20px; color: White;'>Enter Search Criteria: </span>">
                    <Items>
                        <rad:RadPanelItem runat="server" Expanded="true">
                            <Items>
                                <rad:RadPanelItem></rad:RadPanelItem>
                            </Items>
                            <ItemTemplate>
--%>                                <asp:Panel runat="server" DefaultButton="SearchButton">
                                 
                                <table width="400px">
                                
                                    <tr>
                                        <td>
                                            <label><span style="color: #A5C13A; font-weight: bold;">What City, State or Zip are you searching in?</span> </label><br /><br />
                                            <div style="padding-left: 56px;">
                                            <div style="width: 160px;">
                                            <rad:RadComboBox Skin="Black" width="160px" runat="server" OnSelectedIndexChanged="ChangeState" 
                                            DataSourceID="SqlDataSourceCountry" DataTextField="country_name" 
                                            DataValueField="country_id" AutoPostBack="true"  ID="CountryDropDown">
                                            </rad:RadComboBox>
                                            </div>
                                                                        <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                                        ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                                        </div>
                                                                
                                            <rad:RadPanelBar runat="server" CollapseAnimation-Duration="100" CollapseAnimation-Type="linear" ExpandAnimation-Type="linear" ExpandAnimation-Duration="100" OnClientItemClicking="onPanelItemClicking" ID="locationSearchPanel" CssClass="HandleOverflow" 
                                            AllowCollapseAllItems="true" Width="400px" ExpandMode="SingleExpandedItem">
                                                <Items>
                                                    <rad:RadPanelItem Text="<div style='color: #1fb6e7;'>Using City and State</div>">
                                                        <Items>
                                                            <rad:RadPanelItem runat="server" Expanded="false">
                                                                <Items>
                                                                    <rad:RadPanelItem BackColor="#393939"></rad:RadPanelItem>
                                                                </Items>
                                                                <ItemTemplate>
                                                            <table cellspacing="10px" bgcolor="#393939"  width="100%" >
                                                                <tr>
                                                                    
                                                                    <td>
                                                                        <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                                            <rad:RadTextBox EmptyMessage="State" Skin="Black" ID="StateTextBox" Width="100" runat="server"></rad:RadTextBox>
                                                                        </asp:Panel>
                                                                        <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                                            <div style="width: 100px;">
                                                                            <rad:RadComboBox Text="State" EmptyMessage="State" runat="server" 
                                                                            Width="100px" Skin="Black" ID="StateDropDown"></rad:RadComboBox>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td>
                                                                            <rad:RadTextBox EmptyMessage="City" Skin="Black" Width="100" 
                                                                            runat="server" ID="CityTextBox"></rad:RadTextBox>
                                                                    </td>
                                                                </tr>
                                                                
                                                            </table>
                                                        </ItemTemplate>
                                                                </rad:RadPanelItem>
                                                        </Items>
                                                       </rad:RadPanelItem>
                                                       <rad:RadPanelItem Text="<div style='color: #1fb6e7;'>Using Postal Code</div>">
                                                        <Items>
                                                            <rad:RadPanelItem runat="server" Expanded="false">
                                                                <Items>
                                                                    <rad:RadPanelItem BackColor="#393939"></rad:RadPanelItem>
                                                                </Items>
                                                                <ItemTemplate>
                                                                    <table cellspacing="10px" bgcolor="#393939"  width="100%" >
                                                                        <tr>
                                                                            <td width="200px">
                                                                                enter zip <asp:Panel runat="server" ID="USLabelPanel"><label>(Must be 5 digits)</label></asp:Panel>
                                                                            </td>
                                                                            <asp:Panel runat="server" ID="RadiusTitlePanel">
                                                                            <td>
                                                                                radius
                                                                            </td>
                                                                            </asp:Panel>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="ZipTextBox" Width="100px"></asp:TextBox>
                                                                            </td>
                                                                            <asp:Panel runat="server" ID="RadiusDropPanel">
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="RadiusDropDown">
                                                                                    <asp:ListItem Value="0" Text="Just Zip Code (0 miles)"></asp:ListItem>
                                                                                    <asp:ListItem Value="1" Text="1 mile"></asp:ListItem>
                                                                                    <asp:ListItem Value="5" Text="5 miles"></asp:ListItem>
                                                                                    <asp:ListItem Value="10" Text="10 miles"></asp:ListItem>
                                                                                    <asp:ListItem Value="15" Text="15 miles"></asp:ListItem>
                                                                                    <asp:ListItem Value="30" Text="30 miles"></asp:ListItem>
                                                                                    <asp:ListItem Value="50" Text="50 miles"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            </asp:Panel>
                                                                        </tr>
                                                                    </table>
                                                                     
                                                                </ItemTemplate>
                                                                </rad:RadPanelItem>
                                                        </Items>
                                                       </rad:RadPanelItem>
                                                 </Items>
                                             </rad:RadPanelBar>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><br />
                                                        <asp:Panel ID="Panel1" runat="server"><div style="padding-bottom: 5px; padding-top: 10px;"><label><span style="color: #A5C13A; font-weight: bold;">Give us some distinguishing keywords (ex: part of name)</span></label></div><div style="padding-left: 20px;"><ctrl:HippoTextBox runat="server" ID="SearchTextBox" TEXTBOX_WIDTH="200" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
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
                                            <%--<button id="Button1" runat="server" onserverclick="Search" 
                                            onclick="this.innerHTML = 'Working...';this.disabled=true;this.style.backgroundImage = 'url(image/PostButtonNoPostHover.png)';" 
                                            style=" cursor: pointer;font-weight: bold;padding:0;padding-bottom: 4px; font-size: 11px;height: 30px; width: 112px;background-color: transparent; color: White; background-image: url('image/PostButtonNoPost.png'); background-repeat:no-repeat; border: 0;" 
                                            onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                            onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'" >Search</button>--%>
                                    
                                        <div style="padding-bottom: 5px;">
                                           <asp:Label runat="server" ID="MessageLabel" CssClass="AddLink" ForeColor="Red"></asp:Label>
                                           </div>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3">
                                            <label>Lowest Rating:</label>
                                        </td>
                                    </tr>
                                    <tr>
                                                        
                                        <td colspan="3">
                                            <div style="width: 105px;padding-left: 56px;">
                                                <rad:RadComboBox EmptyMessage="Lowest Rating" runat="server" Width="105" Skin="Black" ID="RatingDropDown" >
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
                                            </div>
                                        </td>
                                      
                                    </tr>--%>
                                    <tr>
                                        <td colspan="3" style="padding-top: 10px; padding-bottom: 5px;">
                                            <div style="padding-bottom: 5px;"><label><span style="color: #A5C13A; font-weight: bold;">What type of Venue/Restaurant are you looking for?</span></label></div>
                                            <div class="EventDiv" style="border: solid 3px #628e02;background-color: #333333; width: 420px; padding-left: 5px; padding-top: 5px; color: #cccccc; min-height: 200px;">
                                                <table>
                                                    <tr>
                                                        <td valign="top">
                                                                <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                                RepeatColumns="4" RepeatDirection="horizontal">
                                                                
                                                                </asp:CheckBoxList>--%>
                                                                <div class="FloatLeft" style="width: 150px;">
                                                                    <rad:RadTreeView  Width="150px" runat="server"  
                                                                    ID="CategoryTree" DataFieldID="ID" ForeColor="#cccccc" DataSourceID="SqlDataSource1" 
                                                                    DataFieldParentID="ParentID" Skin="WebBlue"
                                                                    CheckBoxes="true">
                                                                    <DataBindings>
                                                                         <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                         TextField="Name" Expanded="false" ValueField="ID" />
                                                                     </DataBindings>
                                                                    </rad:RadTreeView>
                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                                        SelectCommand="SELECT * FROM VenueCategories WHERE ID <= 92 ORDER BY Name ASC"
                                                                        ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                    </asp:SqlDataSource>
                                                                </div>
                                                                
                                                           
                                                        </td>
                                                        <td valign="top">
                                                            <div style=" width: 150px;">
                                                                <rad:RadTreeView Width="150px" ForeColor="#cccccc" runat="server"  
                                                                ID="RadTreeView1" DataFieldID="ID" DataSourceID="SqlDataSource2" 
                                                                DataFieldParentID="ParentID"  Skin="WebBlue"
                                                                CheckBoxes="true">
                                                                <DataBindings>
                                                                     <rad:RadTreeNodeBinding  Checkable="true" ValueField="ID" Checked="false" 
                                                                     TextField="Name" Expanded="false" />
                                                                 </DataBindings>
                                                                </rad:RadTreeView>
                                                                <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                                    SelectCommand="SELECT * FROM VenueCategories WHERE ID > 92 ORDER BY Name ASC"
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
<%--                            </ItemTemplate>
                        </rad:RadPanelItem>
                    </Items>
                </rad:RadPanelItem>
--%>                <%--<rad:RadPanelItem Width="420px" Text="<div style='position: relative;'><span style='font-family: Arial; font-size: 20px; color: White;'>Search Results: </span><div style='position: absolute; right: 5px; top: 3px;'>(0 Records Found)</div></div>">
                    <Items>
                        <rad:RadPanelItem runat="server" Expanded="false">
                            <Items>
                                <rad:RadPanelItem></rad:RadPanelItem>
                            </Items>
                            <ItemTemplate>
                                    <asp:Label runat="server" CssClass="ResultsLabel" ID="ResultsLabel"></asp:Label>
                                <br />
                                  <ctrl:VenueSearchElements runat="server" ID="SearchElements2" />
                            </ItemTemplate>
                        </rad:RadPanelItem>
                    </Items>
                    
                </rad:RadPanelItem>
            </Items>
        </rad:RadPanelBar>--%>
        
<%--</ContentTemplate>
</asp:UpdatePanel>--%>
        </div>

              
            
       
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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                 <ctrl:Ads ID="Ads1" runat="server" />
                 </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div style="padding-left: 5px; width: 419px;">
                
               
            </div>
        </div>
</asp:Content>

