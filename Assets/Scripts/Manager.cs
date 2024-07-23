using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{

    Button _RestartButton;
    Transform _PlayerTransform;
    GameObject _WinText;
    GameObject _MovementButtons;
    bool _Won = false;


    [SerializeField] int _SpawnAmountOfDestructableObjects = 10;
    [SerializeField] List<GameObject> _DestructablePrefabsList = new List<GameObject>();

    [SerializeField] float _ObjectsSpawn_Axis_X_Max = 18.0f;
    [SerializeField] float _ObjectsSpawn_Axis_Y = 20.0f;
    [SerializeField] float _ObjectsSpawn_Axis_Z = 0.0f;



    WaitForSeconds _WaitBeforeNextObject = new WaitForSeconds(0.5f);




    Slider _Slider;
    private void Awake()
    {
        _PlayerTransform = GameObject.Find("Slime").transform;
        _WinText = GameObject.Find("Button_Restart");
        _MovementButtons = GameObject.Find("MovementButtons");
        _Slider = GameObject.Find("Hunger").GetComponent<Slider>() ;


        Restart();

    }

    private void Update()
    {

        ChangeSliderValue(_PlayerTransform.localScale.x);
        if (_PlayerTransform.localScale.x >= 10.0f && !_Won)
        {
            Win();
        }
    }
    void Win()
    {
        _Won = true;
        WinTextVisibility(true);
        MovementsButtonVisibilty(false);

    }
    public void Restart()
    {
        WinTextVisibility(false);
        MovementsButtonVisibilty(true);

        Vector3 playerStartingPosition = new Vector3(_ObjectsSpawn_Axis_X_Max / 2.0f, _ObjectsSpawn_Axis_Y, _ObjectsSpawn_Axis_Z);
        _PlayerTransform.GetComponent<CharacterBehaviour>().Restart(playerStartingPosition);

        ProceduralDestruction[] allObjectsToDestroy  = FindObjectsOfType<ProceduralDestruction>();
        _Won = false;
        foreach (var obj in allObjectsToDestroy)
        {
            Destroy(obj.gameObject);
        }

        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        yield return _WaitBeforeNextObject;
        if (_DestructablePrefabsList.Count > 0 && _DestructablePrefabsList != null)
            for (int i = 0; i < _SpawnAmountOfDestructableObjects; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, _DestructablePrefabsList.Count);
                GameObject newDestructableObject = Instantiate(_DestructablePrefabsList[randomIndex]);

                float randomAxisX = UnityEngine.Random.Range(0.0f, _ObjectsSpawn_Axis_X_Max);
                newDestructableObject.transform.position = new Vector3(randomAxisX, _ObjectsSpawn_Axis_Y, _ObjectsSpawn_Axis_Z);

                yield return _WaitBeforeNextObject;
            }

        else yield return null;
    }

    void MovementsButtonVisibilty(bool isVisible)
    {
        if (_MovementButtons != null) _MovementButtons.SetActive(isVisible);
    }

    void WinTextVisibility(bool isVisible)
    {
        if (_WinText != null) _WinText.SetActive(isVisible);
    }

    void ChangeSliderValue(float value)
    {
        if (_Slider != null) _Slider.value = value;
    }
}
