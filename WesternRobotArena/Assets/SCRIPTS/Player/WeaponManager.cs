using Unity.Netcode;
using UnityEngine;

public class WeaponManager : NetworkBehaviour
{
    public enum WeaponType
    {
        Gun,
        Melee
    }

    private InputHandler input;

    [SerializeField]
    private GameObject gun;

    [SerializeField]
    private GameObject meleeWeapon;

    public NetworkVariable<WeaponType> currentWeapon = new NetworkVariable<WeaponType>(WeaponType.Gun);

    private void Awake()
    {
        input = GetComponent<InputHandler>();
    }

    public override void OnNetworkSpawn()
    {
        currentWeapon.OnValueChanged += OnWeaponChanged;

        UpdateWeaponObjects();
    }

    public override void OnNetworkDespawn()
    {
        currentWeapon.OnValueChanged -= OnWeaponChanged;
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        if (input.SwitchWeaponPressed)
        {
            SwitchWeaponRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void SwitchWeaponRpc()
    {
        if (currentWeapon.Value == WeaponType.Gun)
            currentWeapon.Value = WeaponType.Melee;
        else
            currentWeapon.Value = WeaponType.Gun;
    }

    private void OnWeaponChanged(WeaponType oldWeapon, WeaponType newWeapon)
    {
        UpdateWeaponObjects();
    }

    private void UpdateWeaponObjects()
    {
        gun.SetActive(currentWeapon.Value == WeaponType.Gun);

        meleeWeapon.SetActive(currentWeapon.Value == WeaponType.Melee);
    }

    public bool GunEquipped()
    {
        return currentWeapon.Value == WeaponType.Gun;
    }

    public bool MeleeEquipped()
    {
        return currentWeapon.Value == WeaponType.Melee;
    }
}