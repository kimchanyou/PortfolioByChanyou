using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float attackTime = 0.1f; // ���� ��Ÿ��
    
    public Vector2 inputVec;
    [SerializeField]
    private float moveSpeed = 5f;   // �̵� �ӵ�

    public Rigidbody2D rigid;
    public SpriteRenderer spriter;

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
                    anim.CrossFade("DIE", 0.1f);
                    break;
                case Define.PlayerState.IDLE:
                    anim.CrossFade("IDLE", 0.1f);
                    break;
                case Define.PlayerState.WALK:
                    anim.CrossFade("WALK", 0.1f);
                    break;
                case Define.PlayerState.ATTACK:
                    anim.CrossFade("ATTACK", 0.1f);
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
        // �¿� �̵��ÿ� �̹��� ������ȯ
        if (inputVec.x != 0)
            spriter.flipX = inputVec.x < 0;
    }
    private void OnMove()
    {
        // GetAxisRaw : Ű �Է� ���� �� �� ����������
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * moveSpeed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    public void UpdateDie()
    {
        // ��Ȱ �Ұ��� ����� �˾� â �߰�?

    }
    public void UpdateIdle()
    {
        if (!isAttack && Input.GetKeyDown(KeyCode.Space))
            State = Define.PlayerState.ATTACK;
        if (inputVec.sqrMagnitude != 0)
            State = Define.PlayerState.WALK;
    }
    public void UpdateWalk()
    {
        if (!isAttack && Input.GetKeyDown(KeyCode.Space))
            State = Define.PlayerState.ATTACK;
        if (inputVec.sqrMagnitude == 0)
            State = Define.PlayerState.IDLE;
    }
    public void UpdateAttack()
    {
        StartCoroutine(AttackTime(attackTime)); // ���� ���� ��Ÿ�� ����
    }

    // ���� �ִϸ��̼� ������ ȣ�� �Ǵ� �Լ�
    public void AttackEndEvent()
    {
        State = Define.PlayerState.IDLE;
    }
    public void OnHitEvent()
    {

    }
    IEnumerator AttackTime(float attackTime)
    {
        isAttack = true;
        yield return new WaitForSeconds(attackTime);
        isAttack = false;
    }
}
