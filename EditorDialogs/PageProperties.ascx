<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />

<script type="text/javascript">

Type.registerNamespace("Telerik.Web.UI.Widgets");
Telerik.Web.UI.Widgets.PageProperties = function(element)
{
	Telerik.Web.UI.Widgets.PageProperties.initializeBase(this, [element]);

	this._clientParameters = null;
	this._editorRef = null;
	this._editorDocument = null;
	this._editorHead = null;
	this._editorBase = null;

	this._pageTitle = null;
	this._baseLocation = null;
	this._pageDescription = null;
	this._pageKeywords = null;
	this._pageBodyClassName = null;

	this._colorPicker = null;
	this._imageCaller = null;

	this._pageMarginTop = null;
	this._pageMarginBottom = null;
	this._pageMarginLeft = null;
	this._pageMarginRight = null;

	this._confirmButton = null;
	this._cancelButton = null;

	this._oMetaNames =
	{
		description: null,
		keywords: null
	};
}

Telerik.Web.UI.Widgets.PageProperties.prototype = {
	initialize : function()
	{
		Telerik.Web.UI.Widgets.PageProperties.callBaseMethod(this, "initialize");
		this.setupChildren();
	},

	setupChildren : function()
	{
		this._pageTitle = $get("PageTitle");
		this._baseLocation = $get("BaseLocation");
		this._pageDescription = $get("PageDescription");
		this._pageKeywords = $get("PageKeywords");
		this._pageBodyClassName = $find("PageBodyClassName");

		this._pageMarginTop = $get("PageMarginTop");
		this._pageMarginBottom = $get("PageMarginBottom");
		this._pageMarginLeft = $get("PageMarginLeft");
		this._pageMarginRight = $get("PageMarginRight");

		this._confirmButton = $get("confirm");
		this._confirmButton.title = localization["OK"];
		this._cancelButton = $get("cancel");
		this._cancelButton.title = localization["Cancel"];

		this._pageBodyClassName.add_valueSelected(this._cssValueSelected);
		$addHandlers(this._confirmButton, {"click" : this._confirmButtonClickHandler}, this);
		$addHandlers(this._cancelButton, {"click" : this._cancelButtonClickHandler}, this);
	},

	_confirmButtonClickHandler :function(e)
	{
		this._updateEditorDocument();
		Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close(Sys.EventArgs.Empty);
	},

	_cancelButtonClickHandler : function(e)
	{
		Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();
	},

	_cssValueSelected : function(oTool, args)
	{
		if (!oTool) return;
		var commandName = oTool.get_name();

		if("ApplyClass" == commandName)
		{
			var attribValue = oTool.get_selectedItem();
			oTool.updateValue(attribValue);
		}
	},

	clientInit : function(clientParameters)
	{
		this._clientParameters = clientParameters;
		this._editorRef = this._clientParameters.Editor;
		this._cssClasses = clientParameters.CssClasses;

		this._editorDocument = this._editorRef.get_document();
		this._editorHead = this._editorDocument.getElementsByTagName("HEAD")[0];

		this._colorPicker = $find("ColorPicker");
		this._colorPicker.set_items(this._clientParameters.Colors);

		this._imageCaller = $find("ImageCaller");
		this._imageCaller.set_editor(this._editorRef);

		this._loadData();
	},

	_getMetaTag : function(name)
	{
		var metas = this._editorDocument.getElementsByTagName("META");
		name = name.toLowerCase();
		var theMeta = null;

		for (var i = 0; i < metas.length; i++)
		{
			var meta = metas[i];
			var metaName = ("" + meta.getAttribute("name")).toLowerCase();
			if (name == metaName)
			{
				theMeta = meta;
			}
		}
		return theMeta;
	},

	_getAttribute : function(element, attributeName)
	{
		if (!element || !attributeName) return "";
		return element.getAttribute(attributeName);
	},

	_loadData : function()
	{
		if (this._editorDocument)
		{
			//TITLE
			this._pageTitle.value = this._editorDocument.title;

			this._pageDescription.value = "";
			this._pageKeywords.value = "";

			//&lt;meta name="keywords" content="keywords,keyword,keyword phrase,etc."&gt;
			var meta = this._getMetaTag("Description");
			if (meta)
			{
				this._oMetaNames["Description"] = meta.getAttribute("content");
				this._pageDescription.value = this._oMetaNames["Description"];
			}

			meta = this._getMetaTag("Keywords");
			if (meta)
			{
				this._oMetaNames["Keywords"] = meta.getAttribute("content");
				this._pageKeywords.value = this._oMetaNames["Keywords"];
			}

			// CSS classes
			var className = this._editorDocument.body.className;
			this._pageBodyClassName.set_items(this._cssClasses);

			if(className)
			{
				this._pageBodyClassName.updateValue(className);
			}

			var imagePath = this._editorDocument.body.style.backgroundImage;			
			imagePath = (!imagePath) ? "" : imagePath.substring(4, imagePath.length - 1);			
			this._imageCaller.set_value(imagePath);

			//BASES
			var bases = this._editorDocument.getElementsByTagName("BASE");
			if (bases.length > 0)
			{
				this._editorBase = bases[0];
				this._baseLocation.value = this._editorBase.getAttribute("href");
			}

			//Body attributes
			var marginTop = this._editorDocument.body.style.marginTop;
			this._pageMarginTop.value = marginTop.replace("px", "");
			
			var marginRight = this._editorDocument.body.style.marginRight;
			this._pageMarginRight.value = marginRight.replace("px", "");
			var marginBottom = this._editorDocument.body.style.marginBottom;
			this._pageMarginBottom.value = marginBottom.replace("px", "");
			var marginLeft = this._editorDocument.body.style.marginLeft;
			this._pageMarginLeft.value = marginLeft.replace("px", "");
			
						
			//BgColor
			//TODO: Use the following check this._editorDocument.body._getAttribute("bgColor") - otherwise FF returns "#ffffff"
			//even though there is no bgColor attribute.
			var backgroundColor = this._editorDocument.body.style.backgroundColor;			
			this._colorPicker.set_color(backgroundColor);
		}
	},

	_updateEditorMetaTag : function(metaName, metaContent)
	{
		var editorMetaTag = null;

		if (!this._oMetaNames[metaName] && metaContent)
		{
			//TODO: !$telerik.isFirefox
			if (document.all)
			{
				editorMetaTag = this._editorHead.appendChild(this._editorDocument.createElement("<META NAME='" + metaName + "'></META>"));
			}
			else
			{
				editorMetaTag = this._editorHead.appendChild(this._editorDocument.createElement("META"));
				editorMetaTag.setAttribute("name", metaName);
			}
		}
		else editorMetaTag = this._getMetaTag(metaName);

		if (editorMetaTag)
		{
			editorMetaTag.setAttribute("content", metaContent);
		}
	},

	//TODO: Move the _updateEditorDocument code to the callback function of the dialog.
	_updateEditorDocument : function()
	{
		// Title
		this._editorDocument.title = this._pageTitle.value;

		//Problem with setting a title in Mozilla
		if (!document.all)
		{
			var head = this._editorDocument.getElementsByTagName("HEAD")[0];
			var title = head.getElementsByTagName("title")[0];

			if(!title)
			{
				var title = this._editorDocument.createElement("title");
				head.appendChild(title);
			}

			if (title.firstChild) title.removeChild(title.firstChild);
			title.appendChild(this._editorDocument.createTextNode(this._pageTitle.value));
		}

		// Description, Keywords
		this._updateEditorMetaTag("Description", this._pageDescription.value);
		this._updateEditorMetaTag("Keywords", this._pageKeywords.value);

		// CSS
		this._setClass(this._editorDocument.body, this._pageBodyClassName);

		// Base
		var strBase = this._baseLocation.value;
		if (!this._editorBase && strBase)
		{
			this._editorBase = this._editorHead.appendChild(this._editorDocument.createElement("BASE"));
		}

		this._setAttribute(this._editorBase, "href", strBase);

		// COLOR
		//Firefox problem - even if the bg color has not been changed from the default,
		//FF will add a bgcolor attribute to the body tag
		if (this._editorDocument.body.style["backgroundColor"] != this._colorPicker.get_color())
		{
		    this._setCssPropertyValue(this._editorDocument.body, "backgroundColor", this._colorPicker.get_color(), "bgColor");			
		}

		// set the bg image
		var backgroundImage = this._imageCaller.get_value();
		backgroundImage = (backgroundImage) ? "url(" + backgroundImage + ")" : "";
		this._setCssPropertyValue(this._editorDocument.body, "backgroundImage", backgroundImage, "background");		

		//TODO: Make this with style, as FF does not apply the margin attributes.
		var marginTop = this._pageMarginTop.value;
		if(marginTop) marginTop += "px";
		this._setCssPropertyValue(this._editorDocument.body, "marginTop", marginTop);
		
		var marginRight = this._pageMarginRight.value;
		if(marginRight) marginRight += "px";
		this._setCssPropertyValue(this._editorDocument.body, "marginRight", marginRight);
		
		var marginBottom = this._pageMarginBottom.value;
		if(marginBottom) marginBottom += "px";
		this._setCssPropertyValue(this._editorDocument.body, "marginBottom", marginBottom);
		
		var marginLeft = this._pageMarginLeft.value;
		if(marginLeft) marginLeft += "px";		   
		this._setCssPropertyValue(this._editorDocument.body, "marginLeft", marginLeft);
		    		
		if (this._editorRef)
		{
			this._editorRef.set_fullPage(true);
		}
	},

	_setClass : function(element, cssClassHolder)
	{
		if(cssClassHolder.get_value() == "")
		{
			element.removeAttribute("className");
		}
		else
		{
			element.className = cssClassHolder.get_value();
		}
	},

    _setCssPropertyValue : function(element, cssProperty, value, attribute)
    {
        if (!element || !cssProperty) return;
        if (attribute) element.removeAttribute(attribute);
        element.style[cssProperty] = value;
    },

	_setAttribute : function(element, attributeName, attributeValue)
	{
		if (!element || !attributeName) return;
		if (attributeValue)
		{
			element.setAttribute(attributeName, attributeValue);
		}
		else
		{
			element.removeAttribute(attributeName);
		}
	},

	dispose : function()
	{
		$clearHandlers(this._confirmButton);
		$clearHandlers(this._cancelButton);

		this._clientParameters = null;
		this._editorRef = null;
		this._editorDocument = null;
		this._editorHead = null;
		this._editorBase = null;

		this._pageTitle = null;
		this._baseLocation = null;
		this._pageDescription = null;
		this._pageKeywords = null;
		this._pageBodyClassName = null;

		this._colorPicker = null;
		this._imageCaller = null;

		this._pageMarginTop = null;
		this._pageMarginBottom = null;
		this._pageMarginLeft = null;
		this._pageMarginRight = null;

		this._confirmButton = null;
		this._cancelButton = null;

		Telerik.Web.UI.Widgets.PageProperties.callBaseMethod(this, "dispose");
	}
}

