<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" EnableViewState="false" AutoEventWireup="true" 
CodeFile="About.aspx.cs" Inherits="About" Title="About HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="TextNormal PagePad">
  <h1 id="about-HippoHappenings">Who We Are and Our Mission</h1>
   <div class="TextNormal" align="center">
        <div class="PageIntro">
        Find Something Happening. Make Something Happen.
        </div>
       </div>
    <div class="TextNormal AboutTimer">
        <div class="TimerInside"><asp:Timer ID="MissionTimer" OnTick="ChangeMission" runat="server" Interval="15000">
                            </asp:Timer>
                                <asp:UpdatePanel runat="server" UpdateMode="conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="MissionTimer" EventName="Tick" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="TimerIn">
                                            <asp:Literal runat="server" ID="MissionLiteral"></asp:Literal>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel></div>
    <div class="AboutIntro">
            By now you've probably guessed that we're about happenings! That's true, but is there more? 
            Definitely!
            <br /><br />
            Our mission is to provide you with a place to find and share everything that is happening in your area, but we do it quite a bit differently than any other sites out there. Our aim is to provide you not just with events occurring at venues, restaurants and other locales (which could be promoted by the locales themselves), but, to give you a way to share cool adventures, trips and the like which are not associated with any organization or sponsor and can be posted, shared and promoted by anyone! 
            <br /><br />
            By making posting of events, locales, trips and bulletins free, we hope that you'll take this advantage and post content that otherwise has no place to call home anywhere else on the internet and help us bring you closer to your city, surroundings and community. Content, like adventures outdoors, garage sales, impromptu comedy acts on the street, events at mom -and -pop locales and many other awesome escapades are all graciously welcomed to HippoHappenings. We hope you post all your happenings with HippoHappenings and call us home. 
    <br /><br />
            Read more about all the possibilities on HippoHappenings below.<br />
      </div>
    </div>
    <div class="Floaty">
  <div class="topDiv Pad40">     
<table width="100%">
    <tr>
        <td valign="top" class="AboutHome">
            <a href="#home-page" class="NavyLink12UD" >Home Page</a>
            <%--<a href="#hippo-points-hippo-bosses" class="NavyLink12UD" >Hippo Points & Hippo Bosses</a>--%>
        </td>
        <td valign="top">
            <a href="#events" class="NavyLink12UD" >Events</a><br />
&nbsp;&nbsp;&nbsp;&nbsp;<a href="#event-recommendations" class="NavyLink12UD" >Event Recommendations</a><br />
&nbsp;&nbsp;&nbsp;&nbsp;<a href="#event-ownership" class="NavyLink12UD" >Event Ownership</a><br />
<%--&nbsp;&nbsp;&nbsp;&nbsp;<a href="#featuring-event" class="NavyLink12UD" >Featuring an Event</a><br />--%>
        </td>
        <td valign="top">
            <a href="#locales" class="NavyLink12UD" >Locales</a><br />
&nbsp;&nbsp;&nbsp;&nbsp;<a href="#locale-ownership" class="NavyLink12UD" >Locale Ownership</a><br />
<%--&nbsp;&nbsp;&nbsp;&nbsp;<a href="#featuring-locale" class="NavyLink12UD" >Featuring a Locale</a><br />--%>
        </td>
        <td valign="top">    
            <a href="#adventures" class="NavyLink12UD" >Adventures</a><br />
&nbsp;&nbsp;&nbsp;&nbsp;<a href="#adventure-ownership" class="NavyLink12UD" >Adventure Ownership</a><br />
<%--&nbsp;&nbsp;&nbsp;&nbsp;<a href="#featuring-adventure" class="NavyLink12UD" >Featuring an Adventure</a><br />--%>
        </td>
<%--        <td valign="top">
            <a href="#bulletins" class="NavyLink12UD" >Bulletins</a><br />
            &nbsp;&nbsp;&nbsp;&nbsp;<a href="#featuring-bulletin" class="NavyLink12UD" >Featuring a Bulletin</a><br />
