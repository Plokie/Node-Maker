using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public enum Type {
        None,Spawn,Exit,Secret,RoomSmall,RoomMedium,RoomLarge,Split,OneWay,Boss,DeadEnd,Count
    }

    public enum Feature {
        Key, MinorTreasure, MajorTreasure, MinorDanger, MajorDanger, Locked, Count
    }

    public static Dictionary<Feature, Color> featureIconColour = new Dictionary<Feature, Color>() {
        {Feature.Key, new Color(252f/255f,186f/255f,3f/255f)},
        {Feature.MinorTreasure, new Color(77f/255f,25f/255f,10f/255f)},
        {Feature.MajorTreasure, new Color(156f/255f,22f/255f,55f/255f)},
        {Feature.MinorDanger, new Color(194f/255f,100f/255f,0)},
        {Feature.MajorDanger, new Color(214f/255f,25f/255f,0)},
        {Feature.Locked, new Color(48f/255f,48f/255f,48f/255f)},
    };

   public static List<Type> removedTypes = new List<Type>() {
    Type.Secret, Type.Boss, Type.OneWay
   };

    public static Dictionary<Type,string> name = new Dictionary<Type, string>(){
        {Type.None,     "None;Error"},
        {Type.Spawn,    "Spawn"},
        {Type.Exit,     "Exit"},
        //{Type.Danger,   "Danger"},
        //{Type.Lock,     "Lock"},
        {Type.Secret,   "Secret"},
        {Type.RoomSmall,    "Small Room"},
        {Type.RoomMedium,    "Medium Room"},
        {Type.RoomLarge,    "Large Room"},
        {Type.Split,    "Split"},
        {Type.OneWay,   "One-Way"},
        {Type.Boss,     "Boss Room"},
        {Type.DeadEnd,     "Dead End"},
    };

    public static Dictionary<Type, int> inputAmount = new Dictionary<Type, int>() {
        {Type.None,     0},
        {Type.Spawn,    3},
        {Type.Exit,     3},
        //{Type.Danger,   4},
        //{Type.Lock,     4},
        {Type.Secret,   1},
        {Type.RoomSmall,    2},
        {Type.RoomMedium,    3},
        {Type.RoomLarge,    3},
        {Type.Split,    1},
        {Type.OneWay,   1},
        {Type.Boss,     1},
        {Type.DeadEnd,  1},
    };

    public static Dictionary<Type, int> outputAmount = new Dictionary<Type, int>() {
        {Type.None,     0},
        {Type.Spawn,    3},
        {Type.Exit,     3},
        //{Type.Danger,   4},
        //{Type.Lock,     4},
        {Type.Secret,   3},
        {Type.RoomSmall,    3},
        {Type.RoomMedium,    3},
        {Type.RoomLarge,    3},
        {Type.Split,    3},
        {Type.OneWay,   1},
        {Type.Boss,     1},
        {Type.DeadEnd,     0},
    };
}
