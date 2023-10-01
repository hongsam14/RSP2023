using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface spineAnimeController
{
    public void Move();
    public void Stand();
    public void Jump(AirStatus status);
    public void Land(MoveStatus status);
}
