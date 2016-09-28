using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer {

	float prevActivationTime;
	float cooldownTime;

	public Timer(float c) {
		cooldownTime = c;
		prevActivationTime = -c;
	}
	public float TimeLeft() { return cooldownTime - (Time.time - prevActivationTime); }
	public bool IsOffCooldown() { return Time.time - prevActivationTime > cooldownTime; }
	public float CooldownTime { get { return cooldownTime; } set { cooldownTime = value; } }
	public void Reset() { prevActivationTime = Time.time; }
}