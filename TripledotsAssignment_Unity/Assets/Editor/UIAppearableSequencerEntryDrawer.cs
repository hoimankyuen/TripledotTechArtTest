using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UIAppearableSequencerEntry))]
public class UIAppearableSequencerEntryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty delay = property.FindPropertyRelative("delay");
        SerializedProperty appearable = property.FindPropertyRelative("appearable");
        
        Rect delayRect = new Rect(position.x, position.y ,position.width / 4f, position.height);
        Rect appearableRect = new Rect(position.x + position.width / 4f + 10f, position.y, position.width / 4f * 3f - 10f,  position.height);
            
        EditorGUI.PropertyField(delayRect, delay, new GUIContent());
        EditorGUI.PropertyField(appearableRect, appearable, new GUIContent());
            
        EditorGUI.EndProperty();
    }
}