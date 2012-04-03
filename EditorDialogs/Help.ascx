<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />
<style type="text/css">
td
{
	text-align: center;
}
.rade_descriptionCell
{
	text-align: left;
}
.bottomBorder TD
{
	border-bottom: solid 1px gray;
}
</style>
<table cellpadding="0" cellspacing="0" class="rade_dialog HelpDialog " style="width: 710px;
	height: 450px;">
	<tr>
		<td>
			<div class="helpTopics">
				<!-- help topics go here -->
				<table cellpadding="0" cellspacing="0" class="helpTable bottomBorder">
					<tr>
						<th style="width: 24px;">
							Icon
						</th>
						<th style="width: 500px;">
							Description
						</th>
						<th>
							Shortcut</th>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="StyleBuilder">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Style Builder - Allows the user to apply styles to the currently selected element.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="TrackChangesDialog">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							TrackChanges - Provides a comparison between two contents.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="XhtmlValidator">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							XhtmlValidator - Uses the W3C XHTML Validator Page to perform validation of the
							current editor content.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="ConvertToUpper">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							ConvertToUpper - Convert the text of the current selection to upper case, preserving
							the non-text elements such as images and tables.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="ConvertToLower">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							ConvertToLower - Convert the text of the current selection to lower case, preserving
							the non-text elements such as images and tables.
						</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="ImageMapDialog">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							ImageMapDialog - Allow users to create image maps through draging over the images
							and creating hyperlink areas of different shapes.
						</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="FormatCodeBlock">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							FormatCodeBlock - Allow users to insert and format code blocks into the content.
						</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<select disabled="disabled">
							</select>
						</td>
						<td class="rade_descriptionCell">
							RealFontSize - Allows the user to apply to the current selection font size measured
							in pixels, rather than a fixed-size 1 to 7 (as does the FontSize tool).
						</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="ToggleScreenMode">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							ToggleScreenMode - Switches RadEditor into Full Screen Mode.
						</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="ToggleTableBorder">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Show/Hide Border - Shows or hides borders around tables in the content area.
						</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<select disabled="disabled">
							</select>
						</td>
						<td class="rade_descriptionCell">
							Zoom - Changes the level of text magnification.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="ModuleManager">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Module Manager - Activates /Deactivates modules from a drop-down list of available
							modules.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="FindAndReplace">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Find and Replace - Find (and replaces) text in the editor's content area.</td>
						<td>
							Ctrl+F</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Print">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Print button - Prints the contents of the RadEditor or the whole web page.
						</td>
						<td>
							Ctrl+P</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="AjaxSpellCheck">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							AjaxSpellCheck - Launches the spellchecker.
						</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Cut">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Cut button - Cuts the selected content and copies it to the clipboard.
						</td>
						<td>
							Ctrl+X</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Copy">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Copy button - Copies the selected content to the clipboard.
						</td>
						<td>
							Ctrl+C</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Paste">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Paste button - Pastes the copied content from the clipboard into the editor.
						</td>
						<td>
							Ctrl+V</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="PasteFromWord">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Paste from Word button - Pastes content copied from Word and removes the web-unfriendly
							tags.
						</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="PasteFromWordNoFontsNoSizes">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Paste from Word cleaning fonts and sizes button - cleans all Word-specific tags
							and removes font names and text sizes.
						</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="PastePlainText">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Paste Plain Text button - Pastes plain text (no formatting) into the editor.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="PasteAsHtml">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Paste as HTML button - Pastes HTML code in the content area and keeps all the HTML
							tags.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Undo">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Undo button - Undoes the last action.
						</td>
						<td>
							Ctrl+Z</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Redo">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Redo button - Redoes/Repeats the last action, which has been undone.
						</td>
						<td>
							Ctrl+Y</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="FormatStripper">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Format Stripper button - Removes custom or all formatting from selected text.
						</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Help">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Help - Launches the Quick Help you are currently viewing.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="AboutDialog">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							About Dialog - Shows the current version and credentials of RadEditor.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td colspan="3" align="center" valign="middle" style="padding-top: 10px">
							<strong>INSERT AND MANAGE LINKS, TABLES, SPECIAL CHARACTERS, IMAGES and MEDIA</strong></td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="ImageManager">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Image Manager button - Inserts an image from a predefined image folder(s).</td>
						<td>
							Ctrl+G</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="ImageMapDialog">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Image map - Allows users to define clickable areas within image.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="AbsolutePosition">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Absolute Object Position button - Sets an absolute position of an object (free move).</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="InsertTable">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Insert Table button - Inserts a table in the RadEditor.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="ToggleTableBorder">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Toggle Table Borders - Toggles borders of all tables within the editor.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="InsertSnippet">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Insert Snippet - Inserts pre-defined code snippets.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="InsertFormElement">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Insert Form Element - Inserts a form element from a drop-down list with available
							elements.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="InsertDate">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Insert Date button - Inserts current date.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="InsertTime">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Insert Time button - Inserts current time.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="FlashManager">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Flash Manager button - Inserts a Flash animation and lets you set its properties.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="MediaManager">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Windows Media Manager button - Inserts a Windows media object (AVI, MPEG, WAV, etc.)
							and lets you set its properties.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="DocumentManager">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Document Manager - Inserts a link to a document on the server (PDF, DOC, etc.)</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="LinkManager">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Hyperlink Manager button - Makes the selected text or image a hyperlink.</td>
						<td>
							Ctrl+K</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Unlink">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Remove Hyperlink button - Removes the hyperlink from the selected text or image.</td>
						<td>
							Ctrl+Shift+K</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<select disabled="disabled">
							</select>
						</td>
						<td class="rade_descriptionCell">
							Insert Special Character dropdown - Inserts a special character (&euro; &reg;, <font
								face="Arial">©, ±</font>, etc.)</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<select disabled="disabled">
							</select>
						</td>
						<td class="rade_descriptionCell">
							Insert Custom Link dropdown - Inserts an internal or external link from a predefined
							list.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="TemplateManager">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Choose HTML Template - Applies and HTML template from a predefined list of templates.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td colspan="3" align="center" valign="middle" style="padding-top: 10px">
							<strong>CREATE, FORMAT AND EDIT PARAGRAPHS and LINES</strong></td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="InsertParagraph">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Insert New Paragraph button - Inserts new paragraph.</td>
						<td>
							Ctrl+M</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<select disabled="disabled">
							</select>
						</td>
						<td class="rade_descriptionCell">
							Paragraph Style Dropdown button - Applies standard text styles to selected text.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Outdent">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Outdent button - Indents paragraphs to the left.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Indent">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Indent button - Indents paragraphs to the right.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="JustifyLeft">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Align Left button - Aligns the selected paragraph to the left.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="JustifyCenter">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Center button - Aligns the selected paragraph to the center.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="JustifyRight">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Align Right button - Aligns the selected paragraph to the right.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="JustifyFull">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Justify button - Justifies the selected paragraph.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="InsertUnorderedList">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Bulleted List button - Creates a bulleted list from the selection.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="InsertOrderedList">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Numbered List button - Creates a numbered list from the selection.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="InsertHorizontalRule">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Insert horizontal line (e.g. horizontal rule) button - Inserts a horizontal line
							at the cursor position.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td colspan="3" align="center" valign="middle" style="padding-top: 10px">
							<strong>CREATE, FORMAT AND EDIT TEXT, FONT and LISTS</strong></td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Bold">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Bold button - Applies bold formatting to selected text.</td>
						<td>
							Ctrl+B</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Italic">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Italic button - Applies italic formatting to selected text.</td>
						<td>
							Ctrl+I</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Underline">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Underline button - Applies underline formatting to selected text.</td>
						<td>
							Ctrl+U</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="StrikeThrough">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Strikethrough button - Applies strikethrough formatting to selected text.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Superscript">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Superscript button - Makes a text superscript.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="Subscript">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Subscript button - Makes a text subscript.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<select disabled="disabled">
							</select>
						</td>
						<td class="rade_descriptionCell">
							Font Select button - Sets the font typeface.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<select disabled="disabled">
							</select>
						</td>
						<td class="rade_descriptionCell">
							Font Size button - Sets the font size.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="ForeColor">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Text Color (foreground) button - Changes the foreground color of the selected text.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="BackColor">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Text Color (background) button - Changes the background color of the selected text.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<select disabled="disabled">
							</select>
						</td>
						<td class="rade_descriptionCell">
							Apply class - applies custom, predefined styles to the selected text.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<span class="FormatCodeBlock">&nbsp;</span>
						</td>
						<td class="rade_descriptionCell">
							Format Code Block - Allow users to insert and format code blocks into the content.</td>
						<td>
							-</td>
					</tr>
					<tr>
						<td class="rade_tool">
							<select disabled="disabled">
							</select>
						</td>
						<td class="rade_descriptionCell">
							Custom Links dropdown - Inserts custom, predefined link.</td>
						<td>
							-</td>
					</tr>
				</table>
			</div>
		</td>
	</tr>
	<tr>
		<td class="rade_bottomcell">
			<button type="button" onclick="javascript:Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();"
				style="width: 100px;" id="OkButton">
				OK</button>

			<script type="text/javascript">
				setInnerHtml("OkButton", localization["OK"]);
			</script>

		</td>
	</tr>
</table>
<div style="display: none;">
	<!-- hidden color picker will load the necessary css images -->
	<tools:ColorPicker id="ColorPicker" runat="server">
	</tools:ColorPicker>
</div>
