using MightyAttributes;
using UnityEngine;

public class Cadran : MonoBehaviour
{
    [SerializeField, MinMax] private Vector2 _angleAiguille;
    
    [SerializeField] private RectTransform _aiguilleTransform;

    public void SetTurn(int turn)
    {
        var rotation = _aiguilleTransform.eulerAngles;
        rotation.z =  Mathf.Lerp(_angleAiguille.x, _angleAiguille.y, (float) turn / InstanceManager.TurnManager.numberOfTurns);
        _aiguilleTransform.eulerAngles = rotation;
    }
}
