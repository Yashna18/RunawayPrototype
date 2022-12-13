using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grappleHook : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer lineRen;
    public LayerMask hookMask;
    private float speed = 100;
    private float lenOfGrapple = 5;
    [Min(1)]
    private int maxHooks = 3;

    private Rigidbody2D rig;
    private List<Vector2> points = new List<Vector2>();

    // Start is called before the first frame update
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        lineRen.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, lenOfGrapple, hookMask);
            if (hit.collider != null)
            {
                Vector2 hitPoint = hit.point;
                points.Add(hitPoint);

                if (points.Count > maxHooks)
                {
                    points.RemoveAt(0);
                }
            }
        }
        if (points.Count > 0)
        {
            Vector2 moveTo = centroid(points.ToArray());
            rig.MovePosition(Vector2.MoveTowards((Vector2)transform.position, moveTo, Time.deltaTime * speed));

            lineRen.positionCount = 0;
            lineRen.positionCount = points.Count * 2;
            for (int n = 0, j=0; n < points.Count * 2; n+=2,j++)
            {
                lineRen.SetPosition(n, transform.position);
                lineRen.SetPosition(n+1, points[j]);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Detatch();
        }
    }

    public void Detatch()
    {
        lineRen.positionCount = 0;
        points.Clear();
    }

    Vector2 centroid(Vector2[] points)
    {
        Vector2 center = Vector2.zero;
        foreach (Vector2 point in points)
        {
            center += point;
        }
        center /= points.Length;
        return center;
    }

    private void OnDrawGizmos()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction =  (mousePos - (Vector2)transform.position).normalized;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + direction);

        foreach (Vector2 point in points)
        {
            Gizmos.DrawLine(transform.position, point);
        }
    }
}
