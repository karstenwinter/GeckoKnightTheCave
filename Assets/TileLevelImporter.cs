﻿
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Tilemaps;

[ScriptedImporter(1, "tmx")]
public class TileLevelImporter : ScriptedImporter
{
    public float m_Scale = 1;
    public float tileScale = 2;

    public int startY = 1;
    public int startX = 0;
    public int w = 32;
    public int h = 32;

    public GameObject tilemapG;

    //public Material block,water,breakable;
    //public GameObject spike;

    string fm(Color c)
    {
        string s = Convert.ToString((int)(c.r * 255), 16)
        + Convert.ToString((int)(c.g * 255), 16)
        + Convert.ToString((int)(c.b * 255), 16);
        return "#" + s;
    }

    public override void OnImportAsset(AssetImportContext ctx)
    {
        var name = ctx.assetPath.Replace("\\", "/").Split('/').LastOrDefault() ?? "temp";
        var gameObjectRoot = new GameObject(name);
        var target = gameObjectRoot.AddComponent<Tilemap>();
        gameObjectRoot.AddComponent<TilemapRenderer>();
        gameObjectRoot.AddComponent<TilemapCollider2D>();
        //  gameObjectRoot.AddComponent<Transform>();
        string fileContent = File.ReadAllText(ctx.assetPath).Replace("\r", "");
        var lines = fileContent.Split('\n');

        int startY = -1;
        int endY = -1;
        bool found = false;
        int line = -1;
        foreach (var l in lines)
        {
            line++;
            var ltr = l.Trim();
            if (ltr.StartsWith("<layer name=\"Level\" width=\"512\" height=\"512\">"))
            {
                found = true;
                startY = line + 2;
            }
            if (found && ltr.StartsWith("</data>"))
            {
                endY = line;
                break;
            }

        }

        var position = new Vector3(0, 0, 0);
        if (tilemapG != null)
        {
            var tilemap = tilemapG.GetComponent<Tilemap>();
            Debug.Log("tilemap: " + tilemap + ", in " + tilemapG + ", range" + startY + "," + endY);
            for (int y = startY; y < endY; y++)
            {
                var arr = lines[y].Split(',');
                w = Math.Min(w, arr.Length);
                for (var x = startX; x < w; x++)
                {
                    string current = arr[x];
                    //string tag = "t" + current;

                    //GameObject cubeObj = new GameObject("y" + y + "x" + x + " " + tag);
                    //cubeObj.transform.parent = gameObjectRoot.transform;
                    //cubeObj.transform.position = 

                    //* tileScale;
                    //cubeObj.transform.localScale = Vector3.one * tileScale;
                    //if (current != "0")
                    {
                        var value = int.Parse(current);
                        var tileY = value / 8;
                        var tileX = value % 8;
                        var tile = tilemap.GetTile(
                            new Vector3Int(tileX, tileY, 0)
                            );
                        target.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("error " + tilemapG);
        }
        gameObjectRoot.transform.position = position;
        gameObjectRoot.transform.localScale = Vector3.one * m_Scale;

        ctx.AddObjectToAsset("main obj", gameObjectRoot);
        ctx.SetMainObject(gameObjectRoot);
    }
}

#endif