using UnityEngine;

public interface IShape {

    bool Contains(Vector2 pos);
    Vector2 GetEdge(Vector2 pos);
}
