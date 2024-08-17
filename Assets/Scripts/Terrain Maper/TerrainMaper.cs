using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
//TO DO:
// Filtrar los nodos, sacar los bordes de los cliffs y los rios (pintarlos de colores)
// Hacer que la camara se mueva en esos bordes detectados
// Puede ser una tool para settear splines en rios y bordes y tal
namespace AudioTools
{
	[System.Serializable]
	public class TerrainMaper : MonoBehaviour
	{
		//private List<TerrainMaper_Node> terrainNodes;
		private Dictionary<Vector2Int, TerrainMaper_Node> terrainNodes_dict = new Dictionary<Vector2Int, TerrainMaper_Node>(); //Dictionari <MatrixIndex, node>
		private Dictionary<int, List<TerrainMaper_Node>> terrainGroups = new Dictionary<int, List<TerrainMaper_Node>>(); //Contains important groups of nodes  
																														 //Matrix Sizes
		[Header("Matrix Settings")]
		public Vector2 matrixSize;
		public float matrixHeight;
		public float matrixMaxHeightLength;
		public float matrixNodeDistance;
		public Vector3 matrixWorldOrigin;
		private Vector3 matrixOriginOffset;
		[Header("Group Node Settings")]
		public int neighbours_Threshold; //Determines the number of same neighbours node to be part of a group
		[Header("Debug Options")]
		public bool debug_Visual;
		[HideInInspector] public TerrainMaper_Keys.TerrainNode filterNode;
		//Matrix Creator
		public void Create_Matrix()
		{
			//terrainNodes = new List<TerrainMaper_Node>();
			Debug.Log("Creating Matrix");
			matrixOriginOffset = CalculateOffset();

			for (int i = 0; i < matrixSize.x; i++)  //ROW
			{
				for (int z = 0; z < matrixSize.y; z++)  //COLUMN
				{
					GameObject node_go = new GameObject();
					node_go.name = "Node: " + i.ToString() + ":" + z.ToString();
					node_go.transform.parent = transform;
					TerrainMaper_Node terrainNode =	node_go.AddComponent<TerrainMaper_Node>();
					terrainNode.nodeIndex = new Vector2Int(i, z);
					float nodeXposition = matrixNodeDistance * i;
					float nodeYposition = matrixNodeDistance * z;

					node_go.transform.position = Get_NodeWorldPos(i, z); //Mover el nodo a la posicion
					node_go.GetComponent<TerrainMaper_Node>().Set_NodeValues(matrixHeight, debug_Visual, new Vector2Int(i, z), Get_NodeWorldPos(i, z), matrixMaxHeightLength);
					node_go.GetComponent<TerrainMaper_Node>().ScanNode();
					if (node_go != null)
					{
						//terrainNodes.Add(node_go.GetComponent<TerrainMaper_Node>());
						terrainNodes_dict.Add(terrainNode.nodeIndex, terrainNode);
					}
				}
			}
		}
		private Vector3 CalculateOffset()
		{
			float xOffset = matrixSize.x / 2;
			float zOffset = matrixSize.y / 2;
			Vector3 offset = new Vector3(transform.position.x - xOffset * matrixNodeDistance, transform.position.y - matrixHeight, transform.position.z - zOffset * matrixNodeDistance);
			return offset;
		}
		private Vector3 Get_NodeWorldPos(int indexI, int indexZ)
		{
			Vector3 worldPos = new Vector3(matrixNodeDistance * indexI + matrixOriginOffset.x, matrixHeight, matrixNodeDistance * indexZ + matrixOriginOffset.z);
			return worldPos;
		}

		//Node Clasification
		public void Scan_NodeGroups()
		{
			Vector2Int[] neighbourd_coordinates =
				{
				new Vector2Int(-1, 1),
				new Vector2Int(-1, 0),
				new Vector2Int(-1, -1),
				new Vector2Int(0, -1),
				new Vector2Int(0, 1),
				new Vector2Int(1, -1),
				new Vector2Int(1, 0),
				new Vector2Int(1, 1),
			};

			int groupId = 1; //Start from 1, if is 0 I won�t display on the debug
			TerrainMaper_Node tempNode;
			

			foreach (KeyValuePair<Vector2Int, TerrainMaper_Node> node in terrainNodes_dict) //Each node of matrix
			{
				int neighbourd_Success = 0;
				TerrainMaper_Keys.TerrainNode evaluationKey = node.Value.Get_FilteredTerrain();

				foreach (Vector2Int coord in neighbourd_coordinates) //Loop Neighbours of node
				{
					Vector2Int evaluatedIndex = new Vector2Int(coord.x + node.Key.x, coord.y + node.Key.y);
					print("Evaluating Index: " + evaluatedIndex.ToString());
					terrainNodes_dict.TryGetValue(evaluatedIndex, out tempNode);
					try
					{
						
					}
					catch
					{
						continue;
					}

					if(tempNode !=null && tempNode.Get_FilteredTerrain() == evaluationKey) //Same Negihbour
					{
						neighbourd_Success++;
					}
				}

				//After loop neighbourds, is part of a group? AHORA MISMO NO TIENE SENTIDO hacer Groups, es como hacer un filtro
				if (neighbourd_Success >= neighbours_Threshold)
				{
					AddNodeToGroup(node.Value, groupId);
					node.Value.groupId = groupId;
					groupId++; //Generates new group Id
				}

			}
		}
		private void AddNodeToGroup(TerrainMaper_Node newNode, int groupID)
		{
			if (!terrainGroups.ContainsKey(groupID)) //Is in dictionary?
			{
				terrainGroups.Add(groupID, new List<TerrainMaper_Node>());
			}
			//Add node to list by GroupID
			try
			{
				terrainGroups.TryGetValue(groupID, out List<TerrainMaper_Node> nodeGroup);
				if (nodeGroup == null) { nodeGroup = new List<TerrainMaper_Node>(); }
				nodeGroup.Add(newNode);
			}
			catch
			{
				Debug.LogWarning("Error trying to add node to groupList");
			}
		}
		//Debug options
		public void AlternateCollisionNames()
		{
			foreach (KeyValuePair<Vector2Int, TerrainMaper_Node> node in terrainNodes_dict)
			{
				node.Value.AlternateDebugDisplayName();
			}
		}
		public void SetTerrainDisplayFilter()
		{
			foreach (KeyValuePair<Vector2Int, TerrainMaper_Node> node in terrainNodes_dict)
			{
				node.Value.DisplayTerrainFilter(filterNode);
			}
		}

	}

}
