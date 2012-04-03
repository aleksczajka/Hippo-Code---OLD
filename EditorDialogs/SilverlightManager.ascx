<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />

<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");

Telerik.Web.UI.Widgets.SilverlightPreviewer = function(element)
{
	Telerik.Web.UI.Widgets.SilverlightPreviewer.initializeBase(this, [element]);
	this._currentItem = null;
	this._currentSilverlightElement = null;
	
	this._toolbar = null;
	this._toolbarClickDelegate = null;
	
	this._silverlightPropertiesPane = null;
	this._silverlightPreviewPane = null;
	
	this._newImageWidth = null;
	this._newImageHeight = null;
	this._agWindowlessBox = null;
	this._agEnableHtmlAccessBox = null;
	this._agBgrColor = null;
}

Telerik.Web.UI.Widgets.SilverlightPreviewer.prototype = {
	initialize : function() 
	{
		Telerik.Web.UI.Widgets.SilverlightPreviewer.callBaseMethod(this, "initialize");
		this._initializeChildren();
		this._initializeChildEvents();		
	},

	_initializeChildren : function()
	{
		this._toolbar = $find("SilverlightPreviewToolBar");
		
		this._silverlightPropertiesPane = $get("silverlightPropertiesPane");
		this._silverlightPreviewPane = $get("silverlightPreviewPane");
		
	    this._newImageWidth		= $get("NewImageWidth");
	    this._newImageHeight	= $get("NewImageHeight");
	    this._agWindowlessCheckBox	= $get("agWindowless");
	    this._agEnableHtmlAccessCheckBox	= $get("agEnableHtmlAccess");
	    this._agBgrColor		= $get("agBgrColor");
	    this._setComboLocalization(this._agBgrColor);
	},
	
	_initializeChildEvents : function()
	{
		this._toolbarClickDelegate = Function.createDelegate(this, this._toolbarClickHandler);
		this._toolbar.add_buttonClicked(this._toolbarClickDelegate);
	},
	
	_setToolbarEnabled : function(enabled)
	{
		this._toolbar.get_items().forEach(function(item){
			item.set_enabled(enabled);
		});
	},
	
	_setComboLocalization : function(combo)
	{
		var options = combo.getElementsByTagName("option");
		for(var j = 0; j < options.length; j++)
		{
			var optName = options[j].text;
			if (localization[optName])
				options[j].text = localization[optName];
		}
	},
	
	_toolbarClickHandler : function(sender, args)
	{
		var buttonValue = args._item.get_value();
		switch (buttonValue)
		{
			case "Properties":
				if(this._currentItem && this._silverlightPropertiesPane.style.display == "none")
				{
					this._silverlightPreviewPane.innerHTML = "";
					this._currentSilverlightElement = null;
					this._switchPanes("none", "block");
				}
			break;
			case "Preview":
				if(this._currentItem && this._silverlightPreviewPane.style.display == "none")
				{
					this._silverlightPreviewPane.innerHTML = this._createSilverlightElement();
					this._switchPanes("block", "none");
				}
			break;
			default:
			break;
		}

	},

    _getParams : function(element)
    {
        var params = {};

        params["source"] = this._currentItem.getUrl();
        params["background"] = this._agBgrColor.value;
        params["windowless"] = this._agWindowlessCheckBox.checked.toString();
        params["enablehtmlaccess"] = this._agEnableHtmlAccessCheckBox.checked.toString();

        return params;
    },

    _getObjectAttributes : function()
    {
        var objectParams = {};

        if(isNaN(parseInt(this._newImageWidth.value, 10)) || parseInt(this._newImageWidth.value, 10).toString() != this._newImageWidth.value)
        {
            this._newImageWidth.value = "150";
        }
        objectParams["width"] = this._newImageWidth.value;
        if(isNaN(parseInt(this._newImageHeight.value, 10)) || parseInt(this._newImageHeight.value, 10).toString() != this._newImageHeight.value)
        {
            this._newImageHeight.value = "150";
        }
        objectParams["height"] = this._newImageHeight.value;

        objectParams["type"] = "application/x-silverlight";
        objectParams["data"] = "data:application/x-silverlight,";

        return objectParams;
    },

    _createSilverlightElement : function()
    {
        var sb = new Sys.StringBuilder("");
        sb.append("<object");
        
        //set object attributes
        var objArguments = this._getObjectAttributes();
        for (var argument in objArguments)
        {
            sb.append(String.format(" {0}=\"{1}\"", argument, objArguments[argument]));
        }
        sb.append(">");
        
        //set object params
        var params = this._getParams("object");
        for (var member in params)
        {
            sb.append(String.format("<param name=\"{0}\" value=\"{1}\">",member, params[member]));
        }
       //set embed attributes
//        sb.append("<embed > </embed>");
        sb.append("</object>");
        
 
        
//        var divContainer = document.createElement("div");
//        divContainer.innerHTML = sb.toString();
//        this._currentSilverlightElement = divContainer.firstChild;

        return sb.toString();
    },

	_setInitialValues : function()
	{
	    this._silverlightPreviewPane.innerHTML = "";
	    this._currentSilverlightElement = null;
	    
	    this._newImageWidth.value = "150";
	    this._newImageHeight.value = "150"; 
	    this._agWindowlessCheckBox.checked = true;
	    this._agEnableHtmlAccessCheckBox.checked = false;
	    this._agBgrColor.selectedIndex = 0;
	},
	
	_switchPanes : function(previewMode, propertiesMode)
	{
	    this._silverlightPreviewPane.style.display = previewMode;
	    this._silverlightPropertiesPane.style.display = propertiesMode;
	},
	
	setItem : function(item)
	{
	    this._setInitialValues();
	    
	    if (item.type == Telerik.Web.UI.Widgets.FileItemType.Directory)
	    {
		    this._setToolbarEnabled(false);	    

		    this._currentItem = null;		
		    this._silverlightPreviewPane.style.display = "none";
	        this._silverlightPropertiesPane.style.display = "none";
	    }
	    else
	    {
		    this._setToolbarEnabled(true);
	        this._currentItem = item;
		    this._switchPanes("none", "block");
	    }
	},
	
	getResult : function()
	{
	    if (this._currentItem && this._currentItem.type == Telerik.Web.UI.Widgets.FileItemType.File)
		{
			return this._createSilverlightElement();
		}
		return null;
	},
	
	populateObjectProperties : function(selectedObject)
	{
		//WARNING: Currently tested only in IE7:
		if (selectedObject.object)
		{

			this._newImageWidth.value = (selectedObject.width) ? parseInt(selectedObject.width).toString() : "";
			this._newImageHeight.value = (selectedObject.height) ? parseInt(selectedObject.height).toString() : "";

			this._agWindowlessCheckBox.checked = selectedObject.object.Playing;
			this._agEnableHtmlAccessCheckBox.checked = selectedObject.object.Loop;
			this._selectOptionByValue(this._agBgrColor, "#" + selectedObject.object.BGColor, false, 0);
		}
	},
	
	dispose : function() 
	{
		this._setInitialValues();
		this._toolbar.remove_buttonClicked(this._toolbarClickDelegate);
		this._toolbarClickDelegate = null;
		
		Telerik.Web.UI.Widgets.SilverlightPreviewer.callBaseMethod(this, "dispose");
	}
}

