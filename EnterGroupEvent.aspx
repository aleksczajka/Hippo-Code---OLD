<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
AutoEventWireup="true" EnableEventValidation="false" 
CodeFile="EnterGroupEvent.aspx.cs" 
Inherits="EnterGroupEvent" Title="Enter a Group Event | HippoHappenings" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="HippoTextBox" TagPrefix="ctrl" Src="~/Controls/HippoTextBox.ascx" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<rad:RadAjaxPanel runat="server" >--%>
<%--<asp:UpdatePanel runat="server" >
<Triggers>
    <asp:PostBackTrigger ControlID="ImageButton6" />
    <asp:PostBackTrigger ControlID="ImageButton5" />
</Triggers>--%>
<%--<ContentTemplate>--%>
 <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
            ID="MessageRadWindowManager" ReloadOnShow="true" Modal="true" RestrictionZoneID="RestrictionZone"
            runat="server">
                <Windows>
                    <rad:RadWindow Width="300" ClientCallBackFunction="OnClientClose" EnableEmbeddedSkins="true" 
                    VisibleStatusbar="false" Skin="Black" Height="300" ID="MessageRadWindow" Title="Alert" runat="server">
                    </rad:RadWindow>
                     <rad:RadWindow Width="400" ClientCallBackFunction="FillVenueJS" 
                    VisibleStatusbar="false" Skin="Black" Height="500" ID="RadWindow1" 
                     runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
            <script type="text/javascript">
            function OnClientClose(oWnd, args)
            {
                window.location = args;
            }
            function OpenFriends()
                {
                    var win = $find("<%=RadWindow1.ClientID %>");
                    win.setUrl('SelectFriendMembers.aspx');
                    win.show(); 
                    win.center(); 
                 }
                 function OpenMembers()
                {
                    var win = $find("<%=RadWindow1.ClientID %>");
                    win.setUrl('SearchMembers.aspx');
                    win.show(); 
                    win.center(); 
                 }
            function FillFriends(retV, returnValue)
                { 
                   __doPostBack('ctl00$ContentPlaceHolder1$MemberInvitesButton', '');
                }
            </script>
            <script type="text/javascript">
                                                   function fillDrop(theText, theID)
                                                   {
                                                        var theDiv = document.getElementById('ctl00_ContentPlaceHolder1_TimeFrameDiv');
                                                        theDiv.innerHTML = theText.replace("&&", "'");
                                                        
                                                        var theidDiv = document.getElementById('ctl00_ContentPlaceHolder1_VenueIDDIV');
                                                        theidDiv.innerHTML = theID;
                                                        
                                                        var tooltip = Telerik.Web.UI.RadToolTip.getCurrent();
                                                        if (tooltip) tooltip.hide();
                                                        
                                                        __doPostBack('ctl00$ContentPlaceHolder1$LinkButton1',theID);
                                                   }
                                                   
                                                   function HideAllDivs()
                                                   {
                                                        var theDiv = document.getElementById('contentDivA');
                                                        
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivA');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivB');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivB');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivC');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivC');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivD');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivD');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivE');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivE');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivF');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivF');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivG');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivG');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivH');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivH');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivI');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivI');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivJ');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivJ');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivK');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivK');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivL');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivL');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivM');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivM');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivN');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivN');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivO');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivO');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivP');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivP');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivQ');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivQ');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivR');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivR');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivS');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivS');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivT');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivT');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivU');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivU');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivV');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivV');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivW');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivW');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivX');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivX');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivY');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivY');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                        
                                                        theDiv = document.getElementById('contentDivZ');
                                                        if(theDiv != null && theDiv != undefined)
                                                        {
                                                            theDiv.style.display = 'none';
                                                            
                                                            theDiv = document.getElementById('titleDivZ');
                                                            theDiv.className = 'AddWhiteLink';
                                                        }
                                                   }
                                                   
                                                   function SelectLetterDiv(letter)
                                                   {
                                                    HideAllDivs();
                                                    var theDiv = document.getElementById('contentDiv'+letter);
                                                    theDiv.style.display = 'block';
                                                    
                                                    theDiv = document.getElementById('titleDiv'+letter);
                                                    theDiv.className = "AddGreenLink";
                                                   }
                                                   
                                                   function OpenCreateVenue()
                                                   {
                                                        var win = $find("<%=RadWindow1.ClientID %>");
                                                        win.setUrl('CreateVenue.aspx');
                                                        win.show(); 
                                                        win.center(); 
                                                   }
                                                   function FillVenueJS(returnV, returnValue)
                                                   {
                                                        __doPostBack('ctl00$ContentPlaceHolder1$GetNewVenueButton','');
                                                   }
                                                   </script>
