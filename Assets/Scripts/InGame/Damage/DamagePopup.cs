using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;
    public float ySpeed = 1f;
    public float startFadeOutTime = 1f;
    public float lifeTime = 1.5f;
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
        transform.position += new Vector3(0, ySpeed * Time.deltaTime, 0);
        transform.forward = transform.position - Camera.main.transform.position;
    }

    public void Setup(int damageAmount) {
        textMesh.SetText(damageAmount.ToString());
    }

    public IEnumerator FadeOut() {
        float f = 0f;
        float fadeOutTime = lifeTime - startFadeOutTime;
        yield return new WaitForSeconds(startFadeOutTime);

        while(f < fadeOutTime) {
            textMesh.color = Color.Lerp(startColor, endColor, f / fadeOutTime);
            f += Time.deltaTime;
            yield return null;
        }
    }
}
