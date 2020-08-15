using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public static IEnumerator GoForward(Transform transform, float movementTime)
    {
        float elapsedTime = 0;

        Vector3 originalPos = transform.position;

        Vector3 targetPos = (originalPos + transform.forward * 2);

        while (elapsedTime < movementTime)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, (elapsedTime / movementTime));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPos;
    }

    public static IEnumerator Rotate(Transform transform, float degrees, Vector3 axis, float movementTime)
    {
        float elapsedTime = 0;

        Quaternion originalRotation = transform.rotation;

        Quaternion targetRotation = Quaternion.Euler(degrees * axis);

        while (elapsedTime < movementTime)
        {
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, (elapsedTime / movementTime));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.rotation = targetRotation;
    }

    public static IEnumerator RotateTo(Transform transform, float angle, Vector3 axis, float movementTime)
    {
        // calculate rotation speed
        float rotationSpeed = angle / movementTime;

        // save starting rotation position
        Quaternion startRotation = transform.rotation;

        float deltaAngle = 0;

        // rotate until reaching angle
        while (deltaAngle < angle)
        {
            deltaAngle += rotationSpeed * Time.deltaTime;
            deltaAngle = Mathf.Min(deltaAngle, angle);

            transform.rotation = startRotation * Quaternion.AngleAxis(deltaAngle, axis);

            yield return null;
        }

    }
}
