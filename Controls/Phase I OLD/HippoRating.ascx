<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HippoRating.ascx.cs" Inherits="Controls_HippoRating" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>


<script type="text/javascript" language="javascript">
    function hoverOnLeft(theName)
    {
        var name = document.getElementById(theName);
        name.style.backgroundImage='url(image/FullFullHalf.png)';
    }
    function hoverOnRight(theName)
    {
        var name = document.getElementById(theName);
        name.style.backgroundImage='url(image/FullFullRightHalf.png)';
    }
    function hoverOutLeft(theName)
    {
        var name = document.getElementById(theName);
        name.style.backgroundImage='url(image/EmptyEmptyHalf.png)';
    }
    function hoverOutRight(theName)
    {
        var name = document.getElementById(theName);
        name.style.backgroundImage='url(image/EmptyEmptyRightHalf.png)';
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
<asp:Panel runat="server" ID="EntirePanel">
<asp:Literal runat="server" ID="FloatingLiteral"></asp:Literal>
<asp:Panel runat="server" ID="RatePanel" Visible="false">
<asp:Label runat="server" Visible="false" ID="UserLabel"></asp:Label>
<div id="WorkingDiv" style="padding: 0px; margin: 0px; height: 30px;">
<%--    <asp:ImageButton runat="server" ID="imgOne" ImageUrl="~/image/RatingEmpty.png" onmouseover="this.src='image/RatingFull.png'" onmouseout="this.src='image/RatingEmpty.png'" />
    <asp:ImageButton runat="server" ID="imgTwo" ImageUrl="~/image/RatingEmpty.png"   ctl00_ContentPlaceHolder1_HippoRating1_imgOne.src='image/RatingFull.png';" onmouseout="this.src='image/RatingEmpty.png'; ctl00_ContentPlaceHolder1_HippoRating1_imgOne.src='image/RatingEmpty.png';" />
--%>  
<div style="float: left; padding-right: 5px;"><asp:Literal runat="server" ID="TitleLiteral"></asp:Literal> </div>
<div style="padding: 0px; margin: 0px; float: left;"> 
<button runat="server" id="imgOne"  onserverclick="ServerSetRating" value="1" style="cursor: pointer;width: 15px; height: 20px;background-color: transparent;padding: 0px; margin: 0px; border: 0; background-repeat: no-repeat; background-image: url(image/EmptyEmptyHalf.png);"  
 onmouseover="hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" onmouseout="hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>

</div>
<div style="padding: 0px; margin: 0px; float: left;">
</div>
<div style="padding: 0px; margin: 0px; float: left;">
<button runat="server" id="imgTwo" onserverclick="ServerSetRating" value="2"
style="cursor: pointer;width: 15px; height: 20px;background-color: transparent;padding: 0px; margin: 0px; border: 0; background-repeat: no-repeat; background-image: url(image/EmptyEmptyRightHalf.png);"  
 onmouseover="hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>

</div>
<div style="padding: 0px; margin: 0px; float: left;">
<button runat="server" id="imgThree"  onserverclick="ServerSetRating" value="3"
style="cursor: pointer;width: 15px; height: 20px;background-color: transparent;padding: 0px; margin: 0px; border: 0; background-repeat: no-repeat; background-image: url(image/EmptyEmptyHalf.png);"  
 onmouseover="hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>

</div>
<div style="padding: 0px; margin: 0px; float: left;">
<button runat="server" id="imgFour"  onserverclick="ServerSetRating" value="4"  
style="cursor: pointer;width: 15px; height: 20px;background-color: transparent;padding: 0px; margin: 0px; border: 0; background-repeat: no-repeat; background-image: url(image/EmptyEmptyRightHalf.png);"  
 onmouseover="hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>

</div>
<div style="padding: 0px; margin: 0px; float: left;">
<button runat="server" id="imgFive"  onserverclick="ServerSetRating" value="5" 
style="cursor: pointer;width: 15px; height: 20px;background-color: transparent;padding: 0px; margin: 0px; border: 0; background-repeat: no-repeat; background-image: url(image/EmptyEmptyHalf.png);"  
 onmouseover="hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>
</div>
<div style="padding: 0px; margin: 0px; float: left;">
<button runat="server" id="imgSix"  onserverclick="ServerSetRating" value="6" 
style="cursor: pointer;width: 15px; height: 20px;background-color: transparent;padding: 0px; margin: 0px; border: 0; background-repeat: no-repeat; background-image: url(image/EmptyEmptyRightHalf.png);"  
 onmouseover="hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>

</div>
<div style="padding: 0px; margin: 0px; float: left;">
<button runat="server" id="imgSeven"  onserverclick="ServerSetRating" value="7" 
style="cursor: pointer;width: 15px; height: 20px;background-color: transparent;padding: 0px; margin: 0px; border: 0; background-repeat: no-repeat; background-image: url(image/EmptyEmptyHalf.png);"  
 onmouseover="hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>
</div>
<div style="padding: 0px; margin: 0px; float: left;">
<button runat="server" id="imgEight"  onserverclick="ServerSetRating" value="8" 
style="cursor: pointer;width: 15px; height: 20px;background-color: transparent;padding: 0px; margin: 0px; border: 0; background-repeat: no-repeat; background-image: url(image/EmptyEmptyRightHalf.png);"  
 onmouseover="hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgEight');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgEight');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>
</div>
<div style="padding: 0px; margin: 0px; float: left;">
<button runat="server" id="imgNine"  onserverclick="ServerSetRating" value="9" 
style="cursor: pointer;width: 15px; height: 20px;background-color: transparent;padding: 0px; margin: 0px; border: 0; background-repeat: no-repeat; background-image: url(image/EmptyEmptyHalf.png);"  
 onmouseover="hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgNine');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgEight');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOnRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOnLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');" 
 onmouseout="hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgNine');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgEight');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgSeven');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgSix');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgFive');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgFour');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgThree');hoverOutRight('ctl00_ContentPlaceHolder1_HippoRating1_imgTwo');hoverOutLeft('ctl00_ContentPlaceHolder1_HippoRating1_imgOne');"></button>
</div>
<div style="padding: 0px; margin: 0px; float: left;">
<button runat="server" id="imgTen"  onserverclick="ServerSetRating" value="10" 
style="cursor: pointer;width: 15px; height: 20px;background-color: transparent;padding: 0px; margin: 0px; border: 0; background-repeat: no-repeat; background-image: url(image/EmptyEmptyRightHalf.png);"  
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
    <div class="EventDiv" id="StaticDiv" style="clear: both; width: 100%; display: none;">
       
       <div style="padding: 0px; margin: 0px; float: left;"> 
        <img style="padding: 0px; margin: 0px;" id="img1"/>
        </div>
        <div style="padding: 0px; margin: 0px; float: left;">
        </div>
        <div style="padding: 0px; margin: 0px; float: left;">
        <img style="padding: 0px; margin: 0px;" id="img2"/>
        </div>
        <div style="padding: 0px; margin: 0px; float: left;">
        <img src="image/EmptyEmptyHalf.png" id="img3"/>
        </div>
        <div style="padding: 0px; margin: 0px; float: left;">
        <img src="image/EmptyEmptyRightHalf.png" id="img4"/>
        </div>
        <div style="padding: 0px; margin: 0px; float: left;">
        <img src="image/EmptyEmptyHalf.png" id="img5"/>
        </div>
        <div style="padding: 0px; margin: 0px; float: left;">
        <img src="image/EmptyEmptyRightHalf.png" id="img6"/>
        </div>
        <div style="padding: 0px; margin: 0px; float: left;">
        <img src="image/EmptyEmptyHalf.png" id="img7"/>
        </div>
        <div style="padding: 0px; margin: 0px; float: left;">
        <img src="image/EmptyEmptyRightHalf.png" id="img8"/>
        </div>
        <div style="padding: 0px; margin: 0px; float: left;">
        <img src="image/EmptyEmptyHalf.png" id="img9"/>
        </div>
        <div style="padding: 0px; margin: 0px; float: left;">
        <img src="image/EmptyEmptyRightHalf.png" id="img10"/>
        </div>
    </div>

</div>
</asp:Panel>