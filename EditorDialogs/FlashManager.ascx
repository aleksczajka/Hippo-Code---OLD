<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />

<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");

Telerik.Web.UI.Widgets.FlashPreviewer = function(element)
{
	Telerik.Web.UI.Widgets.FlashPreviewer.initializeBase(this, [element]);
	this._currentItem = null;
	this._currentFlashElement = null;
	
	this._toolbar = null;
	this._toolbarClickDelegate = null;
	
	this._flashPropertiesPane = null;
	this._flashPreviewPane = null;
	
	this._classIdCheckBox = null; 
	this._classIDText = null;
	this._classIDLi = null;
	this._newImageWidth = null;
	this._newImageHeight = null;
	this._newImageUnits = null;
	this._flPlayCheckBox = null;
	this._flLoopCheckBox = null;
	this._flMenuCheckBox = null;
	this._flTranspBgrCheckBox = null;
	this._flHtmlAlign = null;
	this._flFlashalign = null;
	this._flBgrColor = null;
}

Telerik.Web.UI.Widgets.FlashPreviewer.prototype = {
	initialize : function() 
	{
		Telerik.Web.UI.Widgets.FlashPreviewer.callBaseMethod(this, "initialize");
		this._initializeChildren();
		this._initializeChildEvents();		
	},

	_initializeChildren : function()
	{
		this._toolbar = $find("FlashPreviewToolBar");
		
		this._flashPropertiesPane = $get("flashPropertiesPane");
		this._flashPreviewPane = $get("flashPreviewPane");
		
		this._classIdCheckBox = $get("ClassID"); 
	    this._classIDText = $get("ClassIDText");
	    this._classIDLi = $get("ClassIDLi");
	    this._newImageWidth = $get("NewImageWidth");
	    this._newImageHeight = $get("NewImageHeight");
	    this._newImageUnits = $get("NewImageUnits");
	    this._flPlayCheckBox = $get("flPlay");
	    this._flLoopCheckBox = $get("flLoop");
	    this._flMenuCheckBox = $get("flMenu");
	    this._flTranspBgrCheckBox = $get("flTranspBgr");
	    this._flHtmlAlign = $get("flHtmlAlign");
	    this._setComboLocalization(this._flHtmlAlign);
	    this._flFlashalign = $get("flFlashalign");
	    this._setComboLocalization(this._flFlashalign);
	    this._flBgrColor = $get("flBgrColor");
	    this._setComboLocalization(this._flBgrColor);
	},
	
	_initializeChildEvents : function()
	{
		this._toolbarClickDelegate = Function.createDelegate(this, this._toolbarClickHandler);
		this._toolbar.add_buttonClicked(this._toolbarClickDelegate);
		$addHandlers(this._classIdCheckBox, {"click" : this._classIdClickHandler}, this);
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
				if(this._currentItem && this._flashPropertiesPane.style.display == "none")
				{
					this._flashPreviewPane.innerHTML = "";
					this._currentFlashElement = null;
					this._switchPanes("none", "block");
				}
			break;
			case "Preview":
				if(this._currentItem && this._flashPreviewPane.style.display == "none")
				{
					this._flashPreviewPane.innerHTML = this._createFlashElement();
					this._switchPanes("block", "none");
				}
			break;
			default:
			break;
		}

	},
	
	_classIdClickHandler : function(e)
	{
	    if(this._classIdCheckBox.checked)
	    {
	        this._classIDLi.style.display = "";
	    }
	    else
	    {
	        this._classIDLi.style.display = "none";
	    }
	},
	
	_getParams : function(element)
    {
        var params = {};
        if (element == "object")
        {
            params["Movie"] = this._currentItem.getUrl();
            params["play"] = this._flPlayCheckBox.checked.toString();
        }
        else
        {
            params["src"] = this._currentItem.getUrl();
            if(isNaN(parseInt(this._newImageWidth.value, 10)) || parseInt(this._newImageWidth.value, 10).toString() != this._newImageWidth.value)
            {
                this._newImageWidth.value = "150";
            }
            params["width"] = this._newImageWidth.value;
            if(isNaN(parseInt(this._newImageHeight.value, 10)) || parseInt(this._newImageHeight.value, 10).toString() != this._newImageHeight.value)
            {
                this._newImageHeight.value = "150";
            }
            params["height"] = this._newImageHeight.value;
            params["type"] = "application/x-shockwave-flash";
            params["pluginspage"] = "http://www.macromedia.com/go/getflashplayer";
        }
        params["quality"] = this._newImageUnits.value;
        if(this._flTranspBgrCheckBox.checked) params["wmode"] = "transparent";
        //optional
        params["loop"] = this._flLoopCheckBox.checked.toString();
        params["menu"] = this._flMenuCheckBox.checked.toString();
        if (this._flHtmlAlign.value.toLowerCase() != "baseline")
            params["align"] = this._flHtmlAlign.value;
        if (this._flFlashalign.value.toLowerCase() != "lt")
            params["salign"] = this._flFlashalign.value;
        if (this._flBgrColor.value.length >0)
            params["bgcolor"] = this._flBgrColor.value;
        return params;
    },

    _getObjectAttributes : function()
    {
        var objectParams = {};
        if(this._classIdCheckBox.checked && this._classIDText.value && this._classIDText.value.length>0)
        {
            objectParams["classid"] = this._classIDText.value;
        }
        else
        {
            objectParams["classid"] = "clsid:D27CDB6E-AE6D-11cf-96B8-444553540000";
        }

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
        if (this._flHtmlAlign.value && this._flHtmlAlign.value.toLowerCase() != "baseline")
        {
            objectParams["align"] = this._flHtmlAlign.value;
        }
        return objectParams;
    },

    _createFlashElement : function()
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
        sb.append("<embed");
        params = this._getParams("embed");
        for (var member in params)
        {
            sb.append(String.format(" {0}=\"{1}\"", member, params[member]));
        }
        sb.append("></embed>");


        sb.append("</object>");

