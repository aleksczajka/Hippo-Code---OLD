<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<dialogs:UserControlResources id="dialogResources" runat="server" />
<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");

Telerik.Web.UI.Widgets.ImagePreviewer = function(element)
{
	Telerik.Web.UI.Widgets.ImagePreviewer.initializeBase(this, [element]);
	this._imageContainer = null;
	this._altText = null;
	this._previewedImage = null;
	this._containerBounds = null;
	this._originalBounds = null;
	this._toolbarClickDelegate = null;
	this._currentItem = "";
}

Telerik.Web.UI.Widgets.ImagePreviewer.prototype = {
	initialize : function() 
	{
		Telerik.Web.UI.Widgets.ImagePreviewer.callBaseMethod(this, 'initialize');
		this._initializeChildren();
	},

	_initializeChildren : function()
	{
		this._imageContainer = this.get_element();
		this._containerBounds = $telerik.getBounds(this._imageContainer);
		this._altText = $get("AltText");
		this._toolbar = $find("ImagePreviewToolBar");
		this._toolbarClickDelegate = Function.createDelegate(this, this._toolbarClickHandler);
		this._toolbar.add_buttonClicked(this._toolbarClickDelegate);
	},
	
	_toolbarClickHandler : function(sender, args)
	{
		var buttonValue = args._item.get_value();
		switch (buttonValue)
		{
			case "BestFit":
				this._fitImage();
			break;
			case "ActualSize":
				this._adjustImage(this._originalBounds);
			break;
			case "ZoomIn":
				this._zoom(80);
			break;
			case "ZoomOut":
				this._zoom(-40);
			break;
			default:
			break;
		}
	},
	
	_zoom : function(zoomFactor)
	{
		this._adjustImage(this._getZoomedSize($telerik.getBounds(this._previewedImage), zoomFactor));
	},

	_getZoomedSize : function(size, zoomFactor)
	{
		return {
					width : size.width * (1 + zoomFactor/100),
					height : size.height * (1 + zoomFactor/100)
				}
	},

	_adjustImage : function(size)
	{
		var containerBounds = $telerik.getBounds(this._imageContainer);
		var topMargin = containerBounds.height - size.height;
		topMargin = topMargin < 0 ? 0 : topMargin/2;

		this._previewedImage.width  = size.width;
		this._previewedImage.height = size.height;
		this._previewedImage.style.marginTop = topMargin + "px";
	},

	_setToolbarEnabled : function(enabled)
	{
		this._toolbar.get_items().forEach(function(item){
			item.set_enabled(enabled);
		});
	},

	setItem : function(item)
	{
		this._imageContainer.innerHTML = "";
		this._altText.value = "";
		this._currentItem = item;
		if (item.type == Telerik.Web.UI.Widgets.FileItemType.Directory)
		{
			this._initNoImageItem();
			this._setToolbarEnabled(false);
		}
		else
		{
			this._initImageItem(item);
			this._setToolbarEnabled(true);
		}
	},
	
	getResult : function()
	{
		if (this._previewedImage)
		{
			var src = this._currentItem.getUrl();
			src = encodeURI(src);
			this._previewedImage.src = src;
			var returnImage = this._previewedImage.cloneNode(true);
			returnImage.setAttribute("alt", this._altText.value);
			returnImage.removeAttribute("width");
			returnImage.removeAttribute("height");
			returnImage.style.cssText = "";
			returnImage._events = {};
			returnImage.removeAttribute("_events");
			returnImage.removeAttribute("width");
			returnImage.removeAttribute("height");
			return returnImage;
		}
		return null;
	},
	
	_initImageItem : function(item)
	{
		this._previewedImage = new Image();
		var src = item.getUrl();
		src = encodeURI(src);
		this._previewedImage.src = src;
		this._previewedImage.style.border = "0";

		this._previewedImage.style.position = "absolute";
		this._previewedImage.style.top = "-10000px";
		this._previewedImage.style.left = "-10000px";
		this._imageContainer.className = "imagePreview";
		this._imageContainer.appendChild(this._previewedImage);

		this._setupPreviewedImage(false);

		this._altText.value = "";
	},
	
	_imageLoadHandler : function(e)
	{
		this._setupPreviewedImage(true);
		$clearHandlers(e.target);
	},
	
	_setupPreviewedImage : function(skipCompleteCheck)
	{
		if (skipCompleteCheck || this._previewedImage.complete)
		{
			this._originalBounds = {width:this._previewedImage.width, height:this._previewedImage.height};
			this._fitImage();
			this._previewedImage.style.position = "";
			this._previewedImage.style.top = "0px";
			this._previewedImage.style.left = "0px";
		}
		else
		{
			$addHandlers(this._previewedImage, {"load" : this._imageLoadHandler}, this);
		}

	},
	
	_initNoImageItem : function(item)
	{
		this._previewedImage = null;
		this._originalBounds = null;
		this._imageContainer.className = "imagePreview noImage";
	},

	_fitImage : function()
	{
		var hRatio = this._originalBounds.height / (this._containerBounds.height-1);
		var wRatio = this._originalBounds.width / (this._containerBounds.width-1);

		var ratio = 1;

		if (this._originalBounds.width > this._containerBounds.width && this._originalBounds.height > this._containerBounds.height)
		{
			ratio = hRatio >= wRatio ? hRatio:wRatio;
		}
		else if (this._originalBounds.width > this._containerBounds.width)
		{
			ratio = wRatio;
		}
		else if (this._originalBounds.height > this._containerBounds.height)
		{
			ratio = hRatio;
		}

		this._adjustImage({width:this._originalBounds.width / ratio, height:this._originalBounds.height / ratio});
	},

	dispose : function() 
	{
		this._toolbar.remove_buttonClicked(this._toolbarClickDelegate);
		this._toolbarClickDelegate = null;
		Telerik.Web.UI.Widgets.ImagePreviewer.callBaseMethod(this, 'dispose');
	}
}