Telerik.Web.UI.Widgets.SilverlightPreviewer.registerClass("Telerik.Web.UI.Widgets.SilverlightPreviewer", Telerik.Web.UI.Widgets.FilePreviewer);

Type.registerNamespace("Telerik.Web.UI.Editor.DialogControls");

Telerik.Web.UI.Editor.DialogControls.SilverlightManager = function(element)
{
	Telerik.Web.UI.Editor.DialogControls.SilverlightManager.initializeBase(this, [element]);
}

Telerik.Web.UI.Editor.DialogControls.SilverlightManager.prototype = {
	initialize : function() 
	{
		this.set_insertButton($get("InsertButton"));
		this.set_cancelButton($get("CancelButton"));
		
		$create(Telerik.Web.UI.Widgets.SilverlightPreviewer, null, null, null, $get("SilverlightPreviewer"));
		this.set_filePreviewer($find("SilverlightPreviewer"));
		this.set_toolbar($find("fileManagerToolBar"));
		this.set_fileLister($find("GenericFileLister1"));
		
		Telerik.Web.UI.Editor.DialogControls.SilverlightManager.callBaseMethod(this, 'initialize');
	},

	dispose : function() 
	{
		Telerik.Web.UI.Editor.DialogControls.SilverlightManager.callBaseMethod(this, 'dispose');
		this._insertButton = null;
		this._cancelButton = null;
	}
}

