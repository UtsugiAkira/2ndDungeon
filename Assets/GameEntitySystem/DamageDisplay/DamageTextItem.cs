using UnityEngine;
using TMPro;
/// <summary>
/// 伤害显示文本的单个实例，负责显示和动画效果。
/// 参数没玩明白，
/// controller = FindObjectOfType<DamageTextController>();这里可能会有性能问题，后续优化
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
        // 设置文本和颜色
        textMesh.text = Mathf.RoundToInt(damageAmount).ToString();
        textMesh.color = color;
        originalColor = color;

        // 如果是暴击，放大文字
        if (isCritical)
        {
            textMesh.fontSize *= 1.3f;
        }

        // 初始化动画变量
        initialPosition = transform.position;
        velocity = Vector3.up * floatHeight / floatDuration;
        currentTime = 0f;
    }

    void Update()
    {
        // 更新动画
        currentTime += Time.deltaTime;

        if (currentTime < floatDuration)
        {
            // 上浮阶段
            velocity -= Vector3.up * gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }
        else
        {
            // 下坠阶段
            velocity += Vector3.down * gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }

        // 渐变消失
        float fadeProgress = Mathf.Clamp01((currentTime - floatDuration) / fadeDuration);
        Color fadedColor = originalColor;
        fadedColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
        //textMesh.color = fadedColor;

        // 动画结束后回收到对象池
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
        // 重置状态
        if (textMesh != null)
        {
            textMesh.fontSize = textMesh.fontSize / 1.3f; // 恢复原始大小
        }
    }
}