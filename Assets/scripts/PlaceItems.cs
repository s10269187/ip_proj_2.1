using UnityEngine;

public class PlaceItems : MonoBehaviour
{
    public GameObject placeholder; // Assign in Inspector
    public GameObject actualItem;  // Assign in Inspector

    private bool itemPlaced = false;

    void Start()
    {
        // Show placeholder, hide actual item at start
        placeholder.SetActive(true);
        actualItem.SetActive(false);

        // Make placeholder translucent and non-interactable
        SetTranslucent(placeholder, 0.3f);
        SetNonInteractable(placeholder);
    }

    void Update()
    {
        // Press R to place the item
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlaceItem();
        }
    }

    // Call this method when the player places the item
    public void PlaceItem()
    {
        if (!itemPlaced)
        {
            placeholder.SetActive(false);
            actualItem.SetActive(true);
            itemPlaced = true;

            // Make actual item fully opaque and interactable
            SetOpaque(actualItem);
            SetInteractable(actualItem);
        }
    }

    // Utility to set transparency
    void SetTranslucent(GameObject obj, float alpha)
    {
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Clone material to avoid affecting other objects
            renderer.material = new Material(renderer.material);

            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;

            renderer.material.SetFloat("_Mode", 2); // Fade mode
            renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            renderer.material.SetInt("_ZWrite", 0);
            renderer.material.DisableKeyword("_ALPHATEST_ON");
            renderer.material.EnableKeyword("_ALPHABLEND_ON");
            renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.renderQueue = 3000;
        }
    }

    // Utility to make non-interactable
    void SetNonInteractable(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    // Utility to set full opacity
    void SetOpaque(GameObject obj)
    {
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Clone material to avoid affecting other objects
            renderer.material = new Material(renderer.material);

            Color color = renderer.material.color;
            color.a = 1f;
            renderer.material.color = color;

            renderer.material.SetFloat("_Mode", 0); // Opaque mode
            renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            renderer.material.SetInt("_ZWrite", 1);
            renderer.material.DisableKeyword("_ALPHATEST_ON");
            renderer.material.DisableKeyword("_ALPHABLEND_ON");
            renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.renderQueue = -1;
        }
    }

    // Utility to make interactable
    void SetInteractable(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        if (col != null)
            col.isTrigger = false;
    }
}
