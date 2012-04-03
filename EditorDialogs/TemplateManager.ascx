<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />

<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");

Telerik.Web.UI.Widgets.TemplatePreviewer = function(element)
{
	Telerik.Web.UI.Widgets.TemplatePreviewer.initializeBase(this, [element]);
	
	this._currentItem = null;
	this._currentTemplateItem = null;
}

Telerik.Web.UI.Widgets.TemplatePreviewer.prototype = {
	initialize : function() 
	{
		Telerik.Web.UI.Widgets.TemplatePreviewer.callBaseMethod(this, 'initialize');
		this._initializeChildren();
	},

	_initializeChildren : function()
	{
	    this._currentTemplateItem = $get("templateIframe");
	},
	
	isValidExtension : function()
	{
	    if(this._currentItem.extension == ".html" || this._currentItem.extension == ".htm") return true;
	    return false;
	},
	
	setItem : function(item)
	{
	    this._currentItem = item;
	    if(this._currentItem.type == Telerik.Web.UI.Widgets.FileItemType.File && this.isValidExtension())
	    {
	        this._currentTemplateItem.src = this._currentItem.getUrl();
	    }
	    else
	    {
	        this._currentTemplateItem.src = "javascript:''";
	    }
	},
	
	getResult : function()
	{
		if (this._currentItem && this._currentItem.type == Telerik.Web.UI.Widgets.FileItemType.File && this.isValidExtension())
		{
	        return this._currentTemplateItem.contentWindow.document.body.innerHTML;
		}
		return null;
	},
	
	dispose : function() 
	{
		Telerik.Web.UI.Widgets.TemplatePreviewer.callBaseMethod(this, 'dispose');
	}
}

Telerik.Web.UI.Widgets.TemplatePreviewer.registerClass('Telerik.Web.UI.Widgets.TemplatePreviewer', Telerik.Web.UI.Widgets.FilePreviewer);


Type.registerNamespace("Telerik.Web.UI.Editor.DialogControls");

Telerik.Web.UI.Editor.DialogControls.TemplateManager = function(element)
{
	Telerik.Web.UI.Editor.DialogControls.TemplateManager.initializeBase(this, [element]);
}

Telerik.Web.UI.Editor.DialogControls.TemplateManager.prototype = {
	initialize : function() 
	{
		this.set_insertButton($get("InsertButton"));
		this.set_cancelButton($get("CancelButton"));
		
		$create(Telerik.Web.UI.Widgets.TemplatePreviewer, null, null, null, $get("TemplatePreviewer"));
		this.set_filePreviewer($find("TemplatePreviewer"));
		this.set_toolbar($find("fileManagerToolBar"));
		this.set_fileLister($find("GenericFileLister1"));
		
		Telerik.Web.UI.Editor.DialogControls.TemplateManager.callBaseMethod(this, 'initialize');
	},

	dispose : function() 
	{
		Telerik.Web.UI.Editor.DialogControls.TemplateManager.callBaseMethod(this, 'dispose');
		this._insertButton = null;
		this._cancelButton = null;
	}
}

Telerik.Web.UI.Editor.DialogControls.TemplateManager.registerClass('Telerik.Web.UI.Editor.DialogControls.TemplateManager', Telerik.Web.UI.Widgets.FileManager);
</script>
<table class="rade_dialog ManagerDialog TemplateManager" border="0" cellpadding="0"
	cellspacing="0" style="width: 100%; table-layout: fixed;">
	<tr>
		<td colspan="2" style="padding: 0; vertical-align: top;">
			<telerik:RadToolBar ID="fileManagerToolBar" runat="server">
				<Items>
					<telerik:RadToolBarButton Text="&nbsp;" ToolTip="Refresh" Value="Refresh" CssClass="rtbIconOnly icnRefresh" />
					<telerik:RadToolBarButton Text="&nbsp;" ToolTip="New Folder" Value="NewFolder" Enabled="false"
						CssClass="rtbIconOnly icnNewFolder" />
					<telerik:RadToolBarButton Text="&nbsp;" ToolTip="Delete" Value="Delete" Enabled="false"
						CssClass="rtbIconOnly icnDelete" />
					<telerik:RadToolBarButton Text="Upload" ToolTip="Upload" Value="Upload" Enabled="false" CheckOnClick="true" AllowSelfUncheck="true" CssClass="icnUpload" />
				</Items>
			</telerik:RadToolBar>
		</td>
	</tr>
	<tr id="listRow">
		<td style="width: 50%;padding: 0; vertical-align: top;">
			<widgets:GenericFileLister height="317px" id="GenericFileLister1" runat="server"></widgets:GenericFileLister>
		</td>
		<td style="width: 50%;padding: 0; vertical-align: top;">
			<div id="TemplatePreviewer">
				<fieldset style="margin: 0 4px; height: 357px;">
					<legend>Template Preview</legend>
					<iframe id="templateIframe" frameborder="0" height="342" width="320">preview</iframe>
				</fieldset>
			</div>
		</td>
	</tr>
	<tr id="uploadRow" style="display: none;">
		<td style="height: 317px;" colspan="2">
			<widgets:GenericFileUploader id="GenericFileUploader1" runat="server"></widgets:GenericFileUploader>
		</td>
	</tr>
	<tr>
		<td colspan="2" class="rade_bottomcell">
			<table border="0">
				<tr>
					<td style="padding-right: 4px;">
						<button type="button" id="InsertButton" style="width: 100px;">
							<script type="text/javascript">
							setInnerHtml("InsertButton", localization["Insert"]);
							</script>
						</button>
					</td>
					<td>
						<button type="button" id="CancelButton" style="width: 100px;">
							<script type="text/javascript">
							setInnerHtml("CancelButton", localization["Cancel"]);
							</script>
						</button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>