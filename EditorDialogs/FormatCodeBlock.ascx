<%@ Control Language="C#" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI.Dialogs" TagPrefix="dialogs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<dialogs:UserControlResources id="dialogResources" runat="server" />

<script type="text/javascript">
Type.registerNamespace("Telerik.Web.UI.Widgets");
Type.registerNamespace("Telerik.Web.UI.Widgets.Highlighter");

Telerik.Web.UI.Widgets.Highlighter.Match = function(value, index, cssStyle)
{
	this._value = value;
	this._index = index;
	this._length = value.length;
	this._cssStyle = cssStyle;
}
Telerik.Web.UI.Widgets.Highlighter.Match.registerClass('Telerik.Web.UI.Widgets.Highlighter.Match');

Telerik.Web.UI.Widgets.Highlighter.Brush = function()
{
	this._showLineNumbers = true;
	this._tabsToSpaces = true;
}

// gets a list of all matches for a given regular expression
Telerik.Web.UI.Widgets.Highlighter.Brush.prototype =
{
/* Properties */
	get_showLineNumbers : function()
	{
		return this._showLineNumbers;
	},
	
	set_showLineNumbers : function(value)
	{
		this._showLineNumbers = value;
	},

	get_tabsToSpaces : function()
	{
		return this._tabsToSpaces;
	},
	
	set_tabsToSpaces : function(value)
	{
		this._tabsToSpaces = value;
	},

/* End Properties */

/* Public methods */
	highlight : function(code)
	{
		var trim = function(str)
		{
			return str.replace(/^\s*(.*?)[\s\n]*$/g, '$1');
		}
		
		var chop = function(str)
		{
			return str.replace(/\n*$/, '').replace(/^\n*/, '');
		}

		var unindent = function(str)
		{
			var lines = str.split('\n');
			var indents = new Array();
			var regex = new RegExp('^\\s*', 'g');
			var min = 1000;

			// go through every line and check for common number of indents
			for (var i=0; i<lines.length && min>0; i++)
			{
				if (trim(lines[i]).length == 0)
					continue;

				var matches = regex.exec(lines[i]);

				if (matches != null && matches.length > 0)
					min = Math.min(matches[0].length, min);
			}

			// trim minimum common number of white space from the begining of every line
			if (min > 0)
				for (var i=0; i<lines.length; i++)
				{
					lines[i] = lines[i].substr(min);
				}

			return lines.join('\n');
		}
		
		// This function returns a portions of the string from pos1 to pos2 inclusive
		var copy = function(string, pos1, pos2)
		{
			return string.substr(pos1, pos2 - pos1);
		}

		var pos	= 0;

		this._originalCode = code;
//		this._code = chop(unindent(code));
		this._code = code;
		this._container = document.createElement('DIV');
		this._matches = new Array();

		// replace tabs with spaces
		if (this.get_tabsToSpaces() == true)
		{
			this._code = this._processSmartTabs(this._code);
		}

		this._processRegexList();	

		// if no matches found, add entire code as plain text
		if (this._matches.length == 0)
		{
			this._formatChunk(this._code, null);
			return this._switchToTable();
		}

		// sort the matches
		// static callback for the match sorting
		var sortCallback = function(m1, m2)
		{
			// sort matches by index first
			if (m1._index < m2._index)
			{
				return -1;
			}
			else if (m1._index > m2._index)
			{
				return 1;
			}
			else
			{
				// if index is the same, sort by length
				if (m1._length < m2._length)
				{
					return -1;
				}
				else if (m1._length > m2._length)
				{
					return 1;
				}
			}
			return 0;
		}

		// sort it in order to make the check for _isInside faster
		this._matches = this._matches.sort(sortCallback);

		// The following loop checks to see if any of the matches are inside
		// of other matches. This process would get rid of highligting strings
		// inside comments, keywords inside strings and so on.
		for (var i=0; i<this._matches.length; i++)
		{
			if (this._isInside(this._matches[i]))
			{
				this._matches[i] = null;
			}
		}

		// Finally, go through the final list of matches and pull the all
		// together adding everything in between that isn't a match.
		for (var i=0; i<this._matches.length; i++)
		{
			var match = this._matches[i];

			if (match == null || match._length == 0)
			{
				continue;
			}

			this._formatChunk(copy(this._code, pos, match._index), null);
			this._formatChunk(match._value, match._cssStyle);

			pos = match._index + match._length;
		}
		
		this._formatChunk(this._code.substr(pos), null);

		return this._switchToTable();
	},

/* End Public methods */
	_getMatches : function(regex, cssStyle)
	{
		var index = 0;
		var match = null;

		while((match = regex.exec(this._code)) != null)
		{
			this._matches[this._matches.length] = new Telerik.Web.UI.Widgets.Highlighter.Match(match[0], match.index, cssStyle);
		}
	},

	_formatChunk : function(str, cssStyle)
	{
		var font = document.createElement('font');
		
		str = str.replace(/&/g, '&amp;');
		str = str.replace(/ /g, '&nbsp;');
		str = str.replace(/</g, '&lt;');
		str = str.replace(/\n/gm, '&nbsp;<br>');

		// when adding a piece of code, check to see if it has line breaks in it 
		// and if it does, wrap individual line breaks with font tags
		if (cssStyle != null)
		{
			var regex = new RegExp('<br>', 'gi');

			if (regex.test(str))
			{
				var lines = str.split('&nbsp;<br>');

				str = '';
				for (var i=0; i<lines.length; i++)
				{
					this._container.innerHTML += String.format('<font style = "{0}">{1}</font>', cssStyle, lines[i]);

					// don't add a <BR> for the last line
					if (i+1 < lines.length)
					{
						this._container.appendChild(document.createElement('BR'));
					}
				}
			}
			else
			{
				font.style.cssText = cssStyle;
				font.innerHTML = str;
				this._container.appendChild(font);
			}
		}
		else
		{
			font.style.cssText = 'font-size: 11px';
			font.innerHTML = str;		
			this._container.appendChild(font);
		}
	},

	// checks if one match is inside any other match
	_isInside : function(match)
	{
		if (match == null || match._length == 0)
			return;

		for (var i=0; i<this._matches.length; i++)
		{
			var currentMatch = this._matches[i];

			if (currentMatch == null)
				continue;

			if ((match._index > currentMatch._index) && (match._index <= currentMatch._index + currentMatch._length))
				return true;
		}

		return false;
	},

	_processRegexList : function()
	{
		for (var i=0; i<this._regexList.length; i++)
		{
			this._getMatches(this._regexList[i].regex, this._regexList[i].cssStyle);
		}
	},

	_processSmartTabs : function(code)
	{
		var lines = code.split('\n');
		var result = '';
		var tabSize = 4;
		var tab = '\t';

		// This function inserts specified amount of spaces in the string
		// where a tab is while removing that given tab. 
		var insertSpaces = function(line, pos, count)
		{
			var left = line.substr(0, pos);
			var right = line.substr(pos + 1, line.length);	// pos + 1 will get rid of the tab
			var spaces = '';

			for (var i=0; i<count; i++)
				spaces += ' ';

			return left + spaces + right;
		}

		// This function process one line for 'smart tabs'
		var processLine = function(line, tabSize)
		{
			if (line.indexOf(tab) == -1)
				return line;

			var pos = 0;

			while ((pos = line.indexOf(tab)) != -1)
			{
				// This is pretty much all there is to the 'smart tabs' logic.
				// Based on the position within the line and size of a tab,
				// calculate the amount of spaces we need to insert.
				var spaces = tabSize - pos % tabSize;

				line = insertSpaces(line, pos, spaces);
			}

			return line;
		}

		// Go through all the lines and do the 'smart tabs' magic.
		for (var i=0; i<lines.length; i++)
			result += processLine(lines[i], tabSize) + '\n';

		return result;
	},

	_switchToTable : function()
	{
		// thanks to Lachlan Donald from SitePoint.com for this <br/> tag fix.
		var html = this._container.innerHTML.replace(/<(br)\/?>/gi, '\n');
		var lines = html.split('\n');

		var arrhtml = [];

		arrhtml.push('<table cellpadding=0 cellspacing=0 style="width: 99%;	margin: 2px 0px 2px 0px;border-collapse: collapse;border-bottom: 2px solid #eee;background-color: #fff;	border-width:0px;">');
		if (this.get_showLineNumbers())
		{
			arrhtml.push('<col style="font-family: Courier New;font-size: 11px;background-color: #eee;padding-right: 5px; padding-left: 10px; width: 5px; border-right: 1px solid gray; color: gray;text-align: right;vertical-align: top;"/>');
		}

		arrhtml.push('<col style="font-family: Courier New;font-size: 11px;padding-left: 10px;border-bottom: 1px solid #F7F7F7;white-space:nowrap;"/>');

		for (var i=0, lineIndex=1; i<lines.length-1; i++, lineIndex++)
		{
			arrhtml.push('<tr>');

			if (this.get_showLineNumbers())
			{
				arrhtml.push('<td><nobr>' + lineIndex + '</nobr></td>');
			}

			var styleAddon = '';
			if ((i%2 + 1) == 2)
			{
				styleAddon = ' style="background-color: #F7F7F7;"';
			}

			arrhtml.push(String.format("<td {0}>{1}</td>", styleAddon, lines[i]));
			arrhtml.push('</tr>');
		}

		arrhtml.push('</table>');

		return arrhtml.join('');
	},

	_getKeywords : function(str)
	{
		return '\\b' + str.replace(/ /g, '\\b|\\b') + '\\b';
	}
}
Telerik.Web.UI.Widgets.Highlighter.Brush.registerClass('Telerik.Web.UI.Widgets.Highlighter.Brush');

