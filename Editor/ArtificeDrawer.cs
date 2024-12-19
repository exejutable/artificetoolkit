using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArtificeToolkit.Attributes;
using ArtificeToolkit.AEditor.Artifice_CustomAttributeDrawers;
using ArtificeToolkit.AEditor.Artifice_CustomAttributeDrawers.CustomAttributeDrawer_ButtonAttribute;
using CustomAttributes;
using Editor.Artifice_VisualElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using SpaceAttribute = ArtificeToolkit.Attributes.SpaceAttribute;

// ReSharper disable GCSuppressFinalizeForTypeWithoutDestructor
// ReSharper disable CanSimplifyDictionaryLookupWithTryGetValue
// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable RedundantIfElseBlock

namespace ArtificeToolkit.AEditor
{
    [InitializeOnLoad]
    public sealed class ArtificeDrawer : IDisposable
    {
        #region FIELDS

        private readonly Stack<IDisposable> _disposableStack = new();
        private bool _disposed;

        // Cached results for custom attribute usage
        private readonly Dictionary<SerializedProperty, bool> _isUsingCustomAttributesCache = new();
        private readonly Dictionary<SerializedProperty, bool> _doChildrenUseCustomAttributesCache = new();

        /// <summary> Specific attributes are meant to be passed upon an array's children, and not affect the array itself. This is what this HashSet defines. </summary>
        public static readonly HashSet<Type> ArrayAppliedCustomAttributes;

        /// <summary> String properties that should be ignored from Artifice. </summary>
        private static readonly HashSet<string> PropertyIgnoreSet;

        // Delegate declaration for serialized property filter method.
        public delegate bool SerializedPropertyFilter(SerializedProperty property);
        private SerializedPropertyFilter _serializedPropertyFilter = property => true;
        
        #endregion

        /// <summary> Static constructor initializes ArrayAppliedCustomAttributes variable since its reused for all artifice drawer instances. </summary>
        static ArtificeDrawer()
        {
            // Refresh toggle of artifice drawer to secure consistency throughout package updates.
            Artifice_Utilities.ToggleArtificeDrawer(Artifice_Utilities.ArtificeDrawerEnabled);
            
            // Initialize array applied custom attributes
            ArrayAppliedCustomAttributes = new HashSet<Type>
            {
                typeof(BoxGroupAttribute),
                typeof(HorizontalGroupAttribute),
                typeof(VerticalGroupAttribute),
                typeof(FoldoutGroupAttribute),
                typeof(TabGroupAttribute),
                typeof(EnableIfAttribute),
                typeof(InvokeOnValueChangeAttribute), 
                typeof(SpaceAttribute), 
                typeof(TitleAttribute), 
                typeof(HideLabelAttribute),
                typeof(InfoBoxAttribute), 
                typeof(ConditionalInfoBoxAttribute),
                typeof(ListElementNameAttribute)
            };

            PropertyIgnoreSet = new HashSet<string>()
            {
                // "Serialized Data Mode Controller",
                "Serialized Data Mode Controller",
            };
        }
        
        /// <summary> Returns the ArtificeInspector of a SerializedObject. </summary>
        public VisualElement CreateInspectorGUI(SerializedObject serializedObject)
        {
            // Do nothing while compiling.
            if (EditorApplication.isCompiling)
                return new VisualElement();
            
            // Make sure serialized object is updated
            serializedObject.Update(); 
            
            // Create initialized artifice inspector container
            var artificeInspector = CreateArtificeInspectorContainerGUI(serializedObject);
            
            // Check whether target object is missing
            if (serializedObject.targetObject == null)
            {
                artificeInspector.Add(CreateScriptMissingUI(serializedObject));
                return artificeInspector;
            }
            
            // Fully render out its visible children properties
            foreach (var property in serializedObject.GetIterator().GetVisibleChildren())
            {
                if (PropertyIgnoreSet.Contains(property.displayName))
                    continue;
                
                artificeInspector.Add(CreatePropertyGUI(property.Copy()));
            }

            // Create optional method buttons Foldout Group for serializedObject
            artificeInspector.Add(CreateMethodsGUI(serializedObject));
            
            // Apply any modified property
            serializedObject.ApplyModifiedProperties(); 

            return artificeInspector;
        }