&nbsp;&nbsp;&nbsp;&nbsp;<a href="#saved-bulletin-search" class="NavyLink12UD" >Saved Bulletin Searches</a><br />
&nbsp;&nbsp;&nbsp;&nbsp;<a href="#in-kind-contributions" class="NavyLink12UD" >In-Kind Contributions</a><br />

        </td>--%>
    </tr>
</table>
</div> 
<div>
<div class="PosRel"><br /><br />
<h2><a id="home-page" class="aboutLink">HOME PAGE</a></h2>

The home page will show you anything there is to do regarding events, locale hours, locale regular events and any adventures during the current month. You do not need to make your content featured in order for it to be displayed on the home page; however, any featured content will make it's way to the front of the line on our "Stuff to Do" strip.


<br/><br/>
The home page is also the host of the local and global Hippo Boss award winners. 
<br /><br />
<%--<h2><a id="hippo-points-hippo-bosses" class="aboutLink">HIPPO POINTS AND HIPPO BOSSES</a></h2>
Each member of HippoHappenings that posts the most content on our site—independent of the content's location—will be featured on our home page (specific to their city/region) for an entire month. As to what text, image and link will be included on the home page for the given winner is entirely up to the winner. The winner can promote a service, business, site, or anything they want on our home page for an entire month. You can read more about Hippo Points and Hippo Bosses on our <a class="NavyLink12UD" href="Hippo-Points.aspx">Hippo Points page</a>.
<br/><br/>--%>
<h2><a id="events" class="aboutLink">EVENTS</a></h2>

Events that have been recommended to you are based on your favorite locations, favorite categories, and events similar to events in your calendar. They are shown on your My Account page and, based on your preferences, emailed to you. These recommendations have icons next to them explain why they are recommended. The event page will also have these icons signifying it as one of your recommended events (if you go to the event page while logged in).


<br/><br/>
<h2><a id="event-recommendations" class="aboutLink">EVENT RECOMMENDATIONS</a></h2>
Events that have been recommended to you based on your favorite location, favorite categories, 
and events similar to events in your calendar are shown on your My Account page and also emailed to you based on your
preferences. These recommendations have icons next to them explain why they are recommended. 
The event page will also have these icons signifying it as one of your recommended events 
(if you go to the event page while you are logged in). 

<br/><br/>
<ul>
    <li>
        <img alt="Favorite locale recommendation" src="image/VenueIcon.png"/>: The event was recommended because it is happening in one of your favorite locales.
    </li>
    <li>
        <img alt="Favorite category recommendation" src="image/CategoryIcon.png"/>: The event was recommended because it matches one or more of your favorite categories. The number inside represents how many of your favorite categories this event matched.
    </li>
    <li>
        <img alt="Similar to events in calendar" src="image/SimilarIcon.png"/>: The event was recommended because it is similar to the events in your calendar.
    </li>
</ul>

<br/><h2><a id="event-ownership" class="aboutLink">EVENT OWNERSHIP</a></h2>
While posting an event, you can choose to be the owner of the event. When you are the owner of an event you are the only person allowed to edit the event. If you choose not to be the owner, any member of HippoHappenings will be able to edit the event that you post. 
<%--<br/>
<br/>
<h2><a id="featuring-event" class="aboutLink">FEATURING AN EVENT</a></h2>
You can choose to feature an event on HippoHappenings. This has bonuses to it: the event will show up first on the 'Stuff to Do' strip on the home page, appear in the top four search results on the event search page and also appear in the side bar of events that are similar to yours. For the full details of featuring an event and what featuring entails visit our <a class="NavyLink12UD" href="termsandconditions.aspx#content-Type">Terms and Conditions</a> page.
--%>
<br/><br/>
<h2><a id="locales" class="aboutLink">LOCALES</a></h2>

Any locale that is associated with public happenings can be posted on HippoHappenings. All locales must have a physical address in order to be posted. Before you post your locale, make sure that it does not already exist in our records already. Remember that whenever you create a locale any other member of HippoHappenings will be able to post events under that locale, of course, they need to be real events and scheduled to occur at the locale. 