<a id="top"></a>
<div style="padding-bottom: 100px;">
    <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
    <asp:Panel runat="server" ID="LoggedOutPanel">
        <div id="topDiv5">
            <div style="float: left; width: 390px;">
               
                            <span style="font-family: Arial; font-size: 30px; color: #cccccc;">Enter Group Event</span>
             
                <br /><br />
                <span style="font-family: Arial; font-size: 12px; color: #cccccc; line-height: 20px;">
            <asp:Label runat="server" ID="WelcomeLabel"></asp:Label>
          </span>
            </div>
            <div style="float: right;">
      
            <ctrl:Ads runat="server" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="BigEventPanel">
    <div style="font-family: Arial; font-size: 30px; color: #cccccc; padding-bottom: 10px;">
        <asp:Label runat="server" ID="TitleLabel" Text="Enter Group Event"></asp:Label>
        <asp:Label runat="server" ID="UserNameLabel" Visible="false"></asp:Label>
    </div>
    <div id="topDiv6">
        <div style="float: left; width: 680px;">
        <div style="margin-left: 11px;">
            <rad:RadTabStrip runat="server" ID="EventTabStrip"
            Skin="HippoHappenings" CssClass="EventTabStrip" OnTabClick="TabClick"  Height="40px"
            EnableEmbeddedSkins="false" MultiPageID="BlogEventPages" SelectedIndex="0">
                <Tabs>
                    <rad:RadTab runat="server" PageViewID="TabOne" ImageUrl="images/DetailsBlueFull.gif" 
                    HoveredImageUrl="images/DetailsBlueFull.gif" TabIndex="0" 
                    DisabledImageUrl="images/DetailsBlueFull.gif" 
                    SelectedImageUrl="images/DetailsBlueFull.gif">
                    </rad:RadTab>
                    <rad:RadTab runat="server" Enabled="false"  PageViewID="RadPageView1" 
                    ImageUrl="images/Media2PurpleFull.gif" 
                    HoveredImageUrl="images/Media2PurpleFull.gif"  TabIndex="1"  
                    DisabledImageUrl="images/Media2PurpleEmpty.gif" 
                    SelectedImageUrl="images/Media2PurpleFull.gif" >
                    </rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView2" 
                    ImageUrl="images/Internals2BlueFull.gif" 
                    HoveredImageUrl="images/Internals2BlueFull.gif"  TabIndex="2" 
                    DisabledImageUrl="images/Internals2BlueEmpty.gif" 
                    SelectedImageUrl="images/Internals2BlueFull.gif" ></rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView5" 
                    ImageUrl="images/Participants4PurpleFull.gif" 
                    HoveredImageUrl="images/Participants4PurpleFull.gif"  TabIndex="3" 
                    DisabledImageUrl="images/Participants4PurpleEmpty.gif" 
                    SelectedImageUrl="images/Participants4PurpleFull.gif" ></rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView3" 
                    ImageUrl="images/Categories5BlueFull.gif" 
                    HoveredImageUrl="images/Categories5BlueFull.gif"  TabIndex="4" 
                    DisabledImageUrl="images/Categories5BlueEmpty.gif" 
                    SelectedImageUrl="images/Categories5BlueFull.gif" ></rad:RadTab>
                    <rad:RadTab runat="server"  Enabled="false"  PageViewID="RadPageView4" 
                    ImageUrl="images/PostItPurpleFull.gif" 
                    HoveredImageUrl="images/PostItPurpleFull.gif" 
                    DisabledImageUrl="images/PostItPurpleEmpty.gif"  TabIndex="5" 
                    SelectedImageUrl="images/PostItPurpleFull.gif" ></rad:RadTab>
                </Tabs>
            </rad:RadTabStrip>
        </div>
        <rad:RadMultiPage runat="server" ID="BlogEventPages" SelectedIndex="0">
            <rad:RadPageView runat="server" ID="TabOne" TabIndex="0" >
                
                <asp:Panel runat="server" ID="Panel1" DefaultButton="DetailsOnwardsButton">
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div id="topDiv" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y; width: 668px; padding-top: 10px;">
                        <div class="EventDiv" style="padding-left: 30px;float: left;">
                        <h1>Details</h1>
                            <table>
                                <tr>
                                    <td width="250px;" valign="top">
                                    <label>All HTML tags will be removed except for links. However, the header cannot contain links at all.</label>
                                            <br /><br />
                                            <script type="text/javascript">
                                                    
                                                function CountCharsHeadline(editor, e){
                                                    
                                                        var theDiv = document.getElementById("CharsDivHeadline");
                                                        var theText = document.getElementById("ctl00_ContentPlaceHolder1_VenueNameTextBox_TheTextBox");
                                                        theDiv.innerHTML = "characters left: "+ (70 - theText.value.length).toString();
                                                    
                                                }
                                            </script>
                                        <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Name</label><br />
                                        <span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;"> 
                                        max 70 characters  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;<span style="color: Red;" id="CharsDivHeadline"></span></span><br />
                                        <ctrl:HippoTextBox runat="server" ON_CLIENT_KEYPRESS="CountCharsHeadline(event)"  ID="VenueNameTextBox" TEXTBOX_WIDTH="200" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                       <br /><br /><br />
                                        
                               <br />
                                
                                                            
                                                   
                                       </td>
                                     <td style="padding-left: 50px;" valign="top">
                                     <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Description</label><br /><span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;"> max 420 characters
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <span style="color: Red;" id="CharsDivEditor"></span>
                                      <rad:RadEditor EditModes="Design" OnClientLoad="OnClientLoad" 
                                                    Skin="Black" runat="server" 
                                                    ID="DescriptionTextBox" Height="200px" Width="240px"
                                                        EnableResize="False"  ToolsFile="toolsfile.xml" 
                                                        StripFormattingOptions="MSWordRemoveAll" 
                                                        StripFormattingOnPaste="MSWordRemoveAll">
                                                        <CssFiles>
                                                        <rad:EditorCssFile Value="contentarea.css" />
                                                       </CssFiles>
                                                        <Tools>
                                                           <rad:EditorToolGroup>
                                                               <rad:EditorTool Name="InsertLink" Text="Insert Link" />
                                                           </rad:EditorToolGroup>
                                                       </Tools>
                                                    </rad:RadEditor>
                                                    <script type="text/javascript">
                                                        Telerik.Web.UI.Editor.CommandList["InsertLink"] = function(commandName, editor, args)
                                                        {
                                                          var elem = editor.getSelectedElement(); //returns the selected element.
                                                                
                                                          if (elem.tagName == "A")
                                                          {
                                                               editor.selectElement(elem);
                                                               argument = elem;
                                                          }
                                                          else
                                                          {
                                                             var content = editor.getSelectionHtml();
                                                             var link = editor.get_document().createElement("A");
                                                             link.innerHTML = content;         
                                                             argument = link;
                                                          }

                                                         
                                                          var myCallbackFunction = function(sender, args)
                                                          {
                                                               
                                                                editor.pasteHtml(String.format("<a target='_blank' class='AddLink' href='{1}'>{0}</a>", args.name, args.url))
                                                          }
                                                          editor.showExternalDialog(
                                                               'InsertLink.html',
                                                               argument,
                                                               270,
                                                               300,
                                                               myCallbackFunction,
                                                               null,
                                                               'Insert Link',
                                                               true,
                                                               Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
                                                               false,
                                                               false);
                                                        };
                                                    </script>
                                                    <script type="text/javascript">
                                                            
                                                                function OnClientLoad(editor)
                                                                {
                                                                    editor.attachEventHandler("onkeyup", CountCharsEditor);
                                                                }  
                                                                function CountCharsEditor(editor)
                                                                {                                                                    
                                                                    var editor = (editor) ? editor : ((event) ? event : null);
                                                                    var node = (editor.target) ? editor.target : ((editor.srcElement) ? editor.srcElement : null);
                                                                    
                                                                    var theEditorBig = document.getElementById("ctl00_ContentPlaceHolder1_DescriptionTextBox_contentIframe");
                                                                    var theDiv = document.getElementById("CharsDivEditor");
                                                                    theDiv.innerHTML = navigator.appCodeName;
                                                                    if(navigator.appName == 'Netscape')
                                                                    {
                                                                        if(this.activeElement.textContent.length > 420)
                                                                            theDiv.innerHTML = "characters left: 0";
                                                                        else
                                                                            theDiv.innerHTML = "characters left: "+ (420 - this.activeElement.textContent.length).toString();
                                                                    }
                                                                    else
                                                                    {
                                                                        var theEditor = $find("<%=DescriptionTextBox.ClientID %>");
                                                                        if(theEditor.get_contentArea().innerText.length > 420)
                                                                            theDiv.innerHTML = "characters left: 0";
                                                                        else
                                                                            theDiv.innerHTML = "characters left: "+ (420 - theEditor.get_contentArea().innerText.length).toString();
                                                                    }
                                                                }
                                                            </script>
                                    
                                     </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-top: solid 1px #666666;" width="613px">
                                        <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                            <ContentTemplate>
                                            <div class="topDiv">
                                                <div style="float: left; padding-top: 5px;">
                                                    <label>what to do --> </label>
                                                </div>
                                                <div style="float: left;">
                                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image8" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                  <rad:RadToolTip ID="RadToolTip8" runat="server" Skin="Black" Width="200px" 
                                        Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" 
                                        RelativeTo="Element" TargetControlID="Image8">
                                        <label> Please indicate here the date and time of your event as well as the location. 
                                        First, choose the start and end date and time, then choose your location. After these fields are filled
                                        in, click 'Add' to add your selection. You can add as many differenct dates and locations
                                        for this event as you like.
                                        </label>
                                        </rad:RadToolTip>
                                                </div>
                                            </div>
                                            
                                                <table>
                                                   <tr>
                                                   <td valign="top" width="350px">
                                                   <style type="text/css">
                                                    .RadCalendarPopup {
                                                            left:420px !important;
                                                            top:550px !important;
                                                            }
                                                   </style>
                                                   <div style="float: left; width: 215px;">
                                                        <h1 class="Green"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Event Occurance</h1>
                                                        <span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;"> [choose all dates this event re-occurs]</span>
                                                                                   <br />     <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Start Date & Time</label> 
                                                                                        <br />
                                                        <rad:RadDateTimePicker runat="server" ID="StartDateTimePicker" ></rad:RadDateTimePicker><br />
                                                        <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>End Date & Time</label><br />
                                                        <rad:RadDateTimePicker runat="server" ID="EndDateTimePicker" ></rad:RadDateTimePicker><br />
                                                    </div>
                                                   </td>
                                                    <td valign="top" width="350px">
                                                    
                                                    
                                                 <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                <ContentTemplate>
                                                <h1 class="Green"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Location </h1>
                                                    <span style="font-family: Arial; font-size: 10px; color: #ff770d; font-weight: bold;"> [choose all places this event occurs]</span>
                                                    <asp:RadioButtonList AutoPostBack="true" 
                                                    OnSelectedIndexChanged="ChangePrivate" 
                                                    runat="server" ID="PrivateCheckList" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Private Location" Selected="true" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Venue" Value="2"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                          <asp:Panel runat="server" ID="PrivatePanel"  
                                          BorderColor="#666666" BorderStyle="solid" BorderWidth="3px" Width="300px"  >
                                          <div style="padding-top: 10px; padding-bottom: 5px; padding-left: 3px;">
                                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image3" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                                    <rad:RadToolTip ID="RadToolTip3" runat="server" Skin="Black" Width="200px" Height="200px" 
                                                    ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" 
                                                    TargetControlID="Image3">
                                            <label>
                                                If the group event occurss at someone's house or similar, you might want to just specify the address here.
                                                If you create a venue out of this address, even though you can still make this a private event the venue will be public.
                                                Meanin others will be able to create events at your house. 
                                                You might not want that.
                                            </label>
                                            </rad:RadToolTip>
                                            </div>
                                                        <table cellpadding="0" cellspacing="5">
                                                        <tr>
                                                            <td colspan="2">
                                                                <label style="padding-right: 10px;"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Country</label><br />
                                                                <asp:DropDownList OnSelectedIndexChanged="ChangeState" AutoPostBack="true" runat="server" DataSourceID="SqlDataSourceCountry" DataTextField="country_name" DataValueField="country_id" ID="CountryDropDown"></asp:DropDownList>
                                                                <asp:SqlDataSource runat="server" ID="SqlDataSourceCountry" SelectCommand="SELECT * FROM Countries"
                                                                ConnectionString="<%$ ConnectionStrings:Connection %>"></asp:SqlDataSource>
                                                            
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>State</label><br />
                                                                        </td>
                                                                        <td>
                                                                            <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>City</label>
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Zip</label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel runat="server" ID="StateTextBoxPanel">
                                                                                <ctrl:HippoTextBox ID="StateTextBox" runat="server" TEXTBOX_WIDTH="100" CSS_CLASS="TextBox25" LITERAL_CSS_CLASS="TextBoxLeftImage25" />
                                                                            </asp:Panel>
                                                                            <asp:Panel runat="server" ID="StateDropDownPanel" Visible="false">
                                                                                <asp:DropDownList runat="server" ID="StateDropDown"></asp:DropDownList>
                                                                            </asp:Panel>
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:TextBox ID="CityTextBox" runat="server" WIDTH="100"></asp:TextBox>
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:TextBox ID="ZipTextBox" runat="server" WIDTH="70" ></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        
                                                        <asp:Panel runat="server" ID="USPanel">
                                                        <tr>
                                                        <td colspan="2">
                                                
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Street No.</label>
                                                                    </td>
                                                                    <td style="padding-left: 3px;">
                                                                        <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Street Name</label>
                                                                    </td>
                                                                    <td>
                                                                       <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Street Title</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="StreetNumberTextBox" Width="70px"></asp:TextBox>
                                                                    </td>
                                                                    <td style="padding-left: 3px;">
                                                                        <asp:TextBox runat="server" ID="StreetNameTextBox" Width="100px"></asp:TextBox>
                                                                    </td>
                                                                    <td style="padding-left: 3px;">
                                                                        <asp:DropDownList runat="server" ID="StreetDropDown" Width="100px">
                                                                            <asp:ListItem Text="Select One..."></asp:ListItem>
                                                                            <asp:ListItem Text="Alley"></asp:ListItem>
                                                                            <asp:ListItem Text="Annex"></asp:ListItem>
                                                                            <asp:ListItem Text="Arcade"></asp:ListItem>
                                                                            <asp:ListItem Text="Avenue"></asp:ListItem>
                                                                            <asp:ListItem Text="Bayoo"></asp:ListItem>
                                                                            <asp:ListItem Text="Beach"></asp:ListItem>
                                                                            <asp:ListItem Text="Bend"></asp:ListItem>
                                                                            <asp:ListItem Text="Bluff"></asp:ListItem>
                                                                            <asp:ListItem Text="Bluffs"></asp:ListItem>
                                                                            <asp:ListItem Text="Bottom"></asp:ListItem>
                                                                            <asp:ListItem Text="Boulevard"></asp:ListItem>
                                                                            <asp:ListItem Text="Branch"></asp:ListItem>
                                                                            <asp:ListItem Text="Bridge"></asp:ListItem>
                                                                            <asp:ListItem Text="Brook"></asp:ListItem>
                                                                            <asp:ListItem Text="Brooks"></asp:ListItem>
                                                                            <asp:ListItem Text="Burg"></asp:ListItem>
                                                                            <asp:ListItem Text="Burgs"></asp:ListItem>
                                                                            <asp:ListItem Text="Bypass"></asp:ListItem>
                                                                            <asp:ListItem Text="Camp"></asp:ListItem>
                                                                            <asp:ListItem Text="Canyon"></asp:ListItem>
                                                                            <asp:ListItem Text="Cape"></asp:ListItem>
                                                                            <asp:ListItem Text="Causeway"></asp:ListItem>
                                                                            <asp:ListItem Text="Center"></asp:ListItem>
                                                                            <asp:ListItem Text="Centers"></asp:ListItem>
                                                                            <asp:ListItem Text="Circle"></asp:ListItem>
                                                                            <asp:ListItem Text="Circles"></asp:ListItem>
                                                                            <asp:ListItem Text="Cliff"></asp:ListItem>
                                                                            <asp:ListItem Text="Cliffs"></asp:ListItem>
                                                                            <asp:ListItem Text="Club"></asp:ListItem>
                                                                            <asp:ListItem Text="Common"></asp:ListItem>
                                                                            <asp:ListItem Text="Corner"></asp:ListItem>
                                                                            <asp:ListItem Text="Corners"></asp:ListItem>
                                                                            <asp:ListItem Text="Course"></asp:ListItem>
                                                                            <asp:ListItem Text="Court"></asp:ListItem>
                                                                            <asp:ListItem Text="Courts"></asp:ListItem>
                                                                            <asp:ListItem Text="Cove"></asp:ListItem>
                                                                            <asp:ListItem Text="Coves"></asp:ListItem>
                                                                            <asp:ListItem Text="Creek"></asp:ListItem>
                                                                            <asp:ListItem Text="Crescent"></asp:ListItem>
                                                                            <asp:ListItem Text="Crest"></asp:ListItem>
                                                                            <asp:ListItem Text="Crossing"></asp:ListItem>
                                                                            <asp:ListItem Text="Crossroad"></asp:ListItem>
                                                                            <asp:ListItem Text="Curve"></asp:ListItem>
                                                                            <asp:ListItem Text="Dale"></asp:ListItem>
                                                                            <asp:ListItem Text="Dam"></asp:ListItem>
                                                                            <asp:ListItem Text="Divide"></asp:ListItem>
                                                                            <asp:ListItem Text="Drive"></asp:ListItem>
                                                                            <asp:ListItem Text="Drives"></asp:ListItem>
                                                                            <asp:ListItem Text="Estate"></asp:ListItem>
                                                                            <asp:ListItem Text="Estates"></asp:ListItem>
                                                                            <asp:ListItem Text="Expressway"></asp:ListItem>
                                                                            <asp:ListItem Text="Extension"></asp:ListItem>
                                                                            <asp:ListItem Text="Extensions"></asp:ListItem>
                                                                            <asp:ListItem Text="Fall"></asp:ListItem>
                                                                            <asp:ListItem Text="Falls"></asp:ListItem>
                                                                            <asp:ListItem Text="Ferry"></asp:ListItem>
                                                                            <asp:ListItem Text="Field"></asp:ListItem>
                                                                            <asp:ListItem Text="Fields"></asp:ListItem>
                                                                            <asp:ListItem Text="Flat"></asp:ListItem>
                                                                            <asp:ListItem Text="Flats"></asp:ListItem>
                                                                            <asp:ListItem Text="Ford"></asp:ListItem>
                                                                            <asp:ListItem Text="Fords"></asp:ListItem>
                                                                            <asp:ListItem Text="Forest"></asp:ListItem>
                                                                            <asp:ListItem Text="Forge"></asp:ListItem>
                                                                            <asp:ListItem Text="Forges"></asp:ListItem>
                                                                            <asp:ListItem Text="Fork"></asp:ListItem>
                                                                            <asp:ListItem Text="Forks"></asp:ListItem>
                                                                            <asp:ListItem Text="Fort"></asp:ListItem>
                                                                            <asp:ListItem Text="Freeway"></asp:ListItem>
                                                                            <asp:ListItem Text="Garden"></asp:ListItem>
                                                                            <asp:ListItem Text="Gardens"></asp:ListItem>
                                                                            <asp:ListItem Text="Gateway"></asp:ListItem>
                                                                            <asp:ListItem Text="Glen"></asp:ListItem>
                                                                            <asp:ListItem Text="Glens"></asp:ListItem>
                                                                            <asp:ListItem Text="Green"></asp:ListItem>
                                                                            <asp:ListItem Text="Greens"></asp:ListItem>
                                                                            <asp:ListItem Text="Grove"></asp:ListItem>
                                                                            <asp:ListItem Text="Harbor"></asp:ListItem>
                                                                            <asp:ListItem Text="Harbors"></asp:ListItem>
                                                                            <asp:ListItem Text="Haven"></asp:ListItem>
                                                                            <asp:ListItem Text="Heights"></asp:ListItem>
                                                                            <asp:ListItem Text="Highway"></asp:ListItem>
                                                                            <asp:ListItem Text="Hill"></asp:ListItem>
                                                                            <asp:ListItem Text="Hills"></asp:ListItem>
                                                                            <asp:ListItem Text="Hollow"></asp:ListItem>
                                                                            <asp:ListItem Text="Inlet"></asp:ListItem>
                                                                            <asp:ListItem Text="Island"></asp:ListItem>
                                                                            <asp:ListItem Text="Islands"></asp:ListItem>
                                                                            <asp:ListItem Text="Isle"></asp:ListItem>
                                                                            <asp:ListItem Text="Junction"></asp:ListItem>
                                                                            <asp:ListItem Text="Junctions"></asp:ListItem>
                                                                            <asp:ListItem Text="Key"></asp:ListItem>
                                                                            <asp:ListItem Text="Keys"></asp:ListItem>
                                                                            <asp:ListItem Text="Knoll"></asp:ListItem>
                                                                            <asp:ListItem Text="Knolls"></asp:ListItem>
                                                                            <asp:ListItem Text="Lake"></asp:ListItem>
                                                                            <asp:ListItem Text="Lakes"></asp:ListItem>
                                                                            <asp:ListItem Text="Land"></asp:ListItem>
                                                                            <asp:ListItem Text="Landing"></asp:ListItem>
                                                                            <asp:ListItem Text="Lane"></asp:ListItem>
                                                                            <asp:ListItem Text="Light"></asp:ListItem>
                                                                            <asp:ListItem Text="Lights"></asp:ListItem>
                                                                            <asp:ListItem Text="Loaf"></asp:ListItem>
                                                                            <asp:ListItem Text="Lock"></asp:ListItem>
                                                                            <asp:ListItem Text="Locks"></asp:ListItem>
                                                                            <asp:ListItem Text="Lodge"></asp:ListItem>
                                                                            <asp:ListItem Text="Loop"></asp:ListItem>
                                                                            <asp:ListItem Text="Mall"></asp:ListItem>
                                                                            <asp:ListItem Text="Manor"></asp:ListItem>
                                                                            <asp:ListItem Text="Manors"></asp:ListItem>
                                                                            <asp:ListItem Text="Meadow"></asp:ListItem>
                                                                            <asp:ListItem Text="Meadows"></asp:ListItem>
                                                                            <asp:ListItem Text="Mews"></asp:ListItem>
                                                                            <asp:ListItem Text="Mill"></asp:ListItem>
                                                                            <asp:ListItem Text="Mills"></asp:ListItem>
                                                                            <asp:ListItem Text="Mission"></asp:ListItem>
                                                                            <asp:ListItem Text="Motorway"></asp:ListItem>
                                                                            <asp:ListItem Text="Mount"></asp:ListItem>
                                                                            <asp:ListItem Text="Mountain"></asp:ListItem>
                                                                            <asp:ListItem Text="Mountains"></asp:ListItem>
                                                                            <asp:ListItem Text="Neck"></asp:ListItem>
                                                                            <asp:ListItem Text="Orchard"></asp:ListItem>
                                                                            <asp:ListItem Text="Oval"></asp:ListItem>
                                                                            <asp:ListItem Text="Overpass"></asp:ListItem>
                                                                            <asp:ListItem Text="Park"></asp:ListItem>
                                                                            <asp:ListItem Text="Parks"></asp:ListItem>
                                                                            <asp:ListItem Text="Parkway"></asp:ListItem>
                                                                            <asp:ListItem Text="Parkways"></asp:ListItem>
                                                                            <asp:ListItem Text="Pass"></asp:ListItem>
                                                                            <asp:ListItem Text="Passage"></asp:ListItem>
                                                                            <asp:ListItem Text="Path"></asp:ListItem>
                                                                            <asp:ListItem Text="Pike"></asp:ListItem>
                                                                            <asp:ListItem Text="Pine"></asp:ListItem>
                                                                            <asp:ListItem Text="Pines"></asp:ListItem>
                                                                            <asp:ListItem Text="Place"></asp:ListItem>
                                                                            <asp:ListItem Text="Plain"></asp:ListItem>
                                                                            <asp:ListItem Text="Plains"></asp:ListItem>
                                                                            <asp:ListItem Text="Plaza"></asp:ListItem>
                                                                            <asp:ListItem Text="Point"></asp:ListItem>
                                                                            <asp:ListItem Text="Points"></asp:ListItem>
                                                                            <asp:ListItem Text="Port"></asp:ListItem>
                                                                            <asp:ListItem Text="Ports"></asp:ListItem>
                                                                            <asp:ListItem Text="Prairie"></asp:ListItem>
                                                                            <asp:ListItem Text="Radial"></asp:ListItem>
                                                                            <asp:ListItem Text="Ramp"></asp:ListItem>
                                                                            <asp:ListItem Text="Ranch"></asp:ListItem>
                                                                            <asp:ListItem Text="Rapid"></asp:ListItem>
                                                                            <asp:ListItem Text="Rapids"></asp:ListItem>
                                                                            <asp:ListItem Text="Rest"></asp:ListItem>
                                                                            <asp:ListItem Text="Ridge"></asp:ListItem>
                                                                            <asp:ListItem Text="Ridges"></asp:ListItem>
                                                                            <asp:ListItem Text="River"></asp:ListItem>
                                                                            <asp:ListItem Text="Road"></asp:ListItem>
                                                                            <asp:ListItem Text="Roads"></asp:ListItem>
                                                                            <asp:ListItem Text="Route"></asp:ListItem>
                                                                            <asp:ListItem Text="Row"></asp:ListItem>
                                                                            <asp:ListItem Text="Rue"></asp:ListItem>
                                                                            <asp:ListItem Text="Run"></asp:ListItem>
                                                                            <asp:ListItem Text="Shoal"></asp:ListItem>
                                                                            <asp:ListItem Text="Shoals"></asp:ListItem>
                                                                            <asp:ListItem Text="Shore"></asp:ListItem>
                                                                            <asp:ListItem Text="Shores"></asp:ListItem>
                                                                            <asp:ListItem Text="Skyway"></asp:ListItem>
                                                                            <asp:ListItem Text="Spring"></asp:ListItem>
                                                                            <asp:ListItem Text="Springs"></asp:ListItem>
                                                                            <asp:ListItem Text="Spur"></asp:ListItem>
                                                                            <asp:ListItem Text="Spurs"></asp:ListItem>
                                                                            <asp:ListItem Text="Square"></asp:ListItem>
                                                                            <asp:ListItem Text="Squares"></asp:ListItem>
                                                                            <asp:ListItem Text="Station"></asp:ListItem>
                                                                            <asp:ListItem Text="Stravenue"></asp:ListItem>
                                                                            <asp:ListItem Text="Stream"></asp:ListItem>
                                                                            <asp:ListItem Text="Street"></asp:ListItem>
                                                                            <asp:ListItem Text="Streets"></asp:ListItem>
                                                                            <asp:ListItem Text="Summit"></asp:ListItem>
                                                                            <asp:ListItem Text="Terrace"></asp:ListItem>
                                                                            <asp:ListItem Text="Throughway"></asp:ListItem>
                                                                            <asp:ListItem Text="Trace"></asp:ListItem>
                                                                            <asp:ListItem Text="Track"></asp:ListItem>
                                                                            <asp:ListItem Text="Trafficway"></asp:ListItem>
                                                                            <asp:ListItem Text="Trail"></asp:ListItem>
                                                                            <asp:ListItem Text="Tunnel"></asp:ListItem>
                                                                            <asp:ListItem Text="Turnpike"></asp:ListItem>
                                                                            <asp:ListItem Text="Underpass"></asp:ListItem>
                                                                            <asp:ListItem Text="Union"></asp:ListItem>
                                                                            <asp:ListItem Text="Valley"></asp:ListItem>
                                                                            <asp:ListItem Text="Valleys"></asp:ListItem>
                                                                            <asp:ListItem Text="Viaduct"></asp:ListItem>
                                                                            <asp:ListItem Text="View"></asp:ListItem>
                                                                            <asp:ListItem Text="Views"></asp:ListItem>
                                                                            <asp:ListItem Text="Village"></asp:ListItem>
                                                                            <asp:ListItem Text="Villages"></asp:ListItem>
                                                                            <asp:ListItem Text="Ville"></asp:ListItem>
                                                                            <asp:ListItem Text="Vista"></asp:ListItem>
                                                                            <asp:ListItem Text="Walk"></asp:ListItem>
                                                                            <asp:ListItem Text="Walks"></asp:ListItem>
                                                                            <asp:ListItem Text="Wall"></asp:ListItem>
                                                                            <asp:ListItem Text="Way"></asp:ListItem>
                                                                            <asp:ListItem Text="Ways"></asp:ListItem>
                                                                            <asp:ListItem Text="Well"></asp:ListItem>
                                                                            <asp:ListItem Text="Wells"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        </tr>
                                                        </asp:Panel>
                                                        <asp:Panel runat="server" ID="InternationalPanel" Visible="false">
                                                        <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                       <label><span style='color: #1fb6e7; font-weight: bold;'>* </span>Location</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="LocationTextBox" Width="200px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        </tr>
                                                        </asp:Panel>
                                                    
                                                    <tr>
                                                        <td colspan="2" style="padding-top: 3px;">
                                                            <asp:DropDownList runat="server" ID="AptDropDown">
                                                                <asp:ListItem Text="Suite" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Apt" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                       
                                                            <asp:TextBox runat="server" ID="AptNumberTextBox" Width="50px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel BorderColor="#666666" BorderStyle="solid" 
                                                    BorderWidth="3px" runat="server" Width="300px" ID="PublicPanel" Visible="false">
                                                        
                                                         <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>--%>
                                             
                                                    <div class="topDiv">
                                                    <div style="float: left; width: 290px ;padding-left: 10px; padding-bottom: 20px; padding-top: 10px;">
                                                    <asp:Image CssClass="HelpImage" runat="server"  ID="Image4" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                                    <rad:RadToolTip ID="RadToolTip4" runat="server" Skin="Black" Width="200px" Height="200px" 
                                                    ManualClose="true" ShowEvent="onclick" Position="MiddleRight" RelativeTo="Element" 
                                                    TargetControlID="Image4">
                                            <label>
                                                If your event is at a venue already in our system, search for it here. You can also create a new venue if it does
                                                not yet exist in our files. However, if you'd rather keep you venue private (i.e. non-searchable through our site and
                                                not accessible to anyone else posting events) you can just include the address in the 'Event is at a Private Location' 
                                                box.
                                            </label>
                                            </rad:RadToolTip>
                                                    <div style="padding-top: 5px;">
                                                        <asp:Panel runat="server" ID="ExistingVenuePanel">
                                                    
                                                            <asp:LinkButton runat="server" ID="GetNewVenueButton" OnClick="GetNewVenue"></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="LinkButton1" OnClick="SetNewVenue"></asp:LinkButton>
                                                      <span style="font-family: Arial; font-size: 10px; color: #cccccc;"><span style='color: #1fb6e7; font-weight: bold;'>*</span>Search existing venue or <span style='color: #1fb6e7; font-weight: bold; cursor: pointer;' onclick="OpenCreateVenue();">create one if it doesn’t exist.</span></span>
                                                        <asp:DropDownList runat="server" ID="VenueCountry" OnSelectedIndexChanged="ChangeVenueStateAction" AutoPostBack="true" ></asp:DropDownList>
                                                        <div style="float: left;padding-top: 5px;">
                                                            <asp:DropDownList runat="server" ID="VenueState"></asp:DropDownList>
                                                            
                                                            <asp:TextBox runat="server" ID="VenueStateTextBox"></asp:TextBox>
                                                            
                                                        </div>
                                                        <div style="float: left; padding-top: 2px;">
                                                                <asp:ImageButton OnClick="GetVenues" runat="server" ID="GoButton" ImageUrl="image/GoButton.png" onmouseover="this.src='image/GoButtonSelected.png'" onmouseout="this.src='image/GoButton.png'" />
                                                                <%--<asp:DropDownList Visible="false" runat="server" ID="VenueDropDown" Visible="false">
                                                                </asp:DropDownList>--%>
                                                            </div>
                                                        <div style="float: left;">
                                                            <div style="float: right; padding-top: 8px;padding-right: 10px;">
                                                       
                                                            <rad:RadToolTip RelativeTo="Element" Position="topCenter" ManualClose="true" runat="server" ID="Tip1" TargetControlID="Div1" ShowEvent="OnClick">
                                                                stuff
                                                            </rad:RadToolTip>
                                                            <div runat="server" id="VenueIDDIV" style="display: none;"></div>
                                                            <div runat="server" style="cursor: pointer;width: 154px; height: 20px; margin-top: -3px;" id="Div1">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <div style="float: left;border: solid 2px #ff7704;padding: 2px;background-color: transparent;color: White; font-weight: bold; font-family: Arial;font-size: 12px;" runat="server" id="TimeFrameDiv">
                                                                            Select Venue >
                                                                        </div>
            <%--                                                            <img style="padding-top: 7px;float: left;" src="image/VenuesArrow.png" />
            --%>                                                    </td>
                                                                </tr>
                                                            </table>
                                                            </div>
                                                            
                                                         </div>
                                                        </div>
                                                    </asp:Panel>
                                                    </div>
                                                    </div>
                                                   
                                              
                                                    </div>
                                                    <%--</ContentTemplate>
                                                     </asp:UpdatePanel>--%>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                             </asp:UpdatePanel>
                                             </td>
                                             
                                                   </tr>
                                                   <tr>
                                            <td colspan="2" align="center" style="padding-top: 10px;">
                                              
                                                         <asp:Button runat="server" ID="AddButton" Text="Add >>" 
                                                         CssClass="SearchButton"
                                                         OnClick="AddDate"
                                                         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                         onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                         onclientclick="this.value = 'Working...';" 
                                                         />
                                                                                                       
                                                        <asp:Button runat="server" ID="RemoveButton" Text="<< Remove" 
                                                         CssClass="SearchButton"
                                                         OnClick="SubtractDate"
                                                         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                         onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                         onclientclick="this.value = 'Working...';" 
                                                         />
                                            </td>
                                        </tr>
                                        <tr>
                                                    <td valign="top" colspan="2">
                                                        <asp:Label runat="server" ID="DateErrorLabel" ForeColor="red" Font-Bold="true" Font-Names="Arial" Font-Size="12px"></asp:Label> 
                                                    </td>
                                                   </tr>
                                                   <tr>
                                                    <td colspan="2" align="center" valign="top">
                                                        <h1 class="Blue">Your Dates and Locations:</h1>
                                                           
                                                        <asp:ListBox runat="server" EnableViewState="true" ID="DateSelectionsListBox" 
                                                          CssClass="ListBox" SelectionMode="single" 
                                                        ForeColor="#cccccc" BackColor="#666666" Width="500px" Height="100px"></asp:ListBox>
                                                    </td>
                                                   </tr>
                                                   
                                                </table>
                                        
                                            
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                
                            </table>
                        </div>
                        <div align="right" style="vertical-align: bottom; width: 600px;">
                                <asp:ImageButton runat="server" ID="DetailsOnwardsButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                                onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                             </div>
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                </asp:Panel>
            </rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView1" TabIndex="1" >
                <asp:Panel runat="server" ID="Panel2" DefaultButton="DescriptionOnwardsButton">
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div  class="EventDiv" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y; width: 668px;">
                         <div style="padding-left: 30px; width: 632px; clear: both; padding-top: 10px;" class="topDiv">
                                <h1>Media</h1>
                               <div style="padding-left: 20px; clear: both;">
                                <asp:CheckBox ID="MainAttractionCheck" Text="Main Attraction <span style='color: #ff770d;'>[these will all be shown together in a rotator window. 20 is the max]</span>" runat="server" AutoPostBack="true" OnCheckedChanged="EnableMainAttractionPanel" />
                                    <asp:Panel runat="server" ID="MainAttractionPanel" Visible="false"> 
                                        <div style="padding-left: 30px;" class="topDiv">
                                            <div class="topDiv" style="padding-left: 30px; border: solid 4px #515151; width: 500px; padding-bottom: 10px; padding-left: 30px;">
                                            <asp:RadioButtonList runat="server" ID="MainAttracionRadioList" 
                                            AutoPostBack="true" RepeatDirection="horizontal" OnSelectedIndexChanged="ShowSliderOrVidPic">
                                                <asp:ListItem Text="Add YouTube Video" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Add Picture" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                                <asp:Panel runat="server" ID="VideoPanel" Visible="false">
                                                    <div style="padding-left: 20px;">
                                                        <%--<asp:RadioButtonList runat="server" ID="VideoRadioList" AutoPostBack="true" 
                                                        OnSelectedIndexChanged="ShowVideo">
                                                            <asp:ListItem Text="Upload a Video" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Paste a You Tube Link" Value="1"></asp:ListItem>
                                                        </asp:RadioButtonList>--%>
                                                        <asp:Panel runat="server" ID="UploadPanel" Visible="false">
                                                            <div id="topDiv3">
                                                                   <div style="float: left; padding-top: 4px;">
                                                                    <asp:FileUpload runat="server" ID="VideoUpload" Width="230px" EnableViewState="true" />
                                                                </div>
                                                                    
                                                                <div style="float: left;">
                                                                <asp:Button runat="server" ID="ImageButton6" Text="Upload!" 
                                                                     CssClass="SearchButton" 
                                                                     OnClick="VideoUpload_Click"
                                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                     onclientclick="this.value = 'Working...';" 
                                                                     />
                                                                <%--<asp:ImageButton runat="server" ID="ImageButton6" OnClick="VideoUpload_Click"
                                                                     ImageUrl="image/MakeItSoButton.png" 
                                                                    onmouseout="this.src='image/MakeItSoButton.png'" 
                                                                    onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />--%>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel runat="server" ID="YouTubePanel">
                                                        <label>Insert the ID of the YouTube video:</label><br />
                                                                
                                                            <div class="topDiv" style="clear: both;">
                                                               <label> http://www.youtube.com/watch?v=<span style="color: Red;">YMLTwq1M2pw</span>  <--This is the ID</label>
                                                                <div style="float: left; padding-top: 4px;">
                                                                    <asp:TextBox runat="server" ID="YouTubeTextBox"></asp:TextBox>
                                                                </div>
                                                                <div style="float: left;">
                                                                <asp:Button runat="server" ID="ImageButton7" Text="Upload!" 
                                                                     CssClass="SearchButton" 
                                                                     OnClick="YouTubeUpload_Click"
                                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                     onclientclick="this.value = 'Working...';" 
                                                                     />
