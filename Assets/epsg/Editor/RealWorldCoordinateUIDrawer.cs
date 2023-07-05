using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace epsg
{
    [CustomPropertyDrawer(typeof(RealWorldCoordinate))]
    public class RealWorldCoordinateUIDrawer : PropertyDrawer
    {
        Foldout mainContainer;
        VisualElement coordinatesContainer;
        VisualElement coordinatePopup;
        CoordinateSystem activeCoordinateSystem;
        VisualElement activeCoordinateSystemObject;
        int activeCoordinateSystemIndex=0;
        List<CoordinateSystem> coordinatesystems = new List<CoordinateSystem>();
        List<string> coordinatesystemNames = new List<string>();
        SerializedObject serializedObject;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            

            mainContainer = new Foldout();
            mainContainer.text = property.displayName;
            //var label = new Label(property.displayName);
            //mainContainer.Add(label);
            
            var expand = new Foldout();
            //getCoordinateSystems(property);
            //coordinatePopup = new DropdownField("coordinateSystem",coordinatesystemNames, activeCoordinateSystemIndex);
            coordinatePopup = new PropertyField(property.FindPropertyRelative("coordinateSystem"));
            
            //coordinatePopup.style.left = 10;
            //serializedObject = property.FindPropertyRelative("coordinateSystem").serializedObject;
            //activeCoordinateSystemObject = new VisualElement();
            //activeCoordinateSystemObject.TrackPropertyValue(property.FindPropertyRelative("coordinateSystem"), drawCoordinates);

            //coordinatePopup.Bind(serializedObject);

            //coordinatePopup.RegisterCallback<ChangeEvent<string>>(x=>csChanged(x.newValue,property));
            //coordinatePopup.style.left = 10;
            mainContainer.Add(coordinatePopup);

            coordinatesContainer = new VisualElement();
            coordinatesContainer.style.left = 10;
            mainContainer.Add(coordinatesContainer);
            drawCoordinates(property);
            return mainContainer;
        }

        public void csChanged(string newName, SerializedProperty property)
        {
            int newindex = 0;
            for (int i = 0; i < coordinatesystemNames.Count; i++)
            {
                if (coordinatesystemNames[i]==newName)
                {
                    newindex = i;
                }
            }
            property.FindPropertyRelative("coordinateSystem").objectReferenceValue = coordinatesystems[newindex];
            property.serializedObject.ApplyModifiedProperties();
            drawCoordinates(property);
        }

        public void drawCoordinates(SerializedProperty property)
        {
            coordinatesContainer.Clear();
            
           activeCoordinateSystem = (CoordinateSystem)property.FindPropertyRelative("coordinateSystem").objectReferenceValue;
            if (activeCoordinateSystem is null)
            {
                return;
            }

           
            var axis1 = activeCoordinateSystem.Axis1;
            var axis2 = activeCoordinateSystem.Axis2;
            var axis3 = activeCoordinateSystem.Axis3;

            int axiscount = activeCoordinateSystem.AxisCount();
            
            if (axiscount>0)
            {
                var value1 = new PropertyField(property.FindPropertyRelative("value1"),$"{axis1.abbreviation}({axis1.Unit})");
                
                coordinatesContainer.Add(value1);
                coordinatesContainer.Bind(property.FindPropertyRelative("value1").serializedObject);
            }
            if (axiscount > 1)
            {
                var value2 = new PropertyField(property.FindPropertyRelative("value2"), $"{axis2.abbreviation}({axis2.Unit})");

                
                coordinatesContainer.Add(value2);
                coordinatesContainer.Bind(property.FindPropertyRelative("value2").serializedObject);
            }
            if (axiscount > 2)
            {
                var value3 = new PropertyField(property.FindPropertyRelative("value3"), $"{axis3.abbreviation}({axis3.Unit})");

                coordinatesContainer.Add(value3);
                coordinatesContainer.Bind(property.FindPropertyRelative("value3").serializedObject);
            }
        }

        public void getCoordinateSystems(SerializedProperty property)
        {
            CoordinateSystemCollection[] csCollections = (CoordinateSystemCollection[])Resources.FindObjectsOfTypeAll(typeof(CoordinateSystemCollection));

            activeCoordinateSystem = (CoordinateSystem)property.FindPropertyRelative("coordinateSystem").objectReferenceValue;

            int index = 0;
            foreach (var cscollection in csCollections)
            {
                for (int i = 0; i < cscollection.coordinateSystems.Count; i++)
                {
                    if (cscollection.coordinateSystems[i]== activeCoordinateSystem)
                    {
                        activeCoordinateSystemIndex = index;
                    }
                    index++;
                    coordinatesystems.Add(cscollection.coordinateSystems[i]);
                    coordinatesystemNames.Add(cscollection.coordinateSystems[i].name + " (epsg:"+ cscollection.coordinateSystems[i].epsgCode + ")");
                }
            }
            

        }

            }
}