//        var divContainer = document.createElement("div");
//        divContainer.innerHTML = sb.toString();
//        this._currentFlashElement = divContainer.firstChild;

        return sb.toString();
    },

	_setInitialValues : function()
	{
	    this._classIDLi.style.display = "none";
	    this._classIdCheckBox.checked = false;
	    this._flashPreviewPane.innerHTML = "";
	    this._currentFlashElement = null;
	    
	    this._classIDText.value = "";
	    this._newImageWidth.value = "150";
	    this._newImageHeight.value = "150"; 
	    this._newImageUnits.selectedIndex = 0;
	    this._flPlayCheckBox.checked = true;
	    this._flLoopCheckBox.checked = false;
	    this._flMenuCheckBox.checked = false;
	    this._flTranspBgrCheckBox.checked = true;
	    this._flHtmlAlign.selectedIndex = 0;
	    this._flFlashalign.selectedIndex = 0;
	    this._flBgrColor.selectedIndex = 0;
	},
	
	_switchPanes : function(previewMode, propertiesMode)
	{
	    this._flashPreviewPane.style.display = previewMode;
	    this._flashPropertiesPane.style.display = propertiesMode;
	},
	
	setItem : function(item)
	{
	    this._setInitialValues();
	    
	    if (item.type == Telerik.Web.UI.Widgets.FileItemType.Directory)
	    {
		    this._setToolbarEnabled(false);

		    this._currentItem = null;
		    this._flashPreviewPane.style.display = "none";
	        this._flashPropertiesPane.style.display = "none";
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
			return this._createFlashElement();
		}
		return null;
	},
	
	populateObjectProperties : function(selectedObject)
	{
		//WARNING: Currently tested only in IE7:
		if (selectedObject.object)
		{
			if (selectedObject.classid)
			{
				this._classIdCheckBox.click();
				this._classIDText.value = selectedObject.classid;
			}
			this._newImageWidth.value = (selectedObject.width) ? parseInt(selectedObject.width).toString() : "";
			this._newImageHeight.value = (selectedObject.height) ? parseInt(selectedObject.height).toString() : "";

			this._flPlayCheckBox.checked = selectedObject.object.Playing;
			this._flLoopCheckBox.checked = selectedObject.object.Loop;
			this._flMenuCheckBox.checked = selectedObject.object.Menu;
			this._flTranspBgrCheckBox.checked = selectedObject.object.WMode.toLowerCase() == "transparent";

			this._selectOptionByValue(this._newImageUnits, selectedObject.object.Quality2, false, 0);
			this._selectOptionByValue(this._flHtmlAlign, selectedObject.align, false, 0);
			this._selectOptionByValue(this._flFlashalign, selectedObject.object.SAlign, false, 0);
			this._selectOptionByValue(this._flBgrColor, "#" + selectedObject.object.BGColor, false, 0);
		}
	},
	
	dispose : function() 
	{
		this._setInitialValues();
		this._toolbar.remove_buttonClicked(this._toolbarClickDelegate);
		this._toolbarClickDelegate = null;
		$clearHandlers(this._classIdCheckBox);
		
		Telerik.Web.UI.Widgets.FlashPreviewer.callBaseMethod(this, "dispose");
	}
}

