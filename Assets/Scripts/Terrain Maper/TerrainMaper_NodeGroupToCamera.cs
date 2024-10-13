using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

namespace AudioTools
{
	[RequireComponent(typeof(TerrainMaper))]

	public class TerrainMaper_NodeGroupToCamera : MonoBehaviour
	{
		[Header("Settings")]
		public TerrainMaper_Keys.TerrainNode TerrainType;
		// Audio
		public AudioSource audioSource;
		protected Camera Camera;
		public float refreshTime;
		protected List<TerrainMaper_Node> nodes;
		protected float closestDistance;
		protected TerrainMaper_Node closestNode;
		protected bool Initialized = false;
		[SerializeField] private float refreshCount;
		public void InitCalculation()
		{
			closestDistance = 0;
			Camera = Camera.main;
			nodes = GetComponent<TerrainMaper>().Get_NodeGroup(TerrainType);
			refreshCount = refreshTime;
			Initialized = true;
			closestNode = nodes[0];
		}
		private void Update()
		{
			if (!Initialized)
			{
				return;
			}
			refreshCount += Time.deltaTime;
			if (refreshCount >= refreshTime)
			{
				Get_ClosestNode();
				refreshCount = 0;
			}
		}
		private void Get_ClosestNode()
		{
			float _distanceToCam = 0;
			closestDistance = 0; // To force Recalculate at the end of the loop
			foreach (var node in nodes)
			{
				 _distanceToCam = Vector3.Distance(Camera.transform.position, node.transform.position);
				if(closestDistance == 0 || _distanceToCam < closestDistance)
				{
					closestDistance = _distanceToCam;
					print("Closest Distance: " + closestDistance.ToString());
					closestNode = node;
				}
			}
			if(audioSource != null)
			{
				UpdateSourcePosition(closestNode.transform);
			}
		}
		public virtual void UpdateSourcePosition(Transform newPosition)
		{
			audioSource.transform.position = newPosition.position;
		}
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			if (closestNode != null)
			{
				Handles.DrawLine(Camera.transform.position, closestNode.transform.position);
				Vector3 midPoint = Camera.transform.position + closestNode.transform.position;
				Handles.Label(midPoint/2 ,"Current Distance:" + closestDistance);
				Gizmos.DrawSphere(closestNode.transform.position, 0.5f);
			}
		}
	}
}


