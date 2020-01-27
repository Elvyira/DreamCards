using MightyAttributes;
using TMPro;
using UnityEngine;

public class TurnStateLabel : MonoBehaviour
{
    [SerializeField, Hide, GetComponent] private TextMeshProUGUI _text;

    public void SetState(TurnState state) => _text.text = state.ToString();
}
