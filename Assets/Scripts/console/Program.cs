using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

class Program {    
    int w = 128;
    int h = 128;
    string freeC = "9CA7AE";
    string wallC = "1A1B1C";
    string npcC = "5DB767";
    string bossC = "550E11";
    string pickupC = "CFA926";
    string breakC = "6E767A";
    string enemyColor = "D0222C";
    string abColor = "C047C1";
    string anColor = "FFFFFF";
    string upgColor = "C047C1";
    string divColor = "FAB3B6";
    string waterColor = "4755C1";
    string[][] cols;
    int cornerNum = 1;

    struct Corner {
        public int y, x;
        public string color;
        public int num;
    }

    static void Main(string[] args) {
        new Program();
    }
    Corner? findTopLeft () {
        for(int y = 1; y < h-1; y++){
            for(int x = 0; x < w-1; x++){
                if(
                    cols[y][x] == anColor && cols[y+1][x] == anColor && cols[y][x+1] == anColor && cols[y+1][x+1] != anColor
                ) {
                    Console.Write("an: y" + y + "x" + x);
                     return new Corner() { x = x, y = y, color = cols[y+1][x+1], num = cornerNum++ };
                }
            }
        }
        return null;
    }
    Corner? findRight (Corner topLeft) {
        int y = topLeft.y;
        for(int x = topLeft.x + 1; x < w-1; x++) {
            if(
                cols[y][x] == anColor && cols[y][x-1] == anColor && cols[y+1][x] == anColor && cols[y+1][x-1] != anColor
            ) {
                    return new Corner() { x = x, y = y, color = "", num = topLeft.num };
            }
        }
        return null;
    }

    private Program() {
        var rows = File.ReadAllText("pixels.txt").Split('\n');
        Array.Reverse(rows);
        cols = Array.ConvertAll(rows, x => x.Split(' '));
        var res = new StringBuilder();
        var startX = 0;
        var startY = 0;
        var endX = w;
        var endY = h;
        var anchors = new List<Corner>();
        var posStart = findTopLeft();
        if(posStart!=null) {
            anchors.Add(posStart.Value);
            var r = findRight(posStart.Value);
            if(r!=null) {
                anchors.Add(r.Value);
            }
        }

        /*next:
        for(int y = startY; y < h-1; y++){
            for(int x = startX; x < w-1; x++){
                if(
                    cols[y][x] == anColor && cols[y][x+1] == anColor && cols[y+1][x+1] == anColor && cols[y+1][x] != anColor
                ) {
                    Console.Write("end an: y" + y + "x" + x);
                     endX = x;
                     endY = y;
                }
            }
        }*/
        for(int y = startY; y < endY; y++){
            for(int x = startX; x < endX; x++){
                //Console.Write(s[y*w+x] == "9CA7AD"?" ":"X");
                //Console.Write(cols[y][x] + "|");
                var c1 = cols[y][x];
                if(c1 == anColor) {
                    res.Append(anchors.Exists(c => c.x == x & c.y == y) ? "G" : "A");
                } else if(c1 == freeC) {
                    res.Append(" ");
                } else if(c1 == waterColor) {
                    res.Append(".");
                } else {
                    res.Append("X");
                }
            }
            res.AppendLine("");
        }
        Console.WriteLine(res);
        //File.WriteAllText("res.txt", res.ToString());
    }
}