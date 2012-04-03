<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />
<style type="text/css">
	#SkinnedFindButton,
	#SkinnedrFindButton,
	#SkinnedReplaceButton,
	#SkinnedReplaceAllButton
	{
		width: 90px !important;
	}
</style>
<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");

Telerik.Web.UI.Widgets.FindAndReplace = function(element)
{
	Telerik.Web.UI.Widgets.FindAndReplace.initializeBase(this, [element]);
	this._clientParameters = null;
	this._findButton = null;
	this._rFindButton = null;
	this._replaceButton = null;
	this._replaceAllButton = null;

	this._editorRef = null;
	this._contentWindow = null;
	//Todo: move these to the server localization
	this._localization = {
		notFound : "The search string was not found.",
		notSupported : "The find function is not supported by this browser!",
		allReplaced : "{0} occurrences in the text have been replaced!"
	};
}

Telerik.Web.UI.Widgets.FindAndReplace.prototype = {
	initialize : function()
	{
		Telerik.Web.UI.Widgets.FindAndReplace.callBaseMethod(this, "initialize");
		this.setupChildren();
	},
	
	dispose : function()
	{
		$clearHandlers(this._findButton);
		$clearHandlers(this._rFindButton);
		$clearHandlers(this._replaceButton);
		$clearHandlers(this._replaceAllButton);
		$clearHandlers(this._cancelButton);

		this._editorRef = null;
		this._contentWindow = null;
		this._clientParameters = null;
		Telerik.Web.UI.Widgets.FindAndReplace.callBaseMethod(this, "dispose");
	},
	
	clientInit : function(clientParameters)
	{
		this._clientParameters = clientParameters;

		//TODO: get textbox instead of iframe when in HTML mode (implement modes)
		this._contentWindow = clientParameters._editor.get_contentWindow();
		this._editorRef = clientParameters._editor;
		
		//feature - copy selected text into the find boxes if it is one line
		this._tab.set_selectedIndex(0);
		var selectedText = this.getSelection().getText();
		if (selectedText.indexOf("\n") == -1)
		{
			this._rFind.value = selectedText;
			this._find.value = selectedText;
			this._rReplace.value = "";
		}
		else
		{
			this._rFind.value = "";
			this._find.value = "";
			this._rReplace.value = "";
		}
		
		this._wholeWordFind.checked = false;
		this._wholeWordReplace.checked = false;
		this._matchCaseReplace.checked = false;
		this._matchCaseFind.checked = false;
		this._upReplace.checked = false;
		this._upFind.checked = false;
		this._downReplace.checked = true;
		this._downFind.checked = true;
	},
	
	_replace : function(search_str, str)
	{
		var newStr = str;
		//escape html content (find and replace is not supposed to work in HTML view anyway).
		if (newStr)
			newStr = newStr.replace(/&/gi,"&amp;").replace(/</gi, "&lt;").replace(/>/gi,"&gt;");
		if (this.getSelection().getHtml()) {
			if ($telerik.isIE)
			{
				//IE selection bug
				try
				{
					this.getSelection().getRange().duplicate().pasteHTML(newStr);
				}
				catch (uspecifiedError)
				{
					//unspecified error is thrown when replacing text inside a textarea element
					this.getSelection().getRange().text = newStr;
				}
			}
			else
			{
				this._clientParameters._editor.pasteHtml(newStr, "FindAndReplace");
			}
		}
	},

	replaceEngine : function (stringToFind, newString, backwards, replaceMode, wholeWord, caseSensitive, wrapContent)
	{
		if (null == stringToFind || "" == stringToFind)
		{
			//nothing to do if search string is empty
			return;
		}
		
		//initialize parameters
		replaceMode =  replaceMode || "find";
		caseSensitive = caseSensitive || false;
		backwards = backwards || false;
		wrapContent = wrapContent || false;
		backwards = backwards || false;

		if (this._contentWindow.document.body.innerHTML == "") {
			this._showMessageBox(this._localization.notFound);
			return true;
		}

		if (replaceMode == "replace") {
			this._replace(stringToFind, newString);
			replaceMode = "find";
		}

		this.getSelection().collapse(backwards);

		if ($telerik.isIE) {
			var rng = this.getSelection().getRange();
			var flags = 0;
			if (wholeWord) flags = flags | 2;
			if (caseSensitive) flags = flags | 4;

			if (!rng.findText) {
				this._showMessageBox(this._localization.notSupported);
				return true;
			}

			if (replaceMode == "replaceAll") {
				var replaceCounter = 0;

				while (rng.findText(stringToFind, backwards ? -1 : 1, flags)) {
					replaceCounter++;
					rng.scrollIntoView();
					rng.select();
					this._replace(stringToFind, newString);
				}

				if (replaceCounter>0)
					this._showMessageBox(String.format(this._localization.allReplaced, replaceCounter));
				else
					this._showMessageBox(this._localization.notFound);

				return true;
			}

			if (rng.findText(stringToFind, backwards ? -1 : 1, flags)) {
				rng.scrollIntoView();
				rng.select();
			} else
				this._showMessageBox(this._localization.notFound);
		} else {
			if (replaceMode == "replaceAll") {
				var replaceCounter = 0;

				while (this._contentWindow.find(stringToFind, caseSensitive, backwards, wrapContent, wholeWord, false, false)) {
					replaceCounter++;
					this._replace(stringToFind, newString);
				}

				if (replaceCounter>0)
					this._showMessageBox(String.format(this._localization.allReplaced, replaceCounter));
				else
					this._showMessageBox(this._localization.notFound);

				return true;
			}

			if (!this._contentWindow.find(stringToFind, caseSensitive, backwards, wrapContent, wholeWord, false, false))
				this._showMessageBox(this._localization.notFound);
		}
	},

	getSelection : function()
	{
		if ($telerik.isIE && this._editorRef)
		{
			this._editorRef.setActive();
			this._editorRef.setFocus();
		}
		return new Telerik.Web.UI.Editor.Selection(this._contentWindow);
	},

	_showMessageBox : function(msg)
	{
		window.alert(msg);
	},

	execFind : function ()
	{
		var stringToFind = "";
		var backwards = false;
		var wholeWord = false;
		var caseSensitive = false;

		//find out which tab is active
		if (this._tab.get_selectedIndex() == 0)
		{
			stringToFind = this._find.value;
			wholeWord = this._wholeWordFind.checked;
			caseSensitive = this._matchCaseFind.checked;
			backwards = this._upFind.checked;
		}
		else
		{
			stringToFind = this._rFind.value;
			wholeWord = this._wholeWordReplace.checked;
			caseSensitive = this._matchCaseReplace.checked;
			backwards = this._upReplace.checked;
		}

		this.replaceEngine(stringToFind, null, backwards, "find", wholeWord, caseSensitive, true);
	},

	execReplace : function ()
	{
		this.replaceEngine(this._rFind.value, this._rReplace.value, this._upReplace.checked, "replace", this._wholeWordReplace.checked, this._matchCaseReplace.checked, true);
	},

	execReplaceAll : function ()
	{
		//up/down does not matter here so we send "false" (down)
		//focus the find box so the editor cursor position is lost
		if (this._rFind.focus)
			this._rFind.focus();
		this.replaceEngine(this._rFind.value, this._rReplace.value, false, "replaceAll", this._wholeWordReplace.checked, this._matchCaseReplace.checked, true);
	},

	setupChildren : function()
	{
		//dialog main controls
		this._tab = $find("dialogtabstrip");
		this._tab.add_tabSelected(Function.createDelegate(this, this._tabChangedHandler));
		
		this._cancelButton = $get("CancelButton");
		this._cancelButton.title = localization["Close"];

		//dialog buttons
		this._findButton = $get("FindButton");
		this._findButton.title = localization["FindNext"];
		this._rFindButton = $get("rFindButton");
		this._rFindButton.title = localization["FindNext"];
		this._replaceButton = $get("ReplaceButton");
		this._replaceButton.title = localization["Replace"];
		this._replaceAllButton = $get("ReplaceAllButton");
		this._replaceAllButton.title = localization["ReplaceAll"];

		//dialog form controls
		this._upReplace = $get("upReplace");
		this._upFind = $get("upFind");
		this._downReplace = $get("downReplace");
		this._downFind = $get("downFind");
		this._find = $get("find");
		this._rFind = $get("rFind");
		this._rReplace = $get("rReplace");
		this._wholeWordFind = $get("wholeWordFind");
		this._wholeWordReplace = $get("wholeWordReplace");
		this._matchCaseReplace = $get("matchCaseReplace");
		this._matchCaseFind = $get("matchCaseFind");

		//preselect "down" radio button
		this._downReplace.checked = true;
		this._downFind.checked = true;

		this._initializeChildEvents();
	},

	_initializeChildEvents : function()
	{
		$addHandlers(this._findButton, {"click" : this._findClickHandler}, this);
		$addHandlers(this._rFindButton, {"click" : this._findClickHandler}, this);
		$addHandlers(this._replaceButton, {"click" : this._replaceClickHandler}, this);
		$addHandlers(this._replaceAllButton, {"click" : this._replaceAllClickHandler}, this);
		$addHandlers(this._cancelButton, {"click" : this._cancelClickHandler}, this);
	},

	_tabChangedHandler : function(sender, args)
	{
		if (sender.get_selectedIndex() == 0)
		{
			this._find.value = this._rFind.value;
			this._wholeWordFind.checked = this._wholeWordReplace.checked;
			this._matchCaseFind.checked = this._matchCaseReplace.checked;
			this._upFind.checked = this._upReplace.checked;
			this._downFind.checked = this._downReplace.checked;
		}
		else
		{
			this._rFind.value = this._find.value;
			this._matchCaseReplace.checked = this._matchCaseFind.checked;
			this._wholeWordReplace.checked = this._wholeWordFind.checked;
			this._upReplace.checked = this._upFind.checked;
			this._downReplace.checked = this._downFind.checked;
		}
	},

	_cancelClickHandler : function(e)
	{
		Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();
	},
	
	_findClickHandler : function()
	{
		this.execFind();
	},

	_replaceClickHandler : function()
	{
		this.execReplace();
	},

	_replaceAllClickHandler : function()
	{
		this.execReplaceAll();
	}
}

