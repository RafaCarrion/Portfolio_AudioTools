using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UIElements;
using AudioTools;

[CustomEditor(typeof(TerrainMaper))]
public class TerrainMaper_Editor : Editor
{

	SerializedProperty terrainTypeProp;
	private void OnEnable()
	{
		terrainTypeProp = serializedObject.FindProperty("filterNode");

	}
	public override void OnInspectorGUI()
	{
		TerrainMaper terrMaper = (TerrainMaper)target;

		serializedObject.Update();

		if (GUILayout.Button("Scan Terrain"))
		{
			terrMaper.Create_Matrix();
		}
		if (GUILayout.Button("Scan Node Groups"))
		{
			terrMaper.Scan_NodeGroups();
		}
		if (GUILayout.Button("Display collision label"))
		{
			terrMaper.AlternateCollisionNames();
		}
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Show Only :"))
		{
			terrMaper.SetTerrainDisplayFilter();
		}
		EditorGUILayout.PropertyField(terrainTypeProp);
		serializedObject.ApplyModifiedProperties();

		GUILayout.EndHorizontal();
		base.OnInspectorGUI();
	}
}
