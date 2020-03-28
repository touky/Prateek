namespace Mayfair.Core.Editor.Protocol
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Database.Interfaces;
    using Mayfair.Core.Code.Protocol;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;

    [CustomEditor(typeof(ProtocolDatabaseContainer))]
    public class ProtocolDatabaseContainerEditor : Editor
    {
        #region Fields
        private bool envelopeFoldout = false;
        private bool containerFoldout = false;
        private EditorDatabaseHelper editorDatabaseHelper = new EditorDatabaseHelper();
        private List<Type> resultTypes = new List<Type>();
        #endregion

        #region Class Methods
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ProtocolDatabaseContainer database = target as ProtocolDatabaseContainer;
            if (database == null)
            {
                return;
            }

            SerializedProperty source = serializedObject.FindProperty("source");
            SerializedProperty className = serializedObject.FindProperty("className");
            SerializedProperty datas = serializedObject.FindProperty("datas");

            EditorGUILayout.LabelField("Source: ");
            using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
            {
                EditorGUILayout.LabelField(source.stringValue);
            }

            EditorGUILayout.LabelField("Database type: ");
            using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
            {
                EditorGUILayout.LabelField(className.stringValue);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Data content: " + datas.arrayElementType + " - " + datas.arraySize);
            envelopeFoldout = EditorGUILayout.Foldout(envelopeFoldout, "Show Envelope content", true);
            if (envelopeFoldout)
            {
                using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
                {
                    ProtocolDatabaseEnvelope envelope = database.GetEnvelope();
                    EditorGUILayout.TextArea(ToReadable(envelope.ToString()));
                }
            }

            containerFoldout = EditorGUILayout.Foldout(containerFoldout, "Show databasecontent", true);
            if (containerFoldout)
            {
                using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
                {
                    IDatabaseTable databaseTable = null;
                    if (editorDatabaseHelper.IsValid)
                    {
                        databaseTable = editorDatabaseHelper.DoUnpack(database);
                    }
                    else
                    {
                        if (AssemblyHelper.GetAllTypeMatchingAnyAssembly(string.Empty, "Unpacker", resultTypes))
                        {
                            foreach (Type type in resultTypes)
                            {
                                if (type.IsAbstract)
                                {
                                    continue;
                                }

                                databaseTable = editorDatabaseHelper.TryUnpack(database, type);
                                if (databaseTable != null)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (databaseTable != null)
                    {
                        EditorGUILayout.TextArea(ToReadable(databaseTable.ToString()));
                    }
                }
            }
        }

        private string ToReadable(string text)
        {
            return text.Replace(", {", ",\n{").Replace("[ {", "[\n{").Replace("\", \"", "\",\t\"");//.Replace(" }", "\n}");
        }
        #endregion

        #region Nested type: EditorDatabaseHelper
        private class EditorDatabaseHelper : DatabaseHelper
        {
            #region Fields
            private Unpacker instance;
            #endregion

            #region Properties
            public bool IsValid
            {
                get { return instance != null; }
            }
            #endregion

            #region Class Methods
            public IDatabaseTable TryUnpack(ProtocolDatabaseContainer container, Type type)
            {
                instance = Activator.CreateInstance(type, true) as Unpacker;
                if (instance == null)
                {
                    return null;
                }

                return instance.TryUnpack(container);
            }

            public IDatabaseTable DoUnpack(ProtocolDatabaseContainer container)
            {
                return instance.TryUnpack(container);
            }
            #endregion
        }
        #endregion
    }
}
