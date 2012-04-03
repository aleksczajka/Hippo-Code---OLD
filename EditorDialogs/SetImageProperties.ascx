<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />

<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");

Telerik.Web.UI.Widgets.ImageProperties = function(element)
{
	Telerik.Web.UI.Widgets.ImageProperties.initializeBase(this, [element]);
	this._clientParameters = null;
	this._allowedASCII = new Array(8, 16, 35, 36, 37, 39, 45, 46);//Maybe add letters "p" and "x"?
	this._constrainDimentions = false;
	this._ratio = 0;
}

Telerik.Web.UI.Widgets.ImageProperties.prototype = 
{
	initialize : function()
	{
		Telerik.Web.UI.Widgets.ImageProperties.callBaseMethod(this, 'initialize');

		this._setupChildren();
	},

	dispose : function()
	{
		$clearHandlers(this._imageWidth);
		$clearHandlers(this._imageHeight);
		$clearHandlers(this._constrainButton);
		$clearHandlers(this._insertButton);
		$clearHandlers(this._cancelButton);
		Telerik.Web.UI.Widgets.ImageProperties.callBaseMethod(this, 'dispose');
	},

	clientInit : function(clientParameters)
	{
		this._originalImage = clientParameters.Element;
		this._colors = clientParameters.Colors;
		this._editor = clientParameters.Editor;
		this._cssClasses = clientParameters.CssClasses;
		this._loadImageProperties();
	},

	_loadImageProperties : function()
	{
		this._imageSrc.set_editor(this._editor);
			
		var currentImage = this._originalImage.cloneNode(true);

		this._imageWidth.value = this._getOriginalWidth(this._originalImage);
		this._imageHeight.value = this._getOriginalHeight(this._originalImage);
        
        //NEW: Support for %		
		this._ratio = parseInt(this._imageWidth.value) / parseInt(this._imageHeight.value);

		this._imageSrc.set_value(currentImage.getAttribute("src", 2));

		this._imageAlt.value = this._getAttribute(currentImage, "alt");
		this._imageLongDecs.value = this._getAttribute(currentImage, "longDesc");
		this._imageAlignment.setTagName("IMG");

//TODO: This should be moved to a separate method - perhaps even a shared method because similar code is used in an editor filter and in the nodeinspectormodule
		var floatJSProperty = ($telerik.isIE) ? "styleFloat" : "cssFloat";
		var styleFloat = (typeof(currentImage.style[floatJSProperty]) == "undefined") ? "" : currentImage.style[floatJSProperty];
		var verticalAlign = (typeof(currentImage.style["verticalAlign"]) == "undefined") ? "" : currentImage.style["verticalAlign"];
		var alignValue = "";

		if(verticalAlign == "" && styleFloat != "")
		{
			switch(styleFloat)
			{
				case "left":
					alignValue = "left";
					break;
				case "right":
					alignValue = "right";
					break;
			}
		}
		
		if(styleFloat == "")
		{
			switch(verticalAlign)
			{
				case "top":
					alignValue = "top";
					break;
				case "middle":
					alignValue = "absmiddle";
					break;
				case "text-bottom":
					alignValue = "bottom";
					break;
			}
		}

		this._imageAlignment.updateValue(alignValue, null);

        var imageStyle = currentImage.style;
        
		if(imageStyle.marginTop)
			this._marginTopSpinBox.set_value(imageStyle.marginTop.replace("px", ""));
		else
			this._marginTopSpinBox.set_value("");

		if(imageStyle.marginRight)
			this._marginRightSpinBox.set_value(imageStyle.marginRight.replace("px", ""));
		else
			this._marginRightSpinBox.set_value("");

		if(imageStyle.marginBottom)
			this._marginBottomSpinBox.set_value(imageStyle.marginBottom.replace("px", ""));
		else
			this._marginBottomSpinBox.set_value("");

		if(imageStyle.marginLeft)
			this._marginLeftSpinBox.set_value(imageStyle.marginLeft.replace("px", ""));
		else
			this._marginLeftSpinBox.set_value("");
					
		var borderValue = parseInt(imageStyle.borderWidth);
		if(isNaN(borderValue)) borderValue = "";
		if(!borderValue)
		{
			var borderAttributeValue = currentImage.getAttribute("border");
			if(borderAttributeValue)
			{
				borderValue = borderAttributeValue;
				imageStyle.borderWidth = borderAttributeValue + "px";
				imageStyle.borderStyle = "solid";
			}
		}
		currentImage.removeAttribute("border");
		this._borderWidthSpinBox.set_value(borderValue);

		//Set colors to the color picker
		this._colorPicker.set_items(this._colors);
		var borderColor = imageStyle.borderColor.toUpperCase();
		this._colorPicker.set_color(borderColor);

		//Set css class names to the css dropdown and selects the one of the selected image if existing
		this._imageCssClassList.set_items(this._cssClasses);

		if(currentImage.className != null && currentImage.className != "")
		{
			this._imageCssClassList.updateValue(currentImage.className);
		}
	},


	_getOriginalWidth : function(currentImage)
	{
		if (!currentImage) return "";

		//Make 4 attempts to obtain width - 1)from style, 2) from attribute 3) from client size
		var width = "";

		if (currentImage.style.width)
		{
			width = currentImage.style.width;
		}

		//Try to obtain the value from the attribute.
		if (!width)
		{
			width = currentImage.getAttribute("width");
		}

		//Try script property
		if (!width)
		{
			width = currentImage.width;
		}

		if (!width)
		{
			width = currentImage.offsetWidth;
		}        
		return width;
	},

	_getOriginalHeight : function(currentImage)
	{
		if (!currentImage) return "";

		//Make 4 attempts to obtain height - 1)from style, 2) from attribute 3) from client size
		var height = "";

		if (currentImage.style.height)
		{
			height = currentImage.style.height;
		}

		//Try to obtain the value from the attribute.
		if (!height)
		{
			height = currentImage.getAttribute("height");
		}

		//Try script property
		if (!height)
		{
			height = currentImage.height;
		}

		if (!height)
		{
			height = currentImage.offsetHeight;
		}		        
		return height;
	},

	_getModifiedImage : function()
	{
		var resultImage =  this._originalImage.cloneNode(true);

		//Make sure the image src attribute is set before the widht/height, as setting the src causes IE to add width and height attributes expicitly
		this._setAttribute(resultImage, "src", this._imageSrc.get_value());

		this._setDimensionAttribute(resultImage, "width", this._imageWidth.value);
		this._setDimensionAttribute(resultImage, "height", this._imageHeight.value);
		this._setAttribute(resultImage, "alt", this._imageAlt.value);
		this._setAttribute(resultImage, "longDesc", this._imageLongDecs.value);

		//image align
		this._setImgAlignStyle(resultImage, this._imageAlignment.getAlign());

		var marginTop = parseInt(this._marginTopSpinBox.get_value());
		resultImage.style.marginTop = (!isNaN(marginTop)) ? marginTop + "px" : "";

		var marginRight = parseInt(this._marginRightSpinBox.get_value());
		resultImage.style.marginRight = (!isNaN(marginRight)) ? marginRight + "px" : "";

		var marginBottom = parseInt(this._marginBottomSpinBox.get_value());
		resultImage.style.marginBottom = (!isNaN(marginBottom)) ? marginBottom + "px" : "";

		var marginLeft = parseInt(this._marginLeftSpinBox.get_value());
		resultImage.style.marginLeft = (!isNaN(marginLeft)) ? marginLeft + "px" : "";

		var borderSize = parseInt(this._borderWidthSpinBox.get_value());
		if (!isNaN(borderSize) && borderSize >= 0)
		{
			resultImage.style.borderWidth = borderSize + "px";
			resultImage.style.borderStyle = "solid";
		}
		else
		{
			resultImage.style.borderWidth = "";
			resultImage.style.borderStyle = "";
		}
		resultImage.removeAttribute("border");

		if(this._colorPicker.get_color())
		{
			resultImage.style.borderColor = this._colorPicker.get_color();
		}

		this._setClass(resultImage, this._imageCssClassList);
		return resultImage;
	},

	_setImgAlignStyle : function(img, align)
	{
		var floatJSProperty = ($telerik.isIE) ? "styleFloat" : "cssFloat";
		var style = img.style;		
		switch(align)
		{
			case "left":
				style[floatJSProperty] = "left";
				style["verticalAlign"] = "";
				break;
			case "right":
				style[floatJSProperty] = "right";
				style["verticalAlign"] = "";
				break;
			case "top":
				style[floatJSProperty] = "";
				style["verticalAlign"] = "top";
				break;
			case "bottom":
				style[floatJSProperty] = "";
				style["verticalAlign"] = "text-bottom";
				break;
			case "absmiddle":
				style[floatJSProperty] = "";
				style["verticalAlign"] = "middle";
				break;
			default:
				style[floatJSProperty] = "";
				style["verticalAlign"] = "";
				break;
		}
		img.removeAttribute("align");
	},
		
	_getAttribute : function(image, attributeName)
	{
		var attributeValue = "";
		if(image.getAttribute(attributeName))
		{
			attributeValue = image.getAttribute(attributeName);
		}
		return attributeValue;
	},

	_setAttribute : function(image, attributeName, attributeValue)
	{
		if(attributeValue.trim())
		{
			image.setAttribute(attributeName, attributeValue);
		}
		else
		{
			image.removeAttribute(attributeName, false);
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

	_setupChildren : function()
	{
		this._imageWidth = $get("ImageWidth");
		this._imageHeight = $get("ImageHeight");
		this._constrainButton = $get("ConstrainButton");
		this._colorPicker = $find("BorderColor");
		this._imageAlignment = $find("ImageAlignment");
		this._imageAlt = $get("ImageAlt");
		this._imageLongDecs = $get("ImageLongDesc");
		this._imageSrc = $find("ImageSrc");
		this._marginTopSpinBox = $find("marginTop");
		this._marginRightSpinBox = $find("marginRight");
		this._marginBottomSpinBox = $find("marginBottom");
		this._marginLeftSpinBox = $find("marginLeft");
		this._borderWidthSpinBox = $find("ImageBorderWidth");
		this._imageCssClassList = $find("ImageCssClass");
		this._insertButton = $get("InsertButton");
		this._insertButton.title = localization["OK"];
		this._cancelButton = $get("CancelButton");
		this._cancelButton.title = localization["Cancel"];

		this._initializeChildEvents();
	},

	_initializeChildEvents : function()
	{
		$addHandlers(this._imageWidth, {"keyup" : this._validateDimensionByWidth}, this);
		$addHandlers(this._imageWidth, {"keydown" : this._validateNumber}, this);
		$addHandlers(this._imageHeight, {"keyup" : this._validateDimensionByHeight}, this);
		$addHandlers(this._imageHeight, {"keydown" : this._validateNumber}, this);
		$addHandlers(this._constrainButton, {"click" : this._constrainClickHandler}, this);
		$addHandlers(this._insertButton, {"click" : this._insertClickHandler}, this);
		$addHandlers(this._cancelButton, {"click" : this._cancelClickHandler}, this);
	},
	
	_setDimensionAttribute : function(image, attributeName, size)
	{
        //TODO - The removeAttribute code looks like it can be factored out to a separate utils method	
		image.removeAttribute(attributeName);

		if (image.style.removeAttribute)
		{
			image.style.removeAttribute(attributeName, false);
		}
		else
		{
			image.style[attributeName] = null;
		}
        				
		var originalSize = "height" == attributeName ? this._getOriginalHeight(this._originalImage) : this._getOriginalWidth(this._originalImage);
		if (size != originalSize)
		{
		    //NEW: Provide support for % as well, not just px
            var unit = $telerik.parseUnit(size);
			image.style[attributeName] = unit.size + unit.type;
		}
	},	
	

	_validateDimensionByWidth : function(e)
	{
		this._validateDimension(e, "width");
	},

	_validateDimensionByHeight : function(e)
	{
		this._validateDimension(e, "height");
	},
	
	_constrainClickHandler : function(e)
	{
		this._constrainDimentions = !this._constrainDimentions;

		if(this._constrainDimentions)
		{		  
		    //Update the readings on the gauges  
            this._updateConstraintGauges();
			Sys.UI.DomElement.addCssClass(this._constrainButton.parentNode, "toggle");
		}
		else
		{
			Sys.UI.DomElement.removeCssClass(this._constrainButton.parentNode, "toggle");
		}

		//Cancel the postback that the button causes in FF
		return $telerik.cancelRawEvent(e);
	},
	
	
	_updateConstraintGauges : function(attributeName)
	{	
	    //If no attributeName is specified, the "width" is assumed
	    var useWidth = (attributeName != "height");
	    
	    var dependantControl = null;
		var rulingControl = null;
		var ratio = 0;
        
	    if (useWidth)
		{
			dependantControl = this._imageHeight;
			rulingControl = this._imageWidth;
			ratio = 1/this._ratio;
		}
		else
		{
			dependantControl = this._imageWidth;
			rulingControl = this._imageHeight;
			ratio = this._ratio;
		}			
					
        //Support %, not just px
        var rulingUnit = $telerik.parseUnit(rulingControl.value);
        var size = rulingUnit.size;
                       
        //Set the value
		dependantControl.value = Math.ceil(size * ratio) + rulingUnit.type;		
    },
					
	_validateDimension : function(e, attributeName)
	{
		if (!this._validateNumber(e))		
			return false;
				
		if (this._constrainDimentions)
            this._updateConstraintGauges(attributeName);				
		
		return true;
	},
	
	_validateNumber : function(e)
	{	
		if (window.event != null) e = window.event;
		
		if (((e.keyCode >= 48) && (e.keyCode <= 57)) ||
			((e.keyCode >= 96) && (e.keyCode <= 105)) ||
			(Array.contains(this._allowedASCII, e.keyCode)))
		{
			return true;
		}
		else
		{			
			return $telerik.cancelRawEvent(e);
		}
	},

	_insertClickHandler : function(e)
	{
		Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close({ Result : this._getModifiedImage() });
	},

	_cancelClickHandler : function(e)
	{
		Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();
	}
}

Telerik.Web.UI.Widgets.ImageProperties.registerClass('Telerik.Web.UI.Widgets.ImageProperties', Telerik.Web.UI.RadWebControl, Telerik.Web.IParameterConsumer);

</script>
<style type="text/css">

</style>
<table cellspacing="0" cellpadding="0" class="rade_dialog ImageProperties" style="width: 100%;">
	<tr>
		<td style="width:42%;">
			<div class="propertyLabel">
				<script type="text/javascript">
				document.write(localization["Width"]);
				</script>:
			</div>
		</td>
		<td rowspan="2">
		    <table cellspacing="0" cellpadding="0">
		        <tr>
		            <td>
		                <input type="text" id="ImageWidth" /> px / %
		            </td>
		            <td rowspan="2">
		                <style type="text/css">
			            .ConstrainButton
			            {
				            position: static !important;
				            margin: -1px 0 0 4px !important;
			            }
			            </style>
			            <ul>
				            <li class="ConstrainProportions">
					            <button id="ConstrainButton" class="ConstrainButton">&nbsp;</button>
				            </li>
			            </ul>
			        </td>
		        </tr>
		        <tr>
		            <td>
		                <input type="text" id="ImageHeight" /> px / %
		            </td>
    		    </tr>
		    </table>
		</td>
	</tr>
	<tr>
		<td>
			<div class="propertyLabel">
				<script type="text/javascript">
				document.write(localization["Height"]);
				</script>:
			</div>
		</td>						
	</tr>
	<tr>
		<td>
			<div class="propertyLabel">
				<script type="text/javascript">
				document.write(localization["BorderColor"]);
				</script>:
			</div>
		</td>
		<td>
			<tools:ColorPicker id="BorderColor" runat="Server" />
		</td>
	</tr>
	<tr>
		<td>
			<div class="propertyLabel">
				<script type="text/javascript">
				document.write(localization["BorderWidth"]);
				</script>:
			</div>
		</td>
		<td >
			<tools:EditorSpinBox id="ImageBorderWidth" runat="server" />
		</td>
	</tr>
	<tr>
		<td>
			<div class="propertyLabel">
				<script type="text/javascript">
				document.write(localization["ImageAltText"]);
				</script>:
			</div>
		</td>
		<td >
			<input type="text" id="ImageAlt" style="width: 230px;" />
		</td>
	</tr>
	<tr>
		<td>
			<div class="propertyLabel">
				<script type="text/javascript">
				document.write(localization["LongDescription"]);
				</script>:
			</div>
		</td>
		<td >
			<input type="text" id="ImageLongDesc" style="width: 230px;" />
		</td>
	</tr>
	<tr>
		<td>
			<div class="propertyLabel">
				<script type="text/javascript">
				document.write(localization["ImageAlignment"]);
				</script>:
			</div>
		</td>
		<td >
			<tools:AlignmentSelector id="ImageAlignment" runat="Server" />
		</td>
	</tr>
	<tr>
		<td>
			<div class="propertyLabel">
				<script type="text/javascript">
				document.write(localization["ImageSrc"]);
				</script>:
			</div>
		</td>
		<td >
			<tools:ImageDialogCaller id="ImageSrc" runat="server" />
		</td>
	</tr>
	<tr>
		<td>
			<div class="propertyLabel">Margin:</div>
		</td>
		<td >
			<table border="0" cellpadding="0" cellspacing="5">
				<tr>
					<td style="width: 32px; text-align:right;">
						<script type="text/javascript">
						document.write(localization["Top"]);
						</script>:
					</td>
					<td>
					    <tools:EditorSpinBox id="marginTop" runat="server" />
					</td>
					<td style="width: 32px; text-align:right;">
						<script type="text/javascript">
						document.write(localization["Right"]);
						</script>:
					</td>
					<td>
					    <tools:EditorSpinBox id="marginRight" runat="server" />
					</td>
				</tr>
				<tr>
					<td style="width: 33px; text-align:right;">
						<script type="text/javascript">
						document.write(localization["Bottom"]);
						</script>:
					</td>
					<td>
					    <tools:EditorSpinBox id="marginBottom" runat="server" />
					</td>
					<td style="width: 33px; text-align:right;">
						<script type="text/javascript">
						document.write(localization["Left"]);
						</script>:
					</td>
					<td>
					    <tools:EditorSpinBox id="marginLeft" runat="server" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
			<div class="propertyLabel">
				<script type="text/javascript">
				document.write(localization["CssClass"]);
				</script>:
			</div>
		</td>
		<td >
			<tools:ApplyClassDropDown id="ImageCssClass" runat="Server" />
		</td>
	</tr>
	<tr>
		<td colspan="2" class="rade_bottomcell" style="padding: 10px 0 0 0;">
			<table border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td style="padding-right: 4px;">
						<button type="button" id="InsertButton" style="width: 100px;">
							<script type="text/javascript">
							setInnerHtml("InsertButton", localization["OK"]);
							</script>
						</button>
					</td>
					<td style="padding-right: 4px;">
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