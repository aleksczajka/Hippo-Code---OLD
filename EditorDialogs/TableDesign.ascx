<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>

<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");
Telerik.Web.UI.Widgets.TableDesign = function(element)
{
	Telerik.Web.UI.Widgets.TableDesign.initializeBase(this, [element]);

	this._clientParameters = null;
	this._currentRowSpan = 1;
	this._currentColSpan = 1;

	this._spinBoxColumns = null;
	this._spinBoxColumnSpan = null;
	this._spinBoxRows = null;
	this._spinBoxRowSpan = null;

	/*THE SELECTED CELL AND ITS INDEX IN THE MODEL TABLE!!!*/
	this._selectedCell = null;
	this._selectedCellIndex = -1;
	this._victimColumns = new Array();
	this._victimRows = new Array();
	this._rowsCount = 0;

	this._tableToModify = null;
	this._tablePreviewControl = null;
	this._tableWizardDialog = null;
}

Telerik.Web.UI.Widgets.TableDesign.prototype = {
	initialize : function()
	{
		Telerik.Web.UI.Widgets.TableDesign.callBaseMethod(this, "initialize");

		this._initializeChildren();
	},

	_initializeChildren : function()
	{
		this._spinBoxDelegate = Function.createDelegate(this, this._spinBoxValueChanged);
		this._selectedCellDelegate = Function.createDelegate(this, this._onSelectedCellChanged);
		
		this._spinBoxColumns = this._initializeSpinBox("SpinBoxColumns");
		this._spinBoxColumnSpan = this._initializeSpinBox("SpinBoxColumnSpan");
		this._spinBoxRows = this._initializeSpinBox("SpinBoxRows");
		this._spinBoxRowSpan = this._initializeSpinBox("SpinBoxRowSpan");

		this._tablePreviewControl = new Telerik.Web.UI.Widgets.TablePreview();
		this._tablePreviewControl.set_previewHolder($get("TableDesign_PreviewTableHolder"));
		this._tablePreviewControl._onSelectedCellChanged = this._selectedCellDelegate;
	},

	clientInit : function(clientParameters)
	{
		this._clientParameters = clientParameters;
		this._tableToModify = this._clientParameters.originalTable;
		//this._selectedCell = this._clientParameters.selectedCell;

		this._loadData();
	},

	_initializeSpinBox : function(boxId)
	{
		var spinBox = $find(boxId);
		spinBox.add_valueSelected(this._spinBoxDelegate);
		spinBox.set_value(1);
		return spinBox;
	},
	
	_spinBoxValueChanged : function(sender, args)
	{
		var oldValue = args.get_oldValue();
		var newValue = args.get_newValue();
		var diff = newValue - oldValue;
		//difference should be either +1 or -1
		if (diff != -1 && diff != 1)
		{
			return;
		}
		switch (sender)
		{
			case this._spinBoxColumns:
				return (diff == 1) ? this._addNewColumn() : this._deleteLastColumn();
			break;
			case this._spinBoxColumnSpan:
				return (diff == 1) ? this._increaseColSpan() : this._decreaseColSpan();
			break;
			case this._spinBoxRows:
				return (diff == 1) ? this._addNewRow() : this._deleteLastRow();
			break;
			case this._spinBoxRowSpan:
				return (diff == 1) ? this._increaseRowSpan() : this._decreaseRowSpan();
			break;
			default:
			break;
		}
	},

	_loadData : function()
	{
		//clear dialog data because the page is not reloaded each time the dialog is opened!
		this._currentRowSpan = 1;
		this._currentColSpan = 1;
		this._selectedCell = null;
		this._selectedCellIndex = -1;
		this._victimColumns = new Array();
		this._victimRows = new Array();

		//clear the table preview control as well
		this._tablePreviewControl.deSelectAllCells();
		
		if(this._tableToModify)
		{
			this._rowsCount = this._tableToModify.rows.length;
		}
		this._tablePreviewControl._updateTable(this._tableToModify, this._selectedCell);
		this._checkButtonAvailability();
		this._spinBoxColumnSpan.set_value(this._currentColSpan);
		this._spinBoxRowSpan.set_value(this._currentRowSpan);
	},

	_onSelectedCellChanged : function()
	{
		this._synchronizeSelectedCell();
		this._checkButtonAvailability();
	},

	_synchronizeSelectedCell : function()
	{
		var previewSelectedCell = this._tablePreviewControl.get_selectedCell();
		var previewTable = this._tablePreviewControl.get_previewTable();

		var previewTableRows = previewTable.rows;
		this._selectedCell = null;
		this._selectedCellIndex = -1;
		for (var i=0; i<previewTableRows.length; i++)
		{
			var previewTableCells = previewTableRows[i].cells;
			for (var j=0; j<previewTableCells.length; j++)
			{
				if (previewTableCells[j] == previewSelectedCell)
				{
					this._selectedCell = this._tableToModify.rows[i].cells[j];
					this._selectedCellIndex = j;
					if(this._tableWizardDialog)
						this._tableWizardDialog._synchronizeSelection(this._selectedCell, this._selectedCellIndex);
					return;
				}
			}
		}
	},

	_checkButtonAvailability : function()
	{
		/*THE BAD THING HAPPENS HERE, BECAUSE WE'RE COMPARING THE CELLS DEPENDING ON THEIR OFFSETTOP ATTRIBUTE :(*/
		var table = this._tablePreviewControl.get_previewTable();

		//check row and column count for the row and column spinboxes
		if (this._getNamedNodesLength(table.firstChild, "TR") > 1)
		{
			this._spinBoxRows.set_enabledDecrease(true);
		}
		else
		{
			this._spinBoxRows.set_enabledDecrease(false);
		}
		if (this._getMaxColumns() > 1)
		{
			this._spinBoxColumns.set_enabledDecrease(true);
		}
		else
		{
			this._spinBoxColumns.set_enabledDecrease(false);
		}

		var selectedCell = this._tablePreviewControl.get_selectedCell();
		//if there is a selected cell, set the column and row span spin boxes
		if (selectedCell)
		{
			//first check the column span spinbox
			//increase
			var nextColumn = this._findNextNamedSibling(selectedCell, "TD");
			var offsetCalc = selectedCell.offsetLeft + selectedCell.offsetWidth;
			if ($telerik.isIE)
			{
				//don't know why
				offsetCalc +=1;
			}
			if (nextColumn && (selectedCell.offsetTop == nextColumn.offsetTop) && (offsetCalc == nextColumn.offsetLeft))
			{
				var i;
				var rows = table.rows;
				var column = nextColumn;
				this._currentColSpan = this._getColSpan(column);
				this._victimColumns = new Array(this._getEquivalentModelTableCell(column));
				for (i = this._getRowSpan(column);  i < this._getRowSpan(selectedCell); i += this._getRowSpan(column) )
				{
					column = this._getCellByOffset(this._findNextNamedSibling(selectedCell.parentNode, "TR"), nextColumn.offsetLeft);
					this._victimColumns[this._victimColumns.length] = this._getEquivalentModelTableCell(column);
					if (this._getColSpan(column)!= this._currentColSpan)
					{
						i = 0;
						break;
					}
				}
				if (this._getRowSpan(selectedCell)== i)
				{
					this._spinBoxColumnSpan.set_enabledIncrease(true);
				}
				else
				{
					this._spinBoxColumnSpan.set_enabledIncrease(false);
				}
			}
			else
			{
				this._spinBoxColumnSpan.set_enabledIncrease(false);
			}
			//decrease
			if (this._getColSpan(selectedCell) > 1)
			{
				this._spinBoxColumnSpan.set_enabledDecrease(true);
			}
			else
			{
				this._spinBoxColumnSpan.set_enabledDecrease(false);
			}

			// check the row span spinbox
			//increase
			var row = table.rows[this._getNamedNodeIndex(selectedCell.parentNode, "TR") + this._getRowSpan(selectedCell)];
			if (row)
			{
				var column = this._getCellByOffset(row, selectedCell.offsetLeft);
				if (column)
				{
					var i;
					var rows = table.rows;
					this._currentRowSpan = this._getRowSpan(column);
					this._victimRows = new Array(this._getEquivalentModelTableCell(column));
					for (i = this._getColSpan(column); i < this._getColSpan(selectedCell); i += this._getColSpan(column) )
					{
						column = this._findNextNamedSibling(column, "TD");
						this._victimRows[this._victimRows.length] = this._getEquivalentModelTableCell(column);
						if (this._getRowSpan(column) != this._currentRowSpan)
						{
							i = 0;
							break;
						}
					}
					if (this._getColSpan(selectedCell) == i)
					{
						this._spinBoxRowSpan.set_enabledIncrease(true);
					}
					else
					{
						this._spinBoxRowSpan.set_enabledIncrease(false);
					}
				}
				else
				{
					this._spinBoxRowSpan.set_enabledIncrease(false);
				}
			}
			else
			{
				this._spinBoxRowSpan.set_enabledIncrease(false);
			}
			//decrease
			if (this._getRowSpan(selectedCell) > 1)
			{
				this._spinBoxRowSpan.set_enabledDecrease(true);
			}
			else
			{
				this._spinBoxRowSpan.set_enabledDecrease(false);
			}
		}
		else
		{
			//no selected cell
			this._spinBoxRowSpan.set_enabledDecrease(false);
			this._spinBoxColumnSpan.set_enabledDecrease(false);
		}
	},

	_getNamedNodesLength : function(node, name)
	{
		var counter = 0;
		if(node)
		{
			for (var i=0; i<node.childNodes.length; i++)
			{
				if (node.childNodes[i].nodeName == name)
				{
					counter++;
				}
			}
		}
		return counter;
	},

	_getMaxColumns : function()
	{
		var maxCols = 0;
		var firstRow = this._tableToModify.rows[0];
		if(firstRow)
		{
			var cells = firstRow.cells;//childNodes;
			for (var i=0; i < cells.length; i++)
			{
				maxCols += this._getColSpan(cells[i]);
			}
		}
		return maxCols;
	},

	_findNextNamedSibling : function(node, name)
	{
		if (node != null)
		{
			var nSibling = node.nextSibling;
			while (nSibling != null)
			{
				if (nSibling.nodeName == name)
				{
					return nSibling;
				}
				nSibling = nSibling.nextSibling;
			}
		}
		return null;
	},

	_getRowSpan : function (oCell)
	{
		if (!oCell) return;
		return oCell.rowSpan > 0 ? oCell.rowSpan : 1;
	},

	_getColSpan : function (oCell)
	{
		if (!oCell) return;
		return oCell.colSpan > 0 ? oCell.colSpan : 1;
	},

	_getEquivalentModelTableCell : function(previewTableCell)
	{
		var table = this._tablePreviewControl.get_previewTable();
		var rows = table.rows;
		for (var i=0; i<rows.length; i++)
		{
			var cells = rows[i].cells;
			for (var j=0; j < cells.length; j++)
			{
				if (cells[j] == previewTableCell)
				{
					return this._tableToModify.rows[i].cells[j];
				}
			}
		}
	},

	_getCellByOffset : function(row, offset)
	{
		var cells = row.cells;
		for (var i=0; i<cells.length; i++)
		{
			if (cells[i].offsetLeft == offset)
			{
				return cells[i];
			}
		}
		return null;
	},

	_getNamedNodeIndex : function(node, name)
	{
		if (node && node.parentNode && node.parentNode.childNodes)
		{
			var nodesChild = node.parentNode.childNodes;
			var nodeIndex = 0;
			for (var i=0; i<nodesChild.length; i++)
			{
				if (nodesChild[i] == node)
				{
					return nodeIndex;
				}
				else if (nodesChild[i].nodeName == name)
				{
					nodeIndex++;
				}
			}
		}
		return -1;
	},

	_removeLastChild : function(node, name)
	{
		if (!node || !node.childNodes)
		{
			return;
		}
		var i=node.childNodes.length-1;
		while (i>=0 && node.childNodes[i].nodeName != name)
		{
			i--;
		}
		if (i>=0)
		{
			node.removeChild(node.childNodes[i]);
		}
	},

	_deleteLastColumn : function()
	{
		if (this._getMaxColumns() == 1)
		{
			return;
		}
		var modelTableRows = this._tableToModify.rows;
		for (var i=0; i<this._rowsCount; i++)
		{
			var modelTableCells = modelTableRows[i].cells;
			if (modelTableCells.length > 1 ||
				(modelTableCells.length == 1 && this._getColSpan(modelTableCells[0]) > 1)
			)
			{
				if (this._getColSpan(modelTableCells[modelTableCells.length - 1]) > 1)
				{
					modelTableCells[modelTableCells.length - 1].colSpan--;
				}
				else
				{
					this._removeLastChild(modelTableRows[i], "TD");
				}
			}
		}
		this._tablePreviewControl._updateTable(this._tableToModify, this._selectedCell);
		this._checkButtonAvailability();
	},

	_deleteLastRow : function()
	{
		if (this._rowsCount == 1)
		{
			return;
		}
		var modelTableRows = this._tableToModify.rows;
		for (var i=0; i<this._rowsCount; i++)
		{
			var modelTableCells = modelTableRows[i].cells;
			for (var j=0; j<modelTableCells.length; j++)
			{
				if (( this._getRowSpan(modelTableCells[j]) > 1) &&
					((i + this._getRowSpan(modelTableCells[j])) == this._rowsCount)
				)
				{
					modelTableCells[j].rowSpan--;
				}
			}
		}
		this._removeLastChild(modelTableRows[this._rowsCount-1].parentNode, "TR");
		this._rowsCount--;
		this._tablePreviewControl._updateTable(this._tableToModify, this._selectedCell);
		this._checkButtonAvailability();
	},

	_addNewColumn : function()
	{
		var rows = this._tableToModify.rows;
		for (var i=0; i < this._rowsCount; i++)
		{
			var oCell = rows[i].insertCell(-1);
			oCell.innerHTML = "&nbsp;";//TEKI: Avoid 'invisible' cells with no content.
		}
		this._tablePreviewControl._updateTable(this._tableToModify, this._selectedCell);
		this._checkButtonAvailability();
	},

	_addNewRow : function()
	{
		var newRow = this._tableToModify.insertRow(-1);
		for (var i=0; i<this._getMaxColumns(); i++)
		{
			var oCell = newRow.insertCell(-1);
			oCell.innerHTML = "&nbsp;";//TEKI: Avoid 'invisible' cells with no content.
		}
		this._rowsCount++;

		this._tablePreviewControl._updateTable(this._tableToModify, this._selectedCell);
		this._checkButtonAvailability();
	},

	_increaseColSpan : function()
	{
		var cell = this._selectedCell;
		if (!cell) return false;
		
		var nextSibling = this.getNextSiblingCell(cell);
		if (!nextSibling) return false;

		cell.colSpan = this._getColSpan(cell) + this._getColSpan(nextSibling);
		if ("" != nextSibling.innerHTML)
		{
			if ("" != cell.innerHTML)
			{
				cell.innerHTML += "<br>";
			}
			cell.innerHTML += nextSibling.innerHTML;
		}
		nextSibling.parentNode.removeChild(nextSibling);

		this._tablePreviewControl._updateTable(this._tableToModify, this._selectedCell);
		this._checkButtonAvailability();
	},

	_decreaseColSpan : function()
	{
		var row = this._selectedCell.parentNode;
		for (var i=0; i < this._getRowSpan(this._selectedCell); i++)
		{
			try
			{
				row.insertCell(this._selectedCellIndex == 0 ? this._selectedCellIndex + 1 : this._selectedCellIndex);
			}
			catch(ex)
			{//ERJO:RE5-2164
				row.insertCell(0);
			}
			row = this._findNextNamedSibling(row, "TR");
		}
		if (this._getColSpan(this._selectedCell) > 1) this._selectedCell.colSpan = this._getColSpan(this._selectedCell)-1;
		this._tablePreviewControl._updateTable(this._tableToModify, this._selectedCell);
		this._checkButtonAvailability();
	},

	_increaseRowSpan : function()
	{
		var cell = this._selectedCell;
		if (!cell) return false;

		var lowerCell = this.getLowerCell(cell);
		if (!lowerCell) return false;
		if ("" != lowerCell.innerHTML)
		{
			if ("" != cell.innerHTML)
			{
				cell.innerHTML += "<br>";
			}
			cell.innerHTML += lowerCell.innerHTML;
		}
		cell.rowSpan = this._getRowSpan(cell) + this._getRowSpan(lowerCell);
		lowerCell.parentNode.removeChild(lowerCell);

		this._tablePreviewControl._updateTable(this._tableToModify, this._selectedCell);
		this._checkButtonAvailability();
	},

	_decreaseRowSpan : function()
	{
		var row = this._tableToModify.rows[this._getNamedNodeIndex(this._selectedCell.parentNode, "TR") + this._getRowSpan(this._selectedCell) - 1];
		if (this._getRowSpan(this._selectedCell) > 1) this._selectedCell.rowSpan = this._getRowSpan(this._selectedCell)-1;

		for (var i=0; i < this._getColSpan(this._selectedCell); i++)
		{
			row.insertCell(0);
		}
		this._tablePreviewControl._updateTable(this._tableToModify, this._selectedCell);
		this._checkButtonAvailability();
	},

	set_selectedCell : function(value)
	{
		this._selectedCell = value;
		if (this._tableToModify != null)
		{
			this._tablePreviewControl._updateTable(this._tableToModify, this._selectedCell);
		}
	},

	set_tableWizardControl : function(value)
	{
		this._tableWizardDialog = value;
	},

	getParentTable : function(oNode)
	{
		if (!oNode)
			return null;

		while (null != oNode
				&& oNode.parentNode != oNode
				&& "TABLE" != oNode.tagName)
		{
			oNode = oNode.parentNode;
		}

		return (oNode && oNode.tagName == "TABLE" ? oNode : null);
	},
	
	getNextSiblingCell : function(cell)
	{
		if (!cell) return null;

		var row = cell.parentNode;
		var nextIndex = cell.cellIndex + 1;

		if (0 <= nextIndex && nextIndex < row.cells.length)
			return row.cells[nextIndex];

		return null;
	},

	getLowerCell : function(cell)
	{
		if (!cell) return null;

		var table = this.getParentTable(cell);
		var row = cell.parentNode;

		var nextRow = table.rows[row.rowIndex + this._getRowSpan(cell)];
		if (!nextRow) return null;

		var lowerCell = nextRow.cells[cell.cellIndex];
		return lowerCell;
	},

	dispose : function()
	{
		this._clientParameters = null;

		this._selectedCell = null;
		this._victimColumns = [];
		this._victimRows = [];

		this._spinBoxColumns = null;
		this._spinBoxColumnSpan = null;
		this._spinBoxRows = null;
		this._spinBoxRowSpan = null;

		this._tableToModify = null;
		this._tablePreviewControl = null;
		this._tableWizardDialog = null;
		Telerik.Web.UI.Widgets.TableDesign.callBaseMethod(this, "dispose");
	}
}
Telerik.Web.UI.Widgets.TableDesign.registerClass('Telerik.Web.UI.Widgets.TableDesign', Telerik.Web.UI.RadWebControl, Telerik.Web.IParameterConsumer);
</script>
<!-- table design pane -->
<table border="0" cellpadding="0" cellspacing="0" style="margin: 6px;">
	<tr>
		<td style="vertical-align: top; padding: 0;">
			<table border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td style="vertical-align: top; padding: 0;">
						<fieldset style="padding: 0; margin: 0;">
							<legend>Table Columns</legend>
							<table border="0" cellpadding="0" cellspacing="0">
								<tr>
									<td style="vertical-align: top;">
										<div class="propertyLabel" style="width: 80px; text-align: left; margin-left: 8px; margin-bottom: 6px;">
											<script type="text/javascript">
											document.write(localization["Columns"]);
											</script>:
										</div>
									</td>
									<td style="vertical-align: top;">
										<tools:EditorSpinBox id="SpinBoxColumns" VisibleInput="false" runat="server" />
									</td>
									<td style="vertical-align: top;">
										<div class="propertyLabel" style="width: 80px; text-align: left; margin-left: 90px; margin-bottom: 6px;">
											<script type="text/javascript">
											document.write(localization["ColumnSpan"]);
											</script>:
										</div>
									</td>
									<td style="vertical-align: top;">
										<tools:EditorSpinBox id="SpinBoxColumnSpan" VisibleInput="false" runat="server" />
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td style="vertical-align: top; padding: 0;">
						<div id="TableDesign_PreviewTableHolder" style="margin: 0;">
							<!-- / -->
						</div>
					</td>
				</tr>
			</table>
		</td>
		<td style="vertical-align: top; padding: 0; padding-left: 6px;">
			<fieldset style="height: 293px; padding: 0; width: 306px; height: 384px;">
				<legend>Table Rows</legend>
				<table border="0" cellpadding="0" cellspacing="0" style="margin-top: 40px;">
					<tr>
						<td style="padding: 3px 0">
							<div class="propertyLabel" style="text-align: left; margin-left: 6px; width: 60px;">
								<script type="text/javascript">document.write(localization["Rows"]);</script>:
							</div>
						</td>
						<td style="padding: 3px 0">
							<tools:EditorSpinBox id="SpinBoxRows" VisibleInput="false" runat="server" />
						</td>
					</tr>
					<tr>
						<td style="padding: 3px 0">
							<div class="propertyLabel" style="text-align: left; margin-left: 6px; width: 60px;">
								<script type="text/javascript">document.write(localization["RowSpan"]);</script>:
							</div>
						</td>
						<td style="padding: 3px 0">
							<tools:EditorSpinBox id="SpinBoxRowSpan" VisibleInput="false" runat="server" />
						</td>
					</tr>
				</table>
			</fieldset>
		</td>
	</tr>
</table>
<!-- end of table design pane -->