Telerik.Web.UI.Widgets.PageProperties.registerClass('Telerik.Web.UI.Widgets.PageProperties', Telerik.Web.UI.RadWebControl, Telerik.Web.IParameterConsumer);

</script>

<style type="text/css">
/* xl_input */
.PageProperties td
{
	padding: 0;
}
</style>
<form id="Form1" action="" method="post">
	<table cellpadding="0" cellspacing="0" class="rade_dialog PageProperties" style="margin: 4px;
		margin-top: 0;">
		<tr>
			<td style="padding: 0;">
				<fieldset>
					<legend>
						<script type="text/javascript">document.write(localization["PageAttributes"]);</script>
					</legend>
					<table cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<label class="propertyLabel" for="PageTitle">
									<script type="text/javascript">document.write(localization["PageTitle"]);</script>:
								</label>
							</td>
							<td>
								<input type="text" id="PageTitle" style="width: 268px;" />
							</td>
						</tr>
						<tr>
							<td>
								<label class="propertyLabel" for="BaseLocation">
									<script type="text/javascript">document.write(localization["BaseLocation"]);</script>:
								</label>
							</td>
							<td>
								<input type="text" id="BaseLocation" style="width: 268px;" />
							</td>
						</tr>
						<tr>
							<td>
								<label class="propertyLabel" for="PageDescription">
									<script type="text/javascript">document.write(localization["Description"]);</script>:
								</label>
							</td>
							<td>
								<textarea id="PageDescription" style="width: 268px;" cols="10" rows="3"></textarea>
							</td>
						</tr>
						<tr>
							<td>
								<label class="propertyLabel" for="PageKeywords">
									<script type="text/javascript">document.write(localization["Keywords"]);</script>:
								</label>
							</td>
							<td>
								<textarea id="PageKeywords" style="width: 268px;" cols="10" rows="3"></textarea>
							</td>
						</tr>
					</table>
				</fieldset>
			</td>
		</tr>
		<tr>
			<td style="padding: 0;">
				<fieldset>
					<legend>
						<script type="text/javascript">document.write(localization["BodyAttributes"])</script>
					</legend>
					<table cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<div class="propertyLabel">
									<script type="text/javascript">document.write(localization["ClassName"]);</script>:
								</div>
							</td>
							<td>
								<tools:ApplyClassDropDown id="PageBodyClassName" runat="server" />
							</td>
						</tr>
						<tr>
							<td>
								<div class="propertyLabel">
									<script type="text/javascript">document.write(localization["BackColor"]);</script>:
								</div>
							</td>
							<td>
								<tools:ColorPicker id="ColorPicker" runat="server" />
							</td>
						</tr>
						<tr>
							<td>
								<div class="propertyLabel">
									<script type="text/javascript">document.write(localization["BackgroundImage"]);</script>:
								</div>
							</td>
							<td>
								<tools:ImageDialogCaller id="ImageCaller" runat="server" />
							</td>
						</tr>
					</table>
				</fieldset>
			</td>
		</tr>
		<tr>
			<td style="padding: 0;">
				<fieldset>
					<legend>Margins</legend>
					<table cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<label class="propertyLabel" for="PageMarginTop">
									<script type="text/javascript">document.write(localization["TopMargin"]);</script>:
								</label>
							</td>
							<td>
								<input type="text" style="width: 40px;" id="PageMarginTop" /> px
							</td>
							<td>
								<label for="PageMarginRight" class="propertyLabel">
									<script type="text/javascript">document.write(localization["RightMargin"]);</script>:
								</label>
							</td>
							<td>
								<input type="text" style="width: 40px;" id="PageMarginRight" /> px
							</td>
						</tr>
						<tr>
							<td>
								<label for="PageMarginBottom" class="propertyLabel">
									<script type="text/javascript">document.write(localization["BottomMargin"]);</script>:
								</label>
							</td>
							<td>
								<input type="text" style="width: 40px;" id="PageMarginBottom" /> px
							</td>
							<td>
								<label for="PageMarginLeft" class="propertyLabel">
									<script type="text/javascript">document.write(localization["LeftMargin"]);</script>:
								</label>
							</td>
							<td>
								<input type="text" style="width: 40px;" id="PageMarginLeft" /> px
							</td>
						</tr>
					</table>
				</fieldset>
			</td>
		</tr>
		<tr>
			<td class="rade_bottomcell" style="padding-right: 1px;">
				<button type="button" id="confirm" style="width: 100px; margin-right: 8px;">
					<script type="text/javascript">setInnerHtml("confirm", localization["OK"]);</script>
				</button>
				<button type="button" id="cancel" style="width: 100px;">
					<script type="text/javascript">setInnerHtml("cancel", localization["Cancel"]);</script>
				</button>
			</td>
		</tr>
	</table>
</form>