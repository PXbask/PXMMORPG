using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;


public class EntityController : MonoBehaviour, Manager.IEntityNotify
{

    public Animator anim;
    public Rigidbody rb;
    private AnimatorStateInfo currentBaseState;

    public Entity entity;

    public UnityEngine.Vector3 position;
    public UnityEngine.Vector3 direction;
    Quaternion rotation;

    public UnityEngine.Vector3 lastPosition;
    Quaternion lastRotation;

    public float speed;
    public float animSpeed = 1.5f;
    public float jumpPower = 3.0f;

    public bool isPlayer = false;

    // Use this for initialization
    void Start () {
        if (entity != null)
        {
            Manager.EntityManager.Instance.RegisterEntityChangeNotify(this.entity.entityId, this);
            this.UpdateTransform();
        }

        if (!this.isPlayer)
            rb.useGravity = false;
    }
    void OnDestroy()
    {
        if(UIWorldElementManager.Instance != null)
            UIWorldElementManager.Instance.RemoveElement(transform);
    }
    void UpdateTransform()
    {
        this.position = GameObjectTool.LogicToWorld(entity.position);
        this.direction = GameObjectTool.LogicToWorld(entity.direction);

        this.rb.MovePosition(this.position);
        this.transform.forward = this.direction;
        this.lastPosition = this.position;
        this.lastRotation = this.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.entity == null)
            return;

        this.entity.OnUpdate(Time.fixedDeltaTime);

        if (!this.isPlayer)
        {
            this.UpdateTransform();
        }
    }

    public void OnEntityEvent(EntityEvent entityEvent)
    {
        switch(entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
        }
    }

    public void OnEntityRemoved()
    {
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveElement(this.transform);
        }
        Destroy(gameObject);
    }

    public void OnEntityChanged(Entity entity)
    {
        Debug.LogFormat("OnEntityChanged: ID:{0} Postion:{1} Direction:{2} Speed:{3}",
            entity.entityId, entity.position, entity.direction, entity.speed);
    }
}
