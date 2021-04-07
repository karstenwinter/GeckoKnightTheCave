using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary; 
using Platformer.Mechanics;

[Serializable]
public class SaveState {
    public float x, y, time;
    public int currentHP, maxHP, percentage, shells;
    public string currentArea;
    public string[] inventory;
    public override string ToString () {
        var t = new DateTime();
        t.AddSeconds(time);
        var h = t.Hour;
        var m = t.Minute;
        var s = t.Second;
        return h + "h " + time / 60 + "m " + s + "s, in " + currentArea + ", " + shells + " shells, " + maxHP + " HP";
        // + " shells, " + (inventory == null ? 0 : inventory.Length) + " items";
        // currentHP, maxHP
    }
}

public static class SaveSystem { 
    public static SaveState Save(int index, GameObject player) { 
        //var formatter = new BinaryFormatter(); 
        var playerContr = player.GetComponent<PlayerController>();
        var path = Application.persistentDataPath + "/player"+index+".fun"; 
        var stream = new FileStream(path, FileMode.Create); 
        
        var data = playerContr.CreateSave();
        var str = JsonUtility.ToJson(data, true);
        //Debug.Log("ToJson:"+str);
        //formatter.Serialize(stream, data);
        var w = new StreamWriter(stream);
        w.Write(str);
        w.Flush();
        w.Dispose();
        stream.Close(); 
        return data;
    }

    public static SaveState LoadState(int index) { 
        var path = Application.persistentDataPath + "/player"+index+".fun"; 
        if (File.Exists(path)) { 
            FileStream stream = new FileStream(path, FileMode.Open); 
            string str = new StreamReader(stream, true).ReadToEnd();
            //Debug.Log("ReadToEnd:" + str);
            var data = JsonUtility.FromJson<SaveState>(str);
            //Debug.Log("FromJson:"+data);
            stream.Close();
            return data; 
        } else { 
            Debug.LogError("Save file not found in " + path); 
            return null; 
        } 
    }

    public static SaveState Load(int index, GameObject player) { 
        var data = LoadState(index);
        if (data != null) { 
            var playerContr = player.GetComponent<PlayerController>();
            playerContr.InitFromSave(data);
        }
        return data; 
    }
}