        /// <summary> Returns an initialized VisualElement container to be used for the Artifice inspector </summary>
        private VisualElement CreateArtificeInspectorContainerGUI(SerializedObject serializedObject)
        {
            var artificeContainer = new VisualElement
            {
                name = serializedObject.GetHashCode().ToString()
            };

            // If for some reason this occurs, the Inspector would be empty and not easily debuggable.
            // Hopefully, the thrown exception will help pinpoint what went wrong.
            if (Artifice_Utilities.GetGlobalStyle() == null || Artifice_Utilities.GetStyle(GetType()) == null)
                throw new Exception("GlobalStyle or ArtificeStyle not found.");

            artificeContainer.styleSheets.Add(Artifice_Utilities.GetGlobalStyle()); // This propagates to all children.
            artificeContainer.styleSheets.Add(Artifice_Utilities.GetStyle(GetType())); // Supports

            if (IsUsingCustomAttributes(serializedObject))
                artificeContainer.Add(CreateArtificeIndicatorGUI(serializedObject));
            
            return artificeContainer;
        }

        /// <summary> Receives a SerializedProperty as a parameter and returns its Artifice GUI </summary>
        public VisualElement CreatePropertyGUI(SerializedProperty property, bool forceArtificeStyle = false)
        {
            var container = new VisualElement();
            container.AddToClassList("property-container");

            // If filtered, return empty container.
            if (_serializedPropertyFilter.Invoke(property) == false)
                return null;
            
            // Check if property enforces Artifice in following calls.
            var customAttributes = property.GetCustomAttributes();
            if (customAttributes != null && customAttributes.Any(attribute => attribute is ForceArtificeAttribute))
                forceArtificeStyle = true;
            
            if (ShouldUseArtificeEditorForProperty(property) || forceArtificeStyle)
            {
                // Arrays need to use custom Artifice List Views (and not a string value!)
                if (property.IsArray())
                {
                    // Discern which properties are to be applied to the list and which to its children.
                    SplitCustomPropertiesForArrays(property, out var arrayCustomAttributes, out var childrenCustomAttributes);
                    
                    // Check whether it should be drawn with table list
                    var isTableList = property.GetAttributes().Any(attribute => attribute.GetType() == typeof(TableListAttribute));
                        
                    // Spawn either ListView or TableView
                    var listView = isTableList ? (Artifice_VisualElement_AbstractListView)new Artifice_VisualElement_TableListView() : new Artifice_VisualElement_ListView();
                    listView.SetSerializedPropertyFilter(_serializedPropertyFilter);
                    listView.SetChildrenInjectedCustomAttributes(childrenCustomAttributes);
                    listView.value = property;
                    container.Add(CreateCustomAttributesGUI(property, listView, arrayCustomAttributes));
                    
                    _disposableStack.Push(listView); // Add to disposable list
                }
                // If property has visible children, wrap it in a foldout to mimic unity's default behaviour or use any potential implemented custom property drawer.
                else if (property.hasVisibleChildren)
                {
                    var hasCustomPropertyDrawer = Artifice_CustomDrawerUtility.HasCustomDrawer(property);
                    if (hasCustomPropertyDrawer)
                    {
                        var customPropertyField = Artifice_CustomDrawerUtility.CreatePropertyGUI(property);
                        container.Add(customPropertyField);
                    }
                    else
                    {
                        var foldout = new Foldout
                        {
                            value = property.isExpanded,
                            text = property.displayName
                        };
                        foldout.AddToClassList("nested-field-property");
                        foldout.BindProperty(property); // Bind to make foldout state (open-closed) be persistent

                        // Create property for each child
                        foreach (var child in property.GetVisibleChildren())
                            foldout.Add(CreatePropertyGUI(child, forceArtificeStyle));
                        
                        // Create methods group
                        foldout.Add(CreateMethodsGUI(property));
                        
                        container.Add(CreateCustomAttributesGUI(property, foldout));
                    }
                }
                else
                {
                    var defaultPropertyField = new PropertyField(property);
                    defaultPropertyField.BindProperty(property);
                    container.Add(CreateCustomAttributesGUI(property, defaultPropertyField));
                }
            }
            else
                container.Add(CreateIMGUIField(property));

            return container;
        }
        
        /// <summary> Uses <see cref="CustomAttribute"/> and <see cref="Artifice_CustomAttributeDrawer"/> to change how the parameterized <see cref="VisualElement"/> will look like using the property's custom attributes. </summary>
        public VisualElement CreateCustomAttributesGUI(SerializedProperty property, VisualElement propertyField)
        {
            var customAttributes = property.GetCustomAttributes();
            if (IsUsingCustomAttributes(property) == false)
                return propertyField;

            return CreateCustomAttributesGUI(property, propertyField, customAttributes.ToList());
        }
        