//***********************************************************************************
//                                   the Brushes
//***********************************************************************************
Telerik.Web.UI.Widgets.Highlighter.CSharpBrush = function()
{
	Telerik.Web.UI.Widgets.Highlighter.CSharpBrush.initializeBase(this);
	var keywords =	'abstract as base bool break byte case catch char checked class const ' +
					'continue decimal default delegate do double else enum event explicit ' +
					'extern false finally fixed float for foreach get goto if implicit in int ' +
					'interface internal is lock long namespace new null object operator out ' +
					'override params private protected public readonly ref return sbyte sealed set ' +
					'short sizeof stackalloc static string struct switch this throw true try ' +
					'typeof uint ulong unchecked unsafe ushort using virtual void while';

	this._regexList = [
		// There's a slight problem with matching single line comments and figuring out
		// a difference between // and ///. Using lookahead and lookbehind solves the
		// problem, unfortunately JavaScript doesn't support lookbehind. So I'm at a 
		// loss how to translate that regular expression to JavaScript compatible one.
//		{ regex: new RegExp('(?<!/)//(?!/).*$|(?<!/)////(?!/).*$|/\\*[^\\*]*(.)*?\\*/', 'gm'),	cssStyle: 'color: green;' },			// one line comments starting with anything BUT '///' and multiline comments
//		{ regex: new RegExp('(?<!/)///(?!/).*$', 'gm'),											cssStyle: 'color: green;' },		// XML comments starting with ///

		{ regex: new RegExp('//.*$', 'gm'), cssStyle: 'color: green;' },			// one line comments
		{ regex: new RegExp('/\\*[\\s\\S]*?\\*/', 'g'), cssStyle: 'color: green;' },			// multiline comments
		{ regex: new RegExp('"(?:\\.|[^\\""])*"', 'g'),	 cssStyle: 'color: blue;' },			// strings
		{ regex: new RegExp('^\\s*#.*', 'gm'), cssStyle: 'color: gray;' },		// preprocessor tags like #region and #endregion
		{ regex: new RegExp(this._getKeywords(keywords), 'gm'), cssStyle: 'color: blue;' }			// c# keyword
		];
}

Telerik.Web.UI.Widgets.Highlighter.CSharpBrush.registerClass('Telerik.Web.UI.Widgets.Highlighter.CSharpBrush', Telerik.Web.UI.Widgets.Highlighter.Brush);