Telerik.Web.UI.Widgets.FindAndReplace.registerClass("Telerik.Web.UI.Widgets.FindAndReplace", Telerik.Web.UI.RadWebControl, Telerik.Web.IParameterConsumer);

</script>

<table cellpadding="0" cellspacing="0" class="rade_dialog" style="width: 420px;">
	<tr>
		<td>
			<telerik:RadTabStrip ID="dialogtabstrip" runat="server" MultiPageID="dialogMultiPage" SelectedIndex="0">
				<Tabs>
					<telerik:RadTab Text="Find" Value="findPanel">
					</telerik:RadTab>
					<telerik:RadTab Text="Replace" Value="replacePanel">
					</telerik:RadTab>
				</Tabs>
			</telerik:RadTabStrip>
		</td>
	</tr>
	<tr>
		<td style="padding: 6px; height: 165px; vertical-align: top;">
			<telerik:RadMultiPage ID="dialogMultiPage" runat="server" SelectedIndex="0">
				<telerik:RadPageView ID="findPanel" runat="server">
					<div>
						<ul>
							<li>
								<label for="find">
									<span style="display: block; float: left; width: 124px; text-overflow: ellipsis;
										overflow: hidden; white-space: nowrap;" id="FindLabel">

										<script type="text/javascript">
								setInnerHtml("FindLabel", localization["Find"]);
										</script>

									</span>
									<input type="text" id="find" class="l_input" style="display: block; float: left;
										width: 170px; margin: 0 8px 0 0;" />
								</label>
								<button type="button" id="FindButton">

									<script type="text/javascript">
							setInnerHtml("FindButton", localization["Find"]);
									</script>

								</button>
								
							</li>
						</ul>
					</div>
					<div style="clear: both; padding-top: 45px;">
						<fieldset style="float: left; width: 180px; margin-right: 12px;">
							<legend id="SearchOptionsLabel">

								<script type="text/javascript">
						setInnerHtml("SearchOptionsLabel", localization["SearchOptions"]);
								</script>

							</legend>
							<ul>
								<li>
									<input type="checkbox" id="matchCaseFind" />
									<label for="matchCaseFind">
										<span id="MatchCaseLabel">

											<script type="text/javascript">
									setInnerHtml("MatchCaseLabel", localization["MatchCase"]);
											</script>

										</span>
									</label>
								</li>
								<li>
									<input type="checkbox" id="wholeWordFind" />
									<label for="wholeWordFind">
										<span id="MatchWholeWords">

											<script type="text/javascript">
									setInnerHtml("MatchWholeWords", localization["MatchWholeWords"]);
											</script>

										</span>
									</label>
								</li>
							</ul>
						</fieldset>
						<fieldset style="float: left; width: 176px;">
							<legend id="SearchDirection">

								<script type="text/javascript">
						setInnerHtml("SearchDirection", localization["SearchDirection"]);
								</script>

							</legend>
							<ul>
								<li>
									<input type="radio" name="searchDirection" id="upFind" />
									<label for="upFind">
										<span id="Up">

											<script type="text/javascript">
									setInnerHtml("Up", localization["Up"]);
											</script>

										</span>
									</label>
								</li>
								<li>
									<input type="radio" name="searchDirection" checked="checked" id="downFind" />
									<label for="downFind">
										<span id="Down">

											<script type="text/javascript">
									setInnerHtml("Down", localization["Down"]);
											</script>

										</span>
									</label>
								</li>
							</ul>
						</fieldset>
					</div>
				</telerik:RadPageView>
				<telerik:RadPageView ID="replacePanel" runat="server">
					<div>
						<ul>
							<li>
								<!-- find next -->
								<label for="rFind">
									<span style="display: block; float: left; width: 124px; text-overflow: ellipsis;
										overflow: hidden; white-space: nowrap;" id="rFindSpan">

										<script type="text/javascript">
								setInnerHtml("rFindSpan", localization["Find"]);
										</script>

									</span>
									<input type="text" id="rFind" class="l_input" style="display: block; float: left;
										width: 170px; margin: 0 8px 0 0;" />
								</label>
								<button type="button" id="rFindButton">

									<script type="text/javascript">
							setInnerHtml("rFindButton", localization["FindNext"]);
									</script>

								</button>
								<!-- / find next -->
							</li>
							<li style="clear: both;">
								<!-- replace -->
								<label for="rReplace">
									<span style="display: block; float: left; width: 124px; text-overflow: ellipsis;
										overflow: hidden; white-space: nowrap;" id="ReplaceWith">

										<script type="text/javascript">
								setInnerHtml("ReplaceWith", localization["ReplaceWith"]);
										</script>

									</span>
									<input type="text" id="rReplace" class="l_input" style="display: block; float: left;
										width: 170px; margin: 0 8px 0 0;" />
								</label>
								<button type="button" id="ReplaceButton">

									<script type="text/javascript">
							setInnerHtml("ReplaceButton", localization["Replace"]);
									</script>

								</button>
								<!-- / replace -->
							</li>
							<li style="height: 23px; overflow: hidden;" class="ieListFindAndReplaceDialogFix">
								<!-- replace all -->
								<label style="visibility: hidden;">
									<span style="display: block; float: left; width: 120px;">&nbsp;</span>
									<input type="text" class="l_input" style="width: 170px; margin: 0 9px 0 0;" />
								</label>
								<button type="button" id="ReplaceAllButton">

									<script type="text/javascript">
							setInnerHtml("ReplaceAllButton", localization["ReplaceAll"]);
									</script>

								</button>
								<!-- / replace all -->
							</li>
						</ul>
						<div style="clear: both; padding: 0 0 0 0;">
							<fieldset style="float: left; width: 180px; margin-right: 12px;">
								<legend id="SearchOptions">

									<script type="text/javascript">
							setInnerHtml("SearchOptions", localization["SearchOptions"]);
									</script>

								</legend>
								<ul>
									<li>
										<input type="checkbox" id="matchCaseReplace" />
										<label for="matchCaseReplace">
											<span id="matchCaseReplaceSpan">

												<script type="text/javascript">
										setInnerHtml("matchCaseReplaceSpan", localization["MatchCase"]);
												</script>

											</span>
										</label>
									</li>
									<li>
										<input type="checkbox" id="wholeWordReplace" />
										<label for="wholeWordReplace">
											<span id="MatchWholeWordsSpan">

												<script type="text/javascript">
										setInnerHtml("MatchWholeWordsSpan", localization["MatchWholeWords"]);
												</script>

											</span>
										</label>
									</li>
								</ul>
							</fieldset>
							<fieldset style="float: left; width: 176px;">
								<legend id="SearchDirectionLabel">

									<script type="text/javascript">
							setInnerHtml("SearchDirectionLabel", localization["SearchDirection"]);
									</script>

								</legend>
								<ul>
									<li>
										<input type="radio" name="replaceDirection" id="upReplace" style="border: 0;" />
										<label for="upReplace">
											<span id="SearchDirectionUpSpan">

												<script type="text/javascript">
										setInnerHtml("SearchDirectionUpSpan", localization["Up"]);
												</script>

											</span>
										</label>
									</li>
									<li>
										<input type="radio" name="replaceDirection" checked="checked" id="downReplace" />
										<label for="downReplace">
											<span id="SearchDirectionDownSpan">

												<script type="text/javascript">
										setInnerHtml("SearchDirectionDownSpan", localization["Down"]);
												</script>

											</span>
										</label>
									</li>
								</ul>
							</fieldset>
						</div>
					</div>
				</telerik:RadPageView>
			</telerik:RadMultiPage>
		</td>
	</tr>
	<tr>
		<td class="rade_bottomcell">
			<button type="button" id="CancelButton" style="width: 100px;">

				<script type="text/javascript">
				setInnerHtml("CancelButton", localization["Close"]);
				</script>

			</button>
		</td>
	</tr>
</table>