        /// <summary> Uses <see cref="CustomAttribute"/> and <see cref="Artifice_CustomAttributeDrawer"/> to change how the parameterized <see cref="VisualElement"/> will look like with any parameterized custom attributes. </summary>
        public VisualElement CreateCustomAttributesGUI(SerializedProperty property, VisualElement propertyField, List<CustomAttribute> customAttributes)
        {
            var attributeDrawers = new List<Artifice_CustomAttributeDrawer>();
            var drawerMap = Artifice_Utilities.GetDrawerMap();
            foreach (var customAttribute in customAttributes)
            {
                // Skip if drawer does not exist for custom attribute
                if(drawerMap.ContainsKey(customAttribute.GetType()) == false)
                    continue;
                
                // Create instance of drawer
                var attributeDrawer = (Artifice_CustomAttributeDrawer)Activator.CreateInstance(drawerMap[customAttribute.GetType()]);
                attributeDrawer.Attribute = customAttribute;
                attributeDrawers.Add(attributeDrawer);
                _disposableStack.Push(attributeDrawer); // Add to disposable stack
            }

            // Copy property because param is an iterator which will move on.
            var rootVisualElement = new VisualElement
            {
                name = property.propertyPath
            };

            // PRE GUI
            foreach (var eachAttributeDrawer in attributeDrawers)
                rootVisualElement.Add(eachAttributeDrawer.OnPrePropertyGUI(property));

            // ON GUI       (Adds first OnPropertyGUI implementation only)
            var propertyReplacementDrawer = attributeDrawers.FirstOrDefault(drawer => drawer.IsReplacingPropertyField);
            propertyField = propertyReplacementDrawer != null ? propertyReplacementDrawer.OnPropertyGUI(property) : propertyField;
            rootVisualElement.Add(propertyField);

            // POST GUI  
            foreach (var drawer in attributeDrawers)
                rootVisualElement.Add(drawer.OnPostPropertyGUI(property));

            // WRAP GUI     (Order matters a lot!)
            // // Reverse order does not break it but not 100% sure why. In correct order it just loops.
            var wrapper = rootVisualElement;
            for (var i = attributeDrawers.Count - 1; i >= 0; i--)
                wrapper = attributeDrawers[i].OnWrapGUI(property, wrapper);

            // ON PROPERTY BOUND GUI
            propertyField?.schedule.Execute(() =>
            {
                foreach (var drawer in attributeDrawers)
                    drawer.OnPropertyBoundGUI(property, propertyField);
            });

            return wrapper;
        }

        /// <summary> Uses <see cref="IMGUIContainer"/> to create the default UI implementation Unity would have offered. </summary>
        private VisualElement CreateIMGUIField(SerializedProperty property)
        {
            // Fallback to default IMGUI properties
            var guiContainer = new IMGUIContainer();
            guiContainer.onGUIHandler = () => CreateIMGUIFieldHandler(property);
            return guiContainer;
        }

        /// <summary> Used by <see cref="CreateIMGUIField"/> to create IMGUI. </summary>
        private void CreateIMGUIFieldHandler(SerializedProperty property)
        {
            property.serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
                
            // Create dummy rect of zero height, to get width of current available rect
            var rect = EditorGUILayout.GetControlRect(true, 0f);
            var viewWidth = rect.width;

            // Cache previous label width
            var previousLabelWidth = EditorGUIUtility.labelWidth;

            // Minimum label width is 123. Else, set 33% of the available width as label.
            EditorGUIUtility.labelWidth = Mathf.Max((viewWidth) * 0.33f, 123);

            // Draw property field
            EditorGUILayout.PropertyField(property);
                
            // Restore label width for custom IMGUI implementations like lists
            EditorGUIUtility.labelWidth = previousLabelWidth;   
                
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
        }

