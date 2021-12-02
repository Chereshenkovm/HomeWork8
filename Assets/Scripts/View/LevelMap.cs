using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View
{
    public class LevelMap : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        [SerializeField] private Transform _root;

        [SerializeField] private List<Vector3> _points;

        [SerializeField] private Transform[] _spawnPoints;

        [SerializeField] private Transform[] _boosterSpawnPoints;

        public IReadOnlyList<Vector3> Points => _points;

        public Transform[] SpawnPoints => _spawnPoints;

        public Transform[] BoosterSpawnPoints => _boosterSpawnPoints;
#if UNITY_EDITOR
        
        [UnityEditor.MenuItem("CONTEXT/LevelMap/Instantiate Points")]
        private static void InstantiatePoints(UnityEditor.MenuCommand command)
        {
            Clear(command);
            
            var levelMap = command.context as LevelMap;
            if (levelMap == null)
                return;

            foreach (var p in levelMap._points.Distinct())
            {
                var prefab = UnityEditor.PrefabUtility.InstantiatePrefab(levelMap._prefab, levelMap._root) as GameObject;
                prefab.transform.position = p;
            }
        }
        
        [UnityEditor.MenuItem("CONTEXT/LevelMap/Clear Points")]
        private static void Clear(UnityEditor.MenuCommand command)
        {
            var levelMap = command.context as LevelMap;
            if (levelMap == null)
                return;
            
            var count = levelMap._root.childCount;
            for (var i = count - 1; i >= 0; i--)
            {
                DestroyImmediate(levelMap._root.GetChild(i).gameObject);
            }
        }
#endif
    }
}