using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class VoiceSplineGenerator : MonoBehaviour
{
    public SpriteShapeController shapeController;
    public float generationSpeed;
    public float horizontalPointSpacing = 1f;
    public float tangentScale = 1f;
    public float minPointDistance = 1f;
    public Vector2 generationOffset;

    public ShapeTangentMode tangentMode;

    private ListenerComponent mic;
    private Spline spline;
    private bool isGenerating = false;
    private float horizontalLength = 0f;
    private float distanceToNewPoint;

    // Start is called before the first frame update
    void Start()
    {
        mic = GetComponent<ListenerComponent>();
        shapeController = shapeController.GetComponent<SpriteShapeController>();
        spline = shapeController.spline;
        ResetShape();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log(spline.GetPointCount());

            if (!isGenerating)
            {
                isGenerating = true;
                spline.InsertPointAt(0, new Vector3(generationOffset.x - minPointDistance, generationOffset.y + mic.pitchHeight, 0f));

                Debug.Log(spline.GetPointCount());

                spline.SetTangentMode(0, tangentMode);
                spline.SetLeftTangent(0, Vector2.left);
                spline.SetRightTangent(0, Vector2.right);
                spline.InsertPointAt(1, new Vector3(generationOffset.x, generationOffset.y + mic.pitchHeight, 0f));

                Debug.Log(spline.GetPointCount());

                spline.SetTangentMode(1, tangentMode);
                spline.SetLeftTangent(1, mic.pitchTangent * Vector2.left);
                spline.SetRightTangent(1, mic.pitchTangent * Vector2.right);
            }

            distanceToNewPoint -= Time.deltaTime * generationSpeed;
            horizontalLength += Time.deltaTime * generationSpeed;
            spline.SetPosition(spline.GetPointCount() - 1, new Vector3(generationOffset.x + horizontalLength, mic.pitchHeight, 0f));
            spline.SetLeftTangent(spline.GetPointCount() - 1, new Vector2(-horizontalPointSpacing, mic.pitchTangent).normalized * tangentScale);
            spline.SetRightTangent(spline.GetPointCount() - 1, new Vector2(horizontalPointSpacing, mic.pitchTangent).normalized * tangentScale);

            if (distanceToNewPoint < 0f)
            {
                distanceToNewPoint = horizontalPointSpacing;
                spline.InsertPointAt(spline.GetPointCount(), new Vector3(generationOffset.x + horizontalLength - minPointDistance, mic.pitchHeight, 0f));
                spline.SetTangentMode(spline.GetPointCount() - 1, tangentMode);
                spline.SetLeftTangent(spline.GetPointCount() - 1, new Vector2(-horizontalPointSpacing, mic.pitchTangent).normalized * tangentScale);
                spline.SetRightTangent(spline.GetPointCount() - 1, new Vector2(horizontalPointSpacing, mic.pitchTangent).normalized * tangentScale);
            }

            shapeController.UpdateSpriteShapeParameters();
        }
        else if(isGenerating)
        {
            isGenerating = false;
            horizontalLength = 0f;
            ResetShape();
        }
    }

    void ResetShape()
    {
        spline.Clear();
        shapeController.UpdateSpriteShapeParameters();
        distanceToNewPoint = horizontalPointSpacing;
    }
}
