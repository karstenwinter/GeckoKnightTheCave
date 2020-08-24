using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using UnityEngine.Tilemaps;

[ScriptedImporter(1, "tmx")]
public class TilemapImporter : ScriptedImporter
{
    public float m_Scale = 1;
    public GameObject withTilemap;
    public int startX, startY, width, height;
    public bool dummy;
    public PhysicsMaterial2D phsyMaterial;
    public Material renderMaterial;

    public override void OnImportAsset(AssetImportContext ctx)
    {
        var t = withTilemap.GetComponent<Tilemap>();
        var parent = new GameObject();
        var layerText = File.ReadAllText(ctx.assetPath)
        .Replace("\r", "")
        .Split('\n');
        string[] txt = layerText;

        var tilemap = parent.AddComponent<Tilemap>();
        var tilemapR = parent.AddComponent<TilemapRenderer>();
        if (renderMaterial != null)
        {
            tilemapR.material = renderMaterial;
        }

        if (phsyMaterial != null)
        {
            var tilemapC =
            parent.AddComponent<TilemapCollider2D>();
            tilemapC.sharedMaterial = phsyMaterial;
        }
        var y = -1;
        foreach (string line in txt)
        {
            y++;
            int x = -1;
            foreach (string c in line.Split(','))
            {
                x++;
                if (startX <= x && x <= width && startY <= y && y <= height && c != "" && c != "0")
                {
                    //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    var position = new Vector3Int(x, -y, 0);
                    long value;
                    if (!long.TryParse(c, out value))
                    {
                        Debug.LogError("c => error " + c);
                    }
                    //if (value < 10)
                    //{
                    ///}
                    var tx = value == 8 ? 7 : value % 8 == 0 ? (value % 8) - 1 : (value % 8) - 1;
                    var ty = value % 8 == 0 ? -(value / 8) + 1 : -(value / 8);
                    /*if (value < 9)
                    {
                        value--;
                    }*/

                    /*if (value > 9)
                    {
                        ty += offset;
                    }
                    else
                    {
                        value--;
                        ty += offset2;
                    }*/
                    //Debug.Log("y" + y + "x" + x + ": v" + value + "=> ty" + ty + "tx" + tx);
                    //tx -= 1;
                    var tile = t.GetTile(
                    new Vector3Int((int)tx, (int)ty, 0));
                    tilemap.SetTile(position
                    - new Vector3Int(startX, startY, 0), tile);
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
        DestroyImmediate(tempMesh);
    }
}