Telerik.Web.UI.Widgets.FlashPreviewer.registerClass("Telerik.Web.UI.Widgets.FlashPreviewer", Telerik.Web.UI.Widgets.FilePreviewer);

Type.registerNamespace("Telerik.Web.UI.Editor.DialogControls");

Telerik.Web.UI.Editor.DialogControls.FlashManager = function(element)
{
	Telerik.Web.UI.Editor.DialogControls.FlashManager.initializeBase(this, [element]);
}

Telerik.Web.UI.Editor.DialogControls.FlashManager.prototype = {
	initialize : function() 
	{
		this.set_insertButton($get("InsertButton"));
		this.set_cancelButton($get("CancelButton"));
		
		$create(Telerik.Web.UI.Widgets.FlashPreviewer, null, null, null, $get("FlashPreviewer"));
		this.set_filePreviewer($find("FlashPreviewer"));
		this.set_toolbar($find("fileManagerToolBar"));
		this.set_fileLister($find("GenericFileLister1"));
		
		Telerik.Web.UI.Editor.DialogControls.FlashManager.callBaseMethod(this, 'initialize');
	},

	dispose : function() 
	{
		Telerik.Web.UI.Editor.DialogControls.FlashManager.callBaseMethod(this, 'dispose');
		this._insertButton = null;
		this._cancelButton = null;
	}
}

