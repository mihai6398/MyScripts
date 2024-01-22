using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrickSlowMotion : MonoBehaviour
{
    public event Action OnStartSlowMotion = delegate {  };
    public event Action OnEndSlowMotion = delegate {  };
    
    [SerializeField, Range(0.1f, 1)] private float _slowFactor = 0.5f;
    [SerializeField] private RCC_CarControllerV3 _target;
    [SerializeField] private float _speedBorder = 40f;
    [SerializeField, Range(0, 5f)] private float _delayFail = 2f;

    private float _normalFixedDeltaTime;
    private Coroutine _slowMotionCoroutine;
    private Coroutine _checkFailedCoroutine;
    private float _fixedDeltaTime;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        _fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered in -> " + name);
        if (other.transform != _target.transform)
            return;
        
        StopCoroutines(_slowMotionCoroutine, _checkFailedCoroutine);
        
        _slowMotionCoroutine = StartCoroutine(StopSlowMotion());
        _checkFailedCoroutine = StartCoroutine(StopFlyingCoroutine(_delayFail));
    }

    private IEnumerator StopSlowMotion()
    {
        yield return new WaitUntil(() => !_target.isGrounded);
        
        if (_target.speed < _speedBorder)
        {
            Debug.Log($"Not enough speed. Current {_target.speed};  Min value {_speedBorder}");
            _slowMotionCoroutine = null;
            yield break;
        }
        
        Debug.Log("Car start fly");
        Time.timeScale = _slowFactor;
        Time.fixedDeltaTime = _fixedDeltaTime * _slowFactor;
        StopCoroutines(_checkFailedCoroutine);
        OnStartSlowMotion.Invoke();
        
        yield return new WaitUntil(() => _target.isGrounded);
        
        Debug.Log("Car on ground");
        Time.timeScale = 1;
        Time.fixedDeltaTime = _fixedDeltaTime;
        _slowMotionCoroutine = null;
        OnEndSlowMotion.Invoke();
    }

    private IEnumerator StopFlyingCoroutine(float delay)
    {
            yield return new WaitForSecondsRealtime(delay);

            if (_slowMotionCoroutine != null)
            {
                StopCoroutine(_slowMotionCoroutine);
                Debug.Log("Car not start trick");
            }
            _checkFailedCoroutine = null;
    }

    private void StopCoroutines(params Coroutine[] coroutines)
    {
        if(coroutines == null)
            return;

        for (var index = 0; index < coroutines.Length; index++)
        {
            if (coroutines[index] == null) continue;
            
            StopCoroutine(coroutines[index]);
            coroutines[index] = null;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.6f);
        Gizmos.DrawSphere(transform.position, 2);
    }
}