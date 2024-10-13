using AudioTools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainMapper_NodeGroupToCamera_Curve))]
public class TerrainMaper_NodeGroupToCamera_curve_Editor : TerrainMaper_NodeGroupToCamera_Editor
{
	public override void OnInspectorGUI()
	{
		TerrainMapper_NodeGroupToCamera_Curve targetClass = (TerrainMapper_NodeGroupToCamera_Curve)target;
		base.OnInspectorGUI();
	}
}