/************************************************* DEPLHI ****************************************/
/* Delphi brush is contributed by Eddie Shipman */
Telerik.Web.UI.Widgets.Highlighter.DelphiBrush = function()
{
	Telerik.Web.UI.Widgets.Highlighter.DelphiBrush.initializeBase(this);
	var keywords =	'abs addr and ansichar ansistring array as asm begin boolean byte cardinal ' +
					'case char class comp const constructor currency destructor div do double ' +
					'downto else end except exports extended false file finalization finally ' +
					'for function goto if implementation in inherited int64 initialization ' +
					'integer interface is label library longint longword mod nil not object ' +
					'of on or packed pansichar pansistring pchar pcurrency pdatetime pextended ' + 
					'pint64 pointer private procedure program property pshortstring pstring ' + 
					'pvariant pwidechar pwidestring protected public published raise real real48 ' +
					'record repeat set shl shortint shortstring shr single smallint string then ' +
					'threadvar to true try type unit until uses val var varirnt while widechar ' +
					'widestring with word write writeln xor';

	this._regexList = [
		{ regex: new RegExp('\\(\\*[\\s\\S]*?\\*\\)', 'gm'), cssStyle: 'color: #008200; font-style: italic;' },  			// multiline comments (* *)
		{ regex: new RegExp('{(?!\\$)[\\s\\S]*?}', 'gm'), cssStyle: 'color: #008200; font-style: italic;' },  			// multiline comments { }
		{ regex: new RegExp('//.*$', 'gm'), cssStyle: 'color: #008200; font-style: italic;' },  			// one line
		{ regex: new RegExp('\'(?:\\.|[^\\\'\'])*\'', 'g'), cssStyle: 'color: blue;' },			// strings
		{ regex: new RegExp('\\{\\$[a-zA-Z]+ .+\\}', 'g'), cssStyle: 'color: #008284;' },			// Compiler Directives and Region tags
		{ regex: new RegExp('\\b[\\d\\.]+\\b', 'g'), cssStyle: 'color: blue;' },			// numbers 12345
		{ regex: new RegExp('\\$[a-zA-Z0-9]+\\b', 'g'), cssStyle: 'color: blue;' },			// numbers $F5D3
		{ regex: new RegExp(this._getKeywords(keywords), 'gm'), cssStyle: 'font-weight: bold; color: navy;' }			// keyword
		];

}
Telerik.Web.UI.Widgets.Highlighter.DelphiBrush.registerClass('Telerik.Web.UI.Widgets.Highlighter.DelphiBrush', Telerik.Web.UI.Widgets.Highlighter.Brush);

/**************************************************** Javascript ***********************************************/
Telerik.Web.UI.Widgets.Highlighter.JScriptBrush = function()
{
	Telerik.Web.UI.Widgets.Highlighter.JScriptBrush.initializeBase(this);
	var keywords =	'abstract boolean break byte case catch char class const continue debugger innerHTML className document window getElementById' +
					'default delete do double else enum export extends false final finally float ' +
					'for function goto if implements import in instanceof int interface long native ' +
					'new null package private protected public return short static super switch ' +
					'synchronized this throw throws transient true try typeof var void volatile while with';

	this._regexList = [
		{ regex: new RegExp('//.*$', 'gm'), cssStyle: 'color: green;' },			// one line comments
		{ regex: new RegExp('/\\*[\\s\\S]*?\\*/', 'g'), cssStyle: 'color: green;' },			// multiline comments
		{ regex: new RegExp('"(?:[^"\n]|[\"])*?"', 'g'), cssStyle: 'color: blue;' },			// double quoted strings
		{ regex: new RegExp("'(?:[^'\n]|[\'])*?'", 'g'), cssStyle: 'color: blue;' },			// single quoted strings
		{ regex: new RegExp('^\\s*#.*', 'gm'), cssStyle: 'color: gray;' },		// preprocessor tags like #region and #endregion
		{ regex: new RegExp(this._getKeywords(keywords), 'gm'), cssStyle: 'color: blue;' }			// keywords
		];
}

Telerik.Web.UI.Widgets.Highlighter.JScriptBrush.registerClass('Telerik.Web.UI.Widgets.Highlighter.JScriptBrush', Telerik.Web.UI.Widgets.Highlighter.Brush);

/**************************************************** CSS ***********************************************/
Telerik.Web.UI.Widgets.Highlighter.CSSBrush = function()
{
	Telerik.Web.UI.Widgets.Highlighter.CSSBrush.initializeBase(this);
	var keywords =	'ascent azimuth background-attachment background-color background-image background-position ' +
					'background-repeat background baseline bbox border-collapse border-color border-spacing border-style border-top ' +
					'border-right border-bottom border-left border-top-color border-right-color border-bottom-color border-left-color ' +
					'border-top-style border-right-style border-bottom-style border-left-style border-top-width border-right-width ' +
					'border-bottom-width border-left-width border-width border bottom cap-height caption-side centerline clear clip color ' +
					'content counter-increment counter-reset cue-after cue-before cue cursor definition-src descent direction display ' +
					'elevation empty-cells float font-size-adjust font-family font-size font-stretch font-style font-variant font-weight font ' +
					'height letter-spacing line-height list-style-image list-style-position list-style-type list-style margin-top ' +
					'margin-right margin-bottom margin-left margin marker-offset marks mathline max-height max-width min-height min-width orphans ' +
					'outline-color outline-style outline-width outline overflow padding-top padding-right padding-bottom padding-left padding page ' +
					'page-break-after page-break-before page-break-inside pause pause-after pause-before pitch pitch-range play-during position ' +
					'quotes richness right size slope src speak-header speak-numeral speak-punctuation speak speech-rate stemh stemv stress ' +
					'table-layout text-align text-decoration text-indent text-shadow text-transform unicode-bidi unicode-range units-per-em ' +
					'vertical-align visibility voice-family volume white-space widows width widths word-spacing x-height z-index';
					
	var values =	'above absolute all always aqua armenian attr aural auto avoid baseline behind below bidi-override black blink block blue bold bolder '+
					'both bottom braille capitalize caption center center-left center-right circle close-quote code collapse compact condensed '+
					'continuous counter counters crop cross crosshair cursive dashed decimal decimal-leading-zero default digits disc dotted double '+
					'embed embossed e-resize expanded extra-condensed extra-expanded fantasy far-left far-right fast faster fixed format fuchsia '+
					'gray green groove handheld hebrew help hidden hide high higher icon inline-table inline inset inside invert italic '+
					'justify landscape large larger left-side left leftwards level lighter lime line-through list-item local loud lower-alpha '+
					'lowercase lower-greek lower-latin lower-roman lower low ltr marker maroon medium message-box middle mix move narrower '+
					'navy ne-resize no-close-quote none no-open-quote no-repeat normal nowrap n-resize nw-resize oblique olive once open-quote outset '+
					'outside overline pointer portrait pre print projection purple red relative repeat repeat-x repeat-y rgb ridge right right-side '+
					'rightwards rtl run-in screen scroll semi-condensed semi-expanded separate se-resize show silent silver slower slow '+
					'small small-caps small-caption smaller soft solid speech spell-out square s-resize static status-bar sub super sw-resize '+
					'table-caption table-cell table-column table-column-group table-footer-group table-header-group table-row table-row-group teal '+
					'text-bottom text-top thick thin top transparent tty tv ultra-condensed ultra-expanded underline upper-alpha uppercase upper-latin '+
					'upper-roman url visible wait white wider w-resize x-fast x-high x-large x-loud x-low x-slow x-small x-soft xx-large xx-small yellow';
	
	var fonts =		'[mM]onospace [tT]ahoma [vV]erdana [aA]rial [hH]elvetica [sS]ans-serif [sS]erif';
	
	this._regexList = [
		{ regex: new RegExp('/\\*[\\s\\S]*?\\*/', 'g'), cssStyle: 'color: green;' },// multiline comments
		{ regex: new RegExp('"(?:[^"\n]|[\"])*?"', 'g'), cssStyle: 'color: blue;' },	// double quoted strings
		{ regex: new RegExp("'(?:[^'\n]|[\'])*?'", 'g'), cssStyle: 'color: blue;' },	// single quoted strings
		{ regex: new RegExp('\\#[a-zA-Z0-9]{3,6}', 'g'), cssStyle: 'color: blue;' },	// html colors
		{ regex: new RegExp('(\\d+)(px|pt|\:)', 'g'), cssStyle: 'color: blue;' },	// size specifications
		{ regex: new RegExp(this._getKeywords(keywords), 'gm'), cssStyle: 'color: red;' },	// keywords
		{ regex: new RegExp(this._getKeywords(values), 'g'), cssStyle: 'color: blue;' },	// values
		{ regex: new RegExp(this._getKeywords(fonts), 'g'), cssStyle: 'color: blue;' }	// fonts		
		];
}

