using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioTools
{
	public class TerrainMapper_NodeGroupToCamera_Curve : TerrainMaper_NodeGroupToCamera
	{
		[SerializeField] private AnimationCurve att_curve;
		[SerializeField] private float currentVol;
		[SerializeField] private float nextVolumeTarget;

		[Header("Curve Settings")]
		public float minDistance, maxDistance;
		[Header("Audio extra Settings")]
		[Range(-80f, 0)] public float initVolume;
		public float minVolume = -80f;
		public float maxVolume = 20f;
		public AudioMixer audioMixer;
		public float volumeSmoother;
		private float deltaTime;
		public float error_threshold;
		private void OnEnable()
		{
			currentVol = initVolume;
		}
		private void LateUpdate()
		{
            if (!Initialized)
            {
				return;
            }

            deltaTime = Time.deltaTime;

			if (nextVolumeTarget > currentVol)
			{
				UpdateVolume(deltaTime, 1);
			}
			else if (nextVolumeTarget < currentVol)
			{
				UpdateVolume(deltaTime, -1);
			}
		}

		private float CurveInterpolation()
		{
			float lerpVolume = math.abs((-1)/(maxDistance-minDistance)*(closestDistance - minDistance));
			nextVolumeTarget = att_curve.Evaluate(lerpVolume) * minVolume;
			return nextVolumeTarget;
		}
		public override void UpdateSourcePosition(Transform position)
		{
			print( "Final Vol: " + CurveInterpolation().ToString());
		}
		public void UpdateVolume(float _deltaTime, int sign)
		{
			currentVol += deltaTime * volumeSmoother * sign;
			if((currentVol > (nextVolumeTarget - error_threshold)) && (currentVol < (nextVolumeTarget + error_threshold ))) //Prevent Volume Blinking within the error range
			{
				return ;
			}
			audioMixer.SetFloat("Forest_Vol", currentVol);
			print("Updating Volume");
		}
	}
}
