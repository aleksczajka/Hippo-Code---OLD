<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />

<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");

Telerik.Web.UI.Widgets.DocumentPreviewer = function(element)
{
	Telerik.Web.UI.Widgets.DocumentPreviewer.initializeBase(this, [element]);
	
	this._currentItem = null;
}

Telerik.Web.UI.Widgets.DocumentPreviewer.prototype = {
	initialize : function() 
	{
		Telerik.Web.UI.Widgets.DocumentPreviewer.callBaseMethod(this, 'initialize');
		this._initializeChildren();
	},

	_initializeChildren : function()
	{
		this._documentTooltip = $get("DocumentTooltip");
		this._documentTargetCombo = $get("DocumentTargetCombo");
		this._setLinkTargetLocalization();
		$addHandlers(this._documentTargetCombo, {"click" : this._targetChangeHandler}, this);
	},
	
	_setLinkTargetLocalization : function()
	{
		var optgroups = this._documentTargetCombo.getElementsByTagName("optgroup");
        for(var i = 0 ; i < optgroups.length ; i++)
        {
            var options = optgroups[i].getElementsByTagName("option");
            var grpName = optgroups[i].label;
            if (localization[grpName])
                optgroups[i].label = localization[grpName];
            for(var j = 0; j < options.length; j++)
            {
                var optName = options[j].text;
                if (localization[optName])
                    options[j].text = localization[optName];
            }
	    }
	},

	setItem : function(item)
	{
	    this._currentItem = item;
		this._documentTooltip.value = "";
		this._documentTargetCombo.selectedIndex = 0;
		
		if (item.type == Telerik.Web.UI.Widgets.FileItemType.Directory)
		{
		    this.get_element().style.display = "none";
		}
		else
		{
			this.get_element().style.display = "";
		}
	},
	
	getResult : function()
	{
		if (this._currentItem && this._currentItem.type == Telerik.Web.UI.Widgets.FileItemType.File)
		{
			var returnLink = document.createElement("A");
			returnLink.innerHTML = this._currentItem.name;
			returnLink.href = this._currentItem.getUrl();
			if(this._documentTooltip.value != "")
			{
			    returnLink.title = this._documentTooltip.value;
			}
			
			if(this._documentTargetCombo.value != "_none")
			{
			    returnLink.target = this._documentTargetCombo.value;
			}
			
			return returnLink;
		}
		return null;
	},
	
	_targetChangeHandler : function()
	{
	    if(this._documentTargetCombo.value == "_custom")
	    {
			var targetprompttext = 'Type Custom Target Here';
			var targetprompt = prompt(targetprompttext, 'CustomWindow');
			
			if(targetprompt)
			{
				var newoption = document.createElement('option'); // create new <option> node
				newoption.innerHTML = targetprompt; // set innerHTML to the new <option> none
				newoption.setAttribute('selected', 'selected'); // set the new <option> node selected="selected"
				newoption.setAttribute('value', targetprompt); // change the value of the new <option> node with the value of the prompt
				this._documentTargetCombo.getElementsByTagName('optgroup')[1].appendChild(newoption); // append the new <option> node to the <optgroup>
	            return;			
			}
			
			this._documentTargetCombo.selectedIndex = 0;
	    }
	},
	
	dispose : function() 
	{
	    $clearHandlers(this._documentTargetCombo);
	
		Telerik.Web.UI.Widgets.DocumentPreviewer.callBaseMethod(this, 'dispose');
	}
}

Telerik.Web.UI.Widgets.DocumentPreviewer.registerClass('Telerik.Web.UI.Widgets.DocumentPreviewer', Telerik.Web.UI.Widgets.FilePreviewer);


Type.registerNamespace("Telerik.Web.UI.Editor.DialogControls");

Telerik.Web.UI.Editor.DialogControls.DocumentManager = function(element)
{
	Telerik.Web.UI.Editor.DialogControls.DocumentManager.initializeBase(this, [element]);
}

Telerik.Web.UI.Editor.DialogControls.DocumentManager.prototype = {
	initialize : function() 
	{
		this.set_insertButton($get("InsertButton"));
		this.set_cancelButton($get("CancelButton"));
		
		$create(Telerik.Web.UI.Widgets.DocumentPreviewer, null, null, null, $get("DocumentPreviewer"));
		this.set_filePreviewer($find("DocumentPreviewer"));
		this.set_toolbar($find("fileManagerToolBar"));
		this.set_fileLister($find("GenericFileLister1"));
		
		Telerik.Web.UI.Editor.DialogControls.DocumentManager.callBaseMethod(this, 'initialize');
	},

	dispose : function() 
	{
		Telerik.Web.UI.Editor.DialogControls.DocumentManager.callBaseMethod(this, 'dispose');
		this._insertButton = null;
		this._cancelButton = null;
	}
}

Telerik.Web.UI.Editor.DialogControls.DocumentManager.registerClass('Telerik.Web.UI.Editor.DialogControls.DocumentManager', Telerik.Web.UI.Widgets.FileManager);
</script>

<table class="rade_dialog ManagerDialog DocumentManager" border="0" cellpadding="0"
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
					<telerik:RadToolBarButton Text="Upload" ToolTip="Upload" Value="Upload" Enabled="false"
						CssClass="icnUpload" CheckOnClick="true" AllowSelfUnCheck="true" />
				</Items>
			</telerik:RadToolBar>
		</td>
	</tr>
	<tr id="listRow">
		<td style="width: 50%; padding: 0; vertical-align: top;">
			<widgets:GenericFileLister height="317px" id="GenericFileLister1" runat="server">
			</widgets:GenericFileLister></td>
		<td style="width: 50%; padding: 8px 0 0 0; vertical-align: top;">
			<fieldset style="margin: 0 4px; height: 344px; margin-top: 12px;">
				<legend><script type="text/javascript">document.write(localization["DocumentPropertiesLegend"]);</script></legend>
				<div id="DocumentPreviewer" style="display: none;">
					<table border="0" cellpadding="0" cellspacing="0" width="100%" class="propertiesList">
						<tr>
							<td>
								<label for="DocumentTooltip" class="propertyLabel" style="width: 60px;">
									<script type="text/javascript">
									document.write(localization["Tooltip"]);
									</script>:
								</label>
							</td>
							<td>
								<input style="width: 244px;" type="text" id="DocumentTooltip" style="width: 246px;" />
							</td>
						</tr>
						<tr>
							<td>
								<label for="DocumentTargetCombo" class="propertyLabel" style="width: 60px;">
									<script type="text/javascript">
									document.write(localization["Target"]);
									</script>:
								</label>
							</td>
							<td>
								<select id="DocumentTargetCombo" style="width: 252px;">
									<optgroup label="PresetTargets">
										<option selected="selected" value="_none">None</option>
										<option value="_self">TargetSelf</option>
										<option value="_blank">TargetBlank</option>
										<option value="_parent">TargetParent</option>
										<option value="_top">TargetTop</option>
										<option value="_search">TargetSearch</option>
										<option value="_media">TargetMedia</option>
									</optgroup>
									<optgroup label="PresetTargets">
										<option value="_custom">AddCustomTarget</option>
									</optgroup>
								</select>
							</td>
						</tr>
					</table>
					<style type="text/css">
				.propertiesList td
				{
					padding: 4px 0 !important;
				}
				</style>
				</div>
			</fieldset>
		</td>
	</tr>
	<tr id="uploadRow" style="display: none;">
		<td style="height: 317px;" colspan="2">
			<widgets:GenericFileUploader id="GenericFileUploader1" runat="server">
			</widgets:GenericFileUploader>
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