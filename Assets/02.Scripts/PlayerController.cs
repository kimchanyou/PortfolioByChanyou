using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float attackTime = 0.5f;     // ���� ��Ÿ��
    public float attackRange = 1.5f;    // ���� �����Ÿ�

    public Vector2 inputVec;
    public Vector2 dirVec;              // �ٶ󺸴� ����
    [SerializeField]
    private float moveSpeed = 5f;       // �̵� �ӵ�
    public float ditectionRange = 4f;   // ���� ���� �����Ÿ�

    public Rigidbody2D rigid;
    public SpriteRenderer spriter;
    public Camera mainCam;

    public GameObject targetGem = null;
    public GameObject gemItemInven;
    public GameObject guideLine;

    public Attackable[] attackables;

    public bool isAttack = false;

    [SerializeField]
    private Define.PlayerState state = Define.PlayerState.IDLE;

    [Header("GemInfo")]
    public int id;
    public float attack;
    public string gemName;
    public string spriteName;
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
        mainCam = Camera.main;
        attackables = UIManager.instance.attackables;
        Managers.Input.KeyAction -= OnKeyborard;
        Managers.Input.KeyAction += OnKeyborard;
        Managers.Input.MounseAction -= OnMouseEvent;
        Managers.Input.MounseAction += OnMouseEvent;
        gemItemInven = Resources.Load<GameObject>("Prefabs/02GemItem"); // �κ��丮�� �� ���� ������ �ε�
        guideLine = transform.GetChild(0).gameObject;
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
        TargetFind();
        MouseCheck();
    }
    
    void LateUpdate()
    {
        // �¿� �̵��ÿ� �̹��� ������ȯ
        if (inputVec.x != 0)
            spriter.flipX = inputVec.x < 0;

        // �ٶ󺸴� ����
        dirVec.x = spriter.flipX ? -1 : 1;
    }
    private void OnKeyborard()
    {
        // GetAxisRaw : Ű �Է� ���� �� �� ����������
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * moveSpeed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        Debug.DrawRay(transform.position, dirVec * attackRange, Color.green);
        
    }
    private void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.PlayerState.IDLE:
                break;
            case Define.PlayerState.WALK:
                break;
            case Define.PlayerState.ATTACK:
                break;
        }
    }

    public void UpdateDie()
    {
        // ��Ȱ �Ұ��� ����� �˾� â �߰�?
    }
    public void UpdateIdle()
    {
        if (!isAttack && Input.GetKeyDown(KeyCode.Space))
            State = Define.PlayerState.ATTACK;
        if (inputVec.magnitude != 0)
            State = Define.PlayerState.WALK;
    }
    public void UpdateWalk()
    {
        if (!isAttack && Input.GetKeyDown(KeyCode.Space))
            State = Define.PlayerState.ATTACK;
        if (Input.GetButton("Horizontal") == false && Input.GetButton("Vertical") == false)
        {
            inputVec = Vector2.zero;
            State = Define.PlayerState.IDLE;
        }
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
        if (targetGem == null) return;
        Slider hpbar = targetGem.GetComponentInChildren<Slider>();
        hpbar.value -= 0.2f;
        if (hpbar.value <= 0.001f)
        {
            GameObject clone = Instantiate(gemItemInven, GetItemInven());
            GemInven gemInven = clone.GetComponent<GemInven>();
            gemInven.id = id;
            gemInven.attack = attack;
            gemInven.gemName = gemName;
            gemInven.spriteName = spriteName;
            Managers.Pool.ReturnObject(targetGem, Managers.Pool.itemRoot, Managers.Pool.itemPool);
        }
        
    }
    private void TargetFind()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirVec.normalized, attackRange, LayerMask.GetMask("Gem"));
        if (hit.collider != null)
        {
            targetGem = hit.collider.gameObject;
            GemItem gem = targetGem.GetComponent<GemItem>();
            GemInfo gemInfo = gem.dicGem[gem.gemId];
            id = gemInfo.id;
            attack = gemInfo.attack;
            gemName = gemInfo.gemName;
            spriteName = gemInfo.spriteName;
        }
        else
        {
            id = 0;
            attack = 0;
            gemName = null;
            spriteName = null;
            targetGem = null;
            //sprite = null;
        }
    }
    IEnumerator AttackTime(float attackTime)
    {
        isAttack = true;
        yield return new WaitForSeconds(attackTime);
        isAttack = false;
    }

    private Transform GetItemInven()
    {
        for (int i = 0; i < UIManager.instance.itemInvenLists.Length; i++)
        {
            if (UIManager.instance.itemInvenLists[i].transform.childCount == 0)
            {
                return UIManager.instance.itemInvenLists[i].transform;
            }
        }
        return null;
    }

    private void MouseCheck()
    {
        Vector2 mousePos = Input.mousePosition;
        // ���� ���콺�� ��ġ�� ���ӳ��� Position ������ ��ȯ
        mousePos = mainCam.ScreenToWorldPoint(mousePos);

        Vector3 playerPos = transform.position;

        Vector2 distanceVec = mousePos - (Vector2)playerPos;

        guideLine.SetActive(distanceVec.magnitude < ditectionRange ? true : false);

        guideLine.transform.right = distanceVec.normalized;
    }
}
