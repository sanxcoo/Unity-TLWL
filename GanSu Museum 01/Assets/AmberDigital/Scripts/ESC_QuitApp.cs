using UnityEngine;
using System.Collections;

public class ESC_QuitApp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnGUI()

    {
        Event e = Event.current;
        if (e.isMouse)
        {
            if ((2 == e.button) && (e.clickCount >= 2))
                Application.Quit();

        }
    }
}