Telerik.Web.UI.Widgets.Highlighter.CSSBrush.registerClass('Telerik.Web.UI.Widgets.Highlighter.CSSBrush', Telerik.Web.UI.Widgets.Highlighter.Brush);

/********************************************************* PHP ***************************************************/
Telerik.Web.UI.Widgets.Highlighter.PhpBrush = function()
{
	Telerik.Web.UI.Widgets.Highlighter.PhpBrush.initializeBase(this);
	var keywords =	'and or xor __FILE__ __LINE__ array as break case ' +
					'cfunction class const continue declare default die do echo else ' +
					'elseif empty enddeclare endfor endforeach endif endswitch endwhile eval exit ' +
					'extends for foreach function global if include include_once isset list ' +
					'new old_function print require require_once return static switch unset use ' +
					'var while __FUNCTION__ __CLASS__';

	this._regexList = [
		{ regex: new RegExp('//.*$', 'gm'), cssStyle: 'color: green;' },			// one line comments
		{ regex: new RegExp('/\\*[\\s\\S]*?\\*/', 'g'), cssStyle: 'color: green;' },			// multiline comments
		{ regex: new RegExp('"(?:[^"\n]|[\"])*?"', 'g'), cssStyle: 'color: blue;' },			// double quoted strings
		{ regex: new RegExp("'(?:[^'\n]|[\'])*?'", 'g'), cssStyle: 'color: blue;' },			// single quoted strings
		{ regex: new RegExp('\\$\\w+', 'g'), cssStyle: 'color: #d00;' },				// variables
		{ regex: new RegExp(this._getKeywords(keywords), 'gm'), cssStyle: 'color: blue;' }			// keyword
		];

}
Telerik.Web.UI.Widgets.Highlighter.PhpBrush.registerClass('Telerik.Web.UI.Widgets.Highlighter.PhpBrush', Telerik.Web.UI.Widgets.Highlighter.Brush);

