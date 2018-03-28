

//#define USE_OLD_METHOD

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;  // for StringBuilder
using System.Threading;


public class SensorListener : MonoBehaviour {

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


#if USE_OLD_METHOD
    #region Old method
    public int SensorIndex = 1;     // base on 1
    public string strSensorID;

    public float shutDelay = 5.0f;
    private bool bDelayToShut = false;
    private float shutDelayTimer = 0.0f;
    [HideInInspector]
    public bool status = false;

    // Indicator light
    public SpriteRenderer spriteRender;

    // delegate
    //public delegate void SensorStatusChangedDelegate(int status, int ranNum);
    //public static event SensorStatusChangedDelegate SensorStatusChanged;

    public static GameObject[] s_relics;

    public GameObject[] monitorObjects;
    private GameObject currentObject = null;
    private System.Random ranGenerater;

    private AmberCustomINI amberCustomIni;

    // Set up sensor id
    void SetSensorID()
    {
        // Open the amber custom ini file.
        amberCustomIni = new AmberCustomINI();

        // Get sensor id
        strSensorID = amberCustomIni.GetIniKeyValue("Sensor" + SensorIndex, "ID");

        //System.Random randomGenerater = new System.Random();
        //uRandomNumMax = randomGenerater.Next(uRandomNumMax);

        // Initialize sensor
        InitCount();


    }

    // Use this for initialization
    void Start () {

        SetSensorID();

        ranGenerater = new System.Random(DateTime.Now.Millisecond);
	
	}
	
	// Update is called once per frame
	void Update () {
        if (0 == monitorObjects.Length)
            return;

        // Get amount of sensor
        int nSensorCount = GetPowerCount();

        // Check sensors
        if (nSensorCount < 1)
            return;

        // Indicator light
        if(spriteRender)
        {
            spriteRender.color = status ? Color.green : Color.red;
        }
        
        // Delay to disable current object
        if(bDelayToShut)
        {
            shutDelayTimer += Time.deltaTime;

            if(shutDelayTimer >= shutDelay)
            {
                currentObject.SetActive(false);
                currentObject = null;
                CancelDisableCurrentObject();
            }
        }

        // Traverse sensors
        for (int index = 1; index <= nSensorCount; index++)
        {
            int len = 1 + 24 + 1;       // 1st char(string length:1 char) + id(24 chars) + null(1 char)
            StringBuilder strTemp = new StringBuilder(len);
            GetIDEx(index, strTemp, ref len);
            if (0 == strTemp.Length)
                continue;
            strTemp = strTemp.Remove(0, 1);

            if(strTemp.ToString().Equals(strSensorID))
            {
                bool newStatus = (GetPower(index) > 0);

                // No status changed
                if (status == newStatus)
                    break;

                // No need to be retriggered.
                if(!status && bDelayToShut)
                {
                    status = true;
                    CancelDisableCurrentObject();
                    break;
                }

                status = newStatus;

                if(status)
                {
                    currentObject = monitorObjects[ranGenerater.Next(monitorObjects.Length)];
                    currentObject.SetActive(true);
                }
                else
                {
                    DelayDisableCurrentObject();
                }

                break;
            }
        }
	}

    private void DelayDisableCurrentObject()
    {
        bDelayToShut = true;
        shutDelayTimer = 0.0f;
    }

    private void CancelDisableCurrentObject()
    {
        bDelayToShut = false;
    }
    #endregion  // Old method
#else

    /// <summary>
    /// 触发模式分为两种：正常触发模式和密集触发模式
    /// 时间单位：秒
    /// </summary>

    public static bool sharedUpdateTimeFlag = true;

    public static float nextActiveTime = 10.0f;
    public static float sharedActiveTimer = 0.0f;

    // 正常触发模式
    public static float maxNormalActiveInterval = 60.0f;     // 正常触发模式下，弹出窗口的间隔时间取值范围
    public static float minNormalActiveInterval = 30.0f;       // 

    // 密集触发模式
    public static bool denseActiveMode = false;
    public static float maxDenseActiveInterval = 5.0f;      // 密集触发模式下，弹出窗口的间隔时间取值范围
    public static float minDenseActiveInterval = 1.0f;

    // 两次密集触发模式的间隔时间
    public static float maxDenseActiveCycle = 150.0f;        // 两次密集弹出窗口事件的时间间隔。在此期间为正常触发模式
    public static float minDenseActiveCycle = 60.0f;
    public static float nextDenseActiveCycle = 1.0f;
    public static float denseActiveCycleTimer = 0.0f;


    public int SensorIndex = 1;     // base on 1
    public string strSensorID;

    public static float maxShutDelay = 15.0f;      // 弹出窗口的显示时间取值范围
    public static float minShutDelay = 10.0f;      // 
    public float shutDelay = 5.0f;  // 无继续触发情况下，多少秒后关闭(驿使图文字简介的滚动循环是45秒)
    private bool bDelayToShut = false;
    private float shutDelayTimer = 0.0f;
    //[HideInInspector]
    public bool status = false;
    
    // Indicator light
    public SpriteRenderer spriteRender;


    // Monitor object
    public GameObject monitorObject;

    private AmberCustomINI amberCustomIni;

    public static float initSensorsDelay = 5.0f;
    private float initSensorsTimer;


    // Set up sensor id
    void SetSensorID()
    {
        // Open the amber custom ini file.
        amberCustomIni = new AmberCustomINI();

        // Get sensor id
        strSensorID = amberCustomIni.GetIniKeyValue("Sensor" + SensorIndex, "ID");

        //System.Random randomGenerater = new System.Random();
        //uRandomNumMax = randomGenerater.Next(uRandomNumMax);

        // Initialize sensor
        InitSensors();


    }