<%--                                                                    <asp:ImageButton runat="server" ID="ImageButton7"  
                                                                                OnClick="YouTubeUpload_Click"
                                                                         ImageUrl="image/MakeItSoButton.png" 
                                                                        onmouseout="this.src='image/MakeItSoButton.png'" 
                                                                        onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
--%>                                                                </div>
                                                            </div>
                                                            <label>ex: if this is the url of your YouTube video <br />http://www.youtube.com/watch?v=<span style="color: Red;">YMLTwq1M2pw</span>  <--This is the ID</label>
                                                        </asp:Panel>
                                                        
                                                     </div>
                                                 </asp:Panel> 
                                                 <div style="padding-left: 30px;" class="topDiv">
                                                        <asp:Panel runat="server" ID="PicturePanel" Visible="false">
                                                            <div id="topDiv2">
                                                                <div style="float: left; padding-top: 4px;">
                                                                    <asp:FileUpload runat="server" ID="PictureUpload" Width="230px" EnableViewState="true" />
                                                                </div>
                                                                <div style="float: left;">
                                                                <asp:Button runat="server" ID="ImageButton5" Text="Upload!" 
                                                                     CssClass="SearchButton" 
                                                                     OnClick="PictureUpload_Click"
                                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                     onclientclick="this.value = 'Working...';" 
                                                                     />
