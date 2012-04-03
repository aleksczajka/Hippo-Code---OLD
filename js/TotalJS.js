function timedout(){var today=new Date();var month=today.getMonth()+1;var day=today.getDate();var year=today.getFullYear();var hours=today.getHours();var mins=today.getMinutes();var seconds=today.getSeconds();var todayDate=month+'/'+day+'/'+year+' '+hours+':'+mins+':'+seconds;setCookie("TimeZone",today.getTimezoneOffset(),1);setCookie("BrowserDate",todayDate,1)}function setCookie(c_name,value,expiredays){var exdate=new Date();exdate.setDate(exdate.getDate()+expiredays);document.cookie=c_name+"="+escape(value)+((expiredays==null)?"":";expires="+exdate.toGMTString())}function getCookie(c_name){if(document.cookie.length>0){c_start=document.cookie.indexOf(c_name+"=");if(c_start!=-1){c_start=c_start+c_name.length+1;c_end=document.cookie.indexOf(";",c_start);if(c_end==-1)c_end=document.cookie.length;return unescape(document.cookie.substring(c_start,c_end))}}return""}
function setWidth(randNum)
{
    var theWidthDiv = document.getElementById('widthDiv'+randNum);
    var theWrapDiv = document.getElementById('WrapDiv'+randNum);
    if(theWidthDiv != undefined && theWidthDiv != null)
        theWrapDiv.style.width = theWidthDiv.childNodes[3].offsetWidth + 14 + 'px';
    var somtinhere = 0;
}

function setWidthGreen(randNum)
{
    var theWidthDiv = document.getElementById('widthDiv'+randNum);
    var theWrapDiv = document.getElementById('WrapDiv'+randNum);
    if(theWidthDiv != undefined && theWidthDiv != null)
        theWrapDiv.style.width = theWidthDiv.childNodes[3].offsetWidth + 16 + 'px';
    var somtinhere = 0;
}

function setWait(event)
{
    var thisGuy = document.getElementById(event.target.id);
    if(thisGuy != null && thisGuy != undefined)
        thisGuy.style.cursor = 'wait';
    return true;
}