using System;
using UnityEngine;

namespace Game
{
    // Author: David Dorion
    public static class Vector2Extensions
    {
        private const float TOLERANCE = 0.01f;

        public static bool AreClose(this Vector2 vector1, Vector2 vector2, float tolerance = TOLERANCE)
        {
            return Math.Abs(vector1.x - vector2.x) < tolerance && Math.Abs(vector1.y - vector2.y) < tolerance;
        }
    }
}