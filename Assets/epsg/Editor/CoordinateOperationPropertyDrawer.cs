using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace epsg.operationMethods
{
    

    // IngredientDrawerUIE
    [CustomPropertyDrawer(typeof(CoordinateOperation))]
    public class CoordinateOperationPropertyDrawer : PropertyDrawer
    {
        PropertyField methodField;
        VisualElement settingsContainer;
        VisualElement mainContainer;
        //nullOperationSettings
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {

            mainContainer = new VisualElement();
            methodField = new PropertyField(property.FindPropertyRelative("operationMethod"));
            mainContainer.Add(methodField);
            methodField.RegisterValueChangeCallback(x => updateSettingsContainer(property));


            settingsContainer = new VisualElement();
            createSettingsContainer(settingsContainer,property);


            mainContainer.Add(settingsContainer);
            return mainContainer;
        }
        public void updateSettingsContainer(SerializedProperty property) 
        {
            settingsContainer.Clear();

            createSettingsContainer(settingsContainer, property);
            mainContainer.Add(settingsContainer);
            mainContainer.MarkDirtyRepaint();

        }

        void createSettingsContainer(VisualElement container,SerializedProperty property)
        {

            OperationMethodEnum method = (OperationMethodEnum)property.FindPropertyRelative("operationMethod").enumValueIndex;

            switch (method)
            {
                case OperationMethodEnum.Helmert7:
                    var HelmertField = new PropertyField(property.FindPropertyRelative("HelmertSettings"));
                    container.Add(HelmertField);
                    container.Bind(property.FindPropertyRelative("HelmertSettings").serializedObject);
                    break;
                case OperationMethodEnum.null_operation:
                    var NullOperationField = new PropertyField(property.FindPropertyRelative("nullOperationSettings"));
                    container.Add(NullOperationField);
                    container.Bind(property.FindPropertyRelative("nullOperationSettings").serializedObject);
                    break;
                case OperationMethodEnum.SetAxisvalue:
                    var AddDimensionField = new PropertyField(property.FindPropertyRelative("addDimensionSettings"));
                    container.Add(AddDimensionField);
                    container.Bind(property.FindPropertyRelative("addDimensionSettings").serializedObject);
                    break;
                case OperationMethodEnum.GeographicToGeocentric:
                    var Addg2gField = new PropertyField(property.FindPropertyRelative("geog2GeocSettings"));
                    container.Add(Addg2gField);
                    container.Bind(property.FindPropertyRelative("geog2GeocSettings").serializedObject);
                    break;
                case OperationMethodEnum.SetAsideElevation:

                    break;
                case OperationMethodEnum.ElevationGridOffset:
                    var elevationGridfield = new PropertyField(property.FindPropertyRelative("elevationGridSettings"));
                    container.Add(elevationGridfield);
                    container.Bind(property.FindPropertyRelative("elevationGridSettings").serializedObject);
                    break;
                case OperationMethodEnum.GridShift:
                    var gridshiftField = new PropertyField(property.FindPropertyRelative("gridShiftSettings"));
                    container.Add(gridshiftField);
                    container.Bind(property.FindPropertyRelative("gridShiftSettings").serializedObject);
                    break;
                default:
                    break;
            }
            
            
        }

    }
}
