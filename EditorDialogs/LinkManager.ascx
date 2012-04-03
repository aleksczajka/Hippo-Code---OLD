<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />
<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");

Telerik.Web.UI.Widgets.LinkManager = function(element)
{
	Telerik.Web.UI.Widgets.LinkManager.initializeBase(this, [element]);
	this._clientParameters = null;
}

Telerik.Web.UI.Widgets.LinkManager.prototype = {
	initialize : function() 
	{
		Telerik.Web.UI.Widgets.LinkManager.callBaseMethod(this, 'initialize');
		this.setupChildren();
	},
	
	dispose : function() 
	{
		$clearHandlers(this._linkType);
		$clearHandlers(this._linkTargetCombo);
		$clearHandlers(this._existingAnchor);
		$clearHandlers(this._insertButton);
		$clearHandlers(this._cancelButton);

		Telerik.Web.UI.Widgets.LinkManager.callBaseMethod(this, 'dispose');
	},

	clientInit : function(clientParameters)
	{
		this._clientParameters = clientParameters;
		if(this._clientParameters.selectedTabIndex && this._clientParameters.selectedTabIndex >= 0)
		{
			this._tab.set_selectedIndex(this._clientParameters.selectedTabIndex);
		}
		//clean
		this._cleanInputBoxes();
		this._loadLinkArchor();
		//load data
		this._loadLinkProperties();
		this._linkTypeChangeHandler();
	},
	
	_cleanInputBoxes : function()
	{
		if (this._linkType.options && this._linkType.options.length>0)
		{
			this._linkType.options[0].selected = true;
		}
		this._linkUrl.value = "";
		this._linkText.value = "";
		if (this._linkTargetCombo.options && this._linkTargetCombo.options.length>0)
		{
			this._linkTargetCombo.options[0].selected = true;
		}
		this._linkTooltip.value = "";
		this._anchorName.value = "";
		this._emailAddress.value = "";
		this._emailLinkText.value = "";
		this._emailSubject.value = "";
		this._linkCssClass.set_value("");
		this._emailCssClass.set_value("");
	},
	
	_loadLinkProperties : function()
	{
	    var currentLink = this._clientParameters.realLink;
	    var currentHref = currentLink.getAttribute("href", 2);
	    var anchors = this._clientParameters.documentAnchors;
	      
	    //NEW: In IE RadFormDecorator styles textboxes in a way that the direct parent [of the textbox] changes, so the original implementation stopped working properly (in IE)
	    this._texTextBoxParentNode = Telerik.Web.UI.Editor.Utils.getElementParentByTag(this._linkText, "LI");  
	    this._emailTextBoxParentNode = Telerik.Web.UI.Editor.Utils.getElementParentByTag(this._emailLinkText, "LI");  
	    
	    
	    if(this._clientParameters.showText)
	    {
	        this._linkText.value = currentLink.innerHTML;
	        this._emailLinkText.value = currentLink.innerHTML;
	        	        
	        if (this._texTextBoxParentNode) this._texTextBoxParentNode.style.display = "";	        	    
	        if (this._emailTextBoxParentNode) this._emailTextBoxParentNode.style.display = "";
	    }
	    else
	    {
	        if (this._texTextBoxParentNode) this._texTextBoxParentNode.style.display = "none";	        	    
	        if (this._emailTextBoxParentNode) this._emailTextBoxParentNode.style.display = "none";
	    }
	    
	    this._loadCssClasses(currentLink);
	    
	    if(currentHref && currentHref.match(/^(mailto:)([^\?&]*)/ig)) // "email"
	    {
	        this._loadEmailAddressAndSubject();
	        this._tab.set_selectedIndex(2);
	        
	        return;
	    }
	   
	    if(currentLink.name && currentLink.name.trim() != "") // "anchor"
	    { 
	        this._anchorName.value = currentLink.name;
	        this._tab.set_selectedIndex(1);
	       
	        return;
	    }
	    
	    var href = "http://"; //"link"
	    
		if (currentLink.href)
		{
			href = currentHref;
		}
		
	    this._linkUrl.value = href;
	    this._loadLinkType(href);
	    
	    this._loadLinkTarget();
	    
	    this._linkTooltip.value = currentLink.title;
	    
	    this._tab.set_selectedIndex(0);
	},
	
	_loadLinkType : function(href)
	{
	    for(var i = 1; i < this._linkType.options.length; i++)
	    {
	        var optionType = this._linkType.options[i].value;
	        var re = new RegExp("^(" + optionType + ")", "gi");
	        
	        if (re.test(href))
			{
				this._linkType.options[i].selected = true;
				return;
			}
	    }
	    
	    this._linkType.options[0].selected = true;
	},
	
	_loadLinkTarget : function()
	{
	    var linkTarget = this._clientParameters.realLink.target;
	 
	    if(!linkTarget)
	    {
	        return;
	    }
	    
	    var optgroups = this._linkTargetCombo.getElementsByTagName("optgroup");
        
        
        for(var i = 0 ; i < optgroups.length ; i++)
        {
            if(optgroups[i].nodeName.toLowerCase() == 'optgroup')
            {   
                var options = optgroups[i].getElementsByTagName("option");
               
                for(var j = 0; j < options.length; j++)
	            {
                    if(options[j].nodeName.toLowerCase() == 'option' && 
                        options[j].value.toLowerCase() == linkTarget.toLowerCase())
                    {
                        options[j].selected = true;
                        return;
                    }
	            }
	        }
	    }
	    
	    var customOption = document.createElement("option");
	    customOption.value = linkTarget;
	    customOption.text = linkTarget;
	    this._linkTargetCombo.options.add(customOption);
	    customOption.selected = true;
	},
	
	_loadLinkArchor : function()
	{
		var anchors = this._clientParameters.documentAnchors;
		var linkHref = this._clientParameters.realLink.getAttribute("href", 2) ? this._clientParameters.realLink.getAttribute("href", 2).toLowerCase() : "";
		
		//clear existing options
		this._existingAnchor.innerHTML = "";
		this._existingAnchor.options.add(new Option(localization["None"], ""));
		this._existingAnchor.options[0].selected = true;
		
		for(var i = 0; i < anchors.length; i++)
		{
			var anchorOption = new Option(anchors[i].name, "#" + anchors[i].name);
			this._existingAnchor.options.add(anchorOption);

			if("#" + anchors[i].name.toLowerCase() == linkHref)
			{
				anchorOption.selected = true;
			}
		}
	},
	
	_loadCssClasses : function(currentLink)
	{
	    var cssClasses = this._clientParameters.CssClasses;
	    
	    //Copy the css classes to avoid one collection being modified by the other dropdown
	    this._linkCssClass.set_items(cssClasses.concat([]));
	    this._emailCssClass.set_items(cssClasses);
	    
	    if(currentLink.className != null && currentLink.className != "")
	    {
	        this._linkCssClass.updateValue(currentLink.className);
	        this._emailCssClass.updateValue(currentLink.className);
	    }
	},
	
	_loadEmailAddressAndSubject : function()
	{
	    var currentHref = this._clientParameters.realLink.getAttribute("href", 2);
        this._emailAddress.value = RegExp.$2;
        
        if(currentHref.match(/(\?|&)subject=([^\b]*)/ig))
        {
            var val = RegExp.$2.replace(/&amp;/gi, "&");
            val = unescape(val);
            this._emailSubject.value = val;
        }
	},
	
	_getModifiedLink : function()
	{
	    var resultLink = this._clientParameters.realLink;
    	var selectedIndex = this._tab.get_selectedIndex();
    	
	    if (selectedIndex == 0)//"link"
	    {
		    resultLink.href = this._linkUrl.value;
		    if(this._linkTargetCombo.value == "_none")
		    {
		        resultLink.removeAttribute("target", 0);
		    }
		    else
		    {
		        resultLink.target = this._linkTargetCombo.value;
		    }
		    
		    if (this._texTextBoxParentNode && this._texTextBoxParentNode.style.display != "none")
		    {
		        resultLink.innerHTML = this._linkText.value;
		    }
		    
		    if (resultLink.innerHTML.trim() == "" || resultLink.innerHTML.trim().length < this._linkText.value.trim().length)
		    {
					//try to replace <> if the content was marked as invalid html by the browser
					resultLink.innerHTML = this._linkText.value.replace(/&/gi,"&amp;").replace(/</gi,"&lt;").replace(/>/gi,"&gt;");
		    }

		    if(resultLink.innerHTML.trim() == "")
		    {
				resultLink.innerHTML = resultLink.href;
		    }
		    
		    if(this._linkTooltip.value.trim() == "")
		    {
		        resultLink.removeAttribute("title", 0);
		    }
		    else
		    {
		        resultLink.title = this._linkTooltip.value;
		    }
		    
		    this._setClass(resultLink, this._linkCssClass);
	    }
	    else if (selectedIndex == 1)//"anchor"
	    {
		    resultLink.removeAttribute("name");
			resultLink.removeAttribute("NAME");
			resultLink.name = null;
			resultLink.name = this._anchorName.value;
			resultLink["NAME"] = this._anchorName.value;
			
			//Make sure the href and some other attributes are removed just in case they are present
			resultLink.removeAttribute("href");
			resultLink.removeAttribute("target");
			resultLink.removeAttribute("title");

	    }
	    else //"email"
	    {
		    resultLink.href = "mailto:" + this._emailAddress.value;
		   
		    if (this._emailSubject.value != "")
		    {
			    resultLink.href += "?subject=" + this._emailSubject.value;
		    }
		    
            if (this._emailTextBoxParentNode && this._emailTextBoxParentNode.style.display != "none")
		    {
		        resultLink.innerHTML = this._emailLinkText.value;
		    }
            
		    this._setClass(resultLink, this._emailCssClass);
	    }
		
	    return resultLink;
	},
	
	_setClass : function(element, cssClassHolder)
	{
	    if(cssClassHolder.get_value() == "")
	    {
	        element.removeAttribute("className", 0);
	    }
	    else
	    {
	        element.className = cssClassHolder.get_value();
	    }
	},
	
	setupChildren : function()
	{
	    this._linkType = $get("LinkType");
		this._linkUrl = $get("LinkURL");
		this._linkText = $get("LinkText");
		
		this._linkTargetCombo = $get("LinkTargetCombo");
		this._setLinkTargetLocalization();
		this._existingAnchor = $get("ExistingAnchor");
		this._linkTooltip = $get("LinkTooltip");
	    this._linkCssClass = $find("LinkCssClass");
	    
	    this._anchorName = $get("AnchorName");
	    
	    this._emailAddress = $get("EmailAddress");
	    this._emailLinkText = $get("EmailLinkText");
	    this._emailSubject = $get("EmailSubject");
	    this._emailCssClass = $find("EmailCssClass");
	    
	    this._insertButton = $get("InsertButton");
	    this._insertButton.title = localization["OK"];
	    this._cancelButton = $get("CancelButton");
	    this._cancelButton.title = localization["Cancel"];
	    this._tab = $find("LinkManagerTab");
	    
	    this._initializeChildEvents();
	},
	
	_initializeChildEvents : function()
	{
	    this._linkCssClass.add_valueSelected(this._cssValueSelected);
	    this._emailCssClass.add_valueSelected(this._cssValueSelected);
	    $addHandlers(this._linkType, {"change" : this._linkTypeChangeHandler}, this);
	    $addHandlers(this._linkTargetCombo, {"change" : this._linkTargetChangeHandler}, this);
	    $addHandlers(this._existingAnchor, {"change" : this._existingAnchorChangeHandler}, this);
	    $addHandlers(this._insertButton, {"click" : this._insertClickHandler}, this);
	    $addHandlers(this._cancelButton, {"click" : this._cancelClickHandler}, this);
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
	
	_linkTypeChangeHandler : function(e)
	{
	    var linkType = this._linkType.options[this._linkType.selectedIndex].value;
	    var urlValue = this._linkUrl.value;
	    var index;

	    index = urlValue.indexOf(":");
	    if (index >= 0)
	    {
		    urlValue = urlValue.substring(index + 1);
	    }
	    index = urlValue.indexOf("//");
	    if (index >= 0)
	    {
		    urlValue = urlValue.substring(index + 2);
	    }
	    
	    if(linkType == "other")
	    {
	        linkType = "";
	    }
	    this._linkUrl.value = linkType + urlValue;
	},
	
	_linkTargetChangeHandler : function(e)
	{
        if(this._linkTargetCombo.value == "_custom")
	    {
			var targetprompttext = 'Type Custom Target Here';
			var targetprompt = prompt(targetprompttext, 'CustomWindow');
			
			if(targetprompt)
			{
				var newoption = document.createElement('option'); // create new <option> node
				newoption.innerHTML = targetprompt; // set innerHTML to the new <option> none
				newoption.setAttribute('selected', 'selected'); // set the new <option> node selected="selected"
				newoption.setAttribute('value', targetprompt); // change the value of the new <option> node with the value of the prompt
				this._linkTargetCombo.getElementsByTagName('optgroup')[1].appendChild(newoption); // append the new <option> node to the <optgroup>
	            return;			
			}
			
			this._linkTargetCombo.selectedIndex = 0;
	    }
	},
	
	_setLinkTargetLocalization : function()
	{
		var optgroups = this._linkTargetCombo.getElementsByTagName("optgroup");
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
	
	_existingAnchorChangeHandler : function(e)
	{
	    if (this._existingAnchor.selectedIndex != 0)
	    {
		    this._linkUrl.value = this._existingAnchor.value;
	    }
	},
	
	_insertClickHandler : function(e)
    {
        var args = {Result : this._getModifiedLink()};
        //backwards compatibility
        args.realLink = args.Result;
        Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close(args);
    },
    
     _cancelClickHandler : function(e)
    {
        Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();
    }
}

Telerik.Web.UI.Widgets.LinkManager.registerClass('Telerik.Web.UI.Widgets.LinkManager', Telerik.Web.UI.RadWebControl, Telerik.Web.IParameterConsumer);
</script>
<table cellpadding="0" cellspacing="0" class="rade_dialog LinkManager" style="width: 400px;height: 255px;">
	<tr>
		<td class="rade_topcell" style="padding: 0;">
			<telerik:RadTabStrip ID="LinkManagerTab" runat="server" SelectedIndex="0" MultiPageID="dialogMultiPage">
			<Tabs>
			<telerik:RadTab Text="Hyperlink" Value="Hyperlink"></telerik:RadTab>
			<telerik:RadTab Text="Anchor" Visible="false" Value="Anchor"></telerik:RadTab>
			<telerik:RadTab Text="E-mail" Value="E-mail"></telerik:RadTab>
			</Tabs>
			</telerik:RadTabStrip>
		</td>
	</tr>
	<tr>
		<td class="rade_middlecell" style="height: 214px; vertical-align: top; padding-bottom: 0;">
			<div class="rade_controlsPanel" style="padding-top: 10px;">
			<telerik:RadMultiPage ID="dialogMultiPage" runat="server" SelectedIndex="0">
			<telerik:RadPageView ID="hyperlinkFieldset" runat="server">
				<ul>
					<li style="margin-bottom: 6px;">
						<label for="LinkType" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">document.write(localization["LinkType"]);</script>
						</label>
						<select id="LinkType" class="l_input" style="width: 231px;">
							<option value=""><script type="text/javascript">document.write(localization["Other"]);</script></option>
							<option value="file://">file:</option>
							<option value="ftp://">ftp:</option>
							<option value="gopher://">gopher:</option>
							<option selected="selected" value="http://">http:</option>
							<option value="https://">https:</option>
							<option value="javascript:">javascript:</option>
							<option value="news://">news:</option>
							<option value="telnet://">telnet:</option>
							<option value="wais://">wais:</option>
						</select>
					</li>
					<li style="margin-bottom: 6px;">
						<label for="LinkURL" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">document.write(localization["LinkUrl"]);</script>
						</label>
						<input type="text" id="LinkURL" style="width: 222px;" />
					</li>
					<li style="margin-bottom: 6px;">
						<label for="LinkText" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">document.write(localization["LinkText"]);</script>
						</label>
						<input type="text" id="LinkText" style="width: 222px;" />
					</li>
					<li style="margin-bottom: 6px;">
						<label for="LinkTargetCombo" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">
							document.write(localization["LinkTarget"]);
							</script>
						</label>
						<select id="LinkTargetCombo" style="width: 231px;">
							<optgroup label="PresetTargets">
								<option value="_none">None</option>
								<option value="_self">TargetSelf</option>
								<option value="_blank">TargetBlank</option>
								<option value="_parent">TargetParent</option>
								<option value="_top">TargetTop</option>
								<option value="_search">TargetSearch</option>
								<option value="_media">TargetMedia</option>
							</optgroup>
							<optgroup label="CustomTargets">
								<option value="_custom">AddCustomTarget</option>
							</optgroup>
						</select>
					</li>
					<%--<li style="margin-bottom: 6px;" visible="false">
						<label for="ExistingAnchor" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">document.write(localization["ExistingAnchor"]);</script>
						</label>
						<select id="ExistingAnchor" class="l_input" style="width: 231px;">
							<option selected="selected">None</option>
						</select>
					</li>--%>
					<li>
						<label for="LinkCssClass" class="rightAlignedInputLabel" style="display: none;width: 100px;">
							<script type="text/javascript">
							document.write(localization["CssClass"]);
							</script>
						</label>
						<tools:ApplyClassDropDown id="LinkCssClass" runat="server" />
					</li>
					<li style="margin-bottom: 6px;">
						<label for="LinkTooltip" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">document.write(localization["LinkTooltip"]);</script>
						</label>
						<input type="text" id="LinkTooltip" style="width: 222px;" />
					</li>
					
				</ul>
			</telerik:RadPageView>
			<telerik:RadPageView ID="anchorFieldset" runat="server">
				<ul>
					<li style="margin-bottom: 6px;">
						<label for="AnchorName" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">document.write(localization["LinkName"]);</script>
						</label>
						<input type="text" id="AnchorName" style="width: 222px;" />
					</li>
				</ul>
			</telerik:RadPageView>
			<telerik:RadPageView ID="emailFieldset" runat="server">
				<ul>
					<li style="margin-bottom: 6px;">
						<label for="EmailAddress" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">
							document.write(localization["LinkAddress"]);
							</script>
						</label>
						<input type="text" id="EmailAddress" style="width: 222px;" />
					</li>
					<li style="margin-bottom: 6px;">
						<label for="EmailLinkText" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">
							document.write(localization["LinkText"]);
							</script>
						</label>
						<input type="text" id="EmailLinkText" style="width: 222px;" />
					</li>
					<li style="margin-bottom: 6px;">
						<label for="EmailSubject" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">
							document.write(localization["LinkSubject"]);
							</script>
						</label>
						<input type="text" id="EmailSubject" style="width: 222px;" />
					</li>
					<li>
						<label for="EmailCssClass" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">
							document.write(localization["CssClass"]);
							</script>
						</label>
						<tools:ApplyClassDropDown id="EmailCssClass" runat="server" />
					</li>
				</ul>
			</telerik:RadPageView>
			</telerik:RadMultiPage>
			</div>
		</td>
	</tr>
	<tr>
		<td class="rade_bottomcell">
			<table border="0" cellpadding="0" cellspacing="0" style="display: block; float: right;">
				<tr>
					<td style="padding-right: 6px;">
						<button type="button" id="InsertButton" style="width: 100px !important;">
							<script type="text/javascript">
							setInnerHtml("InsertButton", localization["OK"]);
							</script>
						</button>
					</td>
					<td>
						<button type="button" id="CancelButton" style="width: 100px !important;">
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