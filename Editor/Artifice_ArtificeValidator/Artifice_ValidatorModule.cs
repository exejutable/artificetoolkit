using System.Collections;
using System.Collections.Generic;
using System.IO;
using Editor.Artifice_ArtificeValidator.Artifice_EditorWindow_ArtificeValidator;
using Editor.Artifice_ArtificeValidator.EditorWindow_ArtificeValidator;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor.Artifice_ArtificeValidator
{
    using ValidatorLog = Artifice_EditorWindow_Validator.ValidatorLog;
    
    public abstract class Artifice_ValidatorModule
    {
        #region FIELDS

        /// <summary>Display name of module</summary>
        public virtual string DisplayName { get; protected set; } = "Undefined";
        
        /// <summary>When set to true, module will only run with dedicated button call</summary>
        public virtual bool OnDemandOnlyModule { get; protected set; } = false;
        
        /// <summary>Each module will empty and fill this list with its validations when <see cref="ValidateCoroutine"/> is called</summary>
        public readonly List<ValidatorLog> Logs = new List<ValidatorLog>();

        private int _currentBatchCount = 0; 
        
        #endregion

        /* Main Abstract Method for Validation */
        public abstract IEnumerator ValidateCoroutine(int batchSize);
        
        #region Utility
        
        protected bool CheckBatchCount(int batchSize)
        {
            if (++_currentBatchCount > batchSize)
            {
                _currentBatchCount = 0;
                return true;
            }
            return false;
        }
        
        protected Artifice_SCR_ValidatorConfig GetConfig()
        {
            // Do this on every call, since its possible for the selected config to be changed
            const string configKeyPath = Artifice_EditorWindow_Validator.ConfigPathKey;
            return AssetDatabase.LoadAssetAtPath<Artifice_SCR_ValidatorConfig>(EditorPrefs.GetString(configKeyPath));
        } 
        protected List<SerializedProperty> FindPropertiesInTrackedScenes()
        {
            var list = new List<SerializedProperty>();

            foreach (var pair in GetConfig().scenesMap)
            {
                var sceneName = pair.Key;
                var shouldSearchScene = pair.Value;

                var scene = SceneManager.GetSceneByName(sceneName);
                if(shouldSearchScene && scene.isLoaded)
                    foreach (var monoBehaviour in Object.FindObjectsOfType<MonoBehaviour>(true))
                        list.Add(new SerializedObject(monoBehaviour).GetIterator());
            }
                
            return list;
        }
        protected List<SerializedProperty> FindPropertiesInTrackedAssets()
        {
            var list = new List<SerializedProperty>();
            
            // Only search at selected asset paths
            foreach (var (assetFolderPath, includedInSearch) in GetConfig().assetPathsMap)
            {
                if (includedInSearch == false)
                    continue;
                
                var filePaths = Directory.GetFiles(assetFolderPath, "*.prefab", SearchOption.AllDirectories);
                foreach (string filePath in filePaths)
                {
                    var relativePath = Artifice_Utilities.ConvertGlobalToRelativePath(filePath);
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);
                    if (prefab == null)
                    {
                        Debug.LogWarning($"Artifice Validator: Unable to load prefab '{relativePath}'");
                        continue;
                    }
        
                    var monoBehaviours = prefab.GetComponents<MonoBehaviour>();
                    foreach (var monoBehaviour in monoBehaviours)
                    {
                        if (monoBehaviour == null)
                            continue;
                        var serializedObject = new SerializedObject(monoBehaviour);
                        var iterator = serializedObject.GetIterator();
                        if(iterator != null)
                            list.Add(iterator);
                    }
                }
            }
            
            return list;
        }
        
        #endregion
    }
}