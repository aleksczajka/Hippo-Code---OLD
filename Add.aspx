<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
ValidateRequest="false" MaintainScrollPositionOnPostback="true"
CodeFile="Add.aspx.cs" Inherits="Add" smartnavigation="true"
Title="Add events locales trips and bulletins on HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ Register TagName="GreenButton" TagPrefix="ctrl" Src="~/Controls/GreenButton.ascx" %>
<%@ Register TagName="SmallButton" TagPrefix="ctrl" Src="~/Controls/SmallButton.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <%-- <rad:RadAjaxPanel runat="server" >  --%> 
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
 </script>
 <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>  
       <div class="topDiv aboutLink"><div class="FloatLeft"><h1>Tell us what's Happening - Add a Happening!</h1></div>
       <%--<div class="Add10"><ctrl:GreenButton CLIENT_CLICK="GoTo('feature');" ID="BlueButton3" runat="server" BUTTON_TEXT="Feature an Event, Locale, Adventure or Bulletin" /></div>--%>
       </div>
       <table cellpadding="0" cellspacing="0">
        <tr>
            <td class="Add1" valign="middle">
                <div class="Add2">
                    <h2 class="HippoH22"><a href="javascript:GoTo('blog-event');" 
                    class="NavyLink">Add an Event</a></h2>
                </div>
            </td>
            <td width="510px" class="Add3" valign="middle">
                <label>
                    Use our site to promote your events for free. An event on HippoHappenings could be absolutely 
                    anything that is local, open to the public and takes place at a public locale (Friday night 
                    dance party, open mic, book signing, public paint party, car wash fund raiser, 
                    civil war reenactment, anything!). Members of HippoHappenings will be able to add your event 
                    to their calendar, share it with their friends, communicate with members going to the event, 
                    have your event recommended to them via email or their My Account if it falls under their interests
                    and more. Your event will also be randomly included in our 'Stuff To Do' strip on the home page.
                </label>
            </td>
        </tr>
        <tr>
            <td class="Add4" valign="middle">
                <div class="Add5">
                    <h2 class="HippoH22"><a href="javascript:GoTo('enter-locale');" 
                    class="NavyLink">Add a Locale</a></h2>
                </div>
            </td>
            <td width="510px" class="Add6" valign="middle">
                <label>
                    Just like with events, you can use our site to promote your locale for free. A locale can be 
                    any place that is open to the public and hosts events. Members of HippoHappenings will be able 
                    to add your locale to their favorites and receive notifications of new events at your locale. 
                    You can also add specific events — like the ones above — at your locale (but, make sure first that 
                    they don't already exist; someone else might have posted them already). However, for any regular
                    events happening at your locale, like 'happy hour', please specify these as you post your locale.
                    Regular events at your locale as well as your locale
                    itself (provided it's hours fall under the time the site is viewed) will be included 
                    in our 'Stuff To Do' strip on the home page randomly.
                </label>
            </td>
        </tr>
        <tr>
            <td class="Add4" valign="middle">
                <div class="Add7">
                    <h2 class="HippoH22"><a href="javascript:GoTo('enter-trip');" 
                    class="NavyLink">Add an Adventure</a></h2>
                </div>
            </td>
            <td width="510px" valign="middle" class="Add6">
                <label>
                    <b>[Patent Pending]</b> Posting of your adventure is entirely free on HippoHappenings. 
                    Your adventure will be included in our 'Stuff To Do' strip on the home page randomly. 
                    The local adventures feature aims to help our viewers discover their city and find 
                    things to do outside of going to an event or a locale. An adventure could be anything 
                    to do locally that requires going from one destination to another: a great hiking 
                    trail you've discovered, a scenic route you loved to take while exploring a specific 
                    city, or a great tour of city landmarks that you think should be seen in a specific 
                    order, and so on. Using the adventures feature to promote a museum, park or any landmark 
                    by including an adventurous route is greatly welcome as well.    
                </label>
            </td>
        </tr>
        <%--<tr>
            <td class="Add4" valign="middle">
                <div class="Add8">
                    <h2 class="HippoH22"><a href="javascript:GoTo('post-bulletin');" 
                    class="NavyLink">Add a Bulletin</a></h2>
                </div>
            </td>
            <td width="510px" valign="middle" class="Add6">
                <label>
                    Posting your bulletin is free on HippoHappenings, however, there is a huge distinction between normal bulletins and featured bulletins. A bulletin could be anything as long as it has a local focus: advertise a garage sale, promote your local service, a notice for a lost/found animal, a band looking for musicians, pretty much anything whatever you want to raise awareness of. A free bulletin is found by searching on our bulletin search page. A featured bulletin shows up on a strip on our home page and all throughout the site. Featured bulletins are also emailed to our members when they fall under the member's saved search criteria, and it doesn't even depend on what date you choose to feature your bulletin.
                </label>
            </td>
        </tr>--%>
        
       </table>
       <div class="Add9">
            <label>*Posting of happenings is open to everyone on HippoHappenings.com. We do, however, request that all happenings be local or applicable locally; i.e. postings cannot be for purely commercial purposes applicable nationwide or globally. For the full Terms and Conditions on posting happenings please visit our
            <a class="NavyLink12UD" href="terms-and-conditions">Terms & Condition Page.</a></label>
       </div>
</asp:Content>

