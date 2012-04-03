<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="Sitemap.aspx.cs" Inherits="Sitemap" 
Title="Sitemap | HippoHappenings" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="EventDiv" style="line-height: 20px; padding-left: 50px; padding-right: 50px;">
        <h1 class="searches">Site Map!</h1>
  <ul style="color: Green; font-size: 20px;">
    <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/AdSearch.aspx">Ads</a>
        <ul>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/AdSearch.aspx">Ad Search: <span style="font-weight: normal; color: #cccccc;">search, share whith friends, email local ads and classifieds in your neighborhood and community</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/PostAnAd.aspx">Post an Ad: <span style="font-weight: normal; color: #cccccc;">post and edit your local ad, along with pictures and video, in your neighborhood with your specific category interests and have it displayed thoughout the site or just be searchable</span></a></li>
        </ul>
    </li>
    <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/EventSearch.aspx">Events</a>
         <ul>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/EventSearch.aspx">Event Search: <span style="font-weight: normal; color: #cccccc;">search, edit, comment on, submit photos, add to your calendar, connect with others going to the event, share, text, email and rate your local events and happenings in your neighborhood happening in your community</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/EnterEvent.aspx">Blog an Event: <span style="font-weight: normal; color: #cccccc;">blog about and post content, pictures, video, audio for local events happening in your neighborhood and in your community</span></a></li>
        </ul>
    </li>
    <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/VenueSearch.aspx">Venues</a>
        <ul>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/VenueSearch.aspx">Venue Search: <span style="font-weight: normal; color: #cccccc;">search, edit, comment on, share, email, text, submit photos to local venues, spots and places in your community or neighborhood</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/EnterVenueIntro.aspx">Enter a Venue: <span style="font-weight: normal; color: #cccccc;">enter a local venue along with description, pictures, video and audio in you neighborhood and community</span></a></li>
        </ul>
    </li>
    <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/GroupSearch.aspx">Groups</a>
         <ul>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/GroupSearch.aspx">Group Search: <span style="font-weight: normal; color: #cccccc;">search, edit, comment on, submit photos, add to your calendar, connect with others going to the group event, share, text and email your local group events and groups in your neighborhood, in your community</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/EnterGroup.aspx">Make a Group: <span style="font-weight: normal; color: #cccccc;">blog about and post content, pictures, video for local groups in your neighborhood and in your community. invite lovers of same interests to the groups you create</span></a></li>
        </ul>
    </li>
    
    <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/UserLogin.aspx">Users</a>
        <ul>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/UserLogin.aspx">User Login: <span style="font-weight: normal; color: #cccccc;">log in or create an account to edit and post events, venues and ads, see your ads progress with our ad statistics chart, save ad searches for easy return to your results next time and for receiving emails each time an ad is posted matching your criteria, and more!</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/User.aspx">My Account</a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/UserCalendar.aspx">My Calendar: <span style="font-weight: normal; color: #cccccc;">view all the events you have added to your calendar and share them with friends. View all the ads you have posted on the site and share them with friends</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/SearchesAndPages.aspx">Pages and Searches: <span style="font-weight: normal; color: #cccccc;">view all the ads, events and venues you are the owner of. Access all your saved serches and manage how many ads you want to receive for each</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/AdStatistics.aspx">Ad Statistics: <span style="font-weight: normal; color: #cccccc;">view statistics for your featured ads. See how many users using which method (though a saved ad search or though viewing it on the site) and in which categories have see your ad up to date!</span></a></li>
        </ul>
    </li>
    
    <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/Home.aspx">HippoHappenings</a>
        <ul>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/About.aspx">About: <span style="font-weight: normal; color: #cccccc;">find out what HippoHappenings is all about. Find out about our values and focus on neighborhoods and communities</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/TermsAndConditions.aspx">Terms of Use: <span style="font-weight: normal; color: #cccccc;">learn how to play nice on HippoHappenings</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/ProhibitedAds.aspx">Prohibited Ads: <span style="font-weight: normal; color: #cccccc;">learn what ads are prohibited and subject to removal and loss of membership</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/PrivacyPolicy.aspx">Privacy Policy: <span style="font-weight: normal; color: #cccccc;">learn about our privacy policy for our users</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/ContactUs.aspx">Contact Us: <span style="font-weight: normal; color: #cccccc;">contact us about your troubles!</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/Feedback.aspx">Feedback: <span style="font-weight: normal; color: #cccccc;">give us feedback about the site, tell us what to improve and what you would like to see.</span></a></li>
             <li><a class="AddLink" rel="bookmark" href="http://hippohappenings.com/Vote.aspx">Vote: <span style="font-weight: normal; color: #cccccc;">vote on new functionality... now, we're really excited about this one so put in your votes because we can't wait to implement any of these!</span></a></li>
        </ul>
    </li>
    
  </ul>

    </div>
</asp:Content>

