<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HippoRating.ascx.cs" Inherits="HippoRating" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>
<div>
<div class="HippoRatingTop">
<script type="text/javascript" language="javascript">
    function hoverOnLeft(theName)
    {
        var name = document.getElementById(theName);
        name.style.backgroundImage='url(NewImages/FullLeftHalf.png)';
    }
    function hoverOnRight(theName)
    {
        var name = document.getElementById(theName);
        name.style.backgroundImage='url(NewImages/FullRightHalf.png)';
    }
    function hoverOutLeft(theName)
    {
        var name = document.getElementById(theName);
        name.style.backgroundImage='url(NewImages/EmptyLeftHalf.png)';
    }
    function hoverOutRight(theName)
    {
        var name = document.getElementById(theName);
        name.style.backgroundImage='url(NewImages/EmptyRightHalf.png)';
    }
    function disableDiv(theDiv)
    {
        var name = document.getElementById(theDiv);
        name.style.display = 'none';
    }
    function showDiv(theDiv)
    {
        var name = document.getElementById(theDiv);
        name.style.display = 'block';
    }
    function setFullHippo(number)
    {
        if(number >= 1)
        {
            var img1 = document.getElementById('img1');
            img1.src = 'image/RatingFull.png';
        }
        if(number >= 2)
        {
            var img2 = document.getElementById('img2');
            img2.src = 'image/RatingFull.png';
        }
        if(number >= 3)
        {
            var img3 = document.getElementById('img3');
            img3.src = 'image/RatingFull.png';
        }
        if(number >= 4)
        {
            var img4 = document.getElementById('img4');
            img4.src = 'image/RatingFull.png';
        }
        if(number == 5)
        {
            var img5 = document.getElementById('img5');
            img5.src = 'image/RatingFull.png';
        }
    }
    

</script>

<asp:Label runat="server" ID="Errorlabel" ForeColor="red"></asp:Label>
<label id="theLabel"></label>
<asp:UpdatePanel runat="server" UpdateMode="conditional">
    <ContentTemplate>
    <div class="HippoRatingInner">
         <script type="text/javascript">
            function ShowCatDiv()
            {
                var theDiv = document.getElementById('WidthCatDiv');
                var loadDiv = document.getElementById('LoadCatDiv');
                
                loadDiv.style.height = theDiv.offsetHeight +'px';
                loadDiv.style.display = 'block';
                
                return true;
            }
        </script>
        <div id="LoadCatDiv" class="LoadDiv">
            <div class="LoadDivInner"></div>
            <div class="LoadDiv2">
                <table height="100%" width="100%" align="center">
                    <tr>
                        <td height="100%" width="100%" align="center" valign="middle">
                            <img alt="Loading Logo Image" src="image/ajax-loaderSmall.gif" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="WidthCatDiv">

<asp:Panel runat="server" ID="EntirePanel">
<asp:Literal runat="server" ID="FloatingLiteral"></asp:Literal>
<asp:Panel runat="server" ID="RatePanel" Visible="false">
<asp:Label runat="server" Visible="false" ID="UserLabel"></asp:Label>
<div id="WorkingDiv" class="LoadDiv3">
<%--    <asp:ImageButton runat="server" ID="imgOne" ImageUrl="~/image/RatingEmpty.png" onmouseover="this.src='image/RatingFull.png'" onmouseout="this.src='image/RatingEmpty.png'" />
    <asp:ImageButton runat="server" ID="imgTwo" ImageUrl="~/image/RatingEmpty.png"   ctl00_ContentPlaceHolder1_HippoRating1_imgOne.src='image/RatingFull.png';" onmouseout="this.src='image/RatingEmpty.png'; ctl00_ContentPlaceHolder1_HippoRating1_imgOne.src='image/RatingEmpty.png';" />
