using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;

public class Vehicle : MonoBehaviour
{
    public static float buttonpressed = 0;

    public float Speed;

    List<Rigidbody2D> rbs;
    Vector3 force;

    private Vector2 moveDirection;

    public float fixVisualTime;
    private float fixVisualTimer;

    private Vector3[] boneTransformsCopy_Positions;
    private Quaternion[] boneTransformsCopy_Rotations;
    private Transform[] boneTransforms;
    public float boneTransformResetTolerance;

    void Start()
    {
        rbs = new List<Rigidbody2D>();
        if(gameObject.GetComponent<Rigidbody2D>())
            rbs.Add(gameObject.GetComponent<Rigidbody2D>());
        rbs.AddRange(gameObject.GetComponentsInChildren<Rigidbody2D>());

        boneTransforms = gameObject.GetComponent<SpriteSkin>().boneTransforms;
        boneTransformsCopy_Positions = new Vector3[boneTransforms.Length];
        boneTransformsCopy_Rotations = new Quaternion[boneTransforms.Length];
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransformsCopy_Positions[i] = boneTransforms[i].localPosition;
            boneTransformsCopy_Rotations[i] = boneTransforms[i].localRotation;
        }

        fixVisualTimer = -2f * fixVisualTime;
    }

    void FixedUpdate()
    {
        if(moveDirection != Vector2.zero)
        {
            foreach (Rigidbody2D rb in rbs)
            {
                rb.AddForce(moveDirection * Speed * Time.fixedDeltaTime);
            }
        }

        if(fixVisualTimer > fixVisualTime)
        {
            fixVisualTimer = 0f;

            //Fix Bone transforms
            for (int i = 0; i < boneTransforms.Length; i++) 
            {
                if (Vector2.Distance(boneTransforms[i].localPosition, boneTransformsCopy_Positions[i]) > boneTransformResetTolerance)
                {
                    boneTransforms[i].localPosition = boneTransformsCopy_Positions[i];
                    boneTransforms[i].localRotation = boneTransformsCopy_Rotations[i];
                }
            }
        }
        fixVisualTimer += Time.fixedDeltaTime;
    }

    public void MoveLeft()
    {
        moveDirection = Vector2.left;
    }
    public void MoveRight()
    {
        moveDirection = Vector2.right;
    }

    public void StopMovement()
    {
        fixVisualTimer = 0f;
        moveDirection = Vector2.zero;
    }
}
