using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 inputVec;
    [SerializeField]
    private float moveSpeed = 5f;

    public Rigidbody2D rigid;
    public SpriteRenderer spriter;

    [SerializeField]
    private Define.PlayerState state = Define.PlayerState.IDLE;

    public Define.PlayerState State
    {
        get { return state; }
        set
        {
            state = value;

            Animator anim = GetComponent<Animator>();
            switch (state)
            {
                case Define.PlayerState.DIE:
                    anim.CrossFade("DIE", 0.2f);
                    break;
                case Define.PlayerState.IDLE:
                    anim.CrossFade("IDLE", 0.2f);
                    break;
                case Define.PlayerState.WALK:
                    anim.CrossFade("WALK", 0.2f);
                    break;
                case Define.PlayerState.ATTACK:
                    anim.CrossFade("ATTACK", 0.2f);
                    break;
            }
        }
    }
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch (State)
        {
            case Define.PlayerState.DIE:
                UpdateDie();
                break;
            case Define.PlayerState.IDLE:
                UpdateIdle();
                break;
            case Define.PlayerState.WALK:
                UpdateWalk();
                break;
            case Define.PlayerState.ATTACK:
                UpdateAttack();
                break;
        }
    }
    private void FixedUpdate()
    {
        OnMove();
    }
    void LateUpdate()
    {
        if (inputVec.x != 0)
            spriter.flipX = inputVec.x < 0;
    }
    private void OnMove()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * moveSpeed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    public void UpdateDie()
    {
        // 부활 할건지 물어보는 팝업 창 뜨게?

    }
    public void UpdateIdle()
    {
        if (inputVec.x != 0 || inputVec.y != 0)
            State = Define.PlayerState.WALK;
    }
    public void UpdateWalk()
    {
        if (inputVec.x == 0 && inputVec.y == 0)
            State = Define.PlayerState.IDLE;
    }
    public void UpdateAttack()
    {

    }
}
