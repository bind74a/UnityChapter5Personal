using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]//��,���� ��ġ 0������ 180��
    public float time;//�ð��� 0.5f ���ɽ� 12�� ����
    public float fullDayLength;//�Ϸ��� �ð���
    public float startTime = 0.4f; // 0.5f �� ������� �����Ұ� 0.4 �� 9������
    private float timeRate;
    public Vector3 noon; // ���� ��,�� ��ġ Vector 90 0 0

    [Header("Sun")]//��
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]//��
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]//�� ���� ��
    public AnimationCurve lightinglntensityMultiplier;
    public AnimationCurve reflectionlntensityMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        timeRate = 1.0f / fullDayLength;// fullDayLength �� �Ϸ��� �ð���
        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; //�ð� ������

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightinglntensityMultiplier.Evaluate(time);//������ �ɼ� ambientIntensity ����
        RenderSettings.reflectionIntensity = reflectionlntensityMultiplier.Evaluate(time);//reflectionIntensity ����
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time); //�ð��� ���� ��� ����

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;//��,�� ���� ���
        lightSource.color = gradient.Evaluate(time);//�� ����ȯ
        lightSource.intensity = intensity;//�� ���

        GameObject go = lightSource.gameObject; //lightSource �� �� ������Ʈ�� go ������ ����


        if (lightSource.intensity == 0 && go.activeInHierarchy)//���� ��Ⱑ 0�̸� �κ��� �ڽ��� ������Ʈ�� Ȱ��ȭ�� true
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
