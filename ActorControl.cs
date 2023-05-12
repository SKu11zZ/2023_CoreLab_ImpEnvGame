using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorControl : Singleton<ActorControl>
{
    private Animator thisAnimator;
    private Vector3 targetPos;
    public Vector3 TargetPos
    {
        get
        {
            return targetPos;
        }
    }
    private Vector3 direction;
    private bool isWalk;
    // Start is called before the first frame update
    void Start()
    {
        thisAnimator = this.GetComponent<Animator>();
        targetPos = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isWalk)
        {
            return;
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, Time.deltaTime);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * 500f);
        if (Vector3.Distance(this.transform.position, targetPos) < 0.01f)
        {
            this.transform.position = targetPos;
            isWalk = false;
            SceneControl.Instance.ShowRefHexagons(targetPos + new Vector3(0f, -0.2f));
        }
        thisAnimator.SetBool("IsWalk", isWalk);
    }
    public bool SetTargetPos(Vector3 targetPos)
    {
        if (this.transform.position != targetPos)
        {
            this.targetPos = targetPos;
            direction = (targetPos - this.transform.position).normalized;
            isWalk = true;
            return true;
        }
        return false;
    }
}
