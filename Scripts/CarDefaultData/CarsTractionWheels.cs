using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarsTractionWheels : MonoBehaviour
{

  public WheelCollider c_FrontWheelPassanger, c_FrontWheelDriver;
  public WheelCollider c_ReartWheelPassanger, c_RearWheelDriver;

  public Transform t_FrontWheelPassanger, t_FrontWheelDriver;
  public Transform t_ReartWheelPassanger, t_RearWheelDriver;

  public int[] reduceBox = { 0, 1, 2, 3, 4 };
  public int[] velocitysBox = { 30, 80, 120, 170, 280 };
  private float maxSteerAngle = 30;
  private float topSpeed = 220;
  public float audioCar;
  public bool isAICar;

  [HideInInspector] public CarSystemControl car;
  private InputSystemRC input;
  public bool carAIControl;
  public int indexCar = 0;

  private Rigidbody rgb;
  private float angle;
  private float steer;
  private float accel;
  private float inRect;
  private float rpm;
  private float forceHelper;
  private float helperAngle;

  public int actualGear = 1;
  private bool automaticTransmisson;
  private bool manualTransmisson;
  private float addForceTrans;
  private bool isBrake;
  private bool shift;
  private bool wait;
  private float velocityKM_H;
  public bool brake;
  private float massWheel;
  private void Awake()
  {
    car = transform.parent.GetComponent<CarSystemControl>();
    transform.gameObject.name = "WheelColliders";
    massWheel = c_FrontWheelDriver.mass;
  }

  private void Start()
  {
    if (car.automaticGear == 1)
    {
      automaticTransmisson = true;
      manualTransmisson = false;
    }
    else if (car.automaticGear == 0)
    {
      automaticTransmisson = false;
      manualTransmisson = true;
    }
    rgb = car.GetComponent<Rigidbody>();
  }

  public void Update()
  {
    if (!carAIControl)
    {
      Steer(input.SteerInput());
      Accel(input.AccelInput());
      UpdateAllPoses();
      Transmission(input.AccelInput());
      RpmControl();
    }
    Hadling();
    BrakeCondition();
  }


  public void Move(float inputA, float inputS)
  {
    Steer(inputS);
    Accel(inputA);
    UpdateAllPoses();
    Transmission(inputA);
    RpmControl();
  }

  public void Steer(float inputSteer)
  {
    if (inputSteer >= 0.1f || inputSteer <= -0.1f)
    {
      steer = Mathf.LerpAngle(0, car.maxSteerAngle, 4f);
      if (helperAngle <= 20)
      {
        angle = (steer - helperAngle) * inputSteer;
      }
    }
    else
    {
      steer = Mathf.LerpAngle(steer, 0, 1f);
      angle = (steer) * inputSteer;
    }

    rgb.angularDrag = helperAngle / 3;


    helperAngle = rgb.velocity.magnitude / 2;

    c_FrontWheelDriver.steerAngle = angle;
    c_FrontWheelPassanger.steerAngle = angle;
  }
  [HideInInspector] public bool brakeSetting;
  void BrakeCondition()
  {
    if (brakeSetting)
    {
      c_FrontWheelDriver.brakeTorque = 9000 * 2;
      c_FrontWheelPassanger.brakeTorque = 9000 * 2;
      c_ReartWheelPassanger.brakeTorque = 9000 * 2;
      c_RearWheelDriver.brakeTorque = 9000 * 2;
      isBrake = true;
    }
    else
    {
      isBrake = false;
      c_FrontWheelDriver.brakeTorque = 0;
      c_FrontWheelPassanger.brakeTorque = 0;
      c_ReartWheelPassanger.brakeTorque = 0;
      c_RearWheelDriver.brakeTorque = 0;

    }
  }

  public void Accel(float inputAccel)
  {
    car.velocityKMPH = velocityKM_H;
    if (inputAccel == 0 || (c_ReartWheelPassanger.rpm < 0 && inputAccel >= 0.1f) || (c_ReartWheelPassanger.rpm > 0 && inputAccel <= -0.1) || brake || actualGear == 0)
    {
      c_FrontWheelDriver.brakeTorque = 90000 * ((inputAccel + inputAccel) + 1);
      c_FrontWheelPassanger.brakeTorque = 90000 * ((inputAccel + inputAccel) + 1);
      c_ReartWheelPassanger.brakeTorque = 90000 * ((inputAccel + inputAccel) + 1);
      c_RearWheelDriver.brakeTorque = 90000 * ((inputAccel + inputAccel) + 1);
      isBrake = true;
    }
    else
    {
      isBrake = false;
      c_FrontWheelDriver.brakeTorque = 0;
      c_FrontWheelPassanger.brakeTorque = 0;
      c_ReartWheelPassanger.brakeTorque = 0;
      c_RearWheelDriver.brakeTorque = 0;

    }

    if (inRect <= 2)
    {
      forceHelper = 5;
    }
    else
    {
      forceHelper = 1;
    }
    if (automaticTransmisson)
    {
      if (rpm < (car.maxRpm * 4))
      {
        accel = inputAccel * (car.topSpeed * 20 * forceHelper);
      }
      else
      {
        accel = -2000;
      }
    }
    c_ReartWheelPassanger.motorTorque = accel;
    c_RearWheelDriver.motorTorque = accel;
    inRect = rgb.velocity.magnitude;
    velocityKM_H = inRect * 3;

  }

  public void Transmission(float inputAccel)
  {
    if (actualGear == 0)
    {
      rpm = 0;
    }
    else
    {

      if (automaticTransmisson)
      {
        if (isAICar)
        {
          actualGear = 1;
        }
        else
        {
          if (velocityKM_H <= velocitysBox[0])
          {
            actualGear = 1;
          }
          if (velocityKM_H <= velocitysBox[1] && velocityKM_H > velocitysBox[0])
          {
            actualGear = 2;
          }
          if (velocityKM_H <= velocitysBox[2] && velocityKM_H > velocitysBox[1])
          {
            actualGear = 3;
          }
          if (velocityKM_H <= velocitysBox[3] && velocityKM_H > velocitysBox[2])
          {
            actualGear = 4;
          }
          if (velocityKM_H <= velocitysBox[4] && velocityKM_H > velocitysBox[3])
          {
            actualGear = 5;
          }
        }
      }
      else if (manualTransmisson)
      {
        if (velocityKM_H <= velocitysBox[0])
        {
          accel = inputAccel * (car.topSpeed * 20 * forceHelper);
          addForceTrans = 0.08f;
        }
        else if (inputAccel == 1 && inRect > 5f && actualGear == 1)
        {
          accel = -2000;
        }

        if (velocityKM_H <= velocitysBox[1] && actualGear == 2)
        {
          accel = inputAccel * (car.topSpeed * 20 * forceHelper);
          addForceTrans = 0.1f;
        }
        else if (inputAccel == 1 && inRect > 5f && actualGear == 2)
        {
          accel = -2000;
        }
        if (velocityKM_H <= velocitysBox[2] && actualGear == 3)
        {
          accel = inputAccel * (car.topSpeed * 20 * forceHelper);
          addForceTrans = 0.15f;
        }
        else if (inputAccel == 1 && inRect > 5f && actualGear == 3)
        {
          accel = -2000;
        }
        if (velocityKM_H <= velocitysBox[3] && actualGear == 4)
        {
          accel = inputAccel * (car.topSpeed * 20 * forceHelper);
          addForceTrans = 0.2f;
        }
        else if (inputAccel == 1 && inRect > 5f && actualGear == 4)
        {
          accel = -2000;
        }
        if (velocityKM_H <= velocitysBox[4] && actualGear == 5)
        {
          accel = inputAccel * (car.topSpeed * 20 * forceHelper);
          addForceTrans = 0.2f;
        }
        else if (inputAccel == 1 && inRect > 5f && actualGear == 5)
        {
          accel = -2000;
        }
      }

      if (actualGear >= car.maxGear)
      {
        actualGear = car.maxGear;
      }

      if (actualGear <= 0)
      {
        actualGear = 0;
      }
      /*
            if (manualTransmisson && input.TransmissionInput() == 1 && !wait)
            {
              StartCoroutine("AddGear");
            }
            if (manualTransmisson && input.TransmissionInput() == -1 && !wait)
            {
              StartCoroutine("ReduceGear");
            }*/
    }
  }

  void RpmControl()
  {
    if (!isBrake)
    {
      if (rpm < 100)
      {
        rpm = inRect * 15;
      }
      else
      {
        rpm = inRect * 15;
      }

    }
    else
    {
      rpm = inRect * 15;
    }

    if (manualTransmisson)
    {
      if (audioCar <= car.maxPitch || actualGear >= 1 && !wait)
      {
        audioCar = 0.3f + (((rpm - 3) / reduceBox[actualGear]) / 100) * 5;
      }
      else if (actualGear >= 1 && !wait)
      {
        audioCar = 0.3f + (((rpm - 3) / reduceBox[actualGear]) / 100) * 5 - addForceTrans;
      }

      if (actualGear == 0)
      {
        audioCar = 0.3f + input.AccelInput();
      }
    }
    else
    {
      audioCar = 0.3f + (((rpm - 3) / reduceBox[actualGear]) / 100) * 5;
    }
  }

  IEnumerator AddGear()
  {
    yield return new WaitForSeconds(0);
    if (!wait)
    {
      actualGear++;
      wait = true;
    }
    audioCar -= 0.3f;
    yield return new WaitForSeconds(1);
    wait = false;
  }
  IEnumerator ReduceGear()
  {
    yield return new WaitForSeconds(0);
    if (!wait)
    {
      actualGear--;
      wait = true;
    }
    yield return new WaitForSeconds(1);
    wait = false;
  }

  void UpdateAllPoses()
  {
    updatePose(c_FrontWheelDriver, t_FrontWheelDriver);
    updatePose(c_FrontWheelPassanger, t_FrontWheelPassanger);
    updatePose(c_ReartWheelPassanger, t_ReartWheelPassanger);
    updatePose(c_RearWheelDriver, t_RearWheelDriver);
  }

  void updatePose(WheelCollider collider, Transform transform)
  {
    Vector3 pos;
    Quaternion quat;

    collider.GetWorldPose(out pos, out quat);

    transform.position = pos;
    transform.rotation = quat;

  }

  void Hadling()
  {
    if (!c_FrontWheelDriver.isGrounded)
    {
      c_FrontWheelDriver.mass = massWheel * 800;
    }
    else
    {
      c_FrontWheelDriver.mass = massWheel;
    }
    if (!c_FrontWheelPassanger.isGrounded)
    {
      c_FrontWheelPassanger.mass = massWheel * 800;
    }
    else
    {
      c_FrontWheelPassanger.mass = massWheel;
    }
    if (!c_ReartWheelPassanger.isGrounded)
    {
      c_ReartWheelPassanger.mass = massWheel * 800;
    }
    else
    {
      c_ReartWheelPassanger.mass = massWheel;
    }
    if (!c_RearWheelDriver.isGrounded)
    {
      c_RearWheelDriver.mass = massWheel * 800;
    }
    else
    {
      c_RearWheelDriver.mass = massWheel;
    }
  }







}
