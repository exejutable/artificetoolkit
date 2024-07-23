using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbzorbaExportRoot.CommonLibrariesAndResources.AbzorbaCustomAttributes
{
    /// <summary> Example script for <see cref="Abz_CustomAttribute"/> </summary>
    public class Abz_ExampleForCustomAttributes : MonoBehaviour
    {
        [Serializable]
        public class Character
        {
            [Serializable]
            public class AbilityScores
            {
                [Abz_Range(0, 20)] public int strength;
                [Abz_Range(0, 20)] public int dexterity;
                [Abz_Range(0, 20)] public int constitution;
                [Abz_Range(0, 20)] public int wisdom;
                [Abz_Range(0, 20)] public int intelligence;
                [Abz_Range(0, 20)] public int charisma;
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
                
                [Abz_MinValue(0)] public int amount;
                [Abz_EnumToggle] public WeaponType weaponType;
            }

            [Serializable]
            public class Skills
            {
                [Abz_MinValue(0)] public int athletics;
                [Abz_MinValue(0)] public int acrobatics;
                [Abz_MinValue(0)] public int arcana;
                [Abz_MinValue(0)] public int deception;
            }
            
            [Abz_Title("Character Name"), Abz_HideLabel]
            public string name;
            
            [Abz_Required, Abz_PreviewSprite, Abz_HorizontalGroup] 
            public Texture2D playerIcon;
            
            [Abz_Required, Abz_PreviewSprite, Abz_HorizontalGroup] 
            public Texture2D playerIcon2;
            
            [Abz_TabGroup("Ability Scores")]
            public AbilityScores abilityScores;
            [Abz_TabGroup("Skills")]
            public Skills skills;
            [Abz_TabGroup("Items")]
            public List<Item> items;
            
            [Abz_Required, Abz_ChildGameObjectsOnly]
            public GameObject characterPrefab;
            
            [Abz_Required, Abz_ChildGameObjectsOnly]
            public Transform mainBodyTransform;
        }
        
        [Abz_InfoBox("Experimental Feature", Abz_InfoBoxAttribute.InfoMessageType.Warning)]
        public List<Character> characters;
    }
}
    