Telerik.Web.UI.Editor.DialogControls.SilverlightManager.registerClass('Telerik.Web.UI.Editor.DialogControls.SilverlightManager', Telerik.Web.UI.Widgets.FileManager);
</script>

<!-- Silverlight Manager Dialog Begin -->
<table class="rade_dialog ManagerDialog SilverlightManager" border="0" cellpadding="0"
	cellspacing="0" style="width: 100%; table-layout: fixed;">
	<tr>
		<td colspan="2">
			<telerik:RadToolBar ID="fileManagerToolBar" runat="server">
				<Items>
					<telerik:RadToolBarButton Text="&nbsp;" ToolTip="Refresh" Value="Refresh" CssClass="rtbIconOnly icnRefresh" />
					<telerik:RadToolBarButton Text="&nbsp;" ToolTip="New Folder" Value="NewFolder" Enabled="false"
						CssClass="rtbIconOnly icnNewFolder" />
					<telerik:RadToolBarButton Text="&nbsp;" ToolTip="Delete" Value="Delete" Enabled="false"
						CssClass="rtbIconOnly icnDelete" />
					<telerik:RadToolBarButton Text="Upload" ToolTip="Upload" Value="Upload" Enabled="false"
						CssClass="icnUpload" />
				</Items>
			</telerik:RadToolBar>
		</td>
	</tr>
	<tr id="listRow">
		<td style="width: 50%;">
			<widgets:GenericFileLister height="317px" id="GenericFileLister1" runat="server">
			</widgets:GenericFileLister></td>
		<td style="width: 50%;">
			<div id="SilverlightPreviewer">
				<em><script type="text/javascript">document.write(localization["Preview"]);</script></em>
				<telerik:RadToolBar ID="SilverlightPreviewToolBar" runat="Server">
					<Items>
						<telerik:RadToolBarButton Text="Properties" ToolTip="Properties" Value="Properties" />
						<telerik:RadToolBarButton Text="Preview" ToolTip="Preview" Value="Preview" />
					</Items>
				</telerik:RadToolBar>
				<div class="silverlightPropertiesPane" id="silverlightPropertiesPane" style="display: none;">
					<fieldset>
						<legend></legend>
						<ul>
							<li>
								<label for="NewImageWidth">
									<script type="text/javascript">document.write(localization["Width"]);</script></label><input type="text" class="textinput" id="NewImageWidth" /></li>
							<li>
								<label for="NewImageHeight">
									<script type="text/javascript">document.write(localization["Height"]);</script></label><input type="text" class="textinput" id="NewImageHeight" /></li>
						</ul>
						<ul>
							<li>
								<input type="checkbox" id="agWindowless" /><label
									for="agWindowless"></label></li>
							<li>
								<input type="checkbox" id="agEnableHtmlAccess" /><label
									for="agEnableHtmlAccess"></label></li>
						</ul>
						<ul>
							<li>
								<label for="agBgrColor">
									<script type="text/javascript">document.write(localization["BackgroundColor"]);</script></label><select id="agBgrColor">
										<option selected="selected" value="">NoColor</option>
										<option value="#000000" style="background-color: #000000;">Black</option>
										<option value="#0000ff" style="background-color: #0000ff;">Blue</option>
										<option value="#008000" style="background-color: #008000;">Green</option>
										<option value="#ffa500" style="background-color: #ffa500;">Orange</option>
										<option value="#ff0000" style="background-color: #ff0000;">Red</option>
										<option value="#ffffff" style="background-color: #ffffff;">White</option>
										<option value="#FFFF00" style="background-color: #FFFF00;">Yellow</option>
									</select></li>
						</ul>
					</fieldset>
				</div>
				<div class="silverlightPreviewPane" id="silverlightPreviewPane"
					style="display: none;">
				</div>
			</div>
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
			<button type="button" id="InsertButton" style="width: 100px; margin-right: 8px;">
				<script type="text/javascript">
				setInnerHtml("InsertButton", localization["Insert"]);
				</script>
			</button>
			<button type="button" id="CancelButton" style="width: 100px; margin-right: 4px;">
				<script type="text/javascript">
				setInnerHtml("CancelButton", localization["Cancel"]);
				</script>
			</button>
		</td>
	</tr>
</table>
<!-- Silverlight Manager Dialog End -->
