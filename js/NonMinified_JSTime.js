// JScript File

timedout();
function timedout()
{
        var today = new Date();
        var month = today.getMonth() + 1;
        var day = today.getDate();
        var year = today.getFullYear();
        var hours = today.getHours();
        var mins = today.getMinutes();
        var seconds = today.getSeconds();
        var todayDate = month + '/' + day +'/'+ year + ' ' + hours+':'+mins+':'+seconds;
        setCookie("BrowserDate",todayDate,1);
        setTimeout ("timedOut()", 1000);
}

function setCookie(c_name,value,expiredays)
{
    var exdate=new Date();
    exdate.setDate(exdate.getDate()+expiredays);
    document.cookie=c_name+ "=" +escape(value)+
    ((expiredays==null) ? "" : ";expires="+exdate.toGMTString());
}
function getCookie(c_name)
{
    if (document.cookie.length>0)
      {
      c_start=document.cookie.indexOf(c_name + "=");
      if (c_start!=-1)
        {
        c_start=c_start + c_name.length+1;
        c_end=document.cookie.indexOf(";",c_start);
        if (c_end==-1) c_end=document.cookie.length;
        return unescape(document.cookie.substring(c_start,c_end));
        }
      }
    return "";
}