<%--                                                                    <asp:ImageButton runat="server" ID="ImageButton5"  OnClick="PictureUpload_Click"
                                                                     ImageUrl="image/MakeItSoButton.png"
                                                                    onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
--%>                                                                </div>
                                                                
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                   
                                           </div> 
                                              
                                       </div>
                                        <div style="float: left; padding-right: 70px;" class="EventDiv">
                                        <asp:Panel runat="server" ID="PicsNVideosPanel" Visible="false">
                                        <br />
                                        <label>Your Pics and Videos </label><br />
                                            <div>
                                                <div style="float: left; padding-right: 20px;">
                                                    <asp:CheckBoxList CssClass="MusicCheckBoxes" ID="PictureCheckList" runat="server"></asp:CheckBoxList>
                                                   
                                                    <asp:ImageButton runat="server" OnClick="SliderNixIt" ID="PictureNixItButton" ImageUrl="image/NixItButton.png" 
                                                    onmouseout="this.src='image/NixItButton.png'" onmouseover="this.src='image/NixItButtonSelected.png'"  />
                                               </div>
                                                <div style="float: left; padding-left: 30px; width: 250px;">
                                                   
                                                    <label>Check a picture to add caption</label>
                                                    <asp:TextBox runat="server" ID="CaptionTextBox" TextMode="MultiLine" Width="200px" Height="50px"></asp:TextBox>
                                                     <asp:Button runat="server" ID="Button3" Text="Add Caption" 
                                                     CssClass="SearchButton" 
                                                     OnClick="AddCaption"
                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                     onclientclick="this.value = 'Working...';" 
                                                     />
                                                     <asp:Button runat="server" ID="Button4" Text="Edit Caption" 
                                                     CssClass="SearchButton" 
                                                     OnClick="EditCaption"
                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                     onclientclick="this.value = 'Working...';" 
                                                     />
                                                </div>
                                            </div>
                                            </asp:Panel>
                                        </div>
                                    </asp:Panel>
                                    </div>
                                    <br /><br />
                                    <div style="clear: both;padding-top : 20px;">
                               <h1>Design</h1>
                               <label>Choose Two Base Colors For Your Group Page</label>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton runat="server" ID="ColorBackRadioButton" 
                                                                AutoPostBack="true" OnCheckedChanged="SelectColorBackNText" />
                                                            </td>
                                                            <td>
                                                                <label>Background</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Panel runat="server" ID="BackColorPanel">
                                                                    <asp:RadioButtonList runat="server" ID="ColorSchemeRadioList" 
                                                                    RepeatDirection="horizontal" AutoPostBack="true" 
                                                                    OnSelectedIndexChanged="ChangeColorPanel">
                                                                        <asp:ListItem Selected="true" Text="Default" Value="0"></asp:ListItem>
                                                                        <asp:ListItem Text="Custom" Value="1"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton runat="server" ID="ColorTextRadioButton" 
                                                                AutoPostBack="true" OnCheckedChanged="SelectColorBackNText" />
                                                            </td>
                                                            <td>
                                                                <label>Text</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Panel runat="server" ID="Panel8">
                                                                    <asp:RadioButtonList runat="server" ID="ColorTextRadioList" RepeatDirection="horizontal" 
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ChangeTextColorPanel">
                                                                        <asp:ListItem Selected="true" Text="Default" Value="0"></asp:ListItem>
                                                                        <asp:ListItem Text="Custom" Value="1"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                
                                                
                                                
                                                <asp:Panel runat="server" ID="ColorPanel" Visible="false">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td valign="top" style="padding-right: 30px;">
                                                                <rad:RadColorPicker runat="server" AutoPostBack="true" OnColorChanged="ChangeColor" 
                                                                ID="RadColorPicker1" ShowEmptyColor="false" PreviewColor="true" Preset="Web216" 
                                                                Width="270px" >
                                                                </rad:RadColorPicker>
                                                            </td>
                                                            <td valign="top">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td valign="top" style="padding-right: 5px;">
                                                                            <asp:CheckBox Checked="true" runat="server" ID="Color1CheckBox" Text="First Scheme" />
                                                                        </td>
                                                                        <td valign="top" runat="server" id="ColorTd1" width="20px" height="20px">
                                                                            <div style="width: 200px; height: 99px; background-image: 
                                                                            url(images/MaskWhole.png);">
                                                                                <div style="font-family: Arial; font-size: 9px;padding-top: 40px;padding-left: 30px;">
                                                                                    <asp:Label runat="server" Text="Our Group Page is Awesome" 
                                                                                    ID="Text1Label"></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" style="padding-top: 5px; padding-right: 5px;">
                                                                            <asp:CheckBox Checked="true" runat="server" ID="Color2CheckBox" Text="Second Scheme" />
                                                                        </td>
                                                                        <td valign="top" style="margin-top: 5px;" runat="server" id="ColorTd2" width="20px" height="20px">
                                                                           <div style="width: 200px; height: 99px; background-image: 
                                                                            url(images/MaskWhole.png);">
                                                                                <div style="font-family: Arial; font-size: 9px; padding-top: 40px; padding-left: 30px;">
                                                                                    <asp:Label runat="server" Text="Our Group Page is Awesome" ID="Text2Label"></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        </div>
                            </div>
                        <div align="right" style=" padding-top: 30px; padding-right: 50px;">
                            <asp:ImageButton runat="server" ID="ImageButton2" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                            <asp:ImageButton runat="server" ID="DescriptionOnwardsButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                         </div>
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                </asp:Panel>
            </rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView2" TabIndex="2" >
                <asp:Panel runat="server" ID="Panel3" DefaultButton="MediaOnwardsButton">
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;width: 668px;">
                        <a href="#top"></a>
                        <div class="EventDiv">
                           <div style="padding-left: 30px; width: 610px; clear: both; padding-top: 10px;" class="topDiv">
                               <h1>Internals</h1>
                              <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                <asp:Panel runat="server" BorderColor="#666666" 
                                  BorderStyle="solid" BorderWidth="3px">
                                  <asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="ShowAgendaPanel" ID="AgendaCheckBox" Text="Include Agenda" />
                                  <asp:Image CssClass="HelpImage" runat="server"  ID="Image1" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                  <rad:RadToolTip ID="RadToolTip1" runat="server" Skin="Black" Width="200px" 
                                        Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" 
                                        RelativeTo="Element" TargetControlID="Image1">
                                        <label> You can include a free-form agenda which will be displayed on the event page. 
                                        </label>
                                        </rad:RadToolTip>
                                  <asp:Panel runat="server" ID="AgendaPanel"  Visible="false">
                                            <table>
                                                <tr>
                                                    <td><br />
                                                        <label>Item Title: </label> <asp:TextBox runat="server" ID="AgendaItemTextBox"></asp:TextBox>
                                                        <br /><br />
                                                        <label>Item Description [Not-Required]: </label>
                                                        <asp:TextBox runat="server" ID="AgendaDescriptionTextBox" TextMode="MultiLine" Width="200px" Height="50px"></asp:TextBox>
                                                        <br /><br />
                                                        <asp:Button runat="server" ID="Button2" Text="Add Item" 
                                                         CssClass="SearchButton"
                                                         OnClick="AddAgendaItem"
                                                         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                         onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                         onclientclick="this.value = 'Working...';" 
                                                         /><br />
                                                         <asp:Label runat="server" ID="AgendaErrorLabel" ForeColor="Red" Font-Size="11px"></asp:Label>
                                                    </td>
                                                    <td valign="top">
                                                        <label>Agenda</label>
                                                        <div style="padding-left: 5px; padding-right: 5px;border: solid 3px #666666;min-height: 100px;width: 350px; background-color: #1b1b1b; padding-top: 10px; padding-bottom: 10px;">
                                                            <asp:Literal runat="server" ID="AgendaLiteral"></asp:Literal>
                                                        </div>
                                                        <asp:Button runat="server" ID="Button6" Text="Remove Last" 
                                                         CssClass="SearchButton"
                                                         OnClick="RemoveOneAgenda"
                                                         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                         onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                         onclientclick="this.value = 'Working...';" 
                                                         />
                                                    </td>
                                                </tr>
                                            </table>
                                  </asp:Panel>
                                  </asp:Panel>
                              </ContentTemplate>
                              </asp:UpdatePanel>
                              <br />
                              <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                  <asp:Panel ID="Panel6" runat="server" BorderColor="#666666" 
                                  BorderStyle="solid" BorderWidth="3px">
                                        
                                      <asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="ShowStuffPanel" ID="StuffCheckBox" Text="Include 'Stuff we Need' Section" />
                                      <asp:Image CssClass="HelpImage" runat="server"  ID="Image2" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                            <rad:RadToolTip ID="RadToolTip2" runat="server" Skin="Black" Width="200px" 
                                            Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" 
                                            RelativeTo="Element" TargetControlID="Image2">
                                            <label> The 'Stuff we Need' section outlines the things that are needed at the event.
                                            On the event page, it is displayed with a check box next to each item on the list. The
                                            user whom check a check box is volunteering to bring the item and will have their 
                                            username displayed next to the item in the list. 
                                            </label>
                                            </rad:RadToolTip>
                                      <asp:Panel runat="server" ID="StuffPanel" Visible="false">
                                            <table style="padding-left: 10px;">
                                                <tr>
                                                    <td colspan="2">
                                                     <asp:CheckBox runat="server" ID="StuffCheckableCheckBox" Text="Make list checkable by users (alternatively it will just be a static list.)" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="230px"><br />
                                                        <label>Item Needed:</label> 
                                                        <br />
                                                        <asp:TextBox runat="server" ID="OneStuffNeededTextBox"></asp:TextBox>
                                                        <br /><br />
                                                        <asp:Button runat="server" ID="Button1" Text="Add Item" 
                                                         CssClass="SearchButton"
                                                         OnClick="AddStuffNeed"
                                                         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                         onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                         onclientclick="this.value = 'Working...';" 
                                                         />
                                                         <asp:Label runat="server" ID="Label1" ForeColor="Red" Font-Size="11px"></asp:Label>
                                                    </td>
                                                    <td valign="top">
                                                        <label>Stuff we need</label>
                                                        <div style="border: solid 3px #666666;">
                                                            <asp:ListBox CssClass="StuffBox" ForeColor="white" runat="server" BackColor="#1b1b1b" 
                                                            Width="350px" ID="StuffNeededListBox"></asp:ListBox>
                                                        </div>
                                                        <asp:Button runat="server" ID="Button5" Text="Remove Item" 
                                                         CssClass="SearchButton"
                                                         OnClick="RemoveStuffNeed"
                                                         onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                         onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                         onclientclick="this.value = 'Working...';" 
                                                         />
                                                    </td>
                                                </tr>
                                            </table>
                                      </asp:Panel>
                                  </asp:Panel>
                              </ContentTemplate>
                              </asp:UpdatePanel>
                            </div>
                        </div>
                    
                        <div align="right" style=" padding-top: 30px; padding-right: 50px;">
                            <asp:ImageButton runat="server" ID="ImageButton3" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                            <asp:ImageButton runat="server" ID="MediaOnwardsButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                         </div>
                    
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                </asp:Panel>
            </rad:RadPageView>
