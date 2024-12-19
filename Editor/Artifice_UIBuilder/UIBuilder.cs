using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

// ReSharper disable InvertIf

namespace ArtificeToolkit.AEditor
{
    /// <summary>Helper class to compartmentalize rebuilding of VisualElements.</summary>
    /// <remarks>This way we avoid rebuilding everything when a change e.g. OrientationChange occurs.</remarks>
    public class UIBuilder
    {
        /* NESTED CLASS */
        private class UIBuilderNode
        {
            public string Name = "undef";
            public VisualElement VisualElement;
            public Action<VisualElement> BuildAction;
            public bool ShouldRebuild = true;
            public bool IsBuild;
        }

        #region FIELDS

        private readonly Dictionary<string, UIBuilderNode> _nodeMap = new Dictionary<string, UIBuilderNode>();

        #endregion

        public T Create<T>(
            string name,
            Action<VisualElement> initializeAction = null,
            Action<VisualElement> buildAction = null,
            bool shouldRebuild = true
        ) where T : VisualElement
        {
            if (_nodeMap.TryGetValue(name, out var queryTest) == false)
            {
                // Create node
                var newNode = new UIBuilderNode
                {
                    Name = name,
                    VisualElement = Activator.CreateInstance<T>(),
                    BuildAction = buildAction,
                    ShouldRebuild = shouldRebuild
                };
                newNode.VisualElement.name = name;

                // Initialize VE.
                initializeAction?.Invoke(newNode.VisualElement);

                _nodeMap.Add(name, newNode);
            }

            return (T)BuildUI(name);
        }

        /* Do not delete. Will be used for optimization purposes at least. */
        public bool UpdateKey(string previousKey, string newKey)
        {
            if (_nodeMap.ContainsKey(newKey) || !_nodeMap.ContainsKey(previousKey))
                return false;

            _nodeMap[newKey] = _nodeMap[previousKey];
            _nodeMap.Remove(previousKey);
            return true;
        }

        private VisualElement BuildUI(string name)
        {
            // Get node and build
            var node = _nodeMap[name];

            // Clean on rebuilds
            if (node.ShouldRebuild)
                node.VisualElement.Clear();

            // Invoke build logic
            if (node.IsBuild == false || node.ShouldRebuild)
            {
                node.BuildAction?.Invoke(node.VisualElement);
                node.IsBuild = true;
            }

            return node.VisualElement;
        }
    }
}