Telerik.Web.UI.Widgets.ImagePreviewer.registerClass('Telerik.Web.UI.Widgets.ImagePreviewer', Telerik.Web.UI.Widgets.FilePreviewer);




Type.registerNamespace("Telerik.Web.UI.Editor.DialogControls");

Telerik.Web.UI.Editor.DialogControls.ImageManager = function(element)
{
	Telerik.Web.UI.Editor.DialogControls.ImageManager.initializeBase(this, [element]);
	this._imageEditorFileSuffix = "_thumb";
	this._enableImageEditor = true;
}

Telerik.Web.UI.Editor.DialogControls.ImageManager.prototype = {
	initialize : function() 
	{
		this.set_insertButton($get("InsertButton"));
		this.set_cancelButton($get("CancelButton"));
		
		$create(Telerik.Web.UI.Widgets.ImagePreviewer, null, null, null, $get("ImagePreviewer"));
		this.set_filePreviewer($find("ImagePreviewer"));
		this.set_toolbar($find("fileManagerToolBar"));
		this.set_fileLister($find("GenericFileLister1"));
		
		Telerik.Web.UI.Editor.DialogControls.ImageManager.callBaseMethod(this, 'initialize');
	},

	openImageEditor : function()
	{
		var lister = this._fileLister;
		if ((lister.get_currentDirectory().permissions & Telerik.Web.UI.Widgets.FileItemPermissions.Upload) && lister.get_selectedItem() != null && lister.get_selectedItem().type == Telerik.Web.UI.Widgets.FileItemType.File)
		{
			var args = {};
			args.imageSrc = lister.get_selectedItem().getUrl();
			args.suffix = this.get_imageEditorFileSuffix();
			var callbackFunction = function(sender, args)
			{
				lister.refresh();
			}
			var dialogOpener = Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().get_dialogOpener();
			dialogOpener.open("ImageEditor", args, callbackFunction);
		}
	},

	listerDisplay : function(sender, args)
	{
		Telerik.Web.UI.Editor.DialogControls.ImageManager.callBaseMethod(this, 'listerDisplay', [sender, args]);
		var hasPermission = this.get_enableImageEditor() ? (this._fileLister.get_currentDirectory().permissions & Telerik.Web.UI.Widgets.FileItemPermissions.Upload) && this._fileLister.get_selectedItem() != null && this._fileLister.get_selectedItem().type == Telerik.Web.UI.Widgets.FileItemType.File : false;
		this._setToolbarButtonEnabledState("ImageEditor", hasPermission);
	},
	
	listerItemClick : function(sender, args)
	{
		Telerik.Web.UI.Editor.DialogControls.ImageManager.callBaseMethod(this, 'listerItemClick', [sender, args]);
		var hasPermission = this.get_enableImageEditor() ? (this._fileLister.get_currentDirectory().permissions & Telerik.Web.UI.Widgets.FileItemPermissions.Upload) && this._fileLister.get_selectedItem() != null && this._fileLister.get_selectedItem().type == Telerik.Web.UI.Widgets.FileItemType.File : false;
		this._setToolbarButtonEnabledState("ImageEditor", hasPermission);
	},
	
	toolbarClickHandler : function(sender, args)
	{
		Telerik.Web.UI.Editor.DialogControls.ImageManager.callBaseMethod(this, 'toolbarClickHandler', [sender, args]);
		if (args._item.get_value() == "ImageEditor")
		{
			this.openImageEditor();
		}
	},
	
	get_enableImageEditor : function()
	{
		return this._enableImageEditor;
	},

	set_enableImageEditor : function(value)
	{
		this._enableImageEditor = value;
	},

	get_imageEditorFileSuffix : function()
	{
		return this._imageEditorFileSuffix;
	},

	set_imageEditorFileSuffix : function(value)
	{
		this._imageEditorFileSuffix = value;
	},

	dispose : function() 
	{
		Telerik.Web.UI.Editor.DialogControls.ImageManager.callBaseMethod(this, 'dispose');
		this._insertButton = null;
		this._cancelButton = null;
	}
}

