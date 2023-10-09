using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public uint fingers
    {
        get
        {
            return _fingers;
        }
        set
        {
            _fingers = value % 6;
            switch (_fingers)
            {
                case 0:
                    _spriteRenderer.sprite = _weapon_zero;
                    break;
                case 1:
                    _spriteRenderer.sprite = _weapon_one;
                    break;
                case 2:
                    _spriteRenderer.sprite = _weapon_two;
                    break;
                case 3:
                    _spriteRenderer.sprite = _weapon_three;
                    break;
                case 4:
                    _spriteRenderer.sprite = _weapon_four;
                    break;
                case 5:
                    _spriteRenderer.sprite = _weapon_five;
                    break;
                default:
                    break;
            }
        }
    }

    [Header("Image")]
    [SerializeField] Sprite _weapon_zero;
    [SerializeField] Sprite _weapon_one;
    [SerializeField] Sprite _weapon_two;
    [SerializeField] Sprite _weapon_three;
    [SerializeField] Sprite _weapon_four;
    [SerializeField] Sprite _weapon_five;

    private SpriteRenderer _spriteRenderer;
    private uint _fingers;

    // Start is called before the first frame update
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _fingers = 0;
    }
}
