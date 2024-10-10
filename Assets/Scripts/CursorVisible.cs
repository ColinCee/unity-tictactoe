#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class cursorVisible : MonoBehaviour
{
    public Texture2D cursorTexture;
    void Start()
    {
#if UNITY_EDITOR
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
#endif
    }
}
