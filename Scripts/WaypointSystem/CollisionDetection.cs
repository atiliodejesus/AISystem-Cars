using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
  public bool have;


  private void OnTriggerEnter(Collider other)
  {
    if ((other.gameObject.tag == "RCSystemAI" || other.gameObject.tag == "RCSystem"))
    {
      have = true;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    have = false;
  }
}
