using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGunController : GunController
{
    public float bulletLineLifeTime = 0.03f;

    private LineRenderer bulletLineRenderer;

    private void Awake() {
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    protected override void Update() {
        base.Update();
    }

    protected override void OnEnable() {
        base.OnEnable();
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
