namespace Mayfair.CoreContent.Code.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
//todo     using Mayfair.Core.Code.DebugMenu;
//    using Mayfair.Core.Code.DebugMenu.Content;
//    using Mayfair.Core.Code.DebugMenu.Fields;
//    using Mayfair.Core.Code.DebugMenu.Pages;
//    using UnityEditor;

//    public class DebugSettingsPage : DebugMenuPage
//    {
//        #region Fields
//        private List<SettingsContent> contents = new List<SettingsContent>();
//        #endregion

//        #region Constructors
//        public DebugSettingsPage(string title) : base(title) { }
//        #endregion

//        #region Class Methods
//        public void Build(DebugMenuNotebook notebook, object instance)
//        {
//            Type type = instance.GetType();
//            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

//            AddContent(notebook, instance, properties);
//        }

//        private void AddContent(DebugMenuNotebook notebook, object instance, PropertyInfo[] properties)
//        {
//            foreach (PropertyInfo propertyInfo in properties)
//            {
//                if (propertyInfo.PropertyType.IsClass)
//                {
//                    string name = propertyInfo.PropertyType.Name;
//#if UNITY_EDITOR
//                    name = ObjectNames.NicifyVariableName(name);
//#endif
//                    object otherInstance = propertyInfo.GetGetMethod().Invoke(instance, null);
//                    DebugSettingsPage newPage = new DebugSettingsPage(name);
//                    notebook.AddPagesWithParent(this, newPage);
//                    newPage.Build(notebook, otherInstance);
//                }
//                else if (propertyInfo.PropertyType == typeof(bool))
//                {
//                    AddBoolProperty(propertyInfo, instance);
//                }
//            }
//        }

//        private void AddBoolProperty(PropertyInfo propertyInfo, object instance)
//        {
//            object[] arguments = new object[1];
//            string name = propertyInfo.Name;
//#if UNITY_EDITOR
//            name = ObjectNames.NicifyVariableName(name);
//#endif
//            SettingsContent<ToggleField> content = new SettingsContent<ToggleField> {debugField = new ToggleField(name)};
//            content.drawAction =
//                (page, context) =>
//                {
//                    bool getValue = (bool) propertyInfo.GetGetMethod().Invoke(instance, null);
//                    if (getValue != content.debugField.Toggled)
//                    {
//                        getValue = content.debugField.Toggled;
//                        arguments[0] = content.debugField.Toggled;
//                        propertyInfo.GetSetMethod().Invoke(instance, arguments);
//                    }

//                    content.debugField.Draw(context, getValue);
//                };
//            contents.Add(content);
//        }

//        public override void Draw(DebugMenuContext context)
//        {
//            foreach (SettingsContent content in contents)
//            {
//                content.drawAction(this, context);
//            }
//        }
//        #endregion

//        #region Nested type: SettingsContent
//        private class SettingsContent
//        {
//            #region Fields
//            public Action<DebugMenuPage, DebugMenuContext> drawAction;
//            #endregion
//        }

//        private class SettingsContent<T> : SettingsContent
//            where T : DebugField
//        {
//            #region Fields
//            public T debugField;
//            #endregion
//        }
//        #endregion
//    }
}
