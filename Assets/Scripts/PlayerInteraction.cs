using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    const float _DevourTotalTime = 3.0f;
    float _DevourRemainingTime = _DevourTotalTime;
    bool _DevourStarted = false;
    Transform _PlayerTransform;

    Vector3 _PlayerSlimeEnlargeSpeed = new Vector3(0.001f, 0.001f, 0.001f);
    private void FixedUpdate()
    {
        if(_DevourStarted && _DevourRemainingTime > 0.0f)
        {
            _DevourRemainingTime -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, _PlayerTransform.position, 0.1f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.05f);

            _PlayerTransform.localScale += _PlayerSlimeEnlargeSpeed;
        }

        else if(_DevourStarted) Destroy(gameObject);


        
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer==13 && !_DevourStarted)
        {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<MeshCollider>());
            StartDevour(collision.transform);
        }
    }

    public void StartDevour(Transform playerTransform)
    {
        _DevourStarted = true;
        _PlayerTransform = playerTransform;
        _DevourRemainingTime = _DevourTotalTime;
    }


}
