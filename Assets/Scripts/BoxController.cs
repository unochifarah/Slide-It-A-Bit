using UnityEngine;

public class BoxController : MonoBehaviour
{
    public int height, pattern, type;
    private Vector2 offset;
    private bool isDragging = false;
    private Vector2 initialPosition;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color glowColor = new Color(1.2f, 1.2f, 1.2f, 1f);
    private const float snapThreshold = 0.8f;
    public bool isPlaced { get; private set; }

    private float swayAmplitude = 3f;
    private float swayFrequency = 5f;
    private float swayStopSpeed = 1f;

    private void Start()
    {
        initialPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnMouseDown()
    {
        offset = (Vector2)transform.position - GetMouseWorldPos();
        isDragging = true;
        spriteRenderer.color = glowColor;
        DetachFromHook();
    }

    private void OnMouseDrag()
    {
        if (isDragging)
            transform.position = GetMouseWorldPos() + offset;
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            SnapToNearest();
            spriteRenderer.color = originalColor;
            Debug.Log("Box released, publishing BoxMoved event.");
            EventBus.PublishBoxMoved();
        }
    }

    private void SnapToNearest()
    {
        Collider2D[] nearbyHooks = Physics2D.OverlapCircleAll(transform.position, snapThreshold, LayerMask.GetMask("HookWall"));
        Transform bestHook = null;
        float bestDistance = float.MaxValue;

        foreach (var hook in nearbyHooks)
        {
            if (hook.transform.childCount > 0) continue;
            float distance = Vector2.Distance(transform.position, hook.transform.position);
            if (distance < bestDistance)
            {
                bestHook = hook.transform;
                bestDistance = distance;
            }
        }

        if (bestHook != null)
        {
            transform.position = bestHook.position;
            transform.SetParent(bestHook);
            SetPlacedState(true);
            Debug.Log($"✅ Box (Height: {height}) successfully placed on a hook!");
            HookController hookController = bestHook.GetComponent<HookController>();
            if (hookController != null) hookController.attachedBox = this;
        }
        else 
        {
            SetPlacedState(false);
            Debug.Log($"❌ Box (Height: {height}) could not be placed.");
        }
    }

    private Vector2 GetMouseWorldPos() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public void DetachFromHook()
    {
        if (transform.parent != null && transform.parent.CompareTag("HookWall"))
            transform.SetParent(null);
    }

    public void ResetPosition()
    {
        if (transform.parent != null && transform.parent.CompareTag("HookWall"))
        {
            HookController hook = transform.parent.GetComponent<HookController>();
            if (hook != null)
            {
                hook.attachedBox = null;
            }
            transform.SetParent(null);
        }

        transform.position = initialPosition;
        isPlaced = false;
    }

    public void SetPlacedState(bool state) => isPlaced = state;

    private void Update()
    {
        if (isDragging)
        {
            float swayAngle = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;
            transform.rotation = Quaternion.Euler(0, 0, swayAngle);
        }
        else if (isPlaced)
        {
            float currentZ = transform.rotation.eulerAngles.z;
            float newZ = Mathf.LerpAngle(currentZ, 0f, Time.deltaTime * swayStopSpeed);
            transform.rotation = Quaternion.Euler(0, 0, newZ);
        }
    }
}
