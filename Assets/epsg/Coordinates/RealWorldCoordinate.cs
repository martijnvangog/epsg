using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using epsg;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[System.Serializable]
public struct RealWorldCoordinate
{
    [SerializeField]
    double value1, value2,value3;
    
    [SerializeField]
    CoordinateObject coordinateSystem;
    bool is3D;

    public RealWorldCoordinate(CoordinateObject coordinateSystem,double value1, double value2, double value3=double.NaN)
    {
        this.value1 = value1;
        this.value2 = value2;
        this.value3 = value3;
        is3D = (value3 != double.NaN);
        this.coordinateSystem = coordinateSystem;
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(RealWorldCoordinate))]
    public class RealWorldCoordianteDrawer : PropertyDrawer
    {
        
        DoubleField Value1;
        DoubleField Value2;
        DoubleField Value3;
        VisualElement container;
        VisualElement coordinateContainer;
        public void createCoordinateContainer(SerializedProperty property)
        {

            Debug.Log("callback aangeroepen");

            CoordinateObject css = (CoordinateObject)property.FindPropertyRelative("coordinateSystem").objectReferenceValue;
            if (css == null)
            {
                Value1.visible = false;
                Value2.visible = false;
                Value3.visible = false;
                return;
            }
            string axisname;
            if (css.Axes.Count > 0)
            {
                axisname = css.Axes[0].abbreviation.ToString() + " ("+css.Axes[0].uom.ToString()+")";
                Value1.label = axisname;
                Value1.visible = true;
            }
            else
            {
                Value1.visible = false;
            }
            if (css.Axes.Count > 1)
            {
                axisname = css.Axes[1].abbreviation.ToString() + " (" + css.Axes[1].uom.ToString() + ")";
                Value2.label = axisname;
                Value2.visible = true;
            }
            else
            {
                Value2.visible = false;
            }
            if (css.Axes.Count > 2)
            {
                axisname = css.Axes[2].abbreviation.ToString() + " (" + css.Axes[2].uom.ToString() + ")";
                Value3.label = axisname;
                Value3.visible = true;
            }
            else
            {
                Value3.visible = false;
            }

            

        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            container = new VisualElement();
            //header
            var csharpLabel = new Label(property.displayName);
            container.Add(csharpLabel);
            //coordianteSystem
            PropertyField coordinateSystemField = new PropertyField(property.FindPropertyRelative("coordinateSystem"));
            coordinateSystemField.RegisterValueChangeCallback(x=> createCoordinateContainer(property));
            container.Add(coordinateSystemField);

            Value1 = new DoubleField("value1");
            Value1.BindProperty(property.FindPropertyRelative("value1"));
            Value1.style.left = 25;
            container.Add(Value1);
            Value2 = new DoubleField("value2");
            Value2.BindProperty(property.FindPropertyRelative("value2"));
            Value2.style.left = 25;
            container.Add(Value2);
            Value3 = new DoubleField("value3");
            Value3.BindProperty(property.FindPropertyRelative("value3"));
            Value3.style.left = 25;
            container.Add(Value3);

            createCoordinateContainer(property);




            return container;
        }
    }

#endif


}

