using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

[System.Serializable]
public class RoomDataManager
{
    public int roomID;
    public Vector3[] artifactsPositions;

    public RoomDataManager(RoomManager roomManager)
    {
        roomID = roomManager._roomID;
        artifactsPositions = new Vector3[roomManager._allArtifacts.Count];

        int i = 0;
        foreach (var artifact in roomManager._allArtifacts)
        {
            artifactsPositions[i] = artifact.position;
            i++;
        }
    }
}
