using UnityEngine;
using TMPro;
/// <summary>
/// �˺���ʾ�ı��ĵ���ʵ����������ʾ�Ͷ���Ч����
/// ����û�����ף�
/// controller = FindObjectOfType<DamageTextController>();������ܻ����������⣬�����Ż�
/// </summary>
public class DamageTextItem : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float floatHeight = 1.5f;
    [SerializeField] private float floatDuration = 0.5f;
    [SerializeField] private float fadeDuration = 0.8f;
    [SerializeField] private float gravity = 2f;
    [SerializeField]private TextMeshProUGUI textMesh;
    private Vector3 initialPosition;
    private Vector3 velocity;
    private float currentTime;
    [SerializeField] private Color originalColor;
    private DamageTextController controller;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        controller = FindObjectOfType<DamageTextController>();
    }

    public void Initialize(float damageAmount, Color color, bool isCritical = false)
    {
        // �����ı�����ɫ
        textMesh.text = Mathf.RoundToInt(damageAmount).ToString();
        textMesh.color = color;
        originalColor = color;

        // ����Ǳ������Ŵ�����
        if (isCritical)
        {
            textMesh.fontSize *= 1.3f;
        }

        // ��ʼ����������
        initialPosition = transform.position;
        velocity = Vector3.up * floatHeight / floatDuration;
        currentTime = 0f;
    }

    void Update()
    {
        // ���¶���
        currentTime += Time.deltaTime;

        if (currentTime < floatDuration)
        {
            // �ϸ��׶�
            velocity -= Vector3.up * gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }
        else
        {
            // ��׹�׶�
            velocity += Vector3.down * gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }

        // ������ʧ
        float fadeProgress = Mathf.Clamp01((currentTime - floatDuration) / fadeDuration);
        Color fadedColor = originalColor;
        fadedColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
        //textMesh.color = fadedColor;

        // ������������յ������
        if (currentTime >= floatDuration + fadeDuration)
        {
            if (controller != null)
            {
                controller.ReturnDamageTextToPool(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnDisable()
    {
        // ����״̬
        if (textMesh != null)
        {
            textMesh.fontSize = textMesh.fontSize / 1.3f; // �ָ�ԭʼ��С
        }
    }
}