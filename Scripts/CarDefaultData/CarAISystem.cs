using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAISystem : MonoBehaviour
{
  public typeCar TypeOfCar = typeCar.UrbanCar;
  public Transform directionSensor;
  public Transform target;
  public Transform targetSet;
  CarsTractionWheels carTractionWheels;
  CarSystemControl car;
  TargetBase targetSensor;

  public float accel;
  public float steer;
  public float sensorData;
  public float sensorDistance;
  public float directionCar = 1;
  public float valueDirection;
  public bool onSteer;
  public bool direction = true;
  public bool wait;
  public bool driveRear;
  public float steerD;
  public float steerC;
  public float influenceAdd;
  public float stopTime = 10;
  ProgressTracker tracker;
  public bool stop;
  public bool obstacle;
  public CollisionDetection detection;
  private void Awake()
  {
    carTractionWheels = gameObject.GetComponentInChildren<CarsTractionWheels>();
    carTractionWheels.carAIControl = true;
    targetSensor = GetComponent<TargetBase>();
    car = GetComponent<CarSystemControl>();
    m_Rigidbody = GetComponent<Rigidbody>();
    tracker = GetComponent<ProgressTracker>();
  }


  void Update()
  {
    SensorDetect();
  }


  void SensorDetect()
  {
    directionSensor.LookAt(target);
    sensorDistance = Vector3.Distance(transform.position, target.position);
    ConditionOfAcceleration();
    SteerControl();
  }

  private void SteerControl()
  {
    if ((steerD > 0.4f || steerD < -0.4) && car.velocityKMPH > 20)
    {
      if (sensorDistance > 20)
      {
        influenceAdd = 3;
      }
      else
      {
        influenceAdd = 2;
      }
      onSteer = true;
      carTractionWheels.brake = true;
    }
    else
    {
      influenceAdd = 1;
      onSteer = false;
      carTractionWheels.brake = false;
    }
  }

  private void ConditionOfAcceleration()
  {
    if (!stop && !obstacle)
    {
      if (!driveRear)
      {
        if ((steerD > 0.1f || steerD < -0.1) || tracker.speed >= 6 || (targetSensor.direction[1] == 1 || targetSensor.direction[0] == 1))
        {
          if (tracker.speed >= 9)
          {
            accel = 0.0001f;
          }
          else
          {
            accel = 0.07f;
          }
        }
        else
        {
          accel = 0.3f;
        }
      }
      else
      {
        Debug.Log("In This Case3");
        if (driveRear)
        {
          accel = -0.6f;
        }
        else
        {
          accel = 0;
        }
      }
    }
    else
    {
      accel = 0;
    }

    if (accel > 0 && car.velocityKMPH < 1 && cd && (targetSensor.direction[1] > 0 || targetSensor.direction[0] > 0) && !ht)
    {
      StartCoroutine("DriveRear");
      ht = true;
    }


    if (obstacle && !desBrake)
    {
      StartCoroutine("DesativeO");
      desBrake = true;
    }
  }
  bool desBrake;
  bool ht;
  IEnumerator DesativeO()
  {
    yield return new WaitForSeconds(5);
    obstacle = false;
    yield return new WaitForSeconds(5);
    desBrake = false;
  }

  IEnumerator DriveRear()
  {
    yield return new WaitForSeconds(0);
    driveRear = true;
    yield return new WaitForSeconds(0.3f);
    driveRear = false;
    yield return new WaitForSeconds(2f);
    ht = false;
  }

  bool waitS;

  IEnumerator Wait()
  {
    yield return new WaitForSeconds(3);
    wait = true;
    yield return new WaitForSeconds(3);
    wait = false;
    waitS = false;
  }

  private void OnTriggerEnter(Collider other)
  {
    if ((other.gameObject.tag == "RCSystemAI" || other.gameObject.tag == "RCSystem") && detection.have)
    {
      obstacle = true;
      Debug.Log("St");
    }
  }
  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag == "RCSystemAI" || other.gameObject.tag == "RCSystem")
    {
      obstacle = false;
    }
  }

  public enum typeCar
  {
    SportCar,
    UrbanCar
  };

  [SerializeField][Range(0, 180)] private float m_CautiousMaxAngle = 50f;
  [SerializeField] private float m_SteerSensitivity = 0.05f;
  [SerializeField] private float m_LateralWanderDistance = 3f;
  [SerializeField] private float m_LateralWanderSpeed = 0.1f;
  [SerializeField] private Transform m_Target;
  [SerializeField] private float m_ReachTargetThreshold = 2;

  private float m_RandomPerlin;

  private float m_AvoidOtherCarTime;
  private float m_AvoidOtherCarSlowdown;
  private float m_AvoidPathOffset;
  private Rigidbody m_Rigidbody;



  private void FixedUpdate()
  {
    TargetAlignment();
  }

  private void TargetAlignment()
  {
    if (m_Target == null)
    {

    }
    else
    {
      Vector3 fwd = transform.forward;

      fwd = m_Rigidbody.velocity;


      Vector3 offsetTargetPos = m_Target.position;

      if (Time.time < m_AvoidOtherCarTime)
      {
        offsetTargetPos += m_Target.right * m_AvoidPathOffset;
      }
      else
      {
        offsetTargetPos += m_Target.right *
                           (Mathf.PerlinNoise(Time.time * m_LateralWanderSpeed, m_RandomPerlin) * 2 - 1) *
                           m_LateralWanderDistance;
      }
      Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);
      float targetAngle = (Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg) * influenceAdd;
      float steer = Mathf.Clamp(targetAngle * m_SteerSensitivity, -1, 1) * Mathf.Sign(car.velocityKMPH);

      if (driveRear || ht)
      {
        steerC = (targetSensor.direction[1] + targetSensor.direction[0]) * -1;
        carTractionWheels.Move(accel, steerC);
      }
      else
      {
        carTractionWheels.Move(accel, steer);
        steerD = steer;
      }
    }
  }

  public void SetTarget(Transform target)
  {
    m_Target = target;
  }

  bool cd;
  private void OnCollisionStay(Collision other)
  {
    if ((targetSensor.direction[3] == 0 || targetSensor.direction[2] == 0) && car.velocityKMPH <= 0.01f && TypeOfCar == typeCar.SportCar)
    {
      driveRear = true;
    }
    if (TypeOfCar == typeCar.UrbanCar && !cd && other.gameObject.tag == "RCSystem")
    {
      StartCoroutine("StopCar");
      finalTime = stopTime;
      cd = true;
    }
    else if (TypeOfCar == typeCar.UrbanCar && !cd)
    {
      StartCoroutine("StopCar");
      finalTime = stopTime / 2.5f;
      cd = true;
    }
  }
  public float finalTime;
  IEnumerator StopCar()
  {
    yield return new WaitForSeconds(0.1f);
    stop = true;
    yield return new WaitForSeconds(finalTime);
    stop = false;
    yield return new WaitForSeconds(5f);
    cd = false;
  }

  private void OnCollisionExit(Collision other)
  {
    StartCoroutine("Desative");
  }

  IEnumerator Desative()
  {
    yield return new WaitForSeconds(2);
    driveRear = false;
  }
}
