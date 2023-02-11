using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace DataManager
{
  [ExecuteInEditMode]
  public class Data : MonoBehaviour
  {
    [HideInInspector] public GameObject[] prefabsOfCars = new GameObject[9];
    [HideInInspector] public Texture2D[] thumbOfStaticCars = new Texture2D[9];

    string pathS = "KDStudiosElements.txt";
    string startP = "Prefabs/car";
    string ThePath;

    private void Start()
    {
      // string[] tags = { "RCSystem", "RCSystemAI", "RCSystemTrackpoint", "Data", "RCSystemTarget" };
      // Application.SetBuildTags(tags);
    }

    [HideInInspector] public bool draw;
    private void Update()
    {
      PrefabImporter();
      ThumbImporter();

    }
    private void OnDrawGizmos()
    {
      Gizmos.color = Color.blue;
      Gizmos.DrawSphere(transform.position, 0.5f);
    }

    public void Make(int id)
    {
      if (!draw)
      {
        draw = true;
      }
      if (draw)
      {
        Instantiate(prefabsOfCars[id], transform.position, Quaternion.identity);
        draw = false;
      }
    }

    private void PrefabImporter()
    {
      for (int y = 0; y < prefabsOfCars.Length; y++)
      {
        ItemObj(y, "Prefabs/car");
      }
    }

    private void ThumbImporter()
    {
      for (int y = 0; y < thumbOfStaticCars.Length; y++)
      {
        ItemThumb(y, "Thumbs/carStaticThumb");
      }
    }

    private void ItemObj(int id, string typeS)
    {
      string path = "";
      string[] guids = AssetDatabase.FindAssets("KDStudiosElements");
      foreach (string guid in guids)
      {
        path = AssetDatabase.GUIDToAssetPath(guid);
        // Debug.Log(path.Remove(path.Length - pathS.Length));
      }
      ThePath = path.Remove(path.Length - pathS.Length) + typeS + ("" + id) + ".prefab";
      //Debug.Log(ThePath);

      if (typeS == "Prefabs/car")
        prefabsOfCars[id] = (GameObject)AssetDatabase.LoadAssetAtPath(ThePath, typeof(GameObject));

      //prefabs[id] = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(transform), typeof(GameObject));
    }
    private void ItemThumb(int id, string typeS)
    {
      string path = "";
      string[] guids = AssetDatabase.FindAssets("KDStudiosElements");
      foreach (string guid in guids)
      {
        path = AssetDatabase.GUIDToAssetPath(guid);
        // Debug.Log(path.Remove(path.Length - pathS.Length));
      }
      ThePath = path.Remove(path.Length - pathS.Length) + typeS + ("" + id) + ".png";
      //Debug.Log(ThePath);

      if (typeS == "Thumbs/carStaticThumb")
        thumbOfStaticCars[id] = (Texture2D)AssetDatabase.LoadAssetAtPath(ThePath, typeof(Texture2D));


      //prefabs[id] = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(transform), typeof(GameObject));
    }
    //Assets/KitRacerTemplate/Scripts/Windows/Prefabs/car0.prefab
  }
}