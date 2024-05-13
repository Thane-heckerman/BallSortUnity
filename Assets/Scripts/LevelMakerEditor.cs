#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using UnityEditor.SceneManagement;
using System;

namespace LevelEditor
{

    public enum PrefabType
    {
        Tube,
        Ball
    }


    public class LevelMakerEditor : EditorWindow
    {
        private int activeBallType = 0;
        private LevelData levelData = new LevelData();
        public PrefabType prefabType;
        static GameObject _selectedGameObject;
        string AssetPath => "Assests/Prefabs/Levels";
        private Vector2 _scrollDir;
        private GameObject ballGO;
        private GameObject tubeGO;
        public int currentLevel = 1;
        private int levelNumber;
        public LevelListScriptable levelList;
        private Camera Camera;
        private LevelMakerTube _selectTube;
        private List<GameObject> tubes;

        [MenuItem("BallSort/LevelMakerEditor")]
        public static void ShowWindow()
        {
            var window = (LevelMakerEditor)EditorWindow.GetWindow(typeof(LevelMakerEditor), false, "level maker editor");
        }

        Vector2 EventMousePoint
        {
            get
            {
                var v = UnityEngine.Event.current.mousePosition;
                return v;
            }
        }

        bool MouseInView
        {
            get
            {
                try
                {
                    return mouseOverWindow != null && mouseOverWindow.titleContent != null &&
                           string.Equals("Scene", mouseOverWindow.titleContent.text);
                }
                catch
                {
                    return false;
                }
            }
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += DuringSceneView;
        }


        private void OnDisable()
        {
            SceneView.duringSceneGui -= DuringSceneView;
        }

        private void OnGUI()
        {
            if (Application.isPlaying) return;
            var stageHandle = StageUtility.GetCurrentStageHandle();
            var level = stageHandle.FindComponentOfType<LevelMakerData>();
            if (level && PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                var levelAsset = AssetDatabase.LoadAssetAtPath<LevelMakerData>(PrefabStageUtility.GetCurrentPrefabStage().assetPath);
                if (levelAsset != null)
                {
                    EditorGUILayout.Space();
                    var directory = new DirectoryInfo(Path.Combine(Application.dataPath, "Prefabs/Levels"));
                    //Debug.Log(directory.Name);
                    if (directory.Exists)
                    {
                        DrawPrefabSelector(directory);

                    }
                }
            }

        }

