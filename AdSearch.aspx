<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" CodeFile="AdSearch.aspx.cs" EnableEventValidation="false" 
Inherits="AdSearch" Title="Search local bulletins | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Src="~/Controls/AdSearchElements.ascx" TagName="SearchElements" TagPrefix="ctrl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/LocDetect.ascx" TagName="LocDetect" TagPrefix="ctrl" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ Register TagName="GreenButton" TagPrefix="ctrl" Src="~/Controls/GreenButton.ascx" %>
<%@ Register TagName="SmallButton" TagPrefix="ctrl" Src="~/Controls/SmallButton.ascx" %>
<%@ Register TagName="Pager" TagPrefix="ctrl" Src="~/Controls/Pager.ascx" %>
<%@ MasterType TypeName="MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="true" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false" Skin="Vista" 
                BorderStyle="solid"  Behaviors="none"
            ID="MessageRadWindowManager" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose"  
                    EnableEmbeddedSkins="true" VisibleTitlebar="false"
                    VisibleStatusbar="false" Skin="Web20" Height="300" ID="MessageRadWindow" 
                    Title="Alert" runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
<asp:Literal runat="server" ID="ScriptLiteral"></asp:Literal>
    <script type="text/javascript">
        function OpenAdvanced()
        {
            var d = document.getElementById('AdvancedDiv');
            var c = document.getElementById('ArrowDiv');
            
            if(d.style.display == 'block')
            {
                d.style.display = 'none';
                c.innerHTML= '&rArr;';
            }
            else
            {
                d.style.display = 'block';
                c.innerHTML = '&dArr;';
            }
        }
        function OpenCategories()
        {
            var d = document.getElementById('FilterDiv');
            var c = document.getElementById('OpenDiv');
            var nameDiv = document.getElementById('NameDiv');
            
            if(d.style.display == 'block')
            {
                d.style.display = 'none';
                c.innerHTML= '&lArr; expand &rArr;';
                nameDiv.style['border'] = 'solid 1px #c9c4c4';
                nameDiv.style['padding'] = '2px';
                
                setCookie("Collapsed",true,1);
            }
            else
            {
                d.style.display = 'block';
                c.innerHTML = '&rArr; collapse &lArr;';
                nameDiv.style['border'] = '';
                nameDiv.style['padding'] = '3px';
            }
        }
        
        function CheckCollapsed()
        {
            var address = getCookie('Collapsed');
            
            if(address != null && address != undefined)
            {
                if(address == "true")
                {
                    var d = document.getElementById('FilterDiv');
                    var c = document.getElementById('OpenDiv');
                    var nameDiv = document.getElementById('NameDiv');
                    d.style.display = 'none';
                    c.innerHTML= '&lArr; expand &rArr;';
                    nameDiv.style['border'] = 'solid 1px #c9c4c4';
                    nameDiv.style['padding'] = '2px';
                }
            }
        }
       function OnClientClose(oWnd, args)
        {
            if(args != null && args != undefined)
                window.location = args;
        }
        
        var idArray = new Array();
        var n = 0;
        function GetRadWindow() 
        { 
          var oWindow = null; 
          if (window.radWindow) 
             oWindow = window.radWindow; 
          else if (window.frameElement.radWindow) 
             oWindow = window.frameElement.radWindow; 
          return oWindow; 
        }   
        function CloseWindow(returnV)
        {
        
            var oWindow = GetRadWindow(); 
            oWindow.Close(returnV); 
        }
        
        function GoToThis(goHere)
        {
            window.location = goHere;
        }
      
        function Search() 
        { 
            CloseWindow();
        }
        
    </script>
    
 <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>  
            
            <ctrl:LocDetect runat="server" />
            <div class="topDiv Text12" style="width: 860px;">
                <div style="clear: both;">
                    <div style="float: left;"><img src="NewImages/BackLeftTopCorner.png" /></div>
                    <div style=" height: 15px;width: 828px;float: left; background: url('NewImages/BackBorder.png'); background-repeat: repeat-x; background-position: top;">
                        &nbsp;
                    </div>
                    <div style="float: left;"><img src="NewImages/BackRightTopCorner.png" /></div>
                </div>
                <div style="clear: both; background-color: White;"> 
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="15px" style="background-image: url('NewImages/BackLeftBorder.png'); background-repeat: repeat-y;"></td>
                            <td>
                                <div class="topDiv" style="width: 820px; background-color: White; font-size: 18px; padding-left: 10px;padding-bottom: 130px;">
                                
                                    <div class="topDiv Text12" style="min-height: 600px;">
                                        <div style="margin-left: -8px;min-height: 600px;float: left; padding-right: 10px; margin-right: 10px; border-right: solid 1px #dedbdb; width: 365px;">
                                            <div class="topDiv TextNormal">
                                                <div style="float: left;">
                                                    <asp:Label runat="server" ID="EventTitle">Search Bulletins</asp:Label>
                                                </div>
                                                <div style="float: right; width: 150px;">
                                                    <div style="float: left;">
                                                        <table width="100%"><tr>
                                                        <td width="127px">
                                                            <ctrl:GreenButton WIDTH="127px" runat="server" ID="SaveSearchButton" BUTTON_TEXT="Save Your Search" />
                                                        <td width="10px">
                                                        <asp:Image runat="server" CssClass="HelpImage" ID="QuestionMark6" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                                         <rad:RadToolTip ID="RadToolTip5" runat="server" Skin="Sunset" Width="200px" Height="200px" 
                                                         ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" 
                                                         TargetControlID="QuestionMark6">
                                                            <div style="padding: 10px;"><label>
                                                            Saving your bulletin search means you will be able 
                                                            to quickly access your search from your My Account page. You won't have to re-type 
                                                            all your search criteria each time you want to find new bulletins. In addition, 
                                                            as a special feature for bulletin searches, you can choose to have an email of new bulletins 
                                                            sent to you as they come into our database. This will save you the trouble of 
                                                            visiting the site over and over to check for new Bulletins.
                                                            </label></div>
                                                        </rad:RadToolTip>
                                                        </td>
                                                        </tr></table>
                                                     </div> 
                                                </div>
                                            </div>
                                            <asp:Panel ID="Panel2" runat="server" DefaultButton="fakeSearchButton">
                                            <div class="topDiv">
                                            <div style="display: none;"><asp:Button runat="server" OnClick="SearchButton_Click" ID="fakeSearchButton" />
                                                        </div>
                                                <div style="float: left;padding-left: 55px;">
                                                    <rad:RadTextBox BorderColor="#09718F" runat="server" EmptyMessage="Keywords" ID="KeywordsTextBox" Width="209px"></rad:RadTextBox>
                                                </div>
                                                <div style="float: left; padding-left: 4px;">
                                                    <ctrl:SmallButton ID="SearchButton" runat="server" />
                                                </div>
                                            </div>
                                            <div class="topDiv">
                                                <asp:Label runat="server" ID="MessageLabel" ForeColor="Red"></asp:Label>
                                                <h1 class="SideColumn"><a onclick="OpenAdvanced();"><span style="text-decoration: underline;">Change Location <span id="ArrowDiv">&rArr;</span></span></a></h1>
                                                <div id="AdvancedDiv" style="display: none; padding-bottom: 20px;">
                                                    <div style="padding-bottom: 10px; border: solid 1px #c9c4c4;">
                                                        <div class="topDiv">
                                                            <div style="float: left; width: 150px; padding:5px;clear: both;">
                                                                    <rad:RadComboBox Skin="WebBlue" runat="server" OnSelectedIndexChanged="ChangeState" 
                                                                        DataSourceID="SqlDataSourceCountry" DataTextField="country_name" 
                                                                        DataValueField="country_id" Width="150px" AutoPostBack="true"  ID="CountryDropDown">
                                                                        </rad:RadComboBox>
                                                                
                                                                    <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                                                    ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                        
                                                            </div>    
                                                            <div style="float: right;">
                                                            <div style="padding-right: 5px;padding-top: 5px;">
                                                                <table cellspacing="0"  width="100%" >
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                                                <rad:RadTextBox EmptyMessage="State" Skin="WebBlue"  ID="StateTextBox" Width="100" runat="server"></rad:RadTextBox>
                                                                            </asp:Panel>
                                                                            <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                                                <div style="width: 100px;">
                                                                                <rad:RadComboBox Text="State" EnableItemCaching="true" EmptyMessage="State" runat="server" 
                                                                                Width="100px" Skin="WebBlue" AutoPostBack="true" OnSelectedIndexChanged="ChangeCity" ID="StateDropDown"></rad:RadComboBox>
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </td>
                                                                        <td align="right">
                                                                                <%--<rad:RadTextBox EmptyMessage="City" Skin="WebBlue" Width="100" 
                                                                                runat="server" ID="CityTextBox"></rad:RadTextBox>--%>
                                                                                
                                                                            <asp:Panel runat="server" ID="CityTextBoxPanel">
                                                                                <asp:TextBox ID="CityTextBox" runat="server" Width="80px" ></asp:TextBox>
                                                                            </asp:Panel>
                                                                            <asp:Panel runat="server" ID="CityDropDownPanel" Visible="false">
                                                                                <asp:DropDownList runat="server" ID="MajorCityDrop"></asp:DropDownList>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                </table>
                                                                <%--<div align="center" style="padding-top: 5px; padding-bottom: 5px;">
                                                                <img src="NewImages/or.png" />
                                                                </div>
                                                                <table cellspacing="0" style=" padding-bottom: 10px;"  width="100%" >
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
                                                                </table>--%>
                                                            </div>
                                                            </div>
                                                            
                                                        </div>
                                                        <div class="topDiv">
                                                             <div style="float: right; padding-right: 7px; padding-top: 20px;">
                                                                <ctrl:SmallButton ID="SmallButton1" runat="server" />
                                                             </div>
                                                        </div>
                                                    </div>
                                                    
                                                </div>
                                            </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="topDiv" style="float: left;width: 435px;">
                                        
                                            <div>
                                                <asp:Label runat="server" ID="TimeFrameLabel"></asp:Label>
                                                        <div style="padding-bottom: 20px;">
                                                <div id="NameDiv" class="topDiv" style="padding: 3px;">
                                                    <div style="float: left;"><h2>Filter your results on categories</h2></div>
                                                    <h2><div style="float: right;cursor: pointer; text-decoration: underline;" id="OpenDiv" onclick="OpenCategories();">&rArr; collapse &lArr;</div></h2>
                                                </div>
                                                <div id="FilterDiv" style="display: block;">
                                                    <div style="border: solid 1px #c9c4c4; ">
                                                        <table>
                                                                                <tr>
                                                                                    <td valign="top">
                                                                                            <div class="FloatLeft">
                                                                                                <rad:RadTreeView  Width="140px" runat="server"  
                                                                                                ID="CategoryTree" DataFieldID="ID" ForeColor="#cccccc" DataSourceID="SqlDataSource1" 
                                                                                                DataFieldParentID="ParentID"  Skin="Vista"
                                                                                                CheckBoxes="true">
                                                                                                <DataBindings>
                                                                                                     <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                                                                     TextField="Name" Expanded="false" ValueField="ID" />
                                                                                                 </DataBindings>
                                                                                                </rad:RadTreeView>
                                                                                                <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                                                                    SelectCommand="SELECT * FROM AdCategories WHERE ((ParentID IN (SELECT ID FROM AdCategories WHERE (Name < 'Fi') AND (ParentID IS NULL))) OR ((Name < 'Fi') AND (ParentID IS NULL))) ORDER BY Name ASC"
                                                                                                    ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                                </asp:SqlDataSource>
                                                                                            </div>
                                                                                            
                                                                                       
                                                                                    </td>
                                                                                    <td valign="top">
                                                                                        <div style=" width: 144px;">
                                                                                            <rad:RadTreeView Width="144px" ForeColor="#cccccc" runat="server"  
                                                                                            ID="RadTreeView1" DataFieldID="ID" DataSourceID="SqlDataSource2" 
                                                                                            DataFieldParentID="ParentID"  Skin="Vista"
                                                                                            CheckBoxes="true">
                                                                                            <DataBindings>
                                                                                                 <rad:RadTreeNodeBinding  Checkable="true" ValueField="ID" Checked="false" 
                                                                                                 TextField="Name" Expanded="false" />
                                                                                             </DataBindings>
                                                                                            </rad:RadTreeView>
                                                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource2" 
                                                                                                SelectCommand="SELECT * FROM AdCategories WHERE  ((ParentID IN (SELECT ID FROM AdCategories WHERE (Name >= 'Fi' AND Name <= 'Po') AND (ParentID IS NULL))) OR ((Name >= 'Fi' AND Name <= 'Po') AND (ParentID IS NULL))) ORDER BY Name ASC"
                                                                                                ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                            </asp:SqlDataSource>
                                                                                        </div>
                                                                                    </td>
                                                                                    <td valign="top">
                                                                                        <div style=" width: 136px;">
                                                                                            <rad:RadTreeView Width="136px" ForeColor="#cccccc" runat="server"  
                                                                                            ID="RadTreeView2" DataFieldID="ID" DataSourceID="SqlDataSource3" 
                                                                                            DataFieldParentID="ParentID"  Skin="Vista"
                                                                                            CheckBoxes="true">
                                                                                            <DataBindings>
                                                                                                 <rad:RadTreeNodeBinding  Checkable="true" ValueField="ID" Checked="false" 
                                                                                                 TextField="Name" Expanded="false" />
                                                                                             </DataBindings>
                                                                                            </rad:RadTreeView>
                                                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                                                                SelectCommand="SELECT * FROM AdCategories WHERE  ((ParentID IN (SELECT ID FROM AdCategories WHERE  (Name > 'Po') AND (ParentID IS NULL))) OR ((Name > 'Po') AND (ParentID IS NULL))) ORDER BY Name ASC"
                                                                                                ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                                                            </asp:SqlDataSource>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                    </div>
                                                    <div class="topDiv">
                                                    <div style="float: right;padding-top: 5px; width: 170px;">
                                                            <%--<asp:LinkButton runat="server" CssClass="NavyLink12" Text="Filter" OnClick="FilterResults"></asp:LinkButton>--%>
                                                            <div style="float: left; padding-top: 7px; padding-right: 10px;">
                                                                <asp:LinkButton ID="LinkButton1" CssClass="NavyLink12" runat="server" Text="Clear Filter" OnClick="ClearFilter"></asp:LinkButton>
                                                            </div>
                                                            <div style="float: left;">
                                                                <ctrl:BlueButton runat="server" WIDTH="100px" ID="FilterButton" BUTTON_TEXT="Filter Results" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <script type="text/javascript">
                                                    CheckCollapsed();
                                                </script>
                                            </div>
                                                           
                                                        
                                                        <div style="float: left; width: 420px; ">
                                                            <asp:Label runat="server" CssClass="ResultsLabel" ID="ResultsLabel"></asp:Label>
                                                                     <div class="topDiv" style="width: 420px;padding-bottom: 10px;padding-top: 10px;"> 
                                                                             <div style="float: left;">  <asp:DropDownList Visible="false" runat="server" AutoPostBack="true"  
                                                                            OnSelectedIndexChanged="SortResults" ID="SortDropDown">
                                                                                <asp:ListItem Text="Sort By..." Value="-1"></asp:ListItem>
                                                                                <asp:ListItem Text="Bulletin Name Up" Value="Header ASC"></asp:ListItem>
                                                                                <asp:ListItem Text="Bulletin Name Down" Value="Header DESC"></asp:ListItem>
                                                                            </asp:DropDownList>   
                                                                            </div>
                                                                            <div style="padding-left: 20px; float: right">
                                                                                <asp:Label runat="server" ID="NumsLabel" CssClass="Green12Link"></asp:Label>
                                                                            </div>
                                                                     </div>   
                                                                    <div class="topDiv">
                                                                        <asp:Panel runat="server" ID="Panel7" Width="440px"><asp:Label runat="server" ForeColor="Red" ID="Label1"></asp:Label>
                                                                                <asp:Literal runat="server" ID="HelpUsLiteral"></asp:Literal>
                                                                              <ctrl:SearchElements Visible="false" runat="server" ID="VenueSearchElements" />
                                                                              
                                                                         </asp:Panel>
                                                                    </div>
                                                                    </div>
                                                <asp:Panel runat="server" ID="NoResultsPanel" Visible="false">
                                                    <label>There are no bulletins matching your criteria, but, you can post them 
                                                    through our <a class="NavyLink12" href="post-bulletin">Post a Bulletin Page</a></label>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="GooglySearch" align="center">
                                            <script type="text/javascript"><!--
                                                google_ad_client = "ca-pub-3961662776797219";
                                                /* Hippo Link Unit */
                                                google_ad_slot = "6130116935";
                                                google_ad_width = 728;
                                                google_ad_height = 15;
                                                //-->
                                                </script>
                                                <script type="text/javascript"
                                                src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
                                                </script>
                                        </div>
                                    </div>
                                    
                                </div>
                            </td>
                            <td width="15px" style="background-image: url('NewImages/BackRightBorder.png'); background-repeat: repeat-y;"></td>
                        </tr>
                    </table> 
                </div>
                              
                <div style="clear: both;">
                    <div style="float: left;"><img src="NewImages/BackLeftBottomCorner.png" /></div>
                    <div style="width: 830px;float: left; background: url('NewImages/BackBottomBorder.png'); background-repeat: repeat-x; background-position: top;">
                        &nbsp;
                    </div>
                    <div style="float: left;"><img src="NewImages/BackRightBottomCorner.png" /></div>
                </div>
           </div>
           <div class="topDiv" style="width: 847px;clear: both;margin-left: 7px;position: relative;top: -123px;">
                                        <div style="clear: both;" class="topDiv">
                                            <div style="float: left;width: 6px;"><img src="NewImages/EventFooterTopLeft.png" /></div>
                                            <div style=" height: 6px;width: 835px;float: left; background: url('NewImages/EventFooterTop.png'); background-repeat: repeat-x; background-position: top;">
                                                &nbsp;
                                            </div>
                                            <div style="float: left;width: 6px;"><img src="NewImages/EventFooterTopRight.png" /></div>
                                        </div>
                                        <div class="topDiv" style="clear: both; background-color: #ebe7e7;"> 
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="6px" style="background-image: url('NewImages/EventFooterLeft.png'); background-repeat: repeat-y;"></td>
                                                    <td>
                                                        <div class="Text12" style="width: 835px; background-color: #ebe7e7;">
                                                            <div class="ContentFooter">
                                                                <b>Hippo Bulletin Tips</b>                                                                <ol>                                                                    <li>                                                                        Earn points toward being featured on our home page by posting bulletins whether featured or non-featured.                                                                     </li>                                                                    <li>                                                                        Post, Share, Text, Email.                                                                     </li>                                                                    <li>                                                                        Save bulletin searches and have them emailed to you when new bulletins arrive matching your criteria.                                                                    </li>                                                                    <li>                                                                        Post a featured ad for as low as $3/day.                                                                    </li>                                                                </ol>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td width="6px" style="background-image: url('NewImages/EventFooterRight.png'); background-repeat: repeat-y;"></td>
                                                </tr>
                                            </table>                       
                                        </div>
                                        <div style="clear: both;" class="topDiv">
                                            <div style="float: left;width: 6px;"><img src="NewImages/EventFooterBottomLeft.png" /></div>
                                            <div style="height: 6px;width: 835px;float: left; background: url('NewImages/EventFooterBottom.png'); background-repeat: repeat-x; background-position: top;">
                                                &nbsp;
                                            </div>
                                            <div style="float: left;width: 6px;"><img src="NewImages/EventFooterBottomRight.png" /></div>
                                            </div>
                                    </div>
<asp:Literal runat="server" ID="TopLiteral"></asp:Literal>
     <asp:Literal runat="server" ID="BottomScriptLiteral"></asp:Literal>
</asp:Content>