        /// <summary> Returns a <see cref="VisualElement"/> with buttons which invoke the methods marked with the <see cref="ButtonAttribute"/>. </summary>
        /// <remarks> Unfortunately, there is not unified structure for SerializedObject and SerializedProperty. A template is used here to avoid deduplicate method overloads. </remarks>
        private VisualElement CreateMethodsGUI<T>(T serializedData) where T : class
        {
            // Obtain the target type depending on the serializedData type.
            var targetType = serializedData switch
            {
                SerializedObject serializedObject => serializedObject.targetObject.GetType(),
                SerializedProperty serializedProperty => serializedProperty.GetTarget<object>().GetType(),
                _ => throw new ArgumentException("Invalid serialized data type.")
            };

            // Get name to show in sliding group title based on serialized data type.
            var slidingGroupTitle = serializedData switch
            {
                SerializedObject serializedObject => serializedObject.targetObject.GetType().Name,
                SerializedProperty serializedProperty => serializedProperty.displayName,
                _ => throw new ArgumentException("Invalid serialized data type.")
            };
            
            // Create main container to return, containing both a list of buttons and a sliding group.
            var container = new VisualElement();
            container.AddToClassList("property-container");

            // Some methods may be in a sliding group. Optional.
            var slidingGroup = new Artifice_VisualElement_SlidingGroup();
            slidingGroup.SetTitle($"{slidingGroupTitle}: Actions");
            slidingGroup.AddToClassList("method-group-container");
            
            var methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                // Get button attribute
                var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
                if (buttonAttribute == null)
                    continue;

                // Create dedicated drawer for it
                var buttonCustomDrawer = new Artifice_CustomAttributeDrawer_ButtonAttribute();
                buttonCustomDrawer.Attribute = buttonAttribute;

                // Create the method GUI using serializedData
                var button = buttonCustomDrawer.CreateMethodGUI(serializedData, method);
                button.AddToClassList("method-button");

                if (buttonAttribute.ShouldAddOnSlidingPanel)
                    slidingGroup.Add(button);
                else
                    container.Add(button);
            }

            // If sliding group is not empty, add it to the container last.
            if (slidingGroup.childCount > 0)
                container.Add(slidingGroup);

            return container.childCount > 0 ? container : null;
        }
        
        /// <summary> Returns an interactable visual indicator to determine whether ArtificeDrawer is enabled or not </summary>
        private VisualElement CreateArtificeIndicatorGUI(SerializedObject serializedObject)
        {
            var indicator = new VisualElement();
            indicator.AddToClassList("artifice-indicator");
            indicator.AddToClassList(Artifice_Utilities.ArtificeDrawerEnabled ? "indicator-enabled" : "indicator-disabled");

            indicator.RegisterCallback<ClickEvent>(evt =>
            {
                Artifice_Utilities.ToggleArtificeDrawer(!Artifice_Utilities.ArtificeDrawerEnabled);
                indicator.RemoveFromClassList(!Artifice_Utilities.ArtificeDrawerEnabled ? "indicator-enabled" : "indicator-disabled");
                indicator.AddToClassList(Artifice_Utilities.ArtificeDrawerEnabled ? "indicator-enabled" : "indicator-disabled");
            });
            
            indicator.tooltip = "Green: ArtificeDrawer is enabled.\nRed: ArtificeDrawer is disabled\n\nClick to toggle.\nNote: Inspector redraw is required.";
            
            return indicator;
        }
        
        #region Utility Methods

        public void SetSerializedPropertyFilter(SerializedPropertyFilter filter)
        {
            _serializedPropertyFilter = filter;
        }
        
        /// <summary> Returns true if property directly or indirectly uses any custom attributes </summary>
        private bool ShouldUseArtificeEditorForProperty(SerializedProperty property)
        {
            return IsUsingCustomAttributes(property) || DoChildrenUseCustomAttributes(property);
        }
        
        /// <summary> Returns true if property directly or indirectly uses any custom attributes </summary>
        private bool IsUsingCustomAttributes(SerializedObject serializedObject)
        {
            return serializedObject.GetIterator().GetVisibleChildren().Any(property => IsUsingCustomAttributes(property) || DoChildrenUseCustomAttributes(property));
        }
        
        /// <summary> Returns true if the property is directly using any <see cref="CustomAttribute"/> </summary>
        private bool IsUsingCustomAttributes(SerializedProperty property)
        {
            if (_isUsingCustomAttributesCache.ContainsKey(property))
                return _isUsingCustomAttributesCache[property];
            
            // Check if property directly has a custom attribute
            var customAttributes = property.GetCustomAttributes();
            if (customAttributes != null && customAttributes.Length > 0)
            {
                _isUsingCustomAttributesCache[property] = true;
                return true;
            }
            
            var obj = property.GetTarget<object>();
            if (obj != null)
            {
                foreach(var method in obj.GetType().GetMethods())
                    if (method.GetCustomAttributes().Any(attribute => attribute is CustomAttribute))
                    {
                        _isUsingCustomAttributesCache[property] = true;
                        return true;
                    }
            }
          
            return false;
        }

