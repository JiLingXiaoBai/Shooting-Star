using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    float minX;
    float maxX;
    float minY;
    float maxY;
    float middleX;

    private void Start()
    {
        Camera mainCamera = Camera.main;

        Vector2 leftBottom = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 rightTop = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));

        minX = leftBottom.x;
        maxX = rightTop.x;
        minY = leftBottom.y;
        maxY = rightTop.y;

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;
    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);
        return position;
    }

    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }

    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(middleX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }

    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }
}
