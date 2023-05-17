using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using epsg.coordinatesystems;
using System.Collections;
using System.Collections.Generic;
namespace epsg.operationMethods
{


    // IngredientDrawerUIE
    [CustomPropertyDrawer(typeof(HelmertSettings))]
    public class HelmertSettingsPropertyDrawer : PropertyDrawer
    {
        List<CoordinateSystem> geocentrics;
       
        int FromStartIndex;
        


        PopupField<string> fromfiltered;
        //nullOperationSettings
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
           
            // Create property container element.
            VisualElement container = new VisualElement();

            // Create property fields.
                        
            var tx = new PropertyField(property.FindPropertyRelative("tX"),"tX (m)");
            var ty = new PropertyField(property.FindPropertyRelative("tY"), "tY (m)");
            var tz = new PropertyField(property.FindPropertyRelative("tZ"), "tZ (m)");
            var rx = new PropertyField(property.FindPropertyRelative("rX"), "rX (arsec)");
            var ry = new PropertyField(property.FindPropertyRelative("rY"), "rY (arsec)");
            var rz = new PropertyField(property.FindPropertyRelative("rZ"), "rZ (arsec)");
            var ds = new PropertyField(property.FindPropertyRelative("dS"), "dS (ppm)");
            // Add fields to the container.
            
            container.Add(tx);
            container.Add(ty);
            container.Add(tz);
            container.Add(rx);
            container.Add(ry);
            container.Add(rz);
            container.Add(ds);

            
            return container;
        }
        
       

    }
}
