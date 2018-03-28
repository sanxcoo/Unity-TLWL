using System;
using System.IO;
using System.Runtime.InteropServices;

public class AmberWindowsINI {
    [DllImport("Kernel32", EntryPoint = "WritePrivateProfileString", CharSet = CharSet.Auto)]
    public static extern long WritePrivateProfileString(string section, string keyname, string val, string filename);

    //[DllImport("Kernel32", EntryPoint = "WritePrivateProfileString", CharSet = CharSet.Auto)]
    //public static extern long GetPrivateProfileString(string section, string keyname, string defaultVal, 

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
