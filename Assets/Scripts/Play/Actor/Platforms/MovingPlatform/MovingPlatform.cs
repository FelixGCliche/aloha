using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    // Author: David D
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Moving Platform")]
        [SerializeField] private MovingPlatformPath path;
        [SerializeField] [Min(0.0f)] private float movingSpeed = 2f;
        [SerializeField] [Min(0.0f)] private float stayAtPointTime = 1f;

        private Vector2 destination;
        private SignalReceiver signalReceiver;
        private IEnumerator<Vector3> nodes;
        private bool atPoint;

        private Vector2 Position => transform.position;

        private void Awake()
        {
#if UNITY_EDITOR
            Debug.Assert(path != null, "No path set");
#endif
            nodes = path.Nodes.Repeat();
            nodes.MoveNext();
            
            transform.position = path.Nodes[0];
            destination = nodes.Current;
            signalReceiver = GetComponent<SignalReceiver>();

            atPoint = false;
        }

        private void Update()
        {
            if (!signalReceiver.IsActivated || atPoint) return;

            if (Position.AreClose(nodes.Current))
            {
                nodes.MoveNext();
                destination = nodes.Current;
                StartCoroutine(StayAtPointRoutine());
            }

            Vector2 movementVector = (destination - Position).normalized * (movingSpeed * Time.deltaTime);
            float distanceMoving = movementVector.magnitude;

            float distanceToDestination = Vector2.Distance(destination, Position);

            if (distanceToDestination <= distanceMoving)
            {
                transform.position = destination;
            }
            else
            {
                transform.Translate(movementVector);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.rigidbody != null)
                other?.transform.SetParent(transform);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.rigidbody != null)
                other?.transform.SetParent(null);
        }

        private IEnumerator StayAtPointRoutine()
        {
            atPoint = true;
            yield return new WaitForSeconds(stayAtPointTime);
            atPoint = false;
        }
    }
}