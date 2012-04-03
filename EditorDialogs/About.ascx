<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Widgets" TagPrefix="widgets" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<dialogs:UserControlResources id="dialogResources" runat="server" />
<style type="text/css">

</style>
<table cellpadding="0" cellspacing="0" class="rade_dialog AboutDialog" style="width: 500px;
	height: 280px;">
	<!-- End of Dialog Content -->
	<tr>
		<td style="padding: 6px;">
			<h6 style="padding: 0; margin: 0;">
			    <span><!----></span>
			</h6>
			<ul style="margin: 0; clear: both; margin-bottom: 70px; margin-left: 12px; padding: 0;">
				<li><a href="http://www.telerik.com" title="Telerik" rel="external">www.telerik.com</a></li>
			</ul>
			<p style="margin: 0; margin-top: 158px; margin-left: 12px; padding: 0;">
			    <script type="text/javascript">
			    document.write(localization["Copyright"])
			    </script>
			</p>
		</td>
	</tr>
	<!-- End of Dialog Content -->
	<!-- Control Buttons -->
	<tr>
		<td class="rade_bottomcell">
			<button type="button" onclick="javascript:Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();" id="CancelButton" style="min-width: 100px; _width: 100px; margin-right: 8px;">
				<script type="text/javascript">
				setInnerHtml("CancelButton", localization["OK"]);
				</script>
			</button>
		</td>
	</tr>
	<!-- End of Control Buttons -->
</table>
