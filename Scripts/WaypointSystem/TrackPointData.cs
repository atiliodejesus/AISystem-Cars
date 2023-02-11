using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPointData : MonoBehaviour
{
  public bool turnDirection;


  private void OnDrawGizmos()
  {
    Gizmos.color = new Color(0.4f, 0.5f, 1, 1);
    Gizmos.DrawCube(transform.position, new Vector3(3, 1, 3));

  }
}
