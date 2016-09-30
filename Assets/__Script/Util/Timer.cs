using UnityEngine;
using System.Collections;

public class Timer {

	float prevActivationTime;
	float cooldownTime;

	public Timer(float c) {
		cooldownTime = c;
		prevActivationTime = -c;
	}
	public float PercentTimePassed { get { return TimePassed / cooldownTime; } }
	public float PercentTimeLeft { get { return TimeLeft / cooldownTime; } }
	public float TimeLeft { get { return cooldownTime - Mathf.Min(Time.time - prevActivationTime, cooldownTime); } }
	public float TimePassed { get { return Mathf.Min(Time.time - prevActivationTime, cooldownTime); } }
	public bool IsOffCooldown { get { return Time.time - prevActivationTime > cooldownTime; } }
	public float CooldownTime { get { return cooldownTime; } set { cooldownTime = value; } }
	public void Reset() { prevActivationTime = Time.time; }

}