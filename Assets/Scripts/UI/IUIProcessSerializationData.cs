using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IUIProcessSerializationData
{
    void Process(IUILayoutSerializable dataInterface);
}