using System;
using AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes;
using AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CommonResources;
using AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_VisualElements.AbzEditor_VisualElement_InfoBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbzorbaExportRoot.Editor.ArtificeToolkit.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_InfoBoxAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(Abz_InfoBoxAttribute))]
    public class Artifice_CustomAttributeDrawer_InfoBoxAttribute : Artifice_CustomAttributeDrawer
    {
        public override VisualElement OnWrapGUI(SerializedProperty property, VisualElement root)
        {
            var container = new VisualElement();
            var attribute = (Abz_InfoBoxAttribute)Attribute;
            container.Add(new Artifice_VisualElement_InfoBox(attribute.Message, LoadSpriteByType(attribute.Type))); 
            container.Add(root);
            
            return container;
        }

        private Sprite LoadSpriteByType(Abz_InfoBoxAttribute.InfoMessageType type)
        {
            switch (type)
            {
                case Abz_InfoBoxAttribute.InfoMessageType.Info:
                    return Artifice_SCR_CommonResourcesHolder.instance.CommentIcon;
                case Abz_InfoBoxAttribute.InfoMessageType.Warning:
                    return Artifice_SCR_CommonResourcesHolder.instance.WarningIcon;
                case Abz_InfoBoxAttribute.InfoMessageType.Error:
                    return Artifice_SCR_CommonResourcesHolder.instance.ErrorIcon;
                case Abz_InfoBoxAttribute.InfoMessageType.None:
                    return null;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
