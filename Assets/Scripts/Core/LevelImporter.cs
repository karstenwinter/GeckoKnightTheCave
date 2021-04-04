#if UNITY_EDITOR
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

[System.Serializable]
public struct PrefabDict2
{
    public int key;
    public GameObject prefab;
}

[ScriptedImporter(1, "level")]
public class LevelImporter : ScriptedImporter
{
    public float m_Scale = 1;
    public float tileSize = 6f;
    public GameObject withTilemap;
    public int startX, startY, width, height;
    public bool dummy;
    public bool foregroundCharacters;
    public PhysicsMaterial2D phsyMaterial;
    public Material renderMaterial;
    int[] foregroundIgnoreArr =
    {
        //51, 59, // geckoDummy
        //65,66,67,68,69,70,71,72, // num
        //73,74,75,76,77,78,79,80, // alpha
         //311,312,319 // fireflies
    };
    [InspectorName("Prefab mapping")]
    public PrefabDict2[] prefabs;
    public Vector3 cellSize = Vector3.one;
    public Vector3 innerObjectOffset = Vector3.zero;


    public Texture2D map;

    string colorToString(Color c) {
        return comp(c.r)+comp(c.g)+comp(c.b);
    }

    string comp(float r) {
        var res=((int)(r * 255));
        if(res < 16) {
        return "0"+res.ToString("X");
        }
        return res.ToString("X");
    }

    public static void CopyToClipboard( string s)
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }

    public override void OnImportAsset(AssetImportContext ctx)
    {
        if(map != null) {
         var res = new System.Text.StringBuilder();
            Color[] pix = map.GetPixels(0, 0, map.width, map.height);
              for(int y = 0; y < map.height; y++) {
                for(int x = 0; x < map.width; x++) {
                    res.Append(colorToString(pix[y*map.width+x]) + " ");
                }
                res.Append("\n");
            }
            CopyToClipboard(res.ToString());
        }
        

        var parent = new GameObject();
        parent.name = "Level " + ctx.assetPath;
        var grid = parent.AddComponent<Grid>();
        grid.cellSize = cellSize;
        var str = File.ReadAllText(ctx.assetPath);
        string[] levelText = str
        .Replace("\r", "")
        .Split('\n');
        Debug.Log("levelText: <"+str+">\nrows:"+levelText.Length);
        foreach(var row in levelText) {
            if(row.StartsWith("#"))
                continue;
            var parts = row.Split('|');
            var str1 = parts[0].Trim();
            var z = float.Parse(str1, CultureInfo.InvariantCulture);
            var file = parts[1].Trim();
            var tilemapType = parts[2].Trim();
            var layer = new GameObject();
            layer.name = "X Layer " + file + " at Z: " + z + " of type " + tilemapType + " // " + str1;
            layer.transform.parent = parent.transform;
            var pos = layer.transform.position;
            pos.z = z;
            layer.transform.position = pos;
            populateLayer(parent, layer, file, tilemapType);
        }
        ctx.AddObjectToAsset("main obj", parent);
        ctx.SetMainObject(parent);

        //var material = new Material(Shader.Find("Standard"));
        //material.color = Color.red;

        // Assets must be assigned a unique identifier string consistent across imports
        //ctx.AddObjectToAsset("my Material", material);

        // Assets that are not passed into the context as import outputs must be destroyed
        //var tempMesh = new Mesh();
        //DestroyImmediate(tempMesh);
    }

    void populateLayer(GameObject parent, GameObject layer, string file, string tilemapType) {
        var foregroundCharacters = tilemapType == "foreground" || tilemapType == "characters";
        var collision = tilemapType == "collision";

        var palette = withTilemap.GetComponent<Tilemap>();
        var tilemap = layer.AddComponent<Tilemap>();
        var tilemapR = layer.AddComponent<TilemapRenderer>();
        if (renderMaterial != null)
        {
            tilemapR.material = renderMaterial;
        }
        //EdgeCollider2D tilemapC = null;
        //var points = new List<Vector2>();
        if (phsyMaterial != null && collision)
        {
            var tilemapC = layer.AddComponent<TilemapCollider2D>();
            tilemapC.sharedMaterial = phsyMaterial;
            tilemapC.usedByComposite = true;
            var rb = layer.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
            var cc = layer.AddComponent<CompositeCollider2D>();
            //tilemapC = layer.AddComponent<EdgeCollider2D>();
        }

        var txt = File.ReadAllText("Assets/Level/"+file)
        .Replace("\r", "")
        .Split('\n');

        var y = -1;
        var dict = new Dictionary<int, GameObject>();
        if (prefabs != null)
        {
            foreach (var prefab in prefabs)
            {
                dict[prefab.key] = prefab.prefab;
            }
        }

        foreach (string line in txt)
        {
            y++;
            int x = -1;
            foreach (string c in line.Split(','))
            {
                x++;
                if (startX <= x && x <= width + startX && startY <= y && y <= height + startY && c != "" && c != "0")
                {
                    var position = new Vector3Int(x, -y, 0);
                    long value;
                    if (!long.TryParse(c, out value))
                    {
                        Debug.LogError("c => error " + c);
                    }

                    GameObject prefab = null;
                    if (foregroundCharacters)
                    {
                        if (!dict.TryGetValue((int)value, out prefab))
                        {
                            // continue;
                        }
                    }

                    if (prefab != null)
                    {
                        var instance = GameObject.Instantiate(prefab);
                        instance.transform.parent = layer.transform;

                        instance.transform.position = position * (int)tileSize + innerObjectOffset;
                        instance.name = (instance.tag == null ? "" : instance.tag) + value + " at " + position + " => " + instance.transform.position;
                    }
                    else
                    {
                        var tx = value % 8 == 0 ? (value % 8) + 8 - 1 : (value % 8) - 1;
                        var ty =
                            value % 8 == 0 ? -(value / 8) + 1 :
                            -(value / 8);
                        // Debug.Log("y" + y + "x" + x + ": v" + value + "=> ty" + ty + "tx" + tx);
               
                        var tile = palette.GetTile(new Vector3Int((int)tx, (int)ty, 0));
                        tilemap.SetTile(position, tile);
                        
                        //var v = position * (int)tileSize + innerObjectOffset;
                        // TODO https://www.reddit.com/r/gamedev/comments/cyn7i5/determined_edges_of_a_2d_tilemap/
                        //points.Add(new Vector2(v.x, v.y));
                    }
                }
            }
        }
        //if(tilemapC != null) {
            //tilemapC.points = points.ToArray();
            //tilemapC.sharedMaterial = phsyMaterial;
        //}
    }
}
#endif