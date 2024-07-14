using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SuckingMachine : MonoBehaviour
{
    public PlayerInput _playerInput;

    private bool _isSucking;

    public Transform _suckingPosition;

    public LayerMask _suckableLayer;

    [SerializeField] private ISuckable _target;

    public float _captureDistance = .1f;

    [SerializeField] private AnimationCurve _captureStrengthCurve;
    public float _suckingForceMultiplier, _aimAssistAmount;
    public float _maxSuckingDistance;

    public Animator _anim;

    public MeshRenderer _suckingCone;
    public float _suckingFXStartingTime;
    public float _suckingGradientDefault = 1, _dissolveDefault = 0.444f;
    private float _suckingGradientIdle = -1, _dissolveIdle = 0.343f;
    private Coroutine suckingFXCoroutine;

    [SerializeField] private ParticleSystem _biteParticle;
    [SerializeField] private Image _cursor;

    private bool _hasLockedOntoGhost;

    #region Instantiate Everything - Awake/Start
    void Awake()
    {
        _playerInput = new PlayerInput();

        _suckingCone.sharedMaterial.SetFloat("_Dissolve", _dissolveIdle);
        _suckingCone.sharedMaterial.SetFloat("_GradientRange", _suckingGradientIdle);
    }
    #endregion

    #region Input - OnEnable/OnDisable
    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Action.Sucking.performed += OnStartSucking;

        _playerInput.Action.Sucking.canceled += OnStopSucking;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Action.Sucking.performed -= OnStartSucking;

        _playerInput.Action.Sucking.canceled -= OnStopSucking;
    }
    private void OnStartSucking(InputAction.CallbackContext obj)
    {
        _isSucking = true;
        _anim.SetBool("IsSucking", true);
        AudioManager.instance.Play("VacuumStart");
        AudioManager.instance.Play("VacuumMiddle");

        // Start lerping sucking cone material variables
        if (suckingFXCoroutine != null)
            StopCoroutine(suckingFXCoroutine);
        suckingFXCoroutine = StartCoroutine(LerpSuckingFXVariables(_dissolveDefault, _suckingGradientDefault));
    }
    private void OnStopSucking(InputAction.CallbackContext obj)
    {
        _isSucking = false;
        if (_target != null)
            _target.ResetSuck();
        _target = null;
        _anim.SetBool("IsSucking", false);
        AudioManager.instance.Stop("VacuumStart");
        AudioManager.instance.Stop("VacuumMiddle");
        AudioManager.instance.Play("VacuumStop");

        if (suckingFXCoroutine != null)
            StopCoroutine(suckingFXCoroutine);
        suckingFXCoroutine = StartCoroutine(LerpSuckingFXVariables(_dissolveIdle, _suckingGradientIdle));
    }
    #endregion

    void FixedUpdate()
    {
        RaycastHit targetHit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out targetHit, _maxSuckingDistance, _suckableLayer))
        {
            if(targetHit.transform.gameObject.GetComponent<ISuckable>() == null)
                _cursor.color = Color.red;
            else _cursor.color = Color.white;
        }
        if (_isSucking)
        {
            RaycastHit hit;
            //Shoot raycast
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _maxSuckingDistance, _suckableLayer))
            {
                ISuckable hitSuckable = hit.transform.gameObject.GetComponent<ISuckable>();

                Vector3 direction = GetSuckingDirection(hit);

                if (_target == hitSuckable && _target != null)
                {
                    //If close enough to suck in completely
                    if (_captureDistance >= direction.magnitude)
                    {
                        CaptureSuckable();
                    }
                    else //Suck it in
                    {
                        SuckSuckableTowardsMachine(direction);
                        _hasLockedOntoGhost = true;
                    }
                }
                else
                {
                    if(hit.transform.gameObject != null)
                        SetTarget(hit.transform.gameObject);
                }
            }
            else
            {
                if (_target != null)
                    _target.ResetSuck();
                _target = null;
            }
        }
        else
        {
            _target = null;
        }
    }
    #region VFX
    private IEnumerator LerpSuckingFXVariables(float targetDissolve, float targetGradient)
    {
        float elapsedTime = 0f;
        float startDissolve = _suckingCone.sharedMaterial.GetFloat("_Dissolve");
        float startGradient = _suckingCone.sharedMaterial.GetFloat("_GradientRange");

        while (elapsedTime < _suckingFXStartingTime)
        {
            float t = elapsedTime / _suckingFXStartingTime;
            float dissolveValue = Mathf.Lerp(startDissolve, targetDissolve, t);
            float gradientValue = Mathf.Lerp(startGradient, targetGradient, t);

            SetSuckingFXVariables(dissolveValue, gradientValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the variables are set to the target values when the lerp finishes
        SetSuckingFXVariables(targetDissolve, targetGradient);
    }
    private void SetSuckingFXVariables(float dissolveValue, float gradientValue)
    {
        Material material = _suckingCone.sharedMaterial;
        material.SetFloat("_Dissolve", dissolveValue);
        material.SetFloat("_GradientRange", gradientValue);
    }
#endregion

    private void SetTarget(GameObject hit)
    {
        if(_target != null) _target.ResetSuck();
        if (hit.GetComponent<ISuckable>() != null)
        {
            _target = hit.GetComponent<ISuckable>();
            _target.IsBeingSucked();
        }
    }
    private Vector3 GetSuckingDirection(RaycastHit hit)
    {
        Vector3 direction = _suckingPosition.position - hit.transform.position;
        return direction;
    }
    void SuckSuckableTowardsMachine(Vector3 direction)
    {
        _target.GetGameObject().transform.position += direction.normalized *
            (_captureStrengthCurve.Evaluate(direction.magnitude / _maxSuckingDistance)) * _suckingForceMultiplier * Time.deltaTime;
        _target.GetGameObject().transform.LookAt(transform);
        _target.GetGameObject().transform.rotation = Quaternion.Inverse(_target.GetGameObject().transform.rotation);
        _target.GetGameObject().transform.eulerAngles = new Vector3(_target.GetGameObject().transform.eulerAngles.x, _target.GetGameObject().transform.eulerAngles.y + 180
            , _target.GetGameObject().transform.eulerAngles.z);
    }
    void MoveTowardsMiddleOfScreen(RaycastHit hit)
    {
        var distance = hit.distance;
        var direction = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)) - _target.GetGameObject().transform.position;
        _target.GetGameObject().transform.position += direction.normalized * _aimAssistAmount * Time.deltaTime;
    }
    private void CaptureSuckable()
    {
        Invoke("BiteEffect", .2f);
        _anim.SetTrigger("Capture");
        AudioManager.instance.Play("VacuumCapture");
        _target.ResetSuck();
        _target.Capture();
        _target = null;
    }
    void BiteEffect()
    {
        _biteParticle.Play();
    }
}
