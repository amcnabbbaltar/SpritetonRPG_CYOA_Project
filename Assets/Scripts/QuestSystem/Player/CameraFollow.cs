using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private Transform target;

    private void OnEnable()
    {
        GameEventsManager.instance.characterEvents.onActiveCharacterChanged += OnActiveCharacterChanged;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.characterEvents.onActiveCharacterChanged -= OnActiveCharacterChanged;
    }

    private void OnActiveCharacterChanged(int index, PlayerContinuousGridMovement character)
    {
        target = character.transform;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }
}
