using UnityEditor;
using UnityEngine;

public class FixUnityError : EditorWindow
{
    [MenuItem("Tools/THE NUCLEAR FIX")]
    public static void NuclearFix()
    {
        // 1. Avmarkera allt
        Selection.activeObject = null;

        // 2. Tvinga Inspectorn att titta på ingenting
        EditorUtility.ClearDirty(Selection.activeObject);
        
        // 3. Försök rensa konsolen automatiskt
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        var clearMethod = logEntries?.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod?.Invoke(null, null);

        Debug.Log("NUCLEAR FIX KLAR: Allt är avmarkerat. Om du fortfarande ser fel: STÄNG FÖNSTRET 'TILE PALETTE' NU!");
    }
}