/************************************************************** Python **********************************************/
/* Python 2.3 syntax contributed by Gheorghe Milas */
Telerik.Web.UI.Widgets.Highlighter.PythonBrush = function()
{
	Telerik.Web.UI.Widgets.Highlighter.PythonBrush.initializeBase(this);
	var keywords =		'and assert break class continue def del elif else except exec ' +
						'finally for from global if import in is lambda not or object pass print ' +
						'raise return try yield while';
	
	var builtins =		'self __builtin__ __dict__ __future__ __methods__ __members__ __author__ __email__ __version__' +
						'__class__ __bases__ __import__ __main__ __name__ __doc__ __self__ __debug__ __slots__ ' +
						'abs append apply basestring bool buffer callable chr classmethod clear close cmp coerce compile complex ' +
						'conjugate copy count delattr dict dir divmod enumerate Ellipsis eval execfile extend False file fileno filter float flush ' +
						'get getattr globals has_key hasarttr hash hex id index input insert int intern isatty isinstance isubclass ' +
						'items iter keys len list locals long map max min mode oct open ord pop pow property range ' +
						'raw_input read readline readlines reduce reload remove repr reverse round seek setattr slice sum ' +
						'staticmethod str super tell True truncate tuple type unichr unicode update values write writelines xrange zip';
	
	var magicmethods =	'__abs__ __add__ __and__ __call__ __cmp__ __coerce__ __complex__ __concat__ __contains__ __del__ __delattr__ __delitem__ ' +
						'__delslice__ __div__ __divmod__ __float__ __getattr__ __getitem__ __getslice__ __hash__ __hex__ __eq__ __le__ __lt__ __gt__ __ge__ ' +
						'__iadd__ __isub__ __imod__ __idiv__ __ipow__ __iand__ __ior__ __ixor__ __ilshift__ __irshift__ ' +
						'__invert__ __init__ __int__ __inv__ __iter__ __len__ __long__ __lshift__ __mod__ __mul__ __new__ __neg__ __nonzero__ __oct__ __or__ ' +
						'__pos__ __pow__ __radd__ __rand__ __rcmp__ __rdiv__ __rdivmod__ __repeat__ __repr__ __rlshift__ __rmod__ __rmul__ ' +
						'__ror__ __rpow__ __rrshift__ __rshift__ __rsub__ __rxor__ __setattr__ __setitem__ __setslice__ __str__ __sub__ __xor__';
	
	var exceptions =	'Exception StandardError ArithmeticError LookupError EnvironmentError AssertionError AttributeError EOFError ' +
						'FutureWarning IndentationError OverflowWarning PendingDeprecationWarning ReferenceError RuntimeWarning ' +
						'SyntaxWarning TabError UnicodeDecodeError UnicodeEncodeError UnicodeTranslateError UserWarning Warning ' +
						'IOError ImportError IndexError KeyError KeyboardInterrupt MemoryError NameError NotImplementedError OSError ' +
						'RuntimeError StopIteration SyntaxError SystemError SystemExit TypeError UnboundLocalError UnicodeError ValueError ' +
						'FloatingPointError OverflowError WindowsError ZeroDivisionError';
	
	var types =			'NoneType TypeType IntType LongType FloatType ComplexType StringType UnicodeType BufferType TupleType ListType ' +
						'DictType FunctionType LambdaType CodeType ClassType UnboundMethodType InstanceType MethodType BuiltinFunctionType BuiltinMethodType ' +
						'ModuleType FileType XRangeType TracebackType FrameType SliceType EllipsisType';
	
	var commonlibs =	'anydbm array asynchat asyncore AST base64 binascii binhex bisect bsddb buildtools bz2 ' +
						'BaseHTTPServer Bastion calendar cgi cmath cmd codecs codeop commands compiler copy copy_reg ' +
						'cPickle crypt cStringIO csv curses Carbon CGIHTTPServer ConfigParser Cookie datetime dbhash ' +
						'dbm difflib dircache distutils doctest DocXMLRPCServer email encodings errno exceptions fcntl ' +
						'filecmp fileinput ftplib gc gdbm getopt getpass glob gopherlib gzip heapq htmlentitydefs ' +
						'htmllib httplib HTMLParser imageop imaplib imgfile imghdr imp inspect itertools jpeg keyword ' +
						'linecache locale logging mailbox mailcap marshal math md5 mhlib mimetools mimetypes mimify mmap ' +
						'mpz multifile mutex MimeWriter netrc new nis nntplib nsremote operator optparse os parser pickle pipes ' +
						'popen2 poplib posix posixfile pprint preferences profile pstats pwd pydoc pythonprefs quietconsole ' +
						'quopri Queue random re readline resource rexec rfc822 rgbimg sched select sets sgmllib sha shelve shutil ' +
						'signal site smtplib socket stat statcache string struct symbol sys syslog SimpleHTTPServer ' +
						'SimpleXMLRPCServer SocketServer StringIO tabnanny tarfile telnetlib tempfile termios textwrap ' +
						'thread threading time timeit token tokenize traceback tty types Tkinter unicodedata unittest ' +
						'urllib urllib2 urlparse user UserDict UserList UserString warnings weakref webbrowser whichdb ' +
						'xml xmllib xmlrpclib xreadlines zipfile zlib';

	this._regexList = [
		{ regex: new RegExp('#.*$', 'gm'), cssStyle: 'color: green;' },			// comments
		{ regex: new RegExp('^\\s*"""(.|\n)*?"""\\s*$', 'gm'), cssStyle: 'color: brown;' },			// documentation string "
		{ regex: new RegExp('^\\s*\'\'\'(.|\n)*?\'\'\'\\s*$', 'gm'), cssStyle: 'color: brown;' },			// documentation string '
		{ regex: new RegExp('"""(.|\n)*?"""', 'g'), cssStyle: 'color: red;' },			// multi-line strings "
		{ regex: new RegExp('\'\'\'(.|\n)*?\'\'\'', 'g'), cssStyle: 'color: red;' },			// multi-line strings '
		{ regex: new RegExp('"(?:\\.|[^\\""])*"', 'g'), cssStyle: 'color: red;' },			// strings "
		{ regex: new RegExp('\'(?:\\.|[^\\\'\'])*\'', 'g'), cssStyle: 'color: red;' },			// strings '
		{ regex: new RegExp(this._getKeywords(keywords), 'gm'), cssStyle: 'color: blue; font-weight: bold;' },			// keywords
		{ regex: new RegExp(this._getKeywords(builtins), 'gm'), cssStyle: 'color: #ff1493;' },			// builtin objects, functions, methods, magic attributes
		{ regex: new RegExp(this._getKeywords(magicmethods), 'gm'), cssStyle: 'color: #808080;' },		// special methods
		{ regex: new RegExp(this._getKeywords(exceptions), 'gm'), cssStyle: 'color: brown;' },		// standard exception classes
		{ regex: new RegExp(this._getKeywords(types), 'gm'), cssStyle: 'color: brown; font-style: italic;' },				// types from types.py
		{ regex: new RegExp(this._getKeywords(commonlibs), 'gm'), cssStyle: 'color: #8A2BE2; font-style: italic;' }			// common standard library modules
		];

}

Telerik.Web.UI.Widgets.Highlighter.PythonBrush.registerClass('Telerik.Web.UI.Widgets.Highlighter.PythonBrush', Telerik.Web.UI.Widgets.Highlighter.Brush);


