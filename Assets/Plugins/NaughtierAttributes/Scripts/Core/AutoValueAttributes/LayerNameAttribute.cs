using System;
using UnityEngine;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LayerNameAttribute : BaseAutoValueAttribute
    {
        public readonly int LayerId;

        public LayerNameAttribute(string layerName, bool playUpdate = false) : base(playUpdate)
        {
            LayerId = LayerMask.NameToLayer(layerName);
        }
    }
}