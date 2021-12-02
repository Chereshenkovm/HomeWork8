using UnityEngine;

namespace View.Input
{
    public interface IPlayerInput
    {
        public (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput();
    }
}