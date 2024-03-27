using UnityEngine;
using TMPro;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public class Slider : MonoBehaviour
{
    [SerializeField] private int _minValue;
    [SerializeField] private int _maxVlaue;
    [SerializeField] private UnityEngine.UI.Slider _slider;
    [SerializeField] private TextMeshProUGUI _value;

    public int Value { get; private set; }

    private void Awake()
    {
        _slider.minValue = _minValue;
        _slider.maxValue = _maxVlaue;
        Value = _minValue;
        _value.text = _minValue.ToString();
        _slider.onValueChanged.AddListener(ValueChanged);
    }

    private void ValueChanged(float arg0)
    {
        Value = Mathf.RoundToInt(arg0);
        _value.text = Value.ToString();
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(ValueChanged);
    }
}
