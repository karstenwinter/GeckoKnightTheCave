using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBasedEnabling : MonoBehaviour
{
    List<GameObject> arr;
    List<GameObject>[,] grid;
    int myX, myY;
    public int gridCellSize = 16;
    // Start is called before the first frame update
    void Awake()
    {
        arr = new List<GameObject>();
        arr.AddRange(GameObject.FindGameObjectsWithTag("NPC"));
        arr.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        grid = new List<GameObject>[512, 512];
        foreach(var obj in arr)
        {
            var x = toIndex(obj.transform.position.x);
            var y = toIndex(-obj.transform.position.y);
            var list = grid[x, y];
            if(list == null) {
                grid[x, y] = list = new List<GameObject>();

                list.Add(obj);
            }
        }
    }

    int toIndex(float pos)
    {
        return System.Math.Max(0, System.Math.Min(511, (int)(pos / gridCellSize)));
    }

    // Update is called once per frame
    void Update()
    {
       // var dx = Input.GetAxis("Horizontal");
       // var dy = Input.GetAxis("Vertical");
     //   transform.position += new Vector3(dx, dy, 0);
        var newMyX = toIndex(transform.position.x);
        var newMyY = toIndex(-transform.position.y);
        if(newMyX != myX || newMyY != myY)
        {
            var obj = grid[myX, myY];
            setActive(obj, false); 
            myX = newMyX;
            myY = newMyY;
            obj = grid[myX, myY];
            setActive(obj, true); 
        }
        //if(dx != 0 || dy != 0)
        //{
          //  InputCanvas.instance.PlaySound("move");
        //}
    }

    void setActive(List<GameObject> list, bool enabled)
    {
        if (list != null)
        {
            foreach(var obj in list)
            {
               obj.GetComponent<SpriteRenderer>().color = enabled 
               ? Color.white : Color.black;
            }
        }
    }

}
