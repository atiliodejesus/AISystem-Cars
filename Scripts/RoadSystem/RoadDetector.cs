using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadDetector : MonoBehaviour
{

  public Type typeOfRegion;

  Receptor receptor;

  private void Start()
  {
    if (typeOfRegion == Type.City)
    {
      value = true;
    }
  }

  bool value;
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "RCSystemAI")
    {
      receptor = other.gameObject.GetComponent<Receptor>();

      if (receptor != null)
        receptor.onCity = value;
    }
  }
  private void OnTriggerExit(Collider other)
  {
    receptor = null;
  }

  public enum Type
  {
    City,
    Township
  };

}
