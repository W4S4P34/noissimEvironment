using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convert2EdgeCollider : MonoBehaviour
{
    private void Awake()
    {
        PolygonCollider2D polygonCollider2D = GetComponent<PolygonCollider2D>();

        List<Vector2> points = new List<Vector2>();
        foreach (Vector2 point in polygonCollider2D.points)
            points.Add(point);
        points.Add(new Vector2(points[0].x, points[0].y));

        EdgeCollider2D edgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider2D.points = points.ToArray();

        Destroy(polygonCollider2D);
    }
}
