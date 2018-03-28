using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

[System.Serializable]
public class ParsingException: Exception{
	public ParsingException(int lineNumber, string line, string msg) : base(string.Format("Error \'{2}\' while parsing line {0}: \'{1}\'", lineNumber, line, msg)) {}
}

[System.Serializable]
public class IniSection{
	public string Name { get; set; }
	public Dictionary<string, string> Keys { get; set; }
	
	public IniSection(string name) {
		Name = name;
		Keys = new Dictionary<string, string>();
	}
}

public class AmberCustomINI {
	private List<IniSection> _sectionList;
	private string _fileName;
	private string[] _lines;
	
	// 文件路径
	public string FilePath = Application.streamingAssetsPath+ "/SensorID.ini";

	
	//实例化并获取固定文件路径
	public AmberCustomINI() {
		_sectionList = new List<IniSection>();

		#if UNITY_EDITOR
		FilePath = Application.dataPath +"/StreamingAssets"+ "/SensorID.ini";
#elif UNITY_IPHONE
		FilePath = Application.dataPath +"/Raw"+"/SensorID.ini";
#elif UNITY_ANDROID
		FilePath = "jar:file://" + Application.dataPath + "!/assets/"+"/SensorID.ini";
#endif

        _fileName = FilePath;
		Parse();
	}
	
	//定制解析ini文件
	private void Parse() {
		_lines = File.ReadAllLines(_fileName);
		
		for (int x = 0; x < _lines.Length; x++) {
			_lines[x] = _lines[x].Trim();
			string line = _lines[x];
			
			if (line.Length == 0 || line[0] == ';' || line[0] == '#' || line[0] == '\n' || line[0] == '\r') {
				continue;
			}
			
			int index;
			if (line[0] == '[') {
				index = line.IndexOf(']');
				if (index < 1) {
					throw new ParsingException(x + 1, line, "Failed to parse section header");
				}
				
				_sectionList.Add(new IniSection(line.Substring(1, index - 1)));
				
				continue;
			}
			
			if (_sectionList.Count == 0) {
				throw new ParsingException(x + 1, line, "Line is not under any section");
			}
			
			index = line.IndexOf('=');
			if (index < 1) {
				throw new ParsingException(x + 1, line, "Failed to parse line");
			}
			string key = line.Substring(0, index).Trim();
			string val = line.Substring(index + 1).Trim();
			
			if (val[0] == '\"' && val[val.Length - 1] == '\"') {
				val = val.Substring(1, val.Length - 2);
			}
			
			var section = _sectionList[_sectionList.Count - 1];
			section.Keys.Add(key, val);
		}
	}
	
	//指定获取ini的Section
	public IniSection GetSection(string sectionName) {
		return _sectionList.FirstOrDefault(s => s.Name == sectionName);
	}
	
	//获取ini所有的Section
	public List<IniSection> GetSections() {
		return _sectionList;
	}
	
	//根据Section和key获取key对应的value（string）
	public string GetIniKeyValue(string sectionName, string key) {
		string val = null;
		
		var section = GetSection(sectionName);
		if (section != null) {
			section.Keys.TryGetValue(key, out val);
		}
		
		return val;
	}
	
	//根据Section和key获取key对应的value（bool）
	public bool GetBool(string sectionName, string key) {
		var val = GetIniKeyValue(sectionName, key);
		if (String.IsNullOrEmpty(val)) {
			return false;
		}
		
		if (val == "1" && val.ToLower() == "true") {
			return true;
		}
		
		return false;
	}
	
	//根据Section和key获取key对应的value（int）
	public int GetInt(string sectionName, string key) {
		var val = GetIniKeyValue(sectionName, key);
		if (String.IsNullOrEmpty(val)) {
			return 0;
		}
		
		int value;
		if (!int.TryParse(val, out value)) {
			return 0;
		}
		
		return value;
	}
	
	//根据Section和key获取key对应的value（double）
	public double GetDouble(string sectionName, string key) {
		var val = GetIniKeyValue(sectionName, key);
		if (String.IsNullOrEmpty(val)) {
			return 0;
		}
		
		double value;
		if (!double.TryParse(val, out value)) {
			return 0;
		}
		return value;
	}
	
	//根据Section和key更新key对应的value（string）
	public void WriteIniKey(string sectionName, string key, string val) {
		var section = GetSection(sectionName);
		if (section == null) {
			throw new ArgumentException("No section found: " + sectionName);
		}
		if (!section.Keys.ContainsKey(key)) {
			throw new ArgumentException("No key found in section " + sectionName + ": " + key);
		}
		
		section.Keys[key] = val;
		
		bool inSection = false;
		for (int x = 0; x < _lines.Length; x++) {
			if (_lines[x] == "[" + sectionName + "]") {
				inSection = true;
				continue;
			}
			
			if (_lines[x].Length > 0 && _lines[x][0] == '[') {
				inSection = false;
				continue;
			}
			
			if (inSection && _lines[x].Contains("=") && (_lines[x].Substring(0, _lines[x].IndexOf('='))).Trim() == key) {
				_lines[x] = key + "=" + val;
				break;
			}
		}
		
		File.WriteAllLines(_fileName, _lines);
	}
}