        void DrawPrefabSelector(DirectoryInfo directory)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("HelpBox");
            var fileInfos = directory.GetFiles();
            var prefabs = new List<GameObject>();
            foreach (var fileInfo in fileInfos)
            {
                var s = fileInfo.FullName.IndexOf("Assets", StringComparison.Ordinal);
                var path = fileInfo.FullName.Substring(s);
                var prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                if (prefab != null)
                {
                    prefabs.Add(prefab);
                }
            }
            const float buttonSize = 100;
            const float spacing = 4;
            var column = Mathf.FloorToInt(EditorGUIUtility.currentViewWidth - 10) / (buttonSize + spacing);
            var row = Mathf.CeilToInt(prefabs.Count) / column;
            for (var i = 0; i < row; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (var j = 0; j < column; j++)
                {
                    int index = Mathf.FloorToInt(i * column + j);
                    if (index >= prefabs.Count)
                    {
                        break;
                    }
                    EditorGUILayout.BeginVertical();
                    GUI.color = _selectedGameObject == prefabs[index] ? Color.cyan : Color.white;
                    if (GUILayout.Button(AssetPreview.GetAssetPreview(prefabs[index].gameObject), GUILayout.Width(100),
                        GUILayout.Height(100)))
                    {
                        if (_selectedGameObject != prefabs[index])
                        {
                            _selectedGameObject = prefabs[index];
                        }
                        else
                        {
                            _selectedGameObject = null;
                        }
                    }

                    GUILayout.Label(prefabs[index].name, GUILayout.MaxWidth(100));
                    EditorGUILayout.EndVertical();
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        private void DuringSceneView(SceneView sceneView)
        {
            OnCustomSceneGUI(sceneView);
        }

        private void OnCustomSceneGUI(SceneView sceneView)
        {
            if (Application.isPlaying)
            {
                return;
            }
            var stageHandle = StageUtility.GetCurrentStageHandle();
            var level = stageHandle.FindComponentOfType<LevelMakerData>();
            if (!level) return;
            PlaceOnPosition(level);
            //if (Selection.activeObject != null)
            //{
            //    Debug.Log("active object instanceID :" + Selection.activeObject.GetInstanceID());
            //}
        }

        private int GetLastLevelNumber()
        {
            if (levelList.LevelDataContainers.Count == 0) return 1;

            return levelList.LevelDataContainers.Count;
        }

        // OnGui a button with setsprite according to enum BallColor in each ball
        // => custom editor  
        private void PlaceOnPosition(LevelMakerData level)
        {
            Camera = SceneView.currentDrawingSceneView.camera;
            if (!mouseOverWindow) return;
            if (_selectedGameObject == null) return;
            Vector3 mouseCast = RayCastPoint(level, EventMousePoint);
            var ec = UnityEngine.Event.current;
            var hsize = HandleUtility.GetHandleSize(mouseCast) * 0.4f;
            Handles.color = new Color(1, 0, 0, 0.5f);
            Handles.DrawSolidDisc(mouseCast, Vector3.up, hsize * 0.5f);
            var component = _selectedGameObject.GetComponent<Component>();
            if (ec.type == EventType.MouseDown && ec.button == 1 && ec.control)
            {
                var newGo = PrefabUtility.InstantiatePrefab(_selectedGameObject) as GameObject;
                StageUtility.PlaceGameObjectInCurrentStage(newGo);
                tubes.Add(newGo);
                newGo.transform.parent = level.transform;
                newGo.GetComponent<LevelMakerTube>().Init();
                newGo.transform.position = new Vector3(Mathf.RoundToInt(mouseCast.x), mouseCast.y,
                        0f);
                newGo.transform.localPosition = new Vector3(newGo.transform.localPosition.x,
                    _selectedGameObject.transform.localPosition.y, newGo.transform.localPosition.z);
                newGo.transform.localRotation = _selectedGameObject.transform.localRotation;
                Selection.activeObject = newGo;
                EditorUtility.SetDirty(level.gameObject);
            }

            if (ec.type == EventType.KeyDown && ec.keyCode == KeyCode.T && Selection.activeGameObject != null)
            {
                var newGO = PrefabUtility.InstantiatePrefab(_selectedGameObject) as GameObject;
                StageUtility.PlaceGameObjectInCurrentStage(newGO);
                newGO.transform.SetParent(Selection.activeTransform);
                //assign ball to ball pos
                _selectTube = Selection.activeGameObject.transform.GetComponent<LevelMakerTube>();
                Ball ball = newGO.GetComponent<Ball>();
                var pos = _selectTube.GetFirstEmtyBallPos();
                ball.SetIndex(pos);
                _selectTube.GetBallPosList()[pos].ballPosData.SetBallObj(ball);
                newGO.transform.localPosition = _selectTube.ballPosList[pos].transform.localPosition;
            }
            else return;

        }

        private void SetBallSprite(GameObject ball, BallColor color, int index = 0)
        {
            ball.GetComponent<IColorableComponent>().SetSprite(color, index);
        }
        
        private Vector3 RayCastPoint(LevelMakerData level, Vector2 point, float dist = 10)
        {
            var ray = Camera.ScreenPointToRay(point);
            if (!RayCast(level, ray, out var hit))
            {
                hit = ray.origin + ray.direction * dist;
            }
            return hit;
        }

        bool RayCast(LevelMakerData level, Ray ray, out Vector3 hitPoint)
        {
            hitPoint = Vector3.zero;
            RaycastHit hit;
            if (level.gameObject.scene.GetPhysicsScene().Raycast(ray.origin, ray.direction, out hit))
            {
                hitPoint = hit.point;
                return true;
            }
            return false;
        }

        private List<Transform> GetAllTube(LevelMakerData level, string gameObjectName = "TubePrefab")
        {
            List<Transform> tubeList = new List<Transform>();
            for (int i = 0; i < level.transform.childCount; i++)
            {
                var child = level.transform.GetChild(i);
                if (child.name == gameObjectName)
                {

                    tubeList.Add(child);
                }
            }
            return tubeList;
        }
    }
}
#endif