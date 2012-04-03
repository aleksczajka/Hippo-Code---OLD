<%@ Page Language="C#" MasterPageFile="~/SecondMasterPage.master" AutoEventWireup="true" 
ValidateRequest="false" MaintainScrollPositionOnPostback="true"
CodeFile="Hippo-Points.aspx.cs" Inherits="HippoPoints" smartnavigation="true"
Title="Hippo Boss and Points Promote Your Cause" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<%@ Register TagName="Ads" TagPrefix="ctrl" Src="~/Controls/Ads.ascx" %>
<%@ Register TagName="BlueButton" TagPrefix="ctrl" Src="~/Controls/BlueButton.ascx" %>
<%@ Register TagName="GreenButton" TagPrefix="ctrl" Src="~/Controls/GreenButton.ascx" %>
<%@ Register TagName="SmallButton" TagPrefix="ctrl" Src="~/Controls/SmallButton.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script type="text/javascript">
    function GoTo(url)
    {
        window.location = url;
    }
 </script>
 <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>  
       <div class="topDiv aboutLink">
        <div class="aboutLink"><h1>Hippo Boss via Hippo Points</h1></div>
        <div class="aboutLink"><div align="center"><h2 class="HippoH2">A great, free way to promote 
        yourself on HippoHappenings</h2></div></div>
       </div>
       <table cellpadding="0" cellspacing="0">
        <tr>
            <td class="Points1" valign="top">
                <div class="Points2">
                    <h2 class="HippoH22"><a href="javascript:GoTo('my-account');" class="NavyLink">Sign Up for Hippo Points</a></h2>
                </div>
            </td>
            <td width="510px" class="Points3" valign="middle">
                    <label>
                    You might have already seen them, the Hippo Boss winners displayed on our home page. These folks are the winners of the current month's Hippo Points contest and have been dubbed Hippo Bosses of the month in their area. The Hippo Points are simple, post the most happenings, trips, locales or bulletins on the Hippo and get featured on our home page, in your city for a full month. The person who wins globally, not just in their city, gets to be featured on Hippo home pages worldwide. 

                    

                    </label>
                    <br /><br />
                    <h2>Qualified Posts</h2>
                    <label>
                    Event, locales, trips and bulletin posts all qualify toward your Hippo Points provided the content is real and honest. Posting happenings, trips, locales and bulletins can be done for any location, regardless of the location associated with your account. For example, if you live in Portland, OR and post an event in New York City will still count as a Hippo Point towards a Hippo Boss in Portland, OR. 
                                       </label>
                    <br /><br />
                    <h2>Hippo Badges</h2>
                    <label>
This is our ranking system for users who contribute the most to their Hippo community. Each time you win the Hippo Boss contest you are awarded with a purple Hippo Badge. Once you win five badges, you receive a bronze badge. After winning ten badges, you receive a silver badge. And, after twenty times of winning the Hippo Boss you receive a golden Hippo Badge. 

                    
                    </label>
                    <br /><br />
                    <h2>Hippo Points Entry</h2>
                    <label>
                    When you are entered into the Hippo Points competition, your My Account page will tell you how you are ranking in the contest thus far. This number can fluctuate up or down due to the posts you make and the posts made by others in your area. However, it can also fluctuate down drastically due to hippo members having the ability of entering into the contest as little as 30 minutes before the final calculation of the winners. This can lead to a scenario where a member could have potentially posted much more content on the hippo than you have while not being entered into the contest. They will, therefore, not have been taken into account in our calculations of your up-to-date ranking on your My Account page. Once they enter, however, they will have ranked higher than you, thus bumping your ranking down. 
                    </label>
                    <br /><br />
                    <h2>Hippo Points Ties</h2>
                    <label>
                    In the case of a tie, a Hippo Boss will be chosen based on the quality of the content posted. The quality of the content can be improved by devoting more time to each post through posting images and videos, and, of course, ensuring accuracy. 
                    </label>
                <br /><br /><br /><br />
                <label>
                    <span class="Asterisk">*</span>For the entire terms regarding Hippo Points and Hippo Boss go to <a class="NavyLink12UD" href="hippo-points-tou">HIPPO POINTS Terms of Use.</a>
                </label>
            </td>
        </tr>
        
       </table>
       <div class="Points4">
            <label>*Posting and Featuring of happenings and bulletins is open to everyone on HippoHappenings.com. We do, however, request that all happenings and bulletins be local or applicable locally; i.e. postings cannot be for purely commercial purposes applicable nationwide or globally. For the full Terms and Conditions on posting and featuring happenings please visit our
            <a class="NavyLink12UD" href="terms-and-conditions">Terms & Condition Page.</a></label>
       </div>
</asp:Content>

