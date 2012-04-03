// JScript File
function saveScroll()


{


  var sScroll;


  if (document.documentElement && document.documentElement.scrollTop)


    sScroll = document.documentElement.scrollTop;


  else if (document.body)


    sScroll = document.body.scrollTop;


  else


  {


    sScroll = 0;


  }


  document.getElementById('__SAVESCROLL').value = sScroll;


}





function restoreScroll()


{


  var sScroll = document.getElementById('__SAVESCROLL').value;


  if (sScroll > 0)


  {


    if (document.documentElement && document.documentElement.scrollTop)


      document.documentElement.scrollTop = sScroll;


    else if (document.body)


    {


      if (window.navigator.appName == 'Netscape')


        window.scroll(0, sScroll);


      else


        document.body.scrollTop = sScroll;


    }


    else


    {


      window.scroll(0, sScroll);


    }


    // here is setting absolute positioning panel, if you need, set correct ID  and uncomment follow 2 lines (and add needed lines/setting for all your panels)


    if (document.getElementById('<asp:Panel align="left" runat="server" ID="FriendsPanel" BorderStyle="solid" BorderWidth="1px" BorderColor="Black" BackColor="#363636" Height="250px" ScrollBars="Vertical" Width="100%">') != null )


    document.getElementById('<asp:Panel align="left" runat="server" ID="FriendsPanel" BorderStyle="solid" BorderWidth="1px" BorderColor="Black" BackColor="#363636" Height="250px" ScrollBars="Vertical" Width="100%">').style.top = '100px';


  }


}





window.onload = restoreScroll;


window.onscroll = saveScroll;


window.onresize = saveScroll;