Telerik.Web.UI.Editor.DialogControls.ImageManager.registerClass('Telerik.Web.UI.Editor.DialogControls.ImageManager', Telerik.Web.UI.Widgets.FileManager);
</script>
<style type="text/css">

</style>
<table class="rade_dialog ManagerDialog" border="0" cellpadding="0"
	cellspacing="0" style="width: 100%; table-layout: fixed;">
	<tr>
		<td colspan="2" style="padding: 0;">
			<telerik:RadToolBar ID="fileManagerToolBar" runat="server">
				<Items>
					<telerik:RadToolBarButton Text="&nbsp;" ToolTip="Refresh" Value="Refresh" CssClass="rtbIconOnly icnRefresh" />
					<telerik:RadToolBarButton Text="&nbsp;" ToolTip="New Folder" Value="NewFolder" Enabled="false" CssClass="rtbIconOnly icnNewFolder" />
					<telerik:RadToolBarButton Text="&nbsp;" ToolTip="Delete" Value="Delete" Enabled="false" CssClass="rtbIconOnly icnDelete" />
					<telerik:RadToolBarButton Text="Upload" ToolTip="Upload" Value="Upload" Enabled="false" CssClass="icnUpload" CheckOnClick="true" AllowSelfUncheck="true" />
					<telerik:RadToolBarButton Text="Image Editor" ToolTip="Image Editor" Value="ImageEditor" Enabled="false" CssClass="icnImageEditor" />
				</Items>
			</telerik:RadToolBar>
		</td>
	</tr>
	<tr id="listRow">
		<td style="width: 50%; padding: 0; vertical-align: top; height:372px;"><widgets:GenericFileLister height="317px" id="GenericFileLister1" runat="server"></widgets:GenericFileLister></td>
		<td style="width: 50%; vertical-align: top; padding: 0; height:372px;">
			<table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
				<tr>
					<td style=" vertical-align: top; padding: 0;" class="propertiesTableCell">
						<telerik:RadToolBar ID="ImagePreviewToolBar" runat="Server">
							<Items>
								<telerik:RadToolBarButton Text="&nbsp;" ToolTip="Best Fit" Value="BestFit" Enabled="false" CssClass="rtbIconOnly icnBestFit" />
								<telerik:RadToolBarButton Text="&nbsp;" ToolTip="Actual Size" Value="ActualSize" Enabled="false" CssClass="rtbIconOnly icnActualSize" />
								<telerik:RadToolBarButton Text="&nbsp;" ToolTip="Zoom In" Value="ZoomIn" Enabled="false" CssClass="rtbIconOnly icnZoomIn" />
								<telerik:RadToolBarButton Text="&nbsp;" ToolTip="Zoom Out" Value="ZoomOut" Enabled="false" CssClass="rtbIconOnly icnZoomOut" />
							</Items>
						</telerik:RadToolBar>
					</td>
				</tr>
				<tr>
					<td style="padding: 0;">
						<div id="ImagePreviewer" class="imagePreview noImage" style="height: 293px;"></div>
					</td>
				</tr>
				<tr>
					<td style="padding-top: 4px;">
						<label for="AltText">
							<span class="propertyLabel" style="text-align: left; float: left; width: 80px; padding-left: 4px;"><script type="text/javascript">document.write(localization["ImageAltText"]);</script>:</span>
						</label>
						<input type="text" id="AltText" style="vertical-align: middle; margin-left: 4px; width: 237px; display: block; float: left;" />
					</td>
				</tr>
			</table>
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
						<button type="button" id="InsertButton" style="width: 100px; margin-right: 8px;">
							<script type="text/javascript">
							setInnerHtml("InsertButton", localization["Insert"]);
							</script>
						</button>
					</td>
					<td>
						<button type="button" id="CancelButton" style="width: 100px; margin-right: 4px;">
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