/************************************************************ SQL *****************************************************/
Telerik.Web.UI.Widgets.Highlighter.SqlBrush = function()
{
	Telerik.Web.UI.Widgets.Highlighter.SqlBrush.initializeBase(this);
	var funcs	=	'abs avg case cast coalesce convert count current_timestamp ' +
					'current_user day isnull left lower month nullif replace right ' +
					'session_user space substring sum system_user upper user year';

	var keywords =	'absolute action add after alter as asc at authorization begin bigint ' +
					'binary bit by cascade char character check checkpoint close collate ' +
					'column commit committed connect connection constraint contains continue ' +
					'create cube current current_date current_time cursor database date ' +
					'deallocate dec decimal declare default delete desc distinct double drop ' +
					'dynamic else end end-exec escape except exec execute false fetch first ' +
					'float for force foreign forward free from full function global goto grant ' +
					'group grouping having hour ignore index inner insensitive insert instead ' +
					'int integer intersect into is isolation key last level load local max min ' +
					'minute modify move name national nchar next no numeric of off on only ' +
					'open option order out output partial password precision prepare primary ' +
					'prior privileges procedure public read real references relative repeatable ' +
					'restrict return returns revoke rollback rollup rows rule schema scroll ' +
					'second section select sequence serializable set size smallint static ' +
					'statistics table temp temporary then time timestamp to top transaction ' +
					'translation trigger true truncate uncommitted union unique update values ' +
					'varchar varying view when where with work';

	var operators =	'all and any between cross in join like not null or outer some';

	this._regexList = [
		{ regex: new RegExp('--(.*)$', 'gm'), cssStyle: 'color: green;' },			// one line and multiline comments
		{ regex: new RegExp('"(?:\\.|[^\\""])*"', 'g'), cssStyle: 'color: red;' },			// strings
		{ regex: new RegExp('\'(?:\\.|[^\\\'\'])*\'', 'g'), cssStyle: 'color: red;' },			// strings
		{ regex: new RegExp(this._getKeywords(funcs), 'gmi'), cssStyle: 'color: #ff1493;' },				// functions
		{ regex: new RegExp(this._getKeywords(operators), 'gmi'), cssStyle: 'color: #808080;' },				// operators and such
		{ regex: new RegExp(this._getKeywords(keywords), 'gmi'), cssStyle: 'color: blue; ' }			// keyword
		];

	this.CssClass = 'dp-sql';
}

Telerik.Web.UI.Widgets.Highlighter.SqlBrush.registerClass('Telerik.Web.UI.Widgets.Highlighter.SqlBrush', Telerik.Web.UI.Widgets.Highlighter.Brush);

/*************************************************** VB ************************************************************/
Telerik.Web.UI.Widgets.Highlighter.VbBrush = function()
{
	Telerik.Web.UI.Widgets.Highlighter.VbBrush.initializeBase(this);
	var keywords =	'AddHandler AddressOf AndAlso Alias And Ansi As Assembly Auto ' +
					'Boolean ByRef Byte ByVal Call Case Catch CBool CByte CChar CDate ' +
					'CDec CDbl Char CInt Class CLng CObj Const CShort CSng CStr CType ' +
					'Date Decimal Declare Default Delegate Dim DirectCast Do Double Each ' +
					'Else ElseIf End Enum Erase Error Event Exit False Finally For Friend ' +
					'Function Get GetType GoSub GoTo Handles If Implements Imports In ' +
					'Inherits Integer Interface Is Let Lib Like Long Loop Me Mod Module ' +
					'MustInherit MustOverride MyBase MyClass Namespace New Next Not Nothing ' +
					'NotInheritable NotOverridable Object On Option Optional Or OrElse ' +
					'Overloads Overridable Overrides ParamArray Preserve Private Property ' +
					'Protected Public RaiseEvent ReadOnly ReDim REM RemoveHandler Resume ' +
					'Return Select Set Shadows Shared Short Single Static Step Stop String ' +
					'Structure Sub SyncLock Then Throw To True Try TypeOf Unicode Until ' +
					'Variant When While With WithEvents WriteOnly Xor';

	this._regexList = [
		{ regex: new RegExp('\'.*$', 'gm'), cssStyle: 'color: green;' },			// one line comments
		{ regex: new RegExp('"(?:\\.|[^\\""])*"', 'g'), cssStyle: 'color: blue;' },			// strings
		{ regex: new RegExp('^\\s*#.*', 'gm'), cssStyle: 'color: gray;' },		// preprocessor tags like #region and #endregion
		{ regex: new RegExp(this._getKeywords(keywords), 'gm'), cssStyle: 'color: blue;' }			// c# keyword
		];

}

Telerik.Web.UI.Widgets.Highlighter.VbBrush.registerClass('Telerik.Web.UI.Widgets.Highlighter.VbBrush', Telerik.Web.UI.Widgets.Highlighter.Brush);

/******************************************************* XML ************************************************************/
Telerik.Web.UI.Widgets.Highlighter.XmlBrush = function()
{
	Telerik.Web.UI.Widgets.Highlighter.XmlBrush.initializeBase(this);
}

Telerik.Web.UI.Widgets.Highlighter.XmlBrush.prototype = 
{
	_processRegexList : function()
	{
		var push = function(array, value)
		{
			array[array.length] = value;
		}

		/* If only there was a way to get index of a group within a match, the whole XML
		   could be matched with the expression looking something like that:

		   (<!\[CDATA\[\s*.*\s*\]\]>)
		   | (<!--\s*.*\s*?-->)
		   | (<)*(\w+)*\s*(\w+)\s*=\s*(".*?"|'.*?'|\w+)(/*>)*
		   | (</?)(.*?)(/?>)
		*/
		var index = 0;
		var match = null;
		var regex = null;

		// Match CDATA in the following format <![ ... [ ... ]]>
		// <\!\[[\w\s]*?\[(.|\s)*?\]\]>
		this._getMatches(new RegExp('<\\!\\[[\\w\\s]*?\\[(.|\\s)*?\\]\\]>', 'gm'), 'color: #ff1493;');

		// Match comments
		// <!--\s*.*\s*?-->
		this._getMatches(new RegExp('<!--\\s*.*\\s*?-->', 'gm'), 'color: green;');

		// Match attributes and their values
		// (\w+)\s*=\s*(".*?"|\'.*?\'|\w+)*
		regex = new RegExp('([\\w-\.]+)\\s*=\\s*(".*?"|\'.*?\'|\\w+)*', 'gm');
		while ((match = regex.exec(this._code)) != null)
		{
			push(this._matches, new Telerik.Web.UI.Widgets.Highlighter.Match(match[1], match.index, 'color: red;'));
		
			// if xml is invalid and attribute has no property value, ignore it	
			if(match[2] != undefined)
			{
				push(this._matches, new Telerik.Web.UI.Widgets.Highlighter.Match(match[2], match.index + match[0].indexOf(match[2]), 'color: blue;'));
			}
		}

		// Match opening and closing tag brackets
		// </*\?*(?!\!)|/*\?*>
		this._getMatches(new RegExp('</*\\?*(?!\\!)|/*\\?*>', 'gm'), 'color: blue;');

		// Match tag names
		// </*\?*\s*(\w+)
		regex = new RegExp('</*\\?*\\s*([\\w-\.]+)', 'gm');
		while ((match = regex.exec(this._code)) != null)
		{
			push(this._matches, new Telerik.Web.UI.Widgets.Highlighter.Match(match[1], match.index + match[0].indexOf(match[1]), 'color: black; font-weight: bold;'));
		}
	}
}

