using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemLine : MonoBehaviour
{
    /// <summary>
    /// 粒子系统
    /// </summary>
    private ParticleSystem mParticleSystem;
    /// <summary>
    /// 线的材质
    /// </summary>
    public Material mMaterial;
    /// <summary>
    /// 粒子数组
    /// </summary>
    private ParticleSystem.Particle[] particles;
    /// <summary>
    /// LineRenderer数组
    /// </summary>
    private List<LineRenderer> linePool = new List<LineRenderer>();
    /// <summary>
    /// 当前存活的粒子
    /// </summary>
    private int numParticlesAlive;
    /// <summary>
    /// 连线距离
    /// </summary>
    public float MinDist = 5;
    /// <summary>
    /// 线的宽度
    /// </summary>
    public float LineWidth = 0.1f;

    /// <summary>
    /// 已经使用的LineRender位置
    /// </summary>
    private int index = 0;

    //public AnimationCurve LineWidthOverLifetime = AnimationCurve.Constant(0,1,1);
    void Start()
    {
        mParticleSystem = GetComponent<ParticleSystem>();
        //初始化粒子数组
        particles = new ParticleSystem.Particle[mParticleSystem.main.maxParticles];
    }

    void LateUpdate()
    {
        //获取粒子数据
        numParticlesAlive = mParticleSystem.GetParticles(particles);

        //两个粒子的距离小于MinDist就连线
        for(int i = 0;i < numParticlesAlive; i++)
        {
            for (int j = i + 1; j < numParticlesAlive; j++)
            {
                //粒子直接距离的平方
                float SqrDis = (particles[j].position - particles[i].position).sqrMagnitude;

                if (SqrDis < MinDist * MinDist)
                {
                    ParticleSystem.Particle particle = particles[j];
                    ParticleSystem.Particle cur_particle = particles[i];
                    //获取粒子的颜色
                    Color sColor = cur_particle.GetCurrentColor(mParticleSystem);
                    Color eColor = particle.GetCurrentColor(mParticleSystem);
                    //计算宽度
                    //float width = LineWidth*LineWidthOverLifetime.Evaluate(cur_particle.remainingLifetime/cur_particle.startLifetime);
                    //绘制线
                    DrawLine(cur_particle.position, particle.position, sColor, eColor, LineWidth);

                }
            }
        }

        mParticleSystem.SetParticles(particles, numParticlesAlive);
        for(int i=index;i<linePool.Count;i++)
        {
            linePool[i].gameObject.SetActive(false);
        }
        index = 0;
    }

    /// <summary>
    /// 绘制线条
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="startColor"></param>
    /// <param name="endColor"></param>
    /// <param name="width"></param>
    void DrawLine(Vector3 startPos,Vector3 endPos,Color startColor,Color endColor,float width = 0.1f)
    {
        LineRenderer line;

        if(linePool.Count==index)
        {
            GameObject go = new GameObject("tempLine");
            go.transform.parent = transform;
            line = go.AddComponent<LineRenderer>();
            line.useWorldSpace = true;
            line.material = mMaterial;
            linePool.Add(line);
            index++;
        }
        else
        {
            line = linePool[index++];
            line.gameObject.SetActive(true);
        }

        //点的数量
        line.positionCount = 2;
        
        //设置线的位置
        line.SetPosition(0,startPos);
        line.SetPosition(1,endPos);

        //线的颜色
        line.startColor = startColor;
        line.endColor = endColor;

        //线的宽度
        line.startWidth = width;
        line.endWidth = width;
    }
}
