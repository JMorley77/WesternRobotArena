
using UnityEngine;

public class LocalWeaponManager : MonoBehaviour
{
    public enum WeaponType
    {
        Gun,
        Melee
    }

    private LocalInputHandler input;

    [SerializeField]
    private GameObject gun;

    [SerializeField]
    private GameObject meleeWeapon;

    private WeaponType currentWeapon = WeaponType.Gun;

    private void Awake()
    {
        input = GetComponent<LocalInputHandler>();
    }

    private void Start()
    {
        UpdateWeaponObjects();
    }

    private void Update()
    {
        if (input.SwitchWeaponPressed)
        {
            SwitchWeapon();
        }
    }

    private void SwitchWeapon()
    {
        if (currentWeapon == WeaponType.Gun)
            currentWeapon = WeaponType.Melee;
        else
            currentWeapon = WeaponType.Gun;

        UpdateWeaponObjects();
    }

    private void UpdateWeaponObjects()
    {
        gun.SetActive(currentWeapon == WeaponType.Gun);

        meleeWeapon.SetActive(currentWeapon == WeaponType.Melee);
    }

    public bool GunEquipped()
    {
        return currentWeapon == WeaponType.Gun;
    }

    public bool MeleeEquipped()
    {
        return currentWeapon == WeaponType.Melee;
    }
}
