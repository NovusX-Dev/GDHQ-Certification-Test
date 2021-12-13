using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SciFiBeamStatic : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] float _damage = 0.15f;
    [SerializeField] float _damageRate = 0.5f;
    [SerializeField] Vector2 _beamSpawnRateRange = new Vector2(2f, 4f);
    [SerializeField] Transform _beamContainer;


    [Header("Prefabs")]
    public GameObject beamLineRendererPrefab = null; //Put a prefab with a line renderer onto here.
    public GameObject beamStartPrefab = null; //This is a prefab that is put at the start of the beam.
    public GameObject beamEndPrefab = null; //Prefab put at end of beam.

    [Header("Beam Options")]
    public bool alwaysOn = true; //Enable this to spawn the beam when script is loaded.
    public bool beamCollides = true; //Beam stops at colliders
    public float beamLength = 100; //Ingame beam length
    public float beamEndOffset = 0f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 0f; //How fast the texture scrolls along the beam, can be negative or positive.
    public float textureLengthScale = 1f;   //Set this to the horizontal length of your texture relative to the vertical. 
                                            //Example: if texture is 200 pixels in height and 600 in length, set this to 3

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;
    private PlayerHealth _player = null;
    private float _nextDamage = -1f;
    private bool _activateBeam = true;
    private bool _isBeamActive;

    private void OnEnable()
    {
        _beamContainer = transform.parent;

        if (alwaysOn) //When the object this script is attached to is enabled, spawn the beam.
            SpawnBeam();
        if (beam == null)
            SpawnBeam();
    }

    private void OnDisable() //If the object this script is attached to is disabled, remove the beam.
    {
        RemoveBeam();
    }

    private void Update()
    {
        if (_activateBeam)
            StartCoroutine(BeamActivationRoutine());

        if (_player != null && _isBeamActive)
        {
            if (Time.time > _nextDamage)
            {
                _player.DamagePlayer(_damage);
                _nextDamage = Time.time + _damageRate;
            }
        }
    }

    void FixedUpdate()
    {
        if (beam) //Updates the beam
        {
            line.SetPosition(0, transform.position);

            Vector3 end;
            RaycastHit hit;
            if (beamCollides && Physics.Raycast(transform.position, transform.forward, out hit)) //Checks for collision
            {
                end = hit.point - (transform.forward * beamEndOffset);
                if (hit.collider.CompareTag("Player"))
                {
                    _player = hit.collider.GetComponent<PlayerHealth>();
                }
                else if (_player != null) _player = null;
            }
            else
            {
                end = transform.position + (transform.forward * beamLength);
                if (_player != null) _player = null;
            }

            line.SetPosition(1, end);

            if (beamStart)
            {
                beamStart.transform.position = transform.position;
                beamStart.transform.LookAt(end);
            }
            if (beamEnd)
            {
                beamEnd.transform.position = end;
                beamEnd.transform.LookAt(beamStart.transform.position);
            }

            float distance = Vector3.Distance(transform.position, end);
            line.material.mainTextureScale = new Vector2(distance / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
            line.material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
        }
    }

    public void SpawnBeam() //This function spawns the prefab with linerenderer
    {
        if (beamLineRendererPrefab)
        {
            if (beamStartPrefab)
                beamStart = Instantiate(beamStartPrefab);
            if (beamEndPrefab)
                beamEnd = Instantiate(beamEndPrefab);
            beam = Instantiate(beamLineRendererPrefab);
            beam.transform.position = transform.position;
            beam.transform.parent = transform;
            beam.transform.rotation = transform.rotation;
            line = beam.GetComponent<LineRenderer>();
            line.useWorldSpace = true;
#if UNITY_5_5_OR_NEWER
            line.positionCount = 2;
#else
			line.SetVertexCount(2); 
#endif
        }
        else
            print("Add a hecking prefab with a line renderer to the SciFiBeamStatic script on " + gameObject.name + "! Heck!");

        beam.transform.parent = _beamContainer;
        beamStart.transform.parent = _beamContainer;
        beamEnd.transform.parent = _beamContainer;
    }

    public void RemoveBeam() //This function removes the prefab with linerenderer
    {
        if (beam)
            Destroy(beam);
        if (beamStart)
            Destroy(beamStart);
        if (beamEnd)
            Destroy(beamEnd);
    }

    public void BeamActivation(bool active)
    {
        if (beam)
            beam.SetActive(active);
        if (beamStart)
            beamStart.SetActive(active);
        if (beamEnd)
            beamEnd.SetActive(active);

        _isBeamActive = active;
    }

    IEnumerator BeamActivationRoutine()
    {
        _activateBeam = false;
        BeamActivation(true);
        yield return new WaitForSeconds(Random.Range(_beamSpawnRateRange.x, _beamSpawnRateRange.y));
        BeamActivation(false);
        yield return new WaitForSeconds(Random.Range(_beamSpawnRateRange.x, _beamSpawnRateRange.y));
        _activateBeam = true;
    }

}