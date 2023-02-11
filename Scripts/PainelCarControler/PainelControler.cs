using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PainelControler : MonoBehaviour
{
    public Image imageVelocity;
	public Text TextVelocity;
    public Text textGear;
	public float Redc;
	public float MaxSpeed;
	public float RedcSpeed = 1.5f;
    public CarsTractionWheels car;
    public Rigidbody rigidbodyCar;
    public AudioSource audioSet;
	public Color[] colors;
	public float setBase = 0.3f;
	float velocity;
	float valueAp;
    int gear;
	float dataActual;

	void Start () {
		
	}
	

	void Update () {

        gear = car.actualGear;

        textGear.text = " " + gear;

		imageVelocity.fillAmount = 0.01f + (car.audioCar / 2.5f) * (0.01f + (valueAp / 2)) / (Redc * 6);
		dataActual = audioSet.pitch / Redc;
		audioSet.pitch = setBase + 0.01f + (car.audioCar / 2.5f) * (0.01f + (valueAp / 2)) / (Redc * 6);
		TextVelocity.text = " " + valueAp.ToString("f0");
		if ((dataActual * 2.5f) >= 0) {
			imageVelocity.color = Color.LerpUnclamped (imageVelocity.color, colors[0], Time.deltaTime * 3.5f);
			if ((dataActual * 2.5f) >= 0.8f) {
				imageVelocity.color = Color.LerpUnclamped (imageVelocity.color, colors[1], Time.deltaTime * 1.5f);
				if ((dataActual * 2.5f) >= 0.9f) {
					imageVelocity.color = Color.LerpUnclamped (imageVelocity.color, colors[2], Time.deltaTime * 2.5f);
					if ((dataActual * 2.5f) >= 0.92f) {
						imageVelocity.color = Color.LerpUnclamped (imageVelocity.color, colors[3], Time.deltaTime * 3.5f);
						if ((dataActual * 2.5f) >= 0.95f) {
							imageVelocity.color = Color.LerpUnclamped (imageVelocity.color, colors[4], Time.deltaTime * 5.5f);
						}
					}
				}
			}
		}

		valueAp = velocity * Redc;
		velocity = rigidbodyCar.velocity.magnitude / RedcSpeed;
	}
}
