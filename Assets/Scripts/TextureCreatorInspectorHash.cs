using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureCreatorHash))]
public class TextureCreatorInspectorHash : Editor {

    private TextureCreatorHash creator;

    private void OnEnable() {
        creator = target as TextureCreatorHash;
        Undo.undoRedoPerformed += RefreshCreator;
    }

	private void OnDisable () {
		Undo.undoRedoPerformed -= RefreshCreator;
	}

	private void RefreshCreator () {
		if (Application.isPlaying) {
			creator.FillTexture();
		}
	}

	public override void OnInspectorGUI () {
		EditorGUI.BeginChangeCheck();
		DrawDefaultInspector();
		if (EditorGUI.EndChangeCheck()) {
			RefreshCreator();
		}
	}
}