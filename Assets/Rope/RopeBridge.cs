using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RopeBridge : MonoBehaviour
{
    public Transform m_StartPoint;
    public Transform m_EndPoint;

    public Material m_RopeMaterial;

    private LineRenderer m_LineRenderer;
    private List<RopeSegment> m_RopeSegments = new List<RopeSegment>();
    private float m_RopeSegLen = 0.25f;
    private int m_SegmentLength = 40;
    private float m_LineWidth = 0.25f;
    private EdgeCollider2D m_EdgeCollider2D;

    private MoveAnchor[] m_MoveAnchors;

    public bool m_RopeDrawn = false;
    private bool m_generatedRope = false;

    private bool m_MouseUp = true;
    private Vector2 StartPosition;

    void Start()
    {
        m_LineRenderer = this.GetComponent<LineRenderer>();
        m_EdgeCollider2D = gameObject.GetComponent<EdgeCollider2D>();

        if (m_generatedRope) return;

        StartPosition = transform.position;

        m_MoveAnchors = gameObject.GetComponentsInChildren<MoveAnchor>();
        for(int i = 0; i < m_MoveAnchors.Length; i++) m_MoveAnchors[i].canMove = false;

        m_StartPoint.position = new Vector3(transform.position.x, transform.position.y, 1f);
        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, transform.position);

        m_MouseUp = false;
    }

    void Update()
    {
        if (m_RopeDrawn)
        {
            DrawRope();

            Vector2[] points = new Vector2[m_LineRenderer.positionCount - 4];
            for (int i = 2; i < m_LineRenderer.positionCount - 2; i++)
            {
                points[i - 2] = m_LineRenderer.GetPosition(i);
            }

            m_EdgeCollider2D.points = points;
            m_EdgeCollider2D.offset = -transform.position;
        }
        else
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    m_MouseUp = false;
            //    Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    m_StartPoint.gameObject.SetActive(true);

            //    m_StartPoint.position = new Vector3(position.x, position.y, -1f);
            //    m_LineRenderer.SetPosition(0, position);
            //}

            if (!m_MouseUp)
            {
                Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_EndPoint.position = new Vector3(position.x, position.y, 1f);
                m_LineRenderer.SetPosition(1, position);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if(Vector2.Distance(StartPosition, position) < Mathf.Epsilon)
                {
                    Destroy(gameObject);
                    return;
                }
                m_EndPoint.position = new Vector3(position.x, position.y, 1f);

                m_SegmentLength = (int)(Vector2.Distance(m_StartPoint.position, m_EndPoint.position) * 3.5f);

                m_LineRenderer.SetPosition(1, position);

                m_MouseUp = true;
                m_RopeDrawn = true;

                Vector3 ropeStartPoint = m_StartPoint.position;

                for (int i = 0; i < m_SegmentLength; i++)
                {
                    m_RopeSegments.Add(new RopeSegment(ropeStartPoint));
                    ropeStartPoint.y -= m_RopeSegLen;
                }

                for (int i = 0; i < m_MoveAnchors.Length; i++) m_MoveAnchors[i].canMove = true;
            }
        }

    }

    private void DrawRope()
    {
        float lineWidth = m_LineWidth;
        m_LineRenderer.startWidth = lineWidth;
        m_LineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[m_SegmentLength];
        for (int i = 0; i < m_SegmentLength; i++)
        {
            ropePositions[i] = m_RopeSegments[i].posNow;
        }

        m_LineRenderer.material = m_RopeMaterial;
        m_LineRenderer.material.mainTextureScale = new Vector2(m_SegmentLength / 4, 1);

        m_LineRenderer.positionCount = ropePositions.Length;
        m_LineRenderer.SetPositions(ropePositions);
    }

    public void GenerateRope(Vector3 startPos, Vector3 endPos)
    {
        m_generatedRope = true;
        m_StartPoint.position = startPos;
        m_EndPoint.position = endPos;

        m_MoveAnchors = gameObject.GetComponentsInChildren<MoveAnchor>();
        for (int i = 0; i < m_MoveAnchors.Length; i++) m_MoveAnchors[i].canMove = true;

        m_SegmentLength = (int)(Vector2.Distance(m_StartPoint.position, m_EndPoint.position) * 3.5f);

        m_MouseUp = true;
        m_RopeDrawn = true;

        Vector3 ropeStartPoint = m_StartPoint.position;

        for (int i = 0; i < m_SegmentLength; i++)
        {
            m_RopeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= m_RopeSegLen;
        }
    }

    private void FixedUpdate()
    {
        if (m_RopeDrawn)
        {
            Simulate();
        }
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, -1f);

        for (int i = 1; i < m_SegmentLength; i++)
        {
            RopeSegment firstSegment = m_RopeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            m_RopeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = m_RopeSegments[0];
        firstSegment.posNow = m_StartPoint.position;
        m_RopeSegments[0] = firstSegment;

        //Constrant to Second Point 
        RopeSegment endSegment = m_RopeSegments[m_RopeSegments.Count - 1];
        endSegment.posNow = m_EndPoint.position;
        m_RopeSegments[m_RopeSegments.Count - 1] = endSegment;

        for (int i = 0; i < m_SegmentLength - 1; i++)
        {
            RopeSegment firstSeg = m_RopeSegments[i];
            RopeSegment secondSeg = m_RopeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - m_RopeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > m_RopeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < m_RopeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                m_RopeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                m_RopeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                m_RopeSegments[i + 1] = secondSeg;
            }
        }
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}