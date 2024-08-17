using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  AudioTools
{

	//This class gets asked by the node, it returns the terrain
	public class TerrainMaper_NodeClasifier : MonoBehaviour
	{
		[SerializeField] private TerrainMaper_Keys.TerrainNode terrainNode = new TerrainMaper_Keys.TerrainNode();
		public TerrainMaper_Keys.TerrainNode TerrainNode { get { return terrainNode; } }
	}
}

