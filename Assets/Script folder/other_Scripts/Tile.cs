using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject HighLight;

    [SerializeField] private Color lightColor = Color.white;
    [SerializeField] private Color darkColor = new Color(0.9f, 1f, 0.9f); // subtle greenish white

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.material.shader = Shader.Find("Sprites/Default");
    }

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? darkColor : lightColor;
    }

    private void OnMouseEnter()
    {
        HighLight.SetActive(true);
    }

    private void OnMouseExit()
    {
        HighLight.SetActive(false);
    }

    private void OnMouseDown()
    {
        GridManager.Instance.HandleTileClick(this);
    }
}
