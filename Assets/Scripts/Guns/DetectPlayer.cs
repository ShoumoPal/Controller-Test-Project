using UnityEngine;
using UnityEngine.UI;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _pickupPrompt;
    [SerializeField] private float _pickupDistance;
    [SerializeField] private float offset;

    PlayerGunHandler player;

    GunScript gun;
    bool enablePrompt;
    bool preEnablePrompt;

    private void OnEnable()
    {
        gun = GetComponent<GunScript>();
        player = FindObjectOfType<PlayerGunHandler>();
    }

    private void Start()
    {
        _pickupPrompt = PlayerHUDScript.Instance.GetPickupPromptUI();
    }

    private void Update()
    {
        #region Enable Prompt Bool Check
        if (player != null && (Vector3.Distance(transform.position, player.transform.position) <= _pickupDistance) && (player.GetCurrentGun() != gun))
        {
            enablePrompt = true;
        }
        else
            enablePrompt = false;
        #endregion

        if (enablePrompt)
            _pickupPrompt.transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);

        if (enablePrompt && !preEnablePrompt)
        {
            _pickupPrompt.enabled = true;
            
            gun.isPickable = true;
            player.SetCurrentGun(gun);
        }
        else if (!enablePrompt && preEnablePrompt)
        {
            _pickupPrompt.enabled = false;
            gun.isPickable = false;
            player.ResetCurrentGun();
        }
        preEnablePrompt = enablePrompt;
    }


}
