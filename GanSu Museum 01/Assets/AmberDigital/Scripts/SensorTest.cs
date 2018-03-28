#define TEST_SENSORS

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading; // for Thread
using System.Text;  // for StringBuilder
using UnityEngine.UI;   // for test Text

public class SensorTest : MonoBehaviour {

    // 初始化设备
    [DllImport("FreqDLL", EntryPoint = "InitCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern void InitCount();

    [DllImport("FreqDLL", EntryPoint = "GetPowerCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPowerCount();
    [DllImport("FreqDLL", EntryPoint = "GetPower", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPower(int Index);
    [DllImport("FreqDLL", EntryPoint = "GetPower1", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPower1();
    [DllImport("FreqDLL", EntryPoint = "GetPower2", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPower2();
    [DllImport("FreqDLL", EntryPoint = "GetPower3", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPower3();
    [DllImport("FreqDLL", EntryPoint = "GetPower4", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPower4();
    [DllImport("FreqDLL", EntryPoint = "GetPower5", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPower5();
    [DllImport("FreqDLL", EntryPoint = "GetPower6", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPower6();
    [DllImport("FreqDLL", EntryPoint = "GetPower7", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPower7();
    [DllImport("FreqDLL", EntryPoint = "GetPower8", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetPower8();

    [DllImport("FreqDLL", EntryPoint = "GetIDEx", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetIDEx(int Index, [MarshalAs(UnmanagedType.LPStr)]StringBuilder APChar, ref int APCharLen);


    // Variable members
    public int SensorCount = 0;

    // We just use 3 sensors
    public bool bGetSensorIDs = false;
    public string[] strSensorIDs = new string[3];
    public int[] intSensorStatus = new int[3] {-2,-2,-2};

    private AmberCustomINI iniReader;

#if TEST_SENSORS
    // Test
    public Text[] testText = new Text[3];
    public GameObject background;
    // !Test
#endif


    // Use this for initialization
    void Start () {
        // Initialize sensors
        InitCount();

        // Get how many sensors
        SensorCount = GetPowerCount();

        // Get sensor ids
        iniReader = new AmberCustomINI();
        bGetSensorIDs = GetSensorIDs();

#if TEST_SENSORS
        //// procedure
        //int len = 24 + 2;       // 1st char(string length:1 char) + id(24 chars) + null(1 char)
        //for (int i = 0; i < SensorCount; i++)
        //{
        //    StringBuilder strTemp = new StringBuilder(len);
        //    GetIDEx(i + 1, strTemp, ref len);
        //    strTemp.Remove(0, 1);
        //    strSensorIDs[i] = strTemp.ToString();
        //    Debug.Log(strTemp.Length.ToString()+","+strTemp.ToString()/*.Length.ToString()*/);
        //}
#endif
    }


    // Update is called once per frame
    void Update () {
        // Check sensor count
        if (SensorCount < 3)
            return;

        // Get sensor status
        if (!bGetSensorIDs)  // Failed to get sensor ids.
        {
            for (int index = 0; index < 3; index++)
            {
                intSensorStatus[index] = GetPower(index + 1);   // Sensor index/id is base on 1.
            }
        }
        else // Get sensor ids succeeded.
        {
            int len = 24 + 2;       // 1st char(string length:1 char) + id(24 chars) + null(1 char)
            for (int iSensor = 0; iSensor < SensorCount; iSensor++)
            {
                StringBuilder strTemp = new StringBuilder(len);
                GetIDEx(iSensor + 1, strTemp, ref len);
                strTemp.Remove(0, 1);   // remove the string length char.

                for (int iSensorID = 0; iSensorID < 3; iSensorID++)
                {
                    // Compare ids
                    if (strTemp.ToString().Equals(strSensorIDs[iSensorID]))
                    {
                        intSensorStatus[iSensorID] = GetPower(iSensor + 1);
                        break;
                    }
                }
            }
        }

#if TEST_SENSORS
        // Test
        bool bAnimBackground = false;
        for (int i = 0; i < 3; i++)
        {
            testText[i].text = intSensorStatus[i].ToString();
            testText[i].color = intSensorStatus[i] > 0 ? Color.green : Color.red;
            bAnimBackground |= (intSensorStatus[i] > 0);
        }
        background.GetComponent<BackgroundFrameAnim>().animEnabled = bAnimBackground;
        // !Test
#endif
    }

    private bool GetSensorIDs()
    {
        strSensorIDs[0] = iniReader.GetIniKeyValue("Sensor1", "ID");
        strSensorIDs[1] = iniReader.GetIniKeyValue("Sensor2", "ID");
        strSensorIDs[2] = iniReader.GetIniKeyValue("Sensor3", "ID");

        for (int i = 0; i < 3; i++)
        {
            if (0 == strSensorIDs[0].Length)
                return false;
        }

        return true;
    }
}