--%>  
<div class="RatingTop"><asp:Literal runat="server" ID="TitleLiteral"></asp:Literal> </div>
<div class="RatingInnerInner"> 
<button runat="server" id="imgOne" onclick="ShowCatDiv();"  onserverclick="ServerSetRating" value="1" class="RatingButton1"  
 onmouseover="hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" onmouseout="hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>

</div>
<div class="RatingInnerInner">
</div>
<div class="RatingInnerInner">
<button runat="server" id="imgTwo" onclick="ShowCatDiv();" onserverclick="ServerSetRating" value="2"
class="RatingButton2"  
 onmouseover="hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>

</div>
<div class="RatingInnerInner">
<button runat="server" id="imgThree" onclick="ShowCatDiv();" onserverclick="ServerSetRating" value="3"
class="RatingButton3"  
 onmouseover="hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>

</div>
<div  class="RatingInnerInner">
<button runat="server" id="imgFour" onclick="ShowCatDiv();" onserverclick="ServerSetRating" value="4"  
class="RatingButton4"  
 onmouseover="hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>

</div>
<div class="RatingInnerInner">
<button runat="server" id="imgFive" onclick="ShowCatDiv();" onserverclick="ServerSetRating" value="5" 
class="RatingButton5"  
 onmouseover="hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>
</div>
<div class="RatingInnerInner">
<button runat="server" id="imgSix" onclick="ShowCatDiv();" onserverclick="ServerSetRating" value="6" 
class="RatingButton6"  
 onmouseover="hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>

</div>
<div class="RatingInnerInner">
<button runat="server" id="imgSeven" onclick="ShowCatDiv();" onserverclick="ServerSetRating" value="7" 
class="RatingButton7"  
 onmouseover="hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>
</div>
<div class="RatingInnerInner">
<button runat="server" id="imgEight" onclick="ShowCatDiv();" onserverclick="ServerSetRating" value="8" 
class="RatingButton8"  
 onmouseover="hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgEight');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgEight');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>
</div>
<div class="RatingInnerInner">
<button runat="server" id="imgNine" onclick="ShowCatDiv();" onserverclick="ServerSetRating" value="9" 
class="RatingButton9"  
 onmouseover="hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgNine');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgEight');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgNine');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgEight');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>
</div>
<div class="RatingInnerInner">
<button runat="server" id="imgTen" onclick="ShowCatDiv();" onserverclick="ServerSetRating" value="10" 
class="RatingButton10"  
 onmouseover="hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTen');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgNine');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgEight');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTen');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgNine');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgEight');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>
</div>
</div>
</asp:Panel>
<%--<img src="image/RatingEmpty.png" id="imgOne" onclick="disableDiv('WorkingDiv'); showDiv('StaticDiv'); setFullHippo(1);return false;" onmouseover="hoverOn('imgOne');" onmouseout="hoverOut('imgOne');" />
    <img src="image/RatingEmpty.png" id="imgTwo" onclick="disableDiv('WorkingDiv'); showDiv('StaticDiv'); setFullHippo(2);return false;"  onmouseover="hoverOn('imgTwo'); hoverOn('imgOne');" onmouseout="hoverOut('imgTwo'); hoverOut('imgOne');" />
    <img src="image/RatingEmpty.png" id="imgThree" onclick="disableDiv('WorkingDiv'); showDiv('StaticDiv'); setFullHippo(3);return false;" onmouseover="hoverOn('imgTwo'); hoverOn('imgOne'); hoverOn('imgThree');" onmouseout="hoverOut('imgTwo'); hoverOut('imgOne');hoverOut('imgThree');" />
    <img src="image/RatingEmpty.png" id="imgFour" onclick="disableDiv('WorkingDiv'); showDiv('StaticDiv'); setFullHippo(4);return false;"  onmouseover="hoverOn('imgTwo'); hoverOn('imgOne');hoverOn('imgThree');hoverOn('imgFour');" onmouseout="hoverOut('imgTwo'); hoverOut('imgOne');hoverOut('imgThree');hoverOut('imgFour');" />
    <img src="image/RatingEmpty.png" id="imgFive" onclick="disableDiv('WorkingDiv'); showDiv('StaticDiv'); setFullHippo(5);return false;"  onmouseover="hoverOn('imgTwo'); hoverOn('imgOne');hoverOn('imgThree');hoverOn('imgFour');hoverOn('imgFive');" onmouseout="hoverOut('imgTwo'); hoverOut('imgOne');hoverOut('imgThree');hoverOut('imgFour');hoverOut('imgFive');" />