Telerik.Web.UI.Widgets.Highlighter.XmlBrush.registerClass('Telerik.Web.UI.Widgets.Highlighter.XmlBrush', Telerik.Web.UI.Widgets.Highlighter.Brush);

//*****************************************************************************************
//                       Telerik.Web.UI.Widgets.SyntaxHighlighter
//*****************************************************************************************
Telerik.Web.UI.Widgets.SyntaxHighlighter = function()
{
	this._brushes = {
		"CSharp" : Telerik.Web.UI.Widgets.Highlighter.CSharpBrush,
		"Delphi" : Telerik.Web.UI.Widgets.Highlighter.DelphiBrush,
		"JScript" : Telerik.Web.UI.Widgets.Highlighter.JScriptBrush,
		"CSS" : Telerik.Web.UI.Widgets.Highlighter.CSSBrush,
		"Php" : Telerik.Web.UI.Widgets.Highlighter.PhpBrush,
		"Python" : Telerik.Web.UI.Widgets.Highlighter.PythonBrush,
		"Sql" : Telerik.Web.UI.Widgets.Highlighter.SqlBrush,
		"Vb" : Telerik.Web.UI.Widgets.Highlighter.VbBrush,
		"Xml" : Telerik.Web.UI.Widgets.Highlighter.XmlBrush
		};
};
Telerik.Web.UI.Widgets.SyntaxHighlighter.prototype = {
	highlight : function(code, langID, showLineNumbers)
	{
		///<summary>
		///highlights all elements identified by name and gets source code from specified property
		///</summary>
		if (!this._brushes[langID])
		{
			return code;
		}
		// instantiate a brush
		var highlighter = new this._brushes[langID]();
		highlighter.set_showLineNumbers((showLineNumbers) ? true : false);
		return highlighter.highlight(code);
	}
}
Telerik.Web.UI.Widgets.SyntaxHighlighter.registerClass('Telerik.Web.UI.Widgets.SyntaxHighlighter');

Telerik.Web.UI.Widgets.CodeFormatter = function(element)
{
	Telerik.Web.UI.Widgets.CodeFormatter.initializeBase(this, [element]);
	this._confirmButton = null;
	this._cancelButton = null;
	this._previewButton = null;
	this._dirtyCode = null;
	this._maxSnippetWidth = null;
	this._language = null;
	this._maxSnippetHeight = null;
	this._showLineNumbers = null;
	this._previewArea = null;
	this._clientParameters = null;
}

Telerik.Web.UI.Widgets.CodeFormatter.prototype = {
	initialize : function() 
	{
		Telerik.Web.UI.Widgets.CodeFormatter.callBaseMethod(this, 'initialize');
		this._initializeChildren();
	},
	
	//Called also when a dilog is cached
	clientInit : function(clientParameters)
	{
		this._clientParameters = clientParameters;
		this.set_dirtyCode(this._clientParameters.dirtyCode);
		this._previewArea.innerHTML = "";
	},
	
	_initializeChildren : function()
	{
		this._confirmButton = $get("confirm");
		this._confirmButton.title = localization["OK"];
		
		this._cancelButton = $get("cancel");
		this._cancelButton.title = localization["Cancel"];
		
		this._previewButton = $get("preview");
		this._previewButton.title = localization["Preview"];
		
		this._dirtyCode = $get("dirtyCode");
		this._maxSnippetWidth = $get("maxSnippetWidth");
		this._language = $get("language");
		this._maxSnippetHeight = $get("maxSnippetHeight");
		this._showLineNumbers = $get("showLineNumbers");
		this._previewArea = $get("previewArea");			

		$addHandlers(this._confirmButton, {"click" : this._confirmClickHandler}, this);
		$addHandlers(this._cancelButton, {"click" : this._cancelClickHandler}, this);
		$addHandlers(this._previewButton, {"click" : this._previewClickHandler}, this);
	},

	_preview : function()
	{
		this._previewArea.innerHTML = this.getFormattedCode();
	},

    /* Dom event handlers */
	_confirmClickHandler : function(e)
	{
		if (this._dirtyCode.value.trim() != "")
		{
			this._preview();

			var eventArgs = new Sys.EventArgs();
			eventArgs._code = this._previewArea.innerHTML;
			eventArgs._width = this.get_maxSnippetWidth();
			eventArgs._height = this._getWrapperHeight();

			eventArgs.get_code = function(){return this._code;};
			eventArgs.set_code = function(value){this._code = value;};
			eventArgs.get_width = function(){return this._width;};
			eventArgs.get_height = function(){return this._height;};
            eventArgs.set_code(String.format("<div style='overflow:auto;background-color:white;" + 
				"border:solid 1px #7f9db9;width:{0};height:{1};line-height: 100% !important;" +
				"font-family: Courier New;font-size: 11px;'>{2}</div>",
				eventArgs.get_width(), eventArgs.get_height(), eventArgs.get_code()));

		    Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close(eventArgs);
		}
	},
	
	_getWrapperHeight : function()
	{
		var heightOffset = 20;
		var codeTable = this._previewArea.getElementsByTagName('table')[0];
		var height = codeTable.offsetHeight + heightOffset;
		
		var maxHeight = this.get_maxSnippetHeight();
		return (height > maxHeight) ? maxHeight : height;
	},
	
	_cancelClickHandler : function(e)
	{
	    Telerik.Web.UI.Dialogs.CommonDialogScript.get_windowReference().close();
	},
	
	_previewClickHandler : function(e)
	{
		this._preview();
	},

    /* End Dom event handlers */

    /* Public API */
	/* Properties */
	get_dirtyCode : function()
	{
		return this._dirtyCode.value;
	},

	set_dirtyCode : function(dirtyCode)
	{
		this._dirtyCode.value = dirtyCode;
	},

	get_maxSnippetWidth : function()
	{
		return this._maxSnippetWidth.value;
	},
	
	set_maxSnippetWidth : function(value)
	{
		this._maxSnippetWidth.value = value;
	},

	get_maxSnippetHeight : function()
	{
		return this._maxSnippetHeight.value;
	},
	
	set_maxSnippetHeight : function(value)
	{
		this._maxSnippetHeight.value = value;
	},

	get_selectedLanguage : function()
	{
		return this._language.options[this._language.selectedIndex].value;
	},
	
	set_selectedLanguage : function(value)
	{
		this._language.selectedIndex = -1;
		for (var i=0; i<this._language.options.length; i++)
		{
			var option = this._language.options[i];
			if (option.value == value)
			{
				option.selected = true;
			}
		}
	},

	get_showLineNumbers : function()
	{
		return this._showLineNumbers.checked;
	},
	
	set_showLineNumbers : function(show)
	{
		this._showLineNumbers.checked = value;
	},
	/* End Properties */


	/* Methods */	
	getFormattedCode : function()
	{
		var highliter = new Telerik.Web.UI.Widgets.SyntaxHighlighter();
		return highliter.highlight(this.get_dirtyCode(), this.get_selectedLanguage(), this.get_showLineNumbers());
	},
	
	/* End Methods*/

    /* End Public API */
	
	dispose : function() 
	{
		$clearHandlers(this._confirmButton);
		$clearHandlers(this._cancelButton);
		$clearHandlers(this._previewButton);
		this._confirmButton = null;
		this._cancelButton = null;
		this._previewButton = null;
		this._dirtyCode = null;
		this._maxSnippetWidth = null;
		this._language = null;
		this._maxSnippetHeight = null;
		this._showLineNumbers = null;
		this._previewArea = null;
		this._clientParameters = null;
		Telerik.Web.UI.Widgets.CodeFormatter.callBaseMethod(this, 'dispose');
	}
}

