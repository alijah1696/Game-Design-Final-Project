using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LiquidAnimation : MonoBehaviour
{
    [SerializeField] Color particle_Color;
    public bool playsParticle;

    private int CorsnersCount = 2;
    [SerializeField]
    private SpriteShapeController spriteShapeController;
    [SerializeField]
    private GameObject wavePointPref;
    [SerializeField]
    private GameObject wavePoints;

    [SerializeField]
    private float resistance;


    [SerializeField]
    [Range(1, 100)]
    private int WavesCount;
    private List<WavePoint> springs = new();
    // How stiff should our spring be constnat
    public float springStiffness = 0.1f;
    // Slowing the movement over time
    public float dampening = 0.03f;
    // How much to spread to the other springs
    public float spread = 0.006f;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnValidate() {
        if (gameObject.activeInHierarchy) {
            // Clean wavepoints 
            StartCoroutine(CreateWaves());
        }
    }


    IEnumerator CreateWaves() {
        foreach (Transform child in wavePoints.transform) {
            StartCoroutine(Destroy(child.gameObject));
        }
        yield return null;
        SetWaves();
        yield return null;
    }

    IEnumerator Destroy(GameObject go) {
        yield return null;
        DestroyImmediate(go);
    }

    private void SetWaves() { 
        Spline waveSpline = spriteShapeController.spline;
        int wavePointsCount = waveSpline.GetPointCount();

        // Remove middle points for the waves
        // Keep only the corners
        // Removing 1 point at a time we can remove only the 1st point
        // This means every time we remove 1st point the 2nd point becomes first
        for (int i = CorsnersCount; i < wavePointsCount - CorsnersCount; i++) {
            waveSpline.RemovePointAt(CorsnersCount);
        }

        Vector3 waveTopLeftCorner = waveSpline.GetPosition(1);
        Vector3 waveTopRightCorner = waveSpline.GetPosition(2);
        float waveWidth = waveTopRightCorner.x - waveTopLeftCorner.x;

        float spacingPerWave = waveWidth / (WavesCount+1);
        // Set new points for the waves
        for (int i = WavesCount; i > 0 ; i--) {
            int index = CorsnersCount;

            float xPosition = waveTopLeftCorner.x + (spacingPerWave*i);
            Vector3 wavePoint = new Vector3(xPosition, waveTopLeftCorner.y, waveTopLeftCorner.z);
            waveSpline.InsertPointAt(index, wavePoint);
            waveSpline.SetHeight(index, 0.1f);
            waveSpline.SetCorner(index, false);
            waveSpline.SetTangentMode(index, ShapeTangentMode.Continuous);

        }


        // loop through all the wave points
        // plus the both top left and right corners
        springs = new();
        for (int i = 0; i <= WavesCount+1; i++) {
            int index = i + 1; 
            
            Smoothen(waveSpline, index);

            GameObject wp = Instantiate(wavePointPref, wavePoints.transform, false);
            wp.transform.localPosition = waveSpline.GetPosition(index);

            WavePoint wavePoint = wp.GetComponent<WavePoint>();
            wavePoint.Init(spriteShapeController, resistance);
            
            //add settings
            wavePoint.shouldPlay = playsParticle;
            wavePoint.particle_Color = particle_Color;
            springs.Add(wavePoint);
        }
    }

    private void Smoothen(Spline waveSpline, int index){
        Vector3 position = waveSpline.GetPosition(index);
        Vector3 positionPrev = position;
        Vector3 positionNext = position;
        if (index > 1) {
            positionPrev = waveSpline.GetPosition(index-1);
        }
        if (index - 1 <= WavesCount) {
            positionNext = waveSpline.GetPosition(index+1);
        }

        Vector3 forward = gameObject.transform.forward;

        float scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;

        Vector3 leftTangent = (positionPrev - position).normalized * scale;
        Vector3 rightTangent = (positionNext - position).normalized * scale;

        SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent, out leftTangent);
        
        waveSpline.SetLeftTangent(index, leftTangent);
        waveSpline.SetRightTangent(index, rightTangent);
    }

    
    void FixedUpdate(){
        foreach(WavePoint wavePointComponent in springs) {
            wavePointComponent.WaveSpringUpdate(springStiffness, dampening);
            wavePointComponent.WavePointUpdate();
        }

        UpdateSprings();
    }

    private void UpdateSprings() { 
        int count = springs.Count;
        float[] left_deltas = new float[count];
        float[] right_deltas = new float[count];

        for(int i = 0; i < count; i++) {
            if (i > 0) {
                left_deltas[i] = spread * (springs[i].height - springs[i-1].height);
                springs[i-1].velocity += left_deltas[i];
            }
            if (i < springs.Count - 1) {
                right_deltas[i] = spread * (springs[i].height - springs[i+1].height);
                springs[i+1].velocity += right_deltas[i];
            }
        }
    }

    private void Splash(int index, float speed) { 
        if (index >= 0 && index < springs.Count) {
            springs[index].velocity += speed;
        }
    }
}
