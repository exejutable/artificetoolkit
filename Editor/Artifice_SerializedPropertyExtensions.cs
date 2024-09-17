using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ArtificeToolkit.Attributes;
using UnityEditor;
using UnityEngine;

// ReSharper disable InvertIf

namespace ArtificeToolkit.Editor
{
    public static class Artifice_SerializedPropertyExtensions
    {
        private static readonly Regex ArrayIndexCapturePattern = new Regex(@"\[(\d*)\]");

        /// <summary> This method calls GetTarget but casts the result using generics for better usability </summary>
        public static T GetTarget<T>(this SerializedProperty property)
        {
            return (T)GetTarget(property);
        }
        
        public static Type GetTargetType(this SerializedProperty property)
        {
            var propertyNames = property.propertyPath.Split('.');
            
            object target = property.serializedObject.targetObject;
            var targetType = target.GetType();
            
            var isNextPropertyArrayIndex = false;
            for (var i = 0; i < propertyNames.Length && target != null; ++i)
            {
                var propName = propertyNames[i];

                if (propName == "Array")
                {
                    isNextPropertyArrayIndex = true;
                }
                else if (isNextPropertyArrayIndex)
                {
                    isNextPropertyArrayIndex = false;
                    var arrayIndex = ParseArrayIndex(propName);
                    if (target is IList targetAsArray)
                        target = targetAsArray[arrayIndex];
                }
                else
                {
                    targetType = GetFieldType(target, propName);
                    target = GetField(target, propName);
                }
            }

            return targetType;
        }

        /// <summary>
        /// This method uses reflection to return the objectReference of the serialized property parameter.
        /// In contrast with property.objectReference, it can return even nested values within an array or list.
        /// </summary>
        private static object GetTarget(this SerializedProperty property)
        {
            var propertyNames = property.propertyPath.Split('.');
            object target = property.serializedObject.targetObject;
            var isNextPropertyArrayIndex = false;
            for (var i = 0; i < propertyNames.Length && target != null; ++i)
            {
                var propName = propertyNames[i];
                if (propName == "Array")
                {
                    isNextPropertyArrayIndex = true;
                }
                else if (isNextPropertyArrayIndex)
                {
                    isNextPropertyArrayIndex = false;
                    var arrayIndex = ParseArrayIndex(propName);
                    if (target is IList targetAsArray)
                        target = targetAsArray[arrayIndex];
                }
                else
                {
                    target = GetField(target, propName);
                }
            }

            return target;
        }
        
        /// <summary> Utility method for GetTarget </summary>
        private static object GetField(object target, string name, Type targetType = null)
        {
            if (targetType == null)
                targetType = target.GetType();

            FieldInfo fi = null;
            while (targetType != null)
            {
                fi = targetType.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fi != null)
                    return fi.GetValue(target);

                targetType = targetType.BaseType;
            }

            return null;
        }
        
        /// <summary> Utility method for GetTarget </summary>
        private static Type GetFieldType(object target, string name)
        {
            var fi = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fi != null)
                return fi.FieldType;

