using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGunController : GunController
{
    public float bulletLineLifeTime = 0.03f;

    private LineRenderer bulletLineRenderer;
    [SerializeField]
    private PlayerAnimationController animController;
    private Transform spine;
    private Animator a;
    Vector3 aimPoint;

    private void Awake() {
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;

        //animController = gameObject.transform.parent.GetComponent<PlayerAnimationController>();
    }
    private void Start() {
        a = animController.animator;
        spine = animController.animator.GetBoneTransform(HumanBodyBones.Spine); // upper body
    }

    protected override void Update() {
        base.Update();
    }
    
    Vector3 offset = new Vector3(0, -40, -100);
    private void LateUpdate() {
        spine.LookAt(aimPoint);
        spine.rotation = spine.rotation * Quaternion.Euler(offset);
        
        //this.transform.LookAt(aimPoint);
        //transform.rotation = transform.rotation * Quaternion.Euler(offset);
    }

    protected override void OnEnable() {
        base.OnEnable();
    }

    public override Vector3 CalcAimVector() {
        RaycastHit hitData;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
        //Vector3 aimPoint;

        if(Physics.Raycast(ray, out hitData, attackRange, layerMask))
        {
            aimPoint = hitData.point;
        }
        else
        {
            aimPoint = ray.origin + ray.direction * attackRange;
        }
        
        return (aimPoint - muzzle.position).normalized;
    }

    // 총알 생성
    // 총알의 방향은 총의 방향과 같도록
    public override void Shoot() {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if(Physics.Raycast(muzzle.position, muzzle.forward, out hit, attackRange)) {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target != null) {
                target.OnDamage(attackDamage, hit.point, hit.normal);
                CreateDamagePopup(hit.point, Camera.main.transform.rotation, (int)attackDamage);
            }

            hitPosition = hit.point;
        } else {
            hitPosition = muzzle.position + muzzle.forward * attackRange;
        }

        StartCoroutine(ShotEffect(hitPosition));
    }

    private IEnumerator ShotEffect(Vector3 hitPosition) {
        bulletLineRenderer.SetPosition(0, muzzle.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(bulletLineLifeTime);

        bulletLineRenderer.enabled = false;
    }
}
