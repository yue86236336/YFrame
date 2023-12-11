using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FontTool : EditorWindow
{
    private static FontTool windowTool = null;
    private static Font targetFont;
    private static Font replacedFont;

    [MenuItem("Tools/更换字体")]
    public static void ShowToolWindow()
    {
        if (windowTool == null)
        {
            windowTool = EditorWindow.GetWindow(typeof(FontTool)) as FontTool;
        }
        windowTool.titleContent = new GUIContent("字体工具");
        windowTool.Show();
    }

    private void OnGUI()
    {
        replacedFont = (Font)EditorGUILayout.ObjectField("被替换的字体", replacedFont, typeof(Font), true);
        targetFont = (Font)EditorGUILayout.ObjectField("替换的字体", targetFont, typeof(Font), true);
        if (GUILayout.Button("替换选中字体"))
        {
            ChangeCurrentFont();
        }

        if (GUILayout.Button("替换所有字体"))
        {
            ChangeAllFont();
        }
    }

    private void ChangeCurrentFont()
    {
        Scene currentScene = EditorSceneManager.GetActiveScene();
        if (currentScene.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存场景", "确定");
            return;
        }
        if (replacedFont == null || targetFont == null)
        {
            EditorUtility.DisplayDialog("提示", "请设置要替换的字体", "确定");
            return;
        }
        Text[] texts = GameObject.FindObjectsOfType<Text>();
        foreach (var text in texts)
        {
            if (text.font != replacedFont)
                continue;
            text.font = targetFont;
        }
        EditorUtility.DisplayDialog("提示", "字体替换完成", "确定");
        replacedFont = null;
        targetFont = null;
        windowTool.Close();
    }

    private void ChangeAllFont()
    {
        Scene currentScene = EditorSceneManager.GetActiveScene();
        if (currentScene.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存场景", "确定");
            return;
        }
        if (targetFont == null)
        {
            EditorUtility.DisplayDialog("提示", "请设置替换的字体", "确定");
            return;
        }
        Text[] texts = GameObject.FindObjectsOfType<Text>();
        foreach (var text in texts)
        {
            text.font = targetFont;
        }
        EditorUtility.DisplayDialog("提示", "字体替换完成", "确定");
        replacedFont = null;
        targetFont = null;
        windowTool.Close();
    }

}
