using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float attackTime = 2f;
    
    public Vector2 inputVec;
    [SerializeField]
    private float moveSpeed = 5f;

    public Rigidbody2D rigid;
    public SpriteRenderer spriter;

    public bool isClick = true;
    public bool isAttack = false;

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
        if (State != Define.PlayerState.ATTACK)
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
        if (Input.GetKeyDown(KeyCode.Space))
            State = Define.PlayerState.ATTACK;
    }
    public void UpdateWalk()
    {
        if (inputVec.x == 0 && inputVec.y == 0)
            State = Define.PlayerState.IDLE;
        if (Input.GetKeyDown(KeyCode.Space))
            State = Define.PlayerState.ATTACK;
    }
    public void UpdateAttack()
    {
        
    }
    // 애니메이션 이벤트 호출 생각해보기
    public void AttackEnd()
    {
        StartCoroutine(waitTime(attackTime));
    }
    IEnumerator waitTime(float attackTime) // 공격 속도 조정 할 때 사용할 수 있을듯
    {
        State = Define.PlayerState.IDLE;
        yield return new WaitForSeconds(attackTime);
        State = Define.PlayerState.ATTACK;
    }
}
