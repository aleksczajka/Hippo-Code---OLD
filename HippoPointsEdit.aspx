<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HippoPointsEdit.aspx.cs" Inherits="HippoPointsEdit" %>
<%@ Register Src="~/Controls/BlueButton.ascx" TagName="BlueButton" TagPrefix="ctrl"%>
<%@ OutputCache Duration="1" NoStore="true" VaryByParam="*" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body>

    <form id="form1" runat="server">
         <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true" >
    </asp:ScriptManager>
         <script type="text/javascript">
               function GetRadWindow() 
                { 
                  var oWindow = null; 
                  if (window.radWindow) 
                     oWindow = window.radWindow; 
                  else if (window.frameElement.radWindow) 
                     oWindow = window.frameElement.radWindow; 
                  return oWindow; 
                }   
                function CloseWindow(args)
                {
                
                    var oWindow = GetRadWindow(); 
                    var oArg = args;
                    
                    //oArg.Name = args;
                    oWindow.close(args); 
                }
              
                function Search(args) 
                { 
                    CloseWindow(args);
                } 
                
                function CountCharsHeadline(editor, e){
                    
                        var theDiv = document.getElementById("CharsDivHeadline");
                        var theText = document.getElementById("DescriptionTextBox");
                        theDiv.innerHTML = "characters left: "+ (430 - theText.value.length).toString();
                    
                }

         </script>
    <div style="vertical-align: middle; height: 100%;">
        <div class="Text12">
            <div class="topDiv">
                    <div style="float: left;">
                        <h1>Edit Your Hippo Point Information</h1>
                    </div>
                    <div style="float: left;padding-top: 12px; padding-left: 10px;">
                        <a class="NavyLink12" onclick="Search('hippo-points');">What is Hippo Points?</a>
                    </div>
            </div>
            <h2>Terms & Conditions</h2>
            <div style="margin-top: 10px;margin-bottom: 10px; border: solid 1px #dedbdb; padding-top: 5px; padding-bottom: 5px; width: 500px;">    
            <asp:Panel runat="server" CssClass="TermsPanel"  ScrollBars="Vertical" ID="TACTextBox" Height="50px" Width="470px"></asp:Panel>
            </div>
            <div class="topDiv" style="padding-bottom: 10px;">
                <div style="float: left;padding-right: 5px;">
                    <input runat="server" id="Checkbox1" type="checkbox" />
                </div>
                <div style="float: left; width: 490px;">
                    I would like to be entered in the Hippo Points and agree to 
                    the Hippo Points Terms and Conditions.
                </div>
            </div>
            <asp:Panel runat="server" ID="EnteredPanel">
                <div class="topDiv">
                    <div style="float: left; margin-right: 10px;">
                        <div class="topDiv">
                            <div style="float: left;">
                                <h2>Paragraph to Display [limit 430]</h2>
                            </div>
                            <div style="float: left; padding-left: 5px;padding-top: 3px;">
                                <asp:Image CssClass="HelpImage" runat="server"  ID="Image5" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                                <rad:RadToolTip ID="RadToolTip6" runat="server" Skin="Sunset" 
                                Width="200px" ManualClose="true" ShowEvent="onclick" 
                                Position="MiddleRight" RelativeTo="Element" TargetControlID="Image5">
                                <div style="padding: 10px;"><label>Enter here the text you would like displayed on our home page if you win. 
                                This could be anything that you are trying to promote: a service you 
                                provide, a company you own or even your own website.</label></div>
                                </rad:RadToolTip>
                            </div>
                        </div>
                        
                        <asp:TextBox runat="server" onkeypress="CountCharsHeadline(event)" ID="DescriptionTextBox" TextMode="MultiLine" Width="200px" Height="70px"></asp:TextBox>
                        <br />
                        <span class="NavyLink12"><span style="color: Red;" id="CharsDivHeadline"></span></span>
                        <h2>Link to Go To</h2>
                        <asp:TextBox runat="server" ID="LinkTextBox" Width="200px"></asp:TextBox>

                    </div>
                    <div style="float: left;border-left: solid 1px #dedbdb;padding-left: 10px;">
                    <div class="topDiv">
                        <div style="float: left;">
                            <h2>Image to Display [resized to: 200 x 130]</h2>
                        </div>
                        <div style="float: left; padding-left: 5px;padding-top: 3px;">
                            <asp:Image CssClass="HelpImage" runat="server"  ID="Image1" ImageUrl="~/NewImages/HelpIconNew.png"></asp:Image>
                            <rad:RadToolTip ID="RadToolTip1" runat="server" Skin="Sunset" 
                            Width="200px" ManualClose="true" ShowEvent="onclick" 
                            Position="MiddleRight" RelativeTo="Element" TargetControlID="Image1">
                            <div style="padding: 10px;"><label>Enter here the image you want to be displayed along with your paragraph.
                            If your image is bigger, it will be resized to 200px x 130px. Your image could be anything from a picture
                            of you to a picture of your service. Mind you that the picture should be something that will help promote your 
                            business, service, your site, etc.</label></div>
                            </rad:RadToolTip>
                        </div>
                    </div>
                    <div class="topDiv">
                        <div style="float: left;padding-right: 3px;">
                            <asp:FileUpload Width="225px" runat="server" ID="FileUpload" />
                        </div>
                        <div style="float: left;">
                            <ctrl:BlueButton runat="server" ID="UploadButton" BUTTON_TEXT="Upload" />
                        </div>
                        <div style="padding-top: 10px;clear: both;">
                            <div style="float: left; padding-right: 10px;">
                                <asp:Image runat="server" ID="TheImage" />
                            </div>
                            <div style="float: left;">
                                <ctrl:BlueButton ID="PictureNixItButton" Visible="false" runat="server" BUTTON_TEXT="Nix It" />
                            </div>
                        </div>
                    </div>
                    
                    </div>
                    <div style="clear: both;position: fixed; bottom: 45px;">
                        <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                
            </asp:Panel>
            <div style="bottom: 35px; position: fixed;">
                <div style="right: 10px; position: fixed;">
                    <ctrl:BlueButton runat="server" ID="BlueButton1" CLIENT_CLICK="Search('my-account')" BUTTON_TEXT="Done" />
                </div>
                <div style="left: 10px; position: fixed;"><ctrl:BlueButton runat="server" ID="BlueButton2" BUTTON_TEXT="Save" /></div>
            </div>
                
            
        </div>
        
    </div>
    </form>
</body>
</html>
