using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private RectTransform left;
    [SerializeField] private RectTransform right;
    [SerializeField] private RectTransform up;
    [SerializeField] private RectTransform down;
    [SerializeField] private float smooth = 0.05f;
    private PlayerMovement player;
    private Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<PlayerMovement>())
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        if (FindObjectOfType<Gun>())
        {
            gun = FindObjectOfType<Gun>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player && gun)
        {
            float quantity = player.movementQuantity * gun.accurate;
            Vector3 leftPos = new Vector3(-quantity, left.localPosition.y, left.localPosition.z);
            left.localPosition = Vector3.Lerp(left.localPosition, leftPos, smooth * Time.deltaTime);

            Vector3 rightPos = new Vector3(quantity, right.localPosition.y, right.localPosition.z);
            right.localPosition = Vector3.Lerp(right.localPosition, rightPos, smooth * Time.deltaTime);

            Vector3 downPos = new Vector3(down.localPosition.x, -quantity, down.localPosition.z);
            down.localPosition = Vector3.Lerp(down.localPosition, downPos, smooth * Time.deltaTime);

            Vector3 upPos = new Vector3(up.localPosition.x, quantity, up.localPosition.z);
            up.localPosition = Vector3.Lerp(up.localPosition, upPos, smooth * Time.deltaTime);
        }
        
    }
}
