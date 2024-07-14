using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ghost
{
    public class GhostSpawnPoints : MonoBehaviour
    {
        public Ghost _ghost;

        public int _roomID;

        public Ghost SpawnGhost()
        {
            var spanwedGhost = Instantiate(_ghost, transform.position, Quaternion.identity);
            return spanwedGhost;
        }
    }
}
