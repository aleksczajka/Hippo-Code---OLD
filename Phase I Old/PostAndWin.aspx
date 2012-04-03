<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="PostAndWin.aspx.cs" Inherits="PostAndWin" Title="Post And Win" %>
<%@ Register Src="~/Controls/Ads.ascx" TagName="Ads" TagPrefix="ctrl" %>
<%@ OutputCache Duration="86400" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <div style="float: left; width: 440px;">
            <asp:Label runat="server" CssClass="EventHeader" Text="Hey you, win some stuff!"></asp:Label>
            <div style="font-family: Arial; font-size: 12px; color: #cccccc; line-height: 20px;">
                Just for posting events on our blog you get stuff!!! 
                All you need to do is register with our site and start posting. 
                After you have posted 50 events which fall under the rules 
                and regulations, you have the option of receiving:
                <br /><br />
                 1. Five free ad posts on Sitename <br />
                 2. We’ll pay for an event of your choosing.  <br />
                 3. Hipportland t-shirt. <br />
            </div>
            <br /><br />
            <div style="float: right; clear: both; width: 440px; height: 50px; " align="right"><asp:ImageButton runat="server" ID="LetsDoItButton" OnClick="GoPost" AlternateText="Let's Do This!" ImageUrl="~/image/LetsDoThisButton.png"  onmouseout="this.src='image/LetsDoThisButton.png'" onmouseover="this.src='image/LetsDoThisButtonSelected.png'"  />
            </div>
            <div style="font-family: Arial; color: #666666; font-size: 12px;">
                Legal stuff. (Terms and Conditions) <br /><br />

Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec posuere varius odio. Etiam ac massa quis diam pharetra lobortis. Suspendisse et metus. Ut lobortis. Quisque risus. Maecenas dignissim mi ut odio. Donec enim massa, tristique et, consectetuer dictum, fermentum ac, elit. Maecenas lorem nulla, convallis imperdiet, lobortis eu, sagittis venenatis, neque. Sed augue. 
Quisque ultrices accumsan pede. Praesent adipiscing volutpat ipsum. Suspendisse sit amet turpis. Proin mattis molestie tortor. Sed a lorem. Maecenas malesuada dipiscing nunc. Aenean ornare eros id purus. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. 

Donec posuere varius odio. Etiam ac massa quis diam pharetra lobortis. Suspendisse et metus. Ut lobortis. Quisque risus. Maecenas dignissim mi ut odio. Donec enim massa, tristique et, consectetuer dictum, fermentum ac, elit. Maecenas lorem nulla, convallis imperdiet, lobortis eu, sagittis venenatis, neque. Sed augue. 

Quisque ultrices accumsan pede. Praesent adipiscing volutpat ipsum. Suspendisse sit amet turpis. Proin mattis molestie tortor. Sed a lorem. Maecenas malesuada dipiscing nunc. Aenean ornare eros id purus.   
            </div>
        </div>
        <div style="float: left; padding-left: 5px; width: 419px;">
            <div style="clear: both;">
             <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" 
            codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" 
            width="431" height="575" title="banner" wmode="transparent">  
            <param name="movie" value="gallery.swf" />  
            <param name="quality" value="high" />
            <param name="wmode" value="transparent"/>   
                <embed src="gallery.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" 
                type="application/x-shockwave-flash" width="431" height="575" wmode="transparent">
                </embed>
            </object>
                <%--<ctrl:Ads ID="Ads1" runat="server" />--%>
            </div>
            <div style="padding-left: 5px; width: 419px;">
                <div align="center" style=" padding-top: 13px ;clear: both ;vertical-align: middle; font-weight: bold;background-color: #686868; height: 27px; font-family: Arial; font-size: 12px; color: White;">
                    Want to have your ad featured? fill out your information <a href="PostAnAd.aspx" style="color: #1fb6e7; text-decoration: none;">here!</a>
                </div>
                
                
            </div>
        </div>
    </div>
</asp:Content>

