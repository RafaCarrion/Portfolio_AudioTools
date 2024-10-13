using AudioTools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainMaper_NodeGroupToCamera), true)]
public class TerrainMaper_NodeGroupToCamera_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		TerrainMaper_NodeGroupToCamera targetClass = (TerrainMaper_NodeGroupToCamera)target;
		
		if (GUILayout.Button("Init calculation"))
		{
			targetClass.InitCalculation();
		}
		base.OnInspectorGUI();
	}

}