--%>
<%--    <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="~/image/RatingEmpty.png" onmouseout="this.src='image/RatingEmpty.png'" onmouseover="this.src='image/RatingFull.png'" />
    <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="~/image/RatingEmpty.png" onmouseout="this.src='image/RatingEmpty.png'" onmouseover="this.src='image/RatingFull.png'" />
    <asp:ImageButton runat="server" ID="ImageButton3" ImageUrl="~/image/RatingEmpty.png" onmouseout="this.src='image/RatingEmpty.png'" onmouseover="this.src='image/RatingFull.png'" />
    <asp:ImageButton runat="server" ID="ImageButton4" ImageUrl="~/image/RatingEmpty.png" onmouseout="this.src='image/RatingEmpty.png'" onmouseover="this.src='image/RatingFull.png'" />
    <asp:ImageButton runat="server" ID="ImageButton5" ImageUrl="~/image/RatingEmpty.png" onmouseout="this.src='image/RatingEmpty.png'" onmouseover="this.src='image/RatingFull.png'" />
--%>

<asp:Literal runat="server" ID="HiddenValuesLiteral"></asp:Literal>
<asp:Panel runat="server" ID="RatingPanel" Visible="false">
    <div class="EventDiv">
        
        <asp:Literal runat="server" ID="RatingLiteral"></asp:Literal>
    </div>
</asp:Panel>
    <div class="EventDiv RatingMiddle" id="StaticDiv">
       
       <div class="RatingInnerInner"> 
        <img alt="Background Rating Image" class="RatingImage" id="img1"/>
        </div>
        <div class="RatingInnerInner">
        </div>
        <div class="RatingInnerInner">
        <img alt="Background Rating Image" class="RatingImage" id="img2"/>
        </div>
        <div class="RatingInnerInner">
        <img alt="Background Rating Image" src="NewImages/EmptyLeftHalf.png" id="img3"/>
        </div>
        <div class="RatingInnerInner">
        <img alt="Background Rating Image" src="NewImages/EmptyRightHalf.png" id="img4"/>
        </div>
        <div class="RatingInnerInner">
        <img alt="Background Rating Image" src="NewImages/EmptyLeftHalf.png" id="img5"/>
        </div>
        <div class="RatingInnerInner">
        <img alt="Background Rating Image" src="NewImages/EmptyRightHalf.png" id="img6"/>
        </div>
        <div class="RatingInnerInner">
        <img alt="Background Rating Image" src="NewImages/EmptyLeftHalf.png" id="img7"/>
        </div>
        <div class="RatingInnerInner">
        <img alt="Background Rating Image" src="NewImages/EmptyRightHalf.png" id="img8"/>
        </div>
        <div class="RatingInnerInner">
        <img alt="Background Rating Image" src="NewImages/EmptyLeftHalf.png" id="img9"/>
        </div>
        <div class="RatingInnerInner">
        <img alt="Background Rating Image" src="NewImages/EmptyRightHalf.png" id="img10"/>
        </div>
    </div>


</asp:Panel>

                                            
        </div>
        </div></div>
    </ContentTemplate>
</asp:UpdatePanel>

</div>
<div class="FloatLeft">
        <div id="fb-root"></div><script src="http://connect.facebook.net/en_US/all.js#appId=229624373718810&amp;xfbml=1"></script><asp:Literal runat="server" ID="fbLiteral"></asp:Literal>
    </div>
</div>