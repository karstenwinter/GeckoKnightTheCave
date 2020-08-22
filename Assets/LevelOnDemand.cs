using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using UnityEngine.Tilemaps;

//[ScriptedImporter(1, "tmx")]
public class LevelOnDemand : MonoBehaviour
{
    //public float m_Scale = 1;
    //public GameObject withTilemap;
    //public int offset, offset2;
    //public bool dummy;

    public void Start()
    {
        Render(transform.position, "", null);
    }

    public void Render(Vector3 pos, string layer, Tilemap target)
    {
        /*
        string[] txt = File.ReadAllText()
        .Replace("\r", "")
        .Split('\n');
        
        var tilemap = parent.AddComponent<Tilemap>();
        var tilemapR = parent.AddComponent<TilemapRenderer>();
        var tilemapC = parent.AddComponent<TilemapCollider2D>();
        var y = -1;
        foreach (string line in txt)
        {
            y++;
            int x = -1;
            foreach (string c in line.Split(','))
            {
                x++;
                if (c != "" && c != "0")
                {
                    //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    var position = new Vector3Int(x, -y, 0);
                    var value = int.Parse(c);
                    //if (value < 10)
                    //{
                    ///}
                    var tx = value % 8;
                    var ty = value / 8;
                    Debug.Log("y" + y + "x" + x + ": v" + value + "=> ty" + ty + "tx" + tx);
                    //tx -= 1;
                    var tile = t.GetTile(new Vector3Int(tx - 1, -ty, 0));
                    tilemap.SetTile(position, tile);
                    //cube.name="y"+y+"x"+x;
                    //cube.transform.parent=parent.transform;
                    //  cube.transform.position = position;
                    //cube.transform.localScale = new Vector3(m_Scale, m_Scale, m_Scale);
                }
            }
        }

        //var tile2 = t.GetTile(new Vector3Int(5, 5, 0));
        //tilemap.SetTile(new Vector3Int(offset, offset2, 0), tile2);

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
        DestroyImmediate(tempMesh);*/
    }
}
