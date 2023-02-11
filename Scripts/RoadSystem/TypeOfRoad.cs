using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeOfRoad : MonoBehaviour
{

  public static bool inCity;
  public bool onCity;

  private void Update()
  {
    onCity = inCity;
  }

}
