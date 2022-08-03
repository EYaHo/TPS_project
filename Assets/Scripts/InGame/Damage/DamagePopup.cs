using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;
    public float ySpeed = 0.1f;
    public float lifeTime = 1f;
    public Color startColor;
    private Color endColor;
    private float startTime;

    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
        textMesh.color = startColor;
        endColor = startColor;
        endColor.a = 0f;
        Destroy(this.gameObject, lifeTime);
        StartCoroutine(FadeOut());
    }

    private void Update() {
        transform.position += new Vector3(0, ySpeed, 0);
    }

    public void Setup(int damageAmount) {
        textMesh.SetText(damageAmount.ToString());
    }

    public IEnumerator FadeOut() {
        float f = 0f;

        while(f < lifeTime) {
            textMesh.color = Color.Lerp(startColor, endColor, f / lifeTime);
            f += Time.deltaTime;
            yield return null;
        }
    }
}
