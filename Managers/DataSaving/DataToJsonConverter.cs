using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor.Experimental;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataToJsonConverter : MonoBehaviour
{
    public RoomManager _roomManager;

    //The artifact that gets respawned, will later have to be a list when there's more types of artifacts
    [SerializeField] private List<GameObject> _artifact;

    private void Start()
    {
        //_index = new int[_artifact.Length];

        Invoke("WriteToJson", 1);

        _roomManager.EndedRoom += LoadArtifactPositionsFromJson;
    }
    void WriteToJson()
    {
        RoomDataManager roomData = new RoomDataManager(_roomManager);

        string json = JsonUtility.ToJson(roomData, true);
        File.WriteAllText(Application.dataPath + "/RoomData_" + roomData.roomID + ".json", json);

        LevelManager.instance.DisableArtifactList(_roomManager._roomID);
    }

    public void LoadArtifactPositionsFromJson(object sender, EventArgs e)
    {
        string filePath = Application.dataPath + "/RoomData_" + _roomManager._roomID + ".json";
        string json = File.ReadAllText(filePath);

        if (File.Exists(filePath))
        {
            RoomDataManager data = JsonUtility.FromJson<RoomDataManager>(json);

            for (int i = 0; i < _roomManager.GetArtifactAmount(); i++)
            {
                int randomNumber = Random.Range(0, _artifact.Count);
                var newArtifact = Instantiate(_artifact[randomNumber], data.artifactsPositions[i], Quaternion.identity);

                newArtifact.GetComponent<Artifacts>()._roomID = _roomManager._roomID;
                newArtifact.GetComponent<Collider>().enabled = true;
                newArtifact.GetComponent<Rigidbody>().isKinematic = true;
                newArtifact.transform.parent = this.transform.GetChild(0);
            }

        }
        else {
            Debug.LogError("Save file not sound in: " + filePath);
        }
    }
}
