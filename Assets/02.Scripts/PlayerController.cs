using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private UIManager canvas_UI;
    private FloatingText floatingText;

    public float attackTime = 0.5f;     // ���� ��Ÿ��

    private Vector2 inputVec;
    private Vector2 dirVec;              // �ٶ󺸴� ����

    public static Vector2 attackVec;
    public static string attackSpriteName;

    [SerializeField]
    private float moveSpeed = 5f;       // �̵� �ӵ�
    private float ditectionRange = 10f;   // ���̵���� ���� ���� �Ÿ�

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private AudioSource audioSource;
    private AudioClip attackClip;
    private AudioClip gemAttackClip;
    private Camera mainCam;
    private Texture2D basicCursor;

    public GameObject targetGem = null;
    public GameObject gemItemInven;
    public GameObject guideLine;

    public GemInven[] attackable;

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
        canvas_UI = FindObjectOfType<UIManager>();
        floatingText = canvas_UI.transform.GetChild(4).GetComponent<FloatingText>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        attackClip = Resources.Load<AudioClip>("Sounds/Attack");
        gemAttackClip = Resources.Load<AudioClip>("Sounds/GemAttack");
        basicCursor = Resources.Load<Texture2D>("Textures/Cursor/Basic");
        mainCam = Camera.main;
        Managers.Input.KeyAction -= OnKeyborard;
        Managers.Input.KeyAction += OnKeyborard;
        gemItemInven = Resources.Load<GameObject>("Prefabs/02GemItem"); // �κ��丮�� �� ���� ������ �ε�
        guideLine = transform.GetChild(0).gameObject;
        Cursor.SetCursor(basicCursor, new Vector2(basicCursor.width / 4, basicCursor.height / 4), CursorMode.Auto);
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;  // �κ��丮���� ���콺�� �����Ͻ� �ٸ� �̺�Ʈ ���� �ʵ���
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
        MouseAttack();
        attackable = GameObject.Find("Attack").GetComponentsInChildren<GemInven>();
    }
    
    void LateUpdate()
    {
        // �¿� �̵��ÿ� �̹��� ������ȯ
        if (inputVec.x != 0)
            spriter.flipX = inputVec.x < 0;

        // �ٶ󺸴� ���� RayCastHit��Ŀ��� ���
        //dirVec.x = spriter.flipX ? -1 : 1;
    }
    private void OnKeyborard()
    {
        // GetAxisRaw : Ű �Է� ���� �� �� ����������
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * moveSpeed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextVec);
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
        if (!isAttack && Input.GetMouseButtonDown(0))
            State = Define.PlayerState.ATTACK;
        if (inputVec.magnitude != 0)
            State = Define.PlayerState.WALK;
    }
    public void UpdateWalk()
    {
        if (!isAttack && Input.GetMouseButtonDown(0))
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
        audioSource.clip = attackClip;
        audioSource.Play();
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
            floatingText.ShowText("���� " + (id + 1).ToString() + "���� ȹ��");
        }

    }
    #region RaycastHit ��� TargetFind
    //private void TargetFind()
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, dirVec.normalized, attackRange, LayerMask.GetMask("Gem"));
    //    if (hit.collider != null)
    //    {
    //        targetGem = hit.collider.gameObject;
    //        GemItem gem = targetGem.GetComponent<GemItem>();
    //        GemInfo gemInfo = gem.dicGem[gem.gemId];
    //        id = gemInfo.id;
    //        attack = gemInfo.attack;
    //        gemName = gemInfo.gemName;
    //        spriteName = gemInfo.spriteName;
    //    }
    //    else
    //    {
    //        id = 0;
    //        attack = 0;
    //        gemName = null;
    //        spriteName = null;
    //        targetGem = null;
    //    }
    //}
    #endregion
    private void TargetFind()
    {
        targetGem = FindTarget.targetGem;
        if (targetGem != null)
        {
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

    private void MouseAttack()
    {
        Vector2 mousePos = Input.mousePosition;
        // ���� ���콺�� ��ġ�� ���ӳ��� Position ������ ��ȯ
        mousePos = mainCam.ScreenToWorldPoint(mousePos);
        // �÷��̾��� ���� ��ġ
        Vector3 playerPos = transform.position;
        // ���콺�� ����Ű�� ���⺤��
        Vector2 distanceVec = mousePos - (Vector2)playerPos;
        // ���콺�� ����Ű�� ������ ��Ÿ� ���� ������ ���̵���� Ȱ��ȭ
        guideLine.SetActive(distanceVec.magnitude < ditectionRange ? true : false);
        // ���̵������ ����Ű�� ������ ���� �������� ����
        guideLine.transform.right = distanceVec.normalized;

        if (Input.GetMouseButtonDown(0))
        {
            spriter.flipX = distanceVec.x > 0 ? false : true;
            if (targetGem != null) return;
            for (int i = 0; i < attackable.Length; i++)
            {
                if (attackable[i].isCool == false)
                {
                    attackable[i].isCool = true;
                    attackSpriteName = attackable[i].spriteName;
                    attackVec = distanceVec;
                    GameObject attackGem = Managers.Pool.GetObject(Managers.Pool.attackPool);
                    if (attackGem == null) return;
                    audioSource.clip = gemAttackClip;
                    audioSource.Play();
                    attackGem.transform.position = transform.position + (Vector3)distanceVec.normalized;
                    attackGem.GetComponent<Rigidbody2D>().AddForce(distanceVec.normalized * 1000f);
                    break;
                }
            }
            
        }
    }
}
