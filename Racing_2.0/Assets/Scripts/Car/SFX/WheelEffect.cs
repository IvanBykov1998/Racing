using UnityEngine;

public class WheelEffect : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheels;
    [SerializeField] private ParticleSystem[] wheelsSmoke;

    [SerializeField] private float forwardSlipLimit;
    [SerializeField] private float sidewaysSlipLimit;

    [SerializeField] private AudioSource wheelAudio;

    [SerializeField] private GameObject skidPrefab;

    private WheelHit wheelHit;
    private Transform[] skidTrail;

    private void Start()
    {
        skidTrail = new Transform[wheels.Length];
    }

    private void Update()
    {
        bool isSlip = false;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].GetGroundHit(out wheelHit);

            if (wheels[i].isGrounded == true)
            {
                if (Mathf.Abs(wheelHit.forwardSlip) > forwardSlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) > sidewaysSlipLimit)
                {
                    if (skidTrail[i] == null)
                    {
                        skidTrail[i] = Instantiate(skidPrefab).transform;
                    }

                    if (wheelAudio.isPlaying == false)
                    {
                        wheelAudio.Play();
                    }

                    if (skidTrail[i] != null)
                    {
                        skidTrail[i].position = wheels[i].transform.position - wheelHit.normal * (wheels[i].radius + (0.5f * wheels[i].suspensionDistance));
                        // ƒобавил + (0.5f * wheels[i].suspensionDistance)), так как след рисовалс€ выше подвески с выхлопом(+- по центру колеса)
                        skidTrail[i].forward = -wheelHit.normal;

                        wheelsSmoke[i].transform.position = skidTrail[i].position;
                        wheelsSmoke[i].Emit(2);
                    }
                
                    isSlip = true;
                    continue;
                }
            }

            skidTrail[i] = null;
            wheelsSmoke[i].Stop();
        }

        if (isSlip == false)
        {
            wheelAudio.Stop();
        }
    }
}