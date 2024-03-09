using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUILayoutSerializable
{
    string GetPrefabKey();
    string GetDetailPrefabKey();
    string GetThumbnailPrefabKey();
    string GetName();
    Rect GetLayoutRect();
}