<br/><br/><h2><a id="locale-ownership" class="aboutLink">LOCALE OWNERSHIP</a></h2>
When  you post a locale, you are given the choice of being the owner of the locale. As the owner, you are the only person that is allowed to edit and update the locale. If you choose to not be the owner, any member of HippoHappenings will be able to edit and update the locale that you post. 

<%-- <br/><br/>
 <h2><a id="featuring-locale" class="aboutLink">FEATURING A LOCALE</a></h2>
You can feature a locale on HippoHappenings to make it, and its associated regular events, show up first on the 'Stuff to Do' strip on the home page, in top four search results on the locale search page and in the side bar of similar locales on the similar locales' pages. For the full details of featuring a locale and what featuring entails visit our <a class="NavyLink12UD" href="termsandconditions.aspx#content-Type">Terms and Conditions</a> page.
--%>
<br/><br/>
<h2><a id="adventures" class="aboutLink">ADVENTURES</a></h2>

<b>[Patent Pending]</b> Any happening that cannot be posted as an event and associated with one locale can be posted as an adventure/trip on HippoHappenings. When posting an adventure, include as much detail as possible in order for others viewing your adventure to be able to easily recreate it as well as find it via our adventure search page. Adventures can be directions to an obscure hiking trail, a full day’s pub crawl, a cross-country road trip to see national landmarks, an art walk in a specific city, and so on. 
<br/>
<br/><h2><a id="adventure-ownership" class="aboutLink">ADVENTURE OWNERSHIP</a></h2>
Unlike events and locales, when you post an adventure you are automatically the owner of the content and, therefore, the only one that can edit any of its features. 

<%-- <br/><br/>
 <h2><a id="featuring-adventure" class="aboutLink">FEATURING AN ADVENTURE</a></h2>
You can feature an adventure on HippoHappenings to make it show up first on the 'Stuff to Do' strip on the home page, in top four search results on the adventure search page and in the side bar of similar adventures on the similar adventures' pages. For the full details of featuring an adventure and what featuring entails visit our <a class="NavyLink12UD" href="termsandconditions.aspx#content-Type">Terms and Conditions</a> page.
--%>
<%--<br/><br/>
<h2><a id="bulletins" class="aboutLink">BULLETINS</a></h2>
You can also use our community of locals to spread the word about your local services, offers, notices, news, happenings or any of the like pertaining to the locals. Just like for events, locales and adventures we provide you with a search page for bulletins. 
<br /><br /> <h2><a id="featuring-bulletin" class="aboutLink">FEATURING A BULLETIN</a></h2>
You can feature a bulletin on HippoHappenings to make it show up throughout the site, in top four search results on the adventure search page and in the side bar of similar bulletins on the similar bulletins' pages. In addition, featured bulletins are emailed to user who are interested in your bulletin based on the search criteria they have saved as their saved bulletin search. For the full details of featuring a bulletin and what featuring entails visit our  <a class="NavyLink12UD" href="termsandconditions.aspx#content-Type">Terms and Conditions</a> page.

<br/><br/>
 <h2><a id="saved-bulletin-search" class="aboutLink">SAVED SEARCHES</a></h2>
Saving your bulletin search serves two purposes: 1. you will be able to get back to your search without re-entering any search criteria with the click of a link, and 2. you can choose to receive emails with the number of bulletins of your choosing which match the search criteria of your saved search. Which means you do not have to visit the site to find out whether any new bulletins have been posted. You can always access your saved searches via your Pages & Searches page. 
<br/><br/>
 <h2><a id="in-kind-contributions" class="aboutLink">IN-KIND CONTRIBUTIONS</a></h2>
HippoHappenings offers in-kind contributions of featured bulletins to nonprofit or not-for-profit corporations on a case-by-case basis at our own discretion. To determine whether you're eligible contact us at <a class="NavyLink12UD" href="mailto:Corporate@HippoHappenings.com">Corporate@HippoHappenings.com</a>. 
<br/><br/>

--%>

<a href="#about-HippoHappenings" class="NavyLink12UD UpTopLink">&uArr; Up Top &uArr;</a>

<br /><br /><br />
</div>
</div>
    </div>
    </div>
</asp:Content>

