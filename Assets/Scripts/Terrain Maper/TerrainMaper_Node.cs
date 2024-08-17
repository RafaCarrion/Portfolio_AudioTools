using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
namespace AudioTools
{
	public class TerrainMaper_Node : MonoBehaviour
	{
		private float height;
		private bool debugActive;
		private Vector3 worldPosition;
		private float raycastLength;
		private string terrainDetected;
		public TerrainMaper_Keys.TerrainNode terrainDetected_Filtered;
		//DEBUG and Draw Gizmos Properties 
		public float debug_sphereRadius = 0.2f;
		public Vector2Int nodeIndex;
		public int groupId = 0 ;
		private bool show_collisionNames = false;
		public TerrainMaper_Keys.TerrainNode filterTerrainType;

		public void Set_NodeValues(float _height, bool _debugActive, Vector2Int _nodeIndex, Vector3 _worldPosition, float _raycastLength)
		{
			height = _height;
			debugActive = _debugActive;
			nodeIndex = _nodeIndex;
			worldPosition = _worldPosition;
			raycastLength = _raycastLength;
			terrainDetected = "unknown";
			terrainDetected_Filtered = new TerrainMaper_Keys.TerrainNode();
		}
		public TerrainMaper_Keys.TerrainNode Get_FilteredTerrain()
		{
			return terrainDetected_Filtered;
		}
		public void ScanNode()
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) * raycastLength, out hit))
			{
				transform.position = hit.point;
				if(hit.collider != null) EvaluateHit(hit.collider);
			}
			else
			{
				Destroy(gameObject);
			}
		}
		private void EvaluateHit(Collider collider) //Hacer esto con tags en lugar de Nombres? Crear una clase que devuelva un string
		{
			string colliderName = collider.name;
			//print("Node evaluation: " + colliderName);
			//print("Terrain detected: " + terrainDetected.ToString());
			try
			{
				terrainDetected_Filtered = collider.GetComponent<TerrainMaper_NodeClasifier>().TerrainNode;
			}
			catch
			{
				terrainDetected_Filtered = TerrainMaper_Keys.TerrainNode.Undefined;
			}
		}

		//Debuging Methods
		public void DisplayTerrainFilter(TerrainMaper_Keys.TerrainNode filter)
		{
			filterTerrainType = filter;
		}
		public void AlternateDebugDisplayName()
		{
			show_collisionNames = !show_collisionNames;
		}
		private void OnDrawGizmos()
		{
			if (!debugActive) { return; }
			
			//Format
			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.black;
			Color color = new Color();
			switch (terrainDetected_Filtered.ToString())
			{
				case "Cliff":
					color = Color.green;					
					break;
				case "Terrain":
					color = Color.grey;
					break;
			}
			//Apply colors
			style.normal.textColor = Color.white;
			Handles.color = color;
			Gizmos.color = color;

			Gizmos.DrawSphere(transform.position, debug_sphereRadius);

			// Display Collision Label
			if (filterTerrainType != terrainDetected_Filtered) return;
			if (show_collisionNames)
			{
				Handles.Label(transform.position + new Vector3(0, 1, 0), "Node: " + nodeIndex.x.ToString() 
					+ ":" + nodeIndex.y.ToString() + "\n"
					+ "Group Id: " + groupId.ToString() + "\n" 
					+ terrainDetected_Filtered.ToString(), style);
			}
			else
			{
				Handles.Label(transform.position + new Vector3(0, 1, 0), "Node: " + nodeIndex.x.ToString() + ":" + nodeIndex.y.ToString() + "\n" + terrainDetected_Filtered.ToString(), style);
			}
			
		}
	}
}

