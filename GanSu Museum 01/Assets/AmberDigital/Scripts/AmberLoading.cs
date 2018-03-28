using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AmberLoading : MonoBehaviour {

	public int sceneIndex = 1;
	public Text progressText;
	public delegate void AmberSceneLoadFinishDelegate();
	public static event AmberSceneLoadFinishDelegate AmberSceneLoadFinish;

	public delegate void AmberSceneLoadingDelegate(int progressValue);
	public static event AmberSceneLoadingDelegate AmberSceneLoading;

	//CustomINI
	private AmberCustomINI  amberCustomINI;

	// Use this for initialization
	void Start () {
		// 加载场景
		//StartCoroutine (StartLoading(sceneIndex));
		//WriteIniKey GetIniKeyValue
		//amberCustomINI = new AmberCustomINI ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator StartLoading(int scene){
		
		int displayProgress = 0;
		int toProgress = 0;
		
		// 得到加载场景的异步对象
		AsyncOperation op = Application.LoadLevelAsync(scene);
		// 禁止Unity加载完毕后自动切换场景
		op.allowSceneActivation = false;
		
		// 由于unity在场景异步加载时只能加载到90%，此处需循环判断直至进度到达90%
		while(op.progress < 0.9f){
			
			toProgress = (int)op.progress*100;
			
			while(displayProgress < toProgress){
				++displayProgress;
				
				UpdateLoadingPercentage(displayProgress);
				// 等待帧结束
				yield return new WaitForEndOfFrame();
			}
			
		}
		
		toProgress = 100;
		while(displayProgress < toProgress){
			++displayProgress;
			
			UpdateLoadingPercentage(displayProgress);
			// 等待帧结束
			yield return new WaitForEndOfFrame();
		}
		
		// 等待1秒
		yield return new WaitForSeconds(1.0f);
		// 允许Unity加载完毕后自动切换场景
		//op.allowSceneActivation = true;
		
	}

	// 显示加载进度
	private void UpdateLoadingPercentage(int progressValue){
		Debug.Log("progress:"+progressValue+"%");
		progressText.text = progressValue + "%";
	}
}
