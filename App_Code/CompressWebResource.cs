#region Using

using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Web.Caching;
using System.Net;

#endregion

public class JavaScriptHandler : IHttpHandler
{
	/// <summary>
	/// Enables processing of HTTP Web requests by a custom 
	/// HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
	/// </summary>
	/// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides 
	/// references to the intrinsic server objects 
	/// (for example, Request, Response, Session, and Server) used to service HTTP requests.
	/// </param>
	public void ProcessRequest(HttpContext context)
	{
		string path = context.Request.Url.GetLeftPart(UriPartial.Authority) + context.Request.QueryString["path"];
		string script = null;

		if (!string.IsNullOrEmpty(path))
		{
			if (context.Cache[path] == null)
			{
				script = RetrieveScript(path);
			}
		}

		script = (string)context.Cache[path];
		if (!string.IsNullOrEmpty(script))
		{
			context.Response.Write(script);
			SetHeaders(script.GetHashCode(), context);

			Compress(context);
		}
	}

	/// <summary>
	/// Retrieves the specified remote script using a WebClient.
	/// </summary>
	/// <param name="file">The remote URL</param>
	private static string RetrieveScript(string file)
	{
		string script = null;

		try
		{
			Uri url = new Uri(file, UriKind.Absolute);

			using (WebClient client = new WebClient())
			using (Stream buffer = client.OpenRead(url))
			using (StreamReader reader = new StreamReader(buffer))
			{
				script = StripWhitespace(reader.ReadToEnd());
				HttpContext.Current.Cache.Insert(file, script, null, Cache.NoAbsoluteExpiration, new TimeSpan(7, 0, 0, 0));
			}
		}
		catch (System.Net.Sockets.SocketException)
		{
			// The remote site is currently down. Try again next time.
		}
		catch (UriFormatException)
		{
			// Only valid absolute URLs are accepted
		}

		return script;
	}

	/// <summary>
	/// Strips the whitespace from any .js file.
	/// </summary>
	private static string StripWhitespace(string body)
	{
		string[] lines = body.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		StringBuilder sb = new StringBuilder();
		foreach (string line in lines)
		{
			string s = line.Trim();
			if (s.Length > 0 && !s.StartsWith("//"))
				sb.AppendLine(s.Trim());
		}

		body = sb.ToString();
		body = Regex.Replace(body, @"^[\s]+|[ \f\r\t\v]+$", String.Empty);
		body = Regex.Replace(body, @"([+-])\n\1", "$1 $1");
		body = Regex.Replace(body, @"([^+-][+-])\n", "$1");
		body = Regex.Replace(body, @"([^+]) ?(\+)", "$1$2");
		body = Regex.Replace(body, @"(\+) ?([^+])", "$1$2");
		body = Regex.Replace(body, @"([^-]) ?(\-)", "$1$2");
		body = Regex.Replace(body, @"(\-) ?([^-])", "$1$2");
		body = Regex.Replace(body, @"\n([{}()[\],<>/*%&|^!~?:=.;+-])", "$1");
		body = Regex.Replace(body, @"(\W(if|while|for)\([^{]*?\))\n", "$1");
		body = Regex.Replace(body, @"(\W(if|while|for)\([^{]*?\))((if|while|for)\([^{]*?\))\n", "$1$3");
		body = Regex.Replace(body, @"([;}]else)\n", "$1 ");
		body = Regex.Replace(body, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&ndsp;)\s{2,}(?=[<])", String.Empty);

		return body;
	}

	/// <summary>
	/// This will make the browser and server keep the output
	/// in its cache and thereby improve performance.
	/// </summary>
	private static void SetHeaders(int hash, HttpContext context)
	{
		context.Response.ContentType = "text/javascript";
		context.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;

		context.Response.Cache.SetExpires(DateTime.Now.ToUniversalTime().AddDays(7));
		context.Response.Cache.SetCacheability(HttpCacheability.Public);
		context.Response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
		context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
		context.Response.Cache.SetETag("\"" + hash.ToString() + "\"");
	}

	#region Compression

	private const string GZIP = "gzip";
	private const string DEFLATE = "deflate";

	private static void Compress(HttpContext context)
	{
		if (context.Request.UserAgent != null && context.Request.UserAgent.Contains("MSIE 6"))
			return;

		if (IsEncodingAccepted(GZIP))
		{
			context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
			SetEncoding(GZIP);
		}
		else if (IsEncodingAccepted(DEFLATE))
		{
			context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
			SetEncoding(DEFLATE);
		}
	}

	/// <summary>
	/// Checks the request headers to see if the specified
	/// encoding is accepted by the client.
	/// </summary>
	private static bool IsEncodingAccepted(string encoding)
	{
		return HttpContext.Current.Request.Headers["Accept-encoding"] != null && HttpContext.Current.Request.Headers["Accept-encoding"].Contains(encoding);
	}

	/// <summary>
	/// Adds the specified encoding to the response headers.
	/// </summary>
	/// <param name="encoding"></param>
	private static void SetEncoding(string encoding)
	{
		HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
	}

	#endregion

	/// <summary>
	/// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
	/// </summary>
	/// <value></value>
	/// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
	public bool IsReusable
	{
		get { return false; }
	}

}


/// <summary>
/// Removes whitespace from the webpage.
/// </summary>
public class CompressWebResource : IHttpModule
{

	#region IHttpModule Members

	void IHttpModule.Dispose()
	{
		// Nothing to dispose; 
	}

	void IHttpModule.Init(HttpApplication context)
	{
		context.PostRequestHandlerExecute += new EventHandler(context_BeginRequest);
	}

	#endregion

	void context_BeginRequest(object sender, EventArgs e)
	{
		HttpApplication app = sender as HttpApplication;
		if (app.Context.CurrentHandler is Page)
		{
			app.Response.Filter = new WebResourceFilter(app.Response.Filter);
		}
	}

	#region Stream filter

	private class WebResourceFilter : Stream
	{

		public WebResourceFilter(Stream sink)
		{
			_sink = sink;
		}

		private Stream _sink;

		#region Properites

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override void Flush()
		{
			_sink.Flush();
		}

		public override long Length
		{
			get { return 0; }
		}

		private long _position;
		public override long Position
		{
			get { return _position; }
			set { _position = value; }
		}

		#endregion

		#region Methods

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _sink.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _sink.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_sink.SetLength(value);
		}

		public override void Close()
		{
			_sink.Close();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			byte[] data = new byte[count];
			Buffer.BlockCopy(buffer, offset, data, 0, count);
			string html = System.Text.Encoding.Default.GetString(buffer);

			Regex regex = new Regex("<script\\s*src=\"((?=[^\"]*webresource.axd)[^\"]*)\"\\s*type=\"text/javascript\"[^>]*>[^<]*(?:</script>)?", RegexOptions.IgnoreCase);
			foreach (Match match in regex.Matches(html))
			{
				string relative = match.Groups[1].Value;
				html = html.Replace(relative, "js.axd?path=" + HttpUtility.UrlEncode(relative));
			}

			byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
			_sink.Write(outdata, 0, outdata.GetLength(0));
		}

		#endregion

	}

	#endregion

}
