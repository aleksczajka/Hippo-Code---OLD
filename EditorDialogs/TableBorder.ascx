<%@ control language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Editor" TagPrefix="tools" %>
<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");
Telerik.Web.UI.Widgets.TableBorder = function(element)
{
	Telerik.Web.UI.Widgets.TableBorder.initializeBase(this, [element]);
	
	this._clientParameters = null;
	this._colorPickerTableBorder = null;
	this._spinBoxTableBorder = null;
	
	this._previewTable = null;
	this._table = null;
	
	this._allFourSides = null;
	this._allRowsAndColomns = null;
	this._noBorders = null;
	this._noInteriorBorders = null;
	
	this._leftSide = null;
	this._betweenColumns = null;
	this._rightSide = null;
	this._leftAndRightSide = null;
	
	this._topAndBottomSide = null;
	this._topSide = null;
	this._betweenRows = null;
	this._bottomSide = null;	
}

var TABLE_BORDER_CONTROL = null;

Telerik.Web.UI.Widgets.TableBorder.prototype = {
	initialize : function() 
	{
		Telerik.Web.UI.Widgets.TableBorder.callBaseMethod(this, "initialize");
		
		this._initializeChildren();
		this._initializeChildEvents();
	},
	
	_initializeChildren : function()
	{
	    this._spinBoxTableBorder = $find("SpinBoxTableBorder"); 		
		this._spinBoxTableBorder.add_valueSelected(this._changeBorderWidth);
		
		this._previewTable = $get("previewTable");
		
		this._allFourSides = $get("AllFourSides");
	    this._allRowsAndColomns = $get("AllRowsAndColomns");
	    this._noBorders = $get("NoBorders");
	    this._noInteriorBorders = $get("NoInteriorBorders");
    	
	    this._leftSide = $get("LeftSide");
	    this._betweenColumns = $get("BetweenColumns");
	    this._rightSide = $get("RightSide");
	    this._leftAndRightSide = $get("LeftAndRightSide");
    	
	    this._topAndBottomSide = $get("TopAndBottomSide");
	    this._topSide = $get("TopSide");
	    this._betweenRows = $get("BetweenRows");
	    this._bottomSide = $get("BottomSide");	    
		
		TABLE_BORDER_CONTROL = this;		
	},
	
	_initializeChildEvents : function()
	{
	    $addHandlers(this._allFourSides, {"click" : this._allFourSidesHandler}, this);
		$addHandlers(this._allRowsAndColomns, {"click" : this._allRowsAndColomnsHandler}, this);
		$addHandlers(this._noBorders, {"click" : this._noBordersHandler}, this);
		$addHandlers(this._noInteriorBorders, {"click" : this._noInteriorBordersHandler}, this);
		
		$addHandlers(this._leftSide, {"click" : this._leftSideHandler}, this);
		$addHandlers(this._betweenColumns, {"click" : this._betweenColumnsHandler}, this);
		$addHandlers(this._rightSide, {"click" : this._rightSideHandler}, this);
		$addHandlers(this._leftAndRightSide, {"click" : this._leftAndRightSideHandler}, this);
		
		$addHandlers(this._topAndBottomSide, {"click" : this._topAndBottomSideHandler}, this);
		$addHandlers(this._topSide, {"click" : this._topSideHandler}, this);
		$addHandlers(this._betweenRows, {"click" : this._betweenRowsHandler}, this);
		$addHandlers(this._bottomSide, {"click" : this._bottomSideHandler}, this);
	},
	
	clientInit : function(clientParameters)
	{
		this._clientParameters = clientParameters;
				
		this._colorPickerTableBorder = $find("ColorPickerTableBorder");
		this._colorPickerTableBorder.set_items(this._clientParameters.Colors); 
		this._colorPickerTableBorder.add_valueSelected(this._changeBorderColor);
		
		if(!this._table) this._table = this._clientParameters.tableToModify;   
		
		this._loadData();
	},
	
	_allFourSidesHandler : function(e)
	{
	    this._setFrame('border');
	},
	
	_allRowsAndColomnsHandler : function(e)
	{
	    this._setRules('all');
	},
	
	_noBordersHandler : function(e)
	{
	    this._setFrame('void');
	},
	
	_noInteriorBordersHandler : function(e)
	{
	    this._setRules('none');
	},
	
	_leftSideHandler : function(e)
	{
	    this._setFrame('lhs');
	},
	
	_betweenColumnsHandler : function(e)
	{
	    this._setRules('cols');
	},
	
	_rightSideHandler : function(e)
	{
	    this._setFrame('rhs');
	},
	
	_leftAndRightSideHandler : function(e)
	{
	    this._setFrame('vsides');
	},
	
	_topAndBottomSideHandler : function(e)
	{
	    this._setFrame('hsides');
	},
	
	_topSideHandler : function(e)
	{
	    this._setFrame('above');
	},
	
	_betweenRowsHandler : function(e)
	{
	    this._setRules('rows');
	},
	
	_bottomSideHandler : function(e)
	{
	    this._setFrame('below');
	},
	
	_changeBorderWidth : function(sender)
	{
	    TABLE_BORDER_CONTROL._setBorderSize(sender.get_value());
	},
	
	_changeBorderColor : function(sender)
	{
	    TABLE_BORDER_CONTROL._setBorderColor(sender.get_color());
	},
	
	_loadData : function()
	{
	    if (this._table)
	    {
		    //XHTML Compliance
		    var size = parseInt(this._table.border);		
		    if (isNaN(size))
		    {
			    this._previewTable.removeAttribute('border');
			    this._previewTable.className = "tblBorderTestTable";
		    }
		    else
		    {
			    this._previewTable.border = size;
			    this._previewTable.className = "tblBorderTestTableWithBorder";
		    }
	    }	

	    this._previewTable.rules = this._table ? this._table.rules : "all";
	    this._previewTable.frame = this._table ? this._table.frame : "border";
    	
	    //Moz problem with setting color directly
	    //this.PreviewTable.borderColor = this.TargetTable ? this.TargetTable.borderColor : "";
	    var bColor = (this._table ? this._table.getAttribute("borderColor") :"");
	    if (!bColor) bColor = "";//null or empty
	    this._previewTable.setAttribute("borderColor", bColor);

	    this._spinBoxTableBorder.set_value(this._previewTable.border);
	    this._colorPickerTableBorder.set_color(this._previewTable.getAttribute("borderColor"));
	},
	
	_updateTarget : function()
    {
	    if (null != this._table && null != this._previewTable)
	    {
		    var frame = this._getFrame();
		    if (frame)
		    {
			    this._table.frame = frame;
		    }
		    else
		    {
			    this._table.removeAttribute('frame');
		    }
		    var rules = this._getRules();
		    if (rules)
		    {
			    this._table.rules = rules;
		    }
		    else
		    {
			    this._table.removeAttribute('rules');
		    }
		    //XHTML Compliance
		    if (this._previewTable.border != "")
		    {
			    this._table.border = this._previewTable.border;
		    }
		    else
		    {
			    this._table.removeAttribute('border');
		    }
    		
		    //this.TargetTable.borderColor = this.PreviewTable.borderColor;		
		    var bColor = this._previewTable.getAttribute("borderColor");
		    if (!bColor) bColor = "";
		    this._table.setAttribute("borderColor", bColor);//In Moz does not recognize it as valid attribute.  
		    
		    return this._table;  		
	    }
	    return null;
    },
	
	_setBorderSize : function(size)
    {
	    if (null != this._previewTable)
	    {
		    if (size < 0)
		    {
			    this._previewTable.removeAttribute('border');
		    }
		    else
		    {
			    if (size > 1000)
			    {
				    //alert(localization["BORDER_SIZE_OVERFLOW"]);
				    size = 1000; // max border width
			    }

			    this._previewTable.border = size;
		    }

		    this._previewTable.className = (this._previewTable.border > 0) ?
			    "tblBorderTestTableWithBorder" :
			    "tblBorderTestTable";
	    }
    },
    
    _setBorderColor : function(color)
    {	
	    if (null != this._previewTable)
	    {
		    if ("" != color
			    && (!this._previewTable.border || 0 == parseInt(this._previewTable.border)))
		    {
			    this._spinBoxTableBorder.set_value("1");
			    this._changeBorderWidth(this._spinBoxTableBorder);
		    }

		    //this.PreviewTable.borderColor = color; //In Moz does not recognize it as valid attribute.
		    this._previewTable.setAttribute("borderColor", color);
	    }	
    },
    
    _getFrame : function()
    {
	    if (null != this._previewTable
		    && 'border' != this._previewTable.frame // Moz fix
		    && 'box' != this._previewTable.frame)
	    {
		    return this._previewTable.frame;
	    }
	    else
	    {
		    return '';
	    }
    },
    
    _getRules : function()
    {
	    if (null != this._previewTable
		    && 'all' != this._previewTable.rules)// Moz fix
	    {
		    return this._previewTable.rules;
	    }
	    else
	    {
		    return '';
	    }
    },
    
    _getBorderSize : function()
    {
	    if (null != this._previewTable)
	    {
		    return this._previewTable.border;
	    }
	    else
	    {
		    return 0;
	    }
    },
    
    _setFrame : function(frame)
    {
	    if (null != this._previewTable)
	    {
		    if ("void" != frame
				    && "" != frame
				    && 0 == parseInt(this._previewTable.border))
		    {
			    this._spinBoxTableBorder.set_value("1");
			    this._changeBorderWidth(this._spinBoxTableBorder);
		    }

		    this._previewTable.frame = frame;
	    }
    },
    
    _setRules : function(rule)
    {
	    if (null != this._previewTable)
	    {
		    if ("none" != rule
				    && "" != rule
				    && 0 == parseInt(this._previewTable.border))
		    {
			    this._spinBoxTableBorder.set_value("1");
			    this._changeBorderWidth(this._spinBoxTableBorder);
		    }

		    this._previewTable.rules = rule;
	    }
    },
    
    set_tableToModify : function(value)
    {
        this._table = value;
    },
    
    _disposeChildEvents : function()
    {
        $clearHandlers(this._allFourSides);
		$clearHandlers(this._allRowsAndColomns);
		$clearHandlers(this._noBorders);
		$clearHandlers(this._noInteriorBorders);
		
		$clearHandlers(this._leftSide);
		$clearHandlers(this._betweenColumns);
		$clearHandlers(this._rightSide);
		$clearHandlers(this._leftAndRightSide);
		
		$clearHandlers(this._topAndBottomSide);
		$clearHandlers(this._topSide);
		$clearHandlers(this._betweenRows);
		$clearHandlers(this._bottomSide);
    },
	
	dispose : function() 
	{	
	    this._disposeChildEvents();
		
	    this._clientParameters = null;
	    this._colorPickerTableBorder = null;
	    this._spinBoxTableBorder = null;
	    
	    this._previewTable = null;
	    this._table = null;
	    
	    this._allFourSides = null;
	    this._allRowsAndColomns = null;
	    this._noBorders = null;
	    this._noInteriorBorders = null;
    	
	    this._leftSide = null;
	    this._betweenColumns = null;
	    this._rightSide = null;
	    this._leftAndRightSide = null;
    	
	    this._topAndBottomSide = null;
	    this._topSide = null;
	    this._betweenRows = null;
	    this._bottomSide = null;
	    
	    TABLE_BORDER_CONTROL = null;
	        
		Telerik.Web.UI.Widgets.TableBorder.callBaseMethod(this, "dispose");
	}
}

