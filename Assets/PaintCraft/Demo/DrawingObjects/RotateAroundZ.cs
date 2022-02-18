using UnityEngine;
using UnityEngine.Serialization;

namespace PaintCraft.Demo
{
    public class RotateAroundZ : MonoBehaviour
    {
        public float speed = 1.0f;
        [FormerlySerializedAs("Axis")] public Vector3 axis = Vector3.forward;

        // Update is called once per frame
        private void Update()
        {
            transform.RotateAround(transform.position, transform.TransformDirection(axis), speed * Time.deltaTime);
        }
    }
}