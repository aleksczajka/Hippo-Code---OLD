<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchElements.ascx.cs" Inherits="SearchElements" %>
<%@ Register Src="~/Controls/Pager_TEST.ascx" TagName="Pager" TagPrefix="ctrl" %>
<%@ Register Src="~/Controls/SearchElement.ascx" TagPrefix="ctrl" TagName="SearchElement" %>


<%--<style type="text/css">
.background {
    background-color:#1FB6E7; 
    filter:alpha(opacity=20); 
    opacity:0.2; 
} 
</style>
<div id="updateProgressDiv" class="updateProgressResults" style="display:none">
    <div align="center" style="z-index: 3000;margin-top:30px; filter:alpha(opacity=100); opacity: 1;">
        <img style="z-index: 3000;filter:alpha(opacity=100); opacity: 1;" src="../image/ajax-loaderBig.gif" />
        <span class="updateProgressMessage">Loading ...</span>
    </div>
</div>

<script type="text/javascript">
    var _updateProgressDiv;
    var _backgroundDiv;
    var _gridView;

    function pageLoad(sender, args){    
        //  register for our events
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);    
        
        //  get the updateprogressdiv
        _updateProgressDiv = $get('updateProgressDiv');
        //  fetch the gridview
        _gridView = $get('<%= this.SearchElementsPanel.ClientID %>');
        
        //  create the div that we will position over the gridview
        //  during postbacks
        _backgroundDiv = document.createElement('div');
        _backgroundDiv.style.display = 'none';
        _backgroundDiv.style.zIndex = 10000;
        _backgroundDiv.className = 'background';
        
        //  add the element to the DOM
        _gridView.parentNode.appendChild(_backgroundDiv);
    } 
    
    function beginRequest(sender, args){
    // make it visible
    _updateProgressDiv.style.display = '';             
    _backgroundDiv.style.display = '';
    
    // get the bounds of both the gridview and the progress div
    var gridViewBounds = Sys.UI.DomElement.getBounds(_gridView);
    var updateProgressDivBounds = Sys.UI.DomElement.getBounds(_updateProgressDiv);
               
    //  center of gridview
    var x = gridViewBounds.x + Math.round(gridViewBounds.width / 2) - Math.round(updateProgressDivBounds.width / 2);
    var y = gridViewBounds.y + Math.round(gridViewBounds.height / 2) - Math.round(updateProgressDivBounds.height / 2);        
    
    //  set the dimensions of the background div to the same as the gridview
    //_backgroundDiv.style.width = gridViewBounds.width + 'px';
    _backgroundDiv.style.width = '420px';
    //_backgroundDiv.style.height = gridViewBounds.height + 'px';              
    _backgroundDiv.style.height = '500px';  
    _updateProgressDiv.style.width = '420px';
    _updateProgressDiv.style.height = '500px';
    
    var theX = 200;
    var theY = 300;
    if(window.location.pathname == "/SearchResults.aspx")
    {
        theX = 0;
        theY = 230;
    }
    //    set the progress element to this position
    Sys.UI.DomElement.setLocation(_updateProgressDiv, theX, theY);     
    //  place the div over the gridview
    Sys.UI.DomElement.setLocation(_backgroundDiv, theX, theY);           
}
function endRequest(sender, args) {
    // make it invisible
    _updateProgressDiv.style.display = 'none';
    _backgroundDiv.style.display = 'none';
}
</script>--%>
<%--<asp:UpdatePanel runat="server" ID="UpdatePanel1">
    <ContentTemplate>
--%>            <asp:Label runat="server" ID="ErrorLabel" ForeColor="red"></asp:Label>
        <asp:Panel runat="server" ID="SearchElementsPanel"></asp:Panel>
<%--    </ContentTemplate>
</asp:UpdatePanel>
--%>