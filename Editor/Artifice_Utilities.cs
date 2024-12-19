using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ArtificeToolkit.AEditor.Artifice_CustomAttributeDrawers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArtificeToolkit.AEditor
{
    /// <summary>Provides utilities to other Editor scripts.</summary>
    /// <example>Get the style for a specific <see cref="VisualElement"/>.</example>
    public class Artifice_Utilities
    {
        #region FIELDS
        
        public static bool ArtificeDrawerEnabled
        {
            get => EditorPrefs.GetBool("artificeDrawerEnabled");
            set => EditorPrefs.SetBool("artificeDrawerEnabled", value);
        }
        private StylesHolder _soStylesHolder;
        private Dictionary<Type, Type> _drawerMap;
        
        #endregion

        #region SINGLETON

        private Artifice_Utilities()
        {
        }

        private static Artifice_Utilities _instance;

        private static Artifice_Utilities Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Artifice_Utilities();
                    _instance.InitializeSingleton();
                }

                return _instance;
            }
        }
        
        private void InitializeSingleton()
        {
            var styleHolderPaths = AssetDatabase.FindAssets($"Artifice_StylesHolder t:{nameof(StylesHolder)}");
            
            if(styleHolderPaths.Length != 1)
                Debug.LogError($"Exactly one asset of this kind should get fetched, not {styleHolderPaths.Length}");
            
            _soStylesHolder = AssetDatabase.LoadAssetAtPath<StylesHolder>(AssetDatabase.GUIDToAssetPath(styleHolderPaths[0]));
            InitializeDrawerMap();
        }

        #endregion
        
        #region MenuItems

        private const string ArtificeInspectorOn = "Artifice Toolkit/" + "\u2712 Toggle ArtificeInspector/On";
        private const string ArtificeInspectorOff = "Artifice Toolkit/" +"\u2712 Toggle ArtificeInspector/Off";
        private const string ArtificeDocumentation = "Artifice Toolkit/" +"\ud83d\udcd6 Documentation...";
        private const string ArtificeDocumentationURL = "https://github.com/AbZorbaGames/artificetoolkit";
        
        [MenuItem(ArtificeInspectorOn, true, 0)]
        private static bool ToggleOnCheckmark()
        {
            Menu.SetChecked(ArtificeInspectorOn, ArtificeDrawerEnabled);
            return true;
        }

        /// <summary> Creates a MenuItem to enable and disable the Artifice system. </summary>
        [MenuItem(ArtificeInspectorOn, priority = 11)]
        private static void ToggleArtificeDrawerOn()
        {
            ToggleArtificeDrawer(true);
            Debug.Log("<color=lime>[Artifice Inspector]</color> Enabled");
        }
        
        /// <summary> Creates a MenuItem to enable and disable the Artifice system. </summary>
        [MenuItem(ArtificeInspectorOff, priority = 11)]
        private static void ToggleArtificeDrawerOff()
        {
            ToggleArtificeDrawer(false);
            Debug.Log($"<color=orange>[Artifice Inspector]</color> Disabled");
        }
        
        [MenuItem(ArtificeInspectorOff, true, 0)]
        private static bool ToggleOffCheckmark()
        {
            Menu.SetChecked(ArtificeInspectorOff, !ArtificeDrawerEnabled);
            return true;
        }
        
        [MenuItem(ArtificeDocumentation)]
        private static void OpenArtificeDocumentationURL()
        {
            Application.OpenURL(ArtificeDocumentationURL);
        }
        
        public static void ToggleArtificeDrawer(bool toggle)
        {
            var guid = AssetDatabase.FindAssets("ArtificeInspector").FirstOrDefault();
            if (guid == null)
            {
                Debug.Log("ArtificeToolkit: Cannot find ArtificeInspector script. This makes it unable to turn on/off the ArtificeToolkit.");
                return;
            }
            
            var filePath = AssetDatabase.GUIDToAssetPath(guid);
            if (File.Exists(filePath))
            {
                var hasChangedFile = false;
                var lines = File.ReadAllLines(filePath);

                // Set Regex pattern
                var customEditorAttributePattern = @"^\s*(//\s*)?\[CustomEditor\(typeof\(Object\), true\), CanEditMultipleObjects\]\s*$";
                for (var i = 0; i < lines.Length; i++)
                {
                    if (!Regex.IsMatch(lines[i], customEditorAttributePattern)) 
                        continue;
                    
                    // Check if the line is already commented
                    if (toggle && lines[i].TrimStart().Contains("//"))
                    {
                        // Uncomment the line
                        lines[i] = lines[i].Substring(2);
                        hasChangedFile = true;
                    }
                    else if(!toggle && !lines[i].TrimStart().StartsWith("//"))
                    {
                        // Comment out the line
                        lines[i] = "//" + lines[i];
                        hasChangedFile = true;
                    }
                    
                    break;
                }
                
                if (hasChangedFile)
                {
                    // Empty selection, since it will have to be refocused anyway.
                    Selection.objects = null;
                    
                    // Change toggle and write/refresh.
                    ArtificeDrawerEnabled = toggle;
                    File.WriteAllLines(filePath, lines);
                    AssetDatabase.Refresh();
                }
            }
        }

        #endregion
        
        public static Dictionary<Type, Type> GetDrawerMap()
        {
            return Instance._drawerMap;
        }
        
        /* Uses singleton privately, to allow access with static method */
        public static StyleSheet GetStyle(Type type)
        {
            return Instance._soStylesHolder.GetStyle(type);
        }
        
        /* Uses singleton privately, to allow access with static method */
        public static StyleSheet GetGlobalStyle()
        {
            return Instance._soStylesHolder.GetGlobalStyle();
        }

        /* Method Dictating the equality of certain objects */
        public static bool AreEqual(object object1, object object2)
        {
            if (object1 == null && object2 == null)
            {
                return true;
            }

            if (object1 == null || object2 == null)
            {
                return false;
            }

            // Special case for enums
            if (object1 is Enum || object2 is Enum)
            {
                var enumType = object1 is Enum ? object1.GetType() : object2.GetType();
                var supportsFlags = enumType.GetCustomAttribute(typeof(FlagsAttribute)) != null;

                // Easier to debug instead of inline
                var v1 = Convert.ToInt64(object1);
                var v2 = Convert.ToInt64(object2);

                if (supportsFlags)
                {
                    var result = v1 & v2;
                    return result != 0;
                }

                return v1 == v2;
            }

            // Compare the values based on their types
            if (object1.GetType() == object2.GetType())
            {
                return object1.Equals(object2);
            }


            // Convert to strings and compare if the types are different
            var stringValue1 = object1.ToString();
            var stringValue2 = object2.ToString();

            return stringValue1.Equals(stringValue2);
        }
        
        /* Converts global PC path to Unity relative for AssetDatabase usage */
        public static string ConvertGlobalToRelativePath(string globalPath)
        {
            var dataPath = Application.dataPath;
            
            // DataPath by default includes Assets. So remove if from the the dataPath before extracting.
            dataPath = dataPath.Replace("Assets", "");
            
            // Extract dataPath completely. Whats left, is our relative path.
            return globalPath.Replace(dataPath, "");
        }
        
        /* Returns a map of all the AttributeDrawers */
        private void InitializeDrawerMap()
        {
            _drawerMap = new Dictionary<Type, Type>();

            var allDrawersTypes = TypeCache.GetTypesDerivedFrom<Artifice_CustomAttributeDrawer>();
            foreach (var drawerType in allDrawersTypes)
            {
                if(drawerType.IsAbstract)
                    continue;
                
                var customDrawerAttribute = drawerType.GetCustomAttribute<Artifice_CustomAttributeDrawerAttribute>();
                _drawerMap[customDrawerAttribute.Type] = drawerType;
            }
        }
    }
}