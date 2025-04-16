using UnityEngine;

public class ChaiseMusicale : MonoBehaviour , IInteractable
{
    AudioSource _source = null;
    [SerializeField] float _timeBeforeStopMusic = 2.0f;
    [SerializeField] GameObject _startGameObject = null;
    [SerializeField] GameObject _doorGameObject = null;
    private bool _canInteract = false;
    public void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.Play();
    }

    private void Update()
    {
        _timeBeforeStopMusic -= Time.deltaTime;
        if(_timeBeforeStopMusic < 0) 
        {
            _source.Stop();
            _canInteract = true;
        }
    }


    public void Interact(PlayerPickUp interactor)
    {
        if (!_canInteract) return;

        _canInteract = false;
        _startGameObject.SetActive(true);
        _doorGameObject.SetActive(true);
    }

    public string GetTextInteract()
    {
        return "Serrer le poing pour prend la place";
    }

    public string GetTextCantInteract()
    {
        return "Attendre la fin de la musique pour prendre la place";
    }

    public void SetCanTake(bool canTake)
    {
        
    }

    public bool GetCanTake()
    {
      return _canInteract;
    }
}
