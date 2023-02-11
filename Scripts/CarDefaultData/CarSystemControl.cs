using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSystemControl : MonoBehaviour
{
    public TypeTransmissionEnum typeOfTransmission;
    public float maxSteerAngle = 30;
    public float topSpeed = 220;
    public int maxGear = 5;
    public float maxRpm = 100;
    public float maxPitch = 1.5f;
    public float helperAngle;
    public float velocityKMPH;
    CarsTractionWheels carTractionWheels;


    public int automaticGear;

    void Awake()
    {
        carTractionWheels = GameObject.Find("WheelColliders").GetComponent<CarsTractionWheels>();
        if(typeOfTransmission == TypeTransmissionEnum.automatic)
        {
           automaticGear = 1;
        }
        else
        {
            automaticGear = 0;
        }
    }

    public enum TypeTransmissionEnum 
    {
        automatic,
        manual
    };

}
