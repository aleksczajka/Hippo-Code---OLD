<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MeaslesChart.aspx.cs" Inherits="MeaslesChart" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/StyleSheet.css" type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #333333; scrollbar-base-color: #666666; scrollbar-shadow-color: #666666; scrollbar-highlight-color: #666666; scrollbar-darkshadow-color: #666666; scrollbar-3dlight-color: #666666; scrollbar-track-color: #999999; scrollbar-arrow-color: #363636;">
    <form id="form1" runat="server">
     
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
                function CloseWindow()
                {
                
                    var oWindow = GetRadWindow(); 
                    oWindow.Close(); 
                }
              
                function Search() 
                { 
                    CloseWindow();
                } 
                
              
         </script>
    <div class="MessageDiv">
      <table>
        <tr>
            <td>
            
              
       <table width="100%" cellpadding="0" cellspacing="0">
            
            <tr>
                <td style="padding-top: 10px;">
                    <div>
                        <table cellpadding="0" cellspacing="0">
                            <tr class="MeaslesHeader">
                                <td>
                                    
                                </td>
                                <td class="HeaderColumn" valign="top">
                                    Free Classifieds
                                </td>
                                <td class="HeaderColumn" valign="top">
                                    Normal Featured Classifieds
                                </td>
                                <td class="HeaderColumn" valign="top">
                                    Big Featured Classifieds
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumn" valign="bottom" >
                                    Searchable from Ads & Classifieds tab
                                </td>
                                <td class="XColumn ContentColumn">
                                    X
                                </td>
                                <td class="XColumn ContentColumn">
                                    X
                                </td>
                                <td class="XColumn ContentColumn">
                                    X
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumnAlt">
                                    Ad Statistics
                                </td>
                                <td class="ContentColumnAlt">
                                
                                </td>
                                <td class="XColumn ContentColumnAlt">
                                    X
                                </td>
                                <td class="XColumn ContentColumnAlt">
                                    X
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumn" valign="bottom" >
                                    Ad emailed to interested users (i.e. Saved Ad Search)
                                </td>
                                <td class="ContentColumn">
                                    
                                </td>
                                <td class="XColumn ContentColumn">
                                    X
                                </td>
                                <td class="XColumn ContentColumn">
                                    X
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumnAlt">
                                    30-day limit
                                </td>
                                <td class="XColumn ContentColumnAlt">
                                    X
                                </td>
                                <td class="ContentColumnAlt">
                                    
                                </td>
                                <td class="ContentColumnAlt">
                                    
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumn" valign="bottom" >
                                    Edit at any time
                                </td>
                                <td class="XColumn ContentColumn">
                                    X
                                </td>
                                <td class="XColumn ContentColumn">
                                    X
                                </td>
                                <td class="XColumn ContentColumn">
                                    X
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumnAlt">
                                    Multimedia
                                </td>
                                <td class="XColumn ContentColumnAlt">
                                    X
                                </td>
                                <td class="XColumn ContentColumnAlt">
                                    X
                                </td>
                                <td class="XColumn ContentColumnAlt">
                                    X
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumn" valign="bottom" >
                                    Banner Ads Location
                                </td>
                                <td class="NAColumn ContentColumn">
                                    --
                                </td>
                                <td class="ContentColumn">
                                    Home page, Search Pages, etc.
                                </td>
                                <td class="ContentColumn">
                                    My Account, My Calendar, Groups
                                </td>
                            </tr>
                             <tr class="MeaslesRow">
                                <td class="NameColumnAlt">
                                    Size
                                </td>
                                <td class="NAColumn ContentColumnAlt">
                                    --
                                </td>
                                <td class="ContentColumnAlt">
                                    width: 214px, height: 268px
                                </td>
                                <td class="ContentColumnAlt">
                                    width: 419px, height: 206px
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumn" valign="bottom" >
                                    Cost
                                </td>
                                <td class="ContentColumn">
                                    Free
                                </td>
                                <td class="ContentColumn">
                                    <span style="text-decoration: line-through; color: #cccccc;">$0.01 / view</span> Now Free
                                </td>
                                <td class="ContentColumn">
                                    <span style="text-decoration: line-through; color: #cccccc;">$0.04 / view</span> Now Free
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumnAlt">
                                    Banner repeats per day
                                </td>
                                <td class="NAColumn ContentColumnAlt">
                                    --
                                </td>
                                <td class="ContentColumnAlt">
                                    every 10 mins
                                </td>
                                <td class="ContentColumnAlt">
                                    every 10 mins
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumn" valign="bottom" >
                                    Banner ad stays on for
                                </td>
                                <td class="NAColumn ContentColumn">
                                    --
                                </td>
                                <td class="ContentColumn">
                                    20secs
                                </td>
                                <td class="ContentColumn">
                                    20secs
                                </td>
                            </tr>
                            <tr class="MeaslesRow">
                                <td class="NameColumnAlt">
                                    Banner set-up
                                </td>
                                <td class="NAColumn ContentColumnAlt">
                                    --
                                </td>
                                <td class="ContentColumnAlt">
                                    with 3 other ads
                                </td>
                                <td class="ContentColumnAlt">
                                    by itself
                                </td>
                            </tr>
                            
                            
                            
                           
                            
                        </table>
                    </div>
                </td>
            </tr>
        </table>  
                             </td>
            <td align="right" valign="top" style="padding-left: 10px;padding-bottom: 7px; margin: 0;">
               
                    <a class="AddLink" onclick="Search();" style="text-decoration: underline;">close</a>

            </td>
        </tr>
      </table>
    </div> 
    </form>
</body>
</html>
