using UnityEditor;
using UnityEngine;

public class Wizard : ScriptableWizard
{
  public float range = 500;
  public Color color = Color.red;

  [MenuItem("GameObject/Create Light Wizard")]
  static void CreateWizard()
  {
    ScriptableWizard.DisplayWizard<Wizard>("Create Light", "Create", "Apply");
    //If you don't want to use the secondary button simply leave it out:
    //ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
  }

  void OnWizardCreate()
  {
    GameObject go = new GameObject("New Light");
    Light lt = go.AddComponent<Light>();
    lt.range = range;
    lt.color = color;
  }

  void OnWizardUpdate()
  {
    helpString = "Please set the color of the light!";
  }

  // When the user presses the "Apply" button OnWizardOtherButton is called.
  void OnWizardOtherButton()
  {
    if (Selection.activeTransform != null)
    {
      Light lt = Selection.activeTransform.GetComponent<Light>();

      if (lt != null)
      {
        lt.color = Color.red;
      }
    }
  }
}