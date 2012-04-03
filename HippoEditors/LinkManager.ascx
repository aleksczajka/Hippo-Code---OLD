<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />
<script type="text/javascript">

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
		<td class="rade_middlecell" style="height: 214px; vertical-align: top; padding-bottom: 0;">
			<div class="rade_controlsPanel" style="padding-top: 10px;">
				<ul>
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
					
					<li style="margin-bottom: 6px;">
						<label for="LinkTooltip" class="rightAlignedInputLabel" style="width: 100px;">
							<script type="text/javascript">document.write(localization["LinkTooltip"]);</script>
						</label>
						<input type="text" id="LinkTooltip" style="width: 222px;" />
					</li>
					
				</ul>
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