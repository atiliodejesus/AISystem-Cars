using UnityEngine;
using UnityEditor;
using DataManager;
[ExecuteInEditMode]
public class Groups : EditorWindow
{
  public static Data data;
  Vector2 scrollPos;
  [MenuItem("KD-AISystem/Manager")]
  private static void ShowWindow()
  {
    data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
    var window = GetWindow<Groups>();
    window.titleContent = new GUIContent("Groups");
    if (data == null)
    {
      Debug.Log("Don't Loaded");
    }
    window.Show();
    window.minSize = new Vector2(520, 550);
    window.maxSize = new Vector2(520, 550);
    window.autoRepaintOnSceneChange = true;
  }
  //  ItemForm("Car" + 1, "Des", 1);
  //     ItemForm("Car" + 2, "Des", 2);
  //     ItemForm("Car" + 3, "Des", 3);
  //     ItemForm("Car" + 4, "Des", 4);

  private void Update()
  {
    data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
  }

  private void OnGUI()
  {
    LogoSetting();
    var window = GetWindow<Groups>();
    scrollPos = GUI.BeginScrollView(new Rect(0, 0, window.position.width * 7, window.position.height * 7), scrollPos, new Rect(0, 0, window.position.width * 7, window.position.height * 7));
    data = GameObject.FindGameObjectWithTag("Data").GetComponent<Data>();
    EditorGUILayout.BeginVertical();
    EditorGUILayout.BeginHorizontal();
    ItemForm("Compact", 1, 0);
    ItemForm("SedamLow", 2, 1);
    ItemForm("SedamHigh", 3, 2);
    ItemForm("SUV", 4, 3);
    EditorGUILayout.EndHorizontal();
    EditorGUILayout.BeginHorizontal();
    ItemForm2("PickUp", 1, 4);
    ItemForm2("SimpleTruck", 2, 5);
    ItemForm2("Truck", 3, 6);
    ItemForm2("Bus", 4, 7);
    EditorGUILayout.EndHorizontal();
    EditorGUILayout.BeginHorizontal();
    ItemForm3("AICircuit", 1, 8);
    EditorGUILayout.EndHorizontal();
    EditorGUILayout.EndVertical();
    GUI.EndScrollView();

  }


  int[] posX = { 0, 1, 7, 9, 10 };
  private void ItemForm(string title, int pos, int id)
  {
    GUIStyle titleStyle = new GUIStyle();
    titleStyle.fontSize = 13;
    titleStyle.normal.textColor = Color.white;
    Rect posSetButton = new Rect(10 * pos * posX[pos], 110, 110, 110);
    if (GUI.Button(posSetButton, (data.thumbOfStaticCars[pos - 1])))
    {
      data.Make(id);
    }
    GUI.Label(new Rect(10 * pos * posX[pos], 220, 120, 10), title, titleStyle);

  }
  private void ItemForm2(string title, int pos, int id)
  {
    GUIStyle titleStyle = new GUIStyle();
    titleStyle.fontSize = 13;
    titleStyle.normal.textColor = Color.white;
    Rect posSetButton = new Rect(10 * pos * posX[pos], 130 * 2f, 110, 110);

    if (GUI.Button(posSetButton, (data.thumbOfStaticCars[(pos - 1) + 4])))
    {
      data.Make(id);
    }
    GUI.Label(new Rect(10 * pos * posX[pos], 130 * 2.85f, 120, 10), title, titleStyle);
  }

  private void ItemForm3(string title, int pos, int id)
  {
    GUIStyle titleStyle = new GUIStyle();
    titleStyle.fontSize = 13;
    titleStyle.normal.textColor = Color.white;
    Rect posSetButton = new Rect(10 * pos * posX[pos], 130 * 3.15f, 110, 110);
    if (GUI.Button(posSetButton, (data.thumbOfStaticCars[8])))
    {
      data.Make(id);
    }
    GUI.Label(new Rect(10 * pos * posX[pos], 130 * 4f, 120, 10), title, titleStyle);
  }
  private Texture2D logo;
  public static string pathOfProject;
  private void LogoSetting()
  {
    if (logo == null)
    {
      string[] guids = AssetDatabase.FindAssets("KDStudios-Assets");
      foreach (string guid in guids)
      {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        pathOfProject = path;
        logo = AssetDatabase.LoadMainAssetAtPath(path) as Texture2D;
        if (logo != null)
        {
          break;
        }
      }
    }
    if (logo != null)
    {
      const float maxLogoWidth = 430.0f;
      EditorGUILayout.Separator();
      float w = EditorGUIUtility.currentViewWidth;
      Rect r = new Rect();
      r.width = Mathf.Min(w - 40.0f, maxLogoWidth);
      r.height = r.width / 4.7f;
      Rect r2 = GUILayoutUtility.GetRect(r.width, r.height);
      r.x = ((EditorGUIUtility.currentViewWidth - r.width) * 0.5f) - 4.0f;
      r.y = r2.y;
      GUI.DrawTexture(r, logo, ScaleMode.StretchToFill);
      if (GUI.Button(r, "", new GUIStyle()))
      {
        Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/60955?aid=1011lGnL");
      }
      EditorGUILayout.Separator();
    }
  }
}