Telerik.Web.UI.Editor.DialogControls.FlashManager.registerClass('Telerik.Web.UI.Editor.DialogControls.FlashManager', Telerik.Web.UI.Widgets.FileManager);
</script>
<table class="rade_dialog ManagerDialog FlashManager" border="0" cellpadding="0"
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
					<telerik:RadToolBarButton Text="Upload" ToolTip="Upload" Value="Upload" Enabled="false" CheckOnClick="true" AllowSelfUncheck="true"
						CssClass="icnUpload" />
				</Items>
			</telerik:RadToolBar>
		</td>
	</tr>
	<tr id="listRow">
		<td style="width: 50%; padding: 0; vertical-align: top; height:372px;">
			<widgets:GenericFileLister height="317px" id="GenericFileLister1" runat="server"></widgets:GenericFileLister>
		</td>
		<td style="width: 50%; padding: 0; vertical-align: top; height:372px;">
			<div id="FlashPreviewer" class="propertiesTableCell">
				<telerik:RadToolBar ID="FlashPreviewToolBar" runat="Server">
					<Items>
						<telerik:RadToolBarButton CssClass="NoIcon" Text="Properties" ToolTip="Properties" Value="Properties" CheckOnClick="true" AllowSelfUncheck="true" />
						<telerik:RadToolBarButton CssClass="NoIcon" Text="Preview" ToolTip="Preview" Value="Preview" CheckOnClick="true" AllowSelfUncheck="true" />
					</Items>
				</telerik:RadToolBar>
				<div class="flashPropertiesPane" id="flashPropertiesPane" style="display: none;">
					<fieldset style="margin: 0 4px; height: 325px;">
						<legend>
						    <script type="text/javascript">
						    document.write(localization["FlashPropertiesLegend"]);
						    </script>
			            </legend>
			            <style type="text/css">
			            .FlashPropertiesList td
			            {
			                padding: 2px 0 !important;
			            }
			            </style>
		                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="FlashPropertiesList">
		                    <tr>
		                        <td colspan="2">
		                            <input type="checkbox" id="ClassID" style="border: 0;" />
		                            <label for="ClassID">
		                                <script type="text/javascript">
		                                document.write(localization["ClassID"]);
		                                </script>:
		                            </label>
		                        </td>
		                    </tr>
		                    <tr id="ClassIDLi" style="display: none;">
		                        <td>
		                            <label for="ClassIDText" class="propertyLabel" style="width: 110px; text-align: left; padding-left: 2px;">
									    <script type="text/javascript">
									    document.write(localization["ClassIDText"]);
									    </script>:
									</label>
		                        </td>
		                        <td>
		                            <input type="text" class="textinput" id="ClassIDText" style="width: 198px;" />
		                        </td>
		                    </tr>
		                    <tr>
		                        <td>
		                            <label for="NewImageWidth" class="propertyLabel" style="width: 110px; text-align: left; padding-left: 2px;">
									    <script type="text/javascript">
									    document.write(localization["Width"]);
									    </script>:
									</label>
		                        </td>
		                        <td>
		                            <input type="text" id="NewImageWidth" style="width: 198px;" />
		                        </td>
		                    </tr>
		                    <tr>
		                        <td>
		                            <label for="NewImageHeight" class="propertyLabel" style="width: 110px; text-align: left; padding-left: 2px;">
									    <script type="text/javascript">
									    document.write(localization["Height"]);
									    </script>:
									</label>
		                        </td>
		                        <td>
		                            <input type="text" id="NewImageHeight" style="width: 198px;" />
		                        </td>
		                    </tr>
		                    <tr>
		                        <td>
		                            <label for="NewImageUnits" class="propertyLabel" style="width: 110px; text-align: left; padding-left: 2px;">
									    <script type="text/javascript">
									    document.write(localization["Quality"]);
									    </script>:
									</label>
		                        </td>
		                        <td>
		                            <select id="NewImageUnits" style="width: 204px;">
										<option selected="selected" value="high">high</option>
										<option value="medium">medium</option>
										<option value="low">low</option>
									</select>
		                        </td>
		                    </tr>
		                    <tr>
		                        <td colspan="2">
		                            <ul>
							            <li>
								            <input type="checkbox" id="flPlay" />
								            <label for="flPlay">
								                <script type="text/javascript">
								                document.write(localization["Play"]);
								                </script>
								            </label>
								        </li>
							            <li>
								            <input type="checkbox" id="flLoop" style="border: 0;" />
								            <label for="flLoop">
								                <script type="text/javascript">
								                document.write(localization["Loop"]);
								                </script>
								            </label>
								        </li>
							            <li>
								            <input type="checkbox" id="flMenu" style="border: 0;" />
								            <label for="flMenu">
								                <script type="text/javascript">
								                document.write(localization["FlashMenu"]);
								                </script>
								            </label>
								        </li>
							            <li>
								            <input type="checkbox" id="flTranspBgr" style="border: 0;" />
								            <label for="flTranspBgr">
								                <script type="text/javascript">
								                document.write(localization["Transparent"]);
								                </script>
								            </label>
								        </li>
						            </ul>
		                        </td>
		                    </tr>
		                    <tr>
		                        <td>
		                            <label for="flHtmlAlign" class="propertyLabel" style="width: 110px; text-align: left; padding-left: 2px;">
									    <script type="text/javascript">
									    document.write(localization["HtmlAlign"]);
									    </script>:
									</label>
		                        </td>
		                        <td>
		                            <select id="flHtmlAlign" style="width: 204px;">
										<option selected="selected" value="baseline">Baseline</option>
										<option value="bottom">Bottom</option>
										<option value="left">Left</option>
										<option value="middle">Middle</option>
										<option value="right">Right</option>
										<option value="texttop">TextTop</option>
										<option value="top">Top</option>
									</select>
		                        </td>
		                    </tr>
		                    <tr>
		                        <td>
		                            <label for="flFlashalign" class="propertyLabel" style="width: 110px; text-align: left; padding-left: 2px;">
									    <script type="text/javascript">
									    document.write(localization["FlashAlign"]);
									    </script>:
									</label>
							    </td>
		                        <td>
		                            <select id="flFlashalign" style="width: 204px;">
										<option selected="selected" value="LT">LeftTop</option>
										<option value="LC">LeftCenter</option>
										<option value="LB">LeftBottom</option>
										<option value="TR">RightTop</option>
										<option value="RC">RightCenter</option>
										<option value="RB">RightBottom</option>
										<option value="CT">CenterTop</option>
										<option value="CC">CenterCenter</option>
										<option value="CB">CenterBottom</option>
									</select>
		                        </td>
		                    </tr>
		                    <tr>
		                        <td>
		                            <label for="flBgrColor" class="propertyLabel" style="width: 110px; text-align: left; padding-left: 2px;">
									    <script type="text/javascript">
									    document.write(localization["BackgroundColor"]);
									    </script>:
									</label>
							    </td>
		                        <td>
		                            <select id="flBgrColor" style="width: 204px;">
										<option selected="selected" value="">NoColor</option>
										<option value="#000000" style="background-color: #000000;">Black</option>
										<option value="#0000ff" style="background-color: #0000ff;">Blue</option>
										<option value="#008000" style="background-color: #008000;">Green</option>
										<option value="#ffa500" style="background-color: #ffa500;">Orange</option>
										<option value="#ff0000" style="background-color: #ff0000;">Red</option>
										<option value="#ffffff" style="background-color: #ffffff;">White</option>
										<option value="#FFFF00" style="background-color: #FFFF00;">Yellow</option>
									</select>
		                        </td>
		                     </tr>
		                  </table>
					</fieldset>
				</div>
				<div class="flashPreviewPane" id="flashPreviewPane" style="display: none;"><!-- / --></div>
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