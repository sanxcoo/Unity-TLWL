using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using UnityEditor.Animations;

[CustomEditor(typeof(CreateFrameAnimation))]
public class CreateFrameAnimationEditor : Editor {
    //SerializedProperty animationTime;
    //SerializedProperty animFrameTime;
    //SerializedProperty frames;


    //void OnEnable()
    //{
    //    animationTime = serializedObject.FindProperty("animationTime");
    //    animFrameTime = serializedObject.FindProperty("animFrameRate");
    //    frames = serializedObject.FindProperty("frames");
    //}

    public override void OnInspectorGUI()
    {
        CreateFrameAnimation creator = target as CreateFrameAnimation;


        base.OnInspectorGUI();
        

        // Reorder frames
        if (GUILayout.Button("Reorder frames"))
        {
            if (!ReorderFrames())
                return;
        }

        EditorGUILayout.Separator();


        EditorGUILayout.BeginHorizontal();

        // Animation location
        GUIStyle fontBoldStyle = new GUIStyle();
        fontBoldStyle.fontStyle = FontStyle.Bold;
        fontBoldStyle.alignment = TextAnchor.LowerLeft;
        fontBoldStyle.margin.bottom = 0;
        Vector2 labelSize = fontBoldStyle.CalcSize(new GUIContent("File Location:"));
        EditorGUILayout.LabelField("File Location:", fontBoldStyle, GUILayout.Width(labelSize.x), GUILayout.Height(labelSize.y));

        // Create animation clip button
        if (GUILayout.Button("Create"))
        {
            //
            // Check params
            //
            //if (creator.animationTime <= 0)
            //{
            //    EditorUtility.DisplayDialog("错误", "动画时长不能小于0!", "OK");
            //    return;
            //}
            if (creator.animFrameRate <= 0)
            {
                EditorUtility.DisplayDialog("错误", "动画帧率不能小于0!", "OK");
                return;
            }
            // Reorder frames
            if (!ReorderFrames())
                return;

            // Get animation path
            string strTemp = EditorUtility.SaveFilePanel("创建序列帧动画", creator.animFilePath, "New Frame Animation.anim", "anim");

            if (0 == strTemp.Length)
            {
                return;
            }
            Debug.Log(strTemp);
            creator.animFilePath = strTemp.Substring(strTemp.IndexOf("Assets/"));
            Debug.Log(creator.animFilePath);


            //
            // Create animation clip
            //
            AnimationClip animClip = CreateAnimationClip(creator.animFilePath);

            //
            // Create animation controller
            //
            AnimatorController animController = CreateAnimatorController(animClip, creator.animFilePath);

            //
            // Create animation prefab
            //
            CreateFarmeAnimationPrefab(animController, creator.animFilePath);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox(creator.animFilePath, MessageType.None);
        
        

        
    }

    // Reorder animation frames textures
    private bool ReorderFrames()
    {
        CreateFrameAnimation creator = target as CreateFrameAnimation;

        if (0 == creator.frames.Count)
        {
            EditorUtility.DisplayDialog("错误", "还未选定序列帧!", "OK");
            return false;
        }

        creator.frames.Sort((s1, s2) => s1.name.CompareTo(s2.name));

        return true;
    }

    // Create animation clip
    private AnimationClip CreateAnimationClip(string strFilePath)
    {
        CreateFrameAnimation creator = target as CreateFrameAnimation;

        AnimationClip newClip = new AnimationClip();
        //AnimationUtility.SetAnimationType(newClip, ModelImporterAnimationType.Generic);

        EditorCurveBinding curve = new EditorCurveBinding();
        //curve.type = typeof(RawImage);
        //curve.path = "";
        //curve.propertyName = "m_Texture";

        curve.type = typeof(Image);
        curve.path = "";
        curve.propertyName = "m_Sprite";


        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[creator.frames.Count];
        float frameTime = 1.0f / creator.animFrameRate;
        for(int i = 0; i < creator.frames.Count; i++)
        {
            keyFrames[i] = new ObjectReferenceKeyframe();
            keyFrames[i].time = frameTime * i;
            keyFrames[i].value = creator.frames[i];
        }

        // animation duration
        creator.animationTime = frameTime * creator.frames.Count;

        // Frame rate
        newClip.frameRate = creator.animFrameRate;

        // Set loop true
        AnimationClipSettings clipSettings = new AnimationClipSettings();
        clipSettings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(newClip, clipSettings);


        AnimationUtility.SetObjectReferenceCurve(newClip, curve, keyFrames);

        string strTemp = DataPathToAssetPath(strFilePath);
        strTemp = strTemp.Substring(0, strTemp.LastIndexOf('.'));
        AssetDatabase.CreateAsset(newClip, strTemp + ".anim");


        AssetDatabase.SaveAssets();

        return newClip;
    }


    // Create animation controller
    private AnimatorController CreateAnimatorController(AnimationClip clip, string strFilePath)
    {
        string strTemp = DataPathToAssetPath(strFilePath);
        //strTemp.Remove(strTemp.LastIndexOf('.'), strTemp.Substring(strTemp.LastIndexOf('.')).Length);
        strTemp = strTemp.Substring(0, strTemp.LastIndexOf('.'));

        AnimatorController newController = AnimatorController.CreateAnimatorControllerAtPathWithClip(strTemp + ".controller", clip);

        AssetDatabase.SaveAssets();

        return newController;
    }

    // Create animation prefab
    // Store: Assets/AmberDigital/Prefabs
    private void CreateFarmeAnimationPrefab(AnimatorController animController, string strFilePath)
    {
        CreateFrameAnimation creator = target as CreateFrameAnimation;

        string strFile = strFilePath;
        strFile = strFile.Substring(0, strFile.LastIndexOf('.'));

        GameObject go = new GameObject();

        //RawImage img = go.AddComponent<RawImage>();
        //img.texture = creator.frames[0];
        //SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
        //spriteRenderer.sprite = creator.frames[0];
        Image img = go.AddComponent<Image>();
        img.sprite = creator.frames[0];

        Animator animator = go.AddComponent<Animator>();
        animator.runtimeAnimatorController = animController;

        PrefabUtility.CreatePrefab("Assets/AmberDigital/Prefabs" + strFile.Substring(strFile.LastIndexOf('/')) + ".prefab", go);

        DestroyImmediate(go);
    }


    public static string DataPathToAssetPath(string path)
    {
        //if (Application.platform == RuntimePlatform.WindowsEditor)

        //    return path.Substring(path.IndexOf("Assets\\"));
        //else

            return path.Substring(path.IndexOf("Assets/"));
    }
}