        /// <summary> Returns true if any nested child is using any <see cref="CustomAttribute"/> </summary>
        private bool DoChildrenUseCustomAttributes(SerializedProperty property)
        {
            if (_doChildrenUseCustomAttributesCache.ContainsKey(property))
                return _doChildrenUseCustomAttributesCache[property];

            if (property.serializedObject.targetObject == null)
                return false;
            
            // No point if no children and not array
            if (!property.hasVisibleChildren)
                return false;
            
            // Special case were property is array and empty
            if (property.IsArray() && property.arraySize == 0)
            {
                var arrayElementTypeString = property.arrayElementType;

                // Extracting the type name
                var typeName = arrayElementTypeString.Replace("PPtr<$", "").Replace(">", "");

                // Finding the type using reflection through all assemblies
                var arrayElementType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .FirstOrDefault(type => type.FullName == typeName || type.Name == typeName);

                return arrayElementType != null && DoChildrenOfTypeUseCustomAttributes(arrayElementType);
            }
            
            // Otherwise, check ALL children
            foreach (var visibleChild in property.GetVisibleChildren())
            {
                // If property is array skip the size property when determining artifice usage.
                if(property.propertyType == SerializedPropertyType.ArraySize)
                    continue;
                
                if (ShouldUseArtificeEditorForProperty(visibleChild))
                {
                    _doChildrenUseCustomAttributesCache[property] = true;
                    return true;
                }
            }
            
            _doChildrenUseCustomAttributesCache[property] = false;
            return false;
        }
        
        /// <summary> Returns true if any nested field is using any <see cref="CustomAttribute"/> </summary>
        private bool DoChildrenOfTypeUseCustomAttributes(Type type)
        {
            // Create  queue and already-searched structures for BFS
            var queue = new Queue<FieldInfo>();
            var alreadySearched = new HashSet<FieldInfo>();
            
            // Inject into the queue all the direct children fields of type.
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                queue.Enqueue(field);

            while (queue.Count > 0)
            {
                var currentField = queue.Dequeue();

                // Check if already searched to skip
                if (alreadySearched.Contains(currentField))
                    continue;
                alreadySearched.Add(currentField);
                
                // Check if it uses custom attributes
                var customAttributes = currentField.GetCustomAttributes().ToArray();
                if (customAttributes.Length > 0)
                    return true;
                
                // Add nested fields in queue to search
                foreach(var field in currentField.FieldType.GetFields())
                    queue.Enqueue(field);
            }

            return false;
        }
        
        /// <summary> Some <see cref="CustomAttribute"/> on lists are meant to be passed along its children, instead of the list it self. This method splits them and provides them as out parameters. </summary>
        private void SplitCustomPropertiesForArrays(SerializedProperty property, out List<CustomAttribute> arrayCustomAttributes, out List<CustomAttribute> childrenCustomAttributes)
        {
            // Create new lists
            arrayCustomAttributes = new List<CustomAttribute>();
            childrenCustomAttributes = new List<CustomAttribute>();
            
            // Get property attributes and parse-split them
            var attributes = property.GetCustomAttributes();
            foreach (var attribute in attributes)
                if(ArrayAppliedCustomAttributes.Contains(attribute.GetType()))
                    arrayCustomAttributes.Add(attribute);
                else
                    childrenCustomAttributes.Add(attribute);
        }
        
        /// <summary> Returns a VisualElement notifying for a missing script error </summary>
        private VisualElement CreateScriptMissingUI(SerializedObject serializedObject)
        {
            var container = new VisualElement();

            container.Add(new PropertyField(serializedObject.FindProperty("m_Script")));

            var labelContainer = new VisualElement();
            labelContainer.AddToClassList("label-container");
            container.Add(labelContainer);

            var flavorIconLabel = new Label(":'(");
            // var flavorIconLabel = new Label(":(");
            flavorIconLabel.AddToClassList("flavor-icon");
            labelContainer.Add(flavorIconLabel);

            var textLabel = new Label("The associated script can not be loaded. Please fix any compile errors and assign a valid script.");
            textLabel.AddToClassList("text");
            labelContainer.Add(textLabel);

            return container;
        }
        
        #endregion

        #region Dispose Pattern

        ~ArtificeDrawer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true); // Dispose of unmanaged resources.
            GC.SuppressFinalize(this); // Suppress finalization.
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            // Dispose managed resources
            if (disposing)
            {
                while (_disposableStack.Count > 0)
                    _disposableStack.Pop().Dispose();

                _isUsingCustomAttributesCache.Clear();
                _doChildrenUseCustomAttributesCache.Clear();
            }

            _disposed = true;
        }

        #endregion
    }
}