<rad:RadPageView runat="server" ID="RadPageView5" TabIndex="3" >
               <asp:Panel runat="server" ID="Panel7" DefaultButton="MediaOnwardsButton2">
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;width: 668px;">
                        <a href="#top"></a>
                        <div class="EventDiv">
                           <div style="padding-left: 30px; width: 610px; clear: both; padding-top: 10px;" class="topDiv">
                              <div class="topDiv" style="clear: both;"><div style="float: left;"><h1><span style='color: #1fb6e7; font-weight: bold;'>* </span>
                               Participants</h1></div><div style="float: left; padding-left: 10px;">
                               <asp:Image CssClass="HelpImage" runat="server"  ID="Image6" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                  <rad:RadToolTip ID="RadToolTip7" runat="server" Skin="Black" Width="200px" 
                                        Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" 
                                        RelativeTo="Element" TargetControlID="Image6">
                                        <label>
                                            You can choose the event to be either one of the three:
                                            
                                            1. Public Event: anyone will be able to find the event in search results and
                                                add it to their calendar. <br />
                                            2. Private Event: open to all members. Not searchable from the 'events' tab.<br />
                                            3. Exclusive Event: only open to select members. Not searchable from the 'events' tab.
                                        </label>
                                        </rad:RadToolTip></div> 
                                        </div>
                                 <div>       
                               <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                <asp:Label runat="server" ID="ParticipantsErrorLabel" ForeColor="red"></asp:Label>
                                <asp:Panel runat="server" BorderColor="#666666" 
                                  BorderStyle="solid" BorderWidth="3px">
                                   <asp:RadioButtonList runat="server" AutoPostBack="true" OnSelectedIndexChanged="ChangeUpParticipants" 
                                   RepeatDirection="Horizontal" ID="ParticipantsList">
                                        <asp:ListItem Text="Public Event" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Private Event" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Exclusive Event" Value="3"></asp:ListItem>
                                   </asp:RadioButtonList>
                                   <asp:Panel runat="server" ID="PublicEventPanel"  Visible="false">
                                           <div style="padding-left: 10px;"><h1 class="Blue">Public Event</h1></div>
                                    <div style="margin-top: -5px; padding-bottom: 5px;">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    &nbsp;&nbsp;&nbsp;<label>Your event will be searchable and everyone will be able to add it to their calendar and attend.</label>
                                                     <asp:Label runat="server" ID="Label2" ForeColor="Red" Font-Size="11px"></asp:Label>
                                                </td>
                                                <td valign="top">
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                  </asp:Panel>
                                  <asp:Panel runat="server" ID="PrivateEventPanel"  Visible="false">
                                    <div style="padding-left: 10px;"><h1 class="Blue">Private Event</h1></div>
                                    <div style="margin-top: -5px; padding-bottom: 5px;">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        &nbsp;&nbsp;&nbsp;<label>Your event will be NOT be searchable and only members will be able to add it to their calendar and attend.</label>
                                                         <asp:Label runat="server" ID="Label3" ForeColor="Red" Font-Size="11px"></asp:Label>
                                                    </td>
                                                    <td valign="top">
                                                     </td>
                                                </tr>
                                            </table>
                                    </div>
                                  </asp:Panel>
                                  <asp:Panel runat="server" ID="ExclusiveEventPanel"  Visible="false">
                                    <div style="padding-left: 10px;"><h1 class="Blue">Exclusive Event</h1>
                                    <div style="margin-top: -5px; padding-bottom: 5px;">
                                    &nbsp;&nbsp;&nbsp;<label> Your event will be open to select members only and will NOT be searchable from the 'events' tab.</label>
                                    </div>
                                    <label>Select Your Members</label>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:ListBox runat="server" ID="GroupMembersListBox" 
                                                Width="200px" Height="100px"></asp:ListBox>
                                            </td>
                                            <td width="100px">
                                                <asp:Button runat="server" ID="Button7" Text="Add >" 
                                                     CssClass="SearchButton" 
                                                     OnClick="AddParticipant"
                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                     onclientclick="this.value = 'Working...';" 
                                                     />
                                                     <asp:Button runat="server" ID="Button8" Text="< Remove" 
                                                     CssClass="SearchButton" 
                                                     OnClick="RemoveParticipant"
                                                     onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                     onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                     onclientclick="this.value = 'Working...';" 
                                                     />
                                            </td>
                                            <td>
                                                <asp:ListBox runat="server" Width="200px" Height="100px" ID="GroupMembersSelectedListBox"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                            
                                    <asp:Label runat="server" ID="Label4" ForeColor="Red" Font-Size="11px"></asp:Label>
                                             </div>
                                  </asp:Panel>
                                  <div style="padding: 10px;">
                                  <asp:Panel runat="server" ID="TheGroupingsPanel" BorderColor="#666666" 
                                                BorderStyle="solid" BorderWidth="3px" Visible="false">
                                  <div style="padding-left: 10px; float: left;">
                                  <div style="float: left; padding-top: 3px;"><asp:CheckBox AutoPostBack="true" OnCheckedChanged="OpenGroupings" 
                                  runat="server" ID="GroupingsCheckBox" /></div><div style="float: left;"><h1 class="Blue">
                                  Create Groupings</h1></div>
                                  
                                  </div>
                                  <div style="float: left; padding-left: 10px;">
                               <asp:Image CssClass="HelpImage" runat="server"  ID="Image7" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                  <rad:RadToolTip ID="RadToolTip5" runat="server" Skin="Black" Width="200px" 
                                        Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" 
                                        RelativeTo="Element" TargetControlID="Image7">
                                        <label>
                                            You can organize your event participants into groups. A group can be anything. For example,
                                            if your members need a ride to the event you can group each person under the car they are going to go in.
                                            Or, if you need half of your participants to bring beverages and the other half food, you can
                                            specify a group for each and organize the participants under those two groups.
                                        </label>
                                        </rad:RadToolTip></div>
                                   <div class="topDiv">     
                                            <div style="padding: 10px; float: left; clear: both;">
                                           <asp:Panel runat="server" ID="GroupingPanel" Visible="false">
                                           
                                                <table>
                                                    <tr>
                                                        <td colspan="3">
                                                            <div style="padding-left: 10px; float: left;"><label>
                                        <span style="font-family: Arial; font-size: 11px; color: #ff770d; font-weight: bold;"> 
                                        [Note: switching between Public and Exclusive event will wipe out your groupings. 
                                        So, make sure you decide on the event type before creating the groupings.]</span></label>
                                        </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>Grouping Name </label><br />
                                                            <asp:TextBox runat="server" ID="GroupingNameTextBox"></asp:TextBox> <br />
                                                            <label>Grouping Description</label>
                                                            <asp:TextBox runat="server" TextMode="multiLine" Width="200px" 
                                                            Height="100px" ID="GroupingDescriptionTextBox"></asp:TextBox> <br />
                                                            <br />
                                                            <label>Participants</label>
                                                            <asp:ListBox runat="server" Height="100px" 
                                                            ID="GroupingParticipantsListBox" Width="200px"></asp:ListBox>
                                                        </td>
                                                        <td width="100px">
                                                                <asp:Button runat="server" ID="Button9" Text="Add >" 
                                                                 CssClass="SearchButton" 
                                                                 OnClick="AddGrouping"
                                                                 onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                 onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                 onclientclick="this.value = 'Working...';" 
                                                                 />
                                                                 <asp:Button runat="server" ID="Button10" Text="< Remove" 
                                                                 CssClass="SearchButton" 
                                                                 OnClick="RemoveGrouping"
                                                                 onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                 onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                 onclientclick="this.value = 'Working...';" 
                                                                 />
                                                                 <br /><br /><br /><br /><br /><br /><br /><br />
                                                                 <asp:Button runat="server" ID="Button11" Text="Assign >" 
                                                                 CssClass="SearchButton" 
                                                                 OnClick="AssignParticipantGrouping"
                                                                 onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                 onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                 onclientclick="this.value = 'Working...';" 
                                                                 />
                                                                 <asp:Button runat="server" ID="Button12" Text="< Remove" 
                                                                 CssClass="SearchButton" 
                                                                 OnClick="RemoveParticipantGrouping"
                                                                 onmouseout="this.style.backgroundImage='url(image/PostButtonNoPost.png)'" 
                                                                 onmouseover="this.style.backgroundImage='url(image/PostButtonNoPostHover.png)'"
                                                                 onclientclick="this.value = 'Working...';" 
                                                                 />
                                                        </td>
                                                        <td width="200px">
                                                            <label>Groupings</label><br />
                                                            <rad:RadTreeView BorderColor="#666666" BorderStyle="solid" 
                                                            BorderWidth="3px" Height="300px" runat="server" Skin="WebBlue" 
                                                            ID="GroupingsTreeVie">
                                                            </rad:RadTreeView>
                                                        </td>
                                                    </tr>
                                                </table>
                                                
                                                <asp:Label runat="server" ID="GroupingErrorLabel" ForeColor="red"></asp:Label>
                                           </asp:Panel>
                                           </div>
                                           </div>
                                  </asp:Panel>
                                  </div>
                                  <div style="padding: 10px;">
                                  <asp:Panel runat="server" ID="RegistrationPanel" BorderColor="#666666" 
                                                BorderStyle="solid" BorderWidth="3px" Visible="false">
                                  <div style="padding-left: 10px;"><h1 class="Blue"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Registration Type</h1></div>
                                   <div class="topDiv">     
                                               <div style="float: left; padding-left: 10px;"> <asp:RadioButtonList runat="server" RepeatDirection="horizontal" 
                                                ID="RagistrationRadioList" AutoPostBack="true" OnSelectedIndexChanged="ChangeRegist">
                                                    <asp:ListItem Text="Unlimited Registration" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Limited Registration" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList></div>
                                                <div style="float: left; padding-left: 10px;">
                               <asp:Image CssClass="HelpImage" runat="server"  ID="Image5" ImageUrl="~/image/helpIcon.png"></asp:Image>
                                  <rad:RadToolTip ID="RadToolTip6" runat="server" Skin="Black" Width="200px" 
                                        Height="200px" ManualClose="true" ShowEvent="onclick" Position="MiddleRight" 
                                        RelativeTo="Element" TargetControlID="Image5">
                                        <label>
                                            You can either provide a limit on the number of people that add this event to their calendar (i.e. register), 
                                            or give a deadline before which they can register, or both.
                                        </label>
                                        </rad:RadToolTip></div>
                                            <div style="padding: 10px; float: left; clear: both;">
                                           <asp:Panel runat="server" ID="LimitedRegistrationPanel" Visible="false">
                                                <table>
                                                    <tr>
                                                        <td width="200px">
                                                             <label>Limit of users to register</label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox Width="50px" 
                                                runat="server" ID="NumRegTextBox"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="NumRegTextBox" 
                                                            Type="integer" ErrorMessage="Number of users must be a number." 
                                                            MinimumValue="1" MaximumValue="9999999"></asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>Registration deadline</label>
                                                        </td>
                                                        <td>
                                                            <rad:RadDatePicker runat="server" ID="DeadlineDatePicker"></rad:RadDatePicker>
                                                        </td>
                                                    </tr>
                                                </table>
                                                
                                                <asp:Label runat="server" ID="RegErrorLabel" ForeColor="red"></asp:Label>
                                           </asp:Panel>
                                           </div>
                                           </div>
                                  </asp:Panel>
                                  </div>
                                </asp:Panel>
                              </ContentTemplate>
                              </asp:UpdatePanel>
                                 </div>
                              <br />
                           </div>
                        </div>
                    
                        <div align="right" style=" padding-top: 5px; padding-right: 50px;">
                            <asp:ImageButton runat="server" ID="ImageButton4" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                            <asp:ImageButton runat="server" ID="MediaOnwardsButton2" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                         </div>
                    
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                </asp:Panel>
            </rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView3" TabIndex="3" >
                
                <asp:Panel runat="server" ID="Panel4" DefaultButton="CategoryOnwardButton">          
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div class="EventDiv" style="background: url('image/TabsBackground.png'); background-repeat: repeat-y;  width: 668px;">
                        <div style="padding-left: 30px; width: 550px; padding-top: 10px;">
                        <h1>Categories</h1>
                        <div style="padding-left: 20px;">
                            <label><span style="font-weight: bold;"><span style='color: #1fb6e7; font-weight: bold;'>* </span>Choose Categories to put your group under. </span></label>
                            <br /><br />
