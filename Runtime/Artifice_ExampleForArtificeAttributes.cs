using System;
using System.Collections.Generic;
using ArtificeToolkit.Attributes;
using UnityEngine;

/// <summary> Example script for <see cref="CustomAttribute"/> </summary>
public class Artifice_ExampleForArtificeAttributes : MonoBehaviour
{
    [Serializable]
    public class Character
    {
        [Serializable]
        public class AbilityScores
        {
            [ArtificeToolkit.Attributes.Range(0, 20)] public int strength;
            [ArtificeToolkit.Attributes.Range(0, 20)] public int dexterity;
            [ArtificeToolkit.Attributes.Range(0, 20)] public int constitution;
            [ArtificeToolkit.Attributes.Range(0, 20)] public int wisdom;
            [ArtificeToolkit.Attributes.Range(0, 20)] public int intelligence;
            [ArtificeToolkit.Attributes.Range(0, 20)] public int charisma;
        }

        [Serializable]
        public class Item
        {
            public enum WeaponType
            {
                Longsword,
                Rapier,
                Bow,
                Axe
            }
                
            [MinValue(0)] public int amount;
            [EnumToggle] public WeaponType weaponType;
        }

        [Serializable]
        public class Skills
        {
            [MinValue(0)] public int athletics;
            [MinValue(0)] public int acrobatics;
            [MinValue(0)] public int arcana;
            [MinValue(0)] public int deception;
        }
            
        [Title("Character Name"), HideLabel]
        public string name;
            
        [Required, PreviewSprite, HorizontalGroup] 
        public Texture2D playerIcon;
            
        [Required, PreviewSprite, HorizontalGroup] 
        public Texture2D playerIcon2;
            
        [TabGroup("Ability Scores")]
        public AbilityScores abilityScores;
        [TabGroup("Skills")]
        public Skills skills;
        [TabGroup("Items")]
        public List<Item> items;
            
        [Required, ChildGameObjectsOnly]
        public GameObject characterPrefab;
            
        [Required, ChildGameObjectsOnly]
        public Transform mainBodyTransform;
    }
        
    [InfoBox("Experimental Feature", InfoBoxAttribute.InfoMessageType.Warning)]
    public List<Character> characters;
}