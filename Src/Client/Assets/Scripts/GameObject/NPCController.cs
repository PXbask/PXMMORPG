using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Manager;
using Models;

public class NPCController : MonoBehaviour
{
    public int npcID;
    public Animator animator;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private Color origionColor;
    private bool inInteractive = false;
    private NpcDefine npcDefine;
    private NpcQuestStatus questStatus;
    void Start()
    {
        skinnedMeshRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();
        origionColor=skinnedMeshRenderer.sharedMaterial.color;
        npcDefine = Manager.NPCManager.Instance.GetNpcDefine(npcID);
        NPCManager.Instance.UpdateNpcPosition(npcID, transform.position);
        this.StartCoroutine(Actions());

        RefreshNpcStatus();
        QuestManager.Instance.OnQuestStatusChanged += this.OnQuestStatusChanged;
    }

    private void OnQuestStatusChanged()
    {
        this.RefreshNpcStatus();
    }

    private void RefreshNpcStatus()
    {
        questStatus=QuestManager.Instance.GetQuestStatusByNpc(npcID);
        UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
    }
    private void OnDestroy()
    {
        QuestManager.Instance.OnQuestStatusChanged -= this.OnQuestStatusChanged;
        if(UIWorldElementManager.Instance != null)
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
    }
    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
                yield return new WaitForSeconds(2f);
            else
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            this.Relax();
        }
    }
    private void Update() { }
    private void Relax()
    {
        animator.SetTrigger("Relax");
    }
    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }
    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (Manager.NPCManager.Instance.Interactive(npcDefine))
        {
            animator.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }
    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo=(Models.User.Instance.CurrentCharacterObject.transform.position-transform.position).normalized;
        while(Mathf.Abs(Vector3.Angle(transform.forward, faceTo)) > 5)
        {
            this.transform.forward = Vector3.Lerp(transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }
    void OnMouseDown()
    {
        if (Vector3.Distance(this.transform.position, User.Instance.CurrentCharacterObject.transform.position) > 2f)
        {
            User.Instance.CurrentCharacterObject.StartNav(this.transform.position);
        }
        Interactive();
    }
    void OnMouseOver()
    {
        Highlight(true);
    }
    void OnMouseEnter()
    {
        Highlight(true);
    }
    void OnMouseExit()
    {
        Highlight(false);
    }
    void Highlight(bool highlight)
    {
        if (highlight)
        {
            if (skinnedMeshRenderer.sharedMaterial.color != Color.white)
            {
                skinnedMeshRenderer.sharedMaterial.color=Color.white;
            }
        }
        else
        {
            if (skinnedMeshRenderer.sharedMaterial.color != origionColor)
            {
                skinnedMeshRenderer.sharedMaterial.color = origionColor;
            }
        }
    }
}
