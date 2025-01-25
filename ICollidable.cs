using System.Collections.Generic;

namespace Final_Project_CIS_297
{
    internal interface ICollidable
    {
        List<double> GetGridPosition();

        bool DidCollide(ICollidable other);
    }
}