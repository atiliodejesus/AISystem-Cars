using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Item : MonoBehaviour
{
  private Texture2D thumb;
  private void ItemThumb(int id)
  {
    if (thumb == null)
    {
      string[] guids = AssetDatabase.FindAssets("Thumb" + (id + 0));
      foreach (string guid in guids)
      {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        thumb = AssetDatabase.LoadMainAssetAtPath(path) as Texture2D;
        if (thumb != null)
        {
          break;
        }
      }
    }
    if (thumb != null)
    {
      EditorGUILayout.Separator();
      Rect r = new Rect();
      r.width = 80;
      r.height = 80;
      GUI.DrawTexture(r, thumb, ScaleMode.StretchToFill);
      if (GUI.Button(r, "", new GUIStyle()))
      {
        Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/60955?aid=1011lGnL");
      }
      EditorGUILayout.Separator();
    }
  }
}

