using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private JoyStickController _joyStickController;
    [SerializeField] private AvatarController _avatarController;

    private void Start()
    {
        _avatarController.Init(_joyStickController);
    }

    private void OnDestroy()
    {
        _avatarController.Dispose();
    }
}