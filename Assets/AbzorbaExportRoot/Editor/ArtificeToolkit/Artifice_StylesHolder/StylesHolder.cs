using System;
using System.Collections.Generic;
using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_StylesHolder
{
    /* NESTED CLASS */
    /// <summary> Holds info, regarding stylesheets/></summary>
    [Serializable]
    public class StyleData
    {
        public string name;
        public MonoScript script;
        public StyleSheet stylesheet;
    }

    [Serializable]
    public class StyleDataCategory
    {
        public string categoryName;
        
        [Abz_Title("Data")]
        public List<StyleData> styleData;
    }

    /// <summary>Holds <see cref="StyleData"/>, accessed through <see cref="Artifice_Utilities"/>.</summary>
    [CreateAssetMenu(fileName = "Styles Holder", menuName = "ScriptableObjects/ArtificeToolkit/Styles Holder")]
    public class StylesHolder : ScriptableObject
    {
        #region FIELDS

        [SerializeField] private StyleSheet globalStyle = null;
        [SerializeField] private List<StyleDataCategory> categories;

        #endregion

        /* Public GetStyle */
        public StyleSheet GetStyle(Type type)
        {
            foreach(var category in categories)
                foreach (var data in category.styleData)
                    if (data.script != null && data.script.GetClass() == type)
                        return data.stylesheet;

            Debug.Assert(false, $"[StyleHolderSO] Not style found for class of type ({type})");
            return null;
        }

        /* GlobalStyle getter */
        public StyleSheet GetGlobalStyle()
        {
            return globalStyle;
        }

        /* GlobalStyle setter */
        public void SetGlobalStyle(StyleSheet style)
        {
            globalStyle = style;
        }
    }
}