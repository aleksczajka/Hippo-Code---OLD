<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
ValidateRequest="false" MaintainScrollPositionOnPostback="true"
CodeFile="Feature.aspx.cs" Inherits="Feature" smartnavigation="true"
Title="Feature event locale trip and bulletin on HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ Register TagName="GreenButton" TagPrefix="ctrl" Src="~/Controls/GreenButton.ascx" %>
<%@ Register TagName="SmallButton" TagPrefix="ctrl" Src="~/Controls/SmallButton.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <%-- <rad:RadAjaxPanel runat="server" >  --%> 
            <rad:RadWindowManager ShowContentDuringLoad="true" VisibleTitlebar="false" 
                KeepInScreenBounds="true" VisibleOnPageLoad="false"  Behaviors="none"
                ID="MessageRadWindowManager" Modal="true" RestrictionZoneID="RestrictionZone"
                runat="server">
                <Windows>
                    <rad:RadWindow Width="600" ClientCallBackFunction="FillVenueJS" 
                     VisibleStatusbar="false" Skin="Web20" Height="500" ID="RadWindow1" 
                     runat="server">
                    </rad:RadWindow>
                </Windows>
            </rad:RadWindowManager>
 <script type="text/javascript">
    function setFormAction()
    {
        var a = document.getElementById('aspnetForm');
        a.action = 'https://www.paypal.com/cgi-bin/webscr';
    }
    function GoTo(url)
    {
        window.location = url;
    }
    function OpenFeaturePricing(page)
           {
                var win = $find("<%=RadWindow1.ClientID %>");
                win.setUrl('PricingChart.aspx?P=' + page);
                win.show(); 
                win.center(); 
           }
 </script>
 <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>  
       <h1>Which Happening Would You Like To Feature?</h1>
       <div class="TextNormal Feature1" align="center">
        <div  class="Feature2">
            <h2 class="HippoH2">Featuring an event, locale, adventure or bulletin differs 
            from standard free posts by being, well, featured. Read on for the details of each.</h2>
        </div>
       </div>
       <table cellpadding="0" cellspacing="0">
        <tr>
            <td class="Feature3" valign="middle">
                <div class="Feature4">
                    <h2 class="HippoH22"><a href="javascript:GoTo('SearchesAndPages.aspx#events');" class="NavyLink">Feature an Existing Event</a></h2>
                </div>
                <br />
                <div class="Feature5">
                    <h2 class="HippoH22"><a href="javascript:GoTo('BlogEvent.aspx');" class="NavyLink">Feature a New Event</a></h2>
                </div>
            </td>
            <td width="510px" class="Feature6" valign="middle">
                <label>
                    Your featured event will come up first in our event search results on the event search page and come                     up in an event's side bar when viewing a similar event.                   In addition, your event will be shown first on our home page 'Stuff To Do' strip if it falls under the dates                     a viewer specifies (i.e. Today, Tomorrow, etc.). To feature an event you have already posted select                     'Feature an Existing Event'. To post the event along with making it Featured select 'Feature a New Event'.
                    <div class="Feature7">
                        <ctrl:BlueButton CLIENT_CLICK="OpenFeaturePricing('E')" runat="server" BUTTON_TEXT="Check Pricing >" />
                    </div>
                </label>
            </td>
        </tr>
        <tr>
            <td class="Add4" valign="middle">
                <div class="Feature8">
                    <h2 class="HippoH22"><a href="javascript:GoTo('SearchesAndPages.aspx#locales');" class="NavyLink">Feature an Existing Locale</a></h2>
                </div>
                <br />
                <div class="Feature9">
                    <h2 class="HippoH22"><a href="javascript:GoTo('EnterVenue.aspx');" class="NavyLink">Feature a New Locale</a></h2>
                </div>
            </td>
            <td width="510px" class="Add6" valign="middle">
                <label>
                    Your featured locale will come up first in our locale search results on the locale search page and come                     up in a locale's side bar when viewing a similar locale. In
                    addition, any regular happenings (happy hour, Friday dance night, etc. ) happening at the locale will be
                    featured on our home page 'Stuff To Do' strip. You can also feature a specific event at a locale; for this, use
                    the 'Feature an Event' feature above.    
                    <div class="Feature7">
                        <ctrl:BlueButton ID="BlueButton1" CLIENT_CLICK="OpenFeaturePricing('V')" runat="server" BUTTON_TEXT="Check Pricing >" />
                    </div>            
                </label>
            </td>
        </tr>
        <tr>
            <td class="Add4" valign="middle">
                <div class="Feature10">
                    <h2 class="HippoH22"><a href="javascript:GoTo('SearchesAndPages.aspx#trips');" class="NavyLink">Feature Existing Adventure</a></h2>
                </div>
                <br />
                <div class="Feature10">
                    <h2 class="HippoH22"><a href="javascript:GoTo('enter-trip');" class="NavyLink">Feature a New Adventure</a></h2>
                </div>
            </td>
            <td width="510px" valign="middle" class="Add6">
                <label>
                    <b>[Patent Pending]</b> Your featured adventure will come up first in our adventure search results on the adventure search page and come                     up in an adventure's side bar when viewing a similar adventure. In                     addition, your adventure will be shown first on our home page 'Stuff To Do' strip if it falls under the dates                     a viewer specifies (i.e. Today, Tomorrow, etc.). To feature an adventure you have already posted select 'Feature                    Existing Adventure'. Otherwise, select 'Feature a New Adventure'. 
                    <div class="Feature7">
                        <ctrl:BlueButton ID="BlueButton3" CLIENT_CLICK="OpenFeaturePricing('T')" runat="server" BUTTON_TEXT="Check Pricing >" />
                    </div>                 
                </label>
            </td>
        </tr>
        <tr>
            <td class="Add4" valign="middle">
                <div class="Feature11">
                    <h2 class="HippoH22"><a href="javascript:GoTo('SearchesAndPages.aspx#bulletins');" class="NavyLink">Feature an Existing Bulletin</a></h2>
                </div>
                <br />
                <div class="Feature11">
                    <h2 class="HippoH22"><a href="javascript:GoTo('post-bulletin?Feature=true');" class="NavyLink">Feature a New Bulletin</a></h2>
                </div>
            </td>
            <td width="510px" valign="middle" class="Add6">
                <label>
                    Your featured bulletin will show in a strip on our home page, all throughout our site and come                     up in a bulletin's side bar when viewing a similar bulletin. Your bulletin will also
                                     come up first on bulletin search results. In addition, members have the ability to receive emails with new bulletins
                                     matching their criteria via a saved bulletin search. Only featured bulletins are sent via the saved search, plus,                                      saved searches are not dependent on the day you choose to feature your event. A fetured event will be sent to a member                                      whenever it falls under the member's saved search criteria. To feature a bulletin you have already posted select 'Feature an                    Existing Bulletin'. Otherwise, select 'Feature a New Bulletin'.     
                    <div class="Feature7">
                        <ctrl:BlueButton ID="BlueButton2" CLIENT_CLICK="OpenFeaturePricing('A')" runat="server" BUTTON_TEXT="Check Pricing >" />
                    </div>              
                </label>
            </td>
        </tr>
        
       </table>
       <div class="Add9">
            <label>*Posting and Featuring of happenings and bulletins is open to everyone on HippoHappenings.com. We do, however, request that all happenings and bulletins be local or applicable locally; i.e. postings cannot be for purely commercial purposes applicable nationwide or globally. For the full Terms and Conditions on posting and featuring happenings please visit our
            <a class="NavyLink12UD" href="terms-and-conditions">Terms & Condition Page.</a></label>
       </div>
</asp:Content>