<table>
                                        <tr>
                                            <td valign="top">
                                                    <%--<asp:CheckBoxList runat="server" ID="CategoriesCheckBoxes" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>--%>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Width="200px" runat="server"  
                                                        ID="CategoryTree" DataFieldID="ID" 
                                                        ForeColor="#cccccc"
                                                        DataSourceID="SqlDataSource1" CheckChildNodes="false" CheckBoxes="true">
                                                        
                                                        <DataBindings>
                                                            
                                                             <rad:RadTreeNodeBinding Checkable="true" Checked="false"
                                                             TextField="GroupName" Expanded="false" ValueField="ID" />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" 
                                                            SelectCommand="SELECT * FROM GroupCategories WHERE ID <= 12 ORDER BY GroupName ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                                    
                                               
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBoxList runat="server" ID="CheckBoxList1" 
                                                    RepeatColumns="4" RepeatDirection="horizontal">
                                                    
                                                    </asp:CheckBoxList>
                                                    <div class="FloatLeft" style="width: 150px;">
                                                        <rad:RadTreeView Width="200px" ForeColor="#cccccc" runat="server"  
                                                        ID="RadTreeView2" DataFieldID="ID" DataSourceID="SqlDataSource3"
                                                        DataFieldParentID="Parent"
                                                        CheckBoxes="true">
                                                        <DataBindings>
                                                             <rad:RadTreeNodeBinding  Checkable="true" Checked="false" 
                                                             TextField="GroupName" Expanded="false" ValueField="ID"  />
                                                         </DataBindings>
                                                        </rad:RadTreeView>
                                                        <asp:SqlDataSource runat="server" ID="SqlDataSource3" 
                                                            SelectCommand="SELECT * FROM GroupCategories WHERE ID > 12 ORDER BY GroupName ASC"
                                                            ConnectionString="<%$ ConnectionStrings:Connection %>">
                                                        </asp:SqlDataSource>
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>                        </div>
                        <div style="padding-left: 30px; width: 550px; clear: both;">
                            <div style="float: left; padding-top: 10px;">
                                <label>Don't see a category you want? Suggest one!</label>
                                <asp:TextBox runat="server" ID="CategoriesTextBox"></asp:TextBox>
                            </div>
                            <div style="padding-top: 5px;float: right;">
                                <asp:ImageButton runat="server" ID="ImageButton11" OnClick="SuggestCategoryClick"
                                                             ImageUrl="image/MakeItSoButton.png" 
                                                            onmouseout="this.src='image/MakeItSoButton.png'" onmouseover="this.src='image/MakeItSoButtonSelected.png'"  />
                            </div>
                        </div>
                        </div>
                        <div align="right" style=" padding-top: 30px; padding-right: 50px; clear: both;">
                            <asp:ImageButton runat="server" ID="ImageButton9" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                            <asp:ImageButton runat="server" ID="CategoryOnwardButton" OnClick="Onwards" ImageUrl="image/OnwardsButton.png" 
                            onmouseout="this.src='image/OnwardsButton.png'" onmouseover="this.src='image/OnwardsButtonSelected.png'"  />
                         </div>
                    
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                </asp:Panel>
            </rad:RadPageView>
            <rad:RadPageView runat="server" ID="RadPageView4" TabIndex="5" >
                <asp:Panel runat="server" ID="Panel5" DefaultButton="PostItButton"> 
                    <div style="background: url('image/TabsBackgroundTop.png'); background-repeat: no-repeat; width: 668px; height: 4px;">
                    </div>
                    <div class="EventDiv"  style="background: url('image/TabsBackground.png'); background-repeat: repeat-y; width: 668px;">
                        <div style="padding-left: 30px; width: 550px; padding-top: 10px;">
                        <h1>Post It</h1>
                        </div>
                        <div style="padding-left: 50px;">
                                                    <label><span style="font-weight: bold;">You're Almost Done!</span> <br /><span style='color: #ff770d;'>Read the Terms and Conditions and Go Backwards! if you'd like to change anything!</span></label>
                                                    
                            <br />
                            <label>Terms & Conditions</label>
