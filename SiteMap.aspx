<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
CodeFile="SiteMap.aspx.cs" Inherits="SiteMap" 
Title="Sitemap | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="Text12NU" style="padding-right: 10px;line-height: 16px;">
        <h1>Site Map</h1><br />
  <ul style="color: Green;">
    <%--<li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/ad-search">Bulletins</a>
        <ul>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/ad-search">Bulletin Search: <span class="Text12NU" >search, share whith friends, email local bulletins in your neighborhood and community</span></a></li>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/post-bulletin">Post an Bulletin: <span class="Text12NU" >post and edit your local bulletin, along with pictures and video, in your neighborhood with your specific category interests and have it displayed thoughout the site or just be searchable</span></a></li>
        </ul>
    </li>--%>
    <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/event-search">Events</a>
         <ul>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/event-search">Event Search: <span class="Text12NU" >search, edit, comment on, submit photos, add to your calendar, connect with others going to the event, share, text, email and rate your local events and happenings in your neighborhood happening in your community</span></a></li>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/enter-event">Blog an Event: <span class="Text12NU" >blog about and post content, pictures, video, audio for local events happening in your neighborhood and in your community</span></a></li>
        </ul>
    </li>
    <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/home">HippoHappenings</a>
        <ul>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/about">About: <span class="Text12NU" >find out what HippoHappenings is all about. Find out about our values and focus on neighborhoods and communities</span></a></li>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/terms-and-conditions">Terms of Use: <span class="Text12NU" >learn how to play nice on HippoHappenings</span></a></li>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/Prohibited-Ads">Prohibited Ads: <span class="Text12NU" >learn what ads are prohibited and subject to removal and loss of membership</span></a></li>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/Privacy-Policy">Privacy Policy: <span class="Text12NU" >learn about our privacy policy for our users</span></a></li>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/contact-us">Contact Us: <span class="Text12NU" >contact us about your troubles!</span></a></li>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/feedback">Feedback: <span class="Text12NU" >give us feedback about the site, tell us what to improve and what you would like to see.</span></a></li>
        </ul>
    </li>
    <%--<li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/hippo-points">Hippo Points & Hippo Bosses: <span class="Text12NU" >get featured on HippoHappenings home page for posting the most content on our site.</span></a>
    </li>--%>
    <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/venue-search">Locales</a>
        <ul>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/venue-search">Locale Search: <span class="Text12NU" >search, edit, comment on, share, email, text, submit photos to local locales, spots and places in your community or neighborhood</span></a></li>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/EnterVenue.aspx">Enter a Locale: <span class="Text12NU" >enter a local locale along with description, pictures, video and audio in you neighborhood and community</span></a></li>
        </ul>
    </li>
    <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/trip-search">Trips</a>
         <ul>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/trip-search">Trip Search: <span class="Text12NU" >search, edit, comment on, submit photos, share, text, email and rate your local trips and sights in your neighborhood</span></a></li>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/EnterTripIntro.aspx">Post a Trip: <span class="Text12NU" >blog about and post content, pictures, video, audio for local trips in your neighborhood</span></a></li>
        </ul>
    </li>
    
    <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/login">Users</a>
        <ul>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/login">User Login: <span class="Text12NU" >log in or create an account to edit and post events, locales and ads, see your ads progress with our ad statistics chart, save ad searches for easy return to your results next time and for receiving emails each time an ad is posted matching your criteria, and more!</span></a></li>
             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/login">My Account</a></li>
<%--             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/login">My Calendar: <span class="Text12NU" >view all the events you have added to your calendar and share them with friends. View all the ads you have posted on the site and share them with friends</span></a></li>
--%>             <li><a class="NavyLink12" rel="bookmark" href="http://hippohappenings.com/login">Pages and Searches: <span class="Text12NU" >view all the ads, events and locales you are the owner of. Access all your saved serches and manage how many ads you want to receive for each</span></a></li>
        </ul>
    </li>
    
    
    
  </ul>

    </div>
</asp:Content>

