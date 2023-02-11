using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDrive : MonoBehaviour
{
  private AudioSource audioMotor;
  private Rigidbody rgb;
  private GameObject Player;


  private void Start()
  {
    audioMotor = GetComponent<AudioSource>();
    rgb = GetComponent<Rigidbody>();
    Player = Camera.main.transform.gameObject;
  }

  void Update()
  {
    if (Vector3.Distance(Player.transform.position, transform.position) >= 30)
    {
      audioMotor.volume = Mathf.Lerp(audioMotor.volume, 0, 0.5f * Time.deltaTime);
    }
    else
    {
      audioMotor.volume = Mathf.Lerp(audioMotor.volume, 0.8f, 0.5f * Time.deltaTime);
    }
  }




}
