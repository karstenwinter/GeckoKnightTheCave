using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using UnityEngine.Tilemaps;

[ScriptedImporter(1, "tmx")]
public class TilemapImporter : ScriptedImporter
{
    public float m_Scale = 1;
    public GameObject withTilemap;

    public override void OnImportAsset(AssetImportContext ctx)
    {
        var t = withTilemap.GetComponent<Tilemap>();
        var parent = new GameObject();
        string[] txt = File.ReadAllText(ctx.assetPath)
        .Replace("\r", "")
        .Split('\n');
        //var position = JsonUtility.FromJson<Vector3>(txt);
        var tilemap = parent.AddComponent<Tilemap>();
        var tilemapR = parent.AddComponent<TilemapRenderer>();
        var tilemapC = parent.AddComponent<TilemapCollider2D>();
        var y = -1;
        var x = -1;
        foreach (string line in txt)
        {
            y++;
            x = -1;
            foreach (char c in line)
            {
                x++;
                if (c != '.')
                {
                    //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    var position = new Vector3Int(x, y, 0);
                    var tile = t.GetTile(new Vector3Int(1, 1, 0));
                    tilemap.SetTile(
                         position,
                      tile);
                    //cube.name="y"+y+"x"+x;
                    //cube.transform.parent=parent.transform;
                    //  cube.transform.position = position;
                    //cube.transform.localScale = new Vector3(m_Scale, m_Scale, m_Scale);
                }
            }
        }
        // 'cube' is a a GameObject and will be automatically converted into a Prefab
        // (Only the 'Main Asset' is elligible to become a Prefab.)
        ctx.AddObjectToAsset("main obj", parent);
        ctx.SetMainObject(parent);

        var material = new Material(Shader.Find("Standard"));
        material.color = Color.red;

        // Assets must be assigned a unique identifier string consistent across imports
        ctx.AddObjectToAsset("my Material", material);

        // Assets that are not passed into the context as import outputs must be destroyed
        var tempMesh = new Mesh();
        DestroyImmediate(tempMesh);
    }
}