    void InitSensors()
    {
        InitCount();
    }

    void ReadyToActiveNext()
    {
        // Init next active time.
        nextActiveTime = denseActiveMode ? Random.Range(minDenseActiveInterval, maxDenseActiveInterval) : Random.Range(minNormalActiveInterval, maxNormalActiveInterval);

        sharedActiveTimer = 0.0f;
    }

    public bool IsInDenseActiveMode()
    {
        return denseActiveMode;
    }

    public void EnterDenseActiveMode()
    {
        denseActiveMode = true;
    }

    public void LeaveDenseActiveMode()
    {
        denseActiveMode = false;

        nextDenseActiveCycle = Random.Range(minDenseActiveCycle, maxDenseActiveCycle);
        denseActiveCycleTimer = 0.0f;

        ReadyToActiveNext();
    }

    // Use this for initialization
    void Start()
    {
        SetSensorID();

        // Get sensors' id. And save to the sensorid.ini
        int count = GetPowerCount();
        for (int i = 0; i < count; i++)
        {
            int len = 1 + 24 + 1;
            StringBuilder strTemp = new StringBuilder(len);
            GetIDEx(i + 1, strTemp, ref len);
            //Debug.Log(strTemp.ToString());
            amberCustomIni.WriteIniKey("Sensor_" + (i + 1), "ID", strTemp.Remove(0,1).ToString());
        }

        LeaveDenseActiveMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (null == monitorObject)
            return;

        // Init sensors every 5 sec.
        initSensorsTimer -= Time.deltaTime;
        if(initSensorsTimer <= 0.0f)
        {
            InitSensors();
            initSensorsTimer = initSensorsDelay;
        }

        /*
        // Get amount of sensor
        int nSensorCount = GetPowerCount();
        
        // Check sensors
        if (nSensorCount < 1)
            return;
        */

        // Indicator light
        if (spriteRender)
        {
            spriteRender.color = status ? Color.green : Color.red;
        }

        // Delay to disable current object
        if (bDelayToShut)
        {
            shutDelayTimer += Time.deltaTime;

            if (shutDelayTimer >= shutDelay)
            {
                //monitorObject.SetActive(false);
                monitorObject.GetComponent<PopupRelicWindow>().DelayToDisable();
                CancelDisableCurrentObject();

                // <Notice> 
                // 非传感器触发。由随机变量触发
                // Upate status
                status = false;
                // <!Notice>

            }
        }

        /*
        // Traverse sensors
        for (int index = 1; index <= nSensorCount; index++)
        {
            int len = 1 + 24 + 1;       // 1st char(string length:1 char) + id(24 chars) + null(1 char)
            StringBuilder strTemp = new StringBuilder(len);
            GetIDEx(index, strTemp, ref len);
            if (0 == strTemp.Length)
                continue;
            strTemp = strTemp.Remove(0, 1);

            if (strTemp.ToString().Equals(strSensorID))
            {
                bool newStatus = (GetPower(index) > 0);

                // No status changed
                if (status == newStatus)
                    break;

                // No need to be retriggered.
                if (!status && bDelayToShut)
                {
                    status = true;
                    CancelDisableCurrentObject();
                    break;
                }

                status = newStatus;

                if (status)
                {
                    if(!monitorObject.activeInHierarchy)
                        monitorObject.SetActive(true);
                }
                else
                {
                    DelayDisableCurrentObject();
                }

                break;
            }
        }
        */

        // Enter dense active mode if any.
        if (!denseActiveMode)
        {
            denseActiveCycleTimer += sharedUpdateTimeFlag ? Time.deltaTime : 0;
            if (denseActiveCycleTimer > nextDenseActiveCycle)
            {
                EnterDenseActiveMode();

                ReadyToActiveNext();
            }
        }

        // Ready to active monitor object.
        if(sharedActiveTimer >= nextActiveTime)
        {
            TryActiveMonitorObject();
        }
        else // Update shared timer.
        {
            sharedActiveTimer += sharedUpdateTimeFlag ? Time.deltaTime : 0;
        }

        sharedUpdateTimeFlag = false;
    }

    void LateUpdate()
    {
        sharedUpdateTimeFlag = true;
    }

    private void TryActiveMonitorObject()
    {
        if (monitorObject.activeInHierarchy)
            return;

        if( Random.Range(1,4) == SensorIndex)
        {
            monitorObject.SetActive(true);

            status = monitorObject.activeInHierarchy;
            shutDelay = status ? Random.Range(minShutDelay, maxShutDelay) : 0.0f;

            if(status)
            {
                // 如果是驿使图
                if (monitorObject.GetComponent<PopupRelicWindow>().prefab_Relic.name.Equals("Relic_驿使图壁画砖"))
                {
                    shutDelay = 45.0f;
                }
            }

            // Set delay to shut down / inactive monitor object.
            DelayDisableCurrentObject();

            ReadyToActiveNext();
        }
    }

    private void DelayDisableCurrentObject()
    {
        bDelayToShut = true;
        shutDelayTimer = 0.0f;
    }

    private void CancelDisableCurrentObject()
    {
        bDelayToShut = false;
    }

    //void OnGUI()
    //{
    //    GUI.Button(new Rect(0, 0, 200, 50), Time.fixedTime + " / " + sharedActiveTimer);
    //}

#endif

}
