# Change Log:

## 1.1.5
 - Enhancement: Added ListElementNameAttribute which allows you to set a custom naming extension to your list elements based on sub-property string values.
 - Enchacement: Added context menu options (apply/revert to prefab, copy and paste) to Artifice's list view. Now, it also indicates with the blue indicator if lists have been detected on the list.

## 1.1.4
 - Refactor: Changed MenuItem name from ArtificeDrawer to ArtificeToolkit
 - Enhancement: Updated README.md with complete documentation and examples for each tested and used attribute with images and gifs.
 - Fix: Empty array using custom attributes was not rendering with artifice fixed.

## 1.1.3

- Enhancement: Now toggle button visual element can receive different sprites for each of its states. A example of this was implemented in the validator.

## 1.1.2

- Enhancement: Artifice Off now truly disables the toolkit. It disables the CustomEditor attribute on the artifice inspector, disabling its automatic replacement of the default editor. In addition, it will enforce the toggle option upon every domain reload. This ensures consistency when initializing or updating the package.