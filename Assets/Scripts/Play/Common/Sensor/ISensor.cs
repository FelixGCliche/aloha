using System.Collections.Generic;

namespace Game
{
    public interface ISensor<out T>
    {
        IReadOnlyList<T> SensedObjects { get; }
        event SensorEventHandler<T> OnSensedObject;
        event SensorEventHandler<T> OnUnsensedObject;
    }

    public delegate void SensorEventHandler<in T>(T otherObject);
}