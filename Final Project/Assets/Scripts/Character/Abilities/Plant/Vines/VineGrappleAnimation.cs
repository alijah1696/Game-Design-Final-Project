using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrappleAnimation : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] private int resolution, waveCount, wobbleCount;
    [SerializeField] private float waveSize, animSpeed;
    [SerializeField] private float curlNumber;
    [SerializeField] private float curlRadius;
    [SerializeField] private float curlCutOff;

    private bool animationDone = true;

    float animPercent;

    private bool lineBroken;

    private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBroken(bool state){
        lineBroken = state;
    }

    public float Progress(){
        return animPercent;
    }
    
    public IEnumerator AnimateRope(Vector3 targetPos, float speedMulti){
        animationDone = false;

        line.positionCount = resolution;
        LeanTween.value(gameObject, UpdateAnimPercent, 0, 1f, animSpeed * speedMulti).setEase(LeanTweenType.easeOutSine);
        
        while(animPercent < 1 || !lineBroken){
            float angle = LookAtAngle(targetPos - transform.position);
            SetPoints(targetPos, animPercent, angle, speedMulti);
            yield return null;
        }

        LeanTween.value(gameObject, UpdateAnimPercent, 1f, 0f, (animSpeed) / 2).setEase(LeanTweenType.easeOutSine);
        while(animPercent > 0){
            float angle = LookAtAngle(targetPos - transform.position);
            SetPoints(targetPos, animPercent, angle, 1f);
            yield return null;
        }
        
        animationDone = true;
    }

    public bool AnimationDone(){
        return animationDone;
    }

    public bool FullyExtended(){
        return (animPercent == 1f);
    }

    public void UpdateAnimPercent(float p){
        animPercent = p;
    }

    private void SetPoints(Vector3 targetPos, float percent, float angle, float speedMulti){
        Vector3 ropeEnd = Vector3.Lerp(transform.position, targetPos, percent);
        float length = Vector2.Distance(transform.position, ropeEnd);


        for(int i = 0; i < resolution; i++){
            float xPos = (float) i / resolution * length;
            float reversePercent  = 1 - percent;

            float amplitude = Mathf.Sin(reversePercent * wobbleCount * Mathf.PI) * ((1f - (float) i / resolution) * (waveSize * speedMulti/2));
            
            float yPos = Mathf.Sin((float) waveCount * i / resolution * 2 * Mathf.PI * reversePercent) * amplitude;
            yPos += Mathf.Sin((float) ((waveCount - 2) * speedMulti) * i / resolution * 2 * Mathf.PI) * waveSize/15 * percent;

            Vector2 pos = RotatePoint(new Vector2(xPos + transform.position.x, yPos + transform.position.y), transform.position, angle);
            Vector2 curlpos = pos;

            float curlRes = (float)i / (float)resolution;
            if(percent > 0.75f){
                float cpercent = (percent-0.25f)/0.75f;
                if (curlRes > (curlCutOff * cpercent))
                {
                    float curlProgress = (curlRes - (curlCutOff * cpercent)) / (1f - (curlCutOff * cpercent));
                    float curlAngle = curlProgress * curlNumber * cpercent;
                    //x = rcos(theta) y= rsin(theta)
                    float radius = curlRadius * curlProgress * cpercent;
                    curlpos += new Vector2(Mathf.Sin(curlAngle) * radius, Mathf.Cos(curlAngle) * radius);
                }
            }

            line.SetPosition(i, Vector2.Lerp(pos, curlpos, (percent-0.25f)/0.75f));
        }
    }

    Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle){
        Vector2 dir = point - pivot;
        dir = Quaternion.Euler(0, 0, angle) * dir;
        point  = dir + pivot;
        return point;
    }

    private float LookAtAngle(Vector2 target){
        return Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
    }
}
