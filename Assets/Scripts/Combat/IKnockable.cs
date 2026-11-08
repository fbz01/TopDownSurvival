// Anything that can be pushed. Player and Enemy react differently.
using UnityEngine;

public interface IKnockable
{
    void ApplyKnockback(Vector2 direction, float force);
}
