using System.IO;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AssetReference))]
public class AssetReferenceDrawer : PropertyDrawer
{

    public bool isOpen = false;
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var objectReferenceRect = new Rect(position.x, position.y, position.width / 2, position.height);
        var bundleRect = new Rect(position.x + (position.width / 2) , position.y, position.width / 2, position.height);

        var nameProperty = property.FindPropertyRelative(nameof(AssetReference.name));
        var bundleProperty = property.FindPropertyRelative(nameof(AssetReference.bundle));
        
        var guidProperty = property.FindPropertyRelative("guid");
        var assetPath = AssetDatabase.GUIDToAssetPath(guidProperty.stringValue);
        var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));

        var selectedObject = EditorGUI.ObjectField(objectReferenceRect, asset, typeof(Object), false);
        if (selectedObject != null)
        {
            var selectionPath = AssetDatabase.GetAssetPath(selectedObject);
            var selectionGuid = AssetDatabase.GUIDFromAssetPath(selectionPath);
            guidProperty.stringValue = selectionGuid.ToString();

            nameProperty.stringValue = Path.GetFileNameWithoutExtension(selectionPath);
            bundleProperty.stringValue = AssetDatabase.GetImplicitAssetBundleName(selectionPath);
        }
        else
        {
            guidProperty.stringValue = null;
            nameProperty.stringValue = null;
            bundleProperty.stringValue = null;
        }
        
        EditorGUI.LabelField(bundleRect, bundleProperty.stringValue);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
