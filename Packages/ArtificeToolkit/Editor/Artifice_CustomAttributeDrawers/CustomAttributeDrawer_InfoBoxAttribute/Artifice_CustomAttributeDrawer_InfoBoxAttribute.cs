using System;
using CustomAttributes;
using Editor.Artifice_CommonResources;
using Editor.Artifice_VisualElements.Artifice_VisualElement_InfoBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_InfoBoxAttribute
{
    [Artifice_CustomAttributeDrawer(typeof(InfoBoxAttribute))]
    public class Artifice_CustomAttributeDrawer_InfoBoxAttribute : Artifice_CustomAttributeDrawer
    {
        public override VisualElement OnWrapGUI(SerializedProperty property, VisualElement root)
        {
            var container = new VisualElement();
            var attribute = (InfoBoxAttribute)Attribute;
            container.Add(new Artifice_VisualElement_InfoBox(attribute.Message, LoadSpriteByType(attribute.Type))); 
            container.Add(root);
            
            return container;
        }

        private Sprite LoadSpriteByType(InfoBoxAttribute.InfoMessageType type)
        {
            switch (type)
            {
                case InfoBoxAttribute.InfoMessageType.Info:
                    return Artifice_SCR_CommonResourcesHolder.instance.CommentIcon;
                case InfoBoxAttribute.InfoMessageType.Warning:
                    return Artifice_SCR_CommonResourcesHolder.instance.WarningIcon;
                case InfoBoxAttribute.InfoMessageType.Error:
                    return Artifice_SCR_CommonResourcesHolder.instance.ErrorIcon;
                case InfoBoxAttribute.InfoMessageType.None:
                    return null;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
