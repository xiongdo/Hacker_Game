using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MachineTileCreator
{
    private const string assetPath = "Assets/TilePlate/Special Type";
    
    [MenuItem("Tile/Generate Machine tiles")]
    public static void CreateMachineTile()
    {
        foreach (var o in Selection.objects)
        {
            var sprite = (Sprite) o;
            if (sprite)
            {
                var tileScriptable = ScriptableObject.CreateInstance<MachineTileScriptable>();
                tileScriptable.sprite = sprite;
                tileScriptable._type = TileType.Unsafe;
                tileScriptable.color = Color.white;
                //检查保存路径
                if (!Directory.Exists(assetPath))
                    Directory.CreateDirectory(assetPath);
 
                //删除原有文件，生成新文件
                string fullPath = assetPath + "/" + sprite.name + ".asset";
                AssetDatabase.DeleteAsset(fullPath);
                AssetDatabase.CreateAsset(tileScriptable, fullPath);
                AssetDatabase.Refresh();
            }
        }
    }
}

[CustomEditor(typeof(MachineTileScriptable))]
public class MachineTileEditor : Editor
{
    private MachineTileScriptable _tile;

    private SerializedProperty type;

    private SerializedProperty sprite;

    private SerializedProperty color;

    private void OnEnable()
    {
        _tile = (MachineTileScriptable)target;
        type = serializedObject.FindProperty("_type");
        sprite = serializedObject.FindProperty("m_Sprite");
        color = serializedObject.FindProperty("m_Color");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(type);
        EditorGUILayout.PropertyField(sprite);
        EditorGUILayout.PropertyField(color);
        
        // _tile._type = (TileType)EditorGUILayout.EnumPopup("Tile Type", TileType.Unsafe);

        // _tile.sprite = 
        //     (Sprite) EditorGUILayout.ObjectField(
        //         "Sprite", _tile.sprite, typeof(Sprite), false
        //     );
        //
        // _tile.color = EditorGUILayout.ColorField("Color", Color.white);

        serializedObject.ApplyModifiedProperties();
    }
}
