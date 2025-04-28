using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

namespace Utilities
{
    public static class Utilities
    {
        public static void AddParentConstraint(this GameObject ob, Transform parent)
        {
            var parentConstraint = ob.AddComponent<ParentConstraint>();
            parentConstraint.AddSource(new() { sourceTransform = parent, weight = 1f });
            parentConstraint.constraintActive = true;
        }
        public static void RemoveParentConstraint(this GameObject ob)
        {
            Object.Destroy(ob.GetComponent<ParentConstraint>());
        }
    } 
}
