using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGunController : GunController
{
    public float bulletLineLifeTime = 0.03f;

    public Transform spine;
    public Transform arm_position_left;
    public Transform arm_position_right;

    private LineRenderer bulletLineRenderer;
    [SerializeField]
    private PlayerAnimationController animController;
    
    private Vector3 aimPoint;
    private Vector3 spine_rotation_offset = new Vector3(0, -40, -100);
    private Vector3 arm_position_left_rotation_offset = new Vector3(0, 90, -45);
    private Vector3 arm_position_right_rotation_offset = new Vector3(0, 90, -45);

    private void Awake() {
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;

        //animController = gameObject.transform.parent.GetComponent<PlayerAnimationController>();
    }
    private void Start() {

    }

    protected override void Update() {
        base.Update();
    }
    
    
    private void LateUpdate() {
        spine.LookAt(aimPoint);
        spine.rotation = spine.rotation * Quaternion.Euler(spine_rotation_offset);
        arm_position_left.LookAt(aimPoint);
        arm_position_right.LookAt(aimPoint);
        arm_position_left.rotation = arm_position_left.rotation * Quaternion.Euler(arm_position_left_rotation_offset);
        arm_position_right.rotation = arm_position_right.rotation * Quaternion.Euler(arm_position_right_rotation_offset);


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
