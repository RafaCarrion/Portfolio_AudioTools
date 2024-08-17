using AudioTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.Rendering.DebugUI.MessageBox;

namespace AudioTools
{
	public class TerrainMaper_NodeGroup : MonoBehaviour
	{
		private int groupID;
		private List<TerrainMaper_Node> nodes;

		public TerrainMaper_NodeGroup(int _groupId)
		{
			groupID = _groupId;
			nodes = new List<TerrainMaper_Node>();
		}

		public void AddNodeToGroup(TerrainMaper_Node newNode)
		{
			try
			{
				nodes.Add(newNode);
			}
			catch
			{
				Debug.LogWarning("Node error: adding to group list, Not existing list");
			}
		}
	}
}