Telerik.Web.UI.Widgets.TableBorder.registerClass('Telerik.Web.UI.Widgets.TableBorder', Telerik.Web.UI.RadWebControl, Telerik.Web.IParameterConsumer);
</script>
<div style="height: 124px;">
	<table border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td>
				<!-- preview table -->
				<table cellpadding="0" cellspacing="0" class="tblBorderTestTable" ID="previewTable">
					<tr>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
				</table>
				<!-- end of preview table -->
			</td>
		</tr>
	</table>
</div>
<table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <div class="propertiesLabel">
                    <script type="text/javascript">
	                document.write(localization["BorderColor"]);
	                </script>
	            </div>
            </td>
          </tr>
          <tr>
            <td style="padding-bottom: 6px;">
                <tools:ColorPicker id="ColorPickerTableBorder" runat="server"></tools:ColorPicker>
            </td>
        </tr>
        <tr>
            <td>
                <div class="propertiesLabel">
                    <script type="text/javascript">
		            document.write(localization["BorderWidth"]);
		            </script>
	            </div>
            </td>
           </tr>
           <tr>
            <td style="padding-bottom: 6px;">
                <tools:EditorSpinBox id="SpinBoxTableBorder" runat="server"></tools:EditorSpinBox>
            </td>
        </tr>
     </table>
     <style type="text/css">
     .breakLabel
     {
		border-bottom: solid 1px #e5e5e5;
		display: block;
		margin-bottom: 2px;
		padding-bottom: 2px;
     }
     </style>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <ul>
                <li class="customControlContainer" style="clear: both; margin-bottom: 6px;">
		            <label class="breakLabel">Table:</label>
		            <ul class="tblBorderPropsToolbar">
			            <li><a href="javascript:void(0);" id="AllFourSides" title="All Four Sides" class="rade_AllFourSides">&nbsp;</a></li>
			            <li><a href="javascript:void(0);" id="AllRowsAndColomns" title="All Rows and Columns" class="rade_AllRowsAndColumns">&nbsp;</a></li>
			            <li><a href="javascript:void(0);" id="NoBorders" title="No Borders" class="rade_NoBorders">&nbsp;</a></li>
			            <li><a href="javascript:void(0);" id="NoInteriorBorders" title="No Interior Borders" class="rade_NoInteriorBorders">&nbsp;</a></li>
		            </ul>
	            </li>
	            <li class="customControlContainer rade_verticalIconList" style="clear: both; margin-bottom: 6px;">
		            <label class="breakLabel">Vertical:</label>
		            <ul class="tblBorderPropsToolbar">
			            <li><a href="javascript:void(0);" id="LeftAndRightSide" title="No Interior Borders" class="rade_RightAndLeftSidesOnly">&nbsp;</a></li>
			            <li><a href="javascript:void(0);" id="LeftSide" title="Left Side" class="rade_LeftSide">&nbsp;</a></li>
			            <li><a href="javascript:void(0);" id="BetweenColumns" title="Between Columns" class="rade_BetweenColumns">&nbsp;</a></li>
			            <li><a href="javascript:void(0);" id="RightSide" title="No Borders" class="rade_RightSide">&nbsp;</a></li>
		            </ul>
	            </li>
	            <li class="customControlContainer" style="clear: both; margin-bottom: 6px;">
		            <label class="breakLabel">Horizontal:</label>
		            <ul class="tblBorderPropsToolbar">
			            <li><a href="javascript:void(0);" id="TopAndBottomSide" title="Top and Bottom Sides Only" class="rade_TopAndBottomSidesOnly">&nbsp;</a></li>
			            <li><a href="javascript:void(0);" id="TopSide" title="Top Side Only" class="rade_TopSideOnly">&nbsp;</a></li>
			            <li><a href="javascript:void(0);" id="BetweenRows" title="Between Rows" class="rade_BetweenRows">&nbsp;</a></li>
			            <li><a href="javascript:void(0);" id="BottomSide" title="Bottom Side Only" class="rade_BottomSideOnly">&nbsp;</a></li>
		            </ul>
	            </li>
            </ul>
        </td>
    </tr>
</table>
<script type="text/javascript">
localization.setTitle("AllFourSides", "AllFourSides");
localization.setTitle("AllRowsAndColomns", "AllRowsAndColomns");
localization.setTitle("NoBorders", "NoBorders");
localization.setTitle("NoInteriorBorders", "NoInteriorBorders");

localization.setTitle("LeftSide", "LeftSide");
localization.setTitle("BetweenColumns", "BetweenColumns");
localization.setTitle("RightSide", "RightSide");
localization.setTitle("LeftAndRightSide", "LeftAndRightSide");

localization.setTitle("TopAndBottomSide", "TopAndBottomSide");
localization.setTitle("TopSide", "TopSide");
localization.setTitle("BetweenRows", "BetweenRows");
localization.setTitle("BottomSide", "BottomSide");
</script>