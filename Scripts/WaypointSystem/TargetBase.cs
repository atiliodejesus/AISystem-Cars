using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBase : MonoBehaviour
{

  public CarSystemControl car;
  public Transform[] targets;

  public float obstacleDirectionF;
  public float obstacleDirectionR;
  public LayerMask layerMask;


  public float[] direction;

  private void Start()
  {
    direction = new float[targets.Length];
  }
  private void Update()
  {
    for (int t = 0; t < targets.Length; t++)
    {
      if (Physics.Linecast(transform.position, targets[t].position, layerMask))
      {
        direction[t] = 1;
      }
      else
      {
        direction[t] = 0;
      }
      obstacleDirectionF = direction[t] + direction[t];
      obstacleDirectionR = direction[t] + direction[t];
    }
  }

  private void OnDrawGizmos()
  {
    for (int t = 0; t < targets.Length; t++)
    {
      if (Application.isPlaying)
      {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, targets[t].position);
      }
    }
  }


}
