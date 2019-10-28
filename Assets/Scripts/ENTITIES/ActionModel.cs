using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Entity/Action", fileName = "Action")]
public class ActionModel : ScriptableObject
{
    public string QRID;

    public AnimationClip animationClip;
}