            return null;
        }
        
        /// <summary> Utility method for GetTarget </summary>
        private static int ParseArrayIndex(string propertyName)
        {
            var match = ArrayIndexCapturePattern.Match(propertyName);
            if (!match.Success)
                throw new Exception($"Invalid array index parsing in {propertyName}");

            return int.Parse(match.Groups[1].Value);
        }
        
        /* Helper method to get the value of a SerializedProperty */
        public static object GetSerializedPropertyValue(this SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    return property.boolValue;
                case SerializedPropertyType.Float:
                    return property.floatValue;
                case SerializedPropertyType.Integer:
                    return property.intValue;
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue;
                case SerializedPropertyType.Enum:
                    return property.enumValueFlag;
                default:
                    throw new ArgumentException();
            }
        }
        
        /// <summary> Returns an array of any <see cref="CustomAttribute"/> found in the property. Otherwise returns null. </summary>
        public static Attribute[] GetAttributes(this SerializedProperty property)
        {
            var fieldInfo = GetFieldNested(property.serializedObject.targetObject, property.propertyPath);

            if (fieldInfo != null)
            {
                // var attributes = (CustomAttribute[])fieldInfo.GetCustomAttributes(typeof(CustomAttribute), true);
                return (Attribute[])fieldInfo.GetCustomAttributes(true);
            }

            return null;
        }
        
        /// <summary> Returns an array of any <see cref="CustomAttribute"/> found in the property. Otherwise returns null. </summary>
        public static CustomAttribute[] GetCustomAttributes(this SerializedProperty property)
        {
            var fieldInfo = GetFieldNested(property.serializedObject.targetObject, property.propertyPath);

            if (fieldInfo != null)
            {
                var attributes = (CustomAttribute[])fieldInfo.GetCustomAttributes(typeof(CustomAttribute), true);
                return attributes;
            }

            return null;
        }

        /// <summary>Gets visible children of a <see cref="SerializedProperty"/> at 1 level depth.</summary>
        public static List<SerializedProperty> GetVisibleChildren(this SerializedProperty property)
        {
            var it = property.Copy();

            var list = new List<SerializedProperty>();
            
            if (it.NextVisible(true))
            {
                // If depth is same or bigger, iterator had no children.
                if (it.depth <= property.depth)
                    return list;
                
                do
                { 
                    list.Add(it.Copy());
                }
                while (it.NextVisible(false) && it.depth > property.depth);
            }

            return list;
        }
        
        /// <summary> Returns the field info of a target object based on the path </summary>
        private static FieldInfo GetFieldNested(object target, string path)
        {
            var fields = path.Split('.');
            var isNextPropertyArrayIndex = false;

            for (int i = 0; i < fields.Length - 1; ++i)
            {
                string propName = fields[i];
                if (propName == "Array")
                {
                    isNextPropertyArrayIndex = true;
                }
                else if (isNextPropertyArrayIndex)
                {
                    isNextPropertyArrayIndex = false;
                    var index = ParseArrayIndex(propName);
                    var targetAsList = target as IList;
                    if (targetAsList != null && targetAsList.Count > index)
                        target = targetAsList[index];
                }
                else
                    target = GetField(target, propName);
            }
            
            FieldInfo fieldInfo = null;
            if (target != null)
            {
                Type targetType = target.GetType();
                while (targetType != null)
                {
                    fieldInfo = targetType.GetField(fields[^1], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (fieldInfo != null)
                        return fieldInfo;

                    targetType = targetType.BaseType;
                }
            }

            return fieldInfo;
        }
        
        /// <summary> Returns a serialized property in the same scope </summary>
        public static SerializedProperty FindPropertyInSameScope(this SerializedProperty property, string propertyName)
       {
           var path = property.propertyPath.Split('.');
           path[^1] = propertyName;

           var newPath = String.Join('.', path);
           return property.serializedObject.FindProperty(newPath);
       }
       
        /// <summary> Returns a serialized property in the same scope </summary>
        public static SerializedProperty FindParentProperty(this SerializedProperty property)
        {
            var path = property.propertyPath.Split('.');
            Array.Resize(ref path, path.Length - 1);
            var newPath = String.Join('.', path);
            return property.serializedObject.FindProperty(newPath);
        }

        /// <summary> Returns array children type from array property. </summary>
        public static Type GetArrayChildrenType(this SerializedProperty property)
        {
            Debug.Assert(property.isArray && property.propertyType != SerializedPropertyType.String, "Property must be an array.");

            Type returnValue = null;

            if (property.arraySize == 0)
            {
                property.InsertArrayElementAtIndex(0);
                property.serializedObject.ApplyModifiedProperties();

                var list = property.GetTarget<object>();
                if (list.GetType().IsArray) // Property is an array
                {
                    returnValue = list.GetType().GetElementType();
                }
                else // Property is a list or serializable collection
                    returnValue = list.GetType().GetGenericArguments().Single();
                
                property.ClearArray();
                property.serializedObject.ApplyModifiedProperties();
            }
            else
                returnValue = property.GetArrayElementAtIndex(0).GetTarget<object>().GetType();
            
            return returnValue;
        }
        
        /// <summary> Utility method to more clearly check for array properties. This was needed because string values, are considered arrays. </summary>
        public static bool IsArray(this SerializedProperty property)
        {
            // String typed SerializedProperties are by default considered arrays.
            // So use this utility method as a cleaner way to check for actual property arrays
            return property.isArray && property.propertyType != SerializedPropertyType.String;
        }

        /// <summary> Returns true if property is part of an array </summary>
        public static bool IsArrayElement(this SerializedProperty property)
        {
            return property.propertyPath.Contains("Array");
        } 
        
        /// <summary> Returns index of property in its array or -1 if its not in an array </summary>
        public static int GetIndexInArray(this SerializedProperty property)
        {
            if (!property.IsArrayElement())
                return -1;
            int startIndex = property.propertyPath.LastIndexOf('[') + 1;
            int length = property.propertyPath.LastIndexOf(']') - startIndex;
            return int.Parse(property.propertyPath.Substring(startIndex, length));
        }
        
        /// <summary> Returns the string value of the underlying serialized property. </summary>
        public static string GetValueString(this SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return property.intValue.ToString();
                case SerializedPropertyType.Boolean:
                    return property.boolValue.ToString();
                case SerializedPropertyType.Float:
                    return property.floatValue.ToString();
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue != null ? property.objectReferenceValue.ToString() : "null";
                case SerializedPropertyType.Enum:
                    return property.enumDisplayNames[property.enumValueIndex];
                case SerializedPropertyType.Vector2:
                    return property.vector2Value.ToString();
                case SerializedPropertyType.Vector3:
                    return property.vector3Value.ToString();
                case SerializedPropertyType.Color:
                    return property.colorValue.ToString();
                case SerializedPropertyType.Rect:
                    return property.rectValue.ToString();
                case SerializedPropertyType.Bounds:
                    return property.boundsValue.ToString();
                default:
                    return $"Unsupported {property.propertyType.ToString()}";
            }
        }
     
        /// <summary> Copies the value of direct value of a serialized property if their SerializedPropertyType match.</summary>
        public static void Copy(this SerializedProperty property, SerializedProperty targetProperty)
        {
            if (targetProperty == null || property.propertyType != targetProperty.propertyType)
            {
                Debug.LogWarning("Cannot paste: mismatched property types or no copied value.");
                return;
            }

            // Paste based on the type of the copied property
            switch (targetProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = targetProperty.intValue;
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = targetProperty.boolValue;
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = targetProperty.floatValue;
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = targetProperty.stringValue;
                    break;
                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = targetProperty.objectReferenceValue;
                    break;
                case SerializedPropertyType.Color:
                    property.colorValue = targetProperty.colorValue;
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = targetProperty.vector2Value;
                    break;
                case SerializedPropertyType.Vector3:
                    property.vector3Value = targetProperty.vector3Value;
                    break;
                case SerializedPropertyType.Vector4:
                    property.vector4Value = targetProperty.vector4Value;
                    break;
                case SerializedPropertyType.Rect:
                    property.rectValue = targetProperty.rectValue;
                    break;
                case SerializedPropertyType.Bounds:
                    property.boundsValue = targetProperty.boundsValue;
                    break;
                case SerializedPropertyType.Enum:
                    property.enumValueFlag = targetProperty.enumValueFlag;
                    break;
                default:
                    Debug.LogWarning($"Unsupported property type for paste: {targetProperty.propertyType}");
                    break;
            }

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}