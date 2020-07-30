using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform backCircle;
    public RectTransform joyCircle;

    bool onTouch = false;

    float radious;

    Player player;

    private void Start()
    {
        radious = backCircle.rect.width / 2;
        backCircle.gameObject.SetActive(false);
    }
    public void Init(Player player)
    {
        this.player = player;
    }
    private void Update()
    {
        if (onTouch)
        {
            //player.SetDirection()
        }
    }

    private void OnTouch (Vector2 touchpos)
    {
        Vector2 pos = new Vector2(touchpos.x - backCircle.position.x, touchpos.y - backCircle.position.y);
        pos = Vector2.ClampMagnitude(pos, radious);
        joyCircle.localPosition = pos;

        float x = Mathf.Abs(pos.x);
        float y = Mathf.Abs(pos.y);

        if (pos.magnitude > radious * 0.3)
        {
            string dir = "00";

            if (x >= y)
            {
                if (pos.x < 0) dir = "01";
                else dir = "21";
            }
            else
            {
                if (pos.y < 0) dir = "10";
                else dir = "12";
            }

            player.SetDirection(dir);
        }
    }

    public void OnPointerDown(PointerEventData e)
    {
        backCircle.gameObject.SetActive(true);
        onTouch = true;
        backCircle.position = e.position;
        OnTouch(e.position);
    }

    public void OnDrag(PointerEventData e)
    {
        OnTouch(e.position);
    }

    public void OnPointerUp(PointerEventData e)
    {
        onTouch = false;
        joyCircle.localPosition = Vector2.zero;
        backCircle.gameObject.SetActive(false);
    }

}
