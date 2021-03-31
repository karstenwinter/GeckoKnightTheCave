using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 

[Serializable]
public class SaveState {
    public float startPositionX, startPositionY;
    public float currentHealth;
    public string area;
    public float time;
    public float percentage;
}

public static class SaveSystem { 
    public static SaveState Save (GameObject player) { 
        var formatter = new BinaryFormatter(); 
        var path = Application.persistentDataPath + "/player.fun"; 
        var stream = new FileStream(path, FileMode.Create); 
        var data = new SaveState() {
            startPositionX = player.transform.position.x, startPositionY = player.transform.position.y,
            currentHealth = 1,
            area = "asd",
            time = Time.fixedDeltaTime,
            percentage = 0
        };
        formatter.Serialize(stream, data);
        stream.Close(); 
        return data;
    }

    public static SaveState Load(GameObject player) { 
        var path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path)) { 
            var formatter= new BinaryFormatter(); 
            var stream = new FileStream(path, FileMode.Open); 
            var data = formatter.Deserialize(stream) as SaveState; 
            stream.Close ();
            var pos = player.transform.position;
            pos.x = data.startPositionX;
            pos.y = data.startPositionY;
            player.transform.position = pos;
            return data; 
        } else { 
            Debug.LogError("Save file not found in " + path); 
            return null; 
        } 
    }
}