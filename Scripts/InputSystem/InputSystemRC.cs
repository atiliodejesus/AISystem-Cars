using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystemRC : MonoBehaviour
{
    public string inputAccelP = "up", inputAccelN = "down";
    public string alt_inputAccelP = "w", alt_inputAccelN = "s";
    public float influentAccel;
    public string inputSteerP = "right", inputSteerN = "left";
    public string alt_inputSteerP = "d", alt_inputSteerN = "a";
    public float influentSteer;
    public float steerAngleInput;
    public string inputTransmissonP = "rightShift", alt_inputTransmissonP = "z";
    public string inputTransmissonN = "leftShift", alt_inputTransmissonN = "x";
   


  
    float AccelInputP()
    {
        if(Input.GetKey(inputAccelP) || Input.GetKey(alt_inputAccelP))
        {
            return Mathf.Lerp(0, 1, 5f);
        }
        else
        {
            return 0;
        }
    }

    float AccelInputN()
    {
        if (Input.GetKey(inputAccelN) || Input.GetKey(alt_inputAccelN))
        {
            return Mathf.Lerp(0, -1, 5f);
        }
        else
        {
            return 0;
        }
    }
    public float AccelInput()
    {
        return AccelInputP() + AccelInputN(); 
    } 

    float SteerInputP()
    {
        if(Input.GetKey(inputSteerP) || Input.GetKey(alt_inputSteerP))
        {
            return Mathf.Lerp(0, 1, 5f);
        }
        else
        {
            return 0;
        }
    }

    float SteerInputN()
    {
        if (Input.GetKey(inputSteerN) || Input.GetKey(alt_inputSteerN))
        {
            return Mathf.Lerp(0, -1, 5f);
        }
        else
        {
            return 0;
        }
    }
    
    float GearInputP()
    {
        if (Input.GetKey(inputTransmissonP) || Input.GetKey(alt_inputTransmissonP))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    float GearInputN()
    {
        if (Input.GetKey(inputTransmissonN) || Input.GetKey(alt_inputTransmissonN))
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public float TransmissionInput()
    {
       return GearInputN() + GearInputP();
    }
    
    public float SteerInput()
    {
       return SteerInputN() + SteerInputP();
    } 

    

}
