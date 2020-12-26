using UnityEngine;
using UnityEditor;
using ICSharpCode.NRefactory.Ast;

[CustomEditor(typeof(WeaponCharger))]
public class WeaponChargerEditor : Editor
{
    Weapon weapon = Weapon.PISTOL;
    Vector3 pistolScale = new Vector3(60, 60, 60);
    string pistolName = "Pistol (Charger)";
    string SubmachineGunName = "Submachine Gun (Charger)";
    string rifleName = "Rifle (Charger)";
    

    [MenuItem("Game Elements/Weapon/Pistol")]
    private static void CreatePistol()
    {
        Debug.Log("Object instantiated");
        Transform pos = SceneView.lastActiveSceneView.camera.transform;
        GameObject weapon = Instantiate(Resources.Load<GameObject>("Prefabs/WeaponCharger"), pos.position + pos.forward * 17.5f, new Quaternion(0, 0, 0, 0));
        weapon.name = "Pistol (Charger)";
        weapon.transform.rotation = Quaternion.Euler(0, 0, 90);
        GameObject weaponSkin = Instantiate(Resources.Load<GameObject>("Prefabs/Pistol 2"), pos.position + pos.forward * 17.5f, new Quaternion(0, 0, 0, 0));
        weaponSkin.transform.parent = weapon.transform;
        weaponSkin.transform.localScale = new Vector3(60, 60, 60);
        weaponSkin.transform.localRotation = Quaternion.Euler(-90, -90, 0);
        weaponSkin.transform.localPosition = Vector3.zero;
        weapon.GetComponent<WeaponCharger>().skin = weaponSkin;
        weapon.transform.localPosition = new Vector3(0.1f, 0.1f, 0.1f);
        weapon.transform.position = pos.position + pos.forward * 5f;
    }

    [MenuItem("Game Elements/Weapon/Submachine Gun")]
    private static void CreateSubmachineGun()
    {
        Debug.Log("Object instantiated");
        Transform pos = SceneView.lastActiveSceneView.camera.transform;
        GameObject weapon = Instantiate(Resources.Load<GameObject>("Prefabs/WeaponCharger"), pos.position + pos.forward * 17.5f, new Quaternion(0, 0, 0, 0));
        weapon.name = "Submachine Gun (Charger)";
        weapon.transform.rotation = Quaternion.Euler(0, 0, 90);
        GameObject weaponSkin = Instantiate(Resources.Load<GameObject>("Prefabs/SubmachineGun With Outlines"), pos.position + pos.forward * 17.5f, new Quaternion(0, 0, 0, 0));
        weaponSkin.transform.parent = weapon.transform;
        weaponSkin.transform.localScale = new Vector3(1, 1, 1);
        weaponSkin.transform.localRotation = Quaternion.Euler(0, 0, 0);
        weaponSkin.transform.localPosition = Vector3.zero;
        weapon.GetComponent<WeaponCharger>().skin = weaponSkin;
        weapon.transform.localPosition = new Vector3(0.1f, 0.1f, 0.1f);
        weapon.transform.position = pos.position + pos.forward * 5;
    }

    [MenuItem("Game Elements/Weapon/Rifle")]
    private static void CreateRifle()
    {
        Debug.Log("Object instantiated");
        Transform pos = SceneView.lastActiveSceneView.camera.transform;
        GameObject weapon = Instantiate(Resources.Load<GameObject>("Prefabs/WeaponCharger"), pos.position + pos.forward * 17.5f, new Quaternion(0, 0, 0, 0));
        weapon.name = "Rifle (Charger)";
        weapon.transform.rotation = Quaternion.Euler(0, 0, 90);
        GameObject weaponSkin = Instantiate(Resources.Load<GameObject>("Prefabs/MT17"), pos.position + pos.forward * 17.5f, new Quaternion(0, 0, 0, 0));
        weaponSkin.transform.parent = weapon.transform;
        weaponSkin.transform.localScale = new Vector3(1, 1, 1);
        weaponSkin.transform.localRotation = Quaternion.Euler(0, 0, 0);
        weaponSkin.transform.localPosition = Vector3.zero;
        weapon.GetComponent<WeaponCharger>().skin = weaponSkin;
        weapon.transform.localPosition = new Vector3(0.1f, 0.1f, 0.1f);
        weapon.transform.position = pos.position + pos.forward * 5;
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();

        var weaponType = target as WeaponCharger;

        weapon = (Weapon)EditorGUILayout.EnumPopup("Weapon Type", weaponType.weapon);

        //weaponType.weapon = this.weapon;

        Transform pos = SceneView.lastActiveSceneView.camera.transform;
        GameObject weaponSkin;

        switch (weapon)
        {
            case Weapon.PISTOL:
                if(weaponType.weapon != Weapon.PISTOL)
                {
                    weaponType.weapon = Weapon.PISTOL;
                    DestroyImmediate(weaponType.skin);
                    weaponType.gameObject.name = pistolName;
                    weaponSkin = Instantiate(Resources.Load<GameObject>("Prefabs/Pistol 2"), pos.position + pos.forward * 17.5f, new Quaternion(0, 0, 0, 0));
                    weaponSkin.transform.parent = weaponType.gameObject.transform;
                    weaponSkin.transform.localScale = new Vector3(1, 1, 1);
                    weaponSkin.transform.localScale = pistolScale;
                    weaponSkin.transform.localRotation = Quaternion.Euler(-90, -90, 0);
                    weaponSkin.transform.localPosition = Vector3.zero;
                    weaponType.skin = weaponSkin;
                }
                
                break;

            case Weapon.SUBMACHINE_GUN:
                if (weaponType.weapon != Weapon.SUBMACHINE_GUN)
                {
                    weaponType.weapon = Weapon.SUBMACHINE_GUN;
                    DestroyImmediate(weaponType.skin);
                    weaponType.gameObject.name = SubmachineGunName;
                    weaponSkin = Instantiate(Resources.Load<GameObject>("Prefabs/SubmachineGun With Outlines"), pos.position + pos.forward * 17.5f, new Quaternion(0, 0, 0, 0));
                    weaponSkin.transform.parent = weaponType.gameObject.transform;
                    weaponSkin.transform.localScale = new Vector3(1, 1, 1);
                    weaponSkin.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    weaponSkin.transform.localPosition = Vector3.zero;
                    weaponType.skin = weaponSkin;
                }
                break;

            case Weapon.RIFLE:
                if (weaponType.weapon != Weapon.RIFLE)
                {
                    weaponType.weapon = Weapon.RIFLE;
                    DestroyImmediate(weaponType.skin);
                    weaponType.gameObject.name = rifleName;
                    weaponSkin = Instantiate(Resources.Load<GameObject>("Prefabs/MT17"), pos.position + pos.forward * 17.5f, new Quaternion(0, 0, 0, 0));
                    weaponSkin.transform.parent = weaponType.gameObject.transform;
                    weaponSkin.transform.localScale = new Vector3(1, 1, 1);
                    weaponSkin.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    weaponSkin.transform.localPosition = Vector3.zero;
                    weaponType.skin = weaponSkin;
                }
                break;
        }

        weaponType.whatIsPlayer = EditorGUILayout.LayerField("What is Player" , weaponType.whatIsPlayer);
    }
}