<div style="margin-top: 10px;margin-bottom: 10px;border: solid 2px #1B1B1B; padding-top: 20px; padding-bottom: 20px; background-color: #666666; width: 580px;">    
                            <asp:Panel runat="server" CssClass="TermsPanel"  ScrollBars="Vertical" ID="TACTextBox" Height="100px" Width="550px"></asp:Panel>
                            </div>  <%--                            <asp:TextBox runat="server" TextMode="MultiLine" Wrap="true" ID="TACTextBox" Height="100px" Width="600px"></asp:TextBox><br />
--%>                            <asp:CheckBox runat="server" ID="AgreeCheckBox" Text="<span style='color: #1fb6e7; font-weight: bold;'>* </span>Agree to the Terms & Conditions" />
                            
                            
                        </div>
                        <div>
                            <asp:Literal runat="server" ID="PostLiteral"></asp:Literal>
                        </div>
                        <div align="right" style=" padding-top: 30px; padding-right: 50px;">
                            <asp:ImageButton runat="server" ID="ImageButton12" OnClick="Backwards" ImageUrl="image/BackwardsButton.png" 
                            onmouseout="this.src='image/BackwardsButton.png'" onmouseover="this.src='image/BackwardsButtonSelected.png'"  />
                            <asp:ImageButton runat="server" ID="PostItButton" OnClick="PostIt" ImageUrl="image/PostItButton.png" 
                            onmouseout="this.src='image/PostItButton.png'" onmouseover="this.src='image/PostItButtonSelected.png'"  />
                         </div>
                    </div>
                    <div align="center" style=" margin-left: 11px ;background-color: #515151 ;height: 4px; width: 640px;"></div>
                
                </asp:Panel>
             </rad:RadPageView>

        </rad:RadMultiPage>
        </div>
        <div style="float: right;">
            <asp:Panel runat="server" ID="MessagePanel" Visible="false">
                <div style="width: 170px; border: solid 3px red; margin-right: 10px; padding: 5px; color: White; font-family: Arial;">
                    Your Messages <br />
                    <asp:Label CssClass="AddLink" runat="server" ID="YourMessagesLabel"></asp:Label>
                </div>
            </asp:Panel>
        </div>
    </div>
    </asp:Panel>
    </div>
             <%-- </ContentTemplate>--%>
<%--</asp:UpdatePanel>--%>
<%--</rad:RadAjaxPanel>--%>
</asp:Content>

