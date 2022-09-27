using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;
using Common.Data;
using System.Collections.Generic;
using UnityEngine;

public class MapTools {
    [MenuItem("Map Tools/Export Teleporters")]
    public static void ExportTeleporters()
    {
        Manager.DataManager.Instance.Load();

        Scene current=EditorSceneManager.GetActiveScene();
        string currentScene=current.name;
        if (current.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前场景", "我知道了");
            return;
        }
        List<TeleporterObject> allTeleporters = new List<TeleporterObject>();
        foreach(var map in Manager.DataManager.Instance.Maps)
        {
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogWarningFormat("Scene [{0}] not existed!", sceneFile);
                continue;
            }
            EditorSceneManager.OpenScene(sceneFile,OpenSceneMode.Single);

            TeleporterObject[] teleporterObjects=GameObject.FindObjectsOfType<TeleporterObject>();
            foreach(var teleporter in teleporterObjects)
            {
                if (!Manager.DataManager.Instance.Teleporters.ContainsKey(teleporter.ID))
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图:{0} 中配置的 Teleporter:[{1}]中不存在", map.Value.Resource, teleporter.ID), "我知道了");
                    return;
                }
                TeleporterDefine define = Manager.DataManager.Instance.Teleporters[teleporter.ID];
                if(define.MapID != map.Value.ID)
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图:{0} 中配置的 Teleporter:[{1}] MapID:{2} 错误", map.Value.Resource, teleporter.ID,define.MapID), "我知道了");
                    return;
                }
                define.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                define.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);
            }
        }
        Manager.DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "传送点导出完成", "太好力!");
    }
    [MenuItem("Map Tools/Export Spawners")]
    public static void ExportSpawners()
    {
        Manager.DataManager.Instance.Load();

        Scene current = EditorSceneManager.GetActiveScene();
        string currentScene = current.name;
        if (current.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前场景", "我知道了");
            return;
        }
        List<SpawnPoint> allTeleporters = new List<SpawnPoint>();
        foreach (var map in Manager.DataManager.Instance.Maps)
        {
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(sceneFile))
            {
                Debug.LogWarningFormat("Scene [{0}] not existed!", sceneFile);
                continue;
            }
            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);

            SpawnPoint[] spawnerObjects = GameObject.FindObjectsOfType<SpawnPoint>();
            foreach (var spawner in spawnerObjects)
            {
                if (!Manager.DataManager.Instance.Teleporters.ContainsKey(spawner.ID))
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图:{0} 中配置的 spawner:[{1}]中不存在", map.Value.Resource, spawner.ID), "我知道了");
                    return;
                }
                SpawnPointDefine define = Manager.DataManager.Instance.SpawnPoints[map.Value.ID][spawner.ID];
                if (define.MapID != map.Value.ID)
                {
                    EditorUtility.DisplayDialog("错误", string.Format("地图:{0} 中配置的 spawner:[{1}] MapID:{2} 错误", map.Value.Resource, spawner.ID, define.MapID), "我知道了");
                    return;
                }
                define.Position = GameObjectTool.WorldToLogicN(spawner.transform.position);
                define.Direction = GameObjectTool.WorldToLogicN(spawner.transform.forward);
            }
        }
        Manager.DataManager.Instance.SaveSpawnPoints();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("提示", "刷怪点导出完成", "太好力!");
    }
}
