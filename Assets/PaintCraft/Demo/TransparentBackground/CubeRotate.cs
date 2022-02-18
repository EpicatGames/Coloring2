using UnityEngine;
using UnityEngine.Serialization;

namespace PaintCraft.Demo
{
    public class CubeRotate : MonoBehaviour
    {
        [FormerlySerializedAs("Speed")] public Vector3 speed;

        private void Update()
        {
            transform.Rotate(speed * Time.deltaTime);
        }
    }
}