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
    public string currentArea, language, difficulty, profile, mode;
    public string[] inventory;
    public override string ToString () {
        var t = new DateTime();
        t.AddSeconds(time);
        var h = t.Hour;
        var m = t.Minute;
        return 
            h + " hour" + (h == 1 ? "" : "s") + " " +
            m + " minute" + (m == 1 ? "" : "s") + " / " +
            currentArea + " / " +
            percentage + "%";
        // + " shells, " + (inventory == null ? 0 : inventory.Length) + " items";
        // currentHP, maxHP
    }
}

[Serializable]
public class GlobalSettings {
    public string profile;
}

public static class SaveSystem { 
    public static void SaveSettings(GlobalSettings data) { 
        var name = "settings.json"; 
        SaveData(data, name);
    }
    
    public static GlobalSettings LoadSettings(bool logErrorOnFail = false) { 
        var name = "settings.json"; 
        var path = GetPath(name);
        if (File.Exists(path)) { 
            return LoadData<GlobalSettings>(name); 
        } else { 
            if(logErrorOnFail) {
                Debug.LogError("Settings file not found in " + path); 
            }
            return null; 
        } 
    }

    public static string GetPath(string name) {
        return Application.persistentDataPath + "/" + name;
    }

    public static T SaveData<T>(T data, string name) {
        //var formatter = new BinaryFormatter(); 
        var path = GetPath(name);
        var stream = new FileStream(path, FileMode.Create); 
        var str = JsonUtility.ToJson(data, true);
        var w = new StreamWriter(stream);
        w.Write(str);
        w.Flush();
        w.Dispose();
        stream.Close(); 
        return data;
    }

    public static T LoadData<T>(string name) {
        var path = GetPath(name);
        FileStream stream = new FileStream(path, FileMode.Open); 
        string str = new StreamReader(stream, true).ReadToEnd();
        //Debug.Log("ReadToEnd:" + str);
        var data = JsonUtility.FromJson<T>(str);
        //Debug.Log("FromJson:"+data);
        stream.Close();
        return data;
    }

    public static SaveState Save(string index, GameObject player) { 
        //var formatter = new BinaryFormatter(); 
        var playerContr = player.GetComponent<PlayerController>();
        var name = "profile"+index+".json"; 
        var data = playerContr.CreateSave();
        return SaveData(data, name);
    }

    public static SaveState LoadState(string index, bool logErrorOnFail = false) { 
        var name = "profile"+index+".json"; 
        var path = GetPath(name);
        if (File.Exists(path)) { 
            return LoadData<SaveState>(name); 
        } else { 
            if(logErrorOnFail) {
                Debug.LogError("Save file not found in " + path); 
            }
            return null; 
        } 
    }

    public static SaveState Load(string index, GameObject player) { 
        var data = LoadState(index);
        if (data != null) { 
            var playerContr = player.GetComponent<PlayerController>();
            playerContr.InitFromSave(data);
        }
        return data; 
    }
}