Telerik.Web.UI.Widgets.CodeFormatter.registerClass('Telerik.Web.UI.Widgets.CodeFormatter', Telerik.Web.UI.RadWebControl);
</script>
<table cellpadding="0" cellspacing="0" class="rade_dialog FormatCodeBlock" style="width: 720px; height: 560px; border: solid 1px #ccc;">
	<tr>
		<td style="padding: 4px; padding-top: 0;">
			<fieldset style="width: 692px;">
				<legend><script type="text/javascript">document.write(localization["PasteSourceCode"]);</script></legend>
				<table cellpadidng="0" cellspacing="0">
					<tr>
						<td style="padding: 0 0 2px 0">
							<textarea cols="10" rows="10" style="width: 686px;" id="dirtyCode"></textarea>
						</td>
					</tr>
				</table>			
			</fieldset>
			<fieldset style="width: 692px;">
				<legend>Options</legend>
				<ul>
					<li style="float: left;">
					    <label for="maxSnippetWidth"><span style="width: 193px; display: block; float: left; text-overflow: ellipsis; overflow: hidden; white-space: nowrap; padding-top: 4px;">
					    <script type="text/javascript">document.write(localization["MaxSnippetWidth"]);</script></span>
					    </label>
					    <input type="text" id="maxSnippetWidth" style="width: 40px;" /> px / %
					</li>
					<li>
						<label for="language" style="">
						    <span style="width: 200px; display: block; float: left; text-overflow: ellipsis; overflow: hidden; white-space: nowrap; padding-top: 4px; margin-left: 38px;">
			    			    <script type="text/javascript">document.write(localization["SelectLanguage"]);</script>
			    			  </span>
						</label>
						<select id="language" style="width: 170px;">
							<option value="Xml" selected="selected"><script type="text/javascript">document.write(localization["LanguageXml"]);</script></option>
							<option value="JScript"><script type="text/javascript">document.write(localization["LanguageJavascript"]);</script></option>
							<option value="CSS"><script type="text/javascript">document.write(localization["LanguageCss"]);</script></option>
							<option value="CSharp"><script type="text/javascript">document.write(localization["LanguageCS"]);</script></option>
							<option value="Vb"><script type="text/javascript">document.write(localization["LanguageVB"]);</script></option>
							<option value="Php"><script type="text/javascript">document.write(localization["LanguagePhp"]);</script></option>
							<option value="Sql"><script type="text/javascript">document.write(localization["LanguageSQL"]);</script></option>
							<option value="Delphi"><script type="text/javascript">document.write(localization["LanguageDelphi"]);</script></option>
							<option value="Python"><script type="text/javascript">document.write(localization["LanguagePython"]);</script></option>
						</select>
					</li>
					<li style="font-size: 6px; line-height: 6px; clear: both;">&nbsp;</li>
					<li style="float: left;">
					    <label for="maxSnippetHeight"><span style="width: 193px; display: block; float: left; text-overflow: ellipsis; overflow: hidden; white-space: nowrap; padding-top: 4px; padding-bottom: 8px;">
					        <script type="text/javascript">document.write(localization["MaxSnippetHeight"]);</script></span>
					    </label>
					    <input type="text" id="maxSnippetHeight" style="width: 40px;" /> px / %
					</li>
					<li>
					    <input type="checkbox" id="showLineNumbers" style="border: 0;" />
					    <label for="showLineNumbers" style="margin-left: 36px;">
					        <script type="text/javascript">document.write(localization["DisplayLineNumbers"]);</script>
					    </label>
					</li>
				</ul>
			</fieldset>
			<fieldset style="width: 692px;">
				<legend>
					<script type="text/javascript">
					document.write(localization["Preview"]);
					</script>
				</legend>
				<table width="100%" cellpadding="0" cellspacing="0">
					<tr>
						<td class="rade_bottomcell">
							<button type="button" id="preview" style="width: 100px;">
								<script type="text/javascript">
								setInnerHtml("preview", localization["Preview"]);
								</script>
							</button>
						</td>
					</tr>
					<tr>
						<td style="padding: 0 0 2px 0">
							<div class="FormattedCodePreview" id="previewArea" style="width: 690px"><!-- / --></div>
						</td>
					</tr>
				</table>
			</fieldset>
		</td>
	</tr>
	<tr>
		<td class="rade_bottomcell">
			<button type="button" id="confirm" style="width: 100px; margin-right: 8px;">
				<script type="text/javascript">
				setInnerHtml("confirm", localization["OK"]);
				</script>
			</button>
			<button type="button" id="cancel" style="width: 100px;">
				<script type="text/javascript">
				setInnerHtml("cancel", localization["Cancel"]);
				</script>
			</button>
		</td>
	</tr>
</table>