using System;
using System.Collections.Generic;
using System.Text;

public class FacebookBatchRequest
{
	public Dictionary<string, string> _parameters = new Dictionary<string, string>();

	private Dictionary<string, object> _requestDict = new Dictionary<string, object>();

	public FacebookBatchRequest(string relativeUrl, string method)
	{
		_requestDict["method"] = method.ToUpper();
		_requestDict["relative_url"] = relativeUrl;
	}

	public void addParameter(string key, string value)
	{
		_parameters[key] = value;
	}

	public Dictionary<string, object> requestDictionary()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (_parameters.Count > 0)
		{
			StringBuilder val = new StringBuilder();
			var enumerator = _parameters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.Current;
					val.AppendFormat("{0}={1}&", (object)current.Key, (object)current.Value);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			val.Remove(val.Length - 1, 1);
			_requestDict["body"] = val.ToString();
		}
		return _requestDict;
	}
}
