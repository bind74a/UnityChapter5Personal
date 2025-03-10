using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]//해,달의 위치 0도에서 180도
    public float time;//시간값 0.5f 가될시 12시 점심
    public float fullDayLength;//하루의 시간값
    public float startTime = 0.4f; // 0.5f 가 정오라고 생각할것 0.4 는 9시정도
    private float timeRate;
    public Vector3 noon; // 정오 해,달 위치 Vector 90 0 0

    [Header("Sun")]//해
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]//달
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]//빛 설정 값
    public AnimationCurve lightinglntensityMultiplier;
    public AnimationCurve reflectionlntensityMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        timeRate = 1.0f / fullDayLength;// fullDayLength 은 하루의 시간값
        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; //시간 증가값

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightinglntensityMultiplier.Evaluate(time);//렌더링 옵션 ambientIntensity 연결
        RenderSettings.reflectionIntensity = reflectionlntensityMultiplier.Evaluate(time);//reflectionIntensity 연결
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time); //시간의 따른 밝기 조절

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;//해,달 각도 계산
        lightSource.color = gradient.Evaluate(time);//빛 색변환
        lightSource.intensity = intensity;//빛 밝기

        GameObject go = lightSource.gameObject; //lightSource 에 들어본 오브젝트를 go 변수로 지정


        if (lightSource.intensity == 0 && go.activeInHierarchy)//빛의 밝기가 0이며 부보와 자식의 오브젝트가 활성화시